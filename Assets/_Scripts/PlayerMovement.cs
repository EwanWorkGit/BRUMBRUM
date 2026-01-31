using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera Cam;
    [SerializeField] CharacterController Ch;

    [SerializeField] float MoveSpeed = 10f;

    Vector3 Movement;

    private void Update()
    {
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector3 camForw = Cam.transform.forward;
        camForw.y = 0f;
        camForw.Normalize();
        Vector3 camRight = Cam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Movement = camForw * inputs.y + camRight * inputs.x;

        Ch.Move(Movement * MoveSpeed * Time.deltaTime);
    }
}
