using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class SharePopUp : MonoBehaviour
    {
        
        [SerializeField] private Text desc;
        // Start is called before the first frame update
        public  void SetData(string desc)
        {
            gameObject.SetActive(true);
            this.desc.text = desc;
        }

      
    }
}
