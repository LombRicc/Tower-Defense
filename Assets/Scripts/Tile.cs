using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X;
    public int Y;
    public Tile[] neighbors;
    public List<Tile> intersects;
    public Tile mySpawn;
    public Tile randomNeighbor;
    public bool roadOn;

    void Start()
    {
        StartCoroutine(CreateFences());    
    }

    private IEnumerator CreateFences()
    {
        yield return new WaitUntil(() => GenerateGrid.instance.created);
        if (roadOn)
        {
            foreach (Tile tile in neighbors)
            {
                if(tile != null && !intersects.Contains(tile) && tile != randomNeighbor)
                {
                    var fencePosition = new Vector3((tile.transform.position.x - this.transform.position.x) / 2, 0f, (tile.transform.position.z - this.transform.position.z) / 2) + Vector3.up * 1.25f;
                    var newFence = Instantiate(GenerateGrid.instance.fence, this.transform.position + fencePosition, Mathf.Abs(this.transform.position.x - tile.transform.position.x)==0 ? Quaternion.Euler(new Vector3(0, 90, 0)) : Quaternion.identity);
                    newFence.transform.parent = this.transform;
                }
            }
        }
    }
}
