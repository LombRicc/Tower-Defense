using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBehavior : EnemyBehavior
{
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target.position);
        }
    }

    private void FixedUpdate()
    {
        body.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
