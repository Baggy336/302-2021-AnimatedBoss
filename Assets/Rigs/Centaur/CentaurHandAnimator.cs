using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurHandAnimator : MonoBehaviour
{
    public Transform handTarget;

    public Transform player;

    private Vector3 startingPos;

    private void Start()
    {
        transform.position = handTarget.position;
        startingPos = transform.position;
    }

    private void Update()
    {
        
    }

    public void SwingAtPlayer()
    {
        Vector3 vToPlayer = player.position - transform.position;

        handTarget.position = AnimMath.Slide(handTarget.position, player.position, .001f);

        transform.position = AnimMath.Slide(transform.position, handTarget.position, .001f);
    }
}
