using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UtilsForEditor))]
public class EDITOR_Utils : Editor
{
    private UtilsForEditor targetScript;

    private void OnEnable()
    {
        targetScript = (UtilsForEditor)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawMainMenuUtils();
    }

    private void DrawMainMenuUtils()
    {
        EditorGUILayout.LabelField("Main Menu");

        if (targetScript.menusItems.Length == 0) return;

        UtilsForEditor.MenusItems[] _menuItems = targetScript.menusItems;
        UtilsForEditor.MenusItems item;
        for (int i = 0; i < _menuItems.Length; i++)
        {
            item = _menuItems[i];

            DrawMenuObjectAndActive(item.menuObject, item.isActive, item.label);
        }
    }

    private void DrawMenuObjectAndActive(GameObject menu, bool isActive, string label)
    {
        bool lastIsActive = isActive = menu.activeInHierarchy;
        EditorGUILayout.BeginHorizontal();

        menu = (GameObject)EditorGUILayout.ObjectField(label, menu, typeof(GameObject), true);
        isActive = EditorGUILayout.Toggle(isActive);

        if (lastIsActive != isActive)
        {
            menu.SetActive(isActive);
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

}