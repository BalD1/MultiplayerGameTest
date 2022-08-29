using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToLink;
    [SerializeField] private Color objectsColor;

    private void Start()
    {
        IColorable ic;
        foreach (var item in objectsToLink)
        {
            ic = item.GetComponent<IColorable>();
            if (ic == null) ic = item.GetComponentInParent<IColorable>();
            if (ic == null) item.GetComponent<MeshRenderer>().material.color = objectsColor;
            else ic.ColorObject(objectsColor);

        }
    }
}
