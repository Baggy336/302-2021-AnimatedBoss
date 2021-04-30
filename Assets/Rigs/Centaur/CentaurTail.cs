using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurTail : MonoBehaviour
{
    public Transform tailTarget;

    private CentaurController centaur;

    private float time;

    public float wiggleSpeed = 2;

    private void Start()
    {
        transform.position = tailTarget.position;

        centaur = GetComponentInParent<CentaurController>();
    }
    private void Update()
    {
        time = Time.deltaTime * wiggleSpeed;

        float tailWiggle = Mathf.Sin(time);

        tailTarget.position += centaur.velocity * tailWiggle;
    }
}
