using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Height = 167;
    public Color HairColor = new Color(0, 0, 0, 1);
    public float speed = 3.0f;

    private PlayerStats playerStats = new PlayerStats();
    public PlayerStats stats { get { return playerStats; } }
    Rigidbody2D rigidbody2d;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3(1, Height / 167.0f, 1); // base height is 5'6" or 167cm
        spriteRenderer.color = HairColor;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        move.Normalize();
        Vector2 position = rigidbody2d.position;

        rigidbody2d.MovePosition(position + move * speed * Time.deltaTime);
    }
}
