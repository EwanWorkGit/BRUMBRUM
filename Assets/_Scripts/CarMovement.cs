using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float MaxVel = 20f;
    public Rigidbody Rb;

    [SerializeField] Transform GroundChecker;
    [SerializeField] float EngineForce = 20f, RotSpeed = 5f, GripForce = 20f, DesiredOffset = 1f, SuspStrength, DamperStrength;
    [SerializeField] bool IsGrounded = true, IsMoving;

    private void Start()
    {
        Physics.gravity = new Vector3(0f, -19.82f, 0f);
    }

    private void Update()
    {
        Vector2 inputs = new(Input.GetAxis("Horizontal"), IsGrounded ? Input.GetAxis("Vertical") : 0);

        Physics.Raycast(GroundChecker.position, -transform.up, out RaycastHit hit, 2f);
        IsGrounded = hit.collider != null;

        IsMoving = Mathf.Abs(Rb.velocity.magnitude) > 0.05f;

        if(IsMoving)
        {
            float progressTurn = Rb.velocity.magnitude / MaxVel; //progress for speedup at low speeds

            Quaternion rot = Rb.rotation;
            rot *= Quaternion.Euler(0f, inputs.x * RotSpeed * Mathf.Lerp(1.5f, 0.9f, progressTurn) * Time.deltaTime, 0f);
            Rb.MoveRotation(rot);
        }

        if (!IsGrounded)
        {
            //DO IT WITH ADDTORQUE
            float rotDelta = 0.3f * Time.deltaTime;
            Rb.MoveRotation(Quaternion.Slerp(Rb.rotation, Quaternion.Euler(15f, Rb.rotation.eulerAngles.y, 0f), rotDelta));
        }

        Vector3 sideForce = Vector3.zero;

        if (IsMoving && IsGrounded)
        {
            Vector3 sideDir = Vector3.Project(Rb.velocity, transform.right); //points outwards from body
            sideForce = -sideDir * GripForce;
        }

        Vector3 springForce = Vector3.zero;

        //suspention
        if(IsGrounded)
        {
            float offset = DesiredOffset - hit.distance; //up = +, down = -
            float suspForce = (offset * SuspStrength) - (Vector3.Dot(transform.up, Rb.velocity) * DamperStrength);
            springForce = suspForce * transform.up;
            Debug.Log(springForce);
        }

        Vector3 engineForce = transform.forward * inputs.y * EngineForce;

        Rb.AddForce(engineForce + sideForce + springForce, ForceMode.Force);
        //CLAMP XZ
    }
}
