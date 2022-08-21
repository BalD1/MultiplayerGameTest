using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UtilsForEditor))]
public class EDITOR_Utils : Editor
{
    private UtilsForEditor targetScript;

    public enum MainMenuState
    {
        FirstMenuBase,
        ConnectionMenuBase,
        InLobby,
        InRoom
    }

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

        EditorGUI.BeginChangeCheck();
        targetScript.mainMenuState = (UtilsForEditor.MainMenuState)EditorGUILayout.EnumPopup("State", targetScript.mainMenuState);
        if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
        {
            targetScript.firstMenu.SetActive(false);
            targetScript.connectionMenu.SetActive(false);
            targetScript.playButtons.SetActive(false);
            targetScript.lobbyPanel.SetActive(false);
            targetScript.roomPanel.SetActive(false);

            switch (targetScript.mainMenuState)
            {

                case UtilsForEditor.MainMenuState.FirstMenuBase:
                    targetScript.firstMenu.SetActive(true);
                    break;

                case UtilsForEditor.MainMenuState.InLobby:
                    targetScript.connectionMenu.SetActive(true);
                    targetScript.connectionMenu.SetActive(true);
                    targetScript.playButtons.SetActive(true);
                    targetScript.lobbyPanel.SetActive(true);
                    break;

                case UtilsForEditor.MainMenuState.InRoom:
                    targetScript.connectionMenu.SetActive(true);
                    targetScript.roomPanel.SetActive(true);
                    break;
            }
        }

        targetScript.firstMenu = (GameObject)EditorGUILayout.ObjectField("First Menu", targetScript.firstMenu, typeof(GameObject), true);
        targetScript.connectionMenu = (GameObject)EditorGUILayout.ObjectField("Connection Menu", targetScript.connectionMenu, typeof(GameObject), true);
        targetScript.playButtons = (GameObject)EditorGUILayout.ObjectField("Play Buttons", targetScript.playButtons, typeof(GameObject), true);
        targetScript.lobbyPanel = (GameObject)EditorGUILayout.ObjectField("Lobby Pannel", targetScript.lobbyPanel, typeof(GameObject), true);
        targetScript.roomPanel = (GameObject)EditorGUILayout.ObjectField("Room Panel", targetScript.roomPanel, typeof(GameObject), true);
    }

}