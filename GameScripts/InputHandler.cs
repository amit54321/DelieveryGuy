using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartGame.KartSystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;







public class InputHandler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

  

    [SerializeField] public KeyboardInput keyboardInput;
    float angle; bool brake; bool accelearate;

    
    // Start is called before the first frame update
    void Start()
    {
      



     

        keyboardInput = GameObject.FindWithTag("Player").transform.GetComponent<KeyboardInput>();
    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (keyboardInput == null)
        {
            keyboardInput = GameManager.Instance.player.transform.GetComponent<KeyboardInput>();
        }
        if (gameObject.CompareTag("LEFT"))
        {
            angle = -1;
            keyboardInput.SetAngle(angle);
        }
         if (gameObject.CompareTag("RIGHT"))
        {
            angle = 1;
            keyboardInput.SetAngle(angle);
        }

        if (gameObject.CompareTag("UP"))
        {
            accelearate = true;
            keyboardInput.SetAccelaration(accelearate);
        }
         if (gameObject.CompareTag("DOWN"))
        {
            brake = true;
            keyboardInput.SetBrake(brake);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (keyboardInput == null)
        {
            keyboardInput = GameManager.Instance.player.transform.GetComponent<KeyboardInput>();
        }
        if (gameObject.CompareTag("LEFT"))
        {
           angle = 0;
            keyboardInput.SetAngle(angle);
        }
         if (gameObject.CompareTag("RIGHT"))
        {
            angle = 0;
            keyboardInput.SetAngle(angle);
        }

        if (gameObject.CompareTag("UP"))
        {
            accelearate = false;
            keyboardInput.SetAccelaration(accelearate);
        }
         if (gameObject.CompareTag("DOWN"))
        {
            brake = false;
            keyboardInput.SetBrake(brake);
        }

    }

    public void Stop()
    {
        
    }

   
   
}
