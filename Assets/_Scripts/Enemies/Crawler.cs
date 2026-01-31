using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler : BaseEnemy
{
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] Transform Target;

    private void Update()
    {
        Agent.destination = Target.position;
    }
}
