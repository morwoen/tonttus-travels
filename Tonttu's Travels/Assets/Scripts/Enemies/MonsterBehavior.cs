using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{

  public GameObject target;
  public float distanceToKill = 2f;
  public float distanceToHunt = 10f;

  private Animator animator;
  private NavMeshAgent agent;
  private PatrolScript patrol;
  private bool isChasing = false;

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponentInChildren<Animator>();
    patrol = GetComponent<PatrolScript>();
  }

  void Update()
  {
    if (target) {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance <= distanceToHunt)
        {
          animator.SetBool("Alert", true);
          agent.SetDestination(target.transform.position);
          isChasing = true;
        }

        if (isChasing && distance > distanceToHunt)
        {
          animator.SetBool("Alert", false);
          patrol.GotoNextPoint();
          SetTarget(null);
        }

        if (distance <= distanceToKill)
        {
            target.GetComponent<PlayerCheckpoint>().LoadCheckpoint();
        }
    }
  }

  public void SetTarget(GameObject player) {
     target = player;
  }

  public bool HasTarget() {
    return !!target;
  }
}
