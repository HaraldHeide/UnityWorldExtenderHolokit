using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WebApiGet))]
public class InstantiateDevices : MonoBehaviour
{
    public GameObject deviceObject;

    private WebApiGet webApiGet;

    void Awake ()
    {
        webApiGet = GetComponent<WebApiGet>();
	}

    protected void OnEnable()
    {
        webApiGet.DevicesRefreshed += WebApiGet_DevicesRefreshed;
    }

    protected void OnDisable()
    {
        webApiGet.DevicesRefreshed -= WebApiGet_DevicesRefreshed;
    }

    private void WebApiGet_DevicesRefreshed(object sender, WebApiGet.DevicesEventArgs e)
    {
        foreach (var device in webApiGet.Devices)
        {
            GameObject myGO = GameObject.Find(device.Name);
            if (myGO == null)
            {
                myGO = Instantiate(deviceObject, new Vector3((float)device.X, (float)device.Y, (float)device.Z), Quaternion.identity);
                myGO.name = device.Name;
            }
            myGO.GetComponentInChildren<TextMesh>().text = "Temperature: " + device.Temperature.ToString() +
                                                            "\nHumidity: " + device.Humidity.ToString() +
                                                            "\nPressure: " + device.Pressure.ToString();
        }
        //foreach (var device in webApiGet.Devices)
        //{
        //    string t = device.Temperature.ToString();
        //    deviceObject.GetComponentInChildren<TextMesh>().text = $"Temp: {t}";
        //}
    }

}
