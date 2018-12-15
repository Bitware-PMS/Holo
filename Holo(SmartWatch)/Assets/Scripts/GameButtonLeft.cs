using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameButtonRight : MonoBehaviour {

    public GameObject mainScript;

    bool ispressed = false;
   

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ispressed)
            mainScript.GetComponent<Menu>().client.send("left");
	}

    public void holdButton()
    {
        ispressed = true;
    }

    public void releaseButton()
    {
        ispressed = false;
    }
}
