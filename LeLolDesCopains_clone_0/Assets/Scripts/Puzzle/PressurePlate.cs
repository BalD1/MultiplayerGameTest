using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IColorable
{
    [SerializeField] private GameObject mesh;

    [SerializeField] private MeshRenderer colorableMesh;

    [SerializeField] private Transform target;
    private Vector3 baseLocalPos;

    [SerializeField] private GameObject activableTarget;
    private Iinteractable interactableTarget;

    private GameObject interactor;

    private void Awake()
    {
        baseLocalPos = mesh.transform.localPosition;
        if (activableTarget != null)
            interactableTarget = activableTarget.GetComponent<Iinteractable>();

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
        interactableTarget.Interact(this.gameObject);

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
