using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battle_pass_manager : MonoBehaviour
{
    struttura_dati script_struttura_dati;

    void Start() {

        GameObject ogg_struttura_dati = GameObject.Find("base_struttura");

        if (ogg_struttura_dati != null) {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }

    }




}
