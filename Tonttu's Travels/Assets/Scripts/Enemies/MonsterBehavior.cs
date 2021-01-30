using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{

    public float distanceToHunt = 10f;
    public float distanceToKill = 2f;

    public GameObject player;
    NavMeshAgent agent;
    private PatrolScript patrol;
    public PlayerCheckpoint checkpoint;

     void Start()
    {
       
        agent = GetComponent<NavMeshAgent>();
    }

     void Update()
    {

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= distanceToHunt)
        {
            
            agent.SetDestination(player.transform.position);
            if (distance >= distanceToHunt)
            {
                print("here");

                patrol.GotoNextPoint();
            }
        }


        if (distance <= distanceToKill)
        {
            
            checkpoint.LoadCheckpoint();
        }
        

    }



}
