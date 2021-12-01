
/*
 * 
 * using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Im an idiot and have lost too many hours of work the past 3 weeks because theres no auto save
/// this is my temporary solution
/// https://forum.unity.com/threads/we-need-auto-save-feature.483853/#:~:text=Nothing%20you%20do%20in%20Unity,a%20prefab%2C%20you%20must%20save.
/// </summary>



[InitializeOnLoad]
public class alexsAutoSave  //AutoSaveOnRunMenuItem
{
public const string MenuName = "Tools/Autosave On Run";
private static bool isToggled;

static alexsAutoSave()
{
    EditorApplication.delayCall += () =>
    {
        isToggled = EditorPrefs.GetBool(MenuName, false);
        UnityEditor.Menu.SetChecked(MenuName, isToggled);
        SetMode();
    };
}

[MenuItem(MenuName)]
private static void ToggleMode()
{
    isToggled = !isToggled;
    UnityEditor.Menu.SetChecked(MenuName, isToggled);
    EditorPrefs.SetBool(MenuName, isToggled);
    SetMode();
}

private static void SetMode()
{
    if (isToggled)
    {
        EditorApplication.playModeStateChanged += AutoSaveOnRun;
    }
    else
    {
        EditorApplication.playModeStateChanged -= AutoSaveOnRun;
    }
}

private static void AutoSaveOnRun(PlayModeStateChange state)
{
    if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
    {
        Debug.Log("Auto-Saving before entering Play mode");

        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
    }
}
}
*/