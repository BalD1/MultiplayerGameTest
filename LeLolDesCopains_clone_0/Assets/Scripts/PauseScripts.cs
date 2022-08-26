using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScripts : MonoBehaviour
{
    public void BackButton()
    {
        GameManager.Instance.GameState = GameManager.E_GameStates.InGame;
    }
}
