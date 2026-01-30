using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGun : MonoBehaviour
{
    [SerializeField] CameraMovement CameraScript;
    [SerializeField] CarMovement Movement;
    [SerializeField] Transform CamTrans;
    [SerializeField] float MaxRange = 20f, RotSpeed = 20f, MinAngle = -5f, MaxAngle = 20f, Force = 200f, Damage = 50f;

    Vector3 Aimpoint = Vector3.zero;

    private void OnDrawGizmos()
    {
        if(Aimpoint != null && Aimpoint != Vector3.zero)
        {
            Gizmos.DrawSphere(Aimpoint, 0.5f);
        }
    }
    private void Update()
    {
        if(CameraScript.GunCanFire) //gun camera is active
        {
            Physics.Raycast(CamTrans.position, CamTrans.forward, out RaycastHit hitCamera, MaxRange);
            Debug.DrawRay(CamTrans.position, CamTrans.forward * MaxRange);
            if (hitCamera.collider != null)
            {
                Aimpoint = hitCamera.point;
            }
            else
            {
                Aimpoint = CamTrans.position + CamTrans.forward * MaxRange;
            }

            //rotate gun towards aimpoint
            Vector3 dir = (Aimpoint - transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            Vector3 targetEuler = targetRot.eulerAngles;
            float xRot = targetEuler.x;
            if (xRot > 180f)
            {
                xRot -= 360f;
            }
            xRot = Mathf.Clamp(xRot, MinAngle, MaxAngle);
            float zRot = Movement.Rb.rotation.eulerAngles.z;
            Quaternion clampedRot = Quaternion.Euler(xRot, targetEuler.y, zRot);
            transform.rotation = Quaternion.Lerp(transform.rotation, clampedRot, RotSpeed * Time.deltaTime);

            if(Input.GetMouseButtonDown(0))
            {
                //fire
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hitGun);
                if(hitGun.collider != null)
                {
                    //throw shit
                    if(hitGun.collider.transform.TryGetComponent(out BaseEnemy enemy))
                    {
                        enemy.Damage(Damage);
                    }

                    if(hitGun.collider.transform.TryGetComponent(out Rigidbody hitRb))
                    {
                        Vector3 force = transform.forward * Force;
                        hitRb.AddForce(force, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
