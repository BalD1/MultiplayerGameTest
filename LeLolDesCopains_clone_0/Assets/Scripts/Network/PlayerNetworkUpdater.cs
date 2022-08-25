using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkUpdater : MonoBehaviour, IPunObservable
{
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Transform headTransform;

    [SerializeField] private PhotonView view;

    [SerializeField] private Transform hudTransform;

    private Vector3 _serverPosition;
    private Quaternion _serverBodyRotation;
    private Quaternion _serverHeadRotation;

    private void Update()
    {
        if (view.IsMine)
        {
        }
        else
        {
            SmoothTransform();
            hudTransform.LookAt(GameManager.Instance.mainCamera.transform);
        }
    }

    private void SmoothTransform()
    {
        bodyTransform.position = Vector3.Lerp(bodyTransform.position, _serverPosition, .1f);

        bodyTransform.localRotation = Quaternion.Lerp(bodyTransform.localRotation, _serverBodyRotation, .1f);

        headTransform.localRotation = Quaternion.Lerp(headTransform.localRotation, _serverHeadRotation, .1f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)   // if is owner
        {
            WriteTransforms(stream);
        }
        else if (stream.IsReading)  // if is client
        {
            ReadTransforms(stream);
        }
    }

    // **************************
    // ********* WRITES *********
    // **************************

    private void WriteTransforms(PhotonStream stream)
    {
        // Write body position
        stream.SendNext(bodyTransform.position);

        // Write body rotation (y)
        stream.SendNext(bodyTransform.eulerAngles.y);

        // Write head rotation (x)
        stream.SendNext(headTransform.eulerAngles.x);

    }

    // **************************
    // ********** READS *********
    // **************************

    private void ReadTransforms(PhotonStream stream)
    {
        // Read body position
        _serverPosition = (Vector3)stream.ReceiveNext();

        // Read Body Rotation (y)
        _serverBodyRotation = Quaternion.Euler(0, (float)stream.ReceiveNext(), 0);

        // Read Head Rotation (x)
        _serverHeadRotation = Quaternion.Euler((float)stream.ReceiveNext(), 0, 0);
    }
}
