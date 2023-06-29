using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace MirrorBasics
{
    public class AutoHostClient : MonoBehaviour
    {

        public InputField nameText;
        string fakeDeviceId;

        public static string userId;
        public static int accountId;

        public GameObject loginScreen;
        public bool _fakeDeviceId;

        public bool NetworkConnected = true;
        bool Respawntime;






        private void OnEnable()
        {
            Invoke("ConnectToMirror", 1f);
        }

        public void ConnectToMirror()
        {
            if (_fakeDeviceId)
                fakeDeviceId = "363987";// SystemInfo.deviceUniqueIdentifier;
            else
                fakeDeviceId = SystemInfo.deviceUniqueIdentifier;

         //   fakeDeviceId = "7f19abff04992a38f3fc3437f4263e89283157a3";
            if (!Application.isBatchMode)
            { //Headless build
                Debug.Log($"=== Client Build ===");
                RegisterAutomatically();
            }
            else
            {
                Debug.Log($"=== Server Build ===");
                ConnectMirror();
            }
        }



        void DeleteaLLmatchesCallBack(string c)
        {

        }
        void ConnectMirror()
        {
            if (!Application.isBatchMode)
            { //Headless build
                Debug.Log($"=== Client Build ===");
                //networkManager.StartClient ();
                NetworkManager.singleton.StartClient();
            }
            else
            {
                Debug.Log($"=== Server Build ===");
              //  APIHandler.instance.DeleteAllMatches(DeleteaLLmatchesCallBack);
            }
        }
        public IEnumerator ReconnectToServer()
        {
            while (!NetworkConnected)
            {
                yield return new WaitForSeconds(10f);
                //APIHandler.instance.transform.GetComponent<AutoHostClient>().ConnectToMirror();
                if (!NetworkConnected)
                {
                   // ConnectToMirror();

                }
            }
        }

        IEnumerator TimetowaitforRespawn()
        {
            Respawntime = true;
            yield return new WaitForSeconds(15f);

            Respawntime = false;
        }

        public void JoinLocal()
        {
            NetworkManager.singleton.networkAddress = "localhost";
            NetworkManager.singleton.StartClient();
        }

        public void RegisterAutomatically()
        {
          //  APIHandler.instance.Register(fakeDeviceId, RegisterCallBack);
        }

        public void Register()
        {
           // APIHandler.instance.AddRegisterName(nameText.text, fakeDeviceId, RegisterCallBack);

        }
        public void RegisterCallBack(string callback)
        {
           // UserDataCallBack data = JsonUtility.FromJson<UserDataCallBack>(callback);
           // APIHandler.instance.userData = data.data;
           // userId = data.data._id;
          //  accountId = data.data.accountId;
           // if (!string.IsNullOrEmpty(APIHandler.instance.userData.name))
            {
           //     RoomContoller.SocketMaster.instance.InitialiseSocket();
                ConnectMirror();
            }
          ///  else
            {
           //     loginScreen.SetActive(true);
            }

        }

    }
}