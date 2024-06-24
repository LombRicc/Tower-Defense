using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitUntil(() => GameManager.instance.gridCreated);
        while (true)
        {
            yield return new WaitUntil(() => GameManager.instance.waveStarted);
            while (GameManager.instance.enemies.Count > 0)
            {
                if(GameManager.instance.enemies.Count > 0 && GameManager.instance.waveStarted && GameManager.instance.gridCreated)
                {
                    var randomIdx = Random.Range(0, GameManager.instance.enemies.Count - 1);
                    if (GameManager.instance.enemies[randomIdx] != null)
                    {
                        var enemy = GameManager.instance.enemies[randomIdx];
                        GameManager.instance.enemies.Remove(enemy);
                        enemy.transform.SetPositionAndRotation(new Vector3(transform.position.x, enemy.transform.position.y, transform.position.z), enemy.transform.rotation);
                        enemy.SetActive(true);
                    }
                }
                yield return new WaitForSeconds(GameManager.instance.spawnTimer);
                if(!GameManager.instance.waveStarted || !GameManager.instance.gridCreated)
                {
                    break;
                }
            }
        }
    }
}
