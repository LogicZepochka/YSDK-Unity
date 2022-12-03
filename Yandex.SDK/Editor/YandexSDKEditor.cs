using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(YandexSDK))]
public class YandexSDKEditor : Editor
{

    Texture banner;

    void OnEnable()
    {
        banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Yandex.SDK/Editor/ysdk-logo.png", typeof(Texture));
    }

    public override void OnInspectorGUI()
    {
        
        GUILayout.Label(banner);
        CheckFirstScene();


        GUILayout.Space(30);
        if(GUILayout.Button("Documentation"))
        {
            Application.OpenURL("https://github.com/LogicZepochka/YSDK-Unity/wiki/%D0%92%D0%B7%D0%B0%D0%B8%D0%BC%D0%BE%D0%B4%D0%B5%D0%B9%D1%81%D1%82%D0%B2%D0%B8%D0%B5");
        }
        if (GUILayout.Button("Github"))
        {
            Application.OpenURL("https://github.com/LogicZepochka/YSDK-Unity");
        }
    }

    private void CheckFirstScene()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Object placement");
        if (EditorSceneManager.GetActiveScene().buildIndex != 0)
        {
            GUILayout.Label(EditorGUIUtility.IconContent("d_Invalid"));
            GUILayout.EndHorizontal();
            GUILayout.TextArea("Prefab must be placed on the first scene of your project", GUI.skin.GetStyle("HelpBox"));
        }
        else
        {
            GUILayout.Label(EditorGUIUtility.IconContent("Installed"));
            GUILayout.EndHorizontal();
        }
        
    }
}
