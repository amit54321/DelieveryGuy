using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public bool cameraDragging = true;

  //  public float outerLeft = -10f;
  //  public float outerRight = 10f;
    Vector3 movement;

    public float left, right,up,down;

    void Update()
    {



   //   Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

    //  float left = Screen.width * 0.2f;
  //    float right = Screen.width - (Screen.width * 0.2f);

      


        if (Input.GetMouseButtonDown(0))
        {
            cameraDragging = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.GetComponent<OnClick>() != null)
                {
                    hit.transform.GetComponent<OnClick>().OnCLickMethod();
                }
            }
        }

        if (cameraDragging)
        {

            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (Input.GetMouseButton(0))
            {

                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
                movement += move*0.01f;
                transform.Translate(move, Space.World);
                if(transform.position.x<=left)
                {
                    transform.position = new Vector3(left, transform.position.y, transform.position.z);
                }
                else if (transform.position.x >= right)
                {
                    transform.position = new Vector3(right, transform.position.y, transform.position.z);
                }

                if (transform.position.z <= up)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y,up);
                }
                else if (transform.position.z >= down)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, down);
                }
            }
        }

            if(!cameraDragging)
        {
          //  movement -= Vector3.one;
           // transform.Translate(movement, Space.World);
        }

        if (Input.GetMouseButtonUp(0))
        {
            cameraDragging = false;
        }

    }


}