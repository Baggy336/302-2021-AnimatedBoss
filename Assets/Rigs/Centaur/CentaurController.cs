using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurController : MonoBehaviour
{
    private CharacterController pawn;

    public Transform groundTargets;

    public float moveSpeed = 3;

    public float stepAnimTime = 4;

    public Vector3 velocity { get; private set; }

    public Vector3 stepScale = Vector3.one;

    private void Start()
    {
        groundTargets.localPosition = new Vector3(0, -.5f, 0);

        pawn = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveCent();
    }

    void MoveCent()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        velocity = transform.forward * v;

        pawn.SimpleMove(velocity * moveSpeed);

        transform.Rotate(0, h * 90 * Time.deltaTime, 0);

        Vector3 localVelocity = groundTargets.InverseTransformDirection(velocity);

        groundTargets.localPosition = AnimMath.Slide(groundTargets.localPosition, localVelocity, .001f);
    }
}
