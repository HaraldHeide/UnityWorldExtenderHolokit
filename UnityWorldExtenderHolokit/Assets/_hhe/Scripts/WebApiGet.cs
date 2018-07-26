/*
 * get from webAPI
 *  https://www.youtube.com/watch?v=9rPJeRF7S_8
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebApiGet : MonoBehaviour
{
    [SerializeField]
    private string url = "http://192.168.0.2/api/devices";

    #region Events C#
    public struct DevicesEventArgs
    {
        public DateTime LastRefreshed; 
    }
    public delegate void DevicesEventHandler(object sender, DevicesEventArgs e);
    public event DevicesEventHandler DevicesRefreshed;
    #endregion Events C#

    public RootObject[] Devices;

    float timeRunning = 0.0f;
    void Start ()
    {
	}

    public void Update()


    {
        timeRunning += Time.deltaTime;
        if(timeRunning > 2.0f)
        {
            StartCoroutine(GetDevices());
            timeRunning = 0.0f;
        }
    }
    IEnumerator GetDevices()
    {
        //string url = "http://172.31.104.145:666/api/devices"; // hjemme Get-5G-A25110
        //string url = "http://192.168.0.8/api/devices";
        //string url = "http://192.168.0.2/api/devices"; //work
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();
            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                string jsonResult = www.downloadHandler.text;
                Debug.Log(jsonResult);
                Devices = JsonHelpers.getjsonArray<RootObject>(jsonResult);

                #region Events C#
                DevicesEventArgs e = new DevicesEventArgs();
                e.LastRefreshed = DateTime.Now;
                if(DevicesRefreshed != null)
                {
                    DevicesRefreshed(this, e);
                }
                #endregion Events C#
            }
        }
    }
}
