using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class sets the movement for the bees. All of the bees move along a set of waypoints.
*/
// waypoints from https://www.youtube.com/watch?v=TJCOC0gcU4k.
public class BeeMovement : MonoBehaviour
{

    public GameObject[] waypoints;
    int current = 0;
    float rotSpeed;
    [SerializeField] public float speed = 4.0f;
    float wPradius = 1;

    void Update()
    {
        if (Vector2.Distance(waypoints[current].transform.position, transform.position) < wPradius)
        {
            current++;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
    }

}
