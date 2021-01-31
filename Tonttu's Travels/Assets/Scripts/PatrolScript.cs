using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class PatrolScript : MonoBehaviour
{
  public Transform[] points;
  public NavMeshAgent agent;
  
  private int destPoint = 0;
  private MonsterBehavior mbScript;

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    mbScript = GetComponent<MonsterBehavior>();
  }

  public GameObject GetNextPoint()
  {
    if (points.Length == 0)
      return null;

    float distance = Vector3.Distance(points[destPoint].position, transform.position);

    if (distance < mbScript.checkpointDistance)
    {
      destPoint = (destPoint + 1) % points.Length;
    }

    var destination = points[destPoint].gameObject;
    
    return destination;
  }
}