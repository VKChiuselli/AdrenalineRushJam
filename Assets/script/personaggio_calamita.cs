using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class personaggio_calamita : MonoBehaviour
{
    GameObject ogg_struttura_dati;
    struttura_dati script_struttura_dati;

    public float velocita_calamita_animazione = 2f;

    void Start()
    {
        ogg_struttura_dati = GameObject.Find("base_struttura");

        

        if (ogg_struttura_dati != null)
        {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }

    }



    


    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Moneta") {
            other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, transform.position, Time.deltaTime * velocita_calamita_animazione);


            float dis = Vector3.Distance(other.gameObject.transform.position, transform.position);

            if (dis < 3)
            {
                Destroy(other.gameObject);

                if (ogg_struttura_dati != null)
                {
                    script_struttura_dati.monete = script_struttura_dati.monete + 1;
                 //   monete_partita_corrente = monete_partita_corrente + 1;

                    Debug.Log(" script_struttura_dati.monete " + script_struttura_dati.monete);
                }
            }


        }
    }

}
