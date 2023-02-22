using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] LevelStructures;
    [SerializeField] private Transform LevelStartLayout;
    [SerializeField] private Transform LevelEndLayout;
    private Vector3 initialSpawnPosition = new Vector3(0f, 0f, 0f);
    private Vector3 spawnPositionOffset = new Vector3(0f, 0f, 10f);
    [SerializeField] private Transform[] Obstacles;
    private List<GameObject> spawnZoneObjects = new List<GameObject>();
    [SerializeField] private List<BoxCollider> SpawnZones = new List<BoxCollider>();
    [SerializeField] private Material[] AvailableColors;
    private GameObject player;
    private Color playerColor;
    private Material playerMaterial;
    private static GameObject instance;
    public int LevelDisplay;
    private int totalLevels = 9;
    private UIManager uIManager;
    [SerializeField] private AudioSource buttonClickSound;
    private GoogleAdsManager googleAdsManager;
    private List<Material> LevelColors = new List<Material>();
    private void Awake()
    {
        googleAdsManager = FindObjectOfType<GoogleAdsManager>();
    }

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        PlayerInfo.isGamePaused = true;
        StartLevel();
        LevelDisplay = PlayerInfo.currentLevel;
    }

    private void StartLevel()
    {
        List<int> uniqueRandomNumbers = new List<int>();
        while(uniqueRandomNumbers.Count < 3) {
            int randomNumber = Random.Range(0, AvailableColors.Length);
            if(!uniqueRandomNumbers.Contains(randomNumber)) uniqueRandomNumbers.Add(randomNumber);
        }
        foreach(int indexValue in uniqueRandomNumbers) {
            Debug.Log("INDEX: " + indexValue);
            LevelColors.Add(AvailableColors[indexValue]);
        }

        FindObjectOfType<PlayerMovement>().gameObject.GetComponent<Renderer>().material = LevelColors[0];
        playerMaterial = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<Renderer>().material;
        playerColor = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<Renderer>().material.color;
        Instantiate(LevelStartLayout, initialSpawnPosition, Quaternion.identity);
        for (int i = 0; i < 3; i++)
        {
            int randomLevelStructure = Random.Range(0, LevelStructures.Length);
            SpawnZones.Clear();
            Transform levelStructure = Instantiate(LevelStructures[randomLevelStructure], initialSpawnPosition + spawnPositionOffset, Quaternion.identity);

            foreach (GameObject spawnZone in GameObject.FindGameObjectsWithTag("SpawnZone"))
            {
                SpawnZones.Add(spawnZone.GetComponent<BoxCollider>());
                spawnZone.tag = "Untagged";
            }

            int randomWay = Random.Range(0, SpawnZones.Count);
            float spawnX, spawnZ, maxSpawnX, maxSpawnZ;
            for (int j = 0; j < SpawnZones.Count; j++)
            {
                Color obstacleColor;
                Material obstacleMaterial;
                int randomObjectIndex = Random.Range(0, Obstacles.Length);
                if (j == randomWay)
                {
                    obstacleColor = playerColor;
                    obstacleMaterial = playerMaterial;
                }
                else
                {
                    obstacleMaterial = LevelColors[1];
                    obstacleColor = obstacleMaterial.color;
                    while (obstacleColor == playerColor)
                    {
                        obstacleMaterial = AvailableColors[Random.Range(0, AvailableColors.Length)];
                        obstacleColor = obstacleMaterial.color;
                    }
                }
                Obstacles[randomObjectIndex].gameObject.GetComponent<Renderer>().material = obstacleMaterial;
                spawnX = SpawnZones[j].bounds.min.x;
                maxSpawnX = SpawnZones[j].bounds.max.x;
                spawnZ = SpawnZones[j].bounds.min.z;
                maxSpawnZ = SpawnZones[j].bounds.max.z;
                // Debug.Log("Spawn Positions: " + spawnX + maxSpawnX + spawnZ + maxSpawnZ);
                while (spawnZ < maxSpawnZ)
                {
                    while (spawnX < maxSpawnX)
                    {
                        Transform spawnedObstacle = Instantiate(Obstacles[randomObjectIndex], new Vector3(spawnX, 0.5f, spawnZ), Quaternion.identity);
                        spawnX += 0.4f;
                    }
                    spawnX = SpawnZones[j].bounds.min.x;
                    spawnZ += 0.4f;
                }
            }
            initialSpawnPosition += spawnPositionOffset;
        }
        initialSpawnPosition += spawnPositionOffset;
        Instantiate(LevelEndLayout, initialSpawnPosition, Quaternion.identity);

        GameObject[] allLevelBases = GameObject.FindGameObjectsWithTag("Base");
        foreach (GameObject levelBase in allLevelBases)
        {
            levelBase.GetComponent<Renderer>().material = LevelColors[2];
        }
        uIManager.ShowLevelStartUI();
    }

    public void LevelComplete()
    {
        if (PlayerInfo.currentLevel >= totalLevels)
        {
            PlayerInfo.currentLevel = 0;
            googleAdsManager.ShowRewardAd();
            // Debug.Log("YOU COMPLETED THE GAME!!");
        }
        else
        {
            PlayerInfo.currentLevel++;
            uIManager.ShowLevelCompleteUI();
        }
    }

    public void GameOver()
    {
        PlayerInfo.isGamePaused = true;
        uIManager.ShowGameOverUI();
    }

    public void RetryLevel()
    {
        buttonClickSound.Play();
        PlayerInfo.isGamePaused = false;
        SceneManager.LoadScene("Main Game");
    }

    public void ExitToMainMenu()
    {
        buttonClickSound.Play();
        SceneManager.LoadScene("Main Menu");
    }

}
