using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject enemy;
    public float spawnTimer = 2f;
    public int waveNumber;
    public int soldierNumber;
    public int knightNumber;
    public int birdNumber;
    public int dragonNumber;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
       
    }
}
