using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTestContoller : MonoBehaviour
{
    public Rigidbody cube;
    public float force;

    void Start()
    {
        cube.AddForce(new Vector3(0.0f, 0.0f, force), ForceMode.Impulse);
    }
}
