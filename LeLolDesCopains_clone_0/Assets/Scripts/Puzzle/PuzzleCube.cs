using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzleCube : MonoBehaviour, Iinteractable
{

    [SerializeField] private MeshRenderer cubeRenderer;

    [SerializeField] private Rigidbody body;
    [SerializeField] private BoxCollider physicsCollider;

    [SerializeField] private Vector3 offset;
    private Vector3 basePos;

    [SerializeField] private float smoothTime = .5f;

    [SerializeField] private List<Collider> activeEnteredColliders = new List<Collider>();

    [SerializeField] private PuzzleCubeNetwork selfNetwork;
    [SerializeField] private Outline outline;
    [SerializeField] private Color canInteractColor;
    [SerializeField] private Color canNotInteractColor;
    private float baseOutlineValue;

    private bool isGrabbed = false;
    private bool isGrabbedByOther = false;

    public bool IsGrabbed { get => isGrabbed; }
    public bool IsGrabbedByOther { get => isGrabbedByOther; }

    private bool canUngrab = true;

    public PressurePlate currentPlate;

    private PlayerCharacter playerOwner;
    private PlayerCharacter playerClient;

    private void Awake()
    {
        basePos = this.transform.position;
    }

    private void Start()
    {
        baseOutlineValue = outline.OutlineWidth;
        outline.OutlineColor = canInteractColor;

        SetOutlineActive(false);

        Invoke(nameof(DelayedStart), 1f);
    }

    private void DelayedStart()
    {
        playerClient = GameManager.Instance.currentPlayerOwner;
        Debug.Log(playerClient);
    }

    public GameObject Interact(GameObject sender)
    {
        if (isGrabbedByOther) return null;

        if (!isGrabbed)
        {
            playerOwner = sender.GetComponent<PlayerCharacter>();
            this.transform.localEulerAngles = Vector3.zero;

            isGrabbed = true;

            SetBody(IsGrabbed);

            ResetCurrentPlate();

            selfNetwork.ChangeGrabState(this.IsGrabbed);

            SetOutlineActive(true);
        }
        else if (isGrabbed && canUngrab)
        {
            playerOwner = null;

            isGrabbed = false;

            SetBody(IsGrabbed);

            body.velocity = Vector3.zero;

            selfNetwork.ChangeGrabState(this.IsGrabbed);

            SetOutlineActive(false);

            return null;
        }

        return this.gameObject;
    }

    public void SetOutlineActive(bool active)
    {
        if (active) outline.OutlineWidth = baseOutlineValue;
        else outline.OutlineWidth = 0;
    }

    public void OnGrabByOther(bool state)
    {
        isGrabbedByOther = state;
        SetBody(isGrabbedByOther);

        if (!isGrabbedByOther)
            ResetCurrentPlate();
    }

    private void SetBody(bool _isGrabbed)
    {
        if (_isGrabbed)
        {
            body.useGravity = false;
            physicsCollider.enabled = false;

            body.isKinematic = true;
        }
        else
        {
            body.useGravity = true;
            physicsCollider.enabled = true;

            body.isKinematic = false;
        }
    }

    private void ResetCurrentPlate()
    {
        if (currentPlate != null)
        {
            currentPlate.OnInteract(null);
            currentPlate = null;
        }
    }

    public void ResetCube()
    {
        canUngrab = true;
        isGrabbedByOther = false;

        Interact(null);

        this.transform.position = basePos;
    }

    private void FixedUpdate()
    {
        if (isGrabbedByOther) return;

        if (isGrabbed)
        {
            Vector3 velocity = body.velocity;
            body.MovePosition(Vector3.SmoothDamp(body.position, playerOwner.GrabbableAnchor.position, ref velocity, smoothTime));
            body.velocity = velocity;
        }
    }

    private void LateUpdate()
    {
        if (isGrabbedByOther) return;

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
        if (isGrabbedByOther) return;

        if (isGrabbed)
        {
            activeEnteredColliders.Add(other);
            canUngrab = false;
            outline.OutlineColor = canNotInteractColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isGrabbedByOther) return;

        if (isGrabbed)
        {
            activeEnteredColliders.Remove(other);
            if (activeEnteredColliders.Count <= 0 && !canUngrab)
            {
                canUngrab = true;
                outline.OutlineColor = canInteractColor;
            }
        }
    }

    private void OnMouseOver()
    {
        if (Vector3.Distance(playerClient.transform.position, this.transform.position) > playerClient.InteractDistance)
            SetOutlineActive(false);
        else
            SetOutlineActive(true);
    }

    private void OnMouseExit()
    {
        if (!isGrabbed)
            SetOutlineActive(false);
    }
}
