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
    public GameObject wall;
    public Tile[] buildings;
    public GameObject mushroom;
    public GameObject spikes;
    private int houseWidth = 4;
    private int houseHeight = 4;
    private GameObject boardHolder;

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
        // Generate Floor & Walls
        for (int x = -20; x < columns + 20; x++)
        {
            for (int y = -15; y < rows + 15; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                // GameObject instance = Instantiate(tile, pos, Quaternion.identity, boardHolder);
                // Generates wall if on edge
                tilemap.SetTile(pos, grass);
                if (x >= 0 && x < columns && y >= 0 && y < rows)
                    tileIsFull[x, y] = false;
                if (x == 0 || x == columns - 1 || y == 0 || y == rows - 1)
                {
                    // GameObject wallObject = Instantiate(wall, pos, Quaternion.identity, instance.transform);
                    // grid[x, y] = new Tile(pos, instance, wallObject);
                    if (x >= 0 && x < columns && y >= 0 && y < rows)
                        spawnItem(wall, pos);
                }
            }
        }
        // Generate floor around edges
        // Generate 1 House
        // Get random coordinate for bottom-left corner of house
        Vector3Int randPos = new Vector3Int(0, 0, 0);
        for (int i = 0; i < buildings.Length; i++)
        {
            do
            {
                randPos.x = Random.Range(1, columns - houseWidth);
                randPos.y = Random.Range(1 + houseHeight, rows);
            } while (tileIsFull[randPos.x, randPos.y]);
            Debug.Log(randPos);
            tilemap.SetTile(randPos, buildings[0]);
            // Mark tiles occupied by house as full
            for (int x = 0; x < houseWidth; x++)
            {
                for (int y = houseHeight - 1; y >= 0; y--)
                {
                    tileIsFull[randPos.x + x, randPos.y - y] = true;
                }
            }
        }
        // // Spawn Mushrooms
        // // (row*columns) / 10 clumps
        // // clumps contain 2-5 mushrooms
        SpawnClump(mushroom, 5, 3, 20, 5, 40, true);
        SpawnClump(spikes, 5, 1, 4, 20, 5);
        // Spawn Player
        do
        {
            randPos.x = Random.Range(1, columns - houseWidth);
            randPos.y = Random.Range(1 + houseHeight, rows);
        } while (tileIsFull[randPos.x, randPos.y]);
        player.transform.position = grid.GetCellCenterWorld(randPos);
    }

    public void setTileState(Vector3Int pos, bool isFull)
    {
        tileIsFull[pos.x, pos.y] = isFull;
    }


    private void SpawnClump(GameObject item, int numClumps, int minClumpSize, int maxClumpSize, int maxDistFromSeed, int spawnChance = 10, bool distanceWeight = false)
    {
        int maxClumpLen = maxDistFromSeed * 2 + 1;
        int maxTilesInClump = maxClumpLen * maxClumpLen;
        if (minClumpSize > maxClumpSize)
        {
            Debug.LogError("minClumpSize must be smaller or equal to maxClumpSize... Lowering minClumpSize");
            minClumpSize = maxClumpSize;
        }
        if (minClumpSize > maxTilesInClump)
        {
            Debug.LogError("Not enough possible tiles to meet minClumpSize... Lowering minClumpSize");
            minClumpSize = 2 * maxDistFromSeed + 1 > maxClumpSize ? maxClumpSize : 2 * maxDistFromSeed + 1;
        }
        for (int n = 0; n < numClumps; n++)
        {
            int clumpSize = 0;
            int itemsToCreate = Random.Range(minClumpSize, maxClumpSize + 1);
            // Get random tile to start clump
            Vector3Int seedPos = new Vector3Int(0, 0, 0);
            do
            {
                seedPos.x = Random.Range(1, columns - houseWidth);
                seedPos.y = Random.Range(1 + houseHeight, rows);
            } while (tileIsFull[seedPos.x, seedPos.y]);
            Vector3Int pos = seedPos;
            // Generate items in spiral around seed
            do
            {
                int x, y, dx, dy;
                float dist;
                x = y = dx = 0;
                dy = -1;
                int t = maxClumpLen;
                for (int i = 0; clumpSize < itemsToCreate && i < maxTilesInClump; i++)
                {
                    if (-maxDistFromSeed <= x && x <= maxDistFromSeed && -maxDistFromSeed <= y && y <= maxDistFromSeed)
                    {
                        // Attempt to generate item
                        pos.x = seedPos.x + x;
                        pos.y = seedPos.y + y;
                        // Spawn if within boundaries, passes spawnChance check and tile isn't full
                        if (pos.x > 0 && pos.x < columns && pos.y > 0 && pos.y < rows && !tileIsFull[pos.x, pos.y])
                        {
                            if (x == 0 && y == 0)
                            {
                                // turn into function
                                spawnItem(item, pos);
                                clumpSize++;
                            }
                            else
                            {
                                if (distanceWeight)
                                {
                                    dist = 1 / Mathf.Sqrt(x * x + y * y); // make less sharp? currently i.e. 2 dist = half chance -> 3 dist = third chance 
                                    if (Random.Range(0, 100) < spawnChance / dist)
                                    {
                                        spawnItem(item, pos);
                                        clumpSize++;
                                    }
                                }
                                else
                                {
                                    if (Random.Range(0, 100) < spawnChance)
                                    {
                                        spawnItem(item, pos);
                                        clumpSize++;
                                    }
                                }


                            }

                        }
                    }
                    if (x == y || (x < 0 && x == -y) || (x > 0 && x == 1 - y))
                    {
                        t = dx;
                        dx = -dy;
                        dy = t;
                    }
                    x += dx;
                    y += dy;
                }
            } while (clumpSize < minClumpSize);

        }
    }


    private void spawnItem(GameObject item, Vector3Int pos)
    {
        Instantiate(item, grid.GetCellCenterWorld(pos), Quaternion.identity, transform);
        setTileState(pos, true);
    }
}