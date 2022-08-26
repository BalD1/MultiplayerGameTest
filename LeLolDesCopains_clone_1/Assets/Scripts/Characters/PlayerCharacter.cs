using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCharacter : Entity
{
    [Header("Components")]

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private CharacterController controller;

    [SerializeField] private GameObject eyeL;
    [SerializeField] private GameObject eyeR;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundMask;

    [SerializeField] private PhotonView view;

    [SerializeField] private string playerName;

    [SerializeField] private Camera cam;

    [SerializeField] private Transform grabbableAnchor;
    public Transform GrabbableAnchor { get => grabbableAnchor; }

    private GameObject activeInteractable;

    public string PlayerName { get => playerName; set => playerName = value; }


    public bool IsInit { get; private set; }

    [Header("Colors")]

    public Dictionary<PlayerColorableParts, Color> playerColor;

    [System.Serializable]
    public enum PlayerColorableParts
    {
        EyeL,
        EyeR,
        Head,
        Body
    }


    [Header("Stats")]

    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float groundDistance = .4f;

    private Vector3 movement;
    private Vector3 velocity;

    private Vector2 mouseAxis;
    private Vector2 movementAxis;

    private float xRotation;

    private bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.currentPlayerOwner = this;
        GameManager.Instance.mainCamera = cam;

        IsInit = true;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.E_GameStates.InGame)
        {
            CharacterMovements();
            CameraUpdate();
        }
        JumpBehaviour();

        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.HandlePause();

        if (Input.GetMouseButtonDown(0))
            SearchForGrabbable();

    }

    private void CameraUpdate()
    {
        mouseAxis.x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseAxis.y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseAxis.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        head.transform.localRotation = Quaternion.Euler(xRotation, 0, 0f);
        this.transform.Rotate(Vector3.up * mouseAxis.x);
    }

    private void CharacterMovements()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        movementAxis.x = Input.GetAxis("Horizontal");
        movementAxis.y = Input.GetAxis("Vertical");

        movement = transform.right * movementAxis.x + transform.forward * movementAxis.y;

        controller.Move(movement * speed * Time.deltaTime);
    }

    private void JumpBehaviour()
    {
        if (isGrounded && GameManager.Instance.GameState == GameManager.E_GameStates.InGame)
        {
            if (Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * GameManager.Instance.Gravity);

            if (velocity.y < 0)
                velocity.y = -2f;
        }

        velocity.y += GameManager.Instance.Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void SearchForGrabbable()
    {
        if (activeInteractable != null)
        {
            activeInteractable = activeInteractable.GetComponent<Iinteractable>().Interact(this.gameObject);
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~LayerMask.NameToLayer("Player")))
        {
            Iinteractable interactable = hit.collider.GetComponent<Iinteractable>();
            if (interactable != null)
            {
                activeInteractable = interactable.Interact(this.gameObject);
            }
        }
    }

}
