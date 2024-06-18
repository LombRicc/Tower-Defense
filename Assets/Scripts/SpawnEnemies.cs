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
        yield return new WaitUntil(() => GenerateGrid.instance.created);
        while(true)
        {
            yield return new WaitForSeconds(GameManager.instance.spawnTimer);
            Instantiate(GameManager.instance.enemy, this.transform.position, Quaternion.identity);
        }
    }
}
