using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError("GameManager Instance was not found, force creation");
                Create();
            }

            return instance;
        }
    }

    [SerializeField] private float gravity = -9.81f;
    public float Gravity { get => gravity; }

    private static GameManager Create()
    {
        GameObject gameManager = new GameObject();
        gameManager.name = "GameManager";

        gameManager.AddComponent<GameManager>();
        instance = gameManager.GetComponent<GameManager>();

        return gameManager.GetComponent<GameManager>();
    }

    private void Awake()
    {
        instance ??= this;
    }
}
