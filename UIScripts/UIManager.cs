using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class UIManager : MonoBehaviour
    {
        public static  UIManager Instance;

        public BasePOpUp constructionPopUp,upgradePopUp,tasksPopUp;

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
            if(currentPopUp==null)
            {
                return;
            }
            currentPopUp.gameObject.SetActive(false);
            currentPopUp = null;
        }
        private void Awake()
        {
            if(Instance==null)
            {
                Instance = this;
            }
            keyboardInput = GameObject.FindWithTag("Player").transform.GetComponent<KeyboardInput>();
        }
        [SerializeField] public KeyboardInput keyboardInput;
        public GameObject inputImage;
       
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
