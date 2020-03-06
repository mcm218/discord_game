using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public GameObject player;
    public int columns = 100;
    public int rows = 100;
    public Tile grass;
    public Tile wall;
    public Tile house;
    public GameObject mushroom;
    public GameObject spikes;
    public GameObject[] spawnItems;
    private int houseWidth = 4;
    private int houseHeight = 4;
    private Transform boardHolder;

    private Grid grid;
    private Tilemap tilemap;
    private bool[,] tileIsFull;

    public void SetupScene()
    {
        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();
        if (columns < 10) columns = 10;
        if (rows < 10) rows = 10;
        tileIsFull = new bool[columns, rows];
        BoardSetup();
    }
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        // Generate Floor & Walls
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                // GameObject instance = Instantiate(tile, pos, Quaternion.identity, boardHolder);
                // Generates wall if on edge
                if (x == 0 || x == columns - 1 || y == 0 || y == rows - 1)
                {
                    // GameObject wallObject = Instantiate(wall, pos, Quaternion.identity, instance.transform);
                    // grid[x, y] = new Tile(pos, instance, wallObject);
                    tilemap.SetTile(pos, wall);
                    tileIsFull[x, y] = true;
                }
                else
                {
                    tilemap.SetTile(pos, grass);
                    tileIsFull[x, y] = false;
                }
            }
        }
        // Generate 1 House
        // Get random coordinate for bottom-left corner of house
        Vector3Int randPos = new Vector3Int(0, 0, 0);
        do
        {
            randPos.x = Random.Range(1, columns - houseWidth);
            randPos.y = Random.Range(1 + houseHeight, rows);
        } while (tileIsFull[randPos.x, randPos.y]);
        Debug.Log(randPos);
        tilemap.SetTile(randPos, house);
        // Mark tiles occupied by house as full
        for (int x = 0; x < houseWidth; x++)
        {
            for (int y = houseHeight - 1; y >= 0; y--)
            {
                tileIsFull[randPos.x + x, randPos.y - y] = true;
            }
        }
        // // Spawn Mushrooms
        // // (row*columns) / 10 clumps
        // // clumps contain 2-5 mushrooms
        SpawnClump(mushroom, 5, 3, 9, 5);
        SpawnClump(spikes, 5, 1, 4, 5);
        // Spawn Player
    }


    private void SpawnClump(GameObject item, int numClumps, int minClumpSize, int maxClumpSize, int spawnChance = 10)
    {
        for (int n = 0; n < numClumps; n++)
        {
            int clumpSize = 0;
            int itemsToCreate = Random.Range(minClumpSize, maxClumpSize + 1);
            // Get random tile to start clump
            Vector3Int randPos = new Vector3Int(0, 0, 0);
            do
            {
                randPos.x = Random.Range(1, columns - houseWidth);
                randPos.y = Random.Range(1 + houseHeight, rows);
            } while (tileIsFull[randPos.x, randPos.y]);
            // convert grid coords to world coords
            Instantiate(item, grid.GetCellCenterWorld(randPos), Quaternion.identity);
            clumpSize++;
            Vector3Int seedPos = randPos;
            Vector3Int newPos = randPos;
            int root = (int)Mathf.Ceil(Mathf.Sqrt(maxClumpSize));
            do
            {
                for (int y = -root; y < root; y++)
                {
                    for (int x = -root; x < root; x++)
                    {
                        if ((int)seedPos.x + x < 0 || (int)seedPos.x + x >= columns || (int)seedPos.y + y < 0 || (int)seedPos.y + y >= rows)
                        {
                            continue;
                        }
                        newPos.x = seedPos.x + x;
                        newPos.y = seedPos.y + y;
                        if (!tileIsFull[newPos.x, newPos.y])
                        {
                            if (Random.Range(0, 100) < spawnChance)
                            {
                                Instantiate(item, grid.GetCellCenterWorld(newPos), Quaternion.identity);
                                clumpSize++;
                            }
                        }
                        if (clumpSize == itemsToCreate)
                        {
                            break;
                        }
                    }
                    if (clumpSize == itemsToCreate)
                    {
                        break;
                    }
                }
            } while (clumpSize < minClumpSize);

        }
    }
}