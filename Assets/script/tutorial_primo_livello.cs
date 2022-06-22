using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_primo_livello : MonoBehaviour {

    public bool avvisato_mina_cratere;
    public bool avvisato_bonus_speed;
    float distanza_raycast_tutorial = 21f;

    gioco_ruota_cilindro Gioco_ruota_cilindro;
    void Start() {


        Gioco_ruota_cilindro = FindObjectOfType<gioco_ruota_cilindro>();
        if (Gioco_ruota_cilindro != null)
        //    if (Gioco_ruota_cilindro.script_struttura_dati.livello_in_uso == 1)
                Gioco_ruota_cilindro.tutorial_in_corso = true;

    }

    void Update() {
        gestione_collisione();
    }


    void gestione_collisione() {

        RaycastHit hit_collider;

        Vector3[] pos_ray_direction = new Vector3[10];
        float[] distanza_direction = new float[10];

        Vector3 pos;

        Vector3[] pos_direction = new Vector3[10];

        pos_direction[0] = new Vector3(0, 0, 0);
        pos_direction[1] = new Vector3(0, 0, -1);
        pos_direction[2] = new Vector3(0, 0, -1);

        pos_direction[3] = new Vector3(-.9f, 0, -.8f);
        pos_direction[4] = new Vector3(.9f, 0, -.8f);

        pos_direction[5] = new Vector3(0, 0, -.5f);
        pos_direction[6] = new Vector3(0, 0, -.5f);

        pos_direction[7] = new Vector3(0, -.5f, 0);


        pos_ray_direction[0] = new Vector3(0, 0, 2.6f);
        pos_ray_direction[1] = new Vector3(-1.5f, 0, .25f);
        pos_ray_direction[2] = new Vector3(1.5f, 0, .25f);
        pos_ray_direction[3] = new Vector3(0, 0, 2.6f);
        pos_ray_direction[4] = new Vector3(0, 0, 2.6f);
        pos_ray_direction[5] = new Vector3(-1.5f, 0, .75f);
        pos_ray_direction[6] = new Vector3(1.5f, 0, .75f);
        pos_ray_direction[7] = new Vector3(0, 0, 2.6f);

        distanza_direction[0] = 2.6f;
        distanza_direction[1] = 1.5f;
        distanza_direction[2] = 1.5f;

        distanza_direction[3] = 2.6f;
        distanza_direction[4] = 2.6f;

        distanza_direction[5] = 1.5f;
        distanza_direction[6] = 1.5f;
        distanza_direction[7] = 2.6f;

        int numero_coll = 0;
        Vector3 punto_coll = new Vector3(0, 0, 0);

        int num_block = -1;

        for (int n = 0; n <= 7; n++) {

            pos = Gioco_ruota_cilindro.astronave.transform.position + pos_direction[n];


            Debug.DrawRay(pos, pos_ray_direction[n], new Color(1, n * .2f, 0, 1));


            if (Physics.Raycast(pos, pos_ray_direction[n], out hit_collider, distanza_raycast_tutorial)) {

                float dis = hit_collider.distance;


                if (!avvisato_mina_cratere) {
                    Time.timeScale = 0;
                    tutorial_comandi_e_mine();
                    avvisato_mina_cratere = true;
                }



                if (dis < distanza_direction[n]) {


                    if (hit_collider.collider.name.IndexOf("blocco") > -1) {



                        string str_block = "" + hit_collider.collider.name;

                    }

                    if (hit_collider.collider.name.IndexOf("bonus1_") > -1) {

                        string str_bonus = "" + hit_collider.collider.name;

                        //   Debug.Log("" + str_block);

                        str_bonus = str_bonus.Replace("bonus1_", "");

                        int num_bonus = int.Parse(str_bonus);


                    }

                }

            }

            if (num_block > -1 && numero_coll > 0) {


                Vector3 punto_coll_uso = punto_coll / numero_coll;



            }

        }


    }

    private void tutorial_comandi_e_mine() {
        Debug.Log("primo tutorial iniziato");
        Gioco_ruota_cilindro.crea_tutorial_primo();
    }
}
