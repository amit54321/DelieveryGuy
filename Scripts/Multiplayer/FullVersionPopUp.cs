using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class FullVersionPopUp : MonoBehaviour
    {
        [SerializeField] private Text desc;
        public  void SetData(string desc)
        {
            gameObject.SetActive(true);
            this.desc.text = desc;
        }
    }
}
