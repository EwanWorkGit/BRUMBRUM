using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float MaxVel = 20f;

    [SerializeField] Rigidbody Rb;
    [SerializeField] Transform[] Wheels;
    [SerializeField] Transform[] Suspentions;
    [SerializeField] Transform COM;
    [SerializeField] float[] CurrentAngles;
    [SerializeField] float EngineForce = 20f, GripFactor = 0.8f, DesiredOffset = 1f, SuspStrength, DamperStrength, RotSpeed = 5f, WheelMass = 1f, MaxTurnAngle = 40f;

    private void Start()
    {
        Physics.gravity = new Vector3(0f, -19.82f, 0f);
        Rb.centerOfMass = COM.position;
    }

    private void FixedUpdate()
    {
        Vector2 inputs = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        for(int i = 0; i < Wheels.Length; i++)
        {
            Physics.Raycast(Wheels[i].position, -Suspentions[i].up, out RaycastHit hit, DesiredOffset * 1.5f);
            if(hit.collider != null)
            {
                //suspention force
                float offset = DesiredOffset - hit.distance;
                float maxCompression = 0.5f * DesiredOffset;
                float softOffset = Mathf.Clamp(offset, -maxCompression, maxCompression);

                float spring = softOffset * SuspStrength;
                float damper = Vector3.Dot(Suspentions[i].up, Rb.GetPointVelocity(hit.point)) * DamperStrength;

                float suspForce = spring - damper;
                Vector3 springForce = Wheels[i].up * suspForce;

                Rb.AddForceAtPosition(springForce, hit.point);
                
                //engine force
                Vector3 engineForce = Wheels[i].forward * inputs.y * EngineForce;
                Rb.AddForceAtPosition(engineForce, hit.point);

                //side force
                Vector3 steerDir = Wheels[i].right;
                Vector3 wheelVel = Rb.GetPointVelocity(Wheels[i].position);
                float steeringVel = Vector3.Dot(steerDir, wheelVel);
                float desiredVelChange = -steeringVel * GripFactor;
                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                Vector3 steeringForce = steerDir * desiredAccel * WheelMass;
                Rb.AddForceAtPosition(steeringForce, hit.point);

                if(i < 2)
                {
                    Vector3 localEuler = Wheels[i].localEulerAngles;

                    float turnFactor = 1f; //cur velocity / max velocity
                    float targetAngle = MaxTurnAngle * inputs.x * turnFactor;
                    CurrentAngles[i] = Mathf.Lerp(CurrentAngles[i], targetAngle, Time.fixedDeltaTime * RotSpeed);
                    localEuler.y = CurrentAngles[i]; // assuming Y axis rotates wheel left/right
                    Wheels[i].localEulerAngles = localEuler;
                }
                
            }
        }
        
    }
}
