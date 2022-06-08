using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

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
    public float velocita_cilindro = 1;

    public float velocita_bonus_base = 2;
    public float velocita_bonus = 1;

    public int inversione_camera = 1;

    public GameObject astronave;

    public float umo_x;
    public float umo_y;
    public float umo_z;

    public bool groundedPlayer;

    public float jump = 15;

    float diff_xm;
    float diff_ym;

    float risoluzione_x;
    float risoluzione_y;

    float rapporto_risoluzione;
    float spostamento_sx;
    float spostamento_sx2;

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

    float astronave_rx=0;
    float astronave_rx_calcolo = 0;

    float astronave_rz = 0;
    float astronave_rz_calcolo = 0;

    GameObject[] snap_rif = new GameObject[3000];

    int crea_snap = 0;

    float distanza_corda = 0;

    float aumento_salto = 1;



    // Start is called before the first frame update
    void Start()
    {

        characterController = GetComponent<CharacterController>();
        cilindro = GameObject.Find("cilindro_esatto");

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

        ogg_struttura_dati = GameObject.Find("base_struttura");

        if (ogg_struttura_dati != null)
        {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }


        cam_pos = new Vector3(0,1.35f,13.0f);
        cam_rot = new Vector3(23, 180, 0);

    }

    // Update is called once per frame
    void Update()
    {

        controllo();

        gestione_camera();

        aggiorna_blocco();

        gestione_cilindro();


     //   gestione_collisione();

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



    void controllo()
    {

       float pressione_tasto_up = Input.GetAxis("Vertical");


      

        if (pressione_tasto_up > 0 && playerVelocity.y <= 0)
        {
            aumento_salto = .6f;
           

        }


        aumento_salto = aumento_salto - Time.deltaTime;
        if (aumento_salto > 0)
        {
            playerVelocity.y += 30 * Time.deltaTime;

        }


        playerVelocity.y += -20.0f * Time.deltaTime;

        if (playerVelocity.y > 3)
        {
            playerVelocity.y = 3;
        }

        if (playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        astronave.transform.position = new Vector3(0, c_save.crea_cilindro[0].raggio+ playerVelocity.y+1, 0);



        pressione_tasto = Input.GetAxis("Horizontal");

        if (Mathf.Abs(pressione_tasto) > 0 && blocco_velocita==1)
        {

            int molt_inversione = 1;

            if (inversione_controllo == 1)
            {
                molt_inversione = -1;

            }


            rotazione_cilindro = pressione_tasto * potenza_tasto * Time.deltaTime* molt_inversione;


            astronave_rz_calcolo = astronave_rz_calcolo + pressione_tasto;

            if (astronave_rz_calcolo > 30)
            {
                astronave_rz_calcolo = 30;

            }

            if (astronave_rz_calcolo < -30)
            {
                astronave_rz_calcolo = -30;

            }

            cilindro.transform.Rotate(new Vector3(0, 0, rotazione_cilindro));

        }


        astronave_rx = playerVelocity.y * -2;

        astronave_rz = Mathf.LerpAngle(astronave_rz, astronave_rz_calcolo, Time.deltaTime*15);


        astronave.transform.localEulerAngles = new Vector3(astronave_rx,0,astronave_rz);

        astronave_rz_calcolo = astronave_rz_calcolo * .9f;

    }


    void gestione_camera()
    {








        if (inversione_camera == 0)
        {
            cam_pos.z = Mathf.Lerp(cam_pos.z, -13, Time.deltaTime * 5);

            cam_rot.y = Mathf.Lerp(cam_rot.y, 0, Time.deltaTime * 50);

        }

        if (inversione_camera == 1)
        {
            cam_pos.z = Mathf.Lerp(cam_pos.z, 13, Time.deltaTime * 5);

            cam_rot.y = Mathf.Lerp(cam_rot.y, 180, Time.deltaTime * 50);

        }


        cam0.transform.localPosition = new Vector3(0,c_save.crea_cilindro[0].raggio+3.81f, cam_pos.z);
        cam0.transform.localEulerAngles = cam_rot;

    }





    void crea_livello()
    {
        /*
        aum_blocco = -1;

     

        for (int n = 0; n < 60; n++)
        {


            for (int k = 0; k < 6; k++)
            {
                if (UnityEngine.Random.Range(0, 10.99f) > 3){

                    int tipo = (int)(UnityEngine.Random.Range(0, 3.99f));

                    int blocco_arrivo_successivo =  (int)(UnityEngine.Random.Range(0, 1.99f));

                //    crea_blocco_singolo(tipo, -300 + 15 * n+k, UnityEngine.Random.Range(0, 360), blocco_arrivo_successivo);

                }
            }

            for (int k = 0; k < 6; k++)
            {
                crea_bonus_gemma(5 * n+k, UnityEngine.Random.Range(0, 360));

            }

           
                crea_bonus_speed(15 * n , UnityEngine.Random.Range(0, 360));

            crea_malus_inversion(15 * n+7, UnityEngine.Random.Range(0, 360));



        }
        */

    }



    void gestione_cilindro()
    {

        velocita_bonus = Mathf.Lerp(velocita_bonus, 1, Time.deltaTime*.333f);


        cilindro.transform.Translate(new Vector3(0,0,-velocita_cilindro*Time.deltaTime* blocco_velocita*velocita_bonus));


    }




    void crea_bonus_gemma(float pos_z, float angolo)
    {


            GameObject bonus_gemma = Instantiate(Resources.Load("grafica_3d/Prefabs_space/gemma")) as GameObject;

            bonus_gemma.name = "bonus_gemma";

        bonus_gemma.transform.SetParent(cilindro.transform);

        float rad = angolo*Mathf.Deg2Rad;

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


                if (Mathf.Abs(zz2) < velocita_cilindro * 5 && c_save.crea_blocco[n].arrivo == 0 && zz2<-15)
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



        if (other.name.IndexOf ("moneta")>-1)
        {
            if (ogg_struttura_dati != null)
            {
                script_struttura_dati.monete = script_struttura_dati.monete + 1;
           

            Debug.Log(" script_struttura_dati.monete " + script_struttura_dati.monete);
            }


            Destroy(other.gameObject);
        }

        if (other.name.IndexOf("gemma") > -1)
        {
            Destroy(other.gameObject);

        }


        if (other.name.IndexOf("Cube")>-1)
        {

            blocco_velocita = 0;

        }

        if (other.name.IndexOf("blocco") > -1)
        {

            blocco_velocita = 0;

        }


        if (other.name.IndexOf("bonus_0") > -1)
        {

            Debug.Log("entra speed ");

            velocita_bonus = velocita_bonus_base;

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

                calcolo_snap();

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
            crea_blocco_mesh2(num, tipo_blocco);
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

            c_save.crea_blocco[num].mesh_renderer = c_save.crea_blocco[num].mesh_dissolve.GetComponent<Renderer>();

            c_save.crea_blocco[num].mesh_renderer.material.shader = Shader.Find("Custom/Dissolve");
        }

        if (tipo_blocco >= 5)
        {
            c_save.crea_blocco[num].mesh_dissolve = c_save.crea_blocco[num].mesh;

            c_save.crea_blocco[num].mesh_renderer = c_save.crea_blocco[num].mesh_dissolve.GetComponent<Renderer>();

            c_save.crea_blocco[num].mesh_renderer.material.shader = Shader.Find("Custom/Dissolve");
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

        c_save.crea_gemma[num].mesh.name = "gemma" ;

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

        Debug.Log(num+" num "+ dissolto);

      //  if (c_save.crea_blocco[num].mesh_renderer != null)
      //  {
            c_save.crea_blocco[num].mesh_renderer.material.SetFloat("_Amount", dissolto);
      //  }

    }


    void crea_blocco_mesh2(int num, int tipo)
    {


        if (c_save.crea_blocco[num].mesh != null)
        {

            DestroyImmediate(c_save.crea_blocco[num].mesh);
        }

        c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_mesh", typeof(GameObject))) as GameObject;



        scala_blocco(c_save.crea_blocco[num].mesh, tipo);

        c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);
    }


    void scala_blocco(GameObject ogg, int tipo)
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

            ristruttura_blocco(ogg, tipo);

        }


    }


    void ristruttura_blocco(GameObject ogg, int tipo)
    {



        string testo_ogg = "696";

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



        Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;

        mesh.Clear();


        int lung = testo_ogg.Length;
        if (lung > 18)
        {
            lung = 18;
        }

        float[] mesh_v = new float[37];

        for (var n = 0; n < lung; n++)
        {
            string str = testo_ogg.Substring(n, 1);


            mesh_v[n] = int.Parse(str);



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



                vertices[nv + 0] = snap_n;
                vertices[nv + 1] = snap_up_n;
                vertices[nv + 2] = snap_up_n1;
                vertices[nv + 3] = snap_n1;

                vertices[nv + 4] = snap_n + dz;
                vertices[nv + 5] = snap_up_n + dz;
                vertices[nv + 6] = snap_up_n1 + dz;
                vertices[nv + 7] = snap_n1 + dz;

                vertices[nv + 8] = snap_n;
                vertices[nv + 9] = snap_up_n;
                vertices[nv + 10] = snap_up_n + dz;
                vertices[nv + 11] = snap_n + dz;

                vertices[nv + 12] = snap_n1;
                vertices[nv + 13] = snap_up_n1;
                vertices[nv + 14] = snap_up_n1 + dz;
                vertices[nv + 15] = snap_n1 + dz;


                vertices[nv + 16] = snap_up_n;
                vertices[nv + 17] = snap_up_n + dz;
                vertices[nv + 18] = snap_up_n1 + dz;
                vertices[nv + 19] = snap_up_n1;

                vertices[nv + 20] = snap_n;
                vertices[nv + 21] = snap_n + dz;
                vertices[nv + 22] = snap_n1 + dz;
                vertices[nv + 23] = snap_n1;


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


        DestroyImmediate(ogg.GetComponent<MeshCollider>());


        ogg.AddComponent<MeshCollider>();

        ogg.transform.localScale = new Vector3(1, 1, 1);


        //   Debug.Break();

    }

    void calcolo_snap()
    {

        
        float rad2 = Mathf.PI / 18.0f;

        float rad_cilindro = cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;

        for (int k = 0; k <= 9; k++)
        {


            for (int n = 0; n < 39; n++)
            {

                float rad = rad2 * n - rad_cilindro;

                float altezza = 0;

                altezza = k + .5f;

                if (k == 0)
                {
                    altezza = 0;
                }



                float xx = Mathf.Sin(rad) * (c_save.crea_cilindro[0].raggio + altezza);
                float yy = Mathf.Cos(rad) * (c_save.crea_cilindro[0].raggio + altezza);


                Vector3 pos = new Vector3(xx, yy, 0);


                if (crea_snap == 0)
                {

                    snap_rif[n + k * 50].transform.position = pos;

                   
                }
            }

        }

        distanza_corda = Vector3.Distance(snap_rif[0].transform.position, snap_rif[1].transform.position);

        crea_snap = 1;
    }


    void gestione_collisione()
    {

        RaycastHit hit_collider;

        Vector3 pos_ray_direction = new Vector3(0, 0, 10);

        Vector3 pos ;

        Vector3[] pos_direction = new Vector3[10];

        pos_direction[0] = new Vector3(0, 0, 0);
        pos_direction[1] = new Vector3(0, .6f, 0);
        pos_direction[2] = new Vector3(0, -.5f, 0);
        pos_direction[3] = new Vector3(-.8f, 0, 0);
        pos_direction[4] = new Vector3(.8f, 0, 0);

        for (int n = 0; n <= 4; n++)
        {

            pos = astronave.transform.position+ pos_direction[n];


            Debug.DrawRay(pos, pos_ray_direction, new Color(1, 0, 0, 1));


            if (Physics.Raycast(pos, pos_ray_direction, out hit_collider, 15))
            {

                float dis = hit_collider.distance;


                if (dis < 2.6f)
                {


                    if (hit_collider.collider.name.IndexOf("blocco") > -1)
                    {

                        blocco_velocita = 0;

                    }

                }

            }

        }


    }



}
