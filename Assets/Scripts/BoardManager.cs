using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int columns = 100;
    public int rows = 100;
    public GameObject tile;
    public GameObject wall;
    public GameObject house;
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
                SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = floor;
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

    }
}
