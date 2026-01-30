using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretStates { Search, Attack }

public class Turret : BaseEnemy
{
    [SerializeField] Transform Target, TPivot, TMuzzle;

    [SerializeField] float DangerRange = 30f, MaxAngle = 80f, RotSpeed = 5f;
    [SerializeField] bool CanAttack = false;

    private void Start()
    {
        //IsInvincible = true;
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, Target.position);
        Vector3 dir = (Target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);
        Debug.DrawRay(transform.position, dir * DangerRange);
        Physics.Raycast(TMuzzle.position, dir, out RaycastHit visHit, DangerRange);

        CanAttack = dist <= DangerRange && angle <= MaxAngle / 2f && visHit.collider != null && visHit.collider.transform == Target;

        if(CanAttack)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            TPivot.rotation = Quaternion.Lerp(TPivot.rotation, targetRot, RotSpeed * Time.deltaTime);
            Physics.Raycast(TMuzzle.position, TMuzzle.forward, out RaycastHit atkHit);
            if(atkHit.collider != null && atkHit.collider.transform == Target)
            {
                Debug.Log("Ray hitting Target");
            }
        }
    }

        //raycast on player, if it hits that means theyre visible, then rotate, then launch another, if that hits then KILL!!!!!
        //get angle y and angle x towards player transform, rotate that angle and if raycast hits then engage, if inside range

    
}
