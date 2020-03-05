using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoardManager : MonoBehaviour
{
    public GameObject player;
    public int columns = 100;
    public int rows = 100;
    public GameObject tile;
    public GameObject wall;
    public GameObject house;
    public GameObject mushroom;
    public GameObject spikes;
    public GameObject[] spawnItems;
    private int houseWidth = 4;
    private int houseHeight = 4;
    public Sprite floor;
    private Transform boardHolder;

    private Tile[,] grid;

    public void SetupScene()
    {
        grid = new Tile[columns, rows];
        if (columns < 10) columns = 10;
        if (rows < 10) rows = 10;
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
                Vector2 pos = new Vector2(x, y);
                GameObject instance = Instantiate(tile, pos, Quaternion.identity, boardHolder);
                // Generates wall if on edge
                if (x == 0 || x == columns - 1 || y == 0 || y == rows - 1)
                {
                    GameObject wallObject = Instantiate(wall, pos, Quaternion.identity, instance.transform);
                    grid[x, y] = new Tile(pos, instance, wallObject);
                }
                else
                {
                    grid[x, y] = new Tile(pos, instance);
                }
            }
        }
        // Generate 1 House
        // Get random coordinate for bottom-left corner of house
        Tile randTile;
        do
        {
            randTile = grid[Random.Range(2, columns - houseWidth), Random.Range(2, rows - houseHeight)];
        } while (randTile.isFull);
        Debug.Log("House Position: ");
        Debug.Log(randTile.position);
        Debug.Log(houseWidth + "x" + houseHeight);
        // Single Game Object, covering multiple tiles
        GameObject houseObject = Instantiate(house, randTile.position, Quaternion.identity, randTile.self.transform);
        // Adjust object position so it fully covers parent tile
        // move to adjust location within child? But need to account for objectWidth/objectHeight... 
        houseObject.transform.position += new Vector3(houseWidth / 2.0f - 0.5f, houseHeight / 2.0f - 0.5f, 0);
        randTile.child = houseObject;
        // Mark tiles occupied by house as full
        for (int x = 1; x < houseWidth; x++)
        {
            int xPos = (int)randTile.position.x + x;
            for (int y = 1; y < houseHeight; y++)
            {
                int yPos = (int)randTile.position.y + y;
                if (grid[xPos, yPos].isFull)
                {
                    //delete whatever is here
                    Destroy(grid[xPos, yPos].child);
                }
                grid[xPos, yPos].child = houseObject;
            }
        }

        // Spawn Mushrooms
        // (row*columns) / 10 clumps
        // clumps contain 2-5 mushrooms
        SpawnClump(mushroom, 5, 3, 9, 5);
        SpawnClump(spikes, 5, 1, 4, 5);
        // Spawn Player
        do
        {
            randTile = grid[Random.Range(2, columns - houseWidth), Random.Range(2, rows - houseHeight)];
        } while (randTile.isFull);
        player.transform.position = randTile.position;
    }


    private void SpawnClump(GameObject item, int numClumps, int minClumpSize, int maxClumpSize, int spawnChance = 10)
    {
        for (int n = 0; n < numClumps; n++)
        {
            int clumpSize = 0;
            int itemsToCreate = Random.Range(minClumpSize, maxClumpSize + 1);
            // Get random tile to start clump
            Tile randTile;
            do
            {
                randTile = grid[Random.Range(1, columns), Random.Range(1, rows)];
            } while (randTile.isFull);
            randTile.child = Instantiate(item, randTile.position, Quaternion.identity, randTile.self.transform);
            clumpSize++;
            Vector2 seedPos = randTile.position;
            Tile newTile;
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
                        newTile = grid[(int)seedPos.x + x, (int)seedPos.y + y];
                        if (!newTile.isFull)
                        {
                            if (Random.Range(0, 100) < spawnChance)
                            {
                                newTile.child = Instantiate(item, newTile.position, Quaternion.identity, randTile.self.transform);
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