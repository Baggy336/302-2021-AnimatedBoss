using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipAnimator : MonoBehaviour
{
    GoonController goon;

    Quaternion startingRot;

    private float rollAmt = 4;

    void Start()
    {
        startingRot = transform.localRotation;

        goon = GetComponentInParent<GoonController>();
    }

    void Update()
    {
        switch (goon.state)
        {
            case GoonController.States.Idle:
                AnimateIdle();
                break;
            case GoonController.States.Walk:
                AnimateWalk();
                break;
        }
    }

    void AnimateIdle()
    {
        transform.localRotation = startingRot;
    }
    void AnimateWalk()
    {
        float time = Time.time * goon.stepSpeed;

        float roll = Mathf.Sin(time) * rollAmt;

        transform.localRotation = Quaternion.Euler(0, 0, roll);
    }
}
