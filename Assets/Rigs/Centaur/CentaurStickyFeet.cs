using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurStickyFeet : MonoBehaviour
{
    public Transform stepLead;

    public AnimationCurve tween;

    private Vector3 startingPos;

    private Vector3 targetPos;

    private Quaternion startingRot;

    private Quaternion targetRot;

    public float offset;

    public float animLength = .5f;

    public float stepDis = 5;

    private float timeCount;

    public bool footHasMoved = false;

    public bool isFootAnimating
    {
        get
        {
            return (timeCount < animLength);
        }
    }

    CentaurController centaur;

    private void Start()
    {
        startingPos = stepLead.position;
        startingRot = transform.localRotation;

        timeCount = 4;

        centaur = GetComponentInParent<CentaurController>();
    }

    void Update()
    {
        DoAStep();

        if (isFootAnimating)
        {
            // Add to the animation timer
            timeCount += Time.deltaTime;

            // Determine how far along in the animation we are based on time
            float percent = timeCount / animLength;

            // Add to the position 
            Vector3 stepPos = AnimMath.Lerp(startingPos, targetPos, percent);

            // Determine the y
            stepPos.y += tween.Evaluate(percent);

            // After y has been evaluated, reset the position to the determined position
            transform.position = stepPos;

            transform.rotation = AnimMath.Lerp(startingRot, targetRot, percent);
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
        if (footHasMoved) return false;

        // Get the vector from the IK target to the step lead
        Vector3 vToTarget = transform.position - stepLead.position;

        if (vToTarget.sqrMagnitude < stepDis * stepDis) return false;

        // Instantiate the ray into the scene.
        Ray ray = new Ray(stepLead.position + Vector3.up, Vector3.down);

        // Draw the ray from the step target (for visualization)
        Debug.DrawRay(ray.origin, ray.direction * 3);

        if (Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            // Setup the positions of feet
            startingPos = transform.position;
            startingRot = transform.rotation;

            targetPos = hit.point;
            targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            timeCount = 0;

            return true;
        }
        return false;
    }
}
