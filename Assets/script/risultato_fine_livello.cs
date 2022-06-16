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

   

}
