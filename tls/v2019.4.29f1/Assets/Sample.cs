using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class Sample : MonoBehaviour
{ 
    [SerializeField]
    private string requestUrl = "https://hogehoge.com";

    private Vector2 _scrollPos;
    private Vector2 _scrollPos2;

    private string _textAreaString = "";

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Width(Screen.width - 10), GUILayout.Height(Screen.height - 10));
        GUILayout.Label($"ApiCompatibilityLevel: {GetApiCompatibilityLevelString()}");
        GUILayout.Label($"Unity version: {UnityEditorInternal.InternalEditorUtility.GetFullUnityVersion()}");
        requestUrl = GUILayout.TextField (requestUrl);

        if (GUILayout.Button("Send UnityWebRequest"))
        {
            StartCoroutine(DoUnityWebRequest());
        }

        if (GUILayout.Button("Send Request from HttpClient"))
        {
            DoRequestFromHttpClient();
        }
        
        if (GUILayout.Button("Copy to ClipBoard"))
        {
            CopyToClipBoard();
        }

        _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.Width(Screen.width - 50), GUILayout.Height(Screen.height * 0.8f));
        _textAreaString = GUILayout.TextArea(_textAreaString, GUILayout.Width(Screen.width - 60), GUILayout.Height(Screen.height * 0.6f));
        GUILayout.EndScrollView();

        if (GUILayout.Button("Clear log"))
        {
            _textAreaString = string.Empty;
        }
        
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private IEnumerator DoUnityWebRequest()
    {
        using (var request = UnityWebRequest.Get(requestUrl))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            
            AddMessage($"request start {requestUrl}");
            yield return request.SendWebRequest();

            if (request.isHttpError)
            {
                AddMessage($"is http error");
            }
            else if (request.isNetworkError)
            {
                AddMessage("is network error");
            }
            else if (!string.IsNullOrEmpty(request.error))
            {
                AddMessage($"request.error: {request.error}");
            }
            else
            {
                AddMessage("success");
            }

            AddMessage($"status code:{request.responseCode}");
            var responseHeaderString = string.Empty;
            foreach (var kv in request.GetResponseHeaders())
            {
                responseHeaderString += $"\n name:{kv.Key}, value:{kv.Value}";
            }
            AddMessage($"response header {responseHeaderString}");
            AddMessage($"response: {request.downloadHandler.text}");
            request.downloadHandler?.Dispose();
        }
    }

    private string DoRequestFromHttpClient()
    {
        return string.Empty;
    }

    private void CopyToClipBoard()
    {
        var message = string.Empty;
        message += $"ApiCompatibilityLevel: {GetApiCompatibilityLevelString()}\n";
        message += $"Unity version: {UnityEditorInternal.InternalEditorUtility.GetFullUnityVersion()}\n";
        message += "===================================== \n";
        GUIUtility.systemCopyBuffer = message + _textAreaString;
    }

    private void AddMessage(string message)
    {
        _textAreaString += $"[{DateTime.Now:O}] {message}\n";
    }

    private string GetApiCompatibilityLevelString()
    {
        var buildTargetGroup = BuildTargetGroup.Unknown;
#if UNITY_IPHONE
        buildTargetGroup = BuildTargetGroup.iOS;
#elif UNITY_ANDROID
        buildTargetGroup = BuildTargetGroup.Android;
#elif UNITY_EDITOR
        buildTargetGroup = BuildTargetGroup.Standalone;
#endif
        return PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup).ToString();
    }
}