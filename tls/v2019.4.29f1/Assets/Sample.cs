using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
        _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Width(Screen.width - 50), GUILayout.Height(Screen.height - 50));
        GUILayout.Label($"ApiCompatibilityLevel: {GetApiCompatibilityLevelString()}");
        requestUrl = GUILayout.TextField (requestUrl);

        if (GUILayout.Button("Send UnityWebRequest"))
        {
            DoUnityWebRequest();
        }

        if (GUILayout.Button("Send Request from HttpClient"))
        {
            DoRequestFromHttpClient();
        }
        
        if (GUILayout.Button("Copy to ClipBoard"))
        {
            CopyToClipBoard();
        }
        _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.Width(Screen.width - 50), GUILayout.Height(Screen.height / 2));
        _textAreaString = GUILayout.TextArea(_textAreaString, GUILayout.Width(Screen.width - 50));
        GUILayout.EndScrollView();
        
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        
        

    }

    private string DoUnityWebRequest()
    {
        return string.Empty;
    }

    private string DoRequestFromHttpClient()
    {
        return string.Empty;
    }

    private void CopyToClipBoard()
    {
        
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