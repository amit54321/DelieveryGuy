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
    public bool stable;
    // Start is called before the first frame update
    void Start()
    {
        if(stable)
        {
            return;
        }
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

            if (currentWayPoint % 2 == 0)
            {
                while (Mathf.Abs(Vector3.Distance(transform.position, pos)) > 1)
                {

                    transform.position = Vector3.MoveTowards(transform.position, pos, 0.1f);
                    yield return new WaitForSeconds(0.01f);

                }
            }
            else
              
            {
                Vector3 current = transform.position;
                // while (Mathf.Abs(Vector3.Distance(transform.position, pos)) > 1)
                {
                    //     Vector3 v0 = pos - transform.position;
                    //Find the vector from the triangle to the square
                    //  Vector3 v1 = current - transform.position;

                    //   Vector3 axis = Vector3.Cross(v0, v1).normalized;
                    Vector3 posR = wayPoints[currentWayPoint].transform.parent.transform.position;
                    //Rotate the square around the triangle at 5 degrees/second, passing through the cross
                    for (int i = 0; i < 50; i++)
                    {
                        yield return new WaitForSeconds(0.01f);
                        transform.RotateAround(posR,new Vector3(0,1,0) , 300 * Time.deltaTime);
                       if( Mathf.Abs(Vector3.Distance(transform.position, pos)) <= 2)
                                
                        {
                            break;
                        }
                    }
                }
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

    IEnumerator Deactivate(Collision col)
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.ResetPlayer();
       // col.gameObject.SetActive(false);
    }
    void OnCollisionEnter(Collision col)
        {
        if(col.transform.CompareTag("Player"))
        {
            StartCoroutine(Deactivate(col));
        }
        }
}
