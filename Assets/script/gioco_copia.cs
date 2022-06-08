using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class gioco_copia : MonoBehaviour
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

    public float umo_x;
    public float umo_y;
    public float umo_z;

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



    // Start is called before the first frame update
    void Start()
    {

        characterController = GetComponent<CharacterController>();
        cilindro = GameObject.Find("cilindro");

        cam0 = GameObject.Find("Main Camera");

        leggi_vertici_cilindro();

        // crea_livello();

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

        pressione_tasto = Input.GetAxis("Horizontal");

        if (Mathf.Abs(pressione_tasto) > 0 && blocco_velocita==1)
        {

            int molt_inversione = 1;

            if (inversione_controllo == 1)
            {
                molt_inversione = -1;

            }


            rotazione_cilindro = pressione_tasto * potenza_tasto * Time.deltaTime* molt_inversione;

            cilindro.transform.Rotate(new Vector3(0, 0, rotazione_cilindro));

        }


       

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


            GameObject bonus_gemma = Instantiate(Resources.Load("grafica_3d/Prefabs/gemma")) as GameObject;

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


        GameObject bonus_speed = Instantiate(Resources.Load("grafica_3d/Prefabs/speed")) as GameObject;

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

            if (c_save.crea_blocco[n].attivo == 0)
            {

                float zz2 = c_save.crea_blocco[n].mesh.transform.position.z;




              

                if (Mathf.Abs(zz2) < velocita_cilindro*5 && c_save.crea_blocco[n].arrivo==0)
                {
                    c_save.crea_blocco[n].altezza = Mathf.Lerp(c_save.crea_blocco[n].altezza, c_save.crea_cilindro[0].raggio, Time.deltaTime*2.5f);

                    if (c_save.crea_blocco[n].altezza < .15f)
                    {
                        c_save.crea_blocco[n].altezza = 0.15f;
                        c_save.crea_blocco[n].altezza = 1;
                    }

                }



                if (Mathf.Abs(zz2) < velocita_cilindro * 5 && c_save.crea_blocco[n].arrivo == 1 && zz2<0)
                {
                    c_save.crea_blocco[n].altezza = Mathf.Lerp(c_save.crea_blocco[n].altezza, c_save.crea_cilindro[0].raggio, Time.deltaTime * 15.0f);

                    if (c_save.crea_blocco[n].altezza < .5f)
                    {
                        c_save.crea_blocco[n].altezza = 0.15f;
                        c_save.crea_blocco[n].altezza = 1;
                    }

                }



                gestione_pos(n);



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



    void crea_blocco_singolo(int tipo, float pos_z, float angolo, int arrivo)
    {
        /*
        aum_blocco = aum_blocco + 1;

        blocco[aum_blocco] = Instantiate(Resources.Load("grafica_3d/Prefabs/blocco_" + tipo)) as GameObject;

        blocco[aum_blocco].name = "blocco" + aum_blocco + " tipo " + tipo;

        blocco_rad[aum_blocco] = angolo*Mathf.Deg2Rad;

        blocco_arrivo[aum_blocco] = arrivo;

        blocco_altezza[aum_blocco] = 20+(UnityEngine.Random.Range(12.0f,20.0f))*arrivo;

        blocco_pos[aum_blocco]=pos_z;

        blocco[aum_blocco].transform.SetParent(cilindro.transform);

        blocco[aum_blocco].transform.localEulerAngles = new Vector3(0,0,angolo-270);

        gestione_pos(aum_blocco);
        */
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

                scala_cilindro();

                characterController.transform.position =new Vector3(0, c_save.crea_cilindro[0].raggio + .962f,0);

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

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_blocco[num].pos;

        int tipo_blocco = c_save.crea_blocco[num].tipo;

        c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs/blocco_" + tipo_blocco, typeof(GameObject))) as GameObject;

        c_save.crea_blocco[num].mesh.name = "blocco " + tipo_blocco;

        c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_blocco[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


    }


    void crea_gemma(int num)
    {


        float rad = c_save.crea_gemma[num].rad;

        float angolo = rad * Mathf.Rad2Deg;

        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

        float zz = c_save.crea_gemma[num].pos;



        c_save.crea_gemma[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs/gemma_0", typeof(GameObject))) as GameObject;

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



        c_save.crea_moneta[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs/moneta_0", typeof(GameObject))) as GameObject;

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



        c_save.crea_malus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs/malus_" + tipo_malus, typeof(GameObject))) as GameObject;

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

        c_save.crea_bonus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs/bonus_" + tipo_bonus, typeof(GameObject))) as GameObject;

        c_save.crea_bonus[num].mesh.name = "bonus_" + tipo_bonus;

        c_save.crea_bonus[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_bonus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_bonus[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


    }


}
