using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour
{
    private static AssetsManager instance;
    public static AssetsManager Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError("AssetsManager instance was not found, force create");
                Create();
            }

            return instance;
        }
    }

    private static AssetsManager Create()
    {
        GameObject assetsManager = new GameObject();
        assetsManager.name = "AssetsManager";

        assetsManager.AddComponent<AssetsManager>();
        instance = assetsManager.GetComponent<AssetsManager>();

        return assetsManager.GetComponent<AssetsManager>();
    }

    private void Awake()
    {
        instance ??= this;
    }
}
