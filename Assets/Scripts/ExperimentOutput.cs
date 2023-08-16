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

        string header = "Time Since Start (in seconds),Participant ID,Trial Number,Condition Number,Head Position x,Head Position y,Head Position z";
        StreamWriter writer8 = new StreamWriter(m_Path, true);
        writer8.WriteLine(header);
        writer8.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<ExperimentController>().recording)
        {
            //Data
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            double cur_time = (double)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
            cur_time = cur_time * 1000;
            
            string id = GetComponent<ExperimentController>().participantID;
            int trial = GetComponent<ExperimentController>().currentTrial;
            int condition = GetComponent<ExperimentController>().condition;
            float x = playerHead.transform.position.x;
            float y = playerHead.transform.position.y;
            float z = playerHead.transform.position.z;

            dataTracked = Time.time + "," + id + "," + trial + "," + condition + "," + x + "," + y + "," + z;

            StreamWriter writer8 = new StreamWriter(m_Path, true);
            writer8.WriteLine(dataTracked);
            writer8.Close();
        }
    }
}
