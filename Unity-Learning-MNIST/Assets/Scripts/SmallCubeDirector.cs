using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCubeDirector : MonoBehaviour
{
    // Fungus
    public Fungus.Flowchart flowchart = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFungus(string message){
        flowchart.SendFungusMessage(message);
    }
    public void StartFungusFromEventPlane(){
        flowchart.SendFungusMessage("start_question");
    }
}
