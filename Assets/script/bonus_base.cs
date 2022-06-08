using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonus_base : MonoBehaviour
{

    struttura_dati script_struttura_dati;

    float rotazione;

    // Start is called before the first frame update
    void Start()
    {

      GameObject ogg_struttura_dati = GameObject.Find("base_struttura");

        if (ogg_struttura_dati != null)
        {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }



    }

    // Update is called once per frame
    void Update()
    {

        rotazione = rotazione + 120 *Time.deltaTime;
        if (rotazione > 360)
        {
            rotazione = 0;
        }


        transform.localEulerAngles = new Vector3(0, rotazione, 0);





    }
}
