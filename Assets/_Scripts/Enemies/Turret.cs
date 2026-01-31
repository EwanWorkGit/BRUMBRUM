using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretStates { Search, Attack }

public class Turret : BaseEnemy
{
    [SerializeField] GameObject ProjPref;
    [SerializeField] Transform Target, TPivot, TMuzzle;

    [SerializeField] float DangerRange = 30f, MaxAngle = 80f, RotSpeed = 5f, TimerDur = 3f, AtkTimer, AtkForce = 200f;
    [SerializeField] bool CanAttack = false;

    private void Start()
    {
        //IsInvincible = true;
        AtkTimer = TimerDur;
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
                AtkTimer -= Time.deltaTime;
                if(AtkTimer <= 0)
                {
                    //attack
                    GameObject proj = Instantiate(ProjPref, TMuzzle.position, Quaternion.identity);
                    Rigidbody rb = proj.GetComponent<Rigidbody>();
                    rb.AddForce(TMuzzle.forward * AtkForce, ForceMode.Impulse);
                    AtkTimer = TimerDur;
                }
            }
            else //doesnt see target anymore
            {
                AtkTimer = TimerDur;
            }
        }
    } 
}
