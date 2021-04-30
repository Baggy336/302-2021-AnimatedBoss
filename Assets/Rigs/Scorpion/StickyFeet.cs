using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class StickyFeet : MonoBehaviour
{
    public Transform stepTarget;

    public AnimationCurve verticalMovement;

    private Quaternion startingRot;

    private Vector3 plantedPos;

    private Vector3 previousPlantedPos;

    private Quaternion plantedRot = Quaternion.identity;

    private Quaternion previousPlantedRotation = Quaternion.identity;

    private float tweenLength = .3f;

    private float timeCurrent = 0;

    public bool isAnimating
    {
        get
        {
            return (timeCurrent < tweenLength);
        }
    }

    public bool footHasMoved = false;

    Transform kneePole;

    private void Start()
    {
        kneePole = transform.GetChild(0);

        startingRot = transform.localRotation;
    }

    void Update()
    { 
        if (isAnimating) // The animation is playing
        {
            timeCurrent += Time.deltaTime; // move playhead forward

            float p = timeCurrent / tweenLength;

            Vector3 finalPos = AnimMath.Lerp(previousPlantedPos, plantedPos, p);

            finalPos.y += verticalMovement.Evaluate(p);

            transform.position = finalPos;

            transform.rotation = AnimMath.Lerp(previousPlantedRotation, plantedRot, p);

            Vector3 vFromCenter = transform.position - transform.parent.position;
            vFromCenter.y = 0;
            vFromCenter.Normalize();
            vFromCenter *= 3;
            vFromCenter.y += 2.5f;
            vFromCenter += transform.position;

            kneePole.position = vFromCenter;

        }

        else // Animation is not playing
        {
            transform.position = plantedPos;
            transform.rotation = plantedRot;
        }
    }

    public bool TryToStep()
    {
        if (isAnimating) return false;

        if (footHasMoved) return false;

        Vector3 vBetween = transform.position - stepTarget.position;
        float threshold = 2;

        if (vBetween.sqrMagnitude < threshold * threshold) return false;

        // Cast a ray from a meter above it's position, down to the ground
        Ray ray = new Ray(stepTarget.position + Vector3.up, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 3);

        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            // Setup beginning of tween
            previousPlantedPos = transform.position;
            previousPlantedRotation = transform.rotation;

            transform.localRotation = startingRot;

            // Setup end of tween
            plantedPos = hit.point;
            plantedRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // Begin Animation
            timeCurrent = 0;

            footHasMoved = true;

            return true;
        }
        return false;
    }
}
