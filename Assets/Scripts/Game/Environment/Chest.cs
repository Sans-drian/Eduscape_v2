using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    //this script holds some data for each chest in the game.

    public bool isAnswered = false;
    public Animator animator;

    public void openChest()
    {
        if(!isAnswered)
        {
            animator.SetBool("isAnswered", isAnswered);
        }
    }

}
