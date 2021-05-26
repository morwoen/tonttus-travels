using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMonsterBehavior : MonoBehaviour
{

    public float distanceToHunt = 10f;
    Transform target;
    public Transform[] points;
    private int destPoint = 0;
    public NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.transform;


        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {

        if (points.Length == 0)
            return;


        agent.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();

        if (distance <= distanceToHunt)
        {
            agent.SetDestination(target.position);
        }
    }
}
