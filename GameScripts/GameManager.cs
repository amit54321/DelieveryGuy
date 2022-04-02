using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StudentData
{
    public string name;
    public int rollName;
    public int marks;

    public StudentData(string n, int rollName, int marks)
    {
        this.name = n;
        this.rollName = rollName;
        this.marks = marks;
    }

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
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }    
    }
    public void SetArrow(Restaurants target)
    {
        currentRestaurant = target;
        arrowHandler.SetArrow(target.transform);
    }

    private void OnEnable()
    {
        if(HomeScreen.status==STATUS.PLAY)
        {
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.tasksPopUp);
            EnablePlayerCamera();
        }
       else if (HomeScreen.status == STATUS.SET)
        {
            EnableCityCamera();
        }
        COnstructDemoBuildings();
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

    public void TaskSelected(TaskData task)
    {
        currentTask = task;
        Restaurants r = FindRestaurantById(task.building_id);
        r.restaurantCollider.gameObject.SetActive(true);
        HouseCollider h = FIndHouseById(task.house_id);
        h.gameObject.SetActive(true);
        SetArrow(r);
    }
}
