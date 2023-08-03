using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExperimentOutput : MonoBehaviour
{
    public bool printBool = false;
    public GameObject playerHead;
    public GameObject pointer;
    public string dataTracked;
    public string m_Path;
    public string m_Path2;
    public string application_Path;

    // Start is called before the first frame update
    void Start()
    {
        application_Path = Application.dataPath;
        application_Path = application_Path + "/"+"headData.txt";
        m_Path = application_Path;

        string header = "Time,Subject ID,Trial Number,Trial Type,Actual Travel Time,Perceived Travel Time,Actual Start Point,Participant Start Point,Actual End Point,Participant End Point,Distance betweeen Actual and Participant Start,Distance between Actual and Participant End";
        StreamWriter writer8 = new StreamWriter(m_Path, true);
        writer8.WriteLine(header);
        writer8.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<ExperimentController>().recording)
        {

        }
    }
}
