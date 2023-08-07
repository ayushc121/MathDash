using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlatformTile : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] obstacles; //Objects that contains different obstacle types which will be randomly activated
    public GameObject mathprob;
    private float cubeSize = 0.3f;

    private List<GameObject> spawnedCubes = new List<GameObject>(); // New List to store spawned cubes

    public void ActivateRandomObstacle()
    {
        DeactivateAllObstacles();
        ClearSpawnedCubes();

        mathprob.GetComponent<RandomMathProblemGenerator>().GenerateRandomMathProblem();
        int Canswer = mathprob.GetComponent<RandomMathProblemGenerator>().answer;
        List<int> answerlist = mathprob.GetComponent<RandomMathProblemGenerator>().randomizedAnswers;
        spawnedCubes.Clear();

        for (int i = 0; i < answerlist.Count; i++)
        {
            if (answerlist[i] == Canswer)
            {
                Vector3 spawnPosition = new Vector3(-3f + 3 * i, -1f, 40f);
                GameObject cube = CreateCube(spawnPosition, cubeSize);
                cube.tag = "Untagged"; // Empty tag means no tag assigned
                Destroy(cube.GetComponent<BoxCollider>()); // Remove Box Collider
                spawnedCubes.Add(cube);
            }
            else
            {
                Vector3 spawnPosition = new Vector3(-3f + 3 * i, -1f, 40f);
                GameObject cube = CreateCube(spawnPosition, cubeSize);
                cube.tag = "Finish"; // Assign the "Finish" tag
                spawnedCubes.Add(cube);
            }
        }
    }

    public void DeactivateAllObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
        }
    }

    public List<GameObject> GetSpawnedCubes()
    {
        return spawnedCubes;
    }

    private GameObject CreateCube(Vector3 position, float size)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.localScale = new Vector3(size, size, size);
        return cube;
    }

    private void ClearSpawnedCubes()
    {
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }
        spawnedCubes.Clear();
    }
}
