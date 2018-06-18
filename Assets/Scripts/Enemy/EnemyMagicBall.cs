using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicBall : MonoBehaviour
{
    public Stats damage;
    Rigidbody rb;
    public float BallSpeed = 5f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(0, 0, BallSpeed, ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }

 
}
