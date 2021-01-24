using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class changes the states for the birds depending on the situation on the situation that arises. When the bird is performing a certain action then energy is added or removed. If the bird is flying then energy is removed. When the bird is resting then energy is added. If the bird eats a bee then the energy becomes the maximum. If a bee is in range of a bird then the bird will chase it. This class sets the bar colour above the bird depending on the energy remaining.
*/
public class BirdState : MonoBehaviour
{

    const float MAXIMUM_ENERGY = 150.0f;
    const float MINIMUM_ENERGY = 0.0f;
    public BirdMovement birdMovement;
    public BeeState beeState;
    public Animator animator;
    float energyLevel = MAXIMUM_ENERGY;
    [SerializeField] float payloadValue = 0.0f;
    public bool isInRange = false;
    public GameObject birdNest;
    private Transform birdNestLocation;
    public SpriteRenderer m_SpriteRenderer;
    float percentage = 0.0f;
    private Transform targetedBee;
    bool isResting = false;

    void Start()
    {
        animator.SetFloat("Energy", energyLevel);
    }

    void Awake()
    {
        birdNestLocation = birdNest.transform;
    }

    void Update()
    {
        Debug.Log("Energy " + energyLevel);
        birdMovement.Move();

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

        Flying();
        Resting();
        Eating();
        Chasing();

        animator.SetFloat("Energy", energyLevel);
    }

    public void Chasing()
    {
        // from https://answers.unity.com/questions/614524/find-objects-with-tag-in-distance-aoe-spell.html
        // from https://forum.unity.com/threads/how-to-make-one-object-move-to-another-object-by-tag.539979/
        GameObject[] bees = GameObject.FindGameObjectsWithTag("Bee");
        targetedBee = bees[Random.Range(0, bees.Length)].transform;

        foreach (GameObject target in bees)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance <= 5)
            {
                Debug.Log("Bird: Bee is in range.");
                isInRange = true;
                energyLevel -= 0.8f;
                animator.SetFloat("Energy", energyLevel);
                animator.SetBool("IsBeeInRange", true);
                float step = 5.0f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetedBee.position, step);
            }
            else
            {
                Debug.Log("Bird: Bee is not in range.");
                isInRange = false;
                animator.SetBool("IsBeeInRange", false);
                animator.SetFloat("Energy", energyLevel);
            }
        }
    }

    public void Eating()
    {
        if (beeState.isEaten == true)
        {
            energyLevel = MAXIMUM_ENERGY;
            animator.SetFloat("Energy", energyLevel);
            animator.SetBool("IsBeeCaught", true);
        }
        else
        {
            animator.SetBool("IsBeeCaught", false);
        }
    }

    public void Flying()
    {
        if (isResting == false)
        {
            energyLevel -= 0.4f;
            animator.SetFloat("Energy", energyLevel);
        }

    }

    public void Resting()
    {
        if (energyLevel <= MINIMUM_ENERGY)
        {
            Debug.Log("Bird: Resting");
            float step = 6.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, birdNestLocation.position, step);
            energyLevel += 0.3f;
            animator.SetFloat("Energy", energyLevel);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bird Nest")
        {
            Debug.Log("Bird: At bird nest.");
        }
    }

}
