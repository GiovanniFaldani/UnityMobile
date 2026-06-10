using TMPro;
using UnityEngine;

public class Interruptions : MonoBehaviour
{
    [SerializeField] TMP_Text connectionTXT, focusTXT, pauseTXT;
    int appLostFocus, appInPause;
    bool appStarted;
    TouchScreenKeyboard keyboard;

    private void Start()
    {
        appStarted = true;
    }

    private void Update()
    {
        // NetworkReachability --> ReachableViaCarrierDataNetwork, ReachableViaLocalAreaNetwork
        if (Application.internetReachability == NetworkReachability.NotReachable)
            connectionTXT.text = "Offline";
        else
            connectionTXT.text = "Online";
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!appStarted) return;
        if (!focus)
        {
            appLostFocus++;
        }
        focusTXT.text = "Ho perso il focus " + appLostFocus + " volte";
    }

    private void OnApplicationPause(bool pause)
    {
        // di solito qui si mette il timescale a 0 se si è in pause e a 1 se si è in !pause
        // AudioListener.pause = true; // se in pausa
        if (pause)
            appInPause++;

        pauseTXT.text = "Sono entrato in pausa " + appInPause + " volte";
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Save");
    }

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Scrivi qualcosa");
    }
}
