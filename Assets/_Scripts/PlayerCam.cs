using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] Camera Cam;

    [SerializeField] float Sens = 1f;

    float XRot, YRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * Sens;

        XRot -= inputs.y;
        YRot += inputs.x;

        XRot = Mathf.Clamp(XRot, -89f, 89f);

        transform.rotation = Quaternion.Euler(0f, YRot, 0f);
        Cam.transform.localRotation = Quaternion.Euler(XRot, 0f, 0f);
    }
}
