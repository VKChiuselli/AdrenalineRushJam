using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotazione_oggetto_x : MonoBehaviour
{

    float rotazione;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        rotazione = rotazione + 120 *Time.deltaTime;
        if (rotazione > 360)
        {
            rotazione = 0;
        }


        transform.localEulerAngles = new Vector3(-90,rotazione, 0);


    }
}
