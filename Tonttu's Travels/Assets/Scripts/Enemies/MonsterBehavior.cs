using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{

  public GameObject player;
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
    float distance = Vector3.Distance(player.transform.position, transform.position);

    if (distance <= distanceToHunt)
    {
      animator.SetBool("Alert", true);
      agent.SetDestination(player.transform.position);
      isChasing = true;
    }

    if (isChasing && distance > distanceToHunt)
    {
      animator.SetBool("Alert", false);
      patrol.GotoNextPoint();
    }

    if (distance <= distanceToKill)
    {
      player.GetComponent<PlayerCheckpoint>().LoadCheckpoint();
    }
  }
}
