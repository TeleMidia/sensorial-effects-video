using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

static class Consts
{
    public const double smellStartTime = 33;//35 ;
    public const double smellStopTime = 36;//38; // stop "spray"
    public const string smellWebURL = "http://192.168.1.100:8080/api/smell";

    //old -30?
    //public const double airStart = 27;
    //public const double airSmallStop = 63;
    //public const double airSmallBack = 67;
    //public const double airStop = 96;
    public const double airStart = 22;//24; //flowormotor++
    public const double servomotor1a = 28;
    public const double servomotor0a = 32;
    public const double servomotor1b = 35;
    public const double servomotor0b = 42;
    public const double servomotor1c = 46;
    public const double airSmallStop = 58;
    public const double servomotor0c = 60;
    public const double airSmallBack = 62;
    public const double airStop = 90;

    public const double shark1 = 30;
    public const double shark2 = 30.5;
    public const double shark3 = 31;
    public const double shark4 = 31.5;

    public const double surf1 = 10;

    public const string airWebURL = "http://192.168.1.100:8080/api/air";

    public const string servoWebURL = "http://192.168.1.100:8080/api/servomotor";

    public const string servo360WebURL = "http://192.168.1.100:8080/api/servomotor360";

    public const string playStopURL = "http://192.168.1.100:8080/api/playstop";

    public const string rumble = "http://192.168.1.104:8080"; //the server waiting a post to rumble

    //public const string fileName = "/CSVclicks.txt";
}

public class NetworkSenses : MonoBehaviour
{

    //private bool toggledSmell = false;
    private VideoPlayer[] videos; //Taken from this gameObject components, each with a defined videoclip
    private int vid; //index of currently playing video component 
    private List<List<double>> pilhaTempos;
    public VideoControl transformScript;
    //private string filePath;
    private int flowormotor;

    Renderer sphereRenderer;
    Shader flippedsphereshader;

    // Use this for initialization
    void Start()
    {
        pilhaTempos = new List<List<double>>
        {
            new List<double>(),
            new List<double>(),
            new List<double>(),
            new List<double>()
        };

        pilhaTempos[0].Add(Consts.smellStartTime);
        pilhaTempos[0].Add(Consts.smellStopTime);

        pilhaTempos[1].Add(Consts.airStart); //0
        pilhaTempos[1].Add(Consts.servomotor1a);
        pilhaTempos[1].Add(Consts.servomotor0a);
        pilhaTempos[1].Add(Consts.servomotor1b);
        pilhaTempos[1].Add(Consts.servomotor0b);
        pilhaTempos[1].Add(Consts.servomotor1c);
        pilhaTempos[1].Add(Consts.airSmallStop); //6
        pilhaTempos[1].Add(Consts.servomotor0c);
        pilhaTempos[1].Add(Consts.airSmallBack); //8
        pilhaTempos[1].Add(Consts.airStop); //9

        pilhaTempos[2].Add(Consts.shark1);
        pilhaTempos[2].Add(Consts.shark2);
        pilhaTempos[2].Add(Consts.shark3);
        pilhaTempos[2].Add(Consts.shark4);

        pilhaTempos[3].Add(Consts.surf1);

        videos = this.GetComponents<VideoPlayer>();
        vid = 3;
        videos[0].loopPointReached += ChangeVid;
        videos[1].loopPointReached += ChangeVid;
        videos[2].loopPointReached += ChangeVid;
        videos[3].loopPointReached += ChangeVid;
        //video.clip

        //filePath = Application.persistentDataPath + Consts.fileName;

        //videos[vid].Play();
        sphereRenderer = GetComponent<Renderer>();
        flippedsphereshader = sphereRenderer.material.shader;

        StartCoroutine(StartVideo());
    }

    IEnumerator UploadSense(string senseURL)
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        Debug.Log("Uploading sense!");
        UnityWebRequest www = UnityWebRequest.Put(senseURL, myData);
        //UnityWebRequest www = UnityWebRequest.Post(senseURL, "");
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

    IEnumerator UploadSensePost(string senseURL)
    {
        Debug.Log("Uploading sense!");
        //UnityWebRequest www = UnityWebRequest.Put(senseURL, myData);
        UnityWebRequest www = UnityWebRequest.Post(senseURL, "");
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

    IEnumerator StartVideo()
    {
        UnityWebRequest www = UnityWebRequest.Get(Consts.playStopURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            flowormotor = 0;
            System.Threading.Thread.Sleep(2000);
            StartCoroutine(StartVideo());
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text.Contains("1"))
            {
                sphereRenderer.material.shader = flippedsphereshader;
                transformScript.Recenter();
                //videos[vid].playbackSpeed = 10;//debug purposes
                videos[vid].Play();
                //File.AppendAllText(filePath, System.Environment.NewLine + "init," + System.DateTime.Now.ToString() + "," + "video" + vid);
                Debug.Log("Play Video");
                if (vid == 3)//when started with 0 : if (vid == 0) 
                {
                    StartCoroutine(VerifyServo());
                    StartCoroutine(VerifySceneRestart());
                }
            }
            else
            {
                flowormotor = 0;
                System.Threading.Thread.Sleep(2000);
                StartCoroutine(StartVideo());
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    IEnumerator VerifyServo()
    {
        UnityWebRequest www = UnityWebRequest.Get(Consts.servoWebURL);
        yield return www.SendWebRequest();

        if (www.downloadHandler.text.Contains("1"))
        {
            //System.Threading.Thread.Sleep(2000);
            StartCoroutine(UploadSense(Consts.servoWebURL));
        }
    }

    IEnumerator VerifySceneRestart()
    {
        UnityWebRequest www = UnityWebRequest.Get(Consts.playStopURL);
        yield return www.SendWebRequest();

        if (www.downloadHandler.text.Contains("1"))
        {
            //System.Threading.Thread.Sleep(2000);
            StartCoroutine(VerifySceneRestart());
        }
        else
        {
            StartCoroutine(transformScript.Turnitoff());
        }

    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(StartVideo());
    }

    // loopPointReached -> video ended
    void ChangeVid(VideoPlayer vp)
    {
        if (vid > 0)
        {
            Debug.Log("changing vid");
            videos[vid].Stop();

            sphereRenderer.material.shader = Shader.Find("Standard");
            vid = vid - 1;

            if (vid == 2)//shark in video has to be at the front
            {
                transform.Rotate(0, 0, 0);
            }
            else
            {
                transform.Rotate(0, -90, 0); //center of video aligned to camera
            }

            StartCoroutine(Waiter());
        }
        else
        {
            this.gameObject.SetActive(false);
            //sphereRenderer.material.shader = Shader.Find("Standard");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pilhaTempos[vid].Count > 0)
        {
            if (videos[vid].time > pilhaTempos[vid][0])
            {
                pilhaTempos[vid].RemoveAt(0);
                if (vid == 0) //on first video
                {
                    //StartCoroutine(UploadSmell());
                    StartCoroutine(UploadSense(Consts.smellWebURL));
                }
                else
                {
                    if (vid == 1)
                    {
                        if (flowormotor == 0 || flowormotor == 6 || flowormotor == 8 || flowormotor == 9)
                            StartCoroutine(UploadSense(Consts.airWebURL));
                        else
                        {
                            StartCoroutine(UploadSense(Consts.servoWebURL));
                        }

                        flowormotor++;
                    }
                    else
                    {
                        if (vid == 2)
                        {
                            Handheld.Vibrate();
                            StartCoroutine(UploadSensePost(Consts.rumble));
                        }
                        else//(vid == 3)
                        {
                            StartCoroutine(UploadSense(Consts.servo360WebURL));
                        }
                    }
                }
            }
        }

        if (Input.anyKeyDown)
        {
            //Debug.Log(vid+" : "+videos[vid].time);
            //Debug.Log(System.DateTime.Now.Hour.ToString());
            //File.AppendAllText(filePath, ","+videos[vid].time);
            //StartCoroutine(transformScript.Turnitoff());
        }

    }
}
