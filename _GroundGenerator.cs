using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _GroundGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public Transform startPoint;
    public _PlatformTile tilePrefab;
    public float movingSpeed = 20;
    public int tilesToPreSpawn = 2;
    public int tilesWithoutObstacles = 0;

    List<_PlatformTile> spawnedTiles = new List<_PlatformTile>();
    List<GameObject> spawnedCubes = new List<GameObject>(); // New List to store all spawned cubes

    public bool gameOver = false;
    static bool gameStarted = false;
    float score = 0;

    public static _GroundGenerator instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        // Initialize the spawnedCubes list
        spawnedCubes = new List<GameObject>();

        Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            _PlatformTile spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity) as _PlatformTile;
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAllObstacles();
                tilesWithNoObstaclesTmp--;
            }
            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }

        // Call ActivateRandomObstacle for the first tile to make sure all cubes are spawned correctly
        spawnedTiles[0].ActivateRandomObstacle();

        // Store the spawned cubes from all tiles in the main list
        foreach (var tile in spawnedTiles)
        {
            spawnedCubes.AddRange(tile.GetSpawnedCubes());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object upward in world space x unit/second.
        // Increase speed the higher score we get
        if (!gameOver && gameStarted)
        {
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed + (score / 500)), Space.World);
            score += Time.deltaTime * movingSpeed;

            // Move the spawned cubes along with the platforms
            foreach (var tile in spawnedTiles)
            {
                foreach (var cube in tile.GetSpawnedCubes())
                {
                    cube.transform.Translate(-tile.transform.forward * Time.deltaTime * movingSpeed, Space.World);
                }
            }
        }

        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            //Move the tile to the front if it's behind the Camera
            _PlatformTile tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.ActivateRandomObstacle();
            spawnedTiles.Add(tileTmp);

            // Update the spawned cubes list with the cubes from the new tile
            spawnedCubes.Clear();
            foreach (var tile in spawnedTiles)
            {
                spawnedCubes.AddRange(tile.GetSpawnedCubes());
            }
        }

        if (gameOver || !gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameOver)
                {
                    //Restart current scene
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
                else
                {
                    //Start the game
                    gameStarted = true;
                }
            }
        }
    }

    void OnGUI()
    {
        if (gameOver)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Game Over\nYour score is: " + ((int)score) + "\nPress 'Space' to restart");
        }
        else
        {
            if (!gameStarted)
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Press 'Space' to start \nPress 'A' to move left\nPress 'D' to move right");
            }
        }

        GUI.color = Color.green;
        GUI.Label(new Rect(5, 5, 200, 25), "Score: " + ((int)score));
    }
}
