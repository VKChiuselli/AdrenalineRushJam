using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.UI;
using TMPro;

public class gioco_ruota_cilindro : MonoBehaviour
{


#if UNITY_EDITOR
    [SerializeField]
#endif
    public classe_save c_save;


    public TextAsset level_json;

    float xm, ym, xm_old, ym_old;



    float[] touch_x = new float[15];
    float[] touch_y = new float[15];

    float[] touch_rx = new float[15];
    float[] touch_ry = new float[15];

    float[] touch_xo = new float[15];
    float[] touch_yo = new float[15];

    public float potenza_tasto = 1;
    public float velocita_personaggio = 1;
    public float aumento_velo = 1;

    public float velocita_bonus_base = 2;
    public float velocita_bonus = 1;

    public int inversione_camera = 1;

    public float velocita_sparo = 20;
    public float velocita_sparo_boss = 20;

    public int numero_spari = 300;

    public GameObject astronave;

    public GameObject boss_mesh;
    public GameObject boss_mesh_sparo;

    public float spostamento_boss_rotazione = .01f;

    public Color newColor = new Color(0, 1, 1, 1);

    public float jump = 15;
    public float distanza_disolve = -15;

    public float energia = 100;
    public int monete_partita_corrente = 0;

    float risoluzione_x;
    float risoluzione_y;

    float rapporto_risoluzione;
    float spostamento_sx;
    float spostamento_sx2;

    float diff_xm;
    float diff_ym;

    int barriera = 0;

    float pressione_tasto = 0;

    GameObject[] pulsante = new GameObject[100];
    GameObject[] pulsante_testo = new GameObject[100];

    GameObject[] grafica = new GameObject[100];
    GameObject[] grafica_testo = new GameObject[100];

    GameObject[] pulsante_field = new GameObject[100];
    GameObject[] pulsante_field_testo = new GameObject[100];
    GameObject[] pulsante_field_testo2 = new GameObject[100];


    struttura_dati script_struttura_dati;

    GameObject canvas;
    GameObject canvas_popup;

    GameObject ogg_struttura_dati;

    CharacterController characterController;

    GameObject cilindro;




    float rotazione_cilindro = 0;

    int blocco_velocita = 1;

    float inversione_controllo = 0;


    GameObject cam0;
    Vector3 cam_pos, cam_rot;

    Vector3[] vertici_cilindro = new Vector3[12000];

    Vector3 playerVelocity = new Vector3(0, 0, 0);

    float astronave_rx = 0;
    float astronave_rx_calcolo = 0;

    float astronave_rz = 0;
    float astronave_rz_calcolo = 0;

    GameObject[] snap_rif = new GameObject[3000];

    int crea_snap = 0;

    float distanza_corda = 0;

    float aumento_salto = 1;

    GameObject boss;
    float boss_rad;

    float[] attivo_tempo_sparo_boss = new float[10];

    GameObject[] sparo_boss = new GameObject[10];

    float[] sparo_boss_rad = new float[10];

    GameObject[] sparo= new GameObject[10];
    float[] attivo_tempo_sparo= new float[10];

    float attivo_tempo_sparo_personaggio = 0;
    float attivo_tempo_sparo_boss2 = 0;

    float spostamento_z = 1;
    float spostamento_z2 = 1;

    int font_size = 15;

    int gemme_prese = 0;
  


    // Start is called before the first frame update
    void Start()
    {

        controllo_risoluzione();

        characterController = GetComponent<CharacterController>();
        cilindro = GameObject.Find("cilindro_esatto");

        boss = GameObject.Find("boss");

        cam0 = GameObject.Find("Main Camera");

        leggi_vertici_cilindro();



        for (int k = 0; k <= 9; k++)
        {

            for (int n = 0; n < 40; n++)
            {

                snap_rif[n + k * 50] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere_rif", typeof(GameObject))) as GameObject;

                snap_rif[n + k * 50].name = "snap_rif " + n;

            }

        }


        load_project();

        canvas = GameObject.Find("Canvas");
        canvas_popup = GameObject.Find("Canvas_popup/Panel");

        canvas_popup.SetActive(false);


        ogg_struttura_dati = GameObject.Find("base_struttura");

        if (ogg_struttura_dati != null)
        {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }


        cam_pos = new Vector3(0, 1.35f, 13.0f);
        cam_rot = new Vector3(23, 180, 0);


        crea_menu();

    }

    // Update is called once per frame
    void Update()
    {
        controllo_risoluzione();

        controllo();

        gestione_camera();

        gestione_boss();

        aggiorna_blocco();

        gestione_cilindro();

        gestione_collisione();


        aggiorna_menu();

    }


    void leggi_vertici_cilindro()
    {



        Mesh mesh = cilindro.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            vertici_cilindro[i] = vertices[i];
        }


    }


    void scala_cilindro()
    {



        Mesh mesh = cilindro.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;


        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertici_cilindro[i] * c_save.crea_cilindro[0].raggio;
        }

        mesh.vertices = vertices;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();


        DestroyImmediate(cilindro.GetComponent<MeshCollider>());


        cilindro.AddComponent<MeshCollider>();


    }


    void crea_sparo()
    {


        Debug.Log("sparo creato ");

        numero_spari = numero_spari - 1;


        int num_sparo = -1;

        for (int n = 0; n < 5; n++)
        {

            if (attivo_tempo_sparo[n] < -1)
            {
                num_sparo = n;

                attivo_tempo_sparo[n] = 5;


                sparo[n] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sparo_personaggio", typeof(GameObject))) as GameObject;

                Vector3 pos_astronave = astronave.transform.position;

                sparo[n].transform.position = new Vector3(pos_astronave.x, pos_astronave.y, pos_astronave.z + 2);


                sparo[n].name = "sparo_personaggio " + n;

                attivo_tempo_sparo_personaggio = .5f;

                n = 1000;


            }

        }


    }



    void controllo()
    {

        if (energia < 0)
        {
            blocco_velocita = 0;

        }


        float pressione_tasto_up = Input.GetAxis("Vertical");

        attivo_tempo_sparo_personaggio = attivo_tempo_sparo_personaggio - Time.deltaTime;


        if (pressione_tasto_up < 0 && attivo_tempo_sparo_personaggio < 0 && numero_spari > 0 && blocco_velocita == 1)
        {
            crea_sparo();

        }


        if (pressione_tasto_up > 0 )
        {
         //   aumento_velo = aumento_velo +.25f;


        }



        astronave.transform.position = new Vector3(0, c_save.crea_cilindro[0].raggio  + .5f, 0);



        pressione_tasto = Input.GetAxis("Horizontal");

        if (Mathf.Abs(pressione_tasto) > 0 && blocco_velocita == 1)
        {

            int molt_inversione = 1;

            if (inversione_controllo == 1)
            {
                molt_inversione = -1;

            }

            if (inversione_camera == 0)
            {
                molt_inversione = molt_inversione * -1;

            }



            rotazione_cilindro = pressione_tasto * potenza_tasto * Time.deltaTime * molt_inversione;


            astronave_rz_calcolo = astronave_rz_calcolo + pressione_tasto;

            if (astronave_rz_calcolo > 30)
            {
                astronave_rz_calcolo = 30;

            }

            if (astronave_rz_calcolo < -30)
            {
                astronave_rz_calcolo = -30;

            }


            boss_rad = boss_rad + rotazione_cilindro * spostamento_boss_rotazione;


            for (int n = 0; n < 5; n++)
            {

                if (sparo_boss[n] != null)
                {

                    sparo_boss_rad[n]= sparo_boss_rad[n] + rotazione_cilindro * spostamento_boss_rotazione;

                }

            }


                        cilindro.transform.Rotate(new Vector3(0, 0, rotazione_cilindro));

        }


       

        astronave_rz = Mathf.LerpAngle(astronave_rz, astronave_rz_calcolo, Time.deltaTime * 15);


        astronave.transform.localEulerAngles = new Vector3(0, 0, astronave_rz);

        astronave_rz_calcolo = astronave_rz_calcolo * .9f;


        gestione_sparo();



    }


    void gestione_sparo()
    {


        for (int n = 0; n < 5; n++)
        {
            attivo_tempo_sparo[n] = attivo_tempo_sparo[n] - Time.deltaTime;

            if (sparo[n] != null)
            {
                

                if (attivo_tempo_sparo[n] > 0)
                {


                    sparo[n].transform.Translate(new Vector3(0, 0, velocita_sparo * Time.deltaTime));

                    gestione_collisione_sparo(n, sparo[n]);

                }
                else
                {
                    DestroyImmediate(sparo[n]);
                }

            }

        }

    }



    void gestione_sparo_boss()
    {

        for (int n = 0; n < 5; n++)
        {
            attivo_tempo_sparo_boss[n] = attivo_tempo_sparo_boss[n] - Time.deltaTime;



            if (sparo_boss[n] != null)
            {


                if (attivo_tempo_sparo_boss[n] > 0)
                {

                    float xx = Mathf.Sin(sparo_boss_rad[n]) * (c_save.crea_cilindro[0].raggio+1);
                    float yy = Mathf.Cos(sparo_boss_rad[n]) * (c_save.crea_cilindro[0].raggio+1);

                    float zz = sparo_boss[n].transform.position.z;

                    sparo_boss[n].transform.position = new Vector3(xx, yy, zz+ velocita_sparo_boss * Time.deltaTime);

                    //  sparo_boss[n].transform.Translate(new Vector3(0, 0, velocita_sparo_boss * Time.deltaTime));

                    //  sparo_boss[n].transform

                    float px = sparo_boss[n].transform.position.x-astronave.transform.position.x;
                    float py = sparo_boss[n].transform.position.y - astronave.transform.position.y;

                   
                    

                    gestione_collisione_sparo(n, sparo_boss[n],1);


                    



                }
                else
                {
                    DestroyImmediate(sparo_boss[n]);
                }

            }

        }

    }




    void gestione_camera()
    {





        float altezza_cam = 3.5f;


        if (inversione_camera == 0)
        {
            cam_pos.z = Mathf.Lerp(cam_pos.z, -13, Time.deltaTime * 5);

            cam_rot.y = Mathf.Lerp(cam_rot.y, 0, Time.deltaTime * 50);

        }

        if (inversione_camera == 1)
        {
            cam_pos.z = Mathf.Lerp(cam_pos.z, 27, Time.deltaTime * 5);

            cam_rot.y = Mathf.Lerp(cam_rot.y, 180, Time.deltaTime * 50);

            altezza_cam = 3;

        }


        cam0.transform.localPosition = new Vector3(0, c_save.crea_cilindro[0].raggio + altezza_cam, cam_pos.z);
        cam0.transform.localEulerAngles = cam_rot;

    }





    void gestione_cilindro()
    {

        velocita_bonus = Mathf.Lerp(velocita_bonus, 1, Time.deltaTime * .333f);
        aumento_velo = Mathf.Lerp(aumento_velo, 1, Time.deltaTime * .333f);

        cilindro.transform.Translate(new Vector3(0, 0, -velocita_personaggio * Time.deltaTime * blocco_velocita * velocita_bonus* aumento_velo));


    }




    void crea_bonus_gemma(float pos_z, float angolo)
    {


        GameObject bonus_gemma = Instantiate(Resources.Load("grafica_3d/Prefabs_space/gemma")) as GameObject;

        bonus_gemma.name = "bonus_gemma";

        bonus_gemma.transform.SetParent(cilindro.transform);

        float rad = angolo * Mathf.Deg2Rad;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        bonus_gemma.transform.position = new Vector3(xx, yy, pos_z);

        bonus_gemma.transform.localEulerAngles = new Vector3(0, 0, angolo);


    }


    void crea_bonus_speed(float pos_z, float angolo)
    {


        GameObject bonus_speed = Instantiate(Resources.Load("grafica_3d/Prefabs_space/speed")) as GameObject;

        bonus_speed.name = "bonus_speed";

        bonus_speed.transform.SetParent(cilindro.transform);

        float rad = angolo * Mathf.Deg2Rad;

        float xx = Mathf.Sin(rad) * 0.01f;
        float yy = Mathf.Cos(rad) * 0.01f;

        bonus_speed.transform.position = new Vector3(xx, yy, pos_z);

        bonus_speed.transform.localEulerAngles = new Vector3(0, 0, angolo);


    }








    void aggiorna_blocco()
    {

        float pos_z = cilindro.transform.position.z;

        int num_blocco = c_save.crea_blocco.Count;


        for (int n = 0; n < num_blocco; n++)
        {

            if (c_save.crea_blocco[n].attivo == 1)
            {

                float zz2 = c_save.crea_blocco[n].mesh.transform.position.z;


                if (c_save.crea_blocco[n].distruzione_oggetto > 0)
                {

                    c_save.crea_blocco[n].forza_impatto = c_save.crea_blocco[n].forza_impatto * .95f - .015f;

                    if (c_save.crea_blocco[n].forza_impatto > .025f)
                    {
                        spostamento_blocco(c_save.crea_blocco[n].mesh, c_save.crea_blocco[n].punto_impatto, c_save.crea_blocco[n].forza_impatto, c_save.crea_blocco[n].rad);
                    }

                    c_save.crea_blocco[n].valore_dissolve = c_save.crea_blocco[n].valore_dissolve + (2+ c_save.crea_blocco[n].distruzione_oggetto) * Time.deltaTime;

                    //  Debug.Log("entra " + n+"  "+ c_save.crea_blocco[n].valore_dissolve);

                    aggiorna_oggetto_dissolto_singolo(n, c_save.crea_blocco[n].valore_dissolve);

                    if (c_save.crea_blocco[n].valore_dissolve > 1)
                    {
                        c_save.crea_blocco[n].valore_dissolve = 1;
                        c_save.crea_blocco[n].attivo = 0;

                        DestroyImmediate(c_save.crea_blocco[n].mesh);
                    }


                }



                if (c_save.crea_blocco[n].arrivo == 0 && zz2 < distanza_disolve)
                {




                    c_save.crea_blocco[n].valore_dissolve = c_save.crea_blocco[n].valore_dissolve + 2 * Time.deltaTime;

                    //  Debug.Log("entra " + n+"  "+ c_save.crea_blocco[n].valore_dissolve);

                    aggiorna_oggetto_dissolto_singolo(n, c_save.crea_blocco[n].valore_dissolve);

                    if (c_save.crea_blocco[n].valore_dissolve > 1)
                    {
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




    void gestione_pos(int num)
    {
        float rad = c_save.crea_blocco[num].rad;


        //  float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_blocco[num].altezza;
        float yy = Mathf.Cos(rad) * c_save.crea_blocco[num].altezza;


        float zz = c_save.crea_blocco[num].pos;

        c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


    }



    private void OnTriggerEnter(Collider other)
    {
          Debug.Log(""+other.name);

        /*

        if (other.name.IndexOf ("moneta")>-1)
        {
            if (ogg_struttura_dati != null)
            {
                script_struttura_dati.monete = script_struttura_dati.monete + 1;
                monete_partita_corrente = monete_partita_corrente + 1;

            Debug.Log(" script_struttura_dati.monete " + script_struttura_dati.monete);
            }


            Destroy(other.gameObject);
        }

        if (other.name.IndexOf("gemma") > -1)
        {
            Destroy(other.gameObject);

        }


      

        if (other.name.IndexOf("malus_") > -1)
        {

            string nome = other.name;

            nome = nome.Replace("malus_","");

            int numero_malus_inversion = int.Parse(nome);


          //  Debug.Log("entra "+ numero_malus_inversion);

            if (c_save.crea_malus[numero_malus_inversion].tipo == 0)
            {

                if (c_save.crea_malus[numero_malus_inversion].inversion_controller == 0)
                {
                    c_save.crea_malus[numero_malus_inversion].inversion_controller = 1;
                    inversione_controllo = 1 - inversione_controllo;

                }

            }

        }

    */


    }



    void OnCollisionEnter(Collision collision)
    {


        Debug.Log("collision " + collision.collider.name);

        blocco_velocita = 0;

    }


    void load_project()
    {




        try
        {

            if (level_json != null)
            {


                if (c_save != null)
                {

                    distruggi_oggetti();

                }



                c_save = null;

                c_save = new classe_save();


                c_save = JsonUtility.FromJson<classe_save>(level_json.text);


                spostamento_z = c_save.crea_cilindro[0].spostamento_z;
                spostamento_z2 = c_save.crea_cilindro[0].spostamento_z2;



                calcolo_snap(1);

                scala_cilindro();


                int num_blocchi = c_save.crea_blocco.Count;

                for (int k = 0; k < num_blocchi; k++)
                {
                    crea_blocco(k);
                }

                int num_gemme = c_save.crea_gemma.Count;

                for (int k = 0; k < num_gemme; k++)
                {
                    crea_gemma(k);
                }


                int num_monete = c_save.crea_moneta.Count;

                for (int k = 0; k < num_monete; k++)
                {
                    crea_moneta(k);
                }

                int num_malus = c_save.crea_malus.Count;

                for (int k = 0; k < num_malus; k++)
                {
                    crea_malus(k);
                }

                int num_bonus = c_save.crea_bonus.Count;

                for (int k = 0; k < num_bonus; k++)
                {
                    crea_bonus(k);
                }



            }


        }
        catch (Exception e)
        {
            print("error " + e.ToString());
        }


    }




    void distruggi_oggetti()
    {

        int num_blocchi2 = c_save.crea_blocco.Count;

        for (int k = 0; k < num_blocchi2; k++)
        {
            if (c_save.crea_blocco[k].mesh != null)
            {
                DestroyImmediate(c_save.crea_blocco[k].mesh);

            }

        }

        int num_gemma2 = c_save.crea_gemma.Count;

        for (int k = 0; k < num_gemma2; k++)
        {
            if (c_save.crea_gemma[k].mesh != null)
            {
                DestroyImmediate(c_save.crea_gemma[k].mesh);

            }

        }

        int num_moneta2 = c_save.crea_moneta.Count;

        for (int k = 0; k < num_moneta2; k++)
        {
            if (c_save.crea_moneta[k].mesh != null)
            {
                DestroyImmediate(c_save.crea_moneta[k].mesh);

            }

        }


        int num_bonus2 = c_save.crea_bonus.Count;

        for (int k = 0; k < num_bonus2; k++)
        {
            if (c_save.crea_bonus[k].mesh != null)
            {
                DestroyImmediate(c_save.crea_bonus[k].mesh);

            }

        }

        int num_malus2 = c_save.crea_malus.Count;

        for (int k = 0; k < num_malus2; k++)
        {
            if (c_save.crea_malus[k].mesh != null)
            {
                DestroyImmediate(c_save.crea_malus[k].mesh);

            }

        }


    }



    void delete_project()
    {




        if (c_save != null)
        {

            distruggi_oggetti();


        }



        c_save.crea_blocco.Clear();
        c_save.crea_bonus.Clear();
        c_save.crea_malus.Clear();
        c_save.crea_gemma.Clear();
        c_save.crea_moneta.Clear();



    }


    void calcolo_snap(int aggiorna = 0)
    {

        Debug.Log("aggiorna " + aggiorna);

        if (aggiorna == 1)
        {
            for (int k = 0; k < snap_rif.Length; k++)
            {

                if (snap_rif[k] != null)
                {
                    DestroyImmediate(snap_rif[k]);

                }


            }
        }



        GameObject rif = GameObject.Find("cilindro_esatto/rif");

        float rad2 = Mathf.PI / 18.0f;

        float rad_cilindro = cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;

        Vector3[] pos_vn = new Vector3[3000];


        for (int k = 0; k <= 9; k++)
        {


            for (int n = 0; n < 39; n++)
            {

                float rad = rad2 * n - rad_cilindro;

                float altezza = 0;

                altezza = k * .25f + .75f;

                if (k == 0)
                {
                    altezza = 0;
                }



                float xx = Mathf.Sin(rad) * (c_save.crea_cilindro[0].raggio + altezza);
                float yy = Mathf.Cos(rad) * (c_save.crea_cilindro[0].raggio + altezza);


                Vector3 pos = new Vector3(xx, yy, 0);

                pos_vn[n + k * 50] = pos;

                if (aggiorna == 1)
                {



                    snap_rif[n + k * 50] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere_rif", typeof(GameObject))) as GameObject;

                    snap_rif[n + k * 50].name = "snap_rif " + n;

                    snap_rif[n + k * 50].transform.position = pos;

                }
            }

        }

        distanza_corda = Vector3.Distance(pos_vn[0], pos_vn[1]);

        crea_snap = 1;
    }



    void crea_blocco(int num)
    {


        float rad = c_save.crea_blocco[num].rad;

        int tipo_blocco = c_save.crea_blocco[num].tipo;



        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_blocco[num].pos;



        if (tipo_blocco <= 4)
        {
            c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_" + tipo_blocco, typeof(GameObject))) as GameObject;
        }
        else
        {
            crea_blocco_mesh2(num, tipo_blocco, c_save.crea_blocco[num].struttura_procedurale, c_save.crea_blocco[num].struttura_procedurale_dz);
        }

        c_save.crea_blocco[num].attivo = 1;

        c_save.crea_blocco[num].mesh.name = "blocco " + num;

        c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_blocco[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);




        if (tipo_blocco <= 8)
        {

            c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);
        }

        if (tipo_blocco >= 9)
        {
            c_save.crea_blocco[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

            c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(0, 0, zz);
        }


        if (tipo_blocco <= 4)
        {
            c_save.crea_blocco[num].mesh_dissolve = GameObject.Find("cilindro_esatto/blocco " + num + "/blocco_mesh");

            c_save.crea_blocco[num].mesh_dissolve.name = "blocco_mesh " + num;

            c_save.crea_blocco[num].mesh_renderer = c_save.crea_blocco[num].mesh_dissolve.GetComponent<Renderer>();

            c_save.crea_blocco[num].mesh_renderer.material.shader = Shader.Find("Custom/Dissolve");
        }

        if (tipo_blocco >= 5)
        {
            c_save.crea_blocco[num].mesh_dissolve = c_save.crea_blocco[num].mesh;

            c_save.crea_blocco[num].mesh_renderer = c_save.crea_blocco[num].mesh_dissolve.GetComponent<Renderer>();

            c_save.crea_blocco[num].mesh_renderer.material.shader = Shader.Find("Custom/Dissolve");


            rigenera_blocco(c_save.crea_blocco[num].mesh);
        }



    }




    void crea_gemma(int num)
    {


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


    void crea_moneta(int num)
    {


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



    void crea_malus(int num)
    {


        float rad = c_save.crea_malus[num].rad;

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_malus[num].pos;

        int tipo_malus = c_save.crea_malus[num].tipo;


        c_save.crea_malus[num].inversion_controller = 0;



        c_save.crea_malus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/malus_" + tipo_malus, typeof(GameObject))) as GameObject;

        c_save.crea_malus[num].mesh.name = "malus_" + num;

        c_save.crea_malus[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_malus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_malus[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


    }


    void crea_bonus(int num)
    {


        float rad = c_save.crea_bonus[num].rad;

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_bonus[num].pos;

        int tipo_bonus = c_save.crea_bonus[num].tipo;

        c_save.crea_bonus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/bonus_" + tipo_bonus, typeof(GameObject))) as GameObject;

        c_save.crea_bonus[num].mesh.name = "bonus_" + tipo_bonus;

        c_save.crea_bonus[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_bonus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_bonus[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


    }


    void aggiorna_oggetto_dissolto_singolo(int num, float dissolto)
    {

           //  Debug.Log(num+" num "+ dissolto);

          if (c_save.crea_blocco[num].mesh_renderer != null)
          {
        c_save.crea_blocco[num].mesh_renderer.material.SetFloat("_Amount", dissolto);


         }

    }



    void crea_blocco_mesh2(int num, int tipo, string struttura, string struttura_dz)
    {


        if (c_save.crea_blocco[num].mesh != null)
        {

            DestroyImmediate(c_save.crea_blocco[num].mesh);
        }

        c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_mesh", typeof(GameObject))) as GameObject;



        scala_blocco(c_save.crea_blocco[num].mesh, tipo, struttura, struttura_dz);

        c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);
    }


    void scala_blocco(GameObject ogg, int tipo, string struttura = "", string struttura_dz = "")
    {

        Vector3 scale = new Vector3(distanza_corda * 2, 1.0f, 2);

        if (tipo == 6)
        {
            scale = new Vector3(distanza_corda * 2, 2.0f, 2);
        }

        if (tipo == 7)
        {
            scale = new Vector3(distanza_corda * 2, 4.0f, 1);
        }

        if (tipo == 8)
        {
            scale = new Vector3(distanza_corda * 2, 4.0f, 4);
        }

        if (tipo <= 8)
        {
            ogg.transform.localScale = scale;

        }

        if (tipo >= 9)
        {

            ristruttura_blocco(ogg, tipo, struttura, struttura_dz);

        }


    }


    void ristruttura_blocco(GameObject ogg, int tipo, string struttura, string struttura_dz)
    {



        string testo_ogg = "696";

        string testo_ogg_dz = "22222222222222222";

        Vector3 dz = new Vector3(0, 0, 2);

        if (tipo == 9)
        {
            //-----------0123456789012345678
            testo_ogg = "0770077004500320077";

        }



        if (tipo == 10)
        {
            //-----------012345678901234567
            testo_ogg = "300500700300600888";

        }

        if (tipo == 11)
        {
            //-----------012345678901234567
            testo_ogg = "6987996";

        }


        if (tipo == 12)
        {
            //-----------012345678901234567
            testo_ogg = "8980096";
            dz = new Vector3(0, 0, 6);
        }


        if (tipo == 13)
        {
            //-----------012345678901234567
            testo_ogg = "811845611898113456";
            dz = new Vector3(0, 0, 6);
        }

        if (tipo == 14)
        {
            //-----------012345678901234567
            testo_ogg = "1234321";
            dz = new Vector3(0, 0, 6);
        }

        if (tipo == 15)
        {
            //-----------012345678901234567
            testo_ogg = "23956236847532532354235";
            dz = new Vector3(0, 0, 22);
        }




        if (tipo == 100)
        {
            testo_ogg = "" + struttura;
            testo_ogg_dz = "" + struttura_dz;

            //  Debug.Log("testo_ogg_dz "+ testo_ogg_dz);
        }



        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        int lung_dz = testo_ogg_dz.Length;
        if (lung_dz > 18)
        {
            lung_dz = 18;
        }


        int lung = testo_ogg.Length;
        if (lung > 18)
        {
            lung = 18;
        }

        float[] mesh_v = new float[37];

        float[] mesh_v_dz = new float[37];

        for (var n = 0; n < lung; n++)
        {
            string str = testo_ogg.Substring(n, 1);
            mesh_v[n] = int.Parse(str);
        }

        for (var n = 0; n < lung_dz; n++)
        {
            string str = testo_ogg_dz.Substring(n, 1);
            mesh_v_dz[n] = int.Parse(str);

        }


        int num_v = 0;

        for (var n = 0; n < 19; n++)
        {
            if (mesh_v[n] > 0)
            {
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




        for (var n = 0; n < 19; n++)
        {
            if (mesh_v[n] > 0)
            {


                int codice = (int)(mesh_v[n]);

                aum_v = aum_v + 1;
                int nv = aum_v * 24;

                int mo = n * 2;


                Vector3 snap_n = snap_rif[mo].transform.position;
                Vector3 snap_n1 = snap_rif[mo + 2].transform.position;

                Vector3 snap_up_n = snap_rif[mo + codice * 50].transform.position;
                Vector3 snap_up_n1 = snap_rif[mo + 2 + codice * 50].transform.position;

                if (mesh_v[n] >= 9)
                {
                    snap_n = snap_rif[mo + 4 * 50].transform.position;
                    snap_n1 = snap_rif[mo + 2 + 4 * 50].transform.position;

                    snap_up_n = snap_rif[mo + 6 * 50].transform.position;
                    snap_up_n1 = snap_rif[mo + 2 + 6 * 50].transform.position;

                }


                dz = dz_base;

                if (mesh_v_dz[n] > 0)
                {
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





                for (int k = 0; k < 6; k++)
                {
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


    void crea_sfera(Vector3 pos, string str = "")
    {
        GameObject sfera = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere", typeof(GameObject))) as GameObject;
        sfera.transform.position = pos;

        sfera.name = "" + str;
    }

    void gestione_collisione()
    {

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

        pos_ray_direction[0] = new Vector3(0,0,2.6f);
        pos_ray_direction[1] = new Vector3(-1.5f, 0, .25f);
        pos_ray_direction[2] = new Vector3(1.5f, 0, .25f);
        pos_ray_direction[3] = new Vector3(0, 0, 2.6f);
        pos_ray_direction[4] = new Vector3(0, 0, 2.6f);
        pos_ray_direction[5] = new Vector3(-1.5f, 0, .75f);
        pos_ray_direction[6] = new Vector3(1.5f, 0, .75f);

        distanza_direction[0] = 2.6f;
        distanza_direction[1] = 1.5f;
        distanza_direction[2] = 1.5f;

        distanza_direction[3] = 2.6f;
        distanza_direction[4] = 2.6f;

        distanza_direction[5] = 1.5f;
        distanza_direction[6] = 1.5f;

        int numero_coll = 0;
        Vector3 punto_coll= new Vector3(0,0,0);

        int num_block = -1;

        for (int n = 0; n <= 6; n++)
        {

            pos = astronave.transform.position + pos_direction[n];


            Debug.DrawRay(pos, pos_ray_direction[n], new Color(1, n*.2f, 0, 1));


            if (Physics.Raycast(pos, pos_ray_direction[n], out hit_collider, 15))
            {

                float dis = hit_collider.distance;


                if (dis < distanza_direction[n])
                {


                    if (hit_collider.collider.name.IndexOf("blocco") > -1)
                    {



                        string str_block = "" + hit_collider.collider.name;

                     //   Debug.Log("" + str_block);

                      


                        int indice = str_block.IndexOf("blocco_mesh");


                        if (indice == -1)
                        {
                            str_block = str_block.Replace("blocco ", "");

                            num_block = int.Parse(str_block);

                            numero_coll = numero_coll+1;

                            punto_coll = punto_coll + hit_collider.point;
                        }

                        if (indice > -1)
                        {

                            str_block = str_block.Replace("blocco_mesh ", "");

                            num_block = int.Parse(str_block);

                          
                            if (c_save.crea_blocco[num_block].disattiva_coll == 0)
                            {
                                c_save.crea_blocco[num_block].distruzione_oggetto = 10;

                                carica_particles("particles/CFXR Explosion 2", c_save.crea_blocco[num_block].mesh_dissolve.transform.position);

                                    c_save.crea_blocco[num_block].disattiva_coll = 1;
                                energia = energia - 30;

                            }
                        }



                    //    Debug.Log("toccato blocco " + num_block);



                      


                    }


                    if (hit_collider.collider.name.IndexOf("moneta") > -1)
                    {
                        if (ogg_struttura_dati != null)
                        {
                            script_struttura_dati.monete = script_struttura_dati.monete + 1;


                            Debug.Log(" script_struttura_dati.monete " + script_struttura_dati.monete);
                        }


                        Destroy(hit_collider.transform.gameObject);
                    }

                    if (hit_collider.collider.name.IndexOf("gemma") > -1)
                    {
                        Destroy(hit_collider.transform.gameObject);

                    }


                }

            }


            if (num_block > -1 && numero_coll>0)
            {
               

                Vector3 punto_coll_uso = punto_coll / numero_coll;



                

               // crea_sfera(punto_coll_uso);

               

             //   Debug.Break();

                if (c_save.crea_blocco[num_block].disattiva_coll == 0)
                {
                    c_save.crea_blocco[num_block].disattiva_coll = 1;
                    energia = energia - 10;

                    carica_particles("particles/CFXR Hit A", punto_coll_uso);

                    c_save.crea_blocco[num_block].punto_impatto = punto_coll_uso;
                    c_save.crea_blocco[num_block].forza_impatto = velocita_personaggio * 8;


                    c_save.crea_blocco[num_block].distruzione_oggetto = 1;

                }
            }



        }


    }


    void carica_particles(string path, Vector3 pos)
    {

        GameObject particles = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;


        particles.transform.position = pos;


    }




    void gestione_collisione_sparo(int num, GameObject ogg, int tipo = 0)
    {

        RaycastHit hit_collider;

        Vector3 pos_ray_direction = new Vector3(0, 0, 10);

        Vector3 pos;

        Vector3[] pos_direction = new Vector3[10];

        pos_direction[0] = new Vector3(0, 0, 0);



        pos = ogg.transform.position + pos_direction[0];

        float distanza_coll = 0;


        if (tipo == 0)
        {
            distanza_coll = Vector3.Distance(sparo[num].transform.position, astronave.transform.position);
        }
        if (tipo == 1)
        {
            distanza_coll = Vector3.Distance(sparo_boss[num].transform.position, astronave.transform.position);
        }


        int toccato_astronave = 0;

        if (distanza_coll < 2 && tipo==1)
        {
            DestroyImmediate(ogg);

            energia = energia - 50;


        }


        if (toccato_astronave == 0)
        {

            if (Physics.Raycast(pos, pos_ray_direction, out hit_collider, 15))
            {

                float dis = hit_collider.distance;


                if (dis < 3.6f)
                {


                    if (hit_collider.collider.name.IndexOf("blocco") > -1)
                    {




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

                            if (tipo == 0)
                            {
                                attivo_tempo_sparo[num] = 0;
                            }
                            else
                            {
                                attivo_tempo_sparo_boss[num] = 0;
                            }
                            DestroyImmediate(ogg);

                        }

                        if (indice > -1)
                        {

                            str_block = str_block.Replace("blocco_mesh ", "");

                            num_block = int.Parse(str_block);

                            carica_particles("particles/CFXR Hit A", hit_collider.point);


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
                        }





                    }




                }


            }

        }

    }

        


    void gestione_boss()
    {

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

        if (attivo_tempo_sparo_boss2 < -1)
        {

            if (Mathf.Abs(diff) < .1f)
            {
                int num_sparo_boss = -1;

                for (int n = 0; n < 5; n++)
                {

                    if (attivo_tempo_sparo_boss[n] < -1)
                    {
                        num_sparo_boss = n;

                        attivo_tempo_sparo_boss[n] = 15;


                        sparo_boss[n] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sparo_boss", typeof(GameObject))) as GameObject;

                        Vector3 pos_sparo_boss = boss_mesh_sparo.transform.position;

                        sparo_boss[n].transform.position = new Vector3(pos_sparo_boss.x, pos_sparo_boss.y, pos_sparo_boss.z);

                        sparo_boss_rad[n] = 0;

                        sparo_boss[n].name = "sparo_boss " + n;

                        attivo_tempo_sparo_boss2 = UnityEngine.Random.Range(6,10);

                        n = 1000;


                    }



                }


            }

        }

        gestione_sparo_boss();

    }


    void rigenera_blocco(GameObject ogg)
    {


        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;

        Vector3 pos = ogg.transform.position;

        Vector3[] vertices_studio = mesh.vertices;
        Vector3[] vertices = mesh.vertices;

        for (int n = 0; n < vertices.Length; n++)
        {
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






    void spostamento_blocco(GameObject ogg, Vector3 pi, float forza, float ogg_rad)
    {

    //    Debug.Log("blocco impatto ");

        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;


        Vector3[] vertices = mesh.vertices;

        Vector3 pos = ogg.transform.position;

     //   crea_sfera(pi,"coll");

        float rad = -ogg.transform.localEulerAngles.z*Mathf.Deg2Rad;

        for (int n = 0; n < vertices.Length; n++)
        {

            Vector3 pv = ogg.transform.TransformPoint(vertices[n]);



       //     crea_sfera(pv);

            float xx = pv.x - pi.x;
            float zz = pv.z - pi.z;

            float dis = Mathf.Sqrt(xx * xx + zz * zz);

        //    float rad = Mathf.Atan2(xx, zz);

            float molt = (6 - dis)/6.0f;

            if (molt < 0)
            {
                molt = 0;
            }

          //  float px = vertices[n].x - Mathf.Sin(rad) * forza*molt;
            float pz = pv.z+   forza*molt*Time.deltaTime-pos.z;



            vertices[n] = new Vector3(vertices[n].x, vertices[n].y, pz);


        }



        mesh.vertices = vertices;


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

   //    Debug.Break();

    }


    void crea_button_input_text(int num, string str = "")
    {

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


    void crea_button_text(int num, string txt, Color colore_testo, GameObject parent, string path = "Canvas", string path_sprite = "")
    {

        pulsante[num] = Instantiate(Resources.Load("UI/Button_text", typeof(GameObject))) as GameObject;

        pulsante[num].name = "pulsante_text" + num;

        pulsante[num].transform.SetParent(parent.transform);

        pulsante[num].GetComponent<Button>().onClick.AddListener(() => pressione_pulsante(num));

        ColorBlock cb = pulsante[num].GetComponent<Button>().colors;
        cb.selectedColor = newColor;
        pulsante[num].GetComponent<Button>().colors = cb;


        if (path_sprite != "")
        {
            pulsante[num].GetComponent<Image>().sprite = Resources.Load<Sprite>(path_sprite);
        }


        pulsante[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


        pulsante_testo[num] = GameObject.Find(path + "/pulsante_text" + num + "/Text_TMP");

        pulsante_testo[num].GetComponent<TextMeshProUGUI>().text = "" + txt;

        pulsante_testo[num].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_y / 20);


        pulsante_testo[num].GetComponent<TextMeshProUGUI>().color = colore_testo;

    }


    void crea_grafica_text(int num, Color colore, string txt, GameObject parent, string path = "Canvas", string path_sprite = "")
    {

        //    Debug.Log("entratooo");


        grafica[num] = Instantiate(Resources.Load("UI/Image_text", typeof(GameObject))) as GameObject;

        grafica[num].name = "grafica_text" + num;

        grafica[num].transform.SetParent(parent.transform);

        grafica[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        grafica[num].GetComponent<Image>().color = colore;

        if (path_sprite != "")
        {
            grafica[num].GetComponent<Image>().sprite = Resources.Load<Sprite>(path_sprite);
        }


        grafica[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);




        grafica_testo[num] = GameObject.Find(path + "/grafica_text" + num + "/Text");



        grafica_testo[num].GetComponent<TextMeshProUGUI>().text = "" + txt;

        grafica_testo[num].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x / 20);


    }


    void pressione_pulsante(int num)
    {

#if UNITY_EDITOR

        Debug.Log("pulsante " + num);

#endif

    }



        void pressione_input_text(int num, InputField tog)
    {


        if (tog.text != null || tog.text != "" && tog.text != "-" && tog.text != ".")
        {

            Debug.Log("pressione!" + tog.text + "!!");

        }


    }


    void crea_menu()
    {

        crea_button_text(0, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Icon_PictoIcon_Setting");

        crea_grafica_text(1, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/StatusBarIcon_Gem");
        crea_grafica_text(2, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");

        crea_grafica_text(3, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/StatusBarIcon_Gold");
        crea_grafica_text(4, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");




    }


    void aggiorna_menu()
    {
        font_size = (int)(risoluzione_x / 25);


        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;

     


      

        if (pulsante[0] != null)  //settings
        {
            float dx2 = dy * .8f / 2;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * -.5f + dx2;
            pos_y = risoluzione_y * 0.45f;
            pulsante[0].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            pulsante[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[3] != null)  //coin
        {

            float dx2 = dy * .8f / 2;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * .5f - dx2 * 2;
            pos_y = risoluzione_y * 0.45f;

            grafica[3].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[4] != null)  //coin  txt
        {

            float dx2 = dy * .8f;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * .5f - dx2;
            pos_y = risoluzione_y * 0.5f - dy2 * .9f;

            grafica[4].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[4].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[4].GetComponent<TextMeshProUGUI>().text = ""+ monete_partita_corrente;

        }

        if (grafica[1] != null)  //gem
        {

            float dx2 = dy * .8f / 2;
            float dy2 = dx2 * (84f / 70f);
            pos_x = 0;
            pos_y = risoluzione_y * 0.45f;

            grafica[1].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[2] != null)  //gem  txt
        {

            float dx2 = dy * .8f;
            float dy2 = dx2 * (76f / 72f);
            pos_x = 0;
            pos_y = risoluzione_y * 0.5f - dy2 * .9f;

            grafica[2].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[2].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[2].GetComponent<TextMeshProUGUI>().text = ""+gemme_prese;


        }

    }



    public void controllo_risoluzione()
    {

        risoluzione_x = Screen.width;
        risoluzione_y = Screen.height;



        float xx = risoluzione_x;

        rapporto_risoluzione = xx / risoluzione_y;

        spostamento_sx = (rapporto_risoluzione / 1.333333f);
        spostamento_sx2 = (rapporto_risoluzione - 1.333333f);


        if (rapporto_risoluzione < 1)
        {

            spostamento_sx = (rapporto_risoluzione / .75f);
            spostamento_sx2 = (rapporto_risoluzione - .75f);

        }


        xm_old = xm;
        ym_old = ym;


        xm = Input.mousePosition.x;
        ym = Input.mousePosition.y;


        diff_xm = xm - xm_old;
        diff_ym = ym - ym_old;


    }


}



