using Authentication;
using LitJson;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public List<Restaurants> restaurants;

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

    public Transform player,enemy;

    public Direction dir;

    public List<WayPoints> wayPoints;

    public Vector3 startingPoint;

    public bool gameOver;
    int totalTasks;

    public GameObject mapCube;
    public GameObject minimap;

    public List<GameObject> allScenes;


    Vector3 enemyRot, enemyPos;
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
       
        Debug.LogError(GetCurrentTime());
        SetCars();

    }
    public void SetPlayerPosition(LobbyData.SyncPosition data)
    {
       

        Debug.LogError(data.id + "  GETTING INFO "+data.message.x );
        if (!PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID).Equals(data.id))
        {
            PlayerPosition player = data.message;

            enemyPos= new Vector3((float)player.x, (float)player.y, (float)player.z); ;
            enemyRot = new Vector3((int)player.rotX, (int)player.rotY, (int)player.rotZ);
         //   enemy.transform.position = new Vector3((float)player.x, (float)player.y, (float)player.z);
         //      //    enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, new Vector3((float)player.x, (float)player.y, (float)player.z), Time.deltaTime * 1500);
          //         enemy.transform.eulerAngles = new Vector3((int)player.rotX, (int)player.rotY, (int)player.rotZ);

                

            }



    }
    public void PLayerPositionToTarget()
    {
        Transform target = null ;
        foreach(Restaurants res in allRestaurants)
        {
            if (res.restaurantCollider.gameObject.activeSelf)
            {
                target = res.restaurantCollider.transform;
                break;
            }
        }
        if(target!=null)
        {
            float y = player.transform.position.y + 1;
            player.transform.position = new Vector3(target.transform.position.x, y, target.transform.position.z);
         //   player.transform.position = target.position; ;
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
    }
    int GetCurrentTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return cur_time;
    }

    int opponentTasksDone = -1;
    public void SetOpponentTasks()
    {
        opponentTasksDone++;
        InGame.UIManager.Instance.screenUI.OpponentSetTasks(opponentTasksDone);
    }
    public bool DisableCurrentTask()
    {

        foreach (TaskData t in taskDatas)
        {
            if (currentTask.id == t.id)
            {
                SendTaskDone(t.id);
                taskDatas.Remove(currentTask);
                break;
            }
        }
        InGame.UIManager.Instance.screenUI.SetTasks(totalTasks - taskDatas.Count);
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


                totalTasks = taskDatas.Count;
        }
    private void OnEnable()
    {
      
        if (HomeScreen.status==STATUS.PLAY)
        {
            InitializeTasksData();
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.tasksPopUp);
            InGame.UIManager.Instance.screenUI.gameObject.SetActive(true);
            EnablePlayerCamera();
           
            COnstructInitialBuildings();
            InGame.UIManager.Instance.HideInputs();
            SetOpponentTasks();
          //  StartCoroutine(Testing());
         

            //  Invoke("ShowInputs",)
        }
        else if (HomeScreen.status == STATUS.SET)
        {
            EnableCityCamera();
            COnstructInitialBuildings();
        }
    }
    public void EnableCityCamera()
    {
        cityCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        InGame.UIManager.Instance.HideInputs();
        arrowHandler.arrows[0].gameObject.SetActive(false);
        player.gameObject.SetActive(false);
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

        if (SocketMaster.instance.gamePlay.ai == 1)
        {
         //   return;
        }

        if (enemy != null)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemyPos, Time.deltaTime * 15);
            enemy.transform.eulerAngles = enemyRot;
        }

        frame++;
        if (frame % 4 == 0)
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
            Debug.LogError( "  GETTING INFO " + playerPosition.x);





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

    void COnstructInitialBuildings()
    {

        foreach (TimersData r in SocketMaster.instance.profileData.timers)
        {
          int leftTime = Mathf.Abs((int)(r.end/1000 - GetCurrentTime()));
            Debug.LogError(GetCurrentTime() +"   NOW  " +  r.end/1000 +  "    "+leftTime);
            ConstructBuilding(r.plot_id, r.restaurant_id, (int)leftTime, 10, 0, r.level);
        }
        foreach (RestaurantsData r in SocketMaster.instance.profileData.restaurants)
        {
                 ConstructBuilding(r.plot_id, r.restaurant_id, 0, 10, 0, r.level);
        }

       for(int i=1;i<=10;i++)
        {
            ConstructBuilding(i, i, 0, 10, 10, 1);
        }
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
        Restaurants r = FindRestaurantById(task.restaurantId);
        r.restaurantCollider.gameObject.SetActive(true);
        r.restaurantCollider.EnableCollider();
        EnableMapCube(r.transform);
      
     //  dir.SetArrow(player, r.transform);
    }





    public void ConstructBuilding(int plot_id, int building_id, int cTime, int quantity, int waitTime, int level)
    {
        Plot p = FindPlotById(plot_id);
        GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/Restaurant/" + building_id), p.transform);
        g.transform.localEulerAngles = p.eulerAngle;
        
        {
            g.GetComponent<Restaurants>().StartCoroutine(g.GetComponent<Restaurants>().StartConstruction(plot_id, cTime, level, quantity, waitTime, building_id));
        }
        if (cTime <= 0)
        {
            g.GetComponent<Restaurants>().ConstructionFinished();
            return;
        }
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

    public void ConstructionFInisged(RestaurantsData r)

    {
        FindPlotById(r.plot_id).transform.GetChild(0).GetComponent<Restaurants>().ConstructionFinished();
    }

    public void SendTaskDone(int taskId)
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
                        id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
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
    public SendTaskDone message;
    public int status;
  
}