using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using LitJson;

namespace Authentication
{
    public class WebRequest : MonoBehaviour
    {
        
        public void SetCurrentScreen()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"id", PlayerPrefs.GetString(PlayerPrefsData.ID)},
                {"screen", gameObject.name}
            };

            StartCoroutine(PostNetworkRequest(AuthenticationConstants.CURRENTSCREEN, data, GetUserDataCallBack, Error, false));
        }

        public void GetUserDataCallBack(string callback)
        {
            
        }
        public void Error(string error)
        {
        
        }

        public IEnumerator PostRequest(string uri, WWWForm form, UnityAction<string> callBack,
            UnityAction<string> error, bool token)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
            {
                webRequest.timeout = 30;
                if (token)
                {
                    webRequest.SetRequestHeader("authorization", PlayerPrefs.GetString(PlayerPrefsData.TOKEN));
                }
                // Request and wait for the desired page.


                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    error.Invoke(webRequest.error);
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    callBack.Invoke(webRequest.downloadHandler.text);
                }
            }
        }
      static   bool sendingRequest = false;

        
        public static IEnumerator PostNetworkRequest(string uri, Dictionary<string, object> data,
            UnityAction<string> callBack, UnityAction<string> error, bool token)
        {
            if(sendingRequest)
            {
               yield break;
            }
            sendingRequest = true;
            var json = JsonMapper.ToJson(data);
            Debug.LogError("json  " + json);
           
            using (var www = PostJson(uri, json))
            {
                www.SetRequestHeader("authorization", PlayerPrefs.GetString(PlayerPrefsData.TOKEN));
                www.timeout = 30;

                Debug.Log("Sending request " + uri + ", " + json);
                yield return www.SendWebRequest();
                sendingRequest = false;
                if (www.isNetworkError)
                {
                    error.Invoke(www.error);
                    Debug.Log(": Error: " + www.error);
                }
                else
                {
                    callBack.Invoke(www.downloadHandler.text);
                    Debug.Log(www.downloadHandler.text);
                }
            }
          //  yield return new WaitForSeconds(2);
          //  sendingRequest = false;
        }



        private static UnityWebRequest PostJson(string uri, string postData)
        {
            UnityWebRequest request = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST);
            SetupJsonPost(request, postData);
            return request;
        }

        private static void SetupJsonPost(UnityWebRequest request, string postData)
        {
            byte[] data = (byte[]) null;
            if (!string.IsNullOrEmpty(postData))
                data = Encoding.UTF8.GetBytes(postData);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
            request.uploadHandler.contentType = "application/json";
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        }

        public IEnumerator GetRequest(string uri, UnityAction<string> callBack, UnityAction<string> error, bool token)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.timeout = 30;
               // if (token)
                {
                    Debug.Log("TOKEn   " + uri);
                  //  webRequest.SetRequestHeader("authorization", PlayerPrefs.GetString(PlayerPrefsData.TOKEN));
                }

                yield return webRequest.SendWebRequest();

             //   string[] pages = uri.Split('/');
              //  int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                    error.Invoke(webRequest.error);
                  
                }
                else
                {
                    Debug.Log(":Received: " + webRequest.downloadHandler.text);
                    callBack.Invoke(webRequest.downloadHandler.text);
                  
                }
            }
        }
    }
}