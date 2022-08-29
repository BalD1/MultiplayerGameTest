using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableSocle : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 basePos;
    private Vector3 currentTarget;

    [SerializeField] private float distanceBeforeStop = 1f;
    [SerializeField] private float lerpSpeed = .5f;
    private float lerpCurrentValue = 0;



    private bool move;

    private void Start()
    {
        basePos = this.transform.position;
    }

    private void Update()
    {
        if (move)
        {
            lerpCurrentValue += (Time.time * lerpSpeed * Time.deltaTime);
            this.transform.position = Vector3.Lerp(this.transform.position, currentTarget, lerpCurrentValue);
        }
    }

    public void MoveToTarget(bool basePosTarget)
    {
        currentTarget = basePosTarget ? basePos : target.transform.position;
        move = true;
    }
}
