using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControlTower : MonoBehaviour
{
    [SerializeField] private GameObject controlTower;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitUntil(() => GenerateGrid.instance.created);
        Instantiate(controlTower, this.transform.position + Vector3.up*3, Quaternion.identity);
    }
}
