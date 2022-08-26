using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCube : MonoBehaviour, Iinteractable
{

    [SerializeField] private MeshRenderer cubeRenderer;

    [SerializeField] private Rigidbody body;
    [SerializeField] private BoxCollider physicsCollider;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float smoothTime = .5f;

    [SerializeField] private List<Collider> activeEnteredColliders = new List<Collider>();

    private Vector3 currentEndPosition;
    private Vector3 updatedEndPosition;

    private bool isGrabbed = false;
    private bool canUngrab = true;

    public PressurePlate currentPlate;

    private PlayerCharacter playerOwner;

    public GameObject Interact(GameObject sender)
    {

        if (!isGrabbed)
        {
            playerOwner = sender.GetComponent<PlayerCharacter>();
            this.transform.localEulerAngles = Vector3.zero;
            currentEndPosition = updatedEndPosition = playerOwner.GrabbableAnchor.position;

            body.useGravity = false;
            physicsCollider.enabled = false;

            body.isKinematic = true;
            isGrabbed = true;

            if (currentPlate != null)
            {
                currentPlate.OnInteract(null);
                currentPlate = null;
            }
        }
        else if (isGrabbed && canUngrab)
        {
            playerOwner = null;

            body.useGravity = true;
            physicsCollider.enabled = true;

            body.isKinematic = false;
            isGrabbed = false;

            body.velocity = Vector3.zero;

            return null;
        }

        return this.gameObject;

    }

    private void FixedUpdate()
    {
        if (isGrabbed)
        {
            Vector3 velocity = body.velocity;
            body.MovePosition(Vector3.SmoothDamp(body.position, playerOwner.GrabbableAnchor.position, ref velocity, smoothTime));
            body.velocity = velocity;
        }
    }

    private void LateUpdate()
    {
        if (isGrabbed)
        {
            this.transform.eulerAngles = new Vector3
            {
                x = 0,
                y = this.transform.eulerAngles.y,
                z = 0
            };
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGrabbed)
        {
            activeEnteredColliders.Add(other);
            canUngrab = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isGrabbed)
        {
            activeEnteredColliders.Remove(other);
            if (activeEnteredColliders.Count <= 0) canUngrab = true;
        }
    }

}
