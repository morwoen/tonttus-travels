using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField]
    float maxSeeDistance = 0f;

    public LayerMask playerMask;
    MonsterBehavior mb;

    private void Start() {
        mb = GetComponent<MonsterBehavior>();
    }

    private void FixedUpdate() {
        //Collider[] collisions = Physics.OverlapSphere(transform.position, mb.distanceToHunt, playerMask);

        //foreach (Collider collider in collisions) {
        //    if (collider.CompareTag("Player")) {
        //        mb.SetTarget(collider.gameObject);
        //        break;
        //    }
        //}

        
    }
}
