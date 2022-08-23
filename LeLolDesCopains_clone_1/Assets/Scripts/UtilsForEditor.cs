using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsForEditor : MonoBehaviour
{
#if UNITY_EDITOR

    [System.Serializable]
    public struct MenusItems
    {
        public GameObject menuObject;
        public bool isActive;
        public string label;
    }

    [HideInInspector] public MenusItems[] menusItems;
#endif
}
