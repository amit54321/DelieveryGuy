using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHandler : MonoBehaviour
{
    public List<Transform> arrows = new List<Transform>();
    private void OnEnable()
    {
        foreach(Transform t in transform)
        {
            arrows.Add(t);
        }
    }
    // Start is called before the first frame update
    public void SetArrow(Transform target)
    {
        foreach(Transform t in arrows)
        {

            if(t.position.x>target.position.x)
            {
                t.localEulerAngles = new Vector3(0, 0, 90); 
            }
           else if (t.position.x <= target.position.x)
            {
                t.localEulerAngles = new Vector3(0, -180, 90);
            }
            else if (t.position.z > target.position.z)
            {
                t.localEulerAngles = new Vector3(0, -90, 90);
            }
            else if (t.position.z <= target.position.z)
            {
                t.localEulerAngles = new Vector3(0, 90, 90);
            }
        }
    }

    

    // Update is called once per frame
    
}
