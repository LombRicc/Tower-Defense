using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTower : MonoBehaviour
{
    Color damageColor = Color.red;
    private bool invincibilityFrames;

    // Start is called before the first frame update
    void Start()
    {
        damageColor.a = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null && collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehavior>().ReduceEnemyUI();
            foreach (EnemyInRange range in collision.gameObject.GetComponent<EnemyBehavior>().inRangeOf)
            {
                range.RemoveEnemy(gameObject);
            }
            Destroy(collision.gameObject);
            if(!invincibilityFrames)
                StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        invincibilityFrames = true;

        GameManager.instance.TakeDamage();
        if(GameManager.instance.health <= 0)
        {
            Destroy(gameObject);
        }

        var stdColor = this.GetComponent<Renderer>().material.color;
        for (int i=0; i<5; i++)
        {
            this.GetComponent<Renderer>().material.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            this.GetComponent<Renderer>().material.color = stdColor;
            yield return new WaitForSeconds(0.2f);
        }

        invincibilityFrames = false;
    }
}
