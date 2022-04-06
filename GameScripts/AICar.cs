using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICar : MonoBehaviour
{
    float currentRot;
  public   Quaternion rot;
    public int lane;
    public List<GameObject> wayPoints;
   public int currentWayPoint;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("car  " + transform.rotation);
        rot = transform.rotation;
        wayPoints = GameManager.Instance.wayPoints[lane].ways;
        StartCoroutine(Move());

    }


  
    // Update is called once per frame
    private IEnumerator Move()
    {
        while (!GameManager.Instance.gameOver)
        {
           
            transform.LookAt(wayPoints[currentWayPoint].transform.position);
            rot = transform.rotation;
            Vector3 pos = wayPoints[currentWayPoint].transform.position;// GetNextPos(transform.rotation.y);
                wayPoints[currentWayPoint].SetActive(false);
          
            while (Mathf.Abs(Vector3.Distance(transform.position, pos)) > 1)
            {
               
                transform.position = Vector3.MoveTowards(transform.position, pos, 0.1f);
                yield return new WaitForSeconds(0.01f);

            }
            if (currentWayPoint >= wayPoints.Count-1)
            {
                currentWayPoint =0 ;
            }
            else
            {
                currentWayPoint++;
            }
        }
       // 
    }

    void OnTrigger(Collider col)
        {

        }
}
