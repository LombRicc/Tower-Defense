using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    internal Tile[,] tiles;
    internal Tile target;
    internal List<Tile> spawnPoints = new List<Tile>();

    void OnEnable()
    {
        var gm = GameManager.instance;
        tiles = new Tile[gm.tilesXrow, gm.tilesXrow];
        for (int i=0; i<gm.tilesXrow; i++)
        {
            for (int j=0; j<gm.tilesXrow; j++)
            {
                var newTile = Instantiate(gm.tilePrefab, new Vector3((gm.tilePrefab.transform.lossyScale.x+0.5f) * i, transform.position.y, (gm.tilePrefab.transform.lossyScale.z+0.5f) * j), Quaternion.identity, this.transform);
                tiles[i,j] = newTile.GetComponent<Tile>();
                newTile.GetComponent<Tile>().X = i;
                newTile.GetComponent<Tile>().Y = j;
            }
        }

        int randomY = Random.Range(1, gm.tilesXrow-1);

        target = tiles[gm.tilesXrow - 1, randomY];
        FindNeighbors(target);
        CreateRoad(target, gm.roadTarget, target);

        int numberSpawnPoints = Mathf.RoundToInt(gm.tilesXrow / 5f);
        for (int i=0; i<numberSpawnPoints; i++)
        {
            randomY = Random.Range(i*gm.tilesXrow/numberSpawnPoints + 1, (i + 1) * gm.tilesXrow / numberSpawnPoints - 1);
            var spawnTile = tiles[0, randomY];
            spawnPoints.Add(spawnTile);
            FindNeighbors(spawnTile);
            CreateRoad(spawnTile, gm.roadSpawn, spawnTile);
        }
        
        for (int i=0; i<gm.tilesXrow; i++)
        {
            Vector3 fencePosition;
            GameObject newFence;
            if (!spawnPoints.Contains(tiles[0, i]) && tiles[0, i].roadOn)
            {
                fencePosition = new Vector3(-2.75f, 0f, 0f) + Vector3.up * 1.25f;
                newFence = Instantiate(gm.fence, tiles[0, i].transform.position + fencePosition, Quaternion.identity);
                newFence.transform.parent = tiles[0, i].transform;
            }

            if (!spawnPoints.Contains(tiles[i, 0]) && tiles[i, 0].roadOn)
            {
                fencePosition = new Vector3(0f, 0f, -2.75f) + Vector3.up * 1.25f;
                newFence = Instantiate(gm.fence, tiles[i, 0].transform.position + fencePosition, Quaternion.Euler(new Vector3(0, 90, 0)));
                newFence.transform.parent = tiles[i, 0].transform;
            }

            if (!spawnPoints.Contains(tiles[gm.tilesXrow - 1, i]) && tiles[gm.tilesXrow - 1, i].roadOn)
            {
                fencePosition = new Vector3(2.75f, 0f, 0f) + Vector3.up * 1.25f;
                newFence = Instantiate(gm.fence, tiles[gm.tilesXrow-1, i].transform.position + fencePosition, Quaternion.identity);
                newFence.transform.parent = tiles[gm.tilesXrow-1, i].transform;
            }

            if (!spawnPoints.Contains(tiles[i, gm.tilesXrow - 1]) && tiles[i, gm.tilesXrow-1].roadOn)
            {
                fencePosition = new Vector3(0f, 0f, 2.75f) + Vector3.up * 1.25f;
                newFence = Instantiate(gm.fence, tiles[i, gm.tilesXrow - 1].transform.position + fencePosition, Quaternion.Euler(new Vector3(0, 90, 0)));
                newFence.transform.parent = tiles[i, gm.tilesXrow - 1].transform;
            }
        }

        GetComponent<NavMeshSurface>().BuildNavMesh();

        GameManager.instance.gridCreated = true;
    }

    public void FindNeighbors(Tile tile) {
        tile.neighbors = new Tile[4];
        if (tile.X > 0)
            tile.neighbors[0] = tiles[tile.X - 1, tile.Y];

        if (tile.Y > 0)
            tile.neighbors[1] = tiles[tile.X, tile.Y - 1];

        if (tile.X < GameManager.instance.tilesXrow - 1)
            tile.neighbors[2] = tiles[tile.X + 1, tile.Y];

        if (tile.Y < GameManager.instance.tilesXrow - 1)
            tile.neighbors[3] = tiles[tile.X, tile.Y + 1];
    }

    bool CheckNeighbor(Tile neighbor, Tile tile, Tile target)
    {
        if (neighbor == null)
        {
            return false;
        }
        else
        {
            return 
            neighbor.X > tile.mySpawn.X && 
            neighbor.X >= tile.X &&
            neighbor.X <= target.X && 
            (neighbor.X >= Mathf.RoundToInt(GameManager.instance.tilesXrow /4f)?  
             (neighbor.X >= Mathf.RoundToInt(GameManager.instance.tilesXrow /4f)*2?
              (neighbor.X >= Mathf.RoundToInt(GameManager.instance.tilesXrow /4f)*3?
               Mathf.Abs(neighbor.Y - target.Y) <= Mathf.Abs(tile.Y - target.Y):
               Mathf.Abs(neighbor.Y - target.Y) >= Mathf.Abs(tile.Y - target.Y)
              ):
              Mathf.Abs(neighbor.Y - target.Y) <= Mathf.Abs(tile.Y - target.Y)
             ):
             Mathf.Abs(neighbor.Y - target.Y) >= Mathf.Abs(tile.Y - target.Y)
            );
        }
    }


    public void CreateRoad(Tile tile, GameObject road, Tile myspawn)
    {
        Instantiate(road, tile.transform.position, Quaternion.identity, tile.transform);
        tile.roadOn = true;
        tile.GetComponent<MeshRenderer>().enabled = false;
        tile.GetComponent<Collider>().enabled = false;
        tile.mySpawn = myspawn;
        if (tile != target) {
            FindNeighbors(tile);
            do
            {
                var randomDir = Random.Range(0, tile.neighbors.Length);
                tile.randomNeighbor = tile.neighbors[randomDir];
                if (CheckNeighbor(tile.randomNeighbor, tile, target))
                {
                    tile.randomNeighbor.intersects.Add(tile);
                    if (tile.randomNeighbor == target)
                        return;

                    if (tile.randomNeighbor.roadOn)
                        return;

                    CreateRoad(tile.randomNeighbor, GameManager.instance.roadPrefab, myspawn);
                }
                else tile.randomNeighbor = null;
            }
            while (tile.randomNeighbor == null);
        }
    }
}
