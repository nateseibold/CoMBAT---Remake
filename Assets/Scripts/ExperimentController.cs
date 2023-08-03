using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentController : MonoBehaviour
{
    //Cameras
    public Camera experimenterCamera;
    public Camera darknessTrialCamera;
    public Camera inBetweenCamera;

    //Can be used to stop the output file from recording
    public bool recording;

    // Start is called before the first frame update
    void Start()
    {
        recording = false;
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
}
