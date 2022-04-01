using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class UIManager : MonoBehaviour
    {
        public static  UIManager Instance;

        public BasePOpUp constructionPopUp;

        public BasePOpUp currentPopUp;
        public void EnablePopUp(BasePOpUp popUp)
        {
            if(currentPopUp!=null)
            {
                currentPopUp.gameObject.SetActive(false);
            }
            currentPopUp = popUp;
            currentPopUp.gameObject.SetActive(true);
        }
        public void DisablePopUp()
        {
            currentPopUp.gameObject.SetActive(false);
            currentPopUp = null;
        }
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
