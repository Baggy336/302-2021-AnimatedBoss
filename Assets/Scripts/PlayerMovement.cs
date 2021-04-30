using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;

    public Transform leftLeg;

    public Transform rightLeg;

    private Vector3 velocity;

    public float moveSpeed = 3;

    private CharacterController pawn;

    private void Start()
    {
        cam = Camera.main;
        pawn = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Walk();
        AnimateLegs();
    }
    void Walk()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isMoving = (h != 0 || v != 0);
        if (isMoving)
        {
            float yaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, yaw, 0), .001f);
        }

        velocity = transform.forward * v + transform.right * h;

        if (velocity.sqrMagnitude > 1) velocity.Normalize();

        pawn.SimpleMove(velocity * moveSpeed);
    }

    void AnimateLegs()
    {
        float degrees = 45;

        float speed = 10;

        // Find the local vector of the input direction
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        Vector3 axis = Vector3.Cross(localVelocity, Vector3.up);

        // align to the forward vector
        float alignment = Vector3.Dot(localVelocity, Vector3.forward);
        // Check to see if the alignment is positive or negative
        alignment = Mathf.Abs(alignment);

        degrees *= AnimMath.Lerp(0.25f, 1, alignment);

        // Rotate legs 
        float wiggle = Mathf.Sin(Time.time * speed) * degrees;

        leftLeg.localRotation = AnimMath.Slide(leftLeg.localRotation, Quaternion.AngleAxis(wiggle, axis), .001f);
        rightLeg.localRotation = AnimMath.Slide(rightLeg.localRotation, Quaternion.AngleAxis(-wiggle, axis), .001f);
    }
}
