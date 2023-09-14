using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

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
    public GameObject startInput;
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
    public GameObject fixation;

    //Experiment Variables
    public string participantID;
    public int numTrials;
    public int currentTrial = 0;
    public int condition;
    private bool foam = false;
    private bool seekingInput = false;
    private bool startTrial = false;
    private int trialLength = 20;

    //VR Action Variables
    public SteamVR_Action_Boolean clickAction;
    public SteamVR_Input_Sources targetSource;

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

        fixation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(seekingInput && (Input.GetKeyDown("space") || clickAction.GetStateDown(targetSource)))
        {
            seekingInput = false;

            if(currentTrial % 8 == 1 || currentTrial % 8 == 4 || currentTrial % 8 == 5)
            {
                StartCoroutine(startNormalTrial());
            }
            else if(currentTrial % 8 == 2 || currentTrial % 8 == 6 || currentTrial % 8 == 0)
            {
                StartCoroutine(startDarkTrial());
            }
            else
            {
                StartCoroutine(startDistortTrial());
            }
        }

        if (startTrial && (Input.GetKeyDown("space") || clickAction.GetStateDown(targetSource)))
        {
            startTrial = false;

            if (currentTrial % 8 == 1 || currentTrial % 8 == 4 || currentTrial % 8 == 5)
            {
                StartCoroutine(startNormalTrialCount());
            }
            else if (currentTrial % 8 == 2 || currentTrial % 8 == 6 || currentTrial % 8 == 0)
            {
                StartCoroutine(startDarkTrialCount());
            }
            else
            {
                StartCoroutine(startDistortTrialCount());
            }
        }
    }

    //Called once the start button is pressed
    public void startExperiment()
    {
        if(currentTrial == 0)
        {
            currentTrial = 1;
        }
        
        startButton.interactable = false;
        TMP_InputField inputField = startInput.GetComponent<TMP_InputField>();
        inputField.interactable = false;
        seekingInput = true;
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

    //Called on End Edit event of the trial start input field
    public void setTrialStart(string trials)
    {
        int num;

        TMP_InputField inputField = startInput.GetComponent<TMP_InputField>();
        string trialNums = inputField.text;
        bool success = Int32.TryParse(trialNums, out num);

        if (success)
        {
            currentTrial = num;
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

        fixation.SetActive(true);

        trialText.text = "Trial Number: " + currentTrial;

        if(currentTrial % 8 == 1)
        {
            conditionText.text = "Normal vision on solid ground--No Sound (#1)";
            condition = 1;
        }
        else if(currentTrial % 8 == 4)
        {
            conditionText.text = "Normal vision on solid ground--WITH SOUND (#4)";
            condition = 4;
        }
        else
        {
            conditionText.text = "Normal vision on foam--No Sound (#5)";
            condition = 5;
        }

        roomText.text = "Feel Free to Look Around";
        roomText2.text = "Feel Free to Look Around";

        yield return new WaitForSeconds(0.00000001f);
        startTrial = true;
    }

    private IEnumerator startNormalTrialCount()
    {
        roomText.text = "Please Look at Point. Trial starts in: 3";
        roomText2.text = "Please Look at Point. Trial starts in: 3";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look at Point. Trial starts in: 2";
        roomText2.text = "Please Look at Point. Trial starts in: 2";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look at Point. Trial starts in: 1";
        roomText2.text = "Please Look at Point. Trial starts in: 1";

        yield return new WaitForSeconds(1);

        roomText.text = "";
        roomText2.text = "";
        fixation.SetActive(false);
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

        if(currentTrial % 8 == 2)
        {
            conditionText.text = "Black screen on solid ground--No Sound (#2)";
            condition = 2;
        }
        else if(currentTrial % 8 == 6)
        {
            conditionText.text = "Black screen on foam--No Sound (#6)";
            condition = 6;
        }
        else
        {
            conditionText.text = "Black screen on foam--WITH SOUND (#8)";
            condition = 8;
        }

        darkText.text = "Feel Free to Look Around";
        darkText2.text = "Feel Free to Look Around";

        yield return new WaitForSeconds(0.00000001f);
        startTrial = true;
    }

    private IEnumerator startDarkTrialCount()
    {
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

        fixation.SetActive(true);

        trialText.text = "Trial Number: " + currentTrial;

        if(currentTrial % 8 == 3)
        {
            conditionText.text = "Head-fixed vision on solid ground--No Sound (#3)";
            foam = true;
            condition = 3;
        }
        else
        {
            conditionText.text = "Head-fixed vision on foam--No Sound (#7)";
            foam = false;
            condition = 7;
        }

        roomText.text = "Feel Free to Look Around";
        roomText2.text = "Feel Free to Look Around";

        yield return new WaitForSeconds(0.00000001f);
        startTrial = true;
    }

    private IEnumerator startDistortTrialCount()
    {
        roomText.text = "Please Look at Point. Trial starts in: 3";
        roomText2.text = "Please Look at Point. Trial starts in: 3";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look at Point. Trial starts in: 2";
        roomText2.text = "Please Look at Point. Trial starts in: 2";

        yield return new WaitForSeconds(1);

        roomText.text = "Please Look at Point. Trial starts in: 1";
        roomText2.text = "Please Look at Point. Trial starts in: 1";

        yield return new WaitForSeconds(1);

        roomText.text = "";
        roomText2.text = "";
        recording = true;
        distortWorld();

        yield return new WaitForSeconds(trialLength);
        recording = false;
        returnWorld();
        resetTrials();
    }

    //Makes the room move with the camera
    private void distortWorld()
    {
        fixation.SetActive(false);
        room.transform.SetParent(roomCamera.transform);
    }

    //Returns to a static camera
    private void returnWorld()
    {
        room.transform.SetParent(null);
    }
}
