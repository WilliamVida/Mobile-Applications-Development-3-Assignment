using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class sets the bird movement. The birds move from left to right on the screen.
*/
public class BirdMovement : MonoBehaviour
{

    [SerializeField] public float speed = 3.0f;
    private float maxX = 10.0f;
    private float maxY = 3.5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1 * speed, 0);
    }

    public void Move()
    {
        if (rb.position.x >= maxX)
            rb.velocity = new Vector2(-1 * speed, 0);
        else if (rb.position.x <= -maxX)
            rb.velocity = new Vector2(1 * speed, 0);

        if (rb.position.y >= maxY)
            rb.velocity = new Vector2(0, -1 * speed);
        else if (rb.position.y <= -maxY)
            rb.velocity = new Vector2(0, -1 * speed);
    }

}
