using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("Checks if the gun camera is active")]
    public bool GunCanFire;

    [SerializeField] Transform[] Transforms;
    [SerializeField] CarMovement Movement;
    [SerializeField] float OrbitRange = 5f, PosYOffset = 2f, BaseFOV = 60f, MaxFOV = 80f, XSens = 1f, YSens = 1f;

    Transform CurrentTrans;
    Camera Cam;

    float VertOffset, Angle = -90;
    int Index;

    private void Start()
    {
        Cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CurrentTrans = Transforms[Index];
    }

    private void Update()
    {
        CurrentTrans = Transforms[Index];
        GunCanFire = CurrentTrans.CompareTag("Gun") ? true : false;

        float ProgressFOV = Mathf.Abs(Movement.Rb.velocity.magnitude / Movement.MaxTurnVel);
        Cam.fieldOfView = Mathf.Lerp(BaseFOV, MaxFOV, ProgressFOV);

        Angle -= Input.GetAxisRaw("Mouse X") * XSens;
        VertOffset -= Input.GetAxisRaw("Mouse Y") * YSens;
        VertOffset = Mathf.Clamp(VertOffset, -5f, 18f);

        float orbitX = Mathf.Cos(Angle * Mathf.Deg2Rad);
        float orbitZ = Mathf.Sin(Angle * Mathf.Deg2Rad);

        Vector3 orbit = OrbitRange * new Vector3(orbitX, 0f, orbitZ);
        transform.position = CurrentTrans.position + orbit + new Vector3(0f, VertOffset, 0f);
        transform.LookAt(CurrentTrans.position + new Vector3(0f, PosYOffset, 0f));

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0.01f)
        {
            if(Index + 1 < Transforms.Length)
            {
                Index++;
            }
            else
            {
                Index = 0;
            }
                
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < -0.01f)
        {
            if (Index - 1 >= 0)
            {
                Index--;
            }
            else
            {
                Index = Transforms.Length - 1;
            }
        }
    }
}
