using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager instance;
    public static SaveLoadManager Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError("SaveLoad Instance was not found, force creation");
                Create();
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public enum SaveKeys
    {
        // COLORS

        C_eyeL,
        C_eyeR,
        C_body,
        C_head,

        Name,
    }

    private static SaveLoadManager Create()
    {
        GameObject saveLoadManager = new GameObject();
        saveLoadManager.name = "GameManager";

        saveLoadManager.AddComponent<SaveLoadManager>();
        instance = saveLoadManager.GetComponent<SaveLoadManager>();

        return saveLoadManager.GetComponent<SaveLoadManager>();
    }

    public void SaveColors(Color eyeLColor, Color eyeRColor, Color headColor, Color bodyColor)
    {
        string key, color;

        // Eye L
        SetColorsKeyAndValue(SaveKeys.C_eyeL, eyeLColor, out key, out color);
        PlayerPrefs.SetString(key, color);

        // Eye R
        SetColorsKeyAndValue(SaveKeys.C_eyeR, eyeRColor, out key, out color);
        PlayerPrefs.SetString(key, color);
        // Head
        SetColorsKeyAndValue(SaveKeys.C_head, headColor, out key, out color);
        PlayerPrefs.SetString(key, color);

        // Body
        SetColorsKeyAndValue(SaveKeys.C_body, bodyColor, out key, out color);
        PlayerPrefs.SetString(key, color);
    }

    private void SetColorsKeyAndValue(SaveKeys key, Color value, out string keyString, out string colorString)
    {
        keyString = EnumToString(key);
        colorString = "#" + ColorUtility.ToHtmlStringRGBA(value);
    }

    /// <summary>
    /// 0 = eye L |
    /// 1 = eye R |
    /// 2 = head |
    /// 3 = body
    /// </summary>
    /// <returns></returns>
    public string[] LoadColors()
    {
        string[] colors = new string[4];

        colors[0] = PlayerPrefs.GetString(EnumToString(SaveKeys.C_eyeL));
        colors[1] = PlayerPrefs.GetString(EnumToString(SaveKeys.C_eyeR));
        colors[2] = PlayerPrefs.GetString(EnumToString(SaveKeys.C_head));
        colors[3] = PlayerPrefs.GetString(EnumToString(SaveKeys.C_body));

        return colors;
    }

    public void SaveName(string _name)
    {
        PlayerPrefs.SetString(EnumToString(SaveKeys.Name), _name);
    }
    public string LoadName()
    {
        return PlayerPrefs.GetString(EnumToString(SaveKeys.Name));
    }

    private string EnumToString(SaveKeys key)
    {
        return Enum.GetName(typeof(SaveKeys), key);
    }
}
