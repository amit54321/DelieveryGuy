using Authentication;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform player;

    public Direction dir;

    public List<WayPoints> wayPoints;

    public Vector3 startingPoint;

    public bool gameOver;
    int totalTasks;

    public GameObject mapCube;
    public GameObject minimap;
    public void ResetPlayer()
    {
        player.transform.position = startingPoint;
        transform.eulerAngles = Vector3.zero;
        player.transform.GetComponent<Rigidbody>().isKinematic = false;
    }
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        gameOver = false;
        totalTasks = taskDatas.Count;

       
    }

    public bool DisableCurrentTask()
    {

        foreach(TaskData t in taskDatas)
        {
            if(currentTask.id == t.id)
            {
                taskDatas.Remove(currentTask);
                break;
            }
        }
        InGame.UIManager.Instance.screenUI.SetTasks(totalTasks - taskDatas.Count);
        if(totalTasks-taskDatas.Count<=0)
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

    private void OnEnable()
    {
      
        if (HomeScreen.status==STATUS.PLAY)
        {
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.tasksPopUp);
            InGame.UIManager.Instance.screenUI.gameObject.SetActive(true);
            EnablePlayerCamera();
            InGame.UIManager.Instance.HideInputs();
            COnstructInitialBuildings();

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
        minimap.SetActive(false);
    }

    public void EnablePlayerCamera()
    {
        cityCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        InGame.UIManager.Instance.ShowInputs();
    }
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
       foreach(RestaurantsData r in SocketMaster.instance.profileData.restaurants)
        {
                 ConstructBuilding(r.plot_id, r.restaurant_id, 10, 10, 0, r.level);
        }
    }
   

    public void EnableCurrentTaskHouse()
    {
        HouseCollider h = FIndHouseById(currentTask.house_id);
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
        Restaurants r = FindRestaurantById(task.building_id);
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
        if (waitTime <= 0)
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
}
