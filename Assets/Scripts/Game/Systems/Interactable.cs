using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    public UnityEvent NotifyPlayer;
    public UnityEvent DeNotifyPlayer;
    
    // Update is called once per frame
    void Update()
    {
        if(isInRange) //check if in range
        {
            if(Input.GetKeyDown(interactKey)) //and if player presses key
            {
                interactAction.Invoke(); //fire the event
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is now in range");
            NotifyPlayer.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is now not in range");
            DeNotifyPlayer.Invoke();
        }
    }
}
