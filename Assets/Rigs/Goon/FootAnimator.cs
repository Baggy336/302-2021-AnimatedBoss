using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script animates the foot / legs by
/// changing the local position of this object (IK target).
/// </summary>
public class FootAnimator : MonoBehaviour
{
    /// <summary>
    /// The local space starting position of this object.
    /// </summary>
    private Vector3 startingPos;

    /// <summary>
    /// Local space starting rotation of this object.
    /// </summary>
    private Quaternion startingRot;

    /// <summary>
    /// An offset value to apply to time. Measured in radians.
    /// </summary>
    public float stepOffset = 0;

    GoonController goon;

    private Vector3 targetPos;

    private Quaternion targetRot;

    void Start()
    {
        // Hold reference to the starting position of this object.
        startingPos = transform.localPosition;
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

        // Ease position and rotation towards the values being set:
        transform.position = AnimMath.Slide(transform.position, targetPos, .001f);
        transform.rotation = AnimMath.Slide(transform.rotation, targetRot, .001f);
    }

    void AnimateWalk()
    {
        Vector3 finalPos = startingPos;

        // Math to move the finalPos
        float time = (Time.time + stepOffset) * goon.stepSpeed;

        // Lateral movement (z, x)
        float frontToBack = Mathf.Sin(time);
        finalPos += goon.moveDir * frontToBack * goon.walkScale.z;

        // Vertical movement (y)
        finalPos.y += Mathf.Cos(time) * goon.walkScale.y;

        bool isOnGround = (finalPos.y < startingPos.y);
        if (isOnGround) finalPos.y = startingPos.y;


        // Convert from z (-1 to 1) to p (0 ,1, 0)
        float p = 1 - Mathf.Abs(frontToBack);
        float anklePitch = isOnGround ? 0 : - p * 20;

        transform.localPosition = finalPos;

        //transform.localRotation = startingRot * Quaternion.Euler(0, 0, anklePitch);
        targetRot = transform.parent.rotation * startingRot * Quaternion.Euler(0, 0, anklePitch);

        targetPos = transform.TransformPoint(finalPos);
    }

    void AnimateIdle()
    {
        targetPos = transform.TransformPoint(startingPos);
        targetRot = transform.parent.rotation * startingRot;
        FindGround();
    }

    void FindGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, .5f, 0), Vector3.down * 2);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPos = hit.point;

            targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else
        {

        }


    }
}
