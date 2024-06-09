using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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
