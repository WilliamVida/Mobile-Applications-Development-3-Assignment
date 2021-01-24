using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class changes the states for the bees depending on the situation that arises. When a bee is at the beehive, it will regain energy. The bees search for flowers which increment a nectar value when they hit a flower and if they are full then they will return to the beehive. If they are searching and their energy goes below a certain level, then they will return to the beehive for safety. If a bee is within range of a bird, then the bee will race back to the beehive. If a bee gets touched by a bird then the bee is destroyed. When the bee is at the beehive it deposits its nectar and starts searching again after it is finished. This class sets the bar colour above the bee depending on the energy remaining.
*/
public class BeeState : MonoBehaviour
{

    const float MAXIMUM_ENERGY = 150.0f;
    const float MINIMUM_ENERGY = 0.0f;
    public float energyLevel = MAXIMUM_ENERGY;
    [SerializeField] float payloadValue = 0.0f;
    public BirdState birdState;
    public BeeMovement beeMovement;
    public Animator animator;
    public bool isEaten = false;
    int nectar = 0;
    private GameObject[] beeHive;
    private Transform beeHiveLocation;
    public SpriteRenderer m_SpriteRenderer;
    float percentage = 0.0f;
    public bool isAtHive = false;

    void Update()
    {
        percentage = energyLevel / MAXIMUM_ENERGY * 100;

        if (percentage < 30.0f)
            m_SpriteRenderer.color = Color.red;
        else if (percentage < 60.0f)
            m_SpriteRenderer.color = Color.yellow;
        else if (percentage >= 60.0f)
            m_SpriteRenderer.color = Color.green;

        if (energyLevel >= MAXIMUM_ENERGY)
        {
            energyLevel = MAXIMUM_ENERGY;
            animator.SetFloat("Energy", energyLevel);
        }
        else if (energyLevel <= MINIMUM_ENERGY)
        {
            energyLevel = MINIMUM_ENERGY;
            animator.SetFloat("Energy", energyLevel);
        }

        AtHive();
        Dancing();
        Fleeing();
        Gathering();
        Searching();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bird")
        {
            Debug.Log("Bee: Eaten by bird");
            isEaten = true;
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Flower")
        {
            animator.SetBool("IsFlower", true);
            Debug.Log("Bee: Nectar picked up. Total " + nectar);
            nectar++;
        }

        if (col.gameObject.tag == "Bee Hive")
        {
            Debug.Log("Bee: At Bee Hive");
            isAtHive = true;
            // I think it works better setting the nectar to 0 when the bee hits the hive rather than incrementing it.
            nectar = 0;
            energyLevel = MAXIMUM_ENERGY;
            animator.SetBool("IsDancing", true);
            animator.SetFloat("Energy", energyLevel);
        }
        else
        {
            animator.SetBool("IsDancing", false);
            isAtHive = false;
        }
    }

    void Awake()
    {
        beeHive = GameObject.FindGameObjectsWithTag("Bee Hive");
        beeHiveLocation = beeHive[Random.Range(0, beeHive.Length)].transform;
    }

    public void AtHive()
    {

    }

    public void Dancing()
    {

    }

    public void Fleeing()
    {
        if (birdState.isInRange)
        {
            float step = 8.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, beeHiveLocation.position, step);
            animator.SetBool("IsDanger", true);
        }
        else
        {
            animator.SetBool("IsDanger", false);
        }
    }

    public void Gathering()
    {
        if (nectar >= 3)
        {
            nectar = 3;
            float step = 8.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, beeHiveLocation.position, step);
            animator.SetBool("IsMax", true);
        }
        else if (nectar == 0)
        {
            animator.SetBool("IsMax", false);
        }
    }

    public void Searching()
    {
        animator.SetFloat("Energy", energyLevel);
        energyLevel -= 0.1f;

        if (energyLevel <= 30)
        {
            Debug.Log("Bee: Low energy.");
            animator.SetFloat("Energy", energyLevel);
            float step = 7.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, beeHiveLocation.position, step);
        }
    }

}
