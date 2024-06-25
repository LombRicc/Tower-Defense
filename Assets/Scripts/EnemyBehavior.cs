using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehavior : MonoBehaviour
{
    public enum EnemyType
    {
        Soldier,
        Bird,
        Knight,
        Dragon
    }

    public EnemyType enemyType;

    public Transform target;
    public int health;
    public float speed;
    public int gold;
    public List<EnemyInRange> inRangeOf;
    private bool updatedUI;

    // Start is called before the first frame update
    protected void Start()
    {
        target = GameObject.FindWithTag("Target").transform;
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        if(health <= 0) {
            GameManager.instance.gold += gold;
            foreach (EnemyInRange range in inRangeOf)
            {
                range.RemoveEnemy(gameObject);
            }
            ReduceEnemyUI();
            Destroy(gameObject);
        }
    }

    public void ReduceEnemyUI()
    {
        if(!updatedUI)
        {
            switch (enemyType)
            {
                case EnemyType.Soldier:
                    GameManager.instance.soldierNumber -= 1;
                    break;
                case EnemyType.Bird:
                    GameManager.instance.birdNumber -= 1;
                    break;
                case EnemyType.Knight:
                    GameManager.instance.knightNumber -= 1;
                    break;
                case EnemyType.Dragon:
                    GameManager.instance.dragonNumber -= 1;
                    break;
            }
            updatedUI = true;
        }
    }
}
