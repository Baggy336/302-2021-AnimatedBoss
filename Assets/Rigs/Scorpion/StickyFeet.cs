using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFeet : MonoBehaviour
{
    public Transform stepTarget;

    public AnimationCurve verticalMovement;

    private Vector3 plantedPos;

    private Vector3 previousPlantedPos;

    private Quaternion plantedRot = Quaternion.identity;

    private Quaternion previousPlantedRotation = Quaternion.identity;

    private float tweenLength = .3f;

    private float timeCurrent = 0;

    private void Start()
    {
        
    }

    void Update()
    {
        if (CheckIfCanStep())
        {
            DoRayCast();
        }

        if (timeCurrent < tweenLength) // The animation is playing
        {
            timeCurrent += Time.deltaTime; // move playhead forward

            float p = timeCurrent / tweenLength;

            Vector3 finalPos = AnimMath.Lerp(previousPlantedPos, plantedPos, p);

            finalPos.y += verticalMovement.Evaluate(p);

            transform.position = finalPos;

            transform.rotation = AnimMath.Lerp(previousPlantedRotation, plantedRot, p);
        }
        else // Animation is not playing
        {
            transform.position = plantedPos;
            transform.rotation = plantedRot;
        }

        


    }

    void DoRayCast()
    {
        // Cast a ray from a meter above it's position, down to the ground
        Ray ray = new Ray(stepTarget.position + Vector3.up, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 3);

        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            // Setup beginning of tween
            previousPlantedPos = transform.position;
            previousPlantedRotation = transform.rotation;

            // Setup end of tween
            plantedPos = hit.point;
            plantedRot = Quaternion.FromToRotation(transform.up, hit.normal);

            // Begin Animation
            timeCurrent = 0;
        }
    }

    bool CheckIfCanStep()
    {
        Vector3 vBetween = transform.position - stepTarget.position;
        float threshold = 5;

        return (vBetween.sqrMagnitude > threshold * threshold);
    }
}
