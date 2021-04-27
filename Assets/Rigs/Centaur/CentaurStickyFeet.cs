using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurStickyFeet : MonoBehaviour
{
    private Vector3 startingPos;

    private Vector3 targetPos;

    private Quaternion startingRot;

    private Quaternion targetRot;

    public float offset;

    CentaurController centaur;

    private void Start()
    {
        startingPos = transform.localPosition;
        startingRot = transform.localRotation;

        centaur = GetComponentInParent<CentaurController>();
    }

    void Update()
    {
        switch (centaur.state)
        {
            case CentaurController.States.Idle:
                Idle();
                break;
            case CentaurController.States.Walking:
                DoStep();
                break;
        }

        transform.position = AnimMath.Slide(transform.position, targetPos, .001f);
        transform.rotation = AnimMath.Slide(transform.rotation, targetRot, .001f);
    }

    void DoStep()
    {
        Vector3 stepPos = startingPos;

        float t = (Time.deltaTime + offset) * centaur.stepAnimTime;

        float legSwing = Mathf.Sin(t);
        stepPos += centaur.velocity * legSwing * centaur.stepScale.z;

        stepPos.y += Mathf.Cos(t) * centaur.stepScale.y;

        bool isOnGround = (stepPos.y < startingPos.y);
        if (isOnGround) stepPos.y = startingPos.y;

        float percent = 1 - Mathf.Abs(legSwing);
        float angleAngle = isOnGround ? 0 : -percent * 20;

        transform.localPosition = stepPos;

        targetRot = transform.parent.rotation * startingRot * Quaternion.Euler(0, 0, angleAngle);

        targetPos = transform.TransformPoint(stepPos);
    }

    void Idle()
    {
        targetPos = transform.TransformPoint(startingPos);
        targetRot = transform.parent.rotation * startingRot;
        DoRaycast();
    }

    void DoRaycast()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, .5f, 0), Vector3.down * 2);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPos = hit.point;

            targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }
}
