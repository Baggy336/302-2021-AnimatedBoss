using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurController : MonoBehaviour
{
    static class States
    {
        public class State
        {
            protected CentaurController centaur;

            virtual public State Update()
            {
                return null;
            }
            virtual public void OnStart(CentaurController centaur)
            {
                this.centaur = centaur;
            }
            virtual public void OnEnd()
            {

            }

            //////////////////// Children of the State class
            
            public class Idle : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transition:
                    if (Vector3.Angle(centaur.transform.forward, centaur.velocity) < centaur.visCone) centaur.SwitchState(new State.Walk());
                    return null;
                }
            }

            public class Walk : State
            {
                public override State Update()
                {
                    // Behavior:
                    centaur.MoveCent();
                    centaur.TurnToPlayer();
                    // Transition:
                    if (Vector3.Angle(centaur.transform.forward, centaur.velocity) > centaur.visCone) centaur.SwitchState(new State.Idle());
                    if (centaur.velocity.sqrMagnitude < centaur.attackDis * centaur.attackDis) centaur.SwitchState(new State.Attack());
                    return null;
                }
            }

            public class Attack : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transition:
                    return null;
                }
            }
            public class TakeDamage : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transition:
                    return null;
                }
            }
            public class Dead : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transition:
                    return null;
                }
            }
        }
    }

    // Reference the list of states
    private States.State state;

    private CharacterController pawn;

    private HealthSystem health;

    public Transform player;

    public List<CentaurStickyFeet> feet = new List<CentaurStickyFeet>();

    public Transform groundTargets;

    public float moveSpeed = 3;

    public float visCone = 120;

    private float attackDis = 5;

    public Vector3 velocity { get; private set; }

    public Vector3 stepScale = Vector3.one;

    private void Start()
    {
        pawn = GetComponent<CharacterController>();
        health = GetComponent<HealthSystem>();
    }

    private void Update()
    {
        if (state == null) SwitchState(new States.State.Idle());

        if (state != null) SwitchState(state.Update());

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

    void SwitchState(States.State newState)
    {
        if (newState == null) return;

        if (state != null) state.OnEnd();

        state = newState;

        state.OnStart(this);
    }

    void MoveCent()
    {
        Vector3 vToPlayer = player.position - transform.position;

        // add to the forward direction based on vertical input
        velocity = vToPlayer;

        // move the object based on the input and speed
        pawn.SimpleMove(velocity * moveSpeed);

        // convert velocity to local space velocity
        Vector3 localVelocity = groundTargets.InverseTransformDirection(velocity);

        localVelocity.y = 0;

        // Move the ground ring forward based on local velocity
        groundTargets.localPosition = AnimMath.Slide(groundTargets.localPosition, localVelocity * moveSpeed, .001f);
    }

    void TurnToPlayer()
    {
        if (!player) return;

        Vector3 vToPlayer = player.position - transform.position;

        Quaternion rotationToPlayer = Quaternion.LookRotation(vToPlayer, Vector3.up);

        Vector3 euler1 = transform.localEulerAngles;
        Quaternion previousRot = transform.rotation;
        transform.rotation = rotationToPlayer;
        Vector3 euler2 = transform.localEulerAngles;
        euler2.x = 0;


        transform.rotation = previousRot;
        transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), .001f);
    }
}
