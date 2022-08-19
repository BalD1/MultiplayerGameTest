using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Player relatedPlayer;

    [SerializeField] private float _cheapInterpolationValue = .1f;

    [SerializeField] private bool _serverAuth;

    [SerializeField] private Rigidbody _body;

    [SerializeField] private Transform _head;

    [SerializeField] private MeshRenderer _bodyRenderer;
    [SerializeField] private MeshRenderer _headRenderer;
    [SerializeField] private MeshRenderer _eyeLRenderer;
    [SerializeField] private MeshRenderer _eyeRRenderer;

    [SerializeField] private GameObject _HUD;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Camera _camera;

    [SerializeField] private List<Behaviour> otherComponentsToDisable;
    [SerializeField] private List<Behaviour> ownerComponentsToDisable;

    private NetworkVariable<PlayerNetworkData> _playerData;
    private NetworkVariable<PlayerNetworkInitiateData> _playerInitiateData;

    private static int playerCount = 0;

    private delegate void _OnPlayerSpawn();
    private static _OnPlayerSpawn _onPlayerSpawn;

    private Vector3 _posVel;
    private float _bodyRotVel;
    private float _headRotVel;

    private bool finishedInit = false;
    private bool initFlag = false;

    [SerializeField] private bool forceInit;


    private void Awake()
    {
        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerData = new NetworkVariable<PlayerNetworkData>(writePerm: permission);
        _playerInitiateData = new NetworkVariable<PlayerNetworkInitiateData>(writePerm: permission);

        playerCount++;

        string playerName = GameManager.Instance.PickedPlayerName.selectedPlayerName;

        playerName ??= "";
        if (playerName.Equals("")) playerName = "Player " + playerCount;

        _name.text = playerName;
        relatedPlayer.PlayerName = playerName;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            if (ownerComponentsToDisable != null)
                foreach (Behaviour item in ownerComponentsToDisable) { item.enabled = false; }

            _bodyRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            _headRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        }
        else
        {
            if (otherComponentsToDisable != null)
                foreach (Behaviour item in otherComponentsToDisable) { item.enabled = false; }

            InitiatePlayer();
        }
    }

    public void InitiatePlayer()
    {
        finishedInit = true;
        if (IsOwner) InitiateTransmitData();
        else InitiateConsumeData();
    }

    public override void OnNetworkDespawn()
    {
        playerCount--;
    }

    private void Update()
    {
        if (IsOwner) TransmitData();
        else ConsumeData();

        if (forceInit)
        {
            InitiatePlayer();
            forceInit = false;
        }

        if (finishedInit && !initFlag)
        {
            InitiatePlayer();
            initFlag = true;
            finishedInit = false;
        }
    }

    // **************************
    // ****** TRANSMISSION ******
    // **************************

    private void InitiateTransmitData()
    {
        var data = new PlayerNetworkInitiateData
        {
            Name = _name.text,
            ColorEyeL = ColorUtility.ToHtmlStringRGBA(_eyeLRenderer.material.color),
            ColorEyeR = ColorUtility.ToHtmlStringRGBA(_eyeRRenderer.material.color),
            ColorHead = ColorUtility.ToHtmlStringRGBA(_headRenderer.material.color),
            ColorBody = ColorUtility.ToHtmlStringRGBA(_bodyRenderer.material.color),
        };

        if (IsServer || !_serverAuth) _playerInitiateData.Value = data;
        else InitiateTransmitDataServerRpc(data);
    }

    private void TransmitData()
    {
        var data = new PlayerNetworkData
        {
            Position = _body.position,
            BodyRotation = this.transform.rotation.eulerAngles.y,
            HeadRotation = _head.localRotation.eulerAngles.x,
            IsInit = finishedInit,
        };

        if (IsServer || !_serverAuth) _playerData.Value = data;
        else TransmiteDataServerRpc(data);
    }

    [ServerRpc]
    private void InitiateTransmitDataServerRpc(PlayerNetworkInitiateData data)
    {
        _playerInitiateData.Value = data;
    }

    [ServerRpc]
    private void TransmiteDataServerRpc(PlayerNetworkData data)
    {
        _playerData.Value = data;
    }


    // *************************
    // ****** CONSUMATION ******
    // *************************

    private void InitiateConsumeData()
    {
        _name.text = _playerInitiateData.Value.Name.ToString();

        SetColorToRenderer(ref _eyeLRenderer, _playerInitiateData.Value.ColorEyeL);
        SetColorToRenderer(ref _eyeRRenderer, _playerInitiateData.Value.ColorEyeR);
        SetColorToRenderer(ref _headRenderer, _playerInitiateData.Value.ColorHead);
        SetColorToRenderer(ref _bodyRenderer, _playerInitiateData.Value.ColorBody);

        Debug.Log(IsOwner + " Name : " + _name.text);
    }

    private void ConsumeData()
    {
        // TODO : remake the interpolation
        _body.MovePosition(Vector3.SmoothDamp(_body.position, _playerData.Value.Position, ref _posVel, _cheapInterpolationValue));

        this.transform.rotation = Quaternion.Euler
                                (0,
                                Mathf.SmoothDampAngle(this.transform.rotation.eulerAngles.y, _playerData.Value.BodyRotation, ref _bodyRotVel, _cheapInterpolationValue),
                                0);

        _head.localRotation = Quaternion.Euler
                                (Mathf.SmoothDampAngle(_head.localRotation.eulerAngles.x, _playerData.Value.HeadRotation, ref _headRotVel, _cheapInterpolationValue),
                                0,
                                0);


        _HUD.transform.LookAt(Camera.main.transform);

        finishedInit = _playerData.Value.IsInit;
    }

    private void SetColorToRenderer(ref MeshRenderer renderer, FixedString32Bytes color)
    {
        Color c;
        ColorUtility.TryParseHtmlString(("#" + color.ToString()), out c);
        renderer.material.color = c;
    }

    private struct PlayerNetworkData : INetworkSerializable
    {
        private Vector3 _playerPos;
        private float _playerBodyRot;
        private float _playerHeadRot;

        private bool _isInit;

        internal Vector3 Position
        {
            get => _playerPos;
            set => _playerPos = value;
        }

        internal float BodyRotation
        {
            get => _playerBodyRot;
            set => _playerBodyRot = value;
        }

        internal float HeadRotation
        {
            get => _playerHeadRot;
            set => _playerHeadRot = value;
        }

        internal bool IsInit
        {
            get => _isInit;
            set => _isInit = value;
        }


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _playerPos);
            serializer.SerializeValue(ref _playerBodyRot);
            serializer.SerializeValue(ref _playerHeadRot);
            serializer.SerializeValue(ref _isInit);
        }
    }

    private struct PlayerNetworkInitiateData : INetworkSerializable
    {
        private FixedString32Bytes _name;

        private FixedString32Bytes _colorEyeL;
        private FixedString32Bytes _colorEyeR;
        private FixedString32Bytes _colorHead;
        private FixedString32Bytes _colorBody;

        internal FixedString32Bytes Name
        {
            get => _name;
            set => _name = value;
        }

        internal FixedString32Bytes ColorEyeL
        {
            get => _colorEyeL;
            set => _colorEyeL = value;
        }

        internal FixedString32Bytes ColorEyeR
        {
            get => _colorEyeR;
            set => _colorEyeR = value;
        }

        internal FixedString32Bytes ColorHead
        {
            get => _colorHead;
            set => _colorHead = value;
        }

        internal FixedString32Bytes ColorBody
        {
            get => _colorBody;
            set => _colorBody = value;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _name);
            serializer.SerializeValue(ref _colorEyeL);
            serializer.SerializeValue(ref _colorEyeR);
            serializer.SerializeValue(ref _colorHead);
            serializer.SerializeValue(ref _colorBody);
        }
    }
}
