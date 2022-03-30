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

}
