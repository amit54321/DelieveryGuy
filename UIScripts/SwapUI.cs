using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapUI : MonoBehaviour
{
   

    // Update is called once per frame
   public void Close()
    {
        GameManager.Instance.SwapCancel(this.gameObject);
    }
}
