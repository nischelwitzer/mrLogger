using DMT;
using System.Net;
using UnityEngine;

public class QuestAppInfos : MonoBehaviour
{
    DMTMQTTSenderSmall myMQTTSender;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMQTTSender = GetComponent<DMTMQTTSenderSmall>();

        Invoke("SendMQTT", 10.0f);
        ShowInfos();
    }

    private void SendMQTT()
    {
        // myMQTTSender.SendPublish("habitat/debug/info", "buttonBpressed");
        myMQTTSender.SendPublish("habitat/mr/helloIP", GetLocalIPAddress().ToString());
        myMQTTSender.SendPublish("habitat/mr/helloSID", GetSystemID().ToString());
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            Debug.Log(MakeDebugString());
            SendMQTT();
        }
    }

    private string MakeDebugString()
    {
        string debugString = "\n";
        debugString += "##### ********************************************************************************\n";
        debugString += "***** ================================================================= InfoApp ======\n";
        debugString += "***** System Device-ID: " + SystemInfo.deviceUniqueIdentifier + "\n";
        debugString += "***** System OperatingSys: " + SystemInfo.operatingSystem + "\n";
        debugString += "***** System Memory: " + SystemInfo.systemMemorySize + "\n";
        debugString += "***** System Graphics: " + SystemInfo.graphicsDeviceName + "\n";
        debugString += "***** System Device Name: " + SystemInfo.deviceName + "\n";
        debugString += "***** System Device Model: " + SystemInfo.deviceModel + "\n";
        debugString += "##### --------------------------------------------------------------------------------\n";
        debugString += "***** Application Name: " + Application.productName + "\n";
        debugString += "***** Application Version: " + Application.version + "\n";
        debugString += "***** Application Platform: " + Application.platform + "\n";
        debugString += "***** Application URL: " + Application.absoluteURL + "\n";
        debugString += "***** Application ID: " + Application.cloudProjectId + "\n";
        debugString += "***** Application Language: " + Application.systemLanguage + "\n";
        debugString += "##### ================================================================================\n";
        debugString += "##### IP-Adresse: " + GetLocalIPAddress() + "\n";
        debugString += "##### MAC-Adresse: " + GetMacAddress() + "\n";
        debugString += "##### System ID (ANDROID_ID): " + GetSystemID() + "\n";
        debugString += "##### ################################################################################\n";
        debugString += "##### ===========================================================================NIS==\n";
        return debugString;
    }

    void ShowInfos()
    {
        Debug.Log("##### ********************************************************************************");
        Debug.Log("***** ================================================================= InfoApp ======");
        Debug.Log("***** System Device-ID: " + SystemInfo.deviceUniqueIdentifier);
        Debug.Log("***** System OperatingSys: " + SystemInfo.operatingSystem);
        Debug.Log("***** System Memory: " + SystemInfo.systemMemorySize);
        Debug.Log("***** System Graphics: " + SystemInfo.graphicsDeviceName);
        Debug.Log("***** System Device Name: " + SystemInfo.deviceName);
        Debug.Log("***** System Device Model: " + SystemInfo.deviceModel);
        Debug.Log("***** --------------------------------------------------------------------------------");
        Debug.Log("***** Application Name: " + Application.productName);
        Debug.Log("***** Application Version: " + Application.version);
        Debug.Log("***** Application Platform: " + Application.platform);
        Debug.Log("***** Application URL: " + Application.absoluteURL);
        Debug.Log("***** Application ID: " + Application.cloudProjectId);
        Debug.Log("***** Application Language: " + Application.systemLanguage);
        Debug.Log("***** ================================================================================");

        if (Application.systemLanguage == SystemLanguage.German)
            Debug.Log("##### System Langage: " + Application.systemLanguage + " -- Platform: "
                + Application.platform + " -- Sys: " + SystemInfo.operatingSystem);
        else
            Debug.LogWarning("##### System Language (NOT GERMAY): " + Application.systemLanguage
                 + " -- Platform: " + Application.platform + " -- Sys: " + SystemInfo.operatingSystem);

        int startLog = PlayerPrefs.GetInt("startLog", 0);
        Debug.Log("##### ApplicationManager: startLog Counter (prefab) " + startLog + " [regedit: " + Application.companyName + "]");
        Debug.Log("##### Screen Info >> " + Screen.width + " x " + Screen.height + " -- Orient: " + Screen.orientation +

           " -- MonitorRes: " + Screen.currentResolution + " -- FullScr: " + Screen.fullScreen);

        Debug.Log("##### IP-Adresse: " + GetLocalIPAddress());
        Debug.Log("##### MAC-Adresse: " + GetMacAddress());
        Debug.Log("##### System ID (ANDROID_ID): " + GetSystemID());
        Debug.Log("##### ################################################################################");
        Debug.Log("##### ===========================================================================NIS==");

    }

    public string GetLocalIPAddress()
    {
        string localIP = "Nicht verfügbar";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    public string GetMacAddress()
    {
        string macAddress = "Nicht verfügbar";
        try
        {
            AndroidJavaClass wifiManagerClass = new AndroidJavaClass("android.net.wifi.WifiManager");
            AndroidJavaObject wifiManager = new AndroidJavaObject("android.content.Context", "WIFI_SERVICE");
            AndroidJavaObject wifiInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo");
            macAddress = wifiInfo.Call<string>("getMacAddress");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Abrufen der MAC-Adresse: " + e.Message);
        }
        return macAddress;
    }

    public string GetSystemID()
    {
        string systemID = "Nicht verfügbar";

        try
        {
            using (AndroidJavaClass settingsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                                                     .GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject contentResolver = activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                systemID = settingsSecure.CallStatic<string>("getString", contentResolver, "android_id");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Abrufen der System ID: " + e.Message);
        }

        return systemID;
    }

}
