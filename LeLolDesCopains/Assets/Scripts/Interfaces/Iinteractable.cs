using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteractable
{
    public GameObject Interact(GameObject sender);

    public void SetOutlineActive(bool active);
}
