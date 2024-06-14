using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Script for player manager stuff

    [SerializeField]
    private GameObject interactIcon;

    public void NotifyPlayer()
    {
        interactIcon.SetActive(true);
    }

    public void DeNotifyPlayer()
    {
        interactIcon.SetActive(false);
    }
}
