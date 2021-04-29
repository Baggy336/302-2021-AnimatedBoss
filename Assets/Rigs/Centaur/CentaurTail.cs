using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurTail : MonoBehaviour
{
    public Transform tailTarget;

    private void Start()
    {
        transform.position = tailTarget.position;
    }
}
