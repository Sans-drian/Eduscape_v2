using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitDoor : MonoBehaviour
{
    public bool isInteracting;
    public GameObject exitMenu;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openExitMenu()
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
}
