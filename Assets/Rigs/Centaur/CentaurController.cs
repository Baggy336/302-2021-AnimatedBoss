using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurController : MonoBehaviour
{
    private CharacterController pawn;

    public List<CentaurStickyFeet> feet = new List<CentaurStickyFeet>();

    public Transform groundTargets;

    public float moveSpeed = 3;

    public Vector3 velocity { get; private set; }

    public Vector3 stepScale = Vector3.one;

    private void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveCent();

        int currentFeetStepping = 0;
        int currentFeetMoved = 0;

        foreach (CentaurStickyFeet foot in feet)
        {
            if (foot.isFootAnimating) currentFeetStepping++;
            if (foot.footHasMoved) currentFeetMoved++;
        }

        if (currentFeetMoved >= 4)
        {
            foreach (CentaurStickyFeet foot in feet)
            {
                foot.footHasMoved = false;
            }
        }

        foreach (CentaurStickyFeet foot in feet)
        {
            if (currentFeetStepping < 2)
            {
                if (foot.DoAStep())
                    currentFeetStepping++;
            }
        }
    }

    void MoveCent()
    {
        // Get vertical and horizontal input
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // add to the forward direction based on vertical input
        velocity = transform.forward * v;

        // move the object based on the input and speed
        pawn.SimpleMove(velocity * moveSpeed);

        // Rotate left and right based on a and d input
        transform.Rotate(0, h * 90 * Time.deltaTime, 0);

        // convert velocity to local space velocity
        Vector3 localVelocity = groundTargets.InverseTransformDirection(velocity);

        // Move the ground ring forward based on local velocity
        groundTargets.localPosition = AnimMath.Slide(groundTargets.localPosition, localVelocity, .001f);
    }
}
