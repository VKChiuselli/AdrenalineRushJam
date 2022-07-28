using System;
using UnityEngine;
using UnityEngine.UI;


public class timer_reward : MonoBehaviour {
   float msToWait;
    Button ClickButton;
    public ulong LastTimeClick90;

    private void Start() {

        if (gameObject.name== "pulsante_text31") {
        msToWait = 43200000f;
        }
        else {
            msToWait = 10800000f;
        }
        ClickButton = gameObject.GetComponent<Button>();


        if (PlayerPrefs.HasKey($"LastTimeClick{gameObject.name}X")) {
            LastTimeClick90 = ulong.Parse(PlayerPrefs.GetString($"LastTimeClick{gameObject.name}X"));
            if (!Ready())
                ClickButton.interactable = false;
        }
        else {
                   LastTimeClick90 = 0;
            ClickButton.interactable = true;
            PlayerPrefs.SetString($"LastTimeClick{gameObject.name}X", LastTimeClick90.ToString());
        }

       
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
        LastTimeClick90 = (ulong)DateTime.Now.Ticks;
        PlayerPrefs.SetString($"LastTimeClick{gameObject.name}X", LastTimeClick90.ToString());
        ClickButton.interactable = false;
    }

    private bool Ready() {
        if (PlayerPrefs.HasKey($"LastTimeClick{gameObject.name}X")) {
            ulong diff = ((ulong)DateTime.Now.Ticks - LastTimeClick90);
            ulong m = diff / TimeSpan.TicksPerMillisecond;

            float secondsLeft = (float)(msToWait - m) / 1000.0f;

            if (secondsLeft < 0) {
                //  TODO azione quando viene cliccato
                return true;
            }

            return false;
        }
        else {
        return true;
        }
     
    }

    public void SetTimeToWait(float timeAmountInMs) {
        msToWait = timeAmountInMs;
    }
    public int GetTimeLeft() {
        ulong diff = ((ulong)DateTime.Now.Ticks - LastTimeClick90);
        ulong m = diff / TimeSpan.TicksPerMillisecond;

        float secondsLeft = (float)(msToWait - m) / 1000.0f;
        return (int)secondsLeft;
    }

}



