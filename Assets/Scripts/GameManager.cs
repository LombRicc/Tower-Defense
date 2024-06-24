using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Prefabs")]
    public GameObject roadPrefab;
    public GameObject roadSpawn;
    public GameObject roadTarget;
    public GameObject tilePrefab;
    public GameObject fence;

    [Header("Turrets")]
    public GameObject pistol;
    public GameObject missile;
    public GameObject sniper;

    [Header("Enemies")]
    public GameObject soldier;
    public GameObject bird;
    public GameObject knight;
    public GameObject dragon;

    [Header("Game Parameters")]
    public float spawnTimer = 2f;
    public int waveNumber=0;
    public int enemiesNumber = 0;
    public int soldierNumber=0;
    public int knightNumber=0;
    public int birdNumber=0;
    public int dragonNumber=0;
    public int health;
    public int gold;
    public int pistolCost=20;
    public int missileCost=40;
    public int sniperCost=80;
    [Range(5, 20)] public int tilesXrow = 5;

    [Header("Game Controllers")]
    public GameObject selectedTurret;
    public GameObject gameOverPane;
    public GameObject playerPaneCollapsed;
    public GameObject playerPaneExpanded;
    public SpriteSwitcher fastForwardButton;
    public GenerateGrid grid;
    public bool gridCreated;
    public bool placingTurret;
    public GameObject placingTurretGo;
    public GameObject originalTurret;
    public GameObject startWaveBtn;
    public List<GameObject> enemies;
    public bool waveStarted;
    public bool newWave;

    private bool fastforward;
    private int originalHealth;
    private int originalGold;

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
        originalHealth = health;
        originalGold = gold;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !newWave)
        {
            ManageWave();
        }
        if(enemies.Count == 0 && newWave)
        {
            newWave = false;
            waveStarted = false;
        }
    }

    public void ManageFastForward()
    {
        if (!fastforward)
        {
            Time.timeScale = 4;
            fastforward = true;
        }
        else
        {
            Time.timeScale = 1;
            fastforward = false;
        }
    }

    public void SetGridDimension(System.Single tilesXrow)
    {
        this.tilesXrow = (int) tilesXrow;
    }

    public void TakeDamage()
    {
        health -= 10;
        if(health <= 0)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        grid.gameObject.SetActive(true);
        Camera.main.GetComponent<InputController>().enabled = true;
    }

    public void GameOver()
    {
        if(Time.timeScale != 1)
        {
            fastForwardButton.SwitchSprite();
            fastforward = false;
        }
        Time.timeScale = 0;
        gameOverPane.SetActive(true);
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ManageWave()
    {
        waveNumber += 1;
        enemiesNumber = 10 * waveNumber;
        if (waveNumber == 1)
        {
            soldierNumber = enemiesNumber;
            spawnTimer = 13;
        }
        if(waveNumber == 2)
        {
            soldierNumber = Mathf.RoundToInt(enemiesNumber / 3f * 2f);
            birdNumber = Mathf.RoundToInt(enemiesNumber/3f);
            spawnTimer = 8;
        }
        if(waveNumber == 3)
        {
            soldierNumber = Mathf.RoundToInt(enemiesNumber / 2f);
            birdNumber = Mathf.RoundToInt(enemiesNumber / 2f);
            spawnTimer = 5;
        }
        if(waveNumber == 4)
        {
            soldierNumber = Mathf.RoundToInt(enemiesNumber / 6f * 3f);
            birdNumber = Mathf.RoundToInt(enemiesNumber / 6f * 2f);
            knightNumber = Mathf.RoundToInt(enemiesNumber / 6f);
            spawnTimer = 3;
        }
        if(waveNumber == 5)
        {
            soldierNumber = Mathf.RoundToInt(enemiesNumber / 5f * 2f);
            birdNumber = Mathf.RoundToInt(enemiesNumber / 5f * 2f);
            knightNumber = Mathf.RoundToInt(enemiesNumber / 5f);
            spawnTimer = 2;
        }
        if(waveNumber == 6)
        {
            soldierNumber = Mathf.RoundToInt(enemiesNumber / 9f * 3f);
            birdNumber = Mathf.RoundToInt(enemiesNumber / 9f * 3f);
            knightNumber = Mathf.RoundToInt(enemiesNumber / 9f * 2f);
            dragonNumber = Mathf.RoundToInt(enemiesNumber / 9f);
            spawnTimer = 1;
        }
        if(waveNumber >= 7)
        {
            soldierNumber = Mathf.RoundToInt(enemiesNumber / 6f * 2f);
            birdNumber = Mathf.RoundToInt(enemiesNumber / 6f * 2f);
            knightNumber = Mathf.RoundToInt(enemiesNumber / 6f);
            dragonNumber = Mathf.RoundToInt(enemiesNumber / 6f);
            spawnTimer = 1;
        }
        GameObject newEnemy;
        for(int i = 0; i < soldierNumber; i++) {
            newEnemy = Instantiate(soldier);
            enemies.Add(newEnemy);
        }
        for (int i = 0; i < birdNumber; i++)
        {
            newEnemy = Instantiate(bird);
            enemies.Add(newEnemy);
        }
        for (int i = 0; i < knightNumber; i++)
        {
            newEnemy = Instantiate(knight);
            enemies.Add(newEnemy);
        }
        for (int i = 0; i < dragonNumber; i++)
        {
            newEnemy = Instantiate(dragon);
            enemies.Add(newEnemy);
        }
        startWaveBtn.SetActive(true);
        waveStarted = false;
        newWave = true;
    }

    public void StartWave()
    {
        waveStarted = true;
    }
}
