using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public enum TurretType
    {
        Pistol,
        Missile,
        Sniper
    }

    public TurretType type;

    public enum TurretOrientation
    {
        Left,
        Up,
        Right,
        Down,
        None
    }

    public TurretOrientation orientation; 

    public Transform magazine;
    public Transform[] bores;
    public GameObject projectile;
    public float fireRate;

    private EnemyInRange range;
    private float time;

    public List<Tile> occupiedTiles;
    public Quaternion defaultRotation;
    public bool placed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fire());
        range = GetComponentInChildren<EnemyInRange>();
        defaultRotation = magazine.transform.rotation;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(range.enemies.Count > 0)
        {
            if(type != TurretType.Missile)
            {
                if (range.enemies[0] != null)
                    magazine.LookAt(range.enemies[0].transform);
                else
                    range.RemoveEnemy(range.enemies[0]);
            }
        }
        else
        {
            magazine.rotation = defaultRotation;
        }

        range.GetComponent<MeshRenderer>().enabled = !placed;
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            if(time >= fireRate && range.enemies.Count > 0) { 
                time = 0;
                foreach(Transform bore in bores)
                {
                    var newProjectile = Instantiate(projectile, bore.position, Quaternion.identity);
                    if(type == TurretType.Missile)
                    {
                        var randomIdx = Random.Range(0, range.enemies.Count-1);
                        if(range.enemies[randomIdx] != null)
                            newProjectile.GetComponent<ProjectileBehavior>().target = range.enemies[randomIdx].transform;
                        else
                            range.RemoveEnemy(range.enemies[randomIdx]);
                    }
                    else
                    {
                        if (range.enemies[0] != null)
                            newProjectile.GetComponent<ProjectileBehavior>().target = range.enemies[0].transform;
                        else
                            range.RemoveEnemy(range.enemies[0]);
                    }     
                }
            }
            yield return null;
        }
    }
}
