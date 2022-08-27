using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PuzzleCube cube = other.GetComponentInParent<PuzzleCube>();
        if (cube != null)
        {
            cube.ResetCube();
        }
    }
}
