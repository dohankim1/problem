using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGene : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapHeight = 50;
    public GameObject[] objectPrefabs; // Object prefabs for different types (e.g., wall low, wall high)
    public Transform planeTransform; // Reference to the plane's transform

    private List<List<int>> mapData = new List<List<int>>();

    void Start()
    {
        // Load map data from CSV
        LoadMapDataFromCSV();

        GenerateMapObjects();
    }

    void LoadMapDataFromCSV()
    {
        string filePath = Path.Combine(Application.dataPath, "MapData.csv");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            mapData.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                List<int> rowData = new List<int>();
                for (int j = 0; j < values.Length; j++)
                {
                    int cellType;
                    if (int.TryParse(values[j], out cellType))
                    {
                        rowData.Add(cellType);
                    }
                    else
                    {
                        Debug.LogWarning("Invalid value in CSV at row " + (i + 1) + ", column " + (j + 1));
                    }
                }
                mapData.Add(rowData);
            }
        }
        else
        {
            Debug.LogError("Map data file not found at: " + filePath);
        }
    }

    void GenerateMapObjects()
    {
        float tileSizeX = 1f;
        float tileSizeZ = 1f;

        // Calculate offsetX and offsetZ for centering objects
        float offsetX = planeTransform.localScale.x / 10f - tileSizeX * mapWidth / 2f;
        float offsetZ = planeTransform.localScale.z / 10f - tileSizeZ * mapHeight / 2f;

        // Loop through map data and instantiate objects based on the type
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                int cellType = mapData[i][j];
                if (cellType != 0 && cellType <= objectPrefabs.Length)
                {
                    GameObject prefab = objectPrefabs[cellType - 1];
                    Vector3 position = new Vector3(j * tileSizeX + offsetX, 0.5f, i * tileSizeZ + offsetZ);
                    Instantiate(prefab, position, Quaternion.identity, planeTransform);
                }
            }
        }
    }
}
