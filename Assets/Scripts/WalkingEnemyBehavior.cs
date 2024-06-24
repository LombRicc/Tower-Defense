using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingEnemyBehavior : EnemyBehavior
{
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            agent.destination = target.position;
    }
}
