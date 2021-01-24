using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
This class outputs to the screen a message depending on if a bee is in range of a bird, a bee has been eaten by a bird or if a bee is not in range of a bird.
*/
public class ChaseController : MonoBehaviour
{

    public BeeState beeState;
    public BirdState birdState;
    public Text chaseText;

    void Update()
    {
        if (birdState.isInRange == true)
            chaseText.text = "A bee is in range of a bird.";
        else if (beeState.isEaten == true)
            chaseText.text = "A bee has been eaten by a bird.";
        else
            chaseText.text = "A bee is not in range of a bird.";
    }

}
