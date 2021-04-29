using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurHandAnimator : MonoBehaviour
{
    public Transform handTarget;

    private void Start()
    {
        transform.position = handTarget.position;
    }

    private void Update()
    {
        
    }
}
