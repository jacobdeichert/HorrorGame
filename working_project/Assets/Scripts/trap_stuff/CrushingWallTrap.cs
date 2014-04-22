using UnityEngine;
using System.Collections;

public class CrushingWallTrap : Trap
{

    private Vector3 force = Vector3.zero;
    private Vector3 accel = Vector3.zero;
    private float mass = 1f;
    private float restLengthOfString = 1f;
    private float springStiffnessConstant = 29f;
    private Vector3 deltaPosition = Vector3.zero;
    public Transform fixedObject;

    void Start()
    {
        activate();
    }


    void FixedUpdate()
    {
        // if the trap has been triggered, apply the spring translation
        if (state == TrapState.BEGUN)
        {
            force.z = 100f;
            force += getSimpleSpringForce();
            accel = force / mass;
            deltaPosition = deltaPosition + ((accel / 2) * Mathf.Pow(Time.deltaTime, 2));
            transform.Translate(deltaPosition);
        }
    }


    public override void correctRotation()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        bool hasHitSomething = Physics.Raycast(ray, out hit, 2.0f);
        int testCount = 4;

        while (hasHitSomething)
        {
            gameObject.transform.parent.gameObject.transform.Rotate(0, 90, 0);
            ray = new Ray(transform.position, transform.forward);
            hasHitSomething = Physics.Raycast(ray, out hit, 2.0f);

            // stop the loop after 4 tries (trap is blocked on all sides)
            testCount--;
            if (testCount <= 0) break;
        }

        // check if there is a floor trap in front of the wall trap
        ray = new Ray(transform.position + transform.up * -3, transform.forward);
        if (Physics.Raycast(ray, out hit, 1.0f))
        {
            if (hit.collider.transform.parent
                && (hit.collider.transform.parent.GetComponentInChildren<SpikeFloorTrap>()) as SpikeFloorTrap)
            {
                // remove the floor trap trigger so that it doesn't interfere with the wall trap trigger
                // the floor trap will never be activated
                Destroy(hit.collider.transform.parent.GetChild(2).gameObject);
            }
        }
    }


    public override void begin()
    {
        if (state == TrapState.IDLE && isActivated)
        {
            base.begin();
        }
    }


    public override void end()
    {
        if (state == TrapState.BEGUN)
        {
            base.end();
        }
    }


    private Vector3 getSimpleSpringForce()
    {
        // use x in place of z because the inner object is rotated 90deg and that never changes
        Vector3 displacement = new Vector3(0, 0, transform.localPosition.x - fixedObject.transform.localPosition.x);
        Vector3 normalized = displacement.normalized;
        normalized *= restLengthOfString;

        Vector3 x = displacement - normalized;

        // -k * x
        return -springStiffnessConstant * x;
    }
}
