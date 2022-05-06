using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InGame
{
    public class UIManager : MonoBehaviour
    {
        public static  UIManager Instance;

        public BasePOpUp constructionPopUp,upgradePopUp,tasksPopUp, quitPopUp;

        public BasePOpUp currentPopUp;

        public ScreenUI screenUI;

        public GameObject swapUI;

        public Transform
           loader, internetConnectionPopUp;
        [SerializeField] private Text error;
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
           
        }

        private void OnEnable()
        {
            //keyboardInput =GameManager.Instance.player.transform.GetComponent<KeyboardInput>();
        }
        [SerializeField] public KeyboardInput keyboardInput;
        public GameObject inputImage;
       
        // Start is called before the first frame update
        public void HideInputs()
        {
            if(keyboardInput==null)
            {
                keyboardInput = GameManager.Instance.player.transform.GetComponent<KeyboardInput>();
            }
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
      

        public void Back()
        {
            if (RoomContoller.HomeScreen.status == STATUS.PLAY)
            {
                EnablePopUp(quitPopUp);
            }
            else
            {
                 SceneManager.LoadScene("Lobby");

            }
        }
        public void ShowError(string error)
        {
            this.error.text = error;
            Invoke("RemoveError", 2);
        }

        void RemoveError()
        {
            this.error.text = "";
        }

        public void ToggleLoader(bool toggle)
        {
            loader.gameObject.SetActive(toggle);
        }

        public bool CheckInternetConnection()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }

            return true;
        }


      
        void Update()
        {
            if (internetConnectionPopUp == null)
            {
                return;
            }

            if (CheckInternetConnection())
            {
                internetConnectionPopUp.gameObject.SetActive(false);
            }
            else
            {
                internetConnectionPopUp.gameObject.SetActive(true);
            }
        }
    }
}
