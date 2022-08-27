using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Iinteractable, IColorable
{
    [SerializeField] private Transform target;

    [SerializeField] private MeshRenderer colorableMesh;

    [SerializeField] private GameObject mesh;

    [SerializeField] private short neededInteractors = 1;
    private short currentInteractors = 0;

    private List<GameObject> interactors = new List<GameObject>();

    [SerializeField] private float lerpTime = 1f;

    [SerializeField] private float distanceBeforeStop = 1f;

    private Vector3 basePos;

    [SerializeField] private bool isLocked = false;
    private bool isOpen = false;

    private bool animationEnded = true;

    private void Start()
    {
        basePos = mesh.transform.position;
    }

    public GameObject Interact(GameObject sender)
    {
        if (isLocked && sender.CompareTag("Player")) return null;

        foreach (GameObject i in interactors)
        {
            if (sender.Equals(i))
            {
                interactors.Remove(i);
                currentInteractors--;

                if (isOpen) animationEnded = false;

                isOpen = false;

                return null;
            }
        }


        interactors.Add(sender);
        currentInteractors++;

        if (currentInteractors >= neededInteractors)
        {
            isOpen = true;
            animationEnded = false;
        }

        return null;
    }

    private void Update()
    {
        if (isOpen && !animationEnded)
        {
            Animate(target.position);
        }
        else if (!isOpen && !animationEnded)
        {
            Animate(basePos);
        }
    }

    private void Animate(Vector3 _target)
    {
        mesh.transform.position = Vector3.Lerp(mesh.transform.position, _target, lerpTime);

        if (Vector3.Distance(mesh.transform.position, _target) <= distanceBeforeStop)
            animationEnded = true;
    }

    public void ColorObject(Color _color)
    {
        colorableMesh.material.color = _color;
    }
}
