using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class menu : MonoBehaviour {


    float xm, ym, xm_old, ym_old;

    public Color newColor = new Color(0, 1, 1, 1);

    float[] touch_x = new float[15];
    float[] touch_y = new float[15];

    float[] touch_rx = new float[15];
    float[] touch_ry = new float[15];

    float[] touch_xo = new float[15];
    float[] touch_yo = new float[15];

    public float scroll_x = 0;

    float diff_xm;
    float diff_ym;

    float risoluzione_x;
    float risoluzione_y;

    float rapporto_risoluzione;
    float spostamento_sx;
    float spostamento_sx2;

    float spostamento_x = 0;
    public float scroll_verticale_sx;
    float scroll_verticale_dx;

    static int num_ogg = 300;

    GameObject[] pulsante = new GameObject[num_ogg];
    GameObject[] pulsante_testo = new GameObject[num_ogg];

    GameObject[] grafica = new GameObject[num_ogg];
    GameObject[] grafica_testo = new GameObject[num_ogg];

    GameObject[] pulsante_field = new GameObject[num_ogg];
    GameObject[] pulsante_field_testo = new GameObject[num_ogg];
    GameObject[] pulsante_field_testo2 = new GameObject[num_ogg];

    


    struttura_dati script_struttura_dati;

    GameObject canvas;
    GameObject canvas_popup;

    int font_size = 14;

    int pagina = 0;

    float limite_verticale_sx = 0;
    float limite_verticale_dx = 0;

    int attivo_popUP = 0;

    void Start() {
        canvas = GameObject.Find("Canvas");
        canvas_popup = GameObject.Find("Canvas_popup/Panel");

        canvas_popup.SetActive(false);

        GameObject ogg_struttura_dati = GameObject.Find("base_struttura");


        if (ogg_struttura_dati != null)
        {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

            Debug.Log("" + script_struttura_dati.caratteristiche_forza);

        }

        controllo_risoluzione();

        crea_menu();
        
    }

    void Update() {
        controllo_risoluzione();

        aggiorna_menu();

        aggiorna_menu_popup();


#if UNITY_EDITOR

        if (Input.GetKeyUp(KeyCode.M)) {

            crea_menu();
        }

#endif

    }


     


void aggiorna_menu()
    {

        if (pagina == -1)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                scroll_verticale_sx = scroll_verticale_sx - risoluzione_y * .025f;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                scroll_verticale_sx = scroll_verticale_sx + risoluzione_y * .025f;
            }
        }

        if (pagina == 1)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                scroll_verticale_dx = scroll_verticale_dx - risoluzione_y * .025f;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                scroll_verticale_dx = scroll_verticale_dx + risoluzione_y * .025f;
            }
        }



        if (scroll_verticale_sx < 0)
        {
            scroll_verticale_sx = 0;
        }

        float limite_sy = -limite_verticale_sx - risoluzione_y * .15f;

        if (scroll_verticale_sx> limite_sy)
        {
            scroll_verticale_sx = limite_sy;
        }


        if (scroll_verticale_dx < 0)
        {
            scroll_verticale_dx = 0;
        }

        float limite_dy = -limite_verticale_dx - risoluzione_y * .15f;

        if (scroll_verticale_dx > limite_dy)
        {
            scroll_verticale_dx = limite_dy;
        }



        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            pagina = pagina - 1;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            pagina = pagina + 1;
        }

        if (pagina < -1)
        {
            pagina = -1;
        }
        if (pagina > 1)
        {
            pagina = 1;
        }


        scroll_x = Mathf.MoveTowards(scroll_x, pagina, Time.deltaTime*5);


        aggiorna_menu_sfondo();
        aggiorna_menu_fix();

        aggiorna_menu_centrale();
       
        aggiorna_menu_sinistra();
        aggiorna_menu_destra();
    }

    private void aggiorna_menu_sfondo()
    {
        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x=0;
        float pos_y = 0;

        font_size = (int)(risoluzione_x / 25);

        spostamento_x = -scroll_x * risoluzione_x;


        for (int n = 0; n <= 2; n++)
        {

            if (grafica[n] != null)  //frame top
            {
                float dx2 = risoluzione_x;
                float dy2 = risoluzione_y;
                pos_x = -risoluzione_x+risoluzione_x*n;
                pos_y = risoluzione_y * 0.5f - dy2 * 0.5f;

                grafica[n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x+ spostamento_x, pos_y);


            }

        }




    }

    private void aggiorna_menu_sinistra()
    {

        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;


        if (pulsante[10] != null)
        { //shop
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x = -risoluzione_x;
            pos_y = risoluzione_y * -0.15f;

            pulsante[10].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            pulsante[10].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y+scroll_verticale_sx);

            pulsante_testo[10].GetComponent<TextMeshProUGUI>().fontSize = font_size;

        }

        int aumento_pos_x = -1;
        int aumento_pos_y = 0;

        for (int n = 0; n < 9; n++)
        {

            float dx2 = risoluzione_x * .4f;
            float dy2 = dx2 ;

            aumento_pos_x = aumento_pos_x + 1;


            if (aumento_pos_x > 1)
            {
                aumento_pos_x = 0;
                aumento_pos_y = aumento_pos_y + 1;
            }

            pos_x = -risoluzione_x-risoluzione_x*.25f+aumento_pos_x*risoluzione_x*.5f;

            


            pos_y = risoluzione_y * .2f- aumento_pos_y*dy2*1.25f;

            if (pulsante[100 + n] != null)
            {

                pulsante[100 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                pulsante[100 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y+scroll_verticale_sx);


                limite_verticale_sx = pos_y  - dy2*.55f;
            }




        }





    }



    private void aggiorna_menu_destra() {

        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;


        if (pulsante[20] != null) { //shop
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x =   risoluzione_x;
            pos_y = risoluzione_y * 0.25f;
           
            pulsante[20].GetComponent<RectTransform>().sizeDelta = new Vector2(dx, dy);
            pulsante[20].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x+spostamento_x, pos_y + scroll_verticale_dx);
            
            pulsante_testo[20].GetComponent<TextMeshProUGUI>().fontSize = font_size;

        }

        if (pulsante[21] != null)  //main
        {

            float dx2 = dy * .8f;
            float dy2 = dx2 * (76f / 72f);
            pos_x =  risoluzione_x;
            pos_y = risoluzione_y * 0.05f ;
            
            pulsante[21].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            pulsante[21].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x+spostamento_x, pos_y + scroll_verticale_dx);
          
        }



        int aumento_pos_x = -1;
        int aumento_pos_y = 0;

        for (int n = 0; n < 9; n++)
        {

            float dx2 = risoluzione_x * .4f;
            float dy2 = dx2;

            aumento_pos_x = aumento_pos_x + 1;


            if (aumento_pos_x > 1)
            {
                aumento_pos_x = 0;
                aumento_pos_y = aumento_pos_y + 1;
            }

            pos_x = risoluzione_x - risoluzione_x * .25f + aumento_pos_x * risoluzione_x * .5f;




            pos_y = risoluzione_y * -.2f - aumento_pos_y * dy2 * 1.25f;

            if (pulsante[150 + n] != null)
            {

                pulsante[150 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                pulsante[150 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_dx);


                limite_verticale_dx = pos_y - dy2 * .55f;
            }




        }





    }

    public void controllo_risoluzione() {

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


        diff_xm = xm - xm_old;
        diff_ym = ym - ym_old;


    }




    void aggiorna_menu_popup()
    {


        float dy = risoluzione_y ;
        float dx = risoluzione_x ;

        float pos_x = 0;
        float pos_y = 0;


        for (int n = 0; n <= 10; n++)
        {
            if (grafica[200+n] != null)  
            {
                grafica[200+n].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, risoluzione_y);
            }

            if (pulsante[200 + n] != null)
            {
                pulsante[200 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, risoluzione_y);
            }
        }




        if (attivo_popUP == 1)
        {

            if (grafica[200] != null)  //pannello
            {

                Debug.Log("dsfsdf");

                float dx2 = dy * .8f;
                float dy2 = dx2;
                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }

            if (grafica[201] != null)  //mmagine
            {

                float dx2 = dy * .6f;
                float dy2 = dx2;
                pos_x = 0;
                pos_y = 0;

                grafica[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }



            if (pulsante[200] != null)  //settings
            {
                float dx2 = dy * .8f ;
                float dy2 = dy*.125f;
                pos_x = 0;
                pos_y = dy * -0.4f;
                pulsante[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }

        }


    }





    void aggiorna_menu_fix()
    {


        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;

        if (grafica[3] != null) // banda bot
        {
            float dx2 = risoluzione_x ;
            float dy2 = dy;
            pos_x = 0;
            pos_y = risoluzione_y * -0.5f + dy2 * 0.5f;
            grafica[3].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            
        }
        if (grafica[4] != null) // banda top
        {
            float dx2 = risoluzione_x ;
            float dy2 = dy;
            pos_x = 0;
            pos_y = risoluzione_y * 0.5f - dy2 * 0.5f;
            grafica[4].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            
        }


        if (pulsante[2] != null) //shop
        {
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x = risoluzione_x * 0.5f - dx2 * 0.5f;
            pos_y = risoluzione_y * -0.5f + dy2 * 0.5f;
            pulsante[2].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2*.97f, dy2);
            pulsante[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x , pos_y);


            pulsante_testo[2].GetComponent<TextMeshProUGUI>().fontSize = font_size;
        }
        if (pulsante[3] != null) //main
        {
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x = 0;
            pos_y = risoluzione_y * -0.5f + dy2 * 0.5f;
            pulsante[3].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            pulsante[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x , pos_y);


            pulsante_testo[3].GetComponent<TextMeshProUGUI>().fontSize = font_size;
        }
        if (pulsante[4] != null) //upgrade
        {
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x = risoluzione_x * 0.5f - dx2 * 0.5f;
            pos_y = risoluzione_y * -0.5f + dy2 * 0.5f;
            pulsante[4].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            pulsante[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x , pos_y);

            pulsante_testo[4].GetComponent<TextMeshProUGUI>().fontSize = font_size;
        }





        for (int n = 0; n <= 2; n++)
        {
            pulsante[n+2].GetComponent<Image>().color = new Color(1,1,1,1);

            if (pagina == -1)
            {
                pulsante[2].GetComponent<Image>().color = newColor;
            }

            if (pagina == 0)
            {
                pulsante[3].GetComponent<Image>().color = newColor;
            }

            if (pagina == 1)
            {
                pulsante[4].GetComponent<Image>().color = newColor;
            }


        }


        if (pulsante[1] != null)  //settings
{
            float dx2 = dy * .8f / 2;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * -.5f + dx2;
            pos_y = risoluzione_y * 0.45f;
            pulsante[1].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            pulsante[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[11] != null)  //coin
  {

            float dx2 = dy * .8f/2;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * .5f - dx2 * 2;
            pos_y = risoluzione_y * 0.45f ;

            grafica[11].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[11].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[12] != null)  //coin  txt
        {

            float dx2 = dy * .8f;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * .5f - dx2 / 2 * 2;
            pos_y = risoluzione_y * 0.5f - dy2 * .9f;

            grafica[12].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[12].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[12].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[12].GetComponent<TextMeshProUGUI>().text = "99999";


        }

  

        if (grafica[13] != null)  //gem
      {

            float dx2 = dy * .8f/2 ;
            float dy2 = dx2 * (84f / 70f) ;
            pos_x = 0;
            pos_y = risoluzione_y * 0.45f;

            grafica[13].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[13].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[14] != null)  //gem  txt
{

            float dx2 = dy * .8f;
            float dy2 = dx2 * (76f / 72f);
            pos_x = 0;
            pos_y = risoluzione_y * 0.5f - dy2 * .9f;

            grafica[14].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[14].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[14].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[14].GetComponent<TextMeshProUGUI>().text = "99999";


        }

    }


    void aggiorna_menu_centrale() {


        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;

       

        if (pulsante[0] != null) { //play
            pos_y = risoluzione_y * -.2f;

            pulsante[0].GetComponent<RectTransform>().sizeDelta = new Vector2(dx, dy);
            pulsante[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x+spostamento_x, pos_y);

            pulsante_testo[0].GetComponent<TextMeshProUGUI>().fontSize = font_size;

        }


    }



    void distruggi_menu() {

        for (int n = 0; n < 200; n++) {
            if (pulsante[n] != null) {
                DestroyImmediate(pulsante[n]);
            }

        }

        for (int n = 0; n < 200; n++)
        {
            if (grafica[n] != null)
            {
                DestroyImmediate(grafica[n]);
            }

        }


    }


    void distruggi_menu_popup()
    {

        for (int n = 200; n < pulsante.Length; n++)
        {
            if (pulsante[n] != null)
            {
                DestroyImmediate(pulsante[n]);
            }

        }

        for (int n = 200; n < grafica.Length; n++)
        {
            if (grafica[n] != null)
            {
                DestroyImmediate(grafica[n]);
            }

        }


    }



    void crea_menu() {

        distruggi_menu();

        //layout top fisso

        //     crea_grafica_text(14, new Color(1, 1, 1, 0), "", "");

        // sfondi

        crea_grafica_text(0, new Color(1, 1, 1, 1), "",canvas,"Canvas", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");
        crea_grafica_text(1, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");
        crea_grafica_text(2, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");


       


        // pagina sinistra


        crea_button_text(100, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");
        crea_button_text(101, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");
        crea_button_text(102, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");
        crea_button_text(103, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");

        crea_button_text(104, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");

        crea_button_text(105, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");

        crea_button_text(106, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");

        crea_button_text(107, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");

        crea_button_text(108, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");




        // pagina a destra
        crea_button_text(20, "DESTRA", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_Blue");
        crea_button_text(21, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Icon_PictoIcon_Setting");


        crea_button_text(150, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");
        crea_button_text(151, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");
        crea_button_text(152, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");
        crea_button_text(153, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");

        crea_button_text(154, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");

        crea_button_text(155, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Frame_carta_1");



        // pagina centrale
        crea_grafica_text(3, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");
        crea_grafica_text(4, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");


       
        crea_grafica_text(11, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/StatusBarIcon_Gold");
        crea_grafica_text(12, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");
        crea_button_text(1, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Icon_PictoIcon_Setting");


        crea_button_text(0, "PLAY", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_Blue");

        crea_button_text(2, "SHOP", new Color(0, 0, 0, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_White");
        crea_button_text(3, "MAIN", new Color(0, 0, 0, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_White");
        crea_button_text(4, "UPGRADE", new Color(0, 0, 0, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_White");
        crea_grafica_text(13, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/StatusBarIcon_Gem");
        crea_grafica_text(14, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");


    }







    void crea_popup(int num=0)
    {

        attivo_popUP = num ;

        distruggi_menu_popup();

        canvas_popup.SetActive(true);

        crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup,"Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White");
        crea_grafica_text(201, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");

        crea_button_text(200, "SHOP", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White");



    }




    void crea_button_text(int num, string txt, Color colore_testo,GameObject parent, string path="Canvas", string path_sprite = "") {

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


        pulsante_testo[num] = GameObject.Find(path+"/pulsante_text" + num + "/Text_TMP");

        pulsante_testo[num].GetComponent<TextMeshProUGUI>().text = "" + txt;

        pulsante_testo[num].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_y / 20);


        pulsante_testo[num].GetComponent<TextMeshProUGUI>().color = colore_testo;

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




    void crea_grafica_text(int num,Color colore, string txt, GameObject parent, string path="Canvas", string path_sprite = "") {

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




        grafica_testo[num] = GameObject.Find(path+"/grafica_text" + num + "/Text");



        grafica_testo[num].GetComponent<TextMeshProUGUI>().text = "" + txt;

        grafica_testo[num].GetComponent<TextMeshProUGUI>().fontSize = (int)(risoluzione_x/20);


    }


    void pressione_pulsante(int num) {

#if UNITY_EDITOR

        Debug.Log("pulsante "+num );

#endif



        if (num == 0) {
            SceneManager.LoadScene("gioco");
        }


        if (num == 2)
        {
            pagina = -1;
        }

        if (num == 3)
        {
            pagina = 0;
        }

        if (num == 4)
        {
            pagina = 1;
        }


        if (num == 100)
        {
            crea_popup(1);
        }


    }



    void pressione_input_text(int num, InputField tog) {


        if (tog.text != null || tog.text != "" && tog.text != "-" && tog.text != ".") {

            Debug.Log("pressione!" + tog.text + "!!");



        }


    }



}
