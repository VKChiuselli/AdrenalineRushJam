using System;
using UnityEngine;
using UnityEngine.UI;


public class timer_reward : MonoBehaviour {
   float msToWait;
    Button ClickButton;
    public ulong lastTimeClicked;

    private void Start() {
        msToWait = 10000f;
        ClickButton = gameObject.GetComponent<Button>();


        if (PlayerPrefs.HasKey("LastTimeClicked")) {
            lastTimeClicked = ulong.Parse(PlayerPrefs.GetString("LastTimeClicked"));
        }
        else {
            msToWait = 300f;
            lastTimeClicked = (ulong)DateTime.Now.Ticks;
            PlayerPrefs.SetString("LastTimeClicked", lastTimeClicked.ToString());
        }

        if (!Ready())
            ClickButton.interactable = false;
    }

    private void Update() {
        if (!ClickButton.IsInteractable()) {
            if (Ready()) {
                ClickButton.interactable = true;
                return;
            }
        }
    }


    public void Click() {
        lastTimeClicked = (ulong)DateTime.Now.Ticks;
        PlayerPrefs.SetString("LastTimeClicked", lastTimeClicked.ToString());
        ClickButton.interactable = false;
    }

    private bool Ready() {
        ulong diff = ((ulong)DateTime.Now.Ticks - lastTimeClicked);
        ulong m = diff / TimeSpan.TicksPerMillisecond;

        float secondsLeft = (float)(msToWait - m) / 1000.0f;

        if (secondsLeft < 0) {
          //  TODO azione quando viene cliccato
            return true;
        }

        return false;
    }

    public void SetTimeToWait(float timeAmountInMs) {
        msToWait = timeAmountInMs;
    }

}



