using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurFeet : MonoBehaviour
{
    public Transform footTarget;

    public AnimationCurve stepHeight;

    private Vector3 previousPos;

    private Vector3 targetPos;

    public float animLength = 4;

    private float animTimer;

    private float stepDistance = 1.5f;

    private void Start()
    {
        previousPos = footTarget.position;
 
    }

    private void Update()
    {
        Vector3 vToTarget = transform.position - footTarget.position;

        if (vToTarget.sqrMagnitude > stepDistance * stepDistance)
        {
            TakeAStep();
        }
        else
        {
            transform.position = previousPos;
        }
        
    }

    void TakeAStep()
    {
        animTimer += Time.deltaTime;

        float percent = animTimer / animLength;

        Vector3 stepPos = AnimMath.Lerp(previousPos, footTarget.position, percent);

        stepPos.y += stepHeight.Evaluate(percent);

        transform.position = stepPos;
    }

}
