using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitDoor : MonoBehaviour
{
    public bool isInteracting;
    public GameObject exitMenu;
    public PlayerMovement playerMovement;
    public PauseMenu pauseMenu;
    

    public void openExitMenu()
    {
        if (!pauseMenu.isPaused)
        {
            if(!isInteracting)
            {
                isInteracting = true;
                exitMenu.SetActive(true);

                playerMovement.GetComponent<PlayerMovement>().canMove = false;            
            }
            else if(isInteracting)
            {
                isInteracting = false;
                exitMenu.SetActive(false);
                
                playerMovement.GetComponent<PlayerMovement>().canMove = true;
            }
        }
        else
        {
            Debug.Log("Is in pause menu. Not able to interact");
        }
    }
}
