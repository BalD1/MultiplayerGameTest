using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError("UIManager instance was not found, force creation");
                Create();
            }

            return instance;
        }
    }

    private static UIManager Create()
    {
        GameObject uiManager = new GameObject();
        uiManager.name = "UIManager";

        uiManager.AddComponent<UIManager>();
        instance = uiManager.GetComponent<UIManager>();

        return instance;
    }

    private void Awake()
    {
        instance ??= this;
    }
}
