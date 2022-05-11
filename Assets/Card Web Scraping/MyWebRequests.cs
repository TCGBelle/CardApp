using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class MyWebRequests
{

    private class MyWebRequestsMB : MonoBehaviour { }// coroutine requires monobehavior

    private static MyWebRequestsMB myWebRequestsMB;

    private static void Init()
    {
        if (myWebRequestsMB == null)
        {
            // make insstance of class in scene
            GameObject gameObject = new GameObject("WebRequests");
            myWebRequestsMB = gameObject.AddComponent<MyWebRequestsMB>();
        }
    }
    public static void Get(string url, Action<string> onError, Action<string> onSuccess)
    {
        Init();
        myWebRequestsMB.StartCoroutine(GetCoroutine(url, onError, onSuccess));
    }

    private static IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSuccess)
    {
        // go to the url if cant tell me why.
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
                Debug.Log("erorr in coroutine");
            }
            else
            {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }

    public static void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        Init();
        myWebRequestsMB.StartCoroutine(GetTextureCoroutine(url, onError, onSuccess));
    }

    private static IEnumerator GetTextureCoroutine(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        // go to the url if cant tell me why.
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                onError(unityWebRequest.error);
            }
            else
            {
                DownloadHandlerTexture downloadHandlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                onSuccess(downloadHandlerTexture.texture);
            }
        }
    }
}
