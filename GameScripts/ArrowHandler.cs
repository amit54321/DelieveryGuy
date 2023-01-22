using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHandler : MonoBehaviour
{
    public Transform arrows;
   public  Transform target;
    public GameObject cube;
    void Start()
    {
        if(arrows!=null && Camera.main!=null)
        arrows.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 1));
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
        arrows.transform.rotation = lookRot;
        arrows.localEulerAngles = new Vector3(0, arrows.localEulerAngles.y+90, 90);

        arrows.transform.position = cube.transform.position;
    }

 
    // Start is called before the first frame update
    public void SetArrow(Transform target)
    {
        this.target = target;
       
    }

    

    
    
}
