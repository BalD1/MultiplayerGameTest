using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsForEditor : MonoBehaviour
{
    [HideInInspector] public GameObject firstMenu;
    [HideInInspector] public GameObject connectionMenu;
    [HideInInspector] public GameObject playButtons;
    [HideInInspector] public GameObject lobbyPanel;
    [HideInInspector] public GameObject roomPanel;

    public enum MainMenuState
    {
        FirstMenuBase,
        InLobby,
        InRoom
    }

    [HideInInspector] public MainMenuState mainMenuState;
}
