using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class UIManager : MonoBehaviour
    {
        public static  UIManager Instance;
        private void Awake()
        {
            if(Instance==null)
            {
                Instance = this;
            }
        }
        [SerializeField] public KeyboardInput keyboardInput;
        public GameObject inputImage;
        void Start()
        {

            keyboardInput = GameObject.FindWithTag("Player").transform.GetComponent<KeyboardInput>();
        }
        // Start is called before the first frame update
        public void HideInputs()
        {
            inputImage.SetActive(false);
            keyboardInput.SetAccelaration(false);
            keyboardInput.SetBrake(false);
            keyboardInput.SetAngle(0);
        }
        public void ShowInputs()
        {
            inputImage.SetActive(true);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
