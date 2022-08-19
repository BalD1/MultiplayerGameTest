using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
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

    [SerializeField] private string playerName;
    public string PlayerName { get => playerName; set => playerName = value; }

    [SerializeField] private PlayerNetwork network;

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
        UpdateColors();


        IsInit = true;
        network.InitiatePlayer();
    }

    private void Update()
    {
        CharacterMovements();
        CameraUpdate();
        JumpBehaviour();
    }

    public void UpdateColors()
    {
        if (playerColor != null)
            return;

        ColorPlayer colors = GameManager.Instance.PickedPlayerColors;
        if (colors == null || colors.PlayerParts == null)
            return;

        playerColor ??= new Dictionary<PlayerColorableParts, Color>();

        for (int i = 0; i < colors.PlayerParts.Count; i++)
        {
            Color c = colors.PlayerParts[i].color;
            playerColor.Add(colors.PlayerPartName[i], c);

            switch (colors.PlayerPartName[i])
            {
                case PlayerColorableParts.EyeL:
                    eyeL.GetComponent<MeshRenderer>().material.color = c;
                    break;

                case PlayerColorableParts.EyeR:
                    eyeR.GetComponent<MeshRenderer>().material.color = c;
                    break;

                case PlayerColorableParts.Head:
                    head.GetComponent<MeshRenderer>().material.color = c;
                    break;

                case PlayerColorableParts.Body:
                    body.GetComponent<MeshRenderer>().material.color = c;
                    break;
            }
        }
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
        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * GameManager.Instance.Gravity);

            if (velocity.y < 0)
                velocity.y = -2f;
        }

        velocity.y += GameManager.Instance.Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
