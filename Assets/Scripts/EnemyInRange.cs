using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInRange : MonoBehaviour
{
    public List<GameObject> enemies;

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            FlyingEnemyBehavior maybeFlying;
            if(GetComponentInParent<TurretBehavior>().type == TurretBehavior.TurretType.Missile)
            {
                if(other.TryGetComponent<FlyingEnemyBehavior>(out maybeFlying))
                {
                    enemies.Add(other.gameObject);
                    other.GetComponent<EnemyBehavior>().inRangeOf.Add(this);
                }
            }
            else
            {
                enemies.Add(other.gameObject);
                other.GetComponent<EnemyBehavior>().inRangeOf.Add(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemies.Contains(other.gameObject))
            {
                enemies.Remove(other.gameObject);
                other.GetComponent<EnemyBehavior>().inRangeOf.Remove(this);
            } 
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
