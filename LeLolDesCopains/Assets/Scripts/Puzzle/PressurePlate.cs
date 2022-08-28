using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IColorable
{
    [SerializeField] private GameObject mesh;

    [SerializeField] private MeshRenderer colorableMesh;

    [SerializeField] private Transform target;
    private Vector3 baseLocalPos;

    [SerializeField] private GameObject[] activableTargets;
    private Iinteractable[] interactableTargets;

    private GameObject interactor;

    private void Awake()
    {
        baseLocalPos = mesh.transform.localPosition;

        interactableTargets = new Iinteractable[activableTargets.Length];
        if (activableTargets != null)
        {
            for (int i = 0; i < activableTargets.Length; i++)
            {
                interactableTargets[i] = activableTargets[i].GetComponent<Iinteractable>();
            }
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (interactor != null) return;

        PuzzleCube cube = collision.gameObject.GetComponent<PuzzleCube>();
        if (cube != null)
        {
            if (!cube.IsGrabbed && !cube.IsGrabbedByOther)
            {
                OnInteract(collision.gameObject);
                cube.currentPlate = this;
                return;
            }
        }

        PlayerCharacter character = collision.gameObject.GetComponentInParent<PlayerCharacter>();
        if (character != null)
        {
            OnInteract(collision.gameObject);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactor != null && other.gameObject.Equals(interactor))
        {
            OnInteract(null);
        }
    }

    public void OnInteract(GameObject _interactor)
    {
        interactor = _interactor;
        foreach (var item in interactableTargets)
        {
            item.Interact(this.gameObject);
        }

        if (_interactor == null)
        {
            mesh.transform.localPosition = baseLocalPos;
        }
        else
        {
            mesh.transform.localPosition = target.localPosition;
        }
    }

    public void ColorObject(Color _color)
    {
        colorableMesh.material.color = _color;
    }
}
