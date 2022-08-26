using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool isLocked = false;
    private bool isOpen = false;

    public GameObject Interact(GameObject sender)
    {
        if (isLocked && sender.CompareTag("Player")) return null;

        isOpen = !isOpen;

        Debug.Log("Is open : " + isOpen);

        return null;
    }
}
