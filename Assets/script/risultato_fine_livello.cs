using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class risultato_fine_livello : MonoBehaviour {

    GameObject personaggio;

    public int StellaFineLivello;
    public int StellaDanni;
    public int SogliaDanniDaSuperare=20;
    public int StellaMoneta;
    public int SogliaMoneteDaSuperare=20;

    void Start() {
        personaggio = GameObject.Find("personaggio");
    }

    void Update() {

    }

    public void calcolo_stelle() {
        StellaFineLivello = 1;
        if (personaggio.GetComponent<gioco_ruota_cilindro>().energia > SogliaDanniDaSuperare) {
            StellaDanni = 1;
        }
        if (personaggio.GetComponent<gioco_ruota_cilindro>().monete_partita_corrente > SogliaMoneteDaSuperare) {
            StellaMoneta = 1;
        }
        //TODO creare evento con i risultati
    }

    //creare un collider in fondo al tracciato
    private void OnTriggerEnter(Collider other) {
      if(  other.gameObject.tag == "Player") {
            calcolo_stelle();
        }
    }

}
