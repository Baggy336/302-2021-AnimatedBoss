using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurController : MonoBehaviour
{
    public List<CentaurStickyFeet> feet = new List<CentaurStickyFeet>();

    private CharacterController pawn;

    private void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveCent();

        int feetStepping = 0;

        foreach (CentaurStickyFeet foot in feet)
        {
            if (foot.isFootAnimating) feetStepping++;
        }

        foreach (CentaurStickyFeet foot in feet)
        {
            if (feetStepping < 2)
            {
                foot.DoAStep();
            }
        }
    }

    void MoveCent()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 velocity = transform.forward * v;

        pawn.SimpleMove(velocity * 3);

        transform.Rotate(0, h * 90 * Time.deltaTime, 0);
    }
}
