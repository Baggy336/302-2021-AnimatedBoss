using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurController : MonoBehaviour
{
    public enum States
    {
        Idle,
        Walking,
        Attack,
        Death
    }

    private CharacterController pawn;

    public float moveSpeed = 3;

    public float stepAnimTime = 4;

    public Vector3 velocity { get; private set; }

    public Vector3 stepScale = Vector3.one;

    public States state { get; private set; }

    private void Start()
    {
        state = States.Idle;

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

        state = (velocity.sqrMagnitude > .1f) ? States.Walking : States.Idle;
    }
}
