using System;
using System.Collections;
using System.Collections.Generic;
using Authentication;
using RoomContoller;
//using RoomContoller;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadImage : MonoBehaviour
{
    public static IEnumerator LoadRawImage(string url, RawImage image)
    {
        if (String.IsNullOrEmpty(url) || image == null)
        {
            yield break;
        }

        if (url.Contains("kaiser"))
        {
            string[] u = url.Split('/');
            int a =int.Parse(u[1]);
            image.texture = SocketMaster.instance.defaultSprites[a];
            yield break;
        }

        if (Authentication.UIManager.profilePictures.ContainsKey(url))
        {
            image.texture = Authentication.UIManager.profilePictures[url];
            yield break;
        }

        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            image.texture = DownloadHandlerTexture.GetContent(unityWebRequest);
            if (Authentication.UIManager.profilePictures.Count < 1000)
            {
                Authentication.UIManager.profilePictures.Add(url, image.texture);
            }
        }
    }
}