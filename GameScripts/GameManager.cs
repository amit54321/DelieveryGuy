using Authentication;
using LitJson;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using static LobbyData;

[System.Serializable]
public class PlayerTeamData
{

    public List<PlayerPosition> playerPos;

}
[System.Serializable]
public class PlayerPosition
{
    public double x;
    public double y;
    public double z;
    public int rotZ, rotY,rotX;
}
[System.Serializable]
public class WayPoints
{
    public List<GameObject> ways;
}
public class GameManager : MonoBehaviour
{
    // public List<Restaurants> restaurants;

    public Restaurants currentRestaurant;

    public ArrowHandler arrowHandler;

    public Camera playerCamera, cityCamera;

    public int clickedPlotId;

    public static GameManager Instance;

    public List<Plot> allPlots;
    public List<Restaurants> allRestaurants;
    public List<HouseCollider> allHouses;

    public TaskData currentTask;
    public List<TaskData> taskDatas;
    public Timer timer;

    public Transform player, enemy;

    public Direction dir;

    public List<WayPoints> wayPoints;

    public Vector3 startingPoint;

    public bool gameOver;
    public int totalTasks;

    public GameObject mapCube;
    public GameObject minimap;

    public List<GameObject> allScenes;

    public Tutorial tutorial;

    Vector3 enemyRot, enemyPos;

    public int swapFirst = -1, swapSecond = -1;
    public GameObject portalButton,cancelTimer;

    public GameObject followPlayer, followEnemy;
    public void CancelTimer()
    {
        InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.canceltimerPopUp);
    }

    public void ToggleCancelTimer(bool toogle)
    {
       
        if(!InGame.UIManager.Instance.tutorialUI.activeSelf &&  RoomContoller.SocketMaster.instance.profileData.restaurants.Count == 10)
        cancelTimer.SetActive(toogle);
       
    }
  
    void EnableOrDisablePortalButton()
    {
       // if (PlayerPrefs.GetInt("PORTAL") == 1)
          //  portalButton.SetActive(false);
       // else
        {
          //  portalButton.SetActive(true);
        }
    }
    void SetCars()
    {
        foreach (GameObject g in allScenes)
        {

            foreach (Transform t in g.transform)
            {

                if (t.GetComponent<Rigidbody>())
                {

                    if (t.GetComponent<Rigidbody>().mass >= 900)
                    {
                        if (t.GetComponent<AICar>() == null)
                        {

                            t.gameObject.AddComponent<AICar>();
                            t.gameObject.GetComponent<AICar>().stable = true;
                        }
                    }
                }
            }
        }
    }

    public void ResetPlayer()
    {
        player.transform.position = startingPoint;
        transform.eulerAngles = Vector3.zero;
        player.transform.GetComponent<Rigidbody>().isKinematic = false;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        gameOver = false;
        SetRestaurantStaticData();
        Debug.LogError(GetCurrentTime());
        SetCars();
        swapFirst = -1;
        swapSecond = -1;
    }

    public void SwapCancel(GameObject g)
    {
        swapFirst = -1;
        swapSecond = -1;
        g.SetActive(false);
    }
    public void SwapBuildings()
    {
        if (swapFirst == -1 || swapSecond==-1 )
        {
            return;
        }
        UnityEngine.Debug.LogError("SAPPING " + swapFirst + "  SAPPING   " + swapSecond);
        Plot p1 = FindPlotById(swapFirst);
        Plot p2 = FindPlotById(swapSecond);

        Restaurants rId1 = null;
        Restaurants rId2 =null ;
        Vector3 pos = Vector3.zero ; 
         foreach(Transform t in p1.transform)
        {
            if(t.GetComponent<Restaurants>()!=null)
            {
                rId1 = t.GetComponent<Restaurants>();
                pos = rId1.transform.localPosition;
            }
        }

        foreach (Transform t in p2.transform)
        {
            if (t.GetComponent<Restaurants>() != null)
            {
                rId2 = t.GetComponent<Restaurants>();
            }
        }

        rId2.transform.SetParent(p1.transform);
        rId1.transform.SetParent(p2.transform);
        rId1.transform.localPosition = pos;
        rId2.transform.localPosition = pos;
       
        swapFirst = -1;
        swapSecond = -1;
        InGame.UIManager.Instance.swapUI.SetActive(false);

    }
    public void SetPlayerPosition(LobbyData.SyncPosition data)
    {
       

        Debug.LogError(data.id + "  GETTING INFO "+ PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID));
        if (!PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID).Equals(data.id))
        {
            PlayerPosition player = data.message;

            enemyPos= new Vector3((float)player.x, (float)player.y, (float)player.z); ;
            enemyRot = new Vector3((int)player.rotX, (int)player.rotY, (int)player.rotZ);

            }



    }
    public void PLayerPositionToTarget()
    {
        Transform target = null ;
        foreach(Restaurants res in allRestaurants)
        {
            if (res.restaurantCollider.transform.GetChild(0).gameObject.activeSelf)
            {
                target = res.restaurantCollider.transform;
                break;
            }
        }
        if(target!=null)
        {
            float y = player.transform.position.y + 1;
            player.transform.position = new Vector3(target.transform.position.x, y, target.transform.position.z);
            return;
        }

        foreach (HouseCollider res in allHouses)
        {
            if (res.gameObject.activeSelf)
            {
                target = res.transform;
                break;
            }
        }
        if (target != null)
        {
            float y = player.transform.position.y + 1;
            player.transform.position = new Vector3(target.transform.position.x, y, target.transform.position.z);
            return;
        }
        PlayerPrefs.SetInt("PORTAL", 1);
        portalButton.SetActive(false);
    }
    int GetCurrentTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return cur_time;
    }

    int opponentTasksDone = -1;
    public void SetOpponentTasks(int tasksDone)
    {
        opponentTasksDone++;
        InGame.UIManager.Instance.screenUI.OpponentSetTasks(tasksDone);
    }
    public void SetTasks(int tasksDone)
    {
     
        InGame.UIManager.Instance.screenUI.SetTasks(tasksDone);
    }
    public void SetInitialOpponentTasks()
    {
        foreach (LobbyData.TaskDoneData u in SocketMaster.instance.gamePlay.tasksDone)
        {
            if (!PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID).Equals(u.id))
            {
              
                opponentTasksDone = u.taskDone.Count;
                Debug.LogError("OPPONENT TASK " + opponentTasksDone + "    " + u.taskDone.Count);
                
                break;
            }

        }
        if(opponentTasksDone>=10)
        {
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.winnerPopUp);
            InGame.UIManager.Instance.HideInputs();
        }
             //   opponentTasksDone = taskDone;
        InGame.UIManager.Instance.screenUI.OpponentSetTasks(opponentTasksDone);
    }
        public bool DisableCurrentTask()
    {

        foreach (TaskData t in taskDatas)
        {
            if (currentTask.id == t.id)
            {
                SendTaskDone(t.id,PlayerPrefs.GetString(PlayerPrefsData.ID));
                taskDatas.Remove(currentTask);
                break;
            }
        }
       // InGame.UIManager.Instance.screenUI.SetTasks(totalTasks - taskDatas.Count);
        if (totalTasks - taskDatas.Count <= 0)
        {
            gameOver = true;
            return true;
        }
        return false;
    }
    public void SetArrow(Transform target)
    {
        //currentRestaurant = target;
        arrowHandler.SetArrow(target.transform);
    }

    void ShowInputs()
    {
        InGame.UIManager.Instance.ShowInputs();
    }

    IEnumerator Testing()
    {
        for (int i = 1; i <= 10; i++)
        {
          //  SendTaskDone(i);
            yield return new WaitForSeconds(1);

        }
    }


           void InitializeTasksData()
        {
            taskDatas.Clear();
            foreach(TaskData t in SocketMaster.instance.gamePlay.tasks)
            {
            
                taskDatas.Add(t);
            }
        foreach (TaskDoneData t1 in SocketMaster.instance.gamePlay.tasksDone)
        {
            if (t1.id == PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID))
            {
                foreach (int tasksDone in t1.taskDone)
                {
                    foreach (TaskData taskData in taskDatas)
                    {
                        if (tasksDone == taskData.id)
                        {
                            taskDatas.Remove(taskData);
                            break;
                        }
                            }
                }
                break;
            }

        }


        totalTasks = taskDatas.Count;
        }
    private void OnEnable()
    {
        Debug.LogError("STATUS  " + Authentication.Authentication.status +"    "
            + SocketMaster.instance.profileData.tutorial);
        if (Authentication.Authentication.status ==STATUS.PLAY)
        {
            EnableOrDisablePortalButton();
            InitializeTasksData();
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.tasksPopUp);
            InGame.UIManager.Instance.screenUI.gameObject.SetActive(true);
            EnablePlayerCamera();
           
            COnstructInitialBuildings();
            InGame.UIManager.Instance.HideInputs();
           
            StartCoroutine(PlayAI());
            SetInitialOpponentTasks();

            followEnemy.SetActive(true);
            followPlayer.SetActive(true);

            // SocketMaster.instance.StartCoroutine(SocketMaster.instance.SendMissions(new List<int>() { 0,1,2 }));

            //  StartCoroutine(Testing());


            //  Invoke("ShowInputs",)
        }
        else if (Authentication.Authentication.status == STATUS.SET)
        {
         
            EnableCityCamera();
            COnstructInitialBuildings();
            InGame.UIManager.Instance.restaurantPopUp.gameObject.SetActive(true);
            InGame.UIManager.Instance.restaurantPopUp.SetData();
            followEnemy.SetActive(false);
            followPlayer.SetActive(false);

            if (SocketMaster.instance.profileData.tutorial==0)
            {
                InGame.UIManager.Instance.tutorialUI.gameObject.SetActive(true);
            }
            else
            {
                InGame.UIManager.Instance.tutorialUI.gameObject.SetActive(false);

            }
        }
    }

    public void SetRestaurantPopUp()
    {
        if(InGame.UIManager.Instance==null)
        {
            return;
        }
      
        InGame.UIManager.Instance.restaurantPopUp.gameObject.SetActive(true);
        InGame.UIManager.Instance.restaurantPopUp.SetData();
    }
    
    public void EnableCityCamera()
    {
        cityCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        InGame.UIManager.Instance.HideInputs();
        arrowHandler.arrows.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);
        cityCamera.fieldOfView = 36;
        minimap.SetActive(false);
    }

    public void EnablePlayerCamera()
    {
        cityCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        InGame.UIManager.Instance.ShowInputs();
    }
    int frame = 0;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnablePlayerCamera();
            InGame.UIManager.Instance.ShowInputs();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            EnableCityCamera();
            InGame.UIManager.Instance.HideInputs();
        }
        //  mainArrow.transform.LookAt(mapCube.transform.position,Vector3.up);

        if (Authentication.Authentication.status == STATUS.SET)
        {
            return;
        }


        if (SocketMaster.instance.gamePlay.ai == 1)
        {
           return;
        }

      
            if (enemy != null)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemyPos, Time.deltaTime * 15);
            enemy.transform.eulerAngles = enemyRot;
        }

        frame++;
        if (frame % 6 == 0)
        {

           
         // PlayerPosition allPlayersPosition = new PlayerPosition();
            LobbyData.SyncPosition data;
            
                
                    PlayerPosition playerPosition = new PlayerPosition();
                    playerPosition.x = player.position.x;
                    playerPosition.y = player.position.y;
            playerPosition.z = player.position.z;
            playerPosition.rotZ = (int)player.rotation.eulerAngles.z;
                    playerPosition.rotY = (int)player.rotation.eulerAngles.y;
            playerPosition.rotX = (int)player.rotation.eulerAngles.x;
          //  Debug.LogError( "  GETTING INFO " + playerPosition.x);





            // Debug.LogError(" DATA  " + playerTeamData.playerPos.Count);
            SocketMaster.instance.socketMaster.Socket.Emit(
               LobbyConstants.CHATSEND,
               (socket, packet, args) =>
               {
                   if (args != null && args.Length > 0)
                   {

                   }
               },
               data = new LobbyData.SyncPosition()
               {
                   id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                   gameId = SocketMaster.instance.gamePlay.game_id,
                   message = playerPosition
               });
        }



    }

    public  Plot FindPlotById(int id)
    {
        foreach(Plot p in allPlots)
        {
            if(p.id==id)
            {
                return p;
            }
        }
        return null;
    }

   public HouseCollider FIndHouseById(int id)
    {
        foreach (HouseCollider p in allHouses)
        {
            if (p.id == id)
            {
                return p;
            }
        }
        return null;
    }
    public Restaurants FindRestaurantById(int id)
    {
        foreach (Restaurants p in allRestaurants)
        {
            if (p.id == id)
            {
                return p;
            }
        }
        return null;
    }
    public Restaurants FindRestaurantByRestaurantId(int id)
    {
        foreach (Restaurants p in allRestaurants)
        {
            if (p.restaurantData.id == id)
            {
                return p;
            }
        }
        return null;
    }
    void COnstructInitialBuildings()
    {
        TimersData t = null;
        if (SocketMaster.instance.profileData.timers.Count > 0)
        {
             t = SocketMaster.instance.profileData.timers[0];
        }
        foreach (RestaurantsData r in SocketMaster.instance.profileData.restaurants)
        {
            if (t != null)
            {
                if (t.plot_id == r.plot_id && t.restaurant_id == r.restaurant_id)
                {

                }
                else
                    ConstructBuilding(r.plot_id, r.restaurant_id, 0, 10, 0, r.level);
            }
            else
            {
                ConstructBuilding(r.plot_id, r.restaurant_id, 0, 10, 0, r.level);
            }
        }

        foreach (TimersData r in SocketMaster.instance.profileData.timers)
        {
          int leftTime = Mathf.Abs((int)(r.end/1000 - GetCurrentTime()));
            Debug.LogError(GetCurrentTime() +"   NOW  " +  r.end/1000 +  "    "+leftTime);
            GameManager.Instance.clickedPlotId = r.plot_id;
           
            ConstructBuilding(r.plot_id, r.restaurant_id, (int)leftTime, 10, 0, r.level,true);
        }
        

        //for (int i = 1; i <= 10; i++)
        //{
        //    ConstructBuilding(i, i, 0, 10, 10, 1);
        //    RestaurantsData r = new RestaurantsData();
        //    r.plot_id = i;
        //    r.restaurant_id = i;
        //    r.level = 2;

        //    SocketMaster.instance.profileData.restaurants.Add(r);
        //}
    }
   

    public void EnableCurrentTaskHouse()
    {
        HouseCollider h = FIndHouseById(currentTask.customerId);
        h.gameObject.SetActive(true);
        h.EnableHouse();
        EnableMapCube(h.transform.parent);
    
    }
    void SetPlayerKinematic()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void StopPlayer(Transform t)
    {
        float y = player.transform.position.y+1;
        player.transform.position = new Vector3(t.transform.position.x, y, t.transform.position.z);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Invoke("SetPlayerKinematic", 0.2f);
    }

   public  void DisableMapCube()
    {
        mapCube.SetActive(false);
    }
    void EnableMapCube(Transform parent)
    {
        mapCube.SetActive(true);
        mapCube.transform.SetParent(parent);
        mapCube.transform.localPosition = new Vector3(0, 90, 0);
        SetArrow(mapCube.transform);
    }
    public void TaskSelected(TaskData task)
    {
        currentTask = task;
        InGame.UIManager.Instance.ShowInputs();
        Restaurants r = FindRestaurantByRestaurantId(task.restaurantId);
        r.restaurantCollider.gameObject.SetActive(true);
        r.restaurantCollider.EnableCollider();
        EnableMapCube(r.transform);
      
     //  dir.SetArrow(player, r.transform);
    }
   public  RestaurantDataUIArray data;
    void SetRestaurantStaticData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("restaurantData");
         data = JsonUtility.FromJson<RestaurantDataUIArray>(textAsset.text);
    }



    public void ConstructBuilding(int plot_id, int building_id, int cTime, int quantity, int waitTime, int level,bool already = false)
    {
        Plot p = FindPlotById(plot_id);
        GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/Restaurant/" + building_id), p.transform);
        g.transform.localEulerAngles = p.eulerAngle;
     

        {
            g.GetComponent<Restaurants>().StartCoroutine(g.GetComponent<Restaurants>().StartConstruction(plot_id, cTime, level, quantity, waitTime, building_id));
        }
        if (cTime <= 0)
        {
            g.GetComponent<Restaurants>().ConstructionFinished(false,true);
            return;
        }
        if(already)
        GameManager.Instance.allRestaurants.Add(g.GetComponent<Restaurants>());
        p.enabled = false;
        p.GetComponent<MeshRenderer>().enabled = false;
         //  allRestaurants.Add(g.GetComponent<Restaurants>());
        p.GetComponent<Collider>().enabled = false;
    }

    public void UpgradeBuilding(int plot_id, int building_id, int cTime, int quantity, int waitTime, int level)
    {
        Restaurants p = FindRestaurantById(plot_id);
        p.StartCoroutine(p.StartConstruction(plot_id, cTime, level, quantity, waitTime, building_id));
        p.enabled = false;
        p.GetComponent<Collider>().enabled = false;
    }
    public void UpgradeFInisged(RestaurantsData r, bool cons)

    {

        Plot p1 = FindPlotById(r.plot_id);
        foreach (Transform t in p1.transform)
        {
            if (t.GetComponent<Restaurants>() != null)
            {

                t.GetComponent<Restaurants>().ConstructionFinished(true, cons);
                t.GetComponent<Restaurants>().restaurantData.level = r.level;
                break;
            }
        }

        // FindPlotById(r.plot_id).transform.GetChild(1).GetComponent<Restaurants>().ConstructionFinished();
    }
    public void ConstructionFInisged(RestaurantsData r , bool cons)

    {
       
        Plot p1 = FindPlotById(r.plot_id);
        foreach (Transform t in p1.transform)
        {
            if (t.GetComponent<Restaurants>() != null)
            {
               
              t.GetComponent<Restaurants>().ConstructionFinished(false,cons);
                t.GetComponent<Restaurants>().restaurantData.level = r.level;
                break;
            }
        }

       // FindPlotById(r.plot_id).transform.GetChild(1).GetComponent<Restaurants>().ConstructionFinished();
    }

    public void SetCityCameraPosition(int plotId)
    {
        Plot p = FindPlotById(plotId);
        cityCamera.transform.position = new Vector3(p.transform.position.x, 150, p.transform.position.z-(130-cityCamera.fieldOfView));
    }
    IEnumerator PlayAI()
    {
       
        foreach (ProfileData u in SocketMaster.instance.gamePlay.users_data)
        {
            if (!PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID).Equals(u._id))
            {
                if (u.role.Equals("ai"))
                    
                {
                    enemy.gameObject.SetActive(false);
                   
                }
                else
                {
                    
                    yield break;
                }
            }
        }
       

    }

    public void SendTaskDone(int taskId,string id)
    {
        Debug.LogError("SENDING TASk " + taskId);
        SendTaskDone roomData;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.TASKDONE,
            (socket, packet, args) =>
                    {
                        if (args != null && args.Length > 0)
                        {
                          
                            Debug.LogError("joinroom" + JsonMapper.ToJson(args[0]));
                          
                           // JoinRoomCallBack(
                             //   JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                        }
                    },
                    roomData = new SendTaskDone()
                    {
                        id = id,
                        taskId = taskId,
                        game_id = RoomContoller.SocketMaster.instance.gamePlay.game_id
                    });
    }

}

[System.Serializable]
public class SendTaskDone
{
    public string id;
    public int taskId;
    public string game_id;
}
[System.Serializable]
public class TaskDoneRecieved
{
    public int message;
    public int status;
    public SendTaskDone obj;

}