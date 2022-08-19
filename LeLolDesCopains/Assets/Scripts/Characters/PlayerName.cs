using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public string selectedPlayerName { get; private set; }

    private void Start()
    {
        string loadedName = SaveLoadManager.Instance.LoadName();
        if (loadedName != null)
        {
            selectedPlayerName = loadedName;
            inputField.text = loadedName;
        }
    }

    public void OnNameChange(string newName)
    {
        selectedPlayerName = newName;
        SaveLoadManager.Instance.SaveName(newName);
    }
}
