using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurStickyFeet : MonoBehaviour
{
    /// <summary>
    /// This will reference an empty object which represents where the next step will be.
    /// </summary>
    public Transform stepLead;

    private Vector3 currentPos;

    private Vector3 targetPos;

    private Vector3 previousPos;

    private Quaternion currentRot;

    private Quaternion previousRot = Quaternion.identity;

    private Quaternion targetRot = Quaternion.identity;

    public bool isFootAnimating
    {
        get
        {
            return (timeCount < animLength);
        }
    }

    private bool hasStepped = false;

    private float timeCount = 0;

    private float animLength = .5f;

    private void Start()
    {
        currentRot = transform.localRotation;
    }

    void Update()
    {
        if (isFootAnimating)
        {
            timeCount += Time.deltaTime;

            float percent = timeCount / animLength;

            Vector3 stepPos = AnimMath.Lerp(previousPos, targetPos, percent);

            transform.position = stepPos;

            transform.rotation = AnimMath.Lerp(previousRot, targetRot, percent);
        }
        else
        {
            transform.position = targetPos;
            transform.rotation = targetRot;
        }
    }

    public bool DoAStep()
    {
        if (isFootAnimating) return false;

        if (hasStepped) return false;

        // Get the vector from the IK target to the step lead
        Vector3 vToTarget = transform.position - stepLead.position;

        // This value will determine how far away the target should be before stepping
        float stepDis = 1.5f;

        if (vToTarget.sqrMagnitude < stepDis * stepDis) return false;

        // Instantiate the ray into the scene.
        Ray ray = new Ray(stepLead.position + Vector3.up, Vector3.down);

        // Draw the ray from the step target (for visualization)
        Debug.DrawRay(ray.origin, ray.direction * 5);

        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            // Setup the positions of feet
            previousPos = transform.position;
            currentRot = transform.rotation;

            transform.localRotation = previousRot;

            targetPos = hit.point;
            targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            timeCount = 0;

            hasStepped = true;

            return true;
        }
        return false;
    }
}
