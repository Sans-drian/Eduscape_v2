using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set;}
    public string connectionString = "Server=localhost;Database=eduscapeqdatabasetest;User=root;Password=;";
    public bool isDatabaseOn = true; //This boolean ensures that the database methods do not run if there is a problem connecting to the database.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevents destruction on scene change
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

}
