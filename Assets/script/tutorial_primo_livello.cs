using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_primo_livello : MonoBehaviour {

    public bool avvisato_mina_cratere;
    public bool avvisato_bonus_speed;
    public bool avvisato_raccogli_ammo;
    public bool avvisato_spara_ammo;
    public bool avvisato_barriera;
    public bool ammo_raccolta;
    float distanza_raycast_tutorial = 9f;

    struttura_dati script_struttura_dati;

    gioco_ruota_cilindro Gioco_ruota_cilindro;
    void Start() {

        avvisato_barriera = false;
        Gioco_ruota_cilindro = FindObjectOfType<gioco_ruota_cilindro>();



       GameObject ogg_struttura_dati = GameObject.Find("base_struttura");

        if (ogg_struttura_dati != null)
        {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }

        if (Gioco_ruota_cilindro != null)
        {
            if (script_struttura_dati.livello_in_uso > 2)
            {
                // this.gameObject.SetActive(false);
            }
        }



        if (script_struttura_dati.livello_in_uso == 1 || script_struttura_dati.livello_in_uso == 2) {//    if (Gioco_ruota_cilindro.script_struttura_dati.livello_in_uso == 1)
            Gioco_ruota_cilindro.tutorial_in_corso = true;
            Gioco_ruota_cilindro.numero_spari = 0;
        }

    }

    void Update() {

        if (script_struttura_dati.livello_in_uso <= 2)
        {//    if (Gioco_ruota_cilindro.script_struttura_dati.livello_in_uso == 1)


            if (Gioco_ruota_cilindro.tutorial_in_corso || ammo_raccolta)
            {
                gestione_collisione_mina();
            }

            if (script_struttura_dati.livello_in_uso == 1)
            {
                gestione_collisione_bonus_speed();
            }

            if (!Gioco_ruota_cilindro.tutorial_in_corso)
            {
                ResumeGame();
            }

        }

    }
    private void gestione_collisione_bonus_speed() {
        RaycastHit hit_collider;


        Vector3 pos = Gioco_ruota_cilindro.astronave.transform.position;


        Vector3 pos_astronave = new Vector3(pos.x, pos.y, pos.z + 1.2f);

        if (Physics.Raycast(pos_astronave, new Vector3(0, -1, 0), out hit_collider, 1)) {

            if (hit_collider.collider.name.IndexOf("bonus0_") > -1) {
                string str_bonus = hit_collider.collider.name;

                if (!avvisato_bonus_speed) {
                    avvisato_bonus_speed = true;
                    Time.timeScale = 0;
                    tutorial_bonus_speed();
                }

                str_bonus = str_bonus.Replace("bonus0_", "");

                int num_bonus = int.Parse(str_bonus);



            }
        }

    }

    void gestione_collisione_mina() {

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

        Vector3 punto_coll = new Vector3(0, 0, 0);

        for (int n = 0; n <= 7; n++) {

            pos = Gioco_ruota_cilindro.astronave.transform.position + pos_direction[n];


            Debug.DrawRay(pos, pos_ray_direction[n], new Color(1, n * .2f, 0, 1));

            {
                if (Physics.Raycast(pos, pos_ray_direction[n], out hit_collider, distanza_raycast_tutorial)) {

                    float dis = hit_collider.distance;
                    if (script_struttura_dati.livello_in_uso == 1) {
                        if (!avvisato_mina_cratere) {
                            Time.timeScale = 0;
                            avvisato_mina_cratere = true;
                            Gioco_ruota_cilindro.tutorial_in_corso = false;
                            tutorial_comandi_e_mine();
                        }
                    }


                    if (script_struttura_dati.livello_in_uso == 2) {
                        if (!avvisato_raccogli_ammo) {
                            distanza_raycast_tutorial = 3f;
                            Time.timeScale = 0;
                            avvisato_raccogli_ammo = true;
                            Gioco_ruota_cilindro.tutorial_in_corso = false;
                            tutorial_raccogli_ammo();

                        }
                    }


                    if (script_struttura_dati.livello_in_uso == 2 && Gioco_ruota_cilindro.numero_spari > 0) {
                        if (!avvisato_spara_ammo) {
                            avvisato_spara_ammo = true;
                            StartCoroutine(ShootingTime());
                        }
                    }




                    if (dis < distanza_direction[n]) {


                        if (hit_collider.collider.name.IndexOf("blocco") > -1) {



                            string str_block = "" + hit_collider.collider.name;

                        }

                        if (hit_collider.collider.name.IndexOf("bonus1_") > -1) {
                            ammo_raccolta = true;

                            Debug.Log("TRIGGERASR TUTORIAL BONUS SPEED");

                        }

                    }

                }

            }

        }

    }

    private IEnumerator ShootingTime() {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        Gioco_ruota_cilindro.tutorial_in_corso = false;
        tutorial_sparare();
    }

    private void tutorial_comandi_e_mine() {
        Debug.Log("tutorial_comandi_e_mine");
        Gioco_ruota_cilindro.crea_tutorial("Dodge mines and craters tapping left or right");

    }
    private void tutorial_raccogli_ammo() {
        Debug.Log("tutorial_raccogli_ammo");
        Gioco_ruota_cilindro.crea_tutorial("Pick Missiles up to gain ammo");
    }

    private void tutorial_sparare() {
        Debug.Log("tutorial_sparare");
        Gioco_ruota_cilindro.crea_tutorial("ou can destroy obstacles by shooting at them. Tap the center of the screen to shoot!");
    }

    private void tutorial_bonus_speed() {
        Debug.Log("tutorial_bonus_speed");
        Gioco_ruota_cilindro.crea_tutorial("You will find a variety of boosts in every level, this one speeds you up!");
    }
    private void tutorial_bonus_magnete() {
        Debug.Log("tutorial_bonus_magnete");
        Gioco_ruota_cilindro.crea_tutorial("This magnet allows you to grab all the coins far away from you!");
    }
    private void tutorial_bonus_barriera() {
        Debug.Log("tutorial_bonus_barriera");
        Gioco_ruota_cilindro.crea_tutorial("Buying this barrier will soak your first hit against walls and mines!");
    }

    private void ResumeGame() {

        if (Gioco_ruota_cilindro.crea_popup_finale == 0)
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) || (Gioco_ruota_cilindro.touch_x[0] > 0))
            { //TODO implemenmtare i comandi touch in ascolto
                Time.timeScale = 1;
                Gioco_ruota_cilindro.distruggi_menu_tutorial();
                if (avvisato_raccogli_ammo)
                {
                    ammo_raccolta = true;
                }
            }

        }



    }

}
