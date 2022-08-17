using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private float _cheapInterpolationValue = .1f;

    [SerializeField] private bool _serverAuth;

    [SerializeField] private Rigidbody _body;
    [SerializeField] private Transform _head;
    [SerializeField] private MeshRenderer _bodyRenderer;
    [SerializeField] private MeshRenderer _headRenderer;
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


    private void Awake()
    {
        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerData = new NetworkVariable<PlayerNetworkData>(writePerm: permission);
        _playerInitiateData = new NetworkVariable<PlayerNetworkInitiateData>(writePerm: permission);

        playerCount++;
        _name.text = "Player " + playerCount;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            if (otherComponentsToDisable != null)
                foreach (Behaviour item in otherComponentsToDisable) { item.enabled = false; }

            _onPlayerSpawn?.Invoke();
        }
        else
        {
            if (ownerComponentsToDisable != null)
                foreach (Behaviour item in ownerComponentsToDisable) { item.enabled = false; }

            _bodyRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            _headRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            _onPlayerSpawn += OnPlayerSpawn;

            InitiateTransmitData();
        }
    }

    private void OnPlayerSpawn()
    {
        InitiateConsumeData();
    }

    public override void OnNetworkDespawn()
    {
        playerCount--;
        _onPlayerSpawn -= OnPlayerSpawn;
    }

    private void Update()
    {
        if (IsOwner) TransmitData();
        else ConsumeData();
    }

    // **************************
    // ****** TRANSMISSION ******
    // **************************

    private void InitiateTransmitData()
    {
        var data = new PlayerNetworkInitiateData
        {
            Name = _name.text.ToString(),
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
    }

    private struct PlayerNetworkData : INetworkSerializable
    {
        private Vector3 _playerPos;
        private float _playerBodyRot;
        private float _playerHeadRot;

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


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _playerPos);
            serializer.SerializeValue(ref _playerBodyRot);
            serializer.SerializeValue(ref _playerHeadRot);
        }
    }

    private struct PlayerNetworkInitiateData : INetworkSerializable
    {
        private FixedString32Bytes _name;

        internal FixedString32Bytes Name
        {
            get => _name;
            set => _name = value;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _name);
        }
    }
}
