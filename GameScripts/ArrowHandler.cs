using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHandler : MonoBehaviour
{
    public List<Transform> arrows = new List<Transform>();
   public  Transform target;
    public GameObject cube;
    void Start()
    {
        arrows[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
        if(target== null)
        {
            return;
        }
       
     
      
        Vector3 d = target.position - cube.transform.position;
        Quaternion lookRot = Quaternion.LookRotation(d);
        arrows[0].transform.rotation = lookRot;
        arrows[0].localEulerAngles = new Vector3(0, arrows[0].localEulerAngles.y+90, 90);

        arrows[0].transform.position = cube.transform.position;
    }

    private void OnEnable()
    {
        foreach(Transform t in transform)
        {
        //    arrows.Add(t);
        }
    }
    // Start is called before the first frame update
    public void SetArrow(Transform target)
    {
        this.target = target;
        foreach(Transform t in arrows)
        {
          //  Vector3 diff = (target.transform.position - t.transform.position).normalized;
          //  t.rotation =  Quaternion.Euler(diff*180);
            if(t.position.x>target.position.x)
            {
               // t.localEulerAngles = new Vector3(0, 0, 90); 
            }
           else if (t.position.x <= target.position.x)
            {
               // t.localEulerAngles = new Vector3(0, -180, 90);
            }
            else if (t.position.z > target.position.z)
            {
               // t.localEulerAngles = new Vector3(0, -90, 90);
            }
            else if (t.position.z <= target.position.z)
            {
              //  t.localEulerAngles = new Vector3(0, 90, 90);
            }
        }
    }

    

    // Update is called once per frame
    
}
