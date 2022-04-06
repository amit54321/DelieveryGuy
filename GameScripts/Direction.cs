using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Direction : MonoBehaviour
{
  public   RectTransform arrow;
  public  Transform t1, t2;
    // Start is called before the first frame update
   public  void SetArrow(Transform t1,Transform t2)
    {
        this.t1 = t1;
        this.t2 = t2;
    }

    // Update is called once per frame
    void Update()
    {
        if(t1!=null)
        {
            Vector3 direction = t2.transform.position - t1.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            arrow.rotation = Quaternion.Lerp(this.transform.rotation, rotation, Time.deltaTime);
        }
    
}
        
    
}
