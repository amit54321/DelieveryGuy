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

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }    
    }
    // Start is called before the first frame update
    void Start()
    {
        currentRestaurant = restaurants[0];
        arrowHandler.SetArrow(currentRestaurant.transform);
    }

    private void OnEnable()
    {
        if(HomeScreen.status==STATUS.PLAY)
        {
            EnablePlayerCamera();
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
    public void ConstructBuilding(int plot_id,int building_id,int cTime)
    {
        Plot p = FindPlotById(plot_id);
        GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/Restaurant/" + building_id), p.transform);
        g.GetComponent<Restaurants>().StartCoroutine( g.GetComponent<Restaurants>().StartConstruction(plot_id, cTime));
        p.enabled = false;
        p.GetComponent<Collider>().enabled = false;
    }
}
