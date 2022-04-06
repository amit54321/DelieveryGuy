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

    public void ResetPlayer()
    {
        player.transform.position = startingPoint;
        transform.eulerAngles = Vector3.zero;

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
            COnstructDemoBuildings();

            //  Invoke("ShowInputs",)
        }
        else if (HomeScreen.status == STATUS.SET)
        {
            EnableCityCamera();
        }
    }
    public void EnableCityCamera()
    {
        cityCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        InGame.UIManager.Instance.HideInputs();
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

    void COnstructDemoBuildings()
    {
        for(int i=1;i<=5;i++)
        {
            ConstructBuilding(i, 1, 10, 10, 10, 0);
        }
    }
    public void ConstructBuilding(int plot_id,int building_id,int cTime,int quantity,int waitTime,int level)
    {
        Plot p = FindPlotById(plot_id);
        GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/Restaurant/" + building_id), p.transform);
        g.transform.localEulerAngles = p.eulerAngle;
        g.GetComponent<Restaurants>().StartCoroutine( g.GetComponent<Restaurants>().StartConstruction(plot_id, cTime,level,quantity,waitTime,building_id));
        p.enabled = false;
        p.GetComponent<MeshRenderer>().enabled = false;
        allRestaurants.Add(g.GetComponent<Restaurants>());
        p.GetComponent<Collider>().enabled = false;
    }

    public void UpgradeBuilding( int plot_id,int building_id, int cTime,int quantity,int waitTime,int level)
    {
        Restaurants p = FindRestaurantById(plot_id);
        p.StartCoroutine(p.StartConstruction(plot_id, cTime,level,quantity,waitTime,building_id));
        p.enabled = false;
        p.GetComponent<Collider>().enabled = false;
    }

    public void EnableCurrentTaskHouse()
    {
        HouseCollider h = FIndHouseById(currentTask.house_id);
        h.gameObject.SetActive(true);
        h.EnableHouse();
        SetArrow(h.transform);
    }
    public void TaskSelected(TaskData task)
    {
        currentTask = task;
        InGame.UIManager.Instance.ShowInputs();
        Restaurants r = FindRestaurantById(task.building_id);
        r.restaurantCollider.gameObject.SetActive(true);
        r.restaurantCollider.EnableCollider();
        SetArrow(r.transform);
        dir.SetArrow(player, r.transform);
    }
}
