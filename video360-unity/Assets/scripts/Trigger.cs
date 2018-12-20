using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    private Vector3 lastPos;

    // Use this for initialization
    void Start()
    {
        lastPos = transform.position;
    }

    public void ClickFunction()
    {
        Debug.Log("Clicked.");
        if( -1 <= lastPos.z && lastPos.z <= 1)
        {
            lastPos = new Vector3(0, 0, 51);
        }
        else
        {
            lastPos = new Vector3(0, 0, 0);
        }
        /*lastPos = new Vector3(0, 0, 51);*/
        transform.position = lastPos;
    }

    // Update is called once per frame
    void Update()
    {
        int fingerCount = 0;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");
            ClickFunction();
        }
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                fingerCount++;

        }
        if (fingerCount > 0)
            print("User has " + fingerCount + " finger(s) touching the screen");
    }
}
