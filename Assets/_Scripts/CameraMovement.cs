using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //freecam
    [SerializeField] Transform CarTrans;
    [SerializeField] CarMovement Movement;
    [SerializeField] float OrbitRange = 5f, Angle = -90, VertOffset = 5f, BaseFOV = 60f, MaxFOV = 80f;

    Camera Cam;

    private void Start()
    {
        Cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //float ProgressFOV = Mathf.Abs(Movement.Rb.velocity.magnitude / Movement.MaxVel);
        //Cam.fieldOfView = Mathf.Lerp(BaseFOV, MaxFOV, ProgressFOV);

        Angle -= Input.GetAxisRaw("Mouse X");
        VertOffset -= Input.GetAxisRaw("Mouse Y");
        VertOffset = Mathf.Clamp(VertOffset, 0f, 18f);

        float orbitX = Mathf.Cos(Angle * Mathf.Deg2Rad);
        float orbitZ = Mathf.Sin(Angle * Mathf.Deg2Rad);

        Vector3 orbit = OrbitRange * new Vector3(orbitX, 0f, orbitZ);
        transform.position = CarTrans.position + orbit + new Vector3(0f, VertOffset, 0f);
        transform.LookAt(CarTrans.position);
    }
}
