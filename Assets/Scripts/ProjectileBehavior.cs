using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed;
    public int damage;

    public Transform target; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehavior>().GetDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
