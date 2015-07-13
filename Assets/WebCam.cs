using UnityEngine;
using System.Collections;
using System.IO;

public class WebCam : MonoBehaviour {
	
	public string deviceName;
	WebCamTexture tex;

    ServerSocket server = null;

    // Use this for initialization
    IEnumerator Start ()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //屏幕常亮
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
	    if(Application.HasUserAuthorization(UserAuthorization.WebCam))
	    {
	    	WebCamDevice[] devices = WebCamTexture.devices;
	    	deviceName = devices[0].name;
	    	tex = new WebCamTexture(deviceName, 960, 720, 30);
	    	GetComponent<Renderer>().material.mainTexture = tex;
	    	tex.Play();
	    }

        server = new ServerSocket();
    }

	void Update ()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

    void onGUI()
    {
        //Texture2D tex = server.getTexture();
        //GetComponent<Renderer>().material.mainTexture = tex;
    }

    void onDestroy()
    {
        Debug.Log("Thread closed");
        server.close();
    }

    void onApplicationQuit()
    {
        Debug.Log("Thread closed");
        server.close();
    }
}





