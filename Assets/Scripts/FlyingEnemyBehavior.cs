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
        transform.LookAt(target.position);
        transform.Rotate(new Vector3(45f, 0f, 0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        body.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
