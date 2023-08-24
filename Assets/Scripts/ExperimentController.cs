using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;

public class ExperimentController : MonoBehaviour
{
    //Cameras
    public Camera experimenterCamera;
    public Camera darknessTrialCamera;
    public Camera darknessTrialCamera2;
    public Camera inBetweenCamera;
    public Camera inBetweenCamera2;
    public Camera roomCamera;
    public Camera roomCamera2;

    //UI Variables
    public GameObject experimenterCanvas;
    public GameObject darknessTrialCanvas;
    public GameObject inBetweenCanvas;
    public GameObject darknessTrialCanvas2;
    public GameObject inBetweenCanvas2;
    public GameObject trialLengthInput;
    public GameObject partIDInput;
    public TMP_Text trialText;
    public TMP_Text conditionText;
    public TMP_Text inBetweenText;
    public TMP_Text inBetweenText2;
    public TMP_Text roomText;
    public TMP_Text roomText2;
    public TMP_Text darkText;
    public TMP_Text darkText2;
    public Button startButton;

    //Can be used to stop the output file from recording
    [HideInInspector]
    public bool recording;

    //Room Variable
    public GameObject room;

    //Experiment Variables
    public string participantID;
    public int numTrials;
    public int currentTrial = 0;
    public int condition;
    private bool foam = false;
    private bool seekingInput = false;
    private int trialLength = 20;
    private bool distort = false;
    private Quaternion rotates;

    // Start is called before the first frame update
    void Start()
    {
        recording = false;

        roomCamera.enabled = false;
        roomCamera2.enabled = false;
        darknessTrialCamera.enabled = false;
        darknessTrialCamera2.enabled = false;
        inBetweenCamera.enabled = true;
        inBetweenCamera2.enabled = true;
        darknessTrialCanvas.SetActive(false);
        inBetweenCanvas.SetActive(true);
        darknessTrialCanvas2.SetActive(false);
        inBetweenCanvas2.SetActive(true);

        rotates = room.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(seekingInput && Input.GetKeyDown("space"))
        {
            seekingInput = false;

            if(currentTrial % 6 == 1 || currentTrial % 6 == 4)
            {
                StartCoroutine(startNormalTrial());
            }
            else if(currentTrial % 6 == 2 || currentTrial % 6 == 5)
            {
                StartCoroutine(startDarkTrial());
            }
            else
            {
                StartCoroutine(startDistortTrial());
            }
        }

        if (distort)
        {
            room.transform.position = roomCamera.transform.position;
        }
    }

    void LateUpdate()
    {
        if(distort)
        {
           
        }
    }

    //Called once the start button is pressed
    public void startExperiment()
    {
        currentTrial = 1;
        startButton.interactable = false;
        StartCoroutine(startNormalTrial());
    }

    //Called on End Edit event of the ID input field
    public void setID(string id)
    {
        TMP_InputField inputField = partIDInput.GetComponent<TMP_InputField>();
        participantID = inputField.text;

        if(participantID != "")
        {
            inputField.interactable = false;
            this.GetComponent<ExperimentOutput>().enabled = true;
        }
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
            inputField.interactable = false;
        }
        else
        {
            inputField.text = "Enter an Integer";
        }
    }

    //Called after the alloted time for each trial is up
    private void resetTrials()
    {
        roomCamera.enabled = false;
        roomCamera2.enabled = false;
        darknessTrialCamera.enabled = false;
        darknessTrialCamera2.enabled = false;
        inBetweenCamera.enabled = true;
        inBetweenCamera2.enabled = true;
        inBetweenCanvas.SetActive(true);
        darknessTrialCanvas.SetActive(false);
        darknessTrialCanvas2.SetActive(false);
        inBetweenCanvas2.SetActive(true);

        currentTrial++;
        if(currentTrial > numTrials)
        {
            seekingInput = false;
            inBetweenText.text = "Experiment Over";
            inBetweenText2.text = "Experiment Over";
        }
        else
        {
            seekingInput = true;
        }
    }

    //Begins Conditions 1 and 4
    private IEnumerator startNormalTrial()
    {
        roomCamera.enabled = true;
        roomCamera2.enabled = true;
        darknessTrialCamera.enabled = false;
        darknessTrialCamera2.enabled = false;
        inBetweenCamera.enabled = false;
        inBetweenCamera2.enabled = false;
        inBetweenCanvas.SetActive(false);
        inBetweenCanvas2.SetActive(false);

        trialText.text = "Trial Number: " + currentTrial;

        if(!foam)
        {
            conditionText.text = "Condition Number: 1";
            condition = 1;
        }
        else
        {
            conditionText.text = "Condition Number: 4";
            condition = 4;
        }

        roomText.text = "Feel Free to Look Around";
        roomText2.text = "Feel Free to Look Around";

        yield return new WaitForSeconds(7);

        roomText.text = "Please Look Forward. Trial starts in: 3";
        roomText2.text = "Please Look Forward. Trial starts in: 3";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look Forward. Trial starts in: 2";
        roomText2.text = "Please Look Forward. Trial starts in: 2";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look Forward. Trial starts in: 1";
        roomText2.text = "Please Look Forward. Trial starts in: 1";

        yield return new WaitForSeconds(1);

        roomText.text = "";
        roomText2.text = "";
        recording = true;

        yield return new WaitForSeconds(trialLength);
        recording = false;
        resetTrials();
    }

    //Begins Conditions 2 and 5
    private IEnumerator startDarkTrial()
    {
        roomCamera.enabled = false;
        roomCamera2.enabled = false;
        darknessTrialCamera.enabled = true;
        darknessTrialCamera2.enabled = true;
        inBetweenCamera.enabled = false;
        inBetweenCamera2.enabled = false;
        inBetweenCanvas.SetActive(false);
        inBetweenCanvas2.SetActive(false);
        darknessTrialCanvas.SetActive(true);
        darknessTrialCanvas2.SetActive(true);

        room.SetActive(false);

        trialText.text = "Trial Number: " + currentTrial;

        if(!foam)
        {
            conditionText.text = "Condition Number: 2";
            condition = 2;
        }
        else
        {
            conditionText.text = "Condition Number: 5";
            condition = 5;
        }

        darkText.text = "Feel Free to Look Around";
        darkText2.text = "Feel Free to Look Around";

        yield return new WaitForSeconds(7);

        darkText.text = "Please Look Forward. Trial starts in: 3";
        darkText2.text = "Please Look Forward. Trial starts in: 3";

        yield return new WaitForSeconds(1);

        darkText.text = "Please Look Forward. Trial starts in: 2";
        darkText2.text = "Please Look Forward. Trial starts in: 2";

        yield return new WaitForSeconds(1);

        darkText.text = "Please Look Forward. Trial starts in: 1";
        darkText2.text = "Please Look Forward. Trial starts in: 1";

        yield return new WaitForSeconds(1);

        darkText.text = "";
        darkText2.text = "";
        recording = true;

        yield return new WaitForSeconds(trialLength);
        recording = false;
        room.SetActive(true);
        resetTrials();
    }

    //Begins Conditions 3 and 6
    private IEnumerator startDistortTrial()
    {
        roomCamera.enabled = true;
        roomCamera2.enabled = true;
        darknessTrialCamera.enabled = false;
        darknessTrialCamera2.enabled = false;
        inBetweenCamera.enabled = false;
        inBetweenCamera2.enabled = false;
        inBetweenCanvas.SetActive(false);
        inBetweenCanvas2.SetActive(false);

        distortWorld();

        trialText.text = "Trial Number: " + currentTrial;

        if(!foam)
        {
            conditionText.text = "Condition Number: 3";
            foam = true;
            condition = 3;
        }
        else
        {
            conditionText.text = "Condition Number: 6";
            foam = false;
            condition = 6;
        }

        roomText.text = "Feel Free to Look Around";
        roomText2.text = "Feel Free to Look Around";

        yield return new WaitForSeconds(7);

        roomText.text = "Please Look Forward. Trial starts in: 3";
        roomText2.text = "Please Look Forward. Trial starts in: 3";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look Forward. Trial starts in: 2";
        roomText2.text = "Please Look Forward. Trial starts in: 2";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look Forward. Trial starts in: 1";
        roomText2.text = "Please Look Forward. Trial starts in: 1";

        yield return new WaitForSeconds(1);

        roomText.text = "";
        roomText2.text = "";
        recording = true;

        yield return new WaitForSeconds(trialLength);
        recording = false;
        returnWorld();
        resetTrials();
    }

    //Makes the room move with the camera
    private void distortWorld()
    {
        //room.transform.SetParent(roomCamera.transform);
        distort = true;
    }

    //Returns to a static camera
    private void returnWorld()
    {
        //room.transform.SetParent(null);
        distort = false;
    }
}
