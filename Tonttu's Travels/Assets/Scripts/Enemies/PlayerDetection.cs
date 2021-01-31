using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField]
    float chaseDetectRange = 0f;
    [SerializeField]
    float maxSeeDistance = 0f;

    public LayerMask playerMask;
    MonsterBehavior mb;

    private void Start() {
        mb = GetComponent<MonsterBehavior>();
    }

    private void FixedUpdate() {
        Collider[] collisions = Physics.OverlapSphere(transform.position, chaseDetectRange, playerMask);

        foreach (Collider collider in collisions) {
            if (collider.CompareTag("Player")) {
                mb.SetTarget(collider.gameObject);
                break;
            }
        }

        if (mb.HasTarget()) {
            RaycastHit hit;

            bool isTargetFound = Physics.Raycast(transform.position, mb.target.transform.position - transform.position, out hit, maxSeeDistance);

            if (!isTargetFound || !hit.collider.gameObject.CompareTag("Player")) {
                mb.SetTarget(null);
            }
        }
    }
}
