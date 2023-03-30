using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(YandexSDK))]
public class YandexSDKCustomEditors : Editor
{

    Texture banner;

    void OnEnable()
    {
        banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Yandex.SDK/Editor/ysdk-logo.png", typeof(Texture));
    }

    public override void OnInspectorGUI()
    {
        
        GUILayout.Label(banner);
        if(CheckFirstScene())
        {
            base.OnInspectorGUI();
        }
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

    private bool CheckFirstScene()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Object placement");
        if (EditorSceneManager.GetActiveScene().buildIndex != 0)
        {
            GUILayout.Label(EditorGUIUtility.IconContent("d_Invalid"));
            GUILayout.EndHorizontal();
            GUILayout.TextArea("Prefab must be placed on the first scene of your project", GUI.skin.GetStyle("HelpBox"));
            return false;
        }
        else
        {
            GUILayout.Label(EditorGUIUtility.IconContent("Installed"));
            GUILayout.EndHorizontal();
            return true;
        }
        
    }
}

#if UNITY_EDITOR
public class YandexSDKPrebuildProcessor : IProcessSceneWithReport
{
    public int callbackOrder => 0;

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        if(scene.buildIndex != 0 && Object.FindObjectOfType<YandexSDK>() != null)
        {
            Debug.LogError($"YandexSDK prefab must be placed on the first scene of your project. Detected on scene index {scene.buildIndex}");
            throw new BuildFailedException("YandexSDK prefab must be placed on the first scene of your project");
        }
    }
}
#endif
