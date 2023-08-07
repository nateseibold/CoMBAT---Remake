using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperimentController : MonoBehaviour
{
    //Cameras
    public Camera experimenterCamera;
    public Camera darknessTrialCamera;
    public Camera inBetweenCamera;

    //UI Variables
    public GameObject experimenterCanvas;
    public GameObject darknessTrialCanvas;
    public GameObject inBetweenCanvas;
    public GameObject trialLengthInput;
    public GameObject partIDInput;

    //Can be used to stop the output file from recording
    [HideInInspector]
    public bool recording;

    //Experiment Variables
    public string participantID;
    public int numTrials;

    // Start is called before the first frame update
    void Start()
    {
        recording = false;

        darknessTrialCamera.enabled = false;
        inBetweenCamera.enabled = false;
        darknessTrialCanvas.SetActive(false);
        inBetweenCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called once the start button is pressed
    public void startExperiment()
    {
        recording = true;
    }

    //Called on End Edit event of the ID input field
    public void setID(string id)
    {
        TMP_InputField inputField = partIDInput.GetComponent<TMP_InputField>();
        participantID = inputField.text;
    }

    //Called on End Edit event of the trial number input field
    public void setNumTrials(string trials)
    {
        int num;

        TMP_InputField inputField = trialLengthInput.GetComponent<TMP_InputField>();
        string trialNums = inputField.text;
        bool success = Int32.TryParse(trialNums, out num);

        if(success)
        {
            numTrials = num;
        }
        else
        {
            inputField.text = "Enter an Integer";
        }
    }
}
