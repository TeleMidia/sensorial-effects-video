using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class VideoControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    //
    public void Recenter()
    {
        Debug.Log("Recentering!");
        UnityEngine.XR.InputTracking.Recenter();
        //transform.Rotate(0, 0, 0);
    }

    public IEnumerator Turnitoff()
    {
        string tifWebURL = "http://192.168.1.100:8080/api/tif";
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");

        Debug.Log("Restart Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnityWebRequest www = UnityWebRequest.Put(tifWebURL, myData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
