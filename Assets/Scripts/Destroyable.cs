using UnityEngine;

public class Destroyable : MonoBehaviour
{
    const float distance = 1.5f;
    public float maxTimer = 1.0f;
    bool counting = false;
    private float timer = 1.0f;
    void OnMouseDown()
    {
        // check if player is close enough
        // start destruction timer
        Vector2 pos = Camera.main.transform.position;
        if (Mathf.Abs(pos.x - transform.position.x) < distance && Mathf.Abs(pos.y - transform.position.y) < distance)
        {
            timer = maxTimer;
            counting = true;
            Debug.Log("Destroying in: " + timer);
        }
    }

    void Update()
    {
        // decrement timer
        // if timer runs out, destroy object
        if (counting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    void OnMouseUp()
    {
        // reset timer
        counting = false;
    }
}