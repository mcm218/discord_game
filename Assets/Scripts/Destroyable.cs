using UnityEngine;


/*
    If performance suffers, could instead Raycast to check if hit object, then check for Destroyable class
    If exists,
        grab destruction info from here and run code from playerController
    else,
        do nothing
*/
public class Destroyable : MonoBehaviour
{
    private GameObject player;
    public int toolTypeRequired;
    const float distance = 1.0f;
    public float timeToDestroy = 1.0f;
    bool counting = false;
    private float timer = 1.0f;
    void Start()
    {
        // Grab reference to player at start
        // Used to verify they have the required tool to harvest 
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("Couldn't find player");
        }
    }
    void OnMouseDown()
    {
        // check if player is close enough
        // start destruction timer
        Vector2 pos = player.transform.position;
        if (Mathf.Abs(pos.x - transform.position.x) < distance && Mathf.Abs(pos.y - transform.position.y) < distance)
        {
            Player playerController = player.GetComponent<Player>();
            if (toolTypeRequired < 0 || playerController.tool == toolTypeRequired)
            {
                timer = timeToDestroy;
                counting = true;
                Debug.Log("Destroying in: " + timer);
            }
        }
    }

    void OnMouseOver()
    {
        // decrement timer
        // if timer runs out, destroy object
        if (counting && player != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                // update tile as no longer full
                Grid grid = GetComponentInParent<Grid>();
                BoardManager boardManager = GetComponentInParent<BoardManager>();
                if (grid != null && boardManager != null)
                {

                    Vector3Int pos = grid.WorldToCell(transform.position);
                    boardManager.setTileState(pos, false);
                    // remove object
                    Destroy(transform.gameObject);
                }
                else
                {
                    Debug.Log("Couldn't find grid or board manager");
                }

            }
        }
    }

    void OnMouseExit()
    {
        counting = false;
    }

    void OnMouseUp()
    {
        // reset timer
        counting = false;
    }
}