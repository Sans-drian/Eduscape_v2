using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class getInput : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField;
    //[SerializeField]
    //TextMeshProUGUI resultText;
    
    public readDatabaseQList readDatabaseQList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetInput()
    {
        string input = inputField.text;
        readDatabaseQList.fileName = input;

        readDatabaseQList.createQuestionList();

    }
}
