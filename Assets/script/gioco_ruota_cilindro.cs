using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEditor;
using System.IO;

public class gioco_ruota_cilindro : MonoBehaviour {

    string path_server = "https://www.mnzgame.com/AstroVortex/testi/";

#if UNITY_EDITOR
    [SerializeField]
#endif
    public classe_save c_save;


#if UNITY_EDITOR
    [SerializeField]
#endif
    public classe_save_parametri c_save_p;


    public TextAsset level_json;

    float xm, ym, xm_old, ym_old;

    public float variabile_x;
    public float variabile_y;

    public float[] touch_x = new float[15];
    public float[] touch_y = new float[15];

    float[] touch_rx = new float[15];
    float[] touch_ry = new float[15];

    float[] touch_xo = new float[15];
    float[] touch_yo = new float[15];

    public float potenza_tasto = 1;
    public float velocita_personaggio = 1;
    public float aumento_velo = 1;

    public float velocita_bonus = 1;

    public int inversione_camera = 1;

    public float velocita_sparo = 20;
    public float velocita_sparo_boss = 20;



    public GameObject astronave;
    public GameObject astronave_barriera;

    public GameObject boss_mesh;
    public GameObject boss_mesh_sparo;

    public float spostamento_boss_rotazione = .01f;

    public Color newColor = new Color(0, 1, 1, 1);


    public float distanza_disolve = -15;

    public bool online_dati = true;
    public bool tutorial_in_corso;

    public int monete_partita_corrente = 0;
    public int gemme_partita_corrente = 0;

    public int ok_energia_completa = 0;
    public int ok_spari_completi = 0;

    public float attivo_tempo_sparo_personaggio_base = 1.0f;


    float monete_UI = 0;
    float gemme_UI = 0;


    float risoluzione_x;
    float risoluzione_y;

    float rapporto_risoluzione;
    float spostamento_sx;
    float spostamento_sx2;

    float diff_xm;
    public float diff_ym;



    float pressione_tasto = 0;

    static int max_ui = 300;

    GameObject[] pulsante = new GameObject[max_ui];
    GameObject[] pulsante_testo = new GameObject[max_ui];

    GameObject[] grafica = new GameObject[max_ui];
    GameObject[] grafica_testo = new GameObject[max_ui];

    GameObject[] pulsante_field = new GameObject[max_ui];
    GameObject[] pulsante_field_testo = new GameObject[max_ui];
    GameObject[] pulsante_field_testo2 = new GameObject[max_ui];


    struttura_dati script_struttura_dati;

    GameObject canvas;
    GameObject canvas_popup;
    GameObject canvas_tutorial;

    GameObject ogg_struttura_dati;


    GameObject cilindro;
    GameObject sfondo;
    GameObject sfondo2;

    public GUISkin skin_scrittura;


    float rotazione_cilindro = 0;

    float blocco_velocita = 1;

    float inversione_controllo = 0;


    GameObject cam0;
    Vector3 cam_pos, cam_rot;

    Vector3[] vertici_cilindro = new Vector3[12000];

    Vector3 playerVelocity = new Vector3(0, 0, 0);

    float astronave_rx = 0;
    float astronave_rx_calcolo = 0;

    float astronave_ry = 0;
    float astronave_rz = 0;
    float astronave_rz_calcolo = 0;

    GameObject[] snap_rif = new GameObject[3000];

    int crea_snap = 0;

    float distanza_corda = 0;



    GameObject boss;
    float boss_rad;

    float[] attivo_tempo_sparo_boss = new float[20];

    GameObject[] sparo_boss = new GameObject[20];

    float[] sparo_boss_rad = new float[20];

    GameObject[] sparo = new GameObject[20];
    float[] attivo_tempo_sparo = new float[20];

    float attivo_tempo_sparo_personaggio = 0;
    float attivo_tempo_sparo_boss2 = 0;

    float spostamento_z = 1;
    float spostamento_z2 = 1;

    int font_size = 15;

    int gemme_prese = 0;

    int attivo_popup = 0;

    float resetTimerPopup = 0;

    float[] grafica_tempo = new float[25];

    float[] grafica_dime = new float[25];
    float[] grafica_pos = new float[25];

    public int crea_popup_finale = 0;

    float crea_popup_finale_tempo = 1;

    // upgrade

    float agilita = 1;

    float tempo_barriera = 3;
    float energia = 100;
    float energia_base = 100;
    public int numero_spari = 3;
    int numero_spari_base = 0;

    float scafo = 0;
    float calamita = 0;

    float attiva_barriera = 0;

    float tempo_fire_work = 0;

    int che_livello = 1;

    bool visione_boss = false;

    float sposta_uv_bonus = 0;

    int controllo_mobile = 0;

    int carica_dati_online = 0;
    int carica_dati_livello_online = 0;

    int inizio_game = 0;
    float tempo_inizio_livello = 1.5f;

    float spostamento_ui_verticale = .75f;

    float potenziometro_touch = 0;

    int[] boss_potere_attivo = new int[5];
    float[] boss_potere = new float[5];

    float[] boss_potere_rad = new float[5];
    float[] boss_potere_altezza = new float[5];


    GameObject[] boss_potere_mesh = new GameObject[5];

    float attivo_tempo_potere_boss = 0;



    AudioSource[] effetto_source_UI = new AudioSource[40];
    int[] effetto_source_UI_caricato = new int[40];

    AudioSource musica;

    int ok_fine_livello = 0;

    float fade_intro = 1;


    // Start is called before the first frame update
    void Start() {

        controllo_risoluzione();


        cilindro = GameObject.Find("cilindro_esatto");
        sfondo = GameObject.Find("sfondo");
        sfondo2 = GameObject.Find("sfondo2");

        boss = GameObject.Find("boss");


        cam0 = GameObject.Find("Main Camera");

        carica_effetto_UI(1, "audio/Prefabs/click");
        carica_effetto_UI(2, "audio/Prefabs/shot");
        carica_effetto_UI(3, "audio/Prefabs/impact");
        carica_effetto_UI(4, "audio/Prefabs/explosion");

        carica_effetto_UI(5, "audio/Prefabs/success");
        carica_effetto_UI(6, "audio/Prefabs/lose");
        carica_effetto_UI(7, "audio/Prefabs/monete");
        carica_effetto_UI(8, "audio/Prefabs/preso_bonus");



        carica_musica("audio/Prefabs/musicaGioco");

        leggi_vertici_cilindro();



        for (int k = 0; k <= 9; k++) {

            for (int n = 0; n < 40; n++) {

                snap_rif[n + k * 50] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere_rif", typeof(GameObject))) as GameObject;

                snap_rif[n + k * 50].name = "snap_rif " + n;

            }

        }







        canvas = GameObject.Find("Canvas");
        canvas_popup = GameObject.Find("Canvas_popup/Panel");
        canvas_tutorial = GameObject.Find("Canvas_tutorial");

        canvas_popup.SetActive(false);
        canvas_tutorial.SetActive(false);


        ogg_struttura_dati = GameObject.Find("base_struttura");

        if (ogg_struttura_dati != null) {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }


        cam_pos = new Vector3(0, 1.35f, 13.0f);
        cam_rot = new Vector3(15, 180, 0);


        crea_menu();

        inizializza_personaggio();

        //  save_parameter();

        StartCoroutine(leggi_dati_online("parametri"));


    }


    void inizializza_personaggio() {
        // agilit� 0
        // barriera 1
        // energia 2
        // spari 3
        // scafo 4
        // calamita 5


        agilita = 1;

        tempo_barriera = 2;
        energia = 100;


        numero_spari = 3;

        scafo = 1.0f;
        calamita = 1;



        if (ogg_struttura_dati != null) {

            agilita = 1.0f + script_struttura_dati.livello_upgrade[1] * .1f;

            tempo_barriera = 2.0f + script_struttura_dati.livello_upgrade[2] * .25f;

            energia = 100.0f + script_struttura_dati.livello_upgrade[3] * 10;

            numero_spari = 3 + script_struttura_dati.livello_upgrade[4];

            scafo = 1.0f + script_struttura_dati.livello_upgrade[5] * .1f;

            calamita = 1.0f + script_struttura_dati.livello_upgrade[6] * .05f;


        }

        energia_base = energia;
        numero_spari_base = numero_spari;

    }



    void analisi_energia(float riduttore) {
        if (crea_popup_finale == 0) {
            if (attiva_barriera < 0) {
                energia = energia - riduttore / scafo;

                Debug.Log("energia " + energia);

                attiva_barriera = tempo_barriera*.5f;
            }
        }


    }

    void Update() {





        if (carica_dati_online == 1 && carica_dati_livello_online == 1) {

            update_game();

        }
        else { 
            aggiorna_menu_offline();
        }

    }


    void update_game() {

        aggiorna_audio();

        controllo_risoluzione();

        controllo();

        controllo_monete();

        gestione_camera();

        gestione_boss();

        aggiorna_blocco();

        gestione_cilindro();

        gestione_collisione();

        gestione_coll_special_bonus_malus();

        gestione_fine_gioco();

        aggiorna_menu();

        aggiorna_menu_popup();

        aggiorna_menu_tutorial();

        aggiorna_potere_boss();


#if UNITY_EDITOR

        if (Input.GetKeyUp(KeyCode.M)) {

            crea_menu();

        }


        if (Input.GetKeyUp(KeyCode.Alpha2)) {

            crea_potere_boss();

        }



#endif

    }



    void leggi_vertici_cilindro() {



        Mesh mesh = cilindro.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for (var i = 0; i < vertices.Length; i++) {
            vertici_cilindro[i] = vertices[i];
        }


    }


    void scala_cilindro() {



        Mesh mesh = cilindro.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;


        for (var i = 0; i < vertices.Length; i++) {
            vertices[i] = vertici_cilindro[i] * c_save.crea_cilindro[0].raggio;
        }

        mesh.vertices = vertices;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();


        DestroyImmediate(cilindro.GetComponent<MeshCollider>());


        cilindro.AddComponent<MeshCollider>();


    }


    void crea_sparo() {


        Debug.Log("sparo creato ");

        numero_spari = numero_spari - 1;


        int num_sparo = -1;

        for (int n = 0; n < 5; n++) {

            if (attivo_tempo_sparo[n] < -1) {
                num_sparo = n;

                attivo_tempo_sparo[n] = 3.5f;


                sparo[n] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sparo_personaggio", typeof(GameObject))) as GameObject;

                Vector3 pos_astronave = astronave.transform.position;

                sparo[n].transform.position = new Vector3(pos_astronave.x, pos_astronave.y-.5f, pos_astronave.z + 2);

                suona_effetto_UI(2, .7f);

                    sparo[n].name = "sparo_personaggio " + n;

                attivo_tempo_sparo_personaggio = attivo_tempo_sparo_personaggio_base;

                n = 1000;


            }

        }


    }



    void controllo_monete() {

        int num_monete = c_save.crea_moneta.Count;

        Vector3 pos_astronave = astronave.transform.position;
        pos_astronave.z += pos_astronave.z + 2.0f;


        float distanza = 6 * calamita;
        float distanza_cattura = .75f;

        float velocita_calamita_animazione = velocita_personaggio * 1.5f;


        for (int k = 0; k < num_monete; k++) {

            if (c_save.crea_moneta[k].presa == 0) {
                float dis = Vector3.Distance(pos_astronave, c_save.crea_moneta[k].mesh.transform.position);

                if (dis < distanza) {
                    c_save.crea_moneta[k].presa = 1;
                }

            }

            if (c_save.crea_moneta[k].presa == 1) {

                c_save.crea_moneta[k].mesh.transform.position = Vector3.Lerp(c_save.crea_moneta[k].mesh.transform.position, pos_astronave, Time.deltaTime * velocita_calamita_animazione);


                float dis = Vector3.Distance(pos_astronave, c_save.crea_moneta[k].mesh.transform.position);

                if (dis < distanza_cattura) {
                    c_save.crea_moneta[k].presa = 2;

                    Destroy(c_save.crea_moneta[k].mesh);

                    monete_partita_corrente = monete_partita_corrente + 1;

                    suona_effetto_UI(7, .35f);


                    if (ogg_struttura_dati != null) {
                        script_struttura_dati.monete = script_struttura_dati.monete + 1;


                        Debug.Log(" script_struttura_dati.monete " + script_struttura_dati.monete);
                    }

                }

            }

        }

    }



    void controllo() {

        float parametro_touch = c_save_p.crea_parametri[0].posizione_touch;

        int livello_uso = 1;

        if (script_struttura_dati != null) {

            livello_uso = script_struttura_dati.livello_in_uso;
        }


        if (livello_uso == 1 || livello_uso == 2) { //TODO da sostituire con script_struttura_dati
            potenza_tasto = -200;
        }
        else {
            potenza_tasto = c_save_p.crea_parametri[0].potenza_tasto;
        }


        attiva_barriera = attiva_barriera - Time.deltaTime;


        if (attiva_barriera < 0) {
            attiva_barriera = -.001f;
        }


        astronave_barriera.GetComponent<Renderer>().material.SetColor("_Color", new Color32(118, 255, 200, (byte)(attiva_barriera * 85)));



        if (energia < 0 || crea_popup_finale == 1) {
            blocco_velocita = blocco_velocita * .9f;

            if (blocco_velocita < .05f) {
                blocco_velocita = 0;

                if (energia <= 0) {

                    if (crea_popup_finale == 0) {
                        crea_popup_finale = 10;
                        crea_popup(2);
                    }


                }


            }

        }



        astronave.transform.position = new Vector3(0, c_save.crea_cilindro[0].raggio + 1.0f, 0);



        if (!tutorial_in_corso && attivo_popup == 0) {

            float pressione_tasto_up = Input.GetAxis("Vertical");


            if (controllo_mobile == 1 && touch_x[0] > risoluzione_x * parametro_touch && touch_x[0] < risoluzione_x * (1 - parametro_touch)) {
                if (Mathf.Abs(diff_ym) < risoluzione_y / c_save_p.crea_parametri[0].touch_sparo_differenza) {

                    pressione_tasto_up = -1;
                }
            }



            attivo_tempo_sparo_personaggio = attivo_tempo_sparo_personaggio - Time.deltaTime;


            if (pressione_tasto_up != 0 && attivo_tempo_sparo_personaggio < 0 && numero_spari > 0 && blocco_velocita > .99f) {

                //   Debug.Log("attivo_tempo_sparo_personaggio"+attivo_tempo_sparo_personaggio);

                crea_sparo();

            }



            if (controllo_mobile == 0) {
                pressione_tasto = Input.GetAxis("Horizontal");

            }

            if (controllo_mobile == 1) {




                if (touch_x[0] > 0 && touch_x[0] < risoluzione_x * (parametro_touch)) {

                    if (pressione_tasto > 0) {
                        pressione_tasto = 0;
                    }

                    pressione_tasto = pressione_tasto - c_save_p.crea_parametri[0].potenziometro_touch * Time.deltaTime;




                }

                if (touch_x[0] > risoluzione_x * (1.0f - parametro_touch)) {


                    if (pressione_tasto < 0) {
                        pressione_tasto = 0;
                    }

                    pressione_tasto = pressione_tasto + c_save_p.crea_parametri[0].potenziometro_touch * Time.deltaTime;



                }


                if (touch_x[0] <= 0) {
                    potenziometro_touch = 0;
                    pressione_tasto = 0;
                }



                if (pressione_tasto > 1) {
                    pressione_tasto = 1;
                }

                if (pressione_tasto < -1) {
                    pressione_tasto = -1;
                }


            }

            rotazione_cilindro = 0;

            if (Mathf.Abs(pressione_tasto) > 0 && blocco_velocita > .99f) {

                int molt_inversione = -1;

                if (visione_boss == true) {
                    molt_inversione = 1;

                }



                rotazione_cilindro = pressione_tasto * potenza_tasto * Time.deltaTime * molt_inversione * agilita;


            }


            int molt_rotazione_z = 1;

            if (visione_boss == true) {
                molt_rotazione_z = -1;

            }


            astronave_rz_calcolo = astronave_rz_calcolo - pressione_tasto * molt_rotazione_z;



            if (astronave_rz_calcolo > 30) {
                astronave_rz_calcolo = 30;

            }

            if (astronave_rz_calcolo < -30) {
                astronave_rz_calcolo = -30;

            }


            boss_rad = boss_rad + rotazione_cilindro * spostamento_boss_rotazione;


            for (int n = 0; n < 5; n++) {

                if (sparo_boss[n] != null) {

                    sparo_boss_rad[n] = sparo_boss_rad[n] + rotazione_cilindro * spostamento_boss_rotazione;

                }

            }


            cilindro.transform.Rotate(new Vector3(0, 0, rotazione_cilindro));




            float astronave_ry_calcolo = 0;



            if (crea_popup_finale == 1) {
                astronave_ry_calcolo = 20;
            }

            astronave_ry = Mathf.LerpAngle(astronave_ry, astronave_ry_calcolo, Time.deltaTime * 3);

            astronave_rz = Mathf.LerpAngle(astronave_rz, astronave_rz_calcolo, Time.deltaTime * 15);



            astronave.transform.localEulerAngles = new Vector3(0, astronave_ry, astronave_rz);

            astronave_rz_calcolo = astronave_rz_calcolo * .9f;




            gestione_sparo();


        }




    }



    void OnGUI() {

        //    GUI.skin = skin_scrittura;

        //    GUI.Label( new Rect(0,0,600,70), "pressione_tasto " + pressione_tasto+ " attiva_barriera " + attiva_barriera); 



    }



    void gestione_sparo() {


        for (int n = 0; n < 5; n++) {
            attivo_tempo_sparo[n] = attivo_tempo_sparo[n] - Time.deltaTime;

            if (sparo[n] != null) {


                if (attivo_tempo_sparo[n] > 0) {


                    sparo[n].transform.Translate(new Vector3(0, 0, velocita_sparo * Time.deltaTime));

                    gestione_collisione_sparo(n, sparo[n]);

                }
                else {
                    DestroyImmediate(sparo[n]);
                }

            }

        }

    }



    void gestione_sparo_boss() {

        for (int n = 0; n < 10; n++) {
            attivo_tempo_sparo_boss[n] = attivo_tempo_sparo_boss[n] - Time.deltaTime;



            if (sparo_boss[n] != null) {


                if (attivo_tempo_sparo_boss[n] > 0) {

                    float xx = Mathf.Sin(sparo_boss_rad[n]) * (c_save.crea_cilindro[0].raggio + 1);
                    float yy = Mathf.Cos(sparo_boss_rad[n]) * (c_save.crea_cilindro[0].raggio + 1);

                    float zz = sparo_boss[n].transform.position.z;

                    sparo_boss[n].transform.position = new Vector3(xx, yy, zz + velocita_sparo_boss * Time.deltaTime);

                    //  sparo_boss[n].transform.Translate(new Vector3(0, 0, velocita_sparo_boss * Time.deltaTime));

                    //  sparo_boss[n].transform

                    float px = sparo_boss[n].transform.position.x - astronave.transform.position.x;
                    float py = sparo_boss[n].transform.position.y - astronave.transform.position.y;




                    gestione_collisione_sparo(n, sparo_boss[n], 1);






                }
                else {
                    DestroyImmediate(sparo_boss[n]);
                }

            }

        }

    }




    void gestione_camera() {





        float altezza_cam = 3.5f;


        if (visione_boss == false) {
            float sposta_z = -6;
            if (crea_popup_finale == 1) {
                sposta_z = -8;
            }



            cam_pos.z = Mathf.Lerp(cam_pos.z, sposta_z, Time.deltaTime * 5);

            cam_rot.y = Mathf.Lerp(cam_rot.y, 0, Time.deltaTime * 50);

        }

        if (visione_boss == true) {
            float sposta_z = 6;

            if (crea_popup_finale == 1) {
                sposta_z = 8;
            }




            cam_pos.z = Mathf.Lerp(cam_pos.z, sposta_z, Time.deltaTime * 5);



            cam_rot.y = Mathf.Lerp(cam_rot.y, 180, Time.deltaTime * 50);

            altezza_cam = 3;

        }


        cam0.transform.localPosition = new Vector3(0, c_save.crea_cilindro[0].raggio + altezza_cam, cam_pos.z);
        cam0.transform.localEulerAngles = cam_rot;

    }





    void gestione_cilindro() {

        if (inizio_game == 1) {

            int livello_uso = 3;

            if (script_struttura_dati != null) {

                livello_uso = script_struttura_dati.livello_in_uso;

            }






            if (livello_uso == 1 || livello_uso == 2) { //TODO da sostituire con script_struttura_dati
                velocita_personaggio = 20;
            }
            else {
                velocita_personaggio = c_save_p.crea_parametri[0].velocita_personaggio;

            }

            float velo_popup = 1;

            if (attivo_popup > 0) {
                velo_popup = 0;
            }

            cilindro.transform.Translate(new Vector3(0, 0, -velocita_personaggio * Time.deltaTime * velo_popup * blocco_velocita * velocita_bonus * aumento_velo));

        }

    }




    void crea_bonus_gemma(float pos_z, float angolo) {


        GameObject bonus_gemma = Instantiate(Resources.Load("grafica_3d/Prefabs_space/gemma")) as GameObject;

        bonus_gemma.name = "bonus_gemma";

        bonus_gemma.transform.SetParent(cilindro.transform);

        float rad = angolo * Mathf.Deg2Rad;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        bonus_gemma.transform.position = new Vector3(xx, yy, pos_z);

        bonus_gemma.transform.localEulerAngles = new Vector3(0, 0, angolo);


    }


    void crea_bonus_speed(float pos_z, float angolo) {


        GameObject bonus_speed = Instantiate(Resources.Load("grafica_3d/Prefabs_space/speed")) as GameObject;

        bonus_speed.name = "bonus_speed";

        bonus_speed.transform.SetParent(cilindro.transform);

        float rad = angolo * Mathf.Deg2Rad;

        float xx = Mathf.Sin(rad) * 0.01f;
        float yy = Mathf.Cos(rad) * 0.01f;

        bonus_speed.transform.position = new Vector3(xx, yy, pos_z);

        bonus_speed.transform.localEulerAngles = new Vector3(0, 0, angolo);


    }








    void aggiorna_blocco() {

        if (blocco_velocita > .99f) {
            sposta_uv_bonus = sposta_uv_bonus - Time.deltaTime;

            int num_bonus = c_save.crea_bonus.Count;


            for (int n = 0; n < num_bonus; n++) {
                if (c_save.crea_bonus[n].tipo == 0) {
                    c_save.crea_bonus[n].mesh.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, sposta_uv_bonus));
                }
            }

        }





        float pos_z = cilindro.transform.position.z;

        int num_blocco = c_save.crea_blocco.Count;


        for (int n = 0; n < num_blocco; n++) {

            if (c_save.crea_blocco[n].attivo == 1) {

                float zz2 = c_save.crea_blocco[n].mesh.transform.position.z;


                if (c_save.crea_blocco[n].rotazione != 0 && blocco_velocita > .99f) {

                    c_save.crea_blocco[n].rad = c_save.crea_blocco[n].rad + c_save.crea_blocco[n].rotazione * Time.deltaTime;



                    float angolo = c_save.crea_blocco[n].rad * Mathf.Rad2Deg;

                    c_save.crea_blocco[n].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

                }




                if (c_save.crea_blocco[n].distruzione_oggetto > 0) {

                    c_save.crea_blocco[n].forza_impatto = c_save.crea_blocco[n].forza_impatto * .95f - .015f;

                    if (c_save.crea_blocco[n].forza_impatto > .025f) {
                        spostamento_blocco(c_save.crea_blocco[n].mesh, c_save.crea_blocco[n].punto_impatto, c_save.crea_blocco[n].forza_impatto, c_save.crea_blocco[n].rad);
                    }

                    c_save.crea_blocco[n].valore_dissolve = c_save.crea_blocco[n].valore_dissolve + (2 + c_save.crea_blocco[n].distruzione_oggetto) * Time.deltaTime;

                    //  Debug.Log("entra " + n+"  "+ c_save.crea_blocco[n].valore_dissolve);

                    aggiorna_oggetto_dissolto_singolo(n, c_save.crea_blocco[n].valore_dissolve);

                    if (c_save.crea_blocco[n].valore_dissolve > 1) {
                        c_save.crea_blocco[n].valore_dissolve = 1;
                        c_save.crea_blocco[n].attivo = 0;

                        DestroyImmediate(c_save.crea_blocco[n].mesh);
                    }


                }






                if (c_save.crea_blocco[n].arrivo == 0 && zz2 < distanza_disolve) {




                    c_save.crea_blocco[n].valore_dissolve = c_save.crea_blocco[n].valore_dissolve + 2 * Time.deltaTime;

                    //  Debug.Log("entra " + n+"  "+ c_save.crea_blocco[n].valore_dissolve);

                    aggiorna_oggetto_dissolto_singolo(n, c_save.crea_blocco[n].valore_dissolve);

                    if (c_save.crea_blocco[n].valore_dissolve > 1) {
                        c_save.crea_blocco[n].valore_dissolve = 1;
                        c_save.crea_blocco[n].attivo = 0;

                        //    Debug.Log("dovresti avere finito "+n);

                        DestroyImmediate(c_save.crea_blocco[n].mesh);
                    }

                }



                //   gestione_pos(n);



            }



        }




    }




    void gestione_pos(int num) {
        float rad = c_save.crea_blocco[num].rad;


        //  float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_blocco[num].altezza;
        float yy = Mathf.Cos(rad) * c_save.crea_blocco[num].altezza;


        float zz = c_save.crea_blocco[num].pos;

        c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


    }



    IEnumerator load_project_online(int num_livello = 1) {


        string path2 = path_server + "livello_" + num_livello + ".json";

        Debug.Log("" + path2);


        using (UnityWebRequest www = UnityWebRequest.Get(path2)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {



                try {


                    string testo_json = www.downloadHandler.text;


                    if (c_save != null) {

                        distruggi_oggetti();

                    }



                    c_save = null;

                    c_save = new classe_save();


                    c_save = JsonUtility.FromJson<classe_save>(testo_json);


                    associa_livello();



                }
                catch (Exception e) {

                    Debug.Log("errore_oggetto_codice " + e.ToString());

                }



            }


        }






    }




    void load_project() {


        try {

            if (level_json != null) {


                if (c_save != null) {

                    distruggi_oggetti();

                }



                c_save = null;

                c_save = new classe_save();


                c_save = JsonUtility.FromJson<classe_save>(level_json.text);


                associa_livello();


            }


        }
        catch (Exception e) {
            print("error " + e.ToString());
        }


    }


    void associa_livello() {


        spostamento_z = c_save.crea_cilindro[0].spostamento_z;
        spostamento_z2 = c_save.crea_cilindro[0].spostamento_z2;


        visione_boss = c_save.crea_cilindro[0].visione_boss;

        if (visione_boss == false) {
            boss.SetActive(false);
        }



        calcolo_snap(1);

        scala_cilindro();

        crea_finale();


        int num_blocchi = c_save.crea_blocco.Count;

        for (int k = 0; k < num_blocchi; k++) {
            crea_blocco(k);
        }

        int num_gemme = c_save.crea_gemma.Count;

        for (int k = 0; k < num_gemme; k++) {
            crea_gemma(k);
        }


        int num_monete = c_save.crea_moneta.Count;

        for (int k = 0; k < num_monete; k++) {
            crea_moneta(k);
        }

        int num_malus = c_save.crea_malus.Count;

        for (int k = 0; k < num_malus; k++) {
            crea_malus(k);
        }

        int num_bonus = c_save.crea_bonus.Count;

        for (int k = 0; k < num_bonus; k++) {
            crea_bonus(k);
        }

        carica_dati_livello_online = 1;

    }




    void distruggi_oggetti() {

        int num_blocchi2 = c_save.crea_blocco.Count;

        for (int k = 0; k < num_blocchi2; k++) {
            if (c_save.crea_blocco[k].mesh != null) {
                DestroyImmediate(c_save.crea_blocco[k].mesh);

            }

        }

        int num_gemma2 = c_save.crea_gemma.Count;

        for (int k = 0; k < num_gemma2; k++) {
            if (c_save.crea_gemma[k].mesh != null) {
                DestroyImmediate(c_save.crea_gemma[k].mesh);

            }

        }

        int num_moneta2 = c_save.crea_moneta.Count;

        for (int k = 0; k < num_moneta2; k++) {
            if (c_save.crea_moneta[k].mesh != null) {
                DestroyImmediate(c_save.crea_moneta[k].mesh);

            }

        }


        int num_bonus2 = c_save.crea_bonus.Count;

        for (int k = 0; k < num_bonus2; k++) {
            if (c_save.crea_bonus[k].mesh != null) {
                DestroyImmediate(c_save.crea_bonus[k].mesh);

            }

        }

        int num_malus2 = c_save.crea_malus.Count;

        for (int k = 0; k < num_malus2; k++) {
            if (c_save.crea_malus[k].mesh != null) {
                DestroyImmediate(c_save.crea_malus[k].mesh);

            }

        }


    }



    void delete_project() {




        if (c_save != null) {

            distruggi_oggetti();


        }



        c_save.crea_blocco.Clear();
        c_save.crea_bonus.Clear();
        c_save.crea_malus.Clear();
        c_save.crea_gemma.Clear();
        c_save.crea_moneta.Clear();



    }


    void calcolo_snap(int aggiorna = 0) {

        Debug.Log("aggiorna " + aggiorna);

        if (aggiorna == 1) {
            for (int k = 0; k < snap_rif.Length; k++) {

                if (snap_rif[k] != null) {
                    DestroyImmediate(snap_rif[k]);

                }


            }
        }



        GameObject rif = GameObject.Find("cilindro_esatto/rif");

        float rad2 = Mathf.PI / 18.0f;

        float rad_cilindro = cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;

        Vector3[] pos_vn = new Vector3[3000];


        for (int k = 0; k <= 9; k++) {


            for (int n = 0; n < 39; n++) {

                float rad = rad2 * n - rad_cilindro;

                float altezza = 0;

                altezza = k * .25f + 1.25f;

                if (k == 0) {
                    altezza = 0;
                }



                float xx = Mathf.Sin(rad) * (c_save.crea_cilindro[0].raggio + altezza);
                float yy = Mathf.Cos(rad) * (c_save.crea_cilindro[0].raggio + altezza);


                Vector3 pos = new Vector3(xx, yy, 0);

                pos_vn[n + k * 50] = pos;

                if (aggiorna == 1) {



                    snap_rif[n + k * 50] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere_rif", typeof(GameObject))) as GameObject;

                    snap_rif[n + k * 50].name = "snap_rif " + n;

                    snap_rif[n + k * 50].transform.position = pos;

                }
            }

        }

        distanza_corda = Vector3.Distance(pos_vn[0], pos_vn[1]);

        crea_snap = 1;
    }



    void crea_blocco(int num) {


        float rad = c_save.crea_blocco[num].rad;

        int tipo_blocco = c_save.crea_blocco[num].tipo;

        if (tipo_blocco > 0) {


            float angolo = rad * Mathf.Rad2Deg;

            float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
            float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

            float zz = c_save.crea_blocco[num].pos;



            if (tipo_blocco <= 4) {
                c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_" + tipo_blocco, typeof(GameObject))) as GameObject;
            }
            else {
                crea_blocco_mesh2(num, tipo_blocco, c_save.crea_blocco[num].struttura_procedurale, c_save.crea_blocco[num].struttura_procedurale_dz);
            }

            c_save.crea_blocco[num].attivo = 1;

            c_save.crea_blocco[num].mesh.name = "blocco " + num;

            c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);

            c_save.crea_blocco[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);




            if (tipo_blocco <= 4) {

                c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);
            }

            if (tipo_blocco >= 5 && tipo_blocco <= 8) {

                c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(0, 0, 0);
            }

            if (tipo_blocco >= 9) {
                c_save.crea_blocco[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

                c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(0, 0, zz);
            }


            if (tipo_blocco <= 4) {
                c_save.crea_blocco[num].mesh_dissolve = GameObject.Find("cilindro_esatto/blocco " + num + "/blocco_mesh");

                c_save.crea_blocco[num].mesh_dissolve.name = "blocco_mesh " + num;

                c_save.crea_blocco[num].mesh_renderer = c_save.crea_blocco[num].mesh_dissolve.GetComponent<Renderer>();

                c_save.crea_blocco[num].mesh_renderer.material.shader = Shader.Find("Custom/Dissolve");
            }

            if (tipo_blocco >= 5) {
                c_save.crea_blocco[num].mesh_dissolve = c_save.crea_blocco[num].mesh;

                c_save.crea_blocco[num].mesh_renderer = c_save.crea_blocco[num].mesh_dissolve.GetComponent<Renderer>();

                c_save.crea_blocco[num].mesh_renderer.material.shader = Shader.Find("Custom/Dissolve");


                if (tipo_blocco >= 9) {
                    rigenera_blocco(c_save.crea_blocco[num].mesh);

                }

            }

        }

    }




    void crea_gemma(int num) {


        float rad = c_save.crea_gemma[num].rad;

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_gemma[num].pos;



        c_save.crea_gemma[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/gemma_0", typeof(GameObject))) as GameObject;

        c_save.crea_gemma[num].mesh.name = "gemma";

        c_save.crea_gemma[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_gemma[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_gemma[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);

    }


    void crea_moneta(int num) {


        float rad = c_save.crea_moneta[num].rad;

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_moneta[num].pos;



        c_save.crea_moneta[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/moneta_0", typeof(GameObject))) as GameObject;

        c_save.crea_moneta[num].mesh.name = "moneta ";

        c_save.crea_moneta[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_moneta[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_moneta[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);

    }



    void crea_malus(int num) {


        float rad = c_save.crea_malus[num].rad;

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_malus[num].pos;

        int tipo_malus = c_save.crea_malus[num].tipo;


        c_save.crea_malus[num].inversion_controller = 0;



        c_save.crea_malus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/malus_" + tipo_malus, typeof(GameObject))) as GameObject;

        c_save.crea_malus[num].mesh.name = "malus" + tipo_malus + "_" + num;

        c_save.crea_malus[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_malus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_malus[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);

        if (tipo_malus == 0) {
            modifica_base(c_save.crea_malus[num].mesh);
        }

    }


    void crea_bonus(int num) {


        float rad = c_save.crea_bonus[num].rad;

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_bonus[num].pos;

        int tipo_bonus = c_save.crea_bonus[num].tipo;

        c_save.crea_bonus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/bonus_" + tipo_bonus, typeof(GameObject))) as GameObject;

        c_save.crea_bonus[num].mesh.name = "bonus" + tipo_bonus + "_" + num;

        c_save.crea_bonus[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_bonus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_bonus[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


        if (tipo_bonus == 0) {
            modifica_base(c_save.crea_bonus[num].mesh, 1);
        }

    }


    void aggiorna_oggetto_dissolto_singolo(int num, float dissolto) {

        //  Debug.Log(num+" num "+ dissolto);

        if (c_save.crea_blocco[num].mesh_renderer != null) {
            c_save.crea_blocco[num].mesh_renderer.material.SetFloat("_Amount", dissolto);


        }

    }



    void crea_blocco_mesh2(int num, int tipo, string struttura, string struttura_dz) {


        if (c_save.crea_blocco[num].mesh != null) {

            DestroyImmediate(c_save.crea_blocco[num].mesh);
        }

        c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_mesh", typeof(GameObject))) as GameObject;


        int scelta_colore = (int)(UnityEngine.Random.Range(.0f, 2.999f));

        Color colore_blocco = c_save_p.crea_parametri[0].colore_blocco_1;

        if (scelta_colore == 1) {
            colore_blocco = c_save_p.crea_parametri[0].colore_blocco_2;
        }

        if (scelta_colore >= 2) {
            colore_blocco = c_save_p.crea_parametri[0].colore_blocco_3;
        }


        c_save.crea_blocco[num].mesh.GetComponent<Renderer>().material.SetColor("_Color", colore_blocco);


        scala_blocco(c_save.crea_blocco[num].mesh, tipo, struttura, struttura_dz);

        c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);

        if (tipo >= 5 && tipo <= 8) {
            c_save.crea_blocco[num].mesh.AddComponent<MeshCollider>();

        }



    }


    void scala_blocco(GameObject ogg, int tipo, string struttura = "", string struttura_dz = "") {

        Vector3 scale = new Vector3(distanza_corda * 2, 1.0f, 2);

        if (tipo == 6) {
            scale = new Vector3(distanza_corda * 2, 2.0f, 2);
        }

        if (tipo == 7) {
            scale = new Vector3(distanza_corda * 2, 4.0f, 1);
        }

        if (tipo == 8) {
            scale = new Vector3(distanza_corda * 2, 4.0f, 4);
        }

        if (tipo <= 8) {
            ogg.transform.localScale = scale;

        }

        if (tipo >= 9) {

            ristruttura_blocco(ogg, tipo, struttura, struttura_dz);

        }


    }


    void ristruttura_blocco(GameObject ogg, int tipo, string struttura, string struttura_dz) {



        string testo_ogg = "696";

        string testo_ogg_dz = "22222222222222222";

        Vector3 dz = new Vector3(0, 0, 2);

        if (tipo == 9) {
            //-----------0123456789012345678
            testo_ogg = "0770077004500320077";

        }



        if (tipo == 10) {
            //-----------012345678901234567
            testo_ogg = "300500700300600888";

        }

        if (tipo == 11) {
            //-----------012345678901234567
            testo_ogg = "6987996";

        }


        if (tipo == 12) {
            //-----------012345678901234567
            testo_ogg = "8980096";
            dz = new Vector3(0, 0, 6);
        }


        if (tipo == 13) {
            //-----------012345678901234567
            testo_ogg = "811845611898113456";
            dz = new Vector3(0, 0, 6);
        }

        if (tipo == 14) {
            //-----------012345678901234567
            testo_ogg = "1234321";
            dz = new Vector3(0, 0, 6);
        }

        if (tipo == 15) {
            //-----------012345678901234567
            testo_ogg = "23956236847532532354235";
            dz = new Vector3(0, 0, 22);
        }




        if (tipo == 100) {
            testo_ogg = "" + struttura;
            testo_ogg_dz = "" + struttura_dz;

            //  Debug.Log("testo_ogg_dz "+ testo_ogg_dz);
        }



        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        int lung_dz = testo_ogg_dz.Length;
        if (lung_dz > 18) {
            lung_dz = 18;
        }


        int lung = testo_ogg.Length;
        if (lung > 18) {
            lung = 18;
        }

        float[] mesh_v = new float[37];

        float[] mesh_v_dz = new float[37];

        for (var n = 0; n < lung; n++) {
            string str = testo_ogg.Substring(n, 1);
            mesh_v[n] = int.Parse(str);
        }

        for (var n = 0; n < lung_dz; n++) {
            string str = testo_ogg_dz.Substring(n, 1);
            mesh_v_dz[n] = int.Parse(str);

        }


        int num_v = 0;

        for (var n = 0; n < 19; n++) {
            if (mesh_v[n] > 0) {
                num_v = num_v + 1;
            }

        }


        int num_vertici = num_v * 24;
        int num_tria = num_v * 12;

        Vector3[] vertices = new Vector3[num_vertici];
        Vector2[] uvs = new Vector2[num_vertici];
        int[] triangles = new int[num_tria * 3];

        int aum_v = -1;
        int aum_t = -1;

        Vector3 dz_base = dz;




        for (var n = 0; n < 19; n++) {
            if (mesh_v[n] > 0) {


                int codice = (int)(mesh_v[n]);

                aum_v = aum_v + 1;
                int nv = aum_v * 24;

                int mo = n * 2;


                Vector3 snap_n = snap_rif[mo].transform.position;
                Vector3 snap_n1 = snap_rif[mo + 2].transform.position;

                Vector3 snap_up_n = snap_rif[mo + codice * 50].transform.position;
                Vector3 snap_up_n1 = snap_rif[mo + 2 + codice * 50].transform.position;

                if (mesh_v[n] >= 9) {
                    snap_n = snap_rif[mo + 4 * 50].transform.position;
                    snap_n1 = snap_rif[mo + 2 + 4 * 50].transform.position;

                    snap_up_n = snap_rif[mo + 6 * 50].transform.position;
                    snap_up_n1 = snap_rif[mo + 2 + 6 * 50].transform.position;

                }


                dz = dz_base;

                if (mesh_v_dz[n] > 0) {
                    dz = new Vector3(0, 0, mesh_v_dz[n]) * spostamento_z;

                }

                Vector3 dz2 = dz * spostamento_z2;

                vertices[nv + 0] = snap_n + dz2;
                vertices[nv + 1] = snap_up_n + dz2;
                vertices[nv + 2] = snap_up_n1 + dz2;
                vertices[nv + 3] = snap_n1 + dz2;

                vertices[nv + 4] = snap_n + dz;
                vertices[nv + 5] = snap_up_n + dz;
                vertices[nv + 6] = snap_up_n1 + dz;
                vertices[nv + 7] = snap_n1 + dz;

                vertices[nv + 8] = snap_n + dz2;
                vertices[nv + 9] = snap_up_n + dz2;
                vertices[nv + 10] = snap_up_n + dz;
                vertices[nv + 11] = snap_n + dz;

                vertices[nv + 12] = snap_n1 + dz2;
                vertices[nv + 13] = snap_up_n1 + dz2;
                vertices[nv + 14] = snap_up_n1 + dz;
                vertices[nv + 15] = snap_n1 + dz;


                vertices[nv + 16] = snap_up_n + dz2;
                vertices[nv + 17] = snap_up_n + dz;
                vertices[nv + 18] = snap_up_n1 + dz;
                vertices[nv + 19] = snap_up_n1 + dz2;

                vertices[nv + 20] = snap_n + dz2;
                vertices[nv + 21] = snap_n + dz;
                vertices[nv + 22] = snap_n1 + dz;
                vertices[nv + 23] = snap_n1 + dz2;





                for (int k = 0; k < 6; k++) {
                    uvs[nv + k * 4 + 0] = new Vector2(0, 0);
                    uvs[nv + k * 4 + 1] = new Vector2(0, 1);
                    uvs[nv + k * 4 + 2] = new Vector2(1, 1);
                    uvs[nv + k * 4 + 3] = new Vector2(1, 0);
                }


                aum_t = aum_t + 1;
                int nt = aum_t * 36;

                triangles[nt + 0] = 0 + nv;
                triangles[nt + 1] = 1 + nv;
                triangles[nt + 2] = 3 + nv;

                triangles[nt + 3] = 1 + nv;
                triangles[nt + 4] = 2 + nv;
                triangles[nt + 5] = 3 + nv;


                triangles[nt + 6] = 7 + nv;
                triangles[nt + 7] = 5 + nv;
                triangles[nt + 8] = 4 + nv;

                triangles[nt + 9] = 7 + nv;
                triangles[nt + 10] = 6 + nv;
                triangles[nt + 11] = 5 + nv;


                triangles[nt + 12] = 9 + nv;
                triangles[nt + 13] = 8 + nv;
                triangles[nt + 14] = 11 + nv;

                triangles[nt + 15] = 11 + nv;
                triangles[nt + 16] = 10 + nv;
                triangles[nt + 17] = 9 + nv;

                triangles[nt + 18] = 13 + nv;
                triangles[nt + 19] = 14 + nv;
                triangles[nt + 20] = 15 + nv;

                triangles[nt + 21] = 13 + nv;
                triangles[nt + 22] = 15 + nv;
                triangles[nt + 23] = 12 + nv;

                triangles[nt + 24] = 16 + nv;
                triangles[nt + 25] = 17 + nv;
                triangles[nt + 26] = 19 + nv;

                triangles[nt + 27] = 17 + nv;
                triangles[nt + 28] = 18 + nv;
                triangles[nt + 29] = 19 + nv;


                triangles[nt + 30] = 23 + nv;
                triangles[nt + 31] = 21 + nv;
                triangles[nt + 32] = 20 + nv;

                triangles[nt + 33] = 23 + nv;
                triangles[nt + 34] = 22 + nv;
                triangles[nt + 35] = 21 + nv;



            }





        }



        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();



        ogg.transform.localScale = new Vector3(1, 1, 1);


        //   Debug.Break();

    }



    void crea_finale()
    {

        GameObject ogg2 = Instantiate(Resources.Load("grafica_3d/blocchi_cilindro/Prefabs/cilindro_esatto_finale", typeof(GameObject))) as GameObject;

        float pos_z = c_save.crea_cilindro[0].pos_finale.z;

        float raggio = c_save.crea_cilindro[0].raggio;

        float dx = raggio + .01f;

        ogg2.transform.SetParent(cilindro.transform);

        ogg2.transform.localScale = new Vector3(dx, dx, dx);

        ogg2.transform.localPosition = new Vector3(0, 0, pos_z);


        crea_blocco_finale(80,1);

     //   crea_blocco_finale(100,0);

    }



    void crea_blocco_finale(float sposta_z,int aumento=0)
    {




        for (int n = 0; n <18; n++)
        {
            int rnd = (int)(UnityEngine.Random.Range(.0f, 3.99f));

            GameObject ogg = Instantiate(Resources.Load("grafica_3d/blocchi_cilindro/Prefabs/blocco_city " + rnd, typeof(GameObject))) as GameObject;



            ogg.name = "blocco_finale ";

            ogg.transform.SetParent(cilindro.transform);





            float pos_z = c_save.crea_cilindro[0].pos_finale.z + sposta_z;



            Vector3 snap_n = snap_rif[n*2+ aumento].transform.position;
            Vector3 snap_n1 = snap_rif[n*2+2+ aumento].transform.position;


            Vector3 p0 = snap_n;
            Vector3 p1 = snap_n1;



           

            Vector3 pm = (snap_n + snap_n1) * .5f;


            ogg.transform.localPosition = new Vector3(pm.x, pm.y, pos_z);


            float xx = snap_n.x - snap_n1.x;
            float yy = snap_n.y - snap_n1.y;


            float dis = Mathf.Sqrt(xx*xx+yy*yy);

            float rad = Mathf.Atan2(xx, yy);




            ogg.transform.localEulerAngles = new Vector3(0, 0,-20+ -20*n);


            float dx = dis;
            float dy =UnityEngine.Random.Range( dis, dis*8);


            ogg.transform.localScale = new Vector3(dx, dx, dx);


        }

    }




    void crea_sfera(Vector3 pos, string str = "") {
        GameObject sfera = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere", typeof(GameObject))) as GameObject;
        sfera.transform.position = pos;

        sfera.name = "" + str;
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

        pos_direction[3] = new Vector3(-.6f, 0, -.8f);
        pos_direction[4] = new Vector3(.6f, 0, -.8f);

        pos_direction[5] = new Vector3(0, 0, -.5f);
        pos_direction[6] = new Vector3(0, 0, -.5f);

        pos_direction[7] = new Vector3(0, -.5f, 0);


        pos_ray_direction[0] = new Vector3(0, 0, 2.6f);
        pos_ray_direction[1] = new Vector3(-.75f, 0, .125f);
        pos_ray_direction[2] = new Vector3(.75f, 0, .125f);
        pos_ray_direction[3] = new Vector3(0, 0, 1.6f);
        pos_ray_direction[4] = new Vector3(0, 0, 1.6f);
        pos_ray_direction[5] = new Vector3(-.8f, 0, .35f);
        pos_ray_direction[6] = new Vector3(.8f, 0, .35f);
        pos_ray_direction[7] = new Vector3(0, 0, 2.6f);

        distanza_direction[0] = 2.6f;
        distanza_direction[1] = .75f;
        distanza_direction[2] = .75f;

        distanza_direction[3] = 1.6f;
        distanza_direction[4] = 1.6f;

        distanza_direction[5] = .8f;
        distanza_direction[6] = .8f;
        distanza_direction[7] = 2.6f;

        int numero_coll = 0;
        Vector3 punto_coll = new Vector3(0, 0, 0);

        int num_block = -1;

        for (int n = 0; n <= 7; n++) {

            pos = astronave.transform.position + pos_direction[n];

#if UNITY_EDITOR
            Debug.DrawRay(pos, pos_ray_direction[n], new Color(1, n * .2f, 0, 1));
#endif

            if (Physics.Raycast(pos, pos_ray_direction[n], out hit_collider, 15)) {

                float dis = hit_collider.distance;




                if (dis < distanza_direction[n]) {


                    if (hit_collider.collider.name.IndexOf("blocco") > -1) {



                        string str_block = "" + hit_collider.collider.name;






                        int indice = str_block.IndexOf("blocco_mesh");


                        if (indice == -1) {
                            str_block = str_block.Replace("blocco ", "");

                            num_block = int.Parse(str_block);

                            numero_coll = numero_coll + 1;

                            punto_coll = punto_coll + hit_collider.point;
                        }

                        if (indice > -1) {

                            str_block = str_block.Replace("blocco_mesh ", "");

                            num_block = int.Parse(str_block);


                            if (c_save.crea_blocco[num_block].disattiva_coll == 0) {
                                c_save.crea_blocco[num_block].distruzione_oggetto = 10;

                                carica_particles("particles/CFXR Explosion 2", c_save.crea_blocco[num_block].mesh_dissolve.transform.position);

                                c_save.crea_blocco[num_block].disattiva_coll = 1;


                                analisi_energia(40);

                                suona_effetto_UI(3, 1);

                                suona_effetto_UI(4, .5f);

                            }
                        }



                        //    Debug.Log("toccato blocco " + num_block);






                    }

                    //----------







                    if (hit_collider.collider.name.IndexOf("bonus1_") > -1) {

                        string str_bonus = "" + hit_collider.collider.name;

                        //   Debug.Log("" + str_block);

                        str_bonus = str_bonus.Replace("bonus1_", "");

                        int num_bonus = int.Parse(str_bonus);

                        suona_effetto_UI(8, 1);

                        numero_spari = numero_spari + 1;
                        if (c_save.crea_bonus[num_bonus].attivo == 0) {
                            c_save.crea_bonus[num_bonus].attivo = 1;
                            Destroy(c_save.crea_bonus[num_bonus].mesh);

                        }
                    }


                    if (hit_collider.collider.name.IndexOf("bonus2_") > -1) {

                        string str_bonus = "" + hit_collider.collider.name;

                        //     Debug.Log("" + str_bonus);

                        str_bonus = str_bonus.Replace("bonus2_", "");

                        int num_bonus = int.Parse(str_bonus);

                        suona_effetto_UI(8, 1);

                        if (c_save.crea_bonus[num_bonus].attivo == 0) {
                            c_save.crea_bonus[num_bonus].attivo = 1;

                            attiva_barriera = tempo_barriera;

                            Destroy(c_save.crea_bonus[num_bonus].mesh);


                        }

                    }



                    if (hit_collider.collider.name.IndexOf("boss_potere_mesh") > -1) {
                        string str_boss_potere_mesh = "" + hit_collider.collider.name;

                        str_boss_potere_mesh = str_boss_potere_mesh.Replace("boss_potere_mesh ", "");

                        int num_boss_potere_mesh = int.Parse(str_boss_potere_mesh);

                        analisi_energia(50);


                        boss_potere[num_boss_potere_mesh] = 0;

                        boss_potere_attivo[num_boss_potere_mesh] = 1;

                        carica_particles("particles/CFXR Explosion 2", hit_collider.point);

                        suona_effetto_UI(3, 1);

                        DestroyImmediate(boss_potere_mesh[num_boss_potere_mesh]);

                    }



                }

            }


            if (num_block > -1 && numero_coll > 0) {


                Vector3 punto_coll_uso = punto_coll / numero_coll;





                // crea_sfera(punto_coll_uso);



                //   Debug.Break();

                if (c_save.crea_blocco[num_block].disattiva_coll == 0) {
                    c_save.crea_blocco[num_block].disattiva_coll = 1;



                    analisi_energia(25);

                    carica_particles("particles/CFXR Hit B", punto_coll_uso);

                    c_save.crea_blocco[num_block].punto_impatto = punto_coll_uso;
                    c_save.crea_blocco[num_block].forza_impatto = velocita_personaggio * 8;

                    suona_effetto_UI(3, 1);

                    c_save.crea_blocco[num_block].distruzione_oggetto = 1;

                }
            }



        }




    }


    void gestione_coll_special_bonus_malus() {
        RaycastHit hit_collider;


        Vector3 pos = astronave.transform.position;


        Vector3 pos_astronave = new Vector3(pos.x, pos.y, pos.z + 1.2f);

        if (Physics.Raycast(pos_astronave, new Vector3(0, -1, 0), out hit_collider, 15)) {
            if (hit_collider.collider.name.IndexOf("bonus0_") > -1) {
                string str_bonus = hit_collider.collider.name;

                str_bonus = str_bonus.Replace("bonus0_", "");

                int num_bonus = int.Parse(str_bonus);

                if (c_save.crea_bonus[num_bonus].attivo == 0) {

                    c_save.crea_bonus[num_bonus].attivo = 1;

                    velocita_bonus = c_save_p.crea_parametri[0].velocita_bonus;


                }

            }

            if (hit_collider.collider.name.IndexOf("malus0_") > -1) {
                string str_malus = hit_collider.collider.name;


                str_malus = str_malus.Replace("malus0_", "");

                int num_malus = int.Parse(str_malus);

                if (c_save.crea_malus[num_malus].attivo == 0) {

                    c_save.crea_malus[num_malus].attivo = 1;

                    inversione_controllo = 1 - inversione_controllo;

                    Vector3 pos_P = c_save.crea_malus[num_malus].mesh.transform.position;

                    Vector3 pos_p1 = new Vector3(pos_P.x, pos_P.y + 4, pos.z);


                    carica_particles("particles/CFX_Text_change", pos_p1);


                }

            }


        }


    }






    void carica_particles(string path, Vector3 pos) {

        GameObject particles = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;


        particles.transform.position = pos;


    }




    void gestione_collisione_sparo(int num, GameObject ogg, int tipo = 0) {

        RaycastHit hit_collider;

        Vector3[] pos_ray_direction = new Vector3[10];

        pos_ray_direction[0] = new Vector3(0, 0, 10);
        pos_ray_direction[1] = new Vector3(0, 0, 10);
        pos_ray_direction[2] = new Vector3(0, 0, 10);
        pos_ray_direction[3] = new Vector3(0, 0, 10);


        Vector3 pos;

        Vector3[] pos_direction = new Vector3[10];

        pos_direction[0] = new Vector3(0, 0, 0);
        pos_direction[1] = new Vector3(0, -.65f, 0);
        pos_direction[2] = new Vector3(-.3f, -.35f, 0);
        pos_direction[3] = new Vector3(.3f, -.35f, 0);
        pos_direction[4] = new Vector3(0, -1.25f, 0);



        float distanza_coll = 0;


        if (tipo == 0) {
            distanza_coll = Vector3.Distance(sparo[num].transform.position, astronave.transform.position);
        }
        if (tipo == 1) {
            distanza_coll = Vector3.Distance(sparo_boss[num].transform.position, astronave.transform.position);
        }


        int toccato_astronave = 0;

        if (distanza_coll < 2 && tipo == 1) {

            toccato_astronave = 1;

            carica_particles("particles/CFXR Explosion 3", ogg.transform.position);

            DestroyImmediate(ogg);
            analisi_energia(30);

            suona_effetto_UI(3, 1);
        }


        if (toccato_astronave == 0) {


            for (int n = 0; n <= 4; n++) {

                pos = ogg.transform.position + pos_direction[n];


#if UNITY_EDITOR
                Debug.DrawRay(pos, pos_ray_direction[n], new Color(1, n * .2f, 0, 1));
#endif


                if (Physics.Raycast(pos, pos_ray_direction[n], out hit_collider, 15)) {

                float dis = hit_collider.distance;


                    if (dis < 3.6f)
                    {


                        if (hit_collider.collider.name.IndexOf("blocco") > -1)
                        {

                            float dis_z = hit_collider.point.z;


                           // Debug.Log("dis_z "+ dis_z);

                            float volume = (60 - hit_collider.point.z) / 60;
                            if (volume < 0)
                            {
                                volume = 0;
                            }


                            string str_block = "" + hit_collider.collider.name;

                            Debug.Log("" + str_block);

                            int num_block = -1;


                            int indice = str_block.IndexOf("blocco_mesh");


                            if (indice == -1)
                            {
                                str_block = str_block.Replace("blocco ", "");

                                num_block = int.Parse(str_block);

                                c_save.crea_blocco[num_block].distruzione_oggetto = 1;

                                c_save.crea_blocco[num_block].punto_impatto = hit_collider.point;
                                c_save.crea_blocco[num_block].forza_impatto = velocita_personaggio * 5;

                                carica_particles("particles/CFXR Hit A", hit_collider.point);


                                

                                suona_effetto_UI(4, volume);


                                if (tipo == 0)
                                {
                                    attivo_tempo_sparo[num] = 0;
                                }
                                else
                                {
                                    attivo_tempo_sparo_boss[num] = 0;
                                }
                                DestroyImmediate(ogg);
                                n = 100;

                                c_save.crea_blocco[num_block].disattiva_coll = 1;

                            }

                            if (indice > -1)
                            {

                                str_block = str_block.Replace("blocco_mesh ", "");

                                num_block = int.Parse(str_block);


                                carica_particles("particles/CFXR Hit A", hit_collider.point);


                                carica_particles("particles/CFXR Explosion 3", hit_collider.collider.transform.position);

                                suona_effetto_UI(4, volume*2);

                                if (tipo == 0)
                                {
                                    attivo_tempo_sparo[num] = 0;
                                }
                                else
                                {
                                    attivo_tempo_sparo_boss[num] = 0;
                                }


                                DestroyImmediate(ogg);

                                c_save.crea_blocco[num_block].distruzione_oggetto = 1;
                                c_save.crea_blocco[num_block].disattiva_coll = 1;

                                n = 100;

                            }





                        }


                    }

                }


            }

        }

    }




    void gestione_boss() {

        if (visione_boss == true) {

            if (blocco_velocita > .99f && attivo_popup == 0) {

                float raggio = c_save.crea_cilindro[0].raggio;


                boss_rad = Mathf.MoveTowards(boss_rad, 0, Time.deltaTime);


                float xx = Mathf.Sin(boss_rad) * raggio;
                float yy = Mathf.Cos(boss_rad) * raggio;

                float rad_angle = Mathf.Atan2(xx, yy);

                float angle = rad_angle * Mathf.Rad2Deg;

                boss.transform.position = new Vector3(xx, yy, -75);

                boss.transform.localEulerAngles = new Vector3(0, 0, -angle);


                float diff = boss_rad;


                attivo_tempo_sparo_boss2 = attivo_tempo_sparo_boss2 - Time.deltaTime;

                attivo_tempo_potere_boss = attivo_tempo_potere_boss - Time.deltaTime;

                if (attivo_tempo_potere_boss < -3) {
                    attivo_tempo_potere_boss = UnityEngine.Random.Range(0.0f, 3.0f);
                    crea_potere_boss();


                }


                if (attivo_tempo_sparo_boss2 < 0) {

                    if (Mathf.Abs(diff) < .1f) {
                        int num_sparo_boss = -1;

                        for (int n = 0; n < 10; n++) {

                            if (attivo_tempo_sparo_boss[n] < -.1) {
                                num_sparo_boss = n;

                                attivo_tempo_sparo_boss[n] = 1.5f;


                                sparo_boss[n] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sparo_boss", typeof(GameObject))) as GameObject;

                                Vector3 pos_sparo_boss = boss_mesh_sparo.transform.position;

                                sparo_boss[n].transform.position = new Vector3(pos_sparo_boss.x, pos_sparo_boss.y, pos_sparo_boss.z);

                                sparo_boss_rad[n] = 0;

                                sparo_boss[n].name = "sparo_boss " + n;

                                attivo_tempo_sparo_boss2 = UnityEngine.Random.Range(2f, 2.0f) - che_livello * .025f;

                                n = 1000;


                            }



                        }


                    }

                }

            }

            gestione_sparo_boss();

        }
    }


    void rigenera_blocco(GameObject ogg) {


        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;

        Vector3 pos = ogg.transform.position;

        Vector3[] vertices_studio = mesh.vertices;
        Vector3[] vertices = mesh.vertices;

        for (int n = 0; n < vertices.Length; n++) {
            Vector3 pv = ogg.transform.TransformPoint(vertices[n]);


            vertices[n] = new Vector3(pv.x, pv.y, pv.z - pos.z);


        }



        mesh.vertices = vertices;


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        ogg.transform.localEulerAngles = new Vector3(0, 0, 0);

        DestroyImmediate(ogg.GetComponent<MeshCollider>());


        ogg.AddComponent<MeshCollider>();

    }






    void spostamento_blocco(GameObject ogg, Vector3 pi, float forza, float ogg_rad) {

        //    Debug.Log("blocco impatto ");

        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;


        Vector3[] vertices = mesh.vertices;

        Vector3 pos = ogg.transform.position;

        //   crea_sfera(pi,"coll");

        float rad = -ogg.transform.localEulerAngles.z * Mathf.Deg2Rad;

        for (int n = 0; n < vertices.Length; n++) {

            Vector3 pv = ogg.transform.TransformPoint(vertices[n]);



            //     crea_sfera(pv);

            float xx = pv.x - pi.x;
            float zz = pv.z - pi.z;

            float dis = Mathf.Sqrt(xx * xx + zz * zz);

            //    float rad = Mathf.Atan2(xx, zz);

            float molt = (6 - dis) / 6.0f;

            if (molt < 0) {
                molt = 0;
            }

            //  float px = vertices[n].x - Mathf.Sin(rad) * forza*molt;
            float pz = pv.z + forza * molt * Time.deltaTime - pos.z;



            vertices[n] = new Vector3(vertices[n].x, vertices[n].y, pz);


        }



        mesh.vertices = vertices;


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        //    Debug.Break();

    }


    void crea_button_input_text(int num, string str = "") {

        pulsante_field[num] = Instantiate(Resources.Load("UI/InputField_txt", typeof(GameObject))) as GameObject;

        pulsante_field[num].name = "pulsante_field_" + num;

        pulsante_field[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        pulsante_field[num].transform.SetParent(canvas.transform);

        pulsante_field_testo[num] = GameObject.Find("Canvas_pannello/pulsante_field_" + num);
        pulsante_field_testo2[num] = GameObject.Find("Canvas_pannello/pulsante_field_" + num + "/Text");



        pulsante_field_testo[num].GetComponent<InputField>().text = "" + str;


        InputField m_Toggle2 = pulsante_field[num].GetComponent<InputField>();

        m_Toggle2.onEndEdit.AddListener(delegate {
            pressione_input_text(num, m_Toggle2);
        });




    }


    void crea_button_text(int num, string txt, Color colore_testo, GameObject parent, string path = "Canvas", string path_sprite = "") {

        pulsante[num] = Instantiate(Resources.Load("UI/Button_text", typeof(GameObject))) as GameObject;

        pulsante[num].name = "pulsante_text" + num;

        pulsante[num].transform.SetParent(parent.transform);

        pulsante[num].GetComponent<Button>().onClick.AddListener(() => pressione_pulsante(num));

        ColorBlock cb = pulsante[num].GetComponent<Button>().colors;
        cb.selectedColor = newColor;
        pulsante[num].GetComponent<Button>().colors = cb;


        if (path_sprite != "") {
            pulsante[num].GetComponent<Image>().sprite = Resources.Load<Sprite>(path_sprite);
        }


        pulsante[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


        pulsante_testo[num] = GameObject.Find(path + "/pulsante_text" + num + "/Text_TMP");

        pulsante_testo[num].GetComponent<TextMeshProUGUI>().text = "" + txt;

        pulsante_testo[num].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_y / 20);


        pulsante_testo[num].GetComponent<TextMeshProUGUI>().color = colore_testo;



        pulsante_testo[num].GetComponent<TextMeshProUGUI>().raycastTarget = false;

    }


    void crea_grafica_text(int num, Color colore, string txt, GameObject parent, string path = "Canvas", string path_sprite = "") {

        //    Debug.Log("entratooo");


        grafica[num] = Instantiate(Resources.Load("UI/Image_text", typeof(GameObject))) as GameObject;

        grafica[num].name = "grafica_text" + num;

        grafica[num].transform.SetParent(parent.transform);

        grafica[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        grafica[num].GetComponent<Image>().color = colore;

        if (path_sprite != "") {
            grafica[num].GetComponent<Image>().sprite = Resources.Load<Sprite>(path_sprite);
        }


        grafica[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);




        grafica_testo[num] = GameObject.Find(path + "/grafica_text" + num + "/Text");



        grafica_testo[num].GetComponent<TextMeshProUGUI>().text = "" + txt;

        grafica_testo[num].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 20);


    }


    void pressione_pulsante(int num) {

#if UNITY_EDITOR

        Debug.Log("pulsante " + num);

#endif


        suona_effetto_UI(1, 1);


        if (num == 0) {

            crea_popup(4);


        }


        if (num == 202) {

            SceneManager.LoadScene("menu");


        }

        if (num == 203) {

            distruggi_menu_popup();


        }

        if (num == 208) // ads raddoppia
        {

        }

        if (num == 209) // next level da salvare e poi 
        {
            if (script_struttura_dati != null) {


                // da passare i parametri stelle


                script_struttura_dati.livello_in_uso = script_struttura_dati.livello_in_uso + 1;

                if (script_struttura_dati.livello_massimo_raggiunto < script_struttura_dati.livello_in_uso) {
                    script_struttura_dati.livello_massimo_raggiunto = script_struttura_dati.livello_massimo_raggiunto + 1;
                    PlayerPrefs.SetInt("livello_massimo_raggiunto", script_struttura_dati.livello_massimo_raggiunto);
                }


                PlayerPrefs.SetInt("livello_in_uso", script_struttura_dati.livello_in_uso);

                SceneManager.LoadScene("gioco");

            }

        }


        if (num == 210) // repeat level
        {

            SceneManager.LoadScene("gioco");


        }


    }



    void pressione_input_text(int num, InputField tog) {


        if (tog.text != null || tog.text != "" && tog.text != "-" && tog.text != ".") {

            Debug.Log("pressione!" + tog.text + "!!");

        }


    }


    void crea_menu() {

        distruggi_menu();



        crea_grafica_text(0, new Color(0, 0, 0, .5f), "", canvas, "Canvas", "");

        crea_button_text(0, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/menu");



        //   crea_grafica_text(1, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/gemme");
        //   crea_grafica_text(2, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");

        //   crea_grafica_text(3, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/monete");
        //    crea_grafica_text(4, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");


        crea_grafica_text(5, new Color(1, 1, 1, 1), "", canvas, "Canvas", "");
        crea_grafica_text(6, new Color(0, .5f, 1, 1), "", canvas, "Canvas", "");
        crea_grafica_text(7, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/energia");

        crea_grafica_text(8, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/ammo");
        crea_grafica_text(9, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");



        crea_grafica_text(10, new Color(1, 1, 1, 1), "", canvas, "Canvas", "");

        crea_grafica_text(11, new Color(1, 1, 1, 1), "", canvas, "Canvas", "");



    }


    void aggiorna_menu_offline()
    {


      
        if (grafica[11] != null)  
        {

            float dx2 = risoluzione_x;
            float dy2 = risoluzione_y;
      

            grafica[11].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[11].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        }


    }




        void aggiorna_menu() {
        font_size = (int)(risoluzione_x / 25);

        float valore_yy = .445f;

        if (crea_popup_finale > 0 || attivo_popup > 0) {
            valore_yy = .75f;

        }


        spostamento_ui_verticale = Mathf.Lerp(spostamento_ui_verticale, valore_yy, Time.deltaTime * 2);


        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;

        if (grafica[0] != null)  //top
        {

            float dx2 = risoluzione_x;
            float dy2 = dy * .5f;
            pos_x = 0;
            pos_y = risoluzione_y * .455f;


            grafica[0].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, risoluzione_y);

        }



        float pos_y2 = pos_y = risoluzione_y * (spostamento_ui_verticale);


        if (pulsante[0] != null)  //settings
        {
            float dx2 = dy * .4f;
            float dy2 = dx2 * (79f / 75f);
            pos_x = risoluzione_x * -.5f + dx2 * .65f;
            pos_y = pos_y2;
            pulsante[0].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            pulsante[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);



        }

        if (grafica[3] != null)  //coin
        {

            float dx2 = dy * .8f;
            float dy2 = dx2;
            pos_x = risoluzione_x * .5f - dx2 * .5f;
            pos_y = pos_y2;

            grafica[3].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[4] != null)  //coin  txt
        {

            float dx2 = dy * .8f;
            float dy2 = dx2;
            pos_x = risoluzione_x * .5f - dx2 * .31f;
            pos_y = pos_y2;

            grafica[4].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[4].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[4].GetComponent<TextMeshProUGUI>().text = "" + (monete_partita_corrente);

        }

        if (grafica[1] != null)  //gem
        {

            float dx2 = dy * .8f;
            float dy2 = dx2;
            pos_x = risoluzione_x * .5f - dx2 * 1.5f;
            pos_y = pos_y2;

            grafica[1].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[2] != null)  //gem  txt
        {

            float dx2 = dy * .8f;
            float dy2 = dx2;
            pos_x = risoluzione_x * .5f - dx2 * (1.5f - .17f);
            pos_y = pos_y2;

            grafica[2].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[2].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[2].GetComponent<TextMeshProUGUI>().text = "" + gemme_prese;

        }


        if (grafica[8] != null)  //gem
        {

            float dx2 = dy * .8f;
            float dy2 = dx2;
            pos_x = risoluzione_x * .5f - dx2 * .5f;
            pos_y = pos_y2;

            grafica[8].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[8].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[9] != null)  //ammo  txt
        {

            float dx2 = dy * .8f;
            float dy2 = dx2;
            pos_x = risoluzione_x * .5f - dx2 * (.5f - .17f);
            pos_y = pos_y2;

            grafica[9].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[9].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[9].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[9].GetComponent<TextMeshProUGUI>().text = "" + numero_spari;

        }




        pos_x = risoluzione_x * -.1f;



        if (grafica[5] != null)  // energia base
        {



            float frazionario = energia / energia_base;

            float dx2 = risoluzione_x * .2f;
            float dy2 = dy * .15f;

            float pos_x2 = pos_x + dx2 * .5f;
            pos_y = pos_y2;

            grafica[5].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * 1.02f, dy2);
            grafica[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x2, pos_y);

            float dx6 = risoluzione_x * .2f * frazionario;
            float pos_x6 = pos_x + dx6 * .5f;

            grafica[6].GetComponent<RectTransform>().sizeDelta = new Vector2(dx6, dy2 * .8f);
            grafica[6].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x6, pos_y);


        }

        if (grafica[7] != null)  // energia
        {

            float dx2 = dy * .85f;
            float dy2 = dx2;

            pos_y = pos_y2;

            grafica[7].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[7].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);



        }


        int font_size_level = (int)(risoluzione_x / 7);

        if (grafica[10] != null) // inizio
        {

            float dx2 = risoluzione_x;
            float dy2 = risoluzione_y;
            pos_x = 0;
            pos_y = 0;



            grafica_testo[10].GetComponent<TextMeshProUGUI>().fontSize = font_size_level;

            if (script_struttura_dati != null) {
                grafica_testo[10].GetComponent<TextMeshProUGUI>().text = "LEVEL " + script_struttura_dati.livello_in_uso;
            }

            tempo_inizio_livello = tempo_inizio_livello - Time.deltaTime;

            float alpha = tempo_inizio_livello;

            if (alpha < 0) {
                alpha = 0;

                inizio_game = 1;
                dx2 = 5;
                dy2 = 5;
                pos_y = risoluzione_y;
            }



            grafica[10].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[10].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica[10].GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            grafica_testo[10].GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, alpha);



        }

        fade_intro = fade_intro - Time.deltaTime*2; 

        if (fade_intro < 0)
        {
            fade_intro = 0;
        }

        if (grafica[11] != null)  // energia
        {

         

            grafica[11].GetComponent<Image>().color = new Color(1,1,1,fade_intro);
          


        }



    }



    public void controllo_risoluzione() {

        controllo_mobile = 0;


#if (UNITY_ANDROID || UNITY_IOS) &&  !UNITY_EDITOR

        controllo_mobile = 1;

#endif

        risoluzione_x = Screen.width;
        risoluzione_y = Screen.height;



        float xx = risoluzione_x;

        rapporto_risoluzione = xx / risoluzione_y;

        spostamento_sx = (rapporto_risoluzione / 1.333333f);
        spostamento_sx2 = (rapporto_risoluzione - 1.333333f);


        if (rapporto_risoluzione < 1) {

            spostamento_sx = (rapporto_risoluzione / .75f);
            spostamento_sx2 = (rapporto_risoluzione - .75f);

        }


        xm_old = xm;
        ym_old = ym;


        xm = Input.mousePosition.x;
        ym = Input.mousePosition.y;






        for (int n = 0; n <= 3; n++) {
            touch_xo[n] = touch_x[n];
            touch_yo[n] = touch_y[n];
        }

        for (int n = 0; n <= 3; n++) {
            touch_x[n] = -1000;
            touch_y[n] = -1000;
        }

#if ((UNITY_ANDROID || UNITY_IOS ))

        for (int i = 0; i < Input.touchCount; i++) {

            Touch touch = Input.GetTouch(i);


            touch_x[i] = touch.position.x;
            touch_y[i] = touch.position.y;

        }

#endif



        if (controllo_mobile == 0 && Input.GetMouseButton(0)) {
            for (int n = 0; n <= 0; n++) {
                touch_x[n] = xm;
                touch_y[n] = ym;
                touch_rx[n] = touch_x[n];
                touch_ry[n] = touch_y[n];
            }


            diff_xm = xm - xm_old;
            diff_ym = ym - ym_old;

        }




        if (controllo_mobile == 1) {
            diff_xm = 0;
            diff_ym = 0;


            if (touch_x[0] > 0 && touch_xo[0] > 0 && touch_xo[1] < 0) {

                diff_xm = (touch_x[0] - touch_xo[0]);

            }


            if (touch_y[0] > 0 && touch_yo[0] > 0 && touch_xo[1] < 0) {
                diff_ym = (touch_y[0] - touch_yo[0]);

            }

        }





    }



    void modifica_base(GameObject ogg, int t_bonus = 0) {


        //    Debug.Log("" + ogg.name);



        Vector3[] pos_v = new Vector3[50];


        float rad2 = Mathf.PI / 18.0f;

        float altezza = .05f;

        for (int n = 0; n < 49; n++) {

            float rad = rad2 * n;



            float xx = Mathf.Sin(rad) * (c_save.crea_cilindro[0].raggio + altezza);
            float yy = Mathf.Cos(rad) * (c_save.crea_cilindro[0].raggio + altezza) - c_save.crea_cilindro[0].raggio;

            pos_v[n] = new Vector3(xx, yy, 0);


        }


        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        int num_v = 5;
        int num_tria = num_v - 1;

        Vector3[] vertices = new Vector3[num_v * 2];
        Vector2[] uvs = new Vector2[num_v * 2];
        int[] tria = new int[num_tria * 6];


        float raggio = c_save.crea_cilindro[0].raggio;

        float u = 1.0f / (num_v - 1);



        for (int n = 0; n < num_v; n++) {
            vertices[n] = pos_v[34 + n];

            vertices[n + num_v] = vertices[n];
            vertices[n + num_v].z = vertices[n + num_v].z + 6;

            uvs[n] = new Vector2(u * n, 0);
            uvs[n + num_v] = new Vector2(u * n, 1);
        }


        for (int n = 0; n < num_v - 1; n++) {
            int num_t = n * 6;

            tria[num_t + 0] = 0 + n;
            tria[num_t + 1] = 5 + n;
            tria[num_t + 2] = 6 + n;

            tria[num_t + 3] = 0 + n;
            tria[num_t + 4] = 6 + n;
            tria[num_t + 5] = 1 + n;

        }







        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = tria;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();


        DestroyImmediate(ogg.GetComponent<MeshCollider>());


        ogg.AddComponent<MeshCollider>();



    }


    void crea_popup(int num = 1) {




        distruggi_menu_popup();

        attivo_popup = num;


        canvas_popup.SetActive(true);


        if (num == 1) {

            crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/sfondo_popUP"); //pannello shop

            crea_grafica_text(201, new Color(1, 1, 1, 0), "LEVEL " + script_struttura_dati.livello_in_uso, canvas_popup, "Canvas_popup/Panel", ""); //pannello shop






            crea_grafica_text(202, new Color(1, 1, 1, 0), "", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop
            crea_grafica_text(203, new Color(1, 1, 1, 0), "COMPLETED", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop

            grafica_testo[203].GetComponent<TextMeshProUGUI>().color = new Color(.8f, 0, 0, 1);

            grafica[203].transform.localEulerAngles = new Vector3(0, 0, 10);

            for (int n = 0; n <= 2; n++) {
                crea_grafica_text(204 + n, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/stella 0"); // stelle
                crea_grafica_text(207 + n, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/stella 1"); // stelle

                grafica_dime[4 + n] = 1;
                grafica_dime[7 + n] = 2;

                grafica_tempo[4 + n] = .0f;
                grafica_tempo[7 + n] = 2.0f + n * .25f;
            }

            crea_grafica_text(210, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ok_livello"); // stelle

            crea_grafica_text(211, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ok_energia"); // stelle
            crea_grafica_text(212, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ok_ammo"); // stelle

            grafica_dime[10] = 2;
            grafica_dime[11] = 2;
            grafica_dime[12] = 2;

            grafica_tempo[10] = 2.0f;
            grafica_tempo[11] = 2.25f;
            grafica_tempo[12] = 2.5f;

            grafica_tempo[1] = .5f;
            grafica_tempo[2] = 1;
            grafica_tempo[3] = 1.5f;


            grafica_dime[1] = 4;

            grafica_dime[2] = 4;

            grafica_dime[3] = 4;


            suona_effetto_UI(5, 1);

        }

        if (num == 2) {

            crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/sfondo_popUP"); //pannello shop

            crea_grafica_text(201, new Color(1, 1, 1, 0), "TRY", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop



            crea_grafica_text(202, new Color(1, 1, 1, 0), "AGAIN", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop


            crea_grafica_text(203, new Color(1, 1, 1, 0), "CLICK\nTO CONTINUE", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop

            grafica_testo[203].GetComponent<TextMeshProUGUI>().color = new Color(.8f, 0, 0, 1);



            for (int n = 0; n <= 2; n++) {
                crea_grafica_text(204 + n, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/stella 0"); // stelle
                crea_grafica_text(207 + n, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/stella 1"); // stelle

                grafica_dime[4 + n] = 1;
                grafica_dime[7 + n] = 2;

                grafica_tempo[4 + n] = .0f;
                grafica_tempo[7 + n] = 2.0f + n * .25f;
            }

            crea_grafica_text(210, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ok_livello"); // stelle

            crea_grafica_text(211, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ok_energia"); // stelle
            crea_grafica_text(212, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ok_ammo"); // stelle

            grafica_dime[10] = 2;
            grafica_dime[11] = 2;
            grafica_dime[12] = 2;

            grafica_tempo[10] = 2.0f;
            grafica_tempo[11] = 2.25f;
            grafica_tempo[12] = 2.5f;

            grafica_tempo[1] = .5f;
            grafica_tempo[2] = 1;
            grafica_tempo[3] = 1.5f;


            grafica_dime[1] = 4;

            grafica_dime[2] = 4;

            grafica_dime[3] = 4;

            suona_effetto_UI(6, 1);

        }



        if (num == 3) {

            crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/sfondo_popUP"); //pannello shop

            crea_grafica_text(201, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/carta_shop 2"); //monete
            crea_grafica_text(202, new Color(1, 1, 1, 0), "", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop

            crea_grafica_text(203, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/carta_shop 7"); //gemme
            crea_grafica_text(204, new Color(1, 1, 1, 0), "", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop



            crea_button_text(208, "Double ALL!", new Color(1, 1, 1, 1), canvas_popup, "Canvas_popup/Panel", ""); //pannello shop

            crea_button_text(209, "Next Level", new Color(1, 1, 1, 1), canvas_popup, "Canvas_popup/Panel", ""); //pannello shop
            crea_button_text(210, "Repeat Level", new Color(1, 1, 1, 1), canvas_popup, "Canvas_popup/Panel", ""); //pannello shop

            if (monete_partita_corrente > 0)
            {
                suona_effetto_UI(7, 1);
            }


            pulsante_testo[208].GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);
            pulsante_testo[209].GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);
            pulsante_testo[210].GetComponent<TextMeshProUGUI>().color = Color.black;



        }

        if (num == 4) {

            crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/sfondo_popUP"); //pannello shop

            crea_grafica_text(201, new Color(1, 1, 1, 0), "ENTER MENU", canvas_popup, "Canvas_popup/Panel", ""); //pannello shop

            crea_button_text(202, "CONFIRM", new Color(1, 1, 1, 0), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //pannello shop

            crea_button_text(203, "", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ExitButton");

           

        }

    }



    public void crea_tutorial(string testo_informativo) {


        canvas_tutorial.SetActive(true);

        crea_grafica_text(100, new Color(1, 1, 1, 0.74f), "", canvas_tutorial, "Canvas_tutorial", "UI/grafica_UI/sfondo_menu");//overlay scuro TODO cambiare grafica
        crea_grafica_text(101, new Color(1, 1, 1, 0), testo_informativo, canvas_tutorial, "Canvas_tutorial", ""); //text testo_informativo
        crea_grafica_text(102, new Color(1, 1, 1, 0), "Tap to continue", canvas_tutorial, "Canvas_tutorial", ""); //text Tap to continue
        crea_grafica_text(103, new Color(1, 1, 1, 1), "", canvas_tutorial, "Canvas_tutorial", "UI/grafica_UI/freccia_sinistra"); //text aarrow left
        crea_grafica_text(104, new Color(1, 1, 1, 1), "", canvas_tutorial, "Canvas_tutorial", "UI/grafica_UI/freccia_destra"); //text aarrow right


    }


    void aggiorna_menu_tutorial() {


        float dy = risoluzione_y;
        float dx = risoluzione_x;
        float pos_x = 0;
        float pos_y = 0;

        if (grafica[100] != null)  //overlay scuro TODO cambiare grafica
        {


            pos_x = 0;
            pos_y = 0;

            grafica[100].GetComponent<RectTransform>().sizeDelta = new Vector2(dx * 0.85f, dy);
            grafica[100].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
        }
        if (grafica[101] != null)  //text tip info
        {


            pos_x = 0;
            pos_y = dy * 0.3f;

            grafica[101].GetComponent<RectTransform>().sizeDelta = new Vector2(dx * 0.85f, dy);
            grafica[101].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
            grafica_testo[101].GetComponent<TextMeshProUGUI>().fontSize = font_size / 0.5f;
        }
        if (grafica[102] != null)  //text Tap to continue
        {


            pos_x = 0;
            pos_y = dy * -0.41f;

            grafica[102].GetComponent<RectTransform>().sizeDelta = new Vector2(dx * 0.85f, dy);
            grafica[102].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
            grafica_testo[102].GetComponent<TextMeshProUGUI>().fontSize = font_size / 0.8f;
        }
        if (grafica[103] != null)  //text aarrow left
        {

            float dx2 = dx * 0.3f;
            float dy2 = dy * 0.3f;
            pos_x = dx2 * -1f;
            pos_y = 0;

            grafica[103].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * 0.85f, dy2);
            grafica[103].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            fade_continuo(grafica[103].GetComponent<Image>());
        }
        if (grafica[104] != null)  //text aarrow right
        {

            float dx2 = dx * 0.3f;
            float dy2 = dy * 0.3f;
            pos_x = dx2 * 1f;
            pos_y = 0;

            grafica[104].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * 0.85f, dy2);
            grafica[104].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
            fade_continuo(grafica[104].GetComponent<Image>());
        }
    }

    [SerializeField] float period = 2f;
    [SerializeField] float movementFactor = 2f;


    private void fade_continuo(Image image) {

        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.realtimeSinceStartup / period;  // continually growing over time

        const float tau = Mathf.PI * 2;  // constant value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau);  // going from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f;   // recalculated to go from 0 to 1 so its cleaner

        image.color = new Color(1, 1, 1, movementFactor);
    }


    public void distruggi_menu_tutorial() {

        canvas_tutorial.SetActive(false);

        for (int n = 100; n < grafica.Length; n++) {
            if (grafica[n] != null) {
                DestroyImmediate(grafica[n]);
            }

        }


    }


    void aggiorna_menu_popup() {


        float dy = risoluzione_y;
        float dx = risoluzione_x;

        float pos_x = 0;
        float pos_y = 0;

        float dime_panel_x = 0;
        float dime_panel_y = 0;

        int font_size = (int)(risoluzione_x / 5.5f);
        int font_size2 = (int)(risoluzione_x / 5);
        int font_size3 = (int)(risoluzione_x / 7.5f);

        if (attivo_popup == 2) {
            font_size3 = (int)(risoluzione_x / 11);

        }

        for (int n = 0; n <= 20; n++) {
            if (grafica[200 + n] != null) {
                grafica[200 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, risoluzione_y);
            }

            if (pulsante[200 + n] != null) {
                pulsante[200 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, risoluzione_y);
            }
        }




        if (attivo_popup == 1 || attivo_popup == 2) {
            if (grafica[200] != null)  //pannello
            {

                float dx2 = dx * .8f;
                float dy2 = dy * .8f;

                float dx_stelle = dx2 * .275f;



                float dy_t = dy * .2f;

                dime_panel_x = dx2;
                dime_panel_y = dy2;

                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);



                grafica_pos[1] = -.13f;
                grafica_pos[2] = -.175f;
                grafica_pos[3] = -.33f;



                for (int n = 4; n <= 12; n++) {
                    grafica_pos[n] = 5.35f;

                }


                if (crea_popup_finale >= 1) {
                    if (energia <= 0)
                    {
                        grafica_pos[1] = .25f;
                        grafica_pos[2] = .1f;
                        grafica_pos[3] = -.2f;

                    }
                    else
                    {
                        grafica_pos[7] = .35f;
                        grafica_pos[10] = .15f;

                    }

                     


                }


                ok_energia_completa = 0;
                ok_spari_completi = 0;

                if (energia > energia_base * script_struttura_dati.livello_in_uso * 0.005f) { // se finisce il livello con energia superiore alla met� del livello attuale, quindi al livello 100, dovr� superare il gioco con il 50% di energia
                    grafica_pos[8] = .35f;
                    grafica_pos[11] = .15f;
                    ok_energia_completa = 1;

                  //  Debug.Log("ok_energia_completa "+ energia);

                }


                if (numero_spari > 0 && energia>0) { // se finisce il livello con almeno 1 sparo
                    grafica_pos[9] = .35f;
                    grafica_pos[12] = .15f;
                    ok_spari_completi = 1;

                 //   Debug.Log("ok_spari_completi "+numero_spari);
                }



                if (ok_fine_livello == 0) {
                    ok_fine_livello = 1;

                    salva_le_stelle();
                }



                for (int n = 1; n <= 12; n++) {
                    if (grafica[200 + n] != null) {

                        grafica_tempo[n] = grafica_tempo[n] - Time.deltaTime;


                        if (grafica_tempo[n] < 0) {

                            //  grafica_pos[n] = Mathf.MoveTowards(grafica_pos[n], 1, Time.deltaTime);

                            grafica_dime[n] = Mathf.MoveTowards(grafica_dime[n], 1, Time.deltaTime * 5);


                            pos_y = dime_panel_y * grafica_pos[n];


                            if (n <= 3) {

                                grafica[200 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy_t);
                                grafica[200 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                            }
                            else {

                                if (n == 4 || n == 7 || n == 10) {
                                    pos_x = dx2 * -.3f;
                                }

                                if (n == 5 || n == 8 || n == 11) {
                                    pos_x = 0;
                                }

                                if (n == 6 || n == 9 || n == 12) {
                                    pos_x = dx2 * .3f;
                                }


                                grafica[200 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx_stelle, dx_stelle);
                                grafica[200 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);



                            }


                            grafica[200 + n].transform.localScale = new Vector3(grafica_dime[n], grafica_dime[n], 1);



                            if (n == 1) {
                                grafica_testo[200 + n].GetComponent<TextMeshProUGUI>().fontSize = font_size;
                            }
                            if (n == 2) {
                                grafica_testo[200 + n].GetComponent<TextMeshProUGUI>().fontSize = font_size2;
                            }
                            if (n == 3) {
                                grafica_testo[200 + n].GetComponent<TextMeshProUGUI>().fontSize = font_size3;
                            }

                        }

                    }

                }

            }

            if (grafica_tempo[12] < -2) {

                crea_popup(3);

            }
        }

        if (attivo_popup == 3) {
            if (grafica[200] != null)  // monete
            {

                float dx2 = dx * .8f;
                float dy2 = dy * .8f;



                float dy_t = dy * .2f;

                dime_panel_x = dx2;
                dime_panel_y = dy2;

                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);


                pos_x = dime_panel_x * -.2f;
                pos_y = dime_panel_y * .3f;

                grafica[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .4f, dx2 * .4f);
                grafica[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                pos_x = dime_panel_x * .2f;
                grafica[202].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .4f, dx2 * .4f);
                grafica[202].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                grafica_testo[202].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 10);

                monete_UI = Mathf.MoveTowards(monete_UI, monete_partita_corrente, Time.deltaTime * 100);



                grafica_testo[202].GetComponent<TextMeshProUGUI>().text = "" + (int)(monete_UI);


                pos_x = dime_panel_x * -.2f;
                pos_y = dime_panel_y * .13f;

                grafica[203].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .4f, dx2 * .4f);
                grafica[203].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                pos_x = dime_panel_x * .2f;
                grafica[204].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .4f, dx2 * .4f);
                grafica[204].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                grafica_testo[204].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 10);

                gemme_UI = Mathf.MoveTowards(gemme_UI, gemme_partita_corrente, Time.deltaTime * 100);



                grafica_testo[204].GetComponent<TextMeshProUGUI>().text = "" + (int)(gemme_UI);



                if (pulsante[208] != null)  //raddoppia
                {
                    float dx3 = risoluzione_x * .5f;
                    float dy3 = risoluzione_x * .15f;
                    pos_x = 0;
                    pos_y = dime_panel_y * -.15f;


                    pulsante[208].GetComponent<RectTransform>().sizeDelta = new Vector2(dx3, dy3);
                    pulsante[208].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                    pulsante_testo[208].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 13f);
                }

                if (pulsante[209] != null && energia > 0)  //next level
                {
                    float dx3 = risoluzione_x * .5f;
                    float dy3 = risoluzione_x * .15f;
                    pos_x = 0;
                    pos_y = dime_panel_y * -.35f;


                    pulsante[209].GetComponent<RectTransform>().sizeDelta = new Vector2(dx3, dy3);
                    pulsante[209].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                    pulsante_testo[209].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 13f);
                }


                if (pulsante[210] != null && energia <= 0)  //repeat level
                {
                    float dx3 = risoluzione_x * .5f;
                    float dy3 = risoluzione_x * .15f;
                    pos_x = 0;
                    pos_y = dime_panel_y * -.35f;


                    pulsante[210].GetComponent<RectTransform>().sizeDelta = new Vector2(dx3, dy3);
                    pulsante[210].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                    pulsante_testo[210].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 13f);
                }




            }

        }



        if (attivo_popup == 4) {
            if (grafica[200] != null)  //pannello
            {

                float dx2 = dx * .8f;
                float dy2 = dy * .8f;



                float dy_t = dy * .2f;

                dime_panel_x = dx2;
                dime_panel_y = dy2;

                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                pos_y = dime_panel_y * .3f;

                grafica[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .9f, dx2 * .7f);
                grafica[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);


                grafica_testo[201].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 10);

                pos_y = dime_panel_y * -.3f;


                if (pulsante[202] != null)  //exit
                {
                    pulsante[202].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .6f, dy2 * .12f);
                    pulsante[202].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);


                    pulsante_testo[202].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 25);

                    pulsante_testo[202].GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);

                }


                if (pulsante[203] != null)  //exit
                {
                    float dx3 = risoluzione_x * 0.12f;
                    float dy3 = dx3;
                    pos_x = dime_panel_x * .465f;
                    pos_y = dime_panel_y * .465f;


                    pulsante[203].GetComponent<RectTransform>().sizeDelta = new Vector2(dx3, dy3);
                    pulsante[203].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                }


                uscita_popup(dime_panel_x, dime_panel_y);

            }

        }


    }



    void salva_le_stelle() {


        if (script_struttura_dati.stelle_livello[script_struttura_dati.livello_in_uso].Substring(1, 1) == "1") {
            ok_energia_completa = 1;
        }
        if (script_struttura_dati.stelle_livello[script_struttura_dati.livello_in_uso].Substring(2, 1) == "1") {
            ok_spari_completi = 1;
        }

        script_struttura_dati.stelle_livello[script_struttura_dati.livello_in_uso] = $"1{ok_energia_completa}{ok_spari_completi}";
        PlayerPrefs.SetString($"stelle_livello{script_struttura_dati.livello_in_uso}", script_struttura_dati.stelle_livello[script_struttura_dati.livello_in_uso]);
        ok_energia_completa = 0;
        ok_spari_completi = 0;

    }




    void uscita_popup(float dime_panel_x, float dime_panel_y) {
        resetTimerPopup += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && resetTimerPopup > 0.5f && energia <= 0) {
            if (crea_popup_finale == 10) {

                SceneManager.LoadScene("gioco");
            }

        }


        if (Input.GetMouseButtonDown(0) && resetTimerPopup > 0.5f && energia > 0) {
            resetTimerPopup = 0;
            float dx = (risoluzione_x - dime_panel_x) * .5f;
            float dy = (risoluzione_y - dime_panel_y) * .5f;


            if (xm < dx || xm > risoluzione_x - dx) {
                distruggi_menu_popup();
            }


            if (ym < dy || ym > risoluzione_y - dy) {
                distruggi_menu_popup();
            }




        }





    }


    void distruggi_menu() {



        for (int n = 0; n < 200; n++) {
            if (pulsante[n] != null) {
                DestroyImmediate(pulsante[n]);
            }

        }

        for (int n = 0; n < 200; n++) {
            if (grafica[n] != null) {
                DestroyImmediate(grafica[n]);
            }

        }


    }


    void distruggi_menu_popup() {


        attivo_popup = 0;

        canvas_popup.SetActive(false);

        for (int n = 200; n < pulsante.Length; n++) {
            if (pulsante[n] != null) {
                DestroyImmediate(pulsante[n]);
            }

        }

        for (int n = 200; n < grafica.Length; n++) {
            if (grafica[n] != null) {
                DestroyImmediate(grafica[n]);
            }

        }


    }


    void gestione_fine_gioco() {

        if (crea_popup_finale == 0) {

            Vector3 pos_finale = c_save.crea_cilindro[0].pos_finale;

            //  Debug.Log(pos_finale +" "+ cilindro.transform.position.z);



            if (cilindro.transform.position.z < -pos_finale.z) {


                crea_popup_finale = 1;
                crea_popup_finale_tempo = 2.2f;

            }

        }



        if (crea_popup_finale == 1) {
            crea_popup_finale_tempo = crea_popup_finale_tempo - Time.deltaTime;


            tempo_fire_work = tempo_fire_work - Time.deltaTime;

            if (tempo_fire_work < 0) {
                tempo_fire_work = UnityEngine.Random.Range(.1f, .3f);


                float xx = UnityEngine.Random.Range(-5, 5);
                float yy = UnityEngine.Random.Range(5, 7) + c_save.crea_cilindro[0].raggio;
                float zz = UnityEngine.Random.Range(-1, 1);


                Vector3 pos0 = new Vector3(xx, yy, zz);

                int tipo_firework = (int)(UnityEngine.Random.Range(0, 3.49f));

                carica_particles("particles/Firework Variants/CFXM_Firework " + tipo_firework, pos0);


            }



            if (crea_popup_finale_tempo < 0) {
                crea_popup_finale = 2;
                crea_popup(1);
            }
        }

    }


    public IEnumerator leggi_dati_online(string nome_pagina) {


        string path2 = path_server + nome_pagina + ".json";

        Debug.Log("" + path2);


        using (UnityWebRequest www = UnityWebRequest.Get(path2)) {


            yield return www.SendWebRequest();



            if (www.isNetworkError || www.isHttpError) {
                Debug.Log("errore " + www.error);
            }
            else {



                Debug.Log("" + www.downloadHandler.text);

                try {


                    string testo_json = www.downloadHandler.text;


                    c_save_p = null;

                    c_save_p = new classe_save_parametri();


                    c_save_p = JsonUtility.FromJson<classe_save_parametri>(www.downloadHandler.text);


                    cilindro.GetComponent<Renderer>().material.SetColor("_Color", c_save_p.crea_parametri[0].colore_cilindro);
                    sfondo.GetComponent<Renderer>().material.SetColor("_Color", c_save_p.crea_parametri[0].colore_sfondo);
                    sfondo2.GetComponent<Renderer>().material.SetColor("_Color", c_save_p.crea_parametri[0].colore_sfondo);


                    int livello_uso = 1;

                    if (script_struttura_dati != null) {

                        livello_uso = script_struttura_dati.livello_in_uso;
                    }


#if UNITY_EDITOR
                    if (level_json != null) {

                        string nome_livello = level_json.name;
                        nome_livello = nome_livello.Replace("livello_", "");

                        livello_uso = int.Parse(nome_livello);

                        if (script_struttura_dati != null) {
                            script_struttura_dati.livello_in_uso = livello_uso;

                        }
                    }

#endif



                    if (online_dati == true) {


                        StartCoroutine(load_project_online(livello_uso)); //TODO mettere il livello da script_struttura_dati
                    }
                    else {

                        load_project();


                    }



                    carica_dati_online = 1;


                }
                catch (Exception e) {

                    Debug.Log("errore_oggetto_codice " + e.ToString());

                }



            }


        }


    }



    void save_parameter() {




#if UNITY_EDITOR


        c_save_p = new classe_save_parametri();

        c_save_p.crea_parametri.Add(new parametri());

        c_save_p.crea_parametri[0].posizione_touch = .2f;

        c_save_p.crea_parametri[0].colore_sfondo = new Color(.4218f, .6914f, .9257f, 1);
        c_save_p.crea_parametri[0].colore_cilindro = new Color(0.6835f, 0.9335f, 0.4531f, 1);

        string jsonData = JsonUtility.ToJson(c_save_p, true);

        string file_name = "parametri";




        string path_data = "Assets/Resources/data_level/" + file_name + ".json";


        File.WriteAllText(path_data, jsonData, Encoding.UTF8);


        AssetDatabase.Refresh();

#endif



    }


    void aggiorna_potere_boss() {

        for (int n = 0; n < 4; n++) {

            boss_potere[n] = boss_potere[n] - Time.deltaTime;

            if (boss_potere_attivo[n] == 1) {



                if (boss_potere[n] >= 6.0f && boss_potere[n] <= 6.5f) {
                    boss_potere_altezza[n] = boss_potere_altezza[n] + Time.deltaTime * 5;

                    update_potere_boss(n, boss_potere_altezza[n]);

                }


                if (boss_potere[n] < 0) {
                    boss_potere_attivo[n] = 1;

                    DestroyImmediate(boss_potere_mesh[n]);
                }

            }


        }



    }



    void crea_potere_boss() {
        int che_potere = -1;

        for (int n = 0; n < 4; n++) {


            if (boss_potere[n] < 0) {
                che_potere = n;
                break;
            }


        }


        if (che_potere > -1) {

            boss_potere_attivo[che_potere] = 1;

            boss_potere[che_potere] = 8;

            crea_potere_boss(che_potere, 0.15f);
        }



    }


    void crea_potere_boss(int num, float altezza = 0) {
        if (boss_potere_mesh[num] != null) {
            DestroyImmediate(boss_potere_mesh[num]);

        }

        boss_potere_mesh[num] = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_mesh_potere", typeof(GameObject))) as GameObject;

        boss_potere_mesh[num].name = "boss_potere_mesh " + num;

        boss_potere_mesh[num].transform.SetParent(cilindro.transform);

        boss_potere_mesh[num].GetComponent<Renderer>().material.SetColor("_Color", c_save_p.crea_parametri[0].colore_blocco_1);


        Mesh mesh = boss_potere_mesh[num].GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        int num_v = 16;
        int num_tria = 8;

        Vector3[] vertices = new Vector3[num_v];
        Vector2[] uvs = new Vector2[num_v];
        int[] tria = new int[num_tria * 3];


        float raggio = c_save.crea_cilindro[0].raggio;

        float divisore = Mathf.PI * 2 / 18.0f + cilindro.transform.localEulerAngles.y * Mathf.Deg2Rad;

        boss_potere_rad[num] = divisore;


        float x0 = Mathf.Sin(0) * raggio;
        float y0 = Mathf.Cos(0) * raggio;

        float x1 = Mathf.Sin(divisore) * raggio;
        float y1 = Mathf.Cos(divisore) * raggio;

        float x2 = Mathf.Sin(0) * (raggio + altezza);
        float y2 = Mathf.Cos(0) * (raggio + altezza);

        float x3 = Mathf.Sin(divisore) * (raggio + altezza);
        float y3 = Mathf.Cos(divisore) * (raggio + altezza);




        vertices[0] = new Vector3(x0, y0, 0);
        vertices[1] = new Vector3(x2, y2, 0);

        vertices[2] = new Vector3(x0, y0, 100);
        vertices[3] = new Vector3(x2, y2, 100);


        vertices[4] = new Vector3(x2, y2, 0);
        vertices[5] = new Vector3(x3, y3, 0);

        vertices[6] = new Vector3(x2, y2, 100);
        vertices[7] = new Vector3(x3, y3, 100);

        vertices[8] = new Vector3(x1, y1, 0);
        vertices[9] = new Vector3(x3, y3, 0);

        vertices[10] = new Vector3(x1, y1, 100);
        vertices[11] = new Vector3(x3, y3, 100);

        vertices[12] = new Vector3(x0, y0, 100);
        vertices[13] = new Vector3(x1, y1, 100);
        vertices[14] = new Vector3(x2, y2, 100);
        vertices[15] = new Vector3(x3, y3, 100);


        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(1, 1);

        uvs[4] = new Vector2(0, 0);
        uvs[5] = new Vector2(0, 1);
        uvs[6] = new Vector2(1, 0);
        uvs[7] = new Vector2(1, 1);

        uvs[8] = new Vector2(0, 0);
        uvs[9] = new Vector2(0, 1);
        uvs[10] = new Vector2(1, 0);
        uvs[11] = new Vector2(1, 1);

        uvs[12] = new Vector2(0, 0);
        uvs[13] = new Vector2(0, 1);
        uvs[14] = new Vector2(1, 0);
        uvs[15] = new Vector2(1, 1);


        tria[0] = 2;
        tria[1] = 3;
        tria[2] = 1;

        tria[3] = 2;
        tria[4] = 1;
        tria[5] = 0;

        tria[6] = 4;
        tria[7] = 6;
        tria[8] = 5;

        tria[9] = 6;
        tria[10] = 7;
        tria[11] = 5;

        tria[12] = 9;
        tria[13] = 11;
        tria[14] = 10;

        tria[15] = 9;
        tria[16] = 10;
        tria[17] = 8;


        tria[18] = 14;
        tria[19] = 13;
        tria[20] = 12;

        tria[21] = 13;
        tria[22] = 15;
        tria[23] = 14;


        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = tria;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();



    }




    void update_potere_boss(int num, float altezza) {


        Mesh mesh = boss_potere_mesh[num].GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        int num_v = 16;
        int num_tria = 8;

        Vector3[] vertices = new Vector3[num_v];
        Vector2[] uvs = new Vector2[num_v];
        int[] tria = new int[num_tria * 3];


        float raggio = c_save.crea_cilindro[0].raggio;

        float divisore = boss_potere_rad[num];




        float x0 = Mathf.Sin(0) * raggio;
        float y0 = Mathf.Cos(0) * raggio;

        float x1 = Mathf.Sin(1 * divisore) * raggio;
        float y1 = Mathf.Cos(1 * divisore) * raggio;

        float x2 = Mathf.Sin(0) * (raggio + altezza);
        float y2 = Mathf.Cos(0) * (raggio + altezza);

        float x3 = Mathf.Sin(1 * divisore) * (raggio + altezza);
        float y3 = Mathf.Cos(1 * divisore) * (raggio + altezza);



        vertices[0] = new Vector3(x0, y0, 0);
        vertices[1] = new Vector3(x2, y2, 0);

        vertices[2] = new Vector3(x0, y0, 100);
        vertices[3] = new Vector3(x2, y2, 100);


        vertices[4] = new Vector3(x2, y2, 0);
        vertices[5] = new Vector3(x3, y3, 0);

        vertices[6] = new Vector3(x2, y2, 100);
        vertices[7] = new Vector3(x3, y3, 100);

        vertices[8] = new Vector3(x1, y1, 0);
        vertices[9] = new Vector3(x3, y3, 0);

        vertices[10] = new Vector3(x1, y1, 100);
        vertices[11] = new Vector3(x3, y3, 100);

        vertices[12] = new Vector3(x0, y0, 100);
        vertices[13] = new Vector3(x1, y1, 100);
        vertices[14] = new Vector3(x2, y2, 100);
        vertices[15] = new Vector3(x3, y3, 100);

        vertices[12] = new Vector3(x0, y0, 100);
        vertices[13] = new Vector3(x1, y1, 100);
        vertices[14] = new Vector3(x2, y2, 100);
        vertices[15] = new Vector3(x3, y3, 100);

        float molt = 10;

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(molt, 0);
        uvs[3] = new Vector2(molt, 1);

        uvs[4] = new Vector2(0, 0);
        uvs[5] = new Vector2(0, 1);
        uvs[6] = new Vector2(molt, 0);
        uvs[7] = new Vector2(molt, 1);

        uvs[8] = new Vector2(0, 0);
        uvs[9] = new Vector2(0, 1);
        uvs[10] = new Vector2(molt, 0);
        uvs[11] = new Vector2(molt, 1);

        uvs[12] = new Vector2(0, 0);
        uvs[13] = new Vector2(0, 1);
        uvs[14] = new Vector2(1, 0);
        uvs[15] = new Vector2(1, 1);


        tria[0] = 2;
        tria[1] = 3;
        tria[2] = 1;

        tria[3] = 2;
        tria[4] = 1;
        tria[5] = 0;

        tria[6] = 4;
        tria[7] = 6;
        tria[8] = 5;

        tria[9] = 6;
        tria[10] = 7;
        tria[11] = 5;

        tria[12] = 9;
        tria[13] = 11;
        tria[14] = 10;

        tria[15] = 9;
        tria[16] = 10;
        tria[17] = 8;


        tria[18] = 13;
        tria[19] = 14;
        tria[20] = 12;

        tria[21] = 13;
        tria[22] = 15;
        tria[23] = 14;


        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = tria;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        DestroyImmediate(boss_potere_mesh[num].GetComponent<MeshCollider>());


        boss_potere_mesh[num].AddComponent<MeshCollider>();

    }




    void carica_effetto_UI(int num, string path) {

        if (effetto_source_UI_caricato[num] == 0) {
            effetto_source_UI_caricato[num] = 1;

            effetto_source_UI[num] = Instantiate(Resources.Load(path) as GameObject).GetComponent<AudioSource>();
            effetto_source_UI[num].transform.parent = cam0.transform;
            effetto_source_UI[num].transform.localPosition = new Vector3(0, 0, 0);
        }

    }


    void carica_musica(string path) {

        musica = Instantiate(Resources.Load(path) as GameObject).GetComponent<AudioSource>();
        musica.transform.parent = cam0.transform;
        musica.transform.localPosition = new Vector3(0, 0, 0);

        musica.volume = 0;

    }


    void suona_effetto_UI(int tipologia = 1, float volume = 0) {

        if (script_struttura_dati != null) {
            if (script_struttura_dati.disattiva_effetti == 0) {


                effetto_source_UI[tipologia].Play();
                effetto_source_UI[tipologia].volume = volume;

            }
        }

    }


    void aggiorna_audio() {

        float volume_musica = 0;

        if (script_struttura_dati != null) {

            if (script_struttura_dati.disattiva_musica == 0) {
                volume_musica = 1;
            }

        }

        musica.volume = volume_musica;

    }

}


