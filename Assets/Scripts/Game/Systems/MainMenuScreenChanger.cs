using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreenChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject playMenu;
    [SerializeField]
    private GameObject howToPlayMenu;
    [SerializeField]
    private GameObject connStringMenu;


    //  FROM THE MAIN MENU ===================================
    public void pressedPlay()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void pressedHTPlay()
    {
        mainMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
    }

    public void pressedCSDev()
    {
        mainMenu.SetActive(false);
        connStringMenu.SetActive(true);
    }
    // ========================================================



    // FROM THE PLAY MENU =====================================
    public void pressedBackPMenu()
    {
        playMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    // ========================================================



    // FROM THE HOW TO PLAY MENU ==============================
    public void pressedBackHTPMenu()
    {
        howToPlayMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    // ========================================================


    // FROM THE CONNECTION STRING (DEV) MENU ==================
    public void pressedBackCSDevMenu()
    {
        connStringMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    // ========================================================



}
