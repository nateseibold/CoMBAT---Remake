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
    public Camera roomCamera;

    //UI Variables
    public GameObject experimenterCanvas;
    public GameObject darknessTrialCanvas;
    public GameObject inBetweenCanvas;
    public GameObject trialLengthInput;
    public GameObject partIDInput;
    public TMP_Text trialText;
    public TMP_Text conditionText;

    //Can be used to stop the output file from recording
    [HideInInspector]
    public bool recording;

    //Experiment Variables
    public string participantID;
    public int numTrials;
    public int currentTrial = 0;
    private bool foam = false;

    // Start is called before the first frame update
    void Start()
    {
        recording = false;

        roomCamera.enabled = false;
        darknessTrialCamera.enabled = false;
        inBetweenCamera.enabled = true;
        darknessTrialCanvas.SetActive(false);
        inBetweenCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called once the start button is pressed
    public void startExperiment()
    {
        currentTrial = 1;
        StartCoroutine(startNormalTrial());
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

    private void resetTrials()
    {
        roomCamera.enabled = false;
        darknessTrialCamera.enabled = false;
        inBetweenCamera.enabled = true;
        inBetweenCanvas.SetActive(true);
    }

    private IEnumerator startNormalTrial()
    {
        roomCamera.enabled = true;
        darknessTrialCamera.enabled = false;
        inBetweenCamera.enabled = false;
        inBetweenCanvas.SetActive(false);

        trialText.text = "Trial Number: " + currentTrial;

        if(!foam)
            conditionText.text = "Condition Number: 1";
        else
            conditionText.text = "Condition Number: 4";

        recording = true;

        yield return new WaitForSeconds(15);
        recording = false;
        resetTrials();
    }
}
