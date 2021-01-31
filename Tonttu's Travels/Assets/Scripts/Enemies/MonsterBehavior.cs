using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{

  public GameObject target;
  public float distanceToKill = 2f;
  public float distanceToHunt = 10f;
  public float patrolSpeed = 5.0f;
  public float chaseSpeed = 10.0f;
  public float checkpointDistance = 0.1f;
  public float turnTime = 10f;

  private Rigidbody rb;
  private Animator animator;
  private NavMeshAgent agent;
  private PatrolScript patrol;
  private float turnSmoothVelocity;

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponentInChildren<Animator>();
    patrol = GetComponent<PatrolScript>();
    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    if (target)
    {
      float distance = Vector3.Distance(target.transform.position, transform.position);

      if (distance < checkpointDistance)
      {
        SetTarget(patrol.GetNextPoint());
      }

      Vector3 moveDirection = target.transform.position - transform.position;
      float speed = target.CompareTag("Player") ? chaseSpeed : patrolSpeed;
      transform.position = transform.position + moveDirection.normalized * speed * Time.fixedDeltaTime;

      Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnTime * Time.fixedDeltaTime);

      if (target.CompareTag("Player"))
      {
        RaycastHit hit;

        bool isTargetFound = Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, distanceToHunt);

        if (!isTargetFound || !hit.collider.gameObject.CompareTag("Player"))
        {
          SetTarget(patrol.GetNextPoint());
          return;
        }

        if (distance <= distanceToKill)
        {
          target.GetComponent<PlayerCheckpoint>().LoadCheckpoint();
          SetTarget(patrol.GetNextPoint());
          animator.SetBool("Alert", false);
        }
      }
    }
    else
    {
      SetTarget(patrol.GetNextPoint());
    }
  }

  public void SetTarget(GameObject player)
  {
    target = player;
  }

  public bool HasTarget()
  {
    return !!target;
  }

  void OnTriggerStay(Collider collider)
  {
    if (collider.CompareTag("Player"))
    {
      RaycastHit hit;

      bool isTargetFound = Physics.Raycast(transform.position, collider.transform.position - transform.position, out hit, distanceToHunt);
      Debug.DrawRay(transform.position, collider.transform.position - transform.position, Color.blue);

      if (!isTargetFound || (isTargetFound && !hit.collider.gameObject.CompareTag("Player")))
      {
        SetTarget(patrol.GetNextPoint());
        return;
      }

      SetTarget(collider.gameObject);
      animator.SetBool("Alert", true);
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.CompareTag("Player"))
    {
      SetTarget(patrol.GetNextPoint());
      animator.SetBool("Alert", false);
    }
  }
}
