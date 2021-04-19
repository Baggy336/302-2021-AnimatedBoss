using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurStickyFeet : MonoBehaviour
{
    /// <summary>
    /// This will reference an empty object which represents where the next step will be.
    /// </summary>
    Transform stepLead;

    Vector3 currentPos;

    Vector3 targetPos;

    bool isFootAnimating = false;

    void Update()
    {
        // Draw a ray below the foot IK
        Raycaster();

        // Step forward.
        DoStep();
    }

    void Raycaster()
    {
        // Instantiate the ray into the scene.
        Ray ray = new Ray(stepLead.position + Vector3.up, Vector3.down);

        // Draw the ray from the step target (for visualization)
        Debug.DrawRay(ray.origin, ray.direction * 5);

        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            // Setup the positions of feet
            currentPos = transform.position;

            targetPos = hit.point;
        }
    }

    void DoStep()
    {
        // Get the vector from the IK target to the step lead
        Vector3 vToTarget = transform.position - stepLead.position;

        // This value will determine how far away the target should be before stepping
        float stepDis = 1.5f;

        if (vToTarget.sqrMagnitude > stepDis * stepDis) isFootAnimating = true;
    }
}
