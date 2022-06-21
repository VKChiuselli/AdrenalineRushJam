using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using System.Globalization;

public class crea_livello : MonoBehaviour
{


#if UNITY_EDITOR
    [SerializeField]
#endif
    public classe_save c_save;

    public TextAsset level_json;

    public int max_blocchi = 5;

    float xm, ym, xm_old, ym_old;

    public int max_linee_blocchi = 30;

    public bool buchi_doppi = true;

    public bool solo_mine_e_ostacoli = true;

    public float distanza_partenza_blocchi = 20;
    public float distanza_blocchi = 26;
    public float distanza_random = 3;
    public int numero_aperture_minimo = 8;
    public int numero_aperture_massimo = 12;
    public int fila_monete_cirolari = 6;
    public int numero_monete_linea = 8;
    public int numero_mine_ostacoli = 5;

    public float percentuale_monete= 60;
    public float percentuale_altri_blocchi = 79;

    public float percentuale_blocchi_rotanti = 60;
    

    public float spostamento_z = 1;
    public float spostamento_z2 = .75f;


    float[] touch_x = new float[15];
    float[] touch_y = new float[15];

    float[] touch_rx = new float[15];
    float[] touch_ry = new float[15];

    float[] touch_xo = new float[15];
    float[] touch_yo = new float[15];


    float diff_xm;
    float diff_ym;

    float risoluzione_x;
    float risoluzione_y;

    float rapporto_risoluzione;
    float spostamento_sx;
    float spostamento_sx2;

    GameObject[] pulsante = new GameObject[100];
    GameObject[] pulsante_testo = new GameObject[100];


    public float move_camera_editor_speed = .2f;

    public float speed_rotation_camera = .3f;

    float[] pos_camera = new float[30];

    GameObject canvas;
    GameObject cilindro;
    GameObject partenza;
    GameObject finale;

    int tipo_blocco = 0;
    int tipo_moneta = 0;
    int tipo_gemma = 0;
    int tipo_bonus = 0;
    int tipo_malus = 0;


    public int attivo_funzione = 0;

    string[] lista_nome_blocco = new string[200];
    string[] lista_nome_bonus = new string[200];
    string[] lista_nome_malus = new string[200];

    GameObject blocco_base;
    GameObject gemma_base;
    GameObject moneta_base;
    GameObject bonus_base;
    GameObject malus_base;

    string name_collider = "";

    Vector3[] vertici_cilindro = new Vector3[12000];

    Vector3[] snap = new Vector3[60];
    Vector3[] snap_up = new Vector3[60];
    Vector3[] snap_up2 = new Vector3[60];
    Vector3[] snap_up3 = new Vector3[60];

    float distanza_corda = 0;

    GameObject[] snap_rif = new GameObject[3000];

    GameObject[] snap_rif2 = new GameObject[3000];

    int crea_snap = 0;

    int difficolta_livello_generato = 0;

    string[] lista_difficolta = new string[20];

    string struttura_generata_dz = "";

    GameObject rif2;


    // Start is called before the first frame update
    void Start()
    {

        rif2 = GameObject.Find("Rif2");

        cilindro = GameObject.Find("cilindro_esatto");
        partenza = GameObject.Find("partenza");
        finale = GameObject.Find("finale");

        leggi_vertici_cilindro();

        canvas = GameObject.Find("Canvas");

        pos_camera[11] = 30;
        pos_camera[20] = 90;

        crea_menu();

        blocco_base = carica_oggetto(blocco_base, "blocco_", tipo_blocco);
        gemma_base = carica_oggetto(gemma_base, "gemma_", tipo_gemma);
        bonus_base = carica_oggetto(bonus_base, "bonus_", tipo_bonus);
        moneta_base = carica_oggetto(moneta_base, "moneta_", tipo_moneta);
        malus_base = carica_oggetto(malus_base, "malus_", tipo_malus);

       

       

        load_project();

       

    }

    private GameObject carica_oggetto(GameObject oggetto_base, string nome_oggetto, int tipo_oggetto) {
        if (oggetto_base != null) {

            DestroyImmediate(oggetto_base);
        }

        oggetto_base = Instantiate(Resources.Load($"grafica_3d/Prefabs_space/{nome_oggetto}{tipo_oggetto}", typeof(GameObject))) as GameObject;

        oggetto_base.transform.SetParent(cilindro.transform);

        oggetto_base.name = nome_oggetto;

        return oggetto_base;

    }


    void crea_menu()
    {

        for (int n = 0; n < 30; n++)
        {

            if (pulsante[n] != null)
            {

                DestroyImmediate(pulsante[n]);

            }

        }

        crea_button_text(0, "+ blocco");

        crea_button_text(1, "tipo blocco");

        crea_button_text(2, "+ gemma");

        crea_button_text(3, "+ moneta");

        crea_button_text(4, "tipo malus");

        crea_button_text(5, "+ malus");

        crea_button_text(6, "tipo bonus");

        crea_button_text(7, "+ bonus");

        crea_button_text(8, "raggio_cilindro");

        crea_button_text(15, "crea_monete");
     
        crea_button_text(17, "genera livello");

        crea_button_text(18, "delete level");
        crea_button_text(19, "save");
        crea_button_text(20, "load");

    }






    void pressione_pulsante(int num)
    {
        if (num ==0)
        {
            attivo_funzione = 1;
        }


        if (num == 1)
        {

            attivo_funzione = 1;

            tipo_blocco = tipo_blocco + 1;

            if (tipo_blocco > max_blocchi)
            {
                tipo_blocco=0;
            }


            if (tipo_blocco <= 4)
            {
                blocco_base = carica_oggetto(blocco_base, "blocco_", tipo_blocco);
            }
            else
            {
                crea_blocco_mesh(tipo_blocco);
            }

        }

        if (num == 2)
        {
            attivo_funzione = 2;
        }

        if (num == 3)
        {
            attivo_funzione = 3;
        }



        if (num == 5)
        {
            attivo_funzione = 5;
        }


        if (num == 6)
        {

            attivo_funzione = 7;

            tipo_bonus = tipo_bonus + 1;

            if (tipo_bonus > 2)
            {
                tipo_bonus = 0;
            }


            bonus_base = carica_oggetto(bonus_base, "bonus_", tipo_bonus);

        }


            if (num == 7)
        {
            attivo_funzione = 7;
        }



        if (num == 8)
        {
            c_save.crea_cilindro[0].raggio = c_save.crea_cilindro[0].raggio + 2;

            if (c_save.crea_cilindro[0].raggio > 26)
            {
                c_save.crea_cilindro[0].raggio = 6;
            }

            scala_cilindro();

            partenza.transform.position = new Vector3(0, c_save.crea_cilindro[0].raggio + .51f, 0);

        }

        if (num == 15)
        {
            crea_genera_monete();
        }


        if (num == 16)
        {
            difficolta_livello_generato = difficolta_livello_generato + 1;

            if (difficolta_livello_generato > 5)
            {
                difficolta_livello_generato = 0;
            }
                
        }


        if (num == 17)
        {
            genera_livello();
        }


        if (num == 18)
        {
            delete_project();
        }
       

        if (num == 19)
        {
            save_project();
        }


        if (num == 20)
        {
            delete_project();
            load_project();
        }

    }

    void aggiorna_menu()
    {

        for (int n = 0; n < lista_nome_blocco.Length; n++)
        {
            lista_nome_blocco[n] = "blocco_"+n;

        }



      




        lista_nome_bonus[0] = "speed";
        lista_nome_bonus[1] = "missile";
        lista_nome_bonus[2] = "barriera";

        lista_nome_malus[0] = "inversion";

        float dy = risoluzione_y * .05f;
        float dx = risoluzione_x * .2f;

        float pos_x = risoluzione_x * .5f - dx * .5f; 
        float pos_y = risoluzione_y * .5f;



        int aum_y = -1;

        for (int n = 0; n < 30; n++)
        {
            if (pulsante[n] != null)
            {
                aum_y = aum_y + 1;

                pos_y = risoluzione_y * .5f - dy * .5f- aum_y * dy;

                pulsante[n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx, dy);
                pulsante[n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }

        }


        pulsante_testo[1].GetComponent<Text>().text = ""+lista_nome_blocco[tipo_blocco];

        pulsante_testo[4].GetComponent<Text>().text = "" + lista_nome_malus[tipo_malus];

        pulsante_testo[6].GetComponent<Text>().text = "" + lista_nome_bonus[tipo_bonus];

        pulsante_testo[8].GetComponent<Text>().text = "raggio" +c_save.crea_cilindro[0].raggio ;



        lista_difficolta[0] = "molto facile";
        lista_difficolta[1] = "facile";
        lista_difficolta[2] = "medio";
        lista_difficolta[3] = "difficile";
        lista_difficolta[4] = "molto difficile";
        lista_difficolta[5] = "impossibile";





     //   pulsante_testo[16].GetComponent<Text>().text = "difficoltà " +lista_difficolta[ difficolta_livello_generato];


    }




    void crea_button_text(int num, string txt)
    {

        pulsante[num] = Instantiate(Resources.Load("UI/Button_text_editor", typeof(GameObject))) as GameObject;

        pulsante[num].name = "pulsante_text" + num;

        pulsante[num].transform.SetParent(canvas.transform);

        pulsante[num].GetComponent<Button>().onClick.AddListener(() => pressione_pulsante(num));

        //pulsante [num].GetComponent<Button> ().OnPointerEnter (delegate(){pressione_pulsante(num);});

        pulsante[num].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


        pulsante_testo[num] = GameObject.Find("Canvas/pulsante_text" + num + "/Text");

        pulsante_testo[num].GetComponent<Text>().text = "" + txt;

        pulsante_testo[num].GetComponent<Text>().fontSize = (int)(30 / risoluzione_x);


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



    void controllo_camera()
    {



     //   pos_camera[11] = pos_camera[11] + Input.GetAxis("Mouse ScrollWheel") * 5;






        if (Input.GetMouseButton(1)  )
        {


            float xx = (xm - xm_old) * speed_rotation_camera;
            float zz = (ym - ym_old) * speed_rotation_camera;

            //	Debug.Log ("quaaaaa");

            pos_camera[20] = pos_camera[20] + zz;
            pos_camera[21] = pos_camera[21] + xx;


            if (pos_camera[20] > 90)
            {
                pos_camera[20] = 90;
            }

            if (pos_camera[20] < -90)
            {
                pos_camera[20] = -90;
            }



        }



        float radx = transform.localEulerAngles.x * Mathf.Deg2Rad;
        float rad = transform.localEulerAngles.y * Mathf.Deg2Rad + 1.57f;


        if (Input.GetMouseButton(0))
        {

            rad = transform.localEulerAngles.y * Mathf.Deg2Rad;


        }
        else
        {



            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {

                pos_camera[10] = pos_camera[10] - Mathf.Sin(rad) * move_camera_editor_speed;
                pos_camera[12] = pos_camera[12] - Mathf.Cos(rad) * move_camera_editor_speed;

            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {

                pos_camera[10] = pos_camera[10] + Mathf.Sin(rad) * move_camera_editor_speed;
                pos_camera[12] = pos_camera[12] + Mathf.Cos(rad) * move_camera_editor_speed;
            }

            rad = transform.localEulerAngles.y * Mathf.Deg2Rad;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {

                pos_camera[10] = pos_camera[10] + Mathf.Sin(rad) * move_camera_editor_speed;
                pos_camera[12] = pos_camera[12] + Mathf.Cos(rad) * move_camera_editor_speed;

                if (Input.GetMouseButton(1))
                {
                    pos_camera[11] = pos_camera[11] - Mathf.Sin(radx) * move_camera_editor_speed;
                }

            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {

                pos_camera[10] = pos_camera[10] - Mathf.Sin(rad) * move_camera_editor_speed;
                pos_camera[12] = pos_camera[12] - Mathf.Cos(rad) * move_camera_editor_speed;

                if (Input.GetMouseButton(1) )
                {
                    pos_camera[11] = pos_camera[11] + Mathf.Sin(radx) * move_camera_editor_speed;
                }

            }


        }


        if (pos_camera[11] < 1)
        {

            pos_camera[11] = 1;
        }


        if (pos_camera[20] < -25)
        {

            pos_camera[20] = -25;
        }


        if (pos_camera[20] > 90)
        {

            pos_camera[20] = 90;
        }


        transform.position = new Vector3(pos_camera[10], pos_camera[11], pos_camera[12]);


        transform.localEulerAngles = new Vector3(pos_camera[20], pos_camera[21], 0);




    }



    // Update is called once per frame
    void Update()
    {
        controllo_risoluzione();

        controllo_camera();

        aggiorna_menu();

        aggiorna_click_3d();

        gestione_cilindro();


        update_editor();

    }


    void gestione_cilindro()
    {

        int pressione_tasto = 0;


        if (Input.GetKey(KeyCode.Alpha1))
        {
            pressione_tasto = -1;

        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            pressione_tasto = 1;

        }

        if (Mathf.Abs(pressione_tasto) != 0)
        {

            float rotazione_cilindro = pressione_tasto * -150 * Time.deltaTime;

            cilindro.transform.Rotate(new Vector3(0, 0, rotazione_cilindro));

        }

     

    }



    void update_editor()
    {
#if UNITY_EDITOR

        if (Input.GetKeyUp(KeyCode.M))
        {
            crea_menu();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Break();
        }


        float pulsante_rotellina = Input.GetAxis("Mouse ScrollWheel")*15;

        


        if (pulsante_rotellina != 0)
        {
            if (pulsante_rotellina > 0)
            {
                tipo_blocco = tipo_blocco + 1;
            }

            if (pulsante_rotellina < 0)
            {
                tipo_blocco = tipo_blocco - 1;
            }

            if (tipo_blocco < 0)
            {
                tipo_blocco = max_blocchi;
            }

            if (tipo_blocco > max_blocchi)
            {
                tipo_blocco = 0;
            }

            if (tipo_blocco <= 4)
            {
                blocco_base = carica_oggetto(blocco_base, "blocco_", tipo_blocco);
            }
            else
            {
                crea_blocco_mesh(tipo_blocco);
            }

        }



        /*
        if (tipo_blocco < 0)
        {
            tipo_blocco = max_blocchi;
        }



        if (Input.GetKeyUp(KeyCode.Alpha3))
        {

            attivo_funzione = 1;

            tipo_blocco = tipo_blocco - 1;

            if (tipo_blocco < 0)
            {
                tipo_blocco = max_blocchi;
            }


            blocco_base = carica_oggetto(blocco_base, "blocco_", tipo_blocco);

        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {

            attivo_funzione = 1;

            tipo_blocco = tipo_blocco + 1;

            if (tipo_blocco > max_blocchi)
            {
                tipo_blocco = 0;
            }


            blocco_base = carica_oggetto(blocco_base, "blocco_", tipo_blocco);

        }
        */




#endif
    }


    void calcolo_snap(int aggiorna=0)
    {

        Debug.Log("aggiorna "+ aggiorna);

        if (aggiorna == 1)
        {
            for (int k = 0; k < snap_rif.Length; k++)
            {

                if (snap_rif[k] != null)
                {
                    DestroyImmediate(snap_rif[k]);

                }

                if (snap_rif2[k] != null)
                {
                    DestroyImmediate(snap_rif2[k]);

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

                altezza = k*.25f + .75f;

                if (k == 0)
                {
                    altezza = 0;
                }



                float xx = Mathf.Sin(rad) * (c_save.crea_cilindro[0].raggio+ altezza);
                float yy = Mathf.Cos(rad) * (c_save.crea_cilindro[0].raggio+ altezza);


                Vector3 pos = new Vector3(xx, yy, 0);

                pos_vn[n + k * 50] = pos;

                if (aggiorna==1)
                {

                    

                            snap_rif[n + k * 50] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere_rif", typeof(GameObject))) as GameObject;

                            snap_rif[n + k * 50].name = "snap_rif " + n;

                            snap_rif2[n + k * 50] = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere_rif", typeof(GameObject))) as GameObject;

                            snap_rif2[n + k * 50].name = "snap_rif2 " + n;




                    snap_rif[n+k*50].transform.position = pos;

                    snap_rif2[n + k * 50].transform.position = pos;

                   //      snap_rif[n + k * 50].transform.SetParent(rif.transform);
                    snap_rif2[n + k * 50].transform.SetParent(rif.transform);

                }
            }

        }

        distanza_corda = Vector3.Distance(pos_vn[0], pos_vn[1]);

        crea_snap = 1;
    }





    void aggiorna_click_3d()
    {


        if (blocco_base != null)
        {
            blocco_base.SetActive(false);
        }

        if (moneta_base != null)
        {
            moneta_base.SetActive(false);
        }

        if (gemma_base != null)
        {
            gemma_base.SetActive(false);
        }

        if (bonus_base != null)
        {
            bonus_base.SetActive(false);
        }

        if (malus_base != null)
        {
            malus_base.SetActive(false);
        }



        name_collider = "";

        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;

        if (Physics.Raycast(ray2, out hit2, 500))
        {

            Vector3 m_ray = hit2.point;

            name_collider = hit2.collider.name;


          //  Debug.Log("name_collider "+ name_collider);

            if (attivo_funzione == 1)
            {
                if (blocco_base != null)
                {
                    blocco_base.SetActive(true);


                    float rad=posizione_base_blocco(blocco_base, m_ray,tipo_blocco);


                    if (Input.GetMouseButtonUp(0))
                    {
                        c_save.crea_blocco.Add(new blocco());

                        int num = c_save.crea_blocco.Count - 1;

                        if (tipo_blocco <= 4)
                        {
                            c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_" + tipo_blocco, typeof(GameObject))) as GameObject;

                            c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);
                        }
                        else
                        {
                            crea_blocco_mesh2(num,tipo_blocco, c_save.crea_blocco[num].struttura_procedurale, c_save.crea_blocco[num].struttura_procedurale_dz);
                        }

                        float rad_calcolo = rad;


                        if (tipo_blocco >= 9)
                        {

                       //     rad = rad_calcolo + cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;
                        }


                        c_save.crea_blocco[num].altezza = Vector3.Distance(m_ray, new Vector3(0, 0, m_ray.z));





                        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
                        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

                        float angolo = rad * Mathf.Rad2Deg;

                        c_save.crea_blocco[num].arrivo = 0;
                       
                        c_save.crea_blocco[num].attivo = 0;
                        c_save.crea_blocco[num].pos = m_ray.z;
                        c_save.crea_blocco[num].rad = rad;
                        c_save.crea_blocco[num].tipo = tipo_blocco;


                        c_save.crea_blocco[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

                        c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(xx, yy, c_save.crea_blocco[num].pos);

                        if (tipo_blocco >= 9)
                        {

                            c_save.crea_blocco[num].mesh.transform.localPosition = new Vector3(0, 0, c_save.crea_blocco[num].pos);

                        }


                    }



                }

            }


            if (attivo_funzione == 2)
            {
                if (gemma_base != null)
                {
                    gemma_base.SetActive(true);


                    float rad = posizione_base(gemma_base, m_ray);


                    if (Input.GetMouseButtonUp(0))
                    {
                        c_save.crea_gemma.Add(new gemma());

                        int num = c_save.crea_gemma.Count - 1;

                        c_save.crea_gemma[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/gemma_0", typeof(GameObject))) as GameObject;

                  //      rad = rad + cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;

                        c_save.crea_gemma[num].altezza = c_save.crea_cilindro[0].raggio;

                        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
                        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

                        float angolo = rad * Mathf.Rad2Deg;


                        c_save.crea_gemma[num].pos = m_ray.z;
                        c_save.crea_gemma[num].rad = rad;
                       


                        c_save.crea_gemma[num].mesh.transform.SetParent(cilindro.transform);

                        c_save.crea_gemma[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

                        c_save.crea_gemma[num].mesh.transform.localPosition = new Vector3(xx, yy, c_save.crea_gemma[num].pos);

                    }



                }

            }

            if (attivo_funzione == 3)
            {
                if (moneta_base != null)
                {
                    moneta_base.SetActive(true);


                    float rad = posizione_base(moneta_base, m_ray);


                    if (Input.GetMouseButtonUp(0))
                    {
                        c_save.crea_moneta.Add(new moneta());

                        int num = c_save.crea_moneta.Count - 1;

                        c_save.crea_moneta[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/moneta_0", typeof(GameObject))) as GameObject;

                    //    rad = rad + cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;

                        c_save.crea_moneta[num].altezza = c_save.crea_cilindro[0].raggio;

                        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
                        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

                        float angolo = rad * Mathf.Rad2Deg;


                        c_save.crea_moneta[num].pos = m_ray.z;
                        c_save.crea_moneta[num].rad = rad;
             

                        c_save.crea_moneta[num].mesh.transform.SetParent(cilindro.transform);

                        c_save.crea_moneta[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

                        c_save.crea_moneta[num].mesh.transform.localPosition = new Vector3(xx, yy, c_save.crea_moneta[num].pos);

                    }



                }

            }

            if (attivo_funzione == 5)
            {
                if (malus_base != null)
                {
                    malus_base.SetActive(true);


                    float rad = posizione_base(malus_base, m_ray);


                    if (Input.GetMouseButtonUp(0))
                    {
                        c_save.crea_malus.Add(new malus());

                        int num = c_save.crea_malus.Count - 1;

                        c_save.crea_malus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/malus_"+tipo_malus, typeof(GameObject))) as GameObject;

                        if (tipo_malus == 0)
                        {
                            modifica_base(c_save.crea_malus[num].mesh);
                        }


                    //    rad = rad + cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;

                        c_save.crea_malus[num].altezza = c_save.crea_cilindro[0].raggio;// Vector3.Distance(m_ray, new Vector3(0, 0, m_ray.z));

                        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
                        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

                        float angolo = rad * Mathf.Rad2Deg;


                        c_save.crea_malus[num].pos = m_ray.z;
                        c_save.crea_malus[num].rad = rad;
                        c_save.crea_malus[num].altezza = c_save.crea_cilindro[0].raggio;
                        c_save.crea_malus[num].tipo = tipo_malus;

                        c_save.crea_malus[num].mesh.transform.SetParent(cilindro.transform);

                        c_save.crea_malus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

                        c_save.crea_malus[num].mesh.transform.localPosition = new Vector3(xx, yy, c_save.crea_malus[num].pos);

                    }



                }

            }

            if (attivo_funzione == 7)
            {
                if (bonus_base != null)
                {
                    bonus_base.SetActive(true);


                    float rad = posizione_base(bonus_base, m_ray);


                    if (Input.GetMouseButtonUp(0))
                    {
                        c_save.crea_bonus.Add(new bonus());

                        int num = c_save.crea_bonus.Count - 1;

                        c_save.crea_bonus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/bonus_" + tipo_bonus, typeof(GameObject))) as GameObject;

                        if (tipo_bonus == 0)
                        {
                            modifica_base(c_save.crea_bonus[num].mesh);
                        }

                     //   rad = rad + cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;

                        c_save.crea_bonus[num].altezza = c_save.crea_cilindro[0].raggio;

                        float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
                        float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

                        float angolo = rad * Mathf.Rad2Deg;


                        c_save.crea_bonus[num].pos = m_ray.z;
                        c_save.crea_bonus[num].rad = rad;
                 
                        c_save.crea_bonus[num].tipo = tipo_bonus;

                        c_save.crea_bonus[num].mesh.transform.SetParent(cilindro.transform);

                        c_save.crea_bonus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

                        c_save.crea_bonus[num].mesh.transform.localPosition = new Vector3(xx, yy, c_save.crea_bonus[num].pos);

                    }



                }

            }

        }


    }


    float posizione_base_blocco(GameObject ogg, Vector3 m_ray, int tipo_blocco )
    {

        float xx = m_ray.x;
        float yy = m_ray.y;

        float dis_min = 1000;

        int num_dis = 0;

        for (int n = 0; n < 37; n++)
        {

            float xp = xx - snap_rif2[n].transform.position.x;
            float yp = yy - snap_rif2[n].transform.position.y;

            float dis = Mathf.Sqrt(xp * xp + yp * yp);

            if (dis < dis_min)
            {
                dis_min = dis;
                num_dis = n;
            }

        }


        xx = snap_rif2[num_dis].transform.position.x;
        yy = snap_rif2[num_dis].transform.position.y;

       



        float rad = Mathf.Atan2(xx, yy) +cilindro.transform.localEulerAngles.z*Mathf.Deg2Rad;



        float angolo = rad * Mathf.Rad2Deg;


        float altezza = c_save.crea_cilindro[0].raggio;


     //   Debug.Log("rad "+rad);


        xx = Mathf.Sin(rad) * altezza;
        yy = Mathf.Cos(rad) * altezza;

        float zz = m_ray.z;

        ogg.transform.localEulerAngles = new Vector3(0, 0, -angolo);


        if (tipo_blocco <= 8)
        {
            ogg.transform.localPosition = new Vector3(xx, yy, zz);
            
        }


        if (tipo_blocco >=9)
        {
            ogg.transform.localPosition = new Vector3(0, 0, zz);
           
        }


        return rad;

    }

    float posizione_base(GameObject ogg, Vector3 m_ray)
    {

        float xx = m_ray.x;
        float yy = m_ray.y;

        float dis_min = 1000;

        int num_dis = 0;





        for (int n = 0; n < 37; n++)
        {

            float xp = xx - snap_rif[n].transform.position.x;
            float yp = yy - snap_rif[n].transform.position.y;

            float dis = Mathf.Sqrt(xp * xp + yp * yp);

            if (dis < dis_min)
            {
                dis_min = dis;
                num_dis = n;
            }

        }


        xx = snap_rif[num_dis].transform.position.x;
        yy = snap_rif[num_dis].transform.position.y;


        float rad = Mathf.Atan2(xx, yy) + cilindro.transform.localEulerAngles.z * Mathf.Deg2Rad;



        float angolo = rad * Mathf.Rad2Deg;


        float altezza = c_save.crea_cilindro[0].raggio;





        xx = Mathf.Sin(rad) * altezza;
        yy = Mathf.Cos(rad) * altezza;

        float zz = m_ray.z;

        ogg.transform.localPosition = new Vector3(xx, yy, zz);
        ogg.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        return rad;

    }


    void save_project()
    {

        c_save.crea_cilindro[0].pos_finale = finale.transform.position;



        string jsonData = JsonUtility.ToJson(c_save, true);

        string file_name = "livello_1";


        if (level_json != null)
        {
            file_name = level_json.name;

        }

        file_name = file_name.Replace(".json", "");


        string path_data = "Assets/Resources/data_level/" + file_name + ".json";


        File.WriteAllText(path_data, jsonData, Encoding.UTF8);

#if UNITY_EDITOR
        AssetDatabase.Refresh();

#endif



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


                int num_cilindro = c_save.crea_cilindro.Count;

                if (num_cilindro == 0)
                {
                    c_save.crea_cilindro.Add(new cilindro());
                }


                cilindro.transform.localEulerAngles = new Vector3(0, 0, 0);

                calcolo_snap(1);


                scala_cilindro();


                partenza.transform.position = new Vector3(0, c_save.crea_cilindro[0].raggio + .51f, 0);

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
            crea_blocco_mesh2(num, tipo_blocco, c_save.crea_blocco[num].struttura_procedurale, c_save.crea_blocco[num].struttura_procedurale_dz);
        }


        c_save.crea_blocco[num].mesh.name = "blocco " + tipo_blocco;

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

        c_save.crea_moneta[num].mesh.name = "moneta " ;

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

        c_save.crea_malus[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/malus_" + tipo_malus, typeof(GameObject))) as GameObject;

        c_save.crea_malus[num].mesh.name = "crea_malus " + tipo_malus;

        if (tipo_malus == 0)
        {
            modifica_base(c_save.crea_malus[num].mesh);
        }

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

        c_save.crea_bonus[num].mesh.name = "crea_bonus " + tipo_bonus;

        if (tipo_bonus == 0)
        {
            modifica_base(c_save.crea_bonus[num].mesh);
        }

        c_save.crea_bonus[num].mesh.transform.SetParent(cilindro.transform);

        c_save.crea_bonus[num].mesh.transform.localEulerAngles = new Vector3(0, 0, -angolo);

        c_save.crea_bonus[num].mesh.transform.localPosition = new Vector3(xx, yy, zz);


    }


    void leggi_vertici_cilindro()
    {



        Mesh mesh = cilindro.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            vertici_cilindro[i] = vertices[i] ;
        }

     
    }


    void scala_cilindro()
    {



        Mesh mesh = cilindro.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertici_cilindro[i] *c_save.crea_cilindro[0].raggio;
        }

        mesh.vertices = vertices;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();


        DestroyImmediate(cilindro.GetComponent<MeshCollider>());
        

        cilindro.AddComponent<MeshCollider>();



        modifica_base(malus_base);
        modifica_base(bonus_base);





    }


    void modifica_base(GameObject ogg)
    {


        Debug.Log("modifica "+ogg.name+" tipo bonus "+tipo_bonus);



        Vector3[] pos_v = new Vector3[50];


        float rad2 = Mathf.PI / 18.0f;

        float altezza = .05f;

        for (int n = 0; n < 49; n++)
        {

            float rad = rad2 * n;

           

            float xx = Mathf.Sin(rad) * (c_save.crea_cilindro[0].raggio + altezza);
            float yy = Mathf.Cos(rad) * (c_save.crea_cilindro[0].raggio + altezza)- c_save.crea_cilindro[0].raggio;

            pos_v[n] = new Vector3(xx,yy,0);


            }


            Mesh mesh = ogg.GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        int num_v = 5;
        int num_tria = num_v-1;

        Vector3[] vertices = new Vector3[num_v*2];
        Vector2[] uvs = new Vector2[num_v * 2];
        int[] tria = new int[num_tria*6];


        float raggio = c_save.crea_cilindro[0].raggio;

        float u = 1.0f / (num_v-1);



        for (int n = 0; n < num_v; n++)
        {
            vertices[n] = pos_v[34+n];

            vertices[n+num_v] = vertices[n];
            vertices[n + num_v].z = vertices[n + num_v].z + 6;

            uvs[n] = new Vector2(u*n, 0);
            uvs[n + num_v] = new Vector2(u * n, 1);
        }


        for (int n = 0; n < num_v-1; n++)
        {
            int num_t = n * 6;

            tria[num_t+0] = 0+n;
            tria[num_t+1] = 5 + n;
            tria[num_t+2] = 6 + n;

            tria[num_t+3] = 0 + n;
            tria[num_t+4] = 6 + n;
            tria[num_t+5] = 1 + n;

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



    void crea_sfera(Vector3 pos,string str="")
    {
        GameObject sfera = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere", typeof(GameObject))) as GameObject;
        sfera.transform.position = pos;

        sfera.name = "" + str;
    }


    void crea_blocco_mesh(int tipo)
    {
       

             if (blocco_base != null)
        {

            DestroyImmediate(blocco_base);
        }

        blocco_base = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_mesh", typeof(GameObject))) as GameObject;

        blocco_base.transform.SetParent(cilindro.transform);

        blocco_base.name = "blocco_base_"+tipo;

     //   cilindro.transform.localEulerAngles =new Vector3(0,0, 0);



       scala_blocco(blocco_base, tipo);

       


    }

    void crea_blocco_mesh2(int num, int tipo, string struttura, string struttura_dz)
    {


        if (c_save.crea_blocco[num].mesh != null)
        {

            DestroyImmediate(c_save.crea_blocco[num].mesh);
        }

        c_save.crea_blocco[num].mesh = Instantiate(Resources.Load("grafica_3d/Prefabs_space/blocco_mesh", typeof(GameObject))) as GameObject;

       

        scala_blocco(c_save.crea_blocco[num].mesh, tipo, struttura,struttura_dz);

        c_save.crea_blocco[num].mesh.transform.SetParent(cilindro.transform);
    }


    void scala_blocco(GameObject ogg, int tipo, string struttura="",string struttura_dz="")
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

            ristruttura_blocco(ogg, tipo, struttura,struttura_dz);

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
            testo_ogg = "2344562364847532532354235";
            dz = new Vector3(0, 0, 1);
        }




        if (tipo == 100)
        {
            testo_ogg = ""+struttura;
           testo_ogg_dz = ""+ struttura_dz;

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

        Vector3[] vertices =new Vector3[num_vertici];
        Vector2[] uvs = new Vector2[num_vertici];
        int[] triangles = new int[num_tria*3];

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

                int mo =n* 2;

                
                Vector3 snap_n = snap_rif[mo].transform.position;
                Vector3 snap_n1 = snap_rif[mo+2].transform.position;

                Vector3 snap_up_n = snap_rif[mo+codice*50].transform.position;
                Vector3 snap_up_n1 = snap_rif[mo+2 + codice * 50].transform.position;

                if (mesh_v[n] >= 9)
                {
                    snap_n = snap_rif[mo + 4 * 50].transform.position;
                    snap_n1 = snap_rif[mo+2 + 4 * 50].transform.position;

                    snap_up_n = snap_rif[mo + 6 * 50].transform.position;
                    snap_up_n1 = snap_rif[mo+2 + 6 * 50].transform.position;

                }


                dz = dz_base;

                if (mesh_v_dz[n] > 0)
                {
                    dz = new Vector3(0,0, mesh_v_dz[n])* spostamento_z;

                }

                Vector3 dz2 = dz * spostamento_z2;

                vertices[nv+0] =snap_n + dz2;
                vertices[nv + 1] = snap_up_n + dz2;
                vertices[nv + 2] = snap_up_n1 + dz2;
                vertices[nv + 3] = snap_n1 + dz2;

                vertices[nv + 4] = snap_n+dz;
                vertices[nv + 5] = snap_up_n + dz ;
                vertices[nv + 6] = snap_up_n1 + dz ;
                vertices[nv + 7] = snap_n1 + dz ;

                vertices[nv + 8] = snap_n + dz2;
                vertices[nv + 9] = snap_up_n + dz2;
                vertices[nv + 10] = snap_up_n+dz;
                vertices[nv + 11] = snap_n+dz ;

                vertices[nv + 12] = snap_n1 + dz2;
                vertices[nv + 13] = snap_up_n1 + dz2;
                vertices[nv + 14] = snap_up_n1 + dz ;
                vertices[nv + 15] = snap_n1 + dz ;


                vertices[nv + 16] = snap_up_n + dz2;
                vertices[nv + 17] = snap_up_n + dz ;
                vertices[nv + 18] = snap_up_n1 + dz ;
                vertices[nv + 19] = snap_up_n1 + dz2;

                vertices[nv +20] = snap_n + dz2;
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

                triangles[nt + 0] = 0+ nv;
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
                triangles[nt + 13] =8 + nv;
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

        ogg.transform.localScale = new Vector3(1,1,1);


        float red = UnityEngine.Random.Range(.0f, 1.0f);
        float green = UnityEngine.Random.Range(.0f, 1.0f);
        float blu = UnityEngine.Random.Range(.0f, 1.0f);


       // ogg.GetComponent<Renderer>().material.SetColor("_Color", new Color(red,green, blu));

        //   Debug.Break();

    }



    void genera_livello()
    {

        float raggio = c_save.crea_cilindro[0].raggio;

        delete_project();


        c_save = null;
        c_save = new classe_save();

        c_save.crea_cilindro.Add(new cilindro());

       

        c_save.crea_cilindro[0].raggio = raggio;
        c_save.crea_cilindro[0].spostamento_z = spostamento_z;
        c_save.crea_cilindro[0].spostamento_z2 = spostamento_z2;

        genera_livello(distanza_partenza_blocchi,distanza_blocchi,distanza_random);
      

       


    }


    void genera_livello(float distanza_partenza, float distanza, float distanza_rnd)
    {


        float rad_base = Mathf.PI * 2 / 36.0f;

        float pz_ultimo = 0;

        for (int n = 0; n < max_linee_blocchi; n++)
        {

            if (solo_mine_e_ostacoli == false)
            {

                string struttura = struttura_generata(numero_aperture_minimo, numero_aperture_massimo, 1, 4, 1);

                c_save.crea_blocco.Add(new blocco());

                int num = c_save.crea_blocco.Count - 1;

                c_save.crea_blocco[num].tipo = 100;
                c_save.crea_blocco[num].rad = rad_base;


                if (UnityEngine.Random.Range(0, 100) < percentuale_blocchi_rotanti)
                {
                    if (UnityEngine.Random.Range(0, 10) > 5)
                    {
                        c_save.crea_blocco[num].rotazione = .2f;
                    }
                    else
                    {
                        c_save.crea_blocco[num].rotazione = -.2f;
                    }


                }


                float pos_z = distanza_partenza + n * distanza + UnityEngine.Random.Range(-distanza_rnd, distanza_rnd);

                c_save.crea_blocco[num].pos = pos_z;

                pz_ultimo = pos_z;

                c_save.crea_blocco[num].struttura_procedurale = struttura;
                c_save.crea_blocco[num].struttura_procedurale_dz = struttura_generata_dz;

            }
            else
            {
                float pos_z = distanza_partenza + n * distanza + UnityEngine.Random.Range(-distanza_rnd, distanza_rnd);

                pz_ultimo = pos_z;
                genera_bonus(n, 4, distanza, distanza_rnd, distanza_partenza);

            }
            

            genera_altri_blocchi(n, numero_mine_ostacoli, distanza, distanza_rnd, distanza_partenza);

        }


        finale.transform.position =new Vector3(0,0, pz_ultimo + distanza*2);


        genera_livello_dati();


    }




    string struttura_generata(int numero_minimo_buchi, int numero_massimo_buchi, int min, int maxx, int genera_porta)
    {
        string struttura = "";

        for (int nk = 0; nk < 1000; nk++)
        {

            struttura = "";
            struttura_generata_dz = "";

            for (int n = 0; n < 19; n++)
            {

                int bb = (int)(UnityEngine.Random.Range(min,maxx+ .49f));

                struttura = struttura + "" + bb;


            }

            for (int n = 0; n < 19; n++)
            {

                int bb = (int)(UnityEngine.Random.Range(2, 7));

                struttura_generata_dz = struttura_generata_dz + "" + bb;


            }



          


            string struttura_controllo = "";

            string[] struc = new string[40];

            for (int n = 0; n < struttura.Length; n++)
            {

                string str = struttura.Substring(n, 1);

                if (UnityEngine.Random.Range(0.0f, 10.0f) > (8.5f- numero_massimo_buchi*.15f))
                {
                    str = "0";

                }

                struc[n] = str;

               

            }

           


            if (genera_porta > 0)
            {

                for (int n = 1; n < 18; n++)
                {

                    if (UnityEngine.Random.Range(0.0f, 10.0f) > 8.5f)
                    {
                        if (struc[n] == "0")
                        {
                            struc[n - 1] = "8";
                            struc[n] = "9";
                            struc[n + 1] = "8";
                        }

                    }

                }

              



            }



            if (buchi_doppi == true)
            {
                for (int n = 1; n < 18; n++)
                {

                    if (struc[n] == "0")
                    {
                        struc[n - 1] = "0";


                        n = n + 2;


                    }

                  

                }

            }

            int vuoto_creato = 0;

            for (int n = 0; n < 19; n++)
            {
                if (struc[n] == "0" || struc[n]=="9")
                {
                    vuoto_creato = vuoto_creato + 1;
                }

               
                    struttura_controllo = struttura_controllo + struc[n];

            }

          


                if (vuoto_creato>= numero_minimo_buchi && vuoto_creato <= numero_massimo_buchi)
            {

                Debug.Log("");

                return struttura_controllo;



            }

        }


        return struttura;


    }



    void crea_genera_monete()
    {

        int num_moneta2 = c_save.crea_moneta.Count;

        for (int k = 0; k < num_moneta2; k++)
        {
            if (c_save.crea_moneta[k].mesh != null)
            {
                DestroyImmediate(c_save.crea_moneta[k].mesh);

            }

        }



        c_save.crea_moneta.Clear();




        for (int n = 0; n < max_linee_blocchi; n++)
        {

            genera_monete(n, fila_monete_cirolari, numero_monete_linea, distanza_blocchi, .25f, 5);

        }


       



        int num_monete = c_save.crea_moneta.Count;

        for (int k = 0; k < num_monete; k++)
        {
            crea_moneta(k);
        }



    }





    void genera_livello_dati()
    {

        cilindro.transform.localEulerAngles = new Vector3(0, 0, 0);

        calcolo_snap(1);


        scala_cilindro();


        partenza.transform.position = new Vector3(0, c_save.crea_cilindro[0].raggio + .51f, 0);

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



    void genera_monete(int pos, int numero_monete, int max_monete_linea, float distanza, float distanza_rnd, float distanza_partenza)
    {


        int max_monete_ciclo = (int)(UnityEngine.Random.Range(max_monete_linea*.5f, max_monete_linea+1));
       

        for (int n = 0; n <= numero_monete; n++)
        {
            if (UnityEngine.Random.Range(0, 100.0f) < percentuale_monete)
            {

                float rad = UnityEngine.Random.Range(0.0f, 6.28f);


                float ok_linea= UnityEngine.Random.Range(-10.0f, 10.0f);

                int direzione = 0;

                if (ok_linea < -4)
                {
                    direzione = -1;
                }

                if (ok_linea > 4)
                {
                    direzione = 1;
                }


                for (int kn = 0; kn <= max_monete_ciclo; kn++)
                {


                    float pos_moneta = distanza_partenza + pos * distanza + UnityEngine.Random.Range(-distanza_rnd, distanza_rnd) + distanza * .5f+kn*2 ;


                   // Debug.Log(distanza_partenza+" pos_moneta " + pos_moneta+" "+ distanza+" pos "+pos);

                    float rad_direzione = direzione * 1.5f/c_save.crea_cilindro[0].raggio*kn;

                 //   Debug.Log(""+ rad_direzione);

                    int ok_moneta = 1;


                    ok_moneta = controllo_coll(pos_moneta, rad+ rad_direzione);


                    if (ok_moneta == 1)
                    {

                        c_save.crea_moneta.Add(new moneta());

                        int num = c_save.crea_moneta.Count - 1;

                        c_save.crea_moneta[num].tipo = 0;
                        c_save.crea_moneta[num].rad = rad + rad_direzione;

                        c_save.crea_moneta[num].pos = pos_moneta;


                    }
                    else
                    {
                        kn = 1000;
                    }

                }

            }

        }




    }


    int controllo_coll(float pos_moneta,float rad)
    {
        int ok = 1;


        float valore_rnd = .5f / c_save.crea_cilindro[0].raggio;

        for (int n = 0; n < 20; n++)
        {

            float pos_rnd = UnityEngine.Random.Range(-1.5f,1.5f);




            float rad_rnd= UnityEngine.Random.Range(-valore_rnd, valore_rnd);

            float xx = Mathf.Sin(rad+ rad_rnd) * 50;
            float yy = Mathf.Cos(rad+ rad_rnd) * 50;

            Vector3 pos = new Vector3(xx, yy, pos_moneta+ pos_rnd);


            RaycastHit hit_collider;

            Vector3 pos_ray_direction = new Vector3(-xx, -yy, 0);

        //    Debug.DrawRay(pos, pos_ray_direction, new Color(1, 0, 0, 1), 5);



            if (Physics.Raycast(pos, pos_ray_direction, out hit_collider, 50))
            {
                if (hit_collider.collider.name != "cilindro_esatto")
                {
                    ok = 0;

                }
              //  Debug.Log(""+hit_collider.collider.name);



            }

        }

        return ok;


    }



    void genera_bonus(int pos, int numero_blocchi, float distanza, float distanza_rnd, float distanza_partenza)
    {

        float molt_rad = Mathf.PI * 2 / (numero_blocchi + 1);


        for (int n = 0; n <= numero_blocchi; n++)
        {
            if (UnityEngine.Random.Range(0, 100.0f) < 70f)
            {

               
                   float rad = UnityEngine.Random.Range(0, 6.28f);
               

                float pos_z = distanza_partenza + pos * distanza + UnityEngine.Random.Range(-distanza_rnd, distanza_rnd) + distanza * .5f;

                float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
                float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

                float zz = pos_z;

                Vector3 pos0 = new Vector3(xx, yy, zz);

                int ok = 1;

                for (int k = 0; k < c_save.crea_bonus.Count; k++)
                {
                    
                        float rad_p = c_save.crea_bonus[k].rad;


                        float px = Mathf.Sin(rad_p) * c_save.crea_cilindro[0].raggio;
                        float py = Mathf.Cos(rad_p) * c_save.crea_cilindro[0].raggio;

                        float pz = c_save.crea_bonus[k].pos;

                        float dis = Vector3.Distance(pos0, new Vector3(px, py, pz));

                        if (dis < 5)
                        {
                            ok = 0;
                            k = 100000;

                        }

                }



                if (ok == 1)
                {

                    c_save.crea_bonus.Add(new bonus());

                    int num = c_save.crea_bonus.Count - 1;

                    int t_bonus= (int)(UnityEngine.Random.Range(0, 100.49f));


                    c_save.crea_bonus[num].tipo = 0;

                    if (t_bonus > 70)
                    {
                        c_save.crea_bonus[num].tipo = 1;
                    }


                    if (t_bonus > 90)
                    {
                        c_save.crea_bonus[num].tipo = 2;
                    }

                    c_save.crea_bonus[num].rad = rad;

                    c_save.crea_bonus[num].pos = pos_z;

                }


            }

        }


    }




    void genera_altri_blocchi(int pos, int numero_blocchi,float distanza,float distanza_rnd, float distanza_partenza)
    {

        float molt_rad = Mathf.PI * 2 / (numero_blocchi+1);


        for (int n = 0; n <= numero_blocchi; n++)
        {
            if (UnityEngine.Random.Range(0, 100.0f) < percentuale_altri_blocchi)
            {

                float rad= molt_rad * n + UnityEngine.Random.Range(-.12f, .12f);

                if (solo_mine_e_ostacoli == true)
                {
                    rad = UnityEngine.Random.Range(0, 6.28f);
                }


                float pos_z = distanza_partenza + pos * distanza + UnityEngine.Random.Range(-distanza_rnd, distanza_rnd) + distanza * .5f;

                float xx = Mathf.Sin(rad) * c_save.crea_cilindro[0].raggio;
                float yy = Mathf.Cos(rad) * c_save.crea_cilindro[0].raggio;

                float zz = pos_z;

                Vector3 pos0 = new Vector3(xx, yy, zz);

                int ok = 1;

                for (int k = 0; k < c_save.crea_blocco.Count; k++)
                {
                    if (c_save.crea_blocco[k].tipo==3 || c_save.crea_blocco[k].tipo == 4)
                    {
                        float rad_p = c_save.crea_blocco[k].rad;


                        float px = Mathf.Sin(rad_p) * c_save.crea_cilindro[0].raggio;
                        float py = Mathf.Cos(rad_p) * c_save.crea_cilindro[0].raggio;

                        float pz = c_save.crea_blocco[k].pos;

                        float dis = Vector3.Distance(pos0, new Vector3(px,py,pz));

                        if (dis < 3)
                        {
                            ok = 0;
                            k = 100000;

                            Debug.Log("vicinooo");
                        }

                    }

                }



                if (ok == 1)
                {

                    c_save.crea_blocco.Add(new blocco());

                    int num = c_save.crea_blocco.Count - 1;

                    c_save.crea_blocco[num].tipo = (int)(UnityEngine.Random.Range(0, 1.49f) + 3);
                    c_save.crea_blocco[num].rad = rad;

                    c_save.crea_blocco[num].pos = pos_z;

                    c_save.crea_blocco[num].struttura_procedurale = "";
                    c_save.crea_blocco[num].struttura_procedurale_dz = "";

                }


            }

        }


    }


}
