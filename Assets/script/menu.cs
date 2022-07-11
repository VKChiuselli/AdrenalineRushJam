using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using System.Globalization;
using TMPro;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {


#if UNITY_EDITOR
    [SerializeField]
#endif

    float xm, ym, xm_old, ym_old;

    public float variabile_x;
    public float variabile_y;

    Color newColor = new Color(1, 1, 1, 1);

    float[] touch_x = new float[15];
    float[] touch_y = new float[15];

    float[] touch_rx = new float[15];
    float[] touch_ry = new float[15];

    float[] touch_xo = new float[15];
    float[] touch_yo = new float[15];

    public float scroll_x = 0;

    float diff_xm;
    float diff_ym;

    float diff_ym_mo;

    float risoluzione_x;
    float risoluzione_y;

    float rapporto_risoluzione;
    float spostamento_sx;
    float spostamento_sx2;

    float spostamento_x = 0;
    public float scroll_verticale_sx;
    public float scroll_verticale_dx;
    public float scroll_verticale_dx_battlepass;

    static int num_ogg = 550;

    GameObject[] pulsante = new GameObject[num_ogg];
    GameObject[] pulsante_testo = new GameObject[num_ogg];

    GameObject[] grafica = new GameObject[num_ogg];
    GameObject[] grafica_testo = new GameObject[num_ogg];

    GameObject[] pulsante_field = new GameObject[num_ogg];
    GameObject[] pulsante_field_testo = new GameObject[num_ogg];
    GameObject[] pulsante_field_testo2 = new GameObject[num_ogg];

    float resetTimerPopup = 0;


    struttura_dati script_struttura_dati;
    public int indice_upgrade_corrente;
    public int indice_shop_corrente;
    public int indice_livello_corrente;
    public int indice_pagina_livello_corrente;

    GameObject canvas;
    GameObject canvas_popup;

    int font_size = 14;

    public int pagina = 0;

    float limite_verticale_sx = 0;
    float limite_verticale_dx = 0;
    float limite_verticale_dx_battlepass = 0;

    [SerializeField] int attivo_popup = 0;


    string[] upgrade_titolo = new string[20];
    string[] upgrade_descrittore = new string[20];
    int[] shop_quantita_monete = new int[20];

    int controllo_mobile = 0;


    AudioSource[] effetto_source_UI = new AudioSource[40];
    int[] effetto_source_UI_caricato = new int[40];

    AudioSource musica;


    GameObject cam0;

    void Start() {

        canvas = GameObject.Find("Canvas");
        canvas_popup = GameObject.Find("Canvas_popup/Panel");
        cam0 = GameObject.Find("Main Camera");

        carica_effetto_UI(1, "audio/Prefabs/click");
        carica_musica("audio/Prefabs/musica");

        canvas_popup.SetActive(false);

        GameObject ogg_struttura_dati = GameObject.Find("base_struttura");


        if (ogg_struttura_dati != null) {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();
            indice_livello_corrente = script_struttura_dati.livello_in_uso;
            indice_pagina_livello_corrente = script_struttura_dati.livello_in_uso / 5 + 1;

        }

        conta_nuove_stelle();

        controllo_risoluzione();

        aggiorno_stelle();


        crea_menu();
    }

    private void conta_nuove_stelle() {

        int stelle_nuove=0;
        for (int i = 0; i < 400; i++) {
            if (script_struttura_dati.stelle_livello[i] == "111") {
                stelle_nuove = stelle_nuove + 3;
            }
            else if (script_struttura_dati.stelle_livello[i] == "101" || script_struttura_dati.stelle_livello[i] == "110") {
                stelle_nuove = stelle_nuove + 2;
            }
            else if (script_struttura_dati.stelle_livello[i] == "100") {
                stelle_nuove = stelle_nuove + 1;
            }
        }

        if(stelle_nuove > script_struttura_dati.stelle_battle_pass) {
            script_struttura_dati.stelle_battle_pass = stelle_nuove;
            PlayerPrefs.SetInt("stelle_battle_pass", script_struttura_dati.stelle_battle_pass);
        }

    }

    private void aggiorno_stelle() {

        PlayerPrefs.SetString($"battle_pass_reward_free", calcolo_stelle_free());
        PlayerPrefs.SetString($"battle_pass_reward_premium", calcolo_stelle_premium());
    }

    private string calcolo_stelle_free() {

        char[] reward_string = script_struttura_dati.battle_pass_reward_free.ToCharArray();

        for (int i = 0; i < script_struttura_dati.stelle_battle_pass; i++) {


            if (reward_string[i] == '0') {
                reward_string[i] = '1';
            }
        }


        script_struttura_dati.battle_pass_reward_free = new string(reward_string);
        return script_struttura_dati.battle_pass_reward_free;
    }


    private string calcolo_stelle_premium() {

        char[] reward_string = script_struttura_dati.battle_pass_reward_premium.ToCharArray();

        for (int i = 0; i < script_struttura_dati.stelle_battle_pass; i++) {


            if (reward_string[i] == '0') {
                reward_string[i] = '1';
            }
        }


        script_struttura_dati.battle_pass_reward_premium = new string(reward_string);

        return script_struttura_dati.battle_pass_reward_premium;
    }



    void Update() {
        controllo_risoluzione();

        aggiorna_audio();

        aggiorna_menu();

        aggiorna_menu_popup();


#if UNITY_EDITOR

        if (Input.GetKeyUp(KeyCode.M)) {

            crea_menu();

            distruggi_menu_popup();


        }

#endif

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


    void aggiorna_menu() {



        diff_ym_mo = diff_ym_mo * .9f;


        if (pagina == -1) {

            if (controllo_mobile == 0) {

                if (Input.GetKey(KeyCode.UpArrow)) {
                    scroll_verticale_sx = scroll_verticale_sx - risoluzione_y * .025f;
                }

                if (Input.GetKey(KeyCode.DownArrow)) {
                    scroll_verticale_sx = scroll_verticale_sx + risoluzione_y * .025f;
                }

            }
            else {
                scroll_verticale_sx = scroll_verticale_sx + diff_ym_mo;

            }


        }

        if (pagina == 1) {

            if (controllo_mobile == 0) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    scroll_verticale_dx = scroll_verticale_dx - risoluzione_y * .025f;
                }

                if (Input.GetKey(KeyCode.DownArrow)) {
                    scroll_verticale_dx = scroll_verticale_dx + risoluzione_y * .025f;
                }

            }
            else {
                scroll_verticale_dx = scroll_verticale_dx + diff_ym_mo;

            }
        }

        if (pagina == 2) {

            if (controllo_mobile == 0) {

                if (Input.GetKey(KeyCode.UpArrow)) {
                    scroll_verticale_dx_battlepass = scroll_verticale_dx_battlepass - risoluzione_y * .025f;
                }

                if (Input.GetKey(KeyCode.DownArrow)) {
                    scroll_verticale_dx_battlepass = scroll_verticale_dx_battlepass + risoluzione_y * .025f;
                }


            }
            else {
                scroll_verticale_dx_battlepass = scroll_verticale_dx_battlepass + diff_ym_mo;

            }
        }





        if (scroll_verticale_sx < 0) {
            scroll_verticale_sx = 0;
        }

        float limite_sy = -limite_verticale_sx - risoluzione_y * .15f;

        if (scroll_verticale_sx > limite_sy) {
            scroll_verticale_sx = limite_sy;
        }


        if (scroll_verticale_dx < 0) {
            scroll_verticale_dx = 0;
        }

        float limite_dy = -limite_verticale_dx - risoluzione_y * .15f;

        if (scroll_verticale_dx > limite_dy) {
            scroll_verticale_dx = limite_dy;
        }

        /// battlepass scroll
        if (scroll_verticale_dx_battlepass < 0) {
            scroll_verticale_dx_battlepass = 0;
        }

        float limite_dy_battlepass = -limite_verticale_dx_battlepass - risoluzione_y * .15f;

        if (scroll_verticale_dx_battlepass > limite_dy_battlepass) {
            scroll_verticale_dx_battlepass = limite_dy_battlepass;
        }



        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            pagina = pagina - 1;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            pagina = pagina + 1;
        }

        if (pagina < -1) {
            pagina = -1;
        }
        if (pagina > 1 && pagina != 2) {
            pagina = 1;
        }


        scroll_x = Mathf.MoveTowards(scroll_x, pagina, Time.deltaTime * 5);


        aggiorna_menu_sfondo();
        aggiorna_menu_fix();

        aggiorna_menu_centrale();

        aggiorna_menu_sinistra();
        aggiorna_menu_destra();
        aggiorna_menu_battlepass();
    }


    private void aggiorna_menu_sfondo() {
        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = 0;

        font_size = (int)(risoluzione_x / 25);

        spostamento_x = -scroll_x * risoluzione_x;


        for (int n = 0; n <= 2; n++) {

            if (grafica[n] != null)  //frame SHOP MAIN UPGRADE
            {
                float dx2 = risoluzione_x;
                float dy2 = risoluzione_y;
                pos_x = -risoluzione_x + risoluzione_x * n;
                pos_y = risoluzione_y * 0.5f - dy2 * 0.5f;

                grafica[n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y);


            }

        }

        if (grafica[5] != null)  //frame BATTLEPASS
         {
            float dx2 = risoluzione_x;
            float dy2 = risoluzione_y;
            pos_x = -risoluzione_x + risoluzione_x * 3;
            pos_y = risoluzione_y * 0.5f - dy2 * 0.5f;

            grafica[5].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y);


        }


    }

    private void aggiorna_menu_sinistra() {

        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;


        if (pulsante[10] != null) { //shop
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x = -risoluzione_x;
            pos_y = risoluzione_y * -0.15f;

            pulsante[10].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            pulsante[10].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_sx);

            pulsante_testo[10].GetComponent<TextMeshProUGUI>().fontSize = font_size;

        }

        int aumento_pos_x = -1;
        int aumento_pos_y = 0;

        for (int n = 0; n < 10; n++) {

            float dx2 = risoluzione_x * .4f;
            float dy2 = dx2 * (650.0f / 470.0f);



            aumento_pos_x = aumento_pos_x + 1;


            if (aumento_pos_x > 1) {
                aumento_pos_x = 0;
                aumento_pos_y = aumento_pos_y + 1;
            }

            pos_x = -risoluzione_x - risoluzione_x * .25f + aumento_pos_x * risoluzione_x * .5f;




            pos_y = risoluzione_y * .1f - aumento_pos_y * dy2 * 1.05f;

            if (pulsante[100 + n] != null) {

                pulsante[100 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                pulsante[100 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_sx);


                limite_verticale_sx = pos_y - dy2 * .15f;
            }

            if (grafica[100 + n] != null) {

                grafica[100 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[100 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_sx + dy2 * -.33f);
                grafica_testo[100 + n].GetComponent<TextMeshProUGUI>().fontSize = risoluzione_x / 18f;
                if (100 + n == 100)
                    grafica_testo[100 + n].GetComponent<TextMeshProUGUI>().text = "FREE";
                else if (100 + n == 101)
                    grafica_testo[100 + n].GetComponent<TextMeshProUGUI>().text = "AD";
                else
                    grafica_testo[100 + n].GetComponent<TextMeshProUGUI>().text = "BUY";

            }

            if (grafica[120 + n] != null) {

                grafica[120 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[120 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_sx + dy2 * 0.33f);
                grafica_testo[120 + n].GetComponent<TextMeshProUGUI>().fontSize = risoluzione_x / 18f;
                grafica_testo[120 + n].GetComponent<TextMeshProUGUI>().text = "" + shop_quantita_monete[n + 1];


            }

        }

    }

    private void aggiorna_menu_battlepass() {

        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;


        int aumento_pos_y = -1;

        for (int n = 0; n < 100; n++) {

            float dx2 = risoluzione_x * 0.1f;
            float dy2 = dx2 * (650.0f / 470.0f);

            aumento_pos_y++;

            pos_x = 2 * risoluzione_x * 0.88f - risoluzione_x * .25f + risoluzione_x * .5f;

            pos_y = risoluzione_y * .1f * 1.9f - aumento_pos_y * dy2 * 1.05f * 1.8f; //distanza dall'inizio pagina  - gap tra stelle




            if (grafica[300 + n] != null) {

                grafica[300 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[300 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_dx_battlepass + dy2 * 0.33f);
                limite_verticale_dx_battlepass = pos_y - dy2 * .15f;

            }

            if (pulsante[300 + n] != null) { // free battlepass

                pulsante[300 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                pulsante[300 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x * 0.85f, pos_y + scroll_verticale_dx_battlepass + dy2 * 0.33f);
                limite_verticale_dx_battlepass = pos_y - dy2 * .15f;
                if (script_struttura_dati.battle_pass_reward_free[n] == '0' || script_struttura_dati.battle_pass_reward_free[n] == '2')
                    pulsante[300 + n].GetComponent<Button>().interactable = false;
            }

            if (pulsante[400 + n] != null) {// premium battlepass

                pulsante[400 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                pulsante[400 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x * 1.15f, pos_y + scroll_verticale_dx_battlepass + dy2 * 0.33f);
                limite_verticale_dx_battlepass = pos_y - dy2 * .15f;
                if (script_struttura_dati.battle_pass_reward_premium[n] == '0' || script_struttura_dati.battle_pass_reward_premium[n] == '2')
                    pulsante[400 + n].GetComponent<Button>().interactable = false;
            }



            if (grafica[297] != null) {


                pos_x = risoluzione_x * 2;
                pos_y = risoluzione_y * 0.25f * 1.35f;


                grafica[297].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * 3f, dy2);
                grafica[297].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x * 0.85f, pos_y + scroll_verticale_dx_battlepass);
                grafica_testo[297].GetComponent<TextMeshProUGUI>().text = "FREE";
                grafica_testo[297].GetComponent<TextMeshProUGUI>().fontSize = font_size / 0.7f;
            }

            if (grafica[298] != null) {

                pos_x = risoluzione_x * 2;
                pos_y = risoluzione_y * 0.25f * 1.35f;
                grafica[298].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * 3f, dy2);
                grafica[298].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_dx_battlepass);
                grafica_testo[298].GetComponent<TextMeshProUGUI>().text = "STARS";
                grafica_testo[298].GetComponent<TextMeshProUGUI>().fontSize = font_size / 0.7f;
            }

            if (grafica[299] != null) {

                pos_x = risoluzione_x * 2;
                pos_y = risoluzione_y * 0.25f * 1.35f;
                grafica[299].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * 3f, dy2);
                grafica[299].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x * 1.15f, pos_y + scroll_verticale_dx_battlepass);
                grafica_testo[299].GetComponent<TextMeshProUGUI>().text = "PREMIUM";
                grafica_testo[299].GetComponent<TextMeshProUGUI>().fontSize = font_size / 0.7f;
            }


        }

    }



    private void aggiorna_menu_destra() {

        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;

        int aumento_pos_x = -1;
        int aumento_pos_y = 0;

        for (int n = 0; n < 9; n++) {

            float dx2 = risoluzione_x * .44f;
            float dy2 = dx2 * (650 / 470.0f);

            float dx_image = dx2 * .8f;


            aumento_pos_x = aumento_pos_x + 1;


            if (aumento_pos_x > 1) {
                aumento_pos_x = 0;
                aumento_pos_y = aumento_pos_y + 1;
            }

            pos_x = risoluzione_x - risoluzione_x * .25f + aumento_pos_x * risoluzione_x * .5f;




            pos_y = risoluzione_y * .1f - aumento_pos_y * dy2 * 1.05f;

            if (pulsante[150 + n] != null) {

                pulsante[150 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                pulsante[150 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_dx);


                limite_verticale_dx = pos_y - dy2 * .15f;
            }

            if (grafica[150 + n] != null) {

                grafica[150 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2 * .1f);
                grafica[150 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_dx + dy2 * -.37f);
                grafica_testo[150 + n].GetComponent<TextMeshProUGUI>().fontSize = risoluzione_x / 18f;
                //  grafica_testo[150 + n].GetComponent<TextMeshProUGUI>().text = "titolo" ;


            }



            if (grafica[160 + n] != null) {

                grafica[160 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx_image, dx_image);
                grafica[160 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y + scroll_verticale_dx);

            }



            if (grafica[170 + n] != null) {

                grafica[170 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2 * .1f);
                grafica[170 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x * 1.15f, pos_y + scroll_verticale_dx + dy2 * 0.37f);
                grafica_testo[170 + n].GetComponent<TextMeshProUGUI>().fontSize = risoluzione_x / 18f;
               


            }

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
                diff_ym_mo = diff_ym;
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

        for (int n = 0; n <= 10; n++) {
            if (grafica[200 + n] != null) {
                grafica[200 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, risoluzione_y);
            }

            if (pulsante[200 + n] != null) {
                pulsante[200 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, risoluzione_y);
            }
        }




        if (attivo_popup == 1) {
            if (grafica[200] != null)  //pannello
            {

                float dx2 = dx * .8f;
                float dy2 = dy * .8f;

                dime_panel_x = dx2;
                dime_panel_y = dy2;

                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }

            if (grafica[201] != null)  //immagine
            {

                float dx2 = dy * .45f;
                float dy2 = dx2;
                pos_x = 0;
                pos_y = 0;

                grafica[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }
            if (grafica[202] != null)  //quantita monete acquistabili
            {

                float dx2 = dy * .45f;
                float dy2 = dx2;
                pos_x = 0;
                pos_y = dime_panel_y * 0.35f;

                grafica[202].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[202].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                grafica_testo[202].GetComponent<TextMeshProUGUI>().fontSize = risoluzione_x / 5f;
            }



            if (pulsante[200] != null)  //pulsante buy shop
            {
                float dx2 = risoluzione_x * 0.333f;
                float dy2 = risoluzione_y * 0.1f;
                pos_x = 0;
                pos_y = dime_panel_y * -0.5f + dy2 * .85f;
                pulsante[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[200].GetComponent<TextMeshProUGUI>().fontSize = dime_panel_y / 15f;

            }

            if (pulsante[201] != null)  //exit
            {
                float dx2 = risoluzione_x * 0.12f;
                float dy2 = dx2;
                pos_x = dime_panel_x * .465f;
                pos_y = dime_panel_y * .465f;


                pulsante[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }


            uscita_popup(dime_panel_x, dime_panel_y);

        } //popup shop
        else if (attivo_popup == 2) {
            if (grafica[200] != null)  //pannello
            {

                float dx2 = dx * .8f;
                float dy2 = dy * .8f;

                dime_panel_x = dx2;
                dime_panel_y = dy2;

                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }

            if (grafica[201] != null)  //titolo  txt
            {

                float dx2 = dy * .8f;
                float dy2 = dx2 * (76f / 72f);
                pos_x = 0;
                pos_y = risoluzione_y * 0.5f - dy2 * .25f;

                grafica[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                grafica_testo[201].GetComponent<TextMeshProUGUI>().fontSize = risoluzione_x / 8f;
                grafica_testo[201].GetComponent<TextMeshProUGUI>().text = upgrade_titolo[indice_upgrade_corrente];


            }


            if (grafica[202] != null)  //immagine oggetto
            {

                float dx2 = dy * .333f;
                float dy2 = dx2;
                pos_x = 0;
                pos_y = dime_panel_y * .1f;

                grafica[202].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[202].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }
            if (grafica[203] != null)  //testo costo valuta
            {
                float dx2 = dy * .8f;
                float dy2 = dx2 * (76f / 72f);
                pos_x = dime_panel_x * 0.2f;
                pos_y = dime_panel_y * -0.32f;

                grafica[203].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[203].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                grafica_testo[203].GetComponent<TextMeshProUGUI>().fontSize = risoluzione_x / 12;
                grafica_testo[203].GetComponent<TextMeshProUGUI>().text = "" + script_struttura_dati.costo_livello[script_struttura_dati.livello_upgrade[indice_upgrade_corrente]];

            }

            if (grafica[204] != null)  //immagine valuta
            {
                float dx2 = dx * .8f;
                float dy2 = dx2 * (118f / 596f);
                pos_x = 0;
                pos_y = dime_panel_y * -0.18f;
                grafica[204].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[204].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }



            if (pulsante[200] != null)  //buy tasto
            {
                float dx2 = dime_panel_x * 0.45f;
                float dy2 = dime_panel_y * 0.09f;
                pos_x = 0;
                pos_y = dime_panel_y * -0.42f;
                pulsante[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[200].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }

            if (pulsante[201] != null)  //exit tasto
            {
                float dx2 = risoluzione_x * 0.12f;
                float dy2 = dx2;
                pos_x = dime_panel_x * .46f;
                pos_y = dime_panel_y * .48f;


                pulsante[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }


            uscita_popup(dime_panel_x, dime_panel_y);

        } //popup upgrade
        else if (attivo_popup == 3)//popup opzioni
        {
            if (grafica[200] != null)  //pannello opzioni
            {

                float dx2 = dx * .8f;
                float dy2 = dy * .8f;

                dime_panel_x = dx2;
                dime_panel_y = dy2;

                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }

            if (grafica[201] != null)  //txt musica
            {
                float dx2 = dy * .8f;
                float dy2 = dx2 * (76f / 72f);
                pos_x = dx2 * -0.5f + dy2 * 0.333f;
                pos_y = risoluzione_y * 0.5f - dy2 * .25f;

                grafica[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                grafica_testo[201].GetComponent<TextMeshProUGUI>().fontSize = font_size;
                grafica_testo[201].GetComponent<TextMeshProUGUI>().text = "Musica";


            }

            if (pulsante[200] != null)  //musica tasto
            {
                float dx2 = risoluzione_x * 0.333f / 2;
                float dy2 = risoluzione_y * 0.1f / 2;
                pos_x = dy * .8f * -0.5f + dy * .8f * (76f / 72f) * 0.333f;
                pos_y = dy2 * 0.5f + dx * 0.8f / 2;
                pulsante[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[200].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }

            if (grafica[202] != null)  //txt sfx
            {

                float dx2 = dy * .8f;
                float dy2 = dx2 * (76f / 72f);
                pos_x = dx2 * 0.5f - dy2 * 0.333f;
                pos_y = risoluzione_y * 0.5f - dy2 * .25f;

                grafica[202].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[202].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                grafica_testo[202].GetComponent<TextMeshProUGUI>().fontSize = font_size;
                grafica_testo[202].GetComponent<TextMeshProUGUI>().text = "SFX";


            }

            if (pulsante[201] != null)  //tasto old sfx
            {
                float dx2 = risoluzione_x * 0.333f / 2;
                float dy2 = risoluzione_y * 0.1f / 2;
                pos_x = dy * .8f * +0.5f - dy * .8f * (76f / 72f) * 0.333f;
                pos_y = dy2 * 0.5f + dx * 0.8f / 2;
                pulsante[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[201].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }


            if (grafica[203] != null)  //txt facebook
            {

                float dx2 = dy * .8f;
                float dy2 = dx2 * (76f / 72f);
                pos_x = 0;
                pos_y = risoluzione_y * 0.5f - dy2 * 0.40f;

                grafica[203].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[203].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                grafica_testo[203].GetComponent<TextMeshProUGUI>().fontSize = font_size;
                grafica_testo[203].GetComponent<TextMeshProUGUI>().text = "Facebook";


            }

            if (pulsante[202] != null)  //tasto old facebook
            {
                float dx2 = dime_panel_x * .7f;
                float dy2 = risoluzione_y * 0.1f;
                pos_x = 0;
                pos_y = risoluzione_y * 0.5f - dy * .8f * (76f / 72f) * 0.50f;
                pulsante[202].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[202].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[202].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }

            if (grafica[204] != null)  //txt credits
            {


                float dx2 = dy * .8f;
                float dy2 = dx2 * (76f / 72f);
                pos_x = 0;
                pos_y = risoluzione_y * -0.5f + dy2 * 0.59f;

                grafica[204].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[204].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

                grafica_testo[204].GetComponent<TextMeshProUGUI>().fontSize = font_size;
                grafica_testo[204].GetComponent<TextMeshProUGUI>().text = "Credits";


            }

            if (pulsante[203] != null)  //tasto credits
            {
                float dx2 = dime_panel_x * .7f;
                float dy2 = dime_panel_y * 0.1f;
                pos_x = 0;
                pos_y = dime_panel_y * -0.09f;
                pulsante[203].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[203].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[203].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }

            if (pulsante[205] != null)  //tasto new facebook
            {
                float dx2 = dime_panel_x * .7f;
                float dy2 = dime_panel_y * 0.1f;
                pos_x = 0;
                pos_y = dime_panel_y * 0.12f;
                pulsante[205].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[205].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[205].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }

            if (pulsante[206] != null)  //tasto new on music
            {
                float dx2 = risoluzione_x * 0.333f / 2;
                float dy2 = risoluzione_y * 0.1f / 2;
                pos_x = dy * .8f * -0.5f + dy * .8f * (76f / 72f) * 0.333f;
                pos_y = dy2 * 0.5f + dx * 0.8f / 2;
                pulsante[206].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[206].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[206].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }
            if (pulsante[207] != null)  //tasto new on sfx
            {
                float dx2 = risoluzione_x * 0.333f / 2;
                float dy2 = risoluzione_y * 0.1f / 2;
                pos_x = dy * .8f * +0.5f - dy * .8f * (76f / 72f) * 0.333f;
                pos_y = dy2 * 0.5f + dx * 0.8f / 2;
                pulsante[207].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[207].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                pulsante_testo[207].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            }

            if (pulsante[201] != null)  //exit tasto
            {
                float dx2 = risoluzione_x * 0.12f;
                float dy2 = dx2;
                pos_x = dime_panel_x * .46f;
                pos_y = dime_panel_y * .48f;


                pulsante[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }


            uscita_popup(dime_panel_x, dime_panel_y);

        } //popup opzioni
        else if (attivo_popup == 4)//popup seleziona livelli
        {
            if (grafica[200] != null)  //pannello opzioni
            {

                float dx2 = dx * .8f;
                float dy2 = dy * .8f;

                dime_panel_x = dx2;
                dime_panel_y = dy2;

                pos_x = 0;
                pos_y = 0;

                grafica[200].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[200].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            }
            if (grafica[201] != null)  //titolo capitolo selezionato
            {

                float dx2 = dy * .43f;
                float dy2 = dx2;
                pos_x = 0;
                pos_y = dime_panel_y * 0.45f;

                grafica[201].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
                grafica[201].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);
                grafica_testo[201].GetComponent<TextMeshProUGUI>().fontSize = dime_panel_y / 15f;
                grafica_testo[201].GetComponent<TextMeshProUGUI>().text = "Chapter " + indice_pagina_livello_corrente;

            }

            for (int n = 0; n < 5; n++) {

                if (pulsante[202 + n] != null) //pulsante testo lista scelta
   {
                    float dx2 = dime_panel_x * 0.4f;
                    float dy2 = dime_panel_y * 0.09f;
                    pos_x = dime_panel_x * 0.25f;
                    pos_y = dime_panel_y * 0.33f + n * dime_panel_y * -0.15f;
                    pulsante[202 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                    pulsante[202 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x, pos_y);


                    pulsante_testo[202 + n].GetComponent<TextMeshProUGUI>().fontSize = dime_panel_y / 20f;
                }

            }

            for (int n = 0; n < 5; n++) {

                if (grafica[202 + n] != null) //stelle acquisite
   {
                    float dx2 = dime_panel_x * 0.33f;
                    float dy2 = dime_panel_y * 0.06f;
                    pos_x = dime_panel_x * -0.2f;
                    pos_y = dime_panel_y * 0.33f + n * dime_panel_y * -0.15f;
                    grafica[202 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                    grafica[202 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x, pos_y);

                }

            }

            if (pulsante[215] != null) //pulsante destra
{
                float dx2 = dime_panel_x * 0.09f;
                float dy2 = dime_panel_y * 0.09f;
                pos_x = dime_panel_x * -.4f;
                pos_y = dime_panel_y * 0.45f;
                pulsante[215].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[215].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x, pos_y);


            }
            if (pulsante[216] != null) //pulsante sinistra
{
                float dx2 = dime_panel_x * 0.09f;
                float dy2 = dime_panel_y * 0.09f;
                pos_x = dime_panel_x * 0.4f;
                pos_y = dime_panel_y * 0.45f;
                pulsante[216].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[216].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x, pos_y);


            }
            if (pulsante[217] != null) //pulsante gioca dal seleziona menu
{
                float dx2 = dime_panel_x * 0.45f;
                float dy2 = dime_panel_y * 0.09f;
                pos_x = 0;
                pos_y = dime_panel_y * -0.42f;
                pulsante[217].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                pulsante[217].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x, pos_y);
                pulsante_testo[217].GetComponent<TextMeshProUGUI>().fontSize = dime_panel_y / 15f;

            }


            uscita_popup(dime_panel_x, dime_panel_y);

        } //popup opzioni


    }



    void uscita_popup(float dime_panel_x, float dime_panel_y) {
        resetTimerPopup += Time.deltaTime;



        if (Input.GetMouseButtonDown(0) && resetTimerPopup > 0.5f) {
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



    void aggiorna_menu_fix() {


        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;

        if (grafica[3] != null) // banda bot
        {
            float dx2 = risoluzione_x;
            float dy2 = dy;
            pos_x = 0;
            pos_y = risoluzione_y * -0.5f + dy2 * 0.35f;
            grafica[3].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            grafica[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);


        }
        if (grafica[4] != null) // banda top
        {
            float dx2 = risoluzione_x;
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
            pulsante[2].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            pulsante[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x, pos_y);


            pulsante_testo[2].GetComponent<TextMeshProUGUI>().fontSize = font_size;
        }
        if (pulsante[3] != null) //main
        {
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x = 0;
            pos_y = risoluzione_y * -0.5f + dy2 * 0.5f;
            pulsante[3].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            pulsante[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);


            pulsante_testo[3].GetComponent<TextMeshProUGUI>().fontSize = font_size;
        }
        if (pulsante[4] != null) //upgrade
        {
            float dx2 = risoluzione_x * .333f;
            float dy2 = dx2 * (55f / 145f);
            pos_x = risoluzione_x * 0.5f - dx2 * 0.5f;
            pos_y = risoluzione_y * -0.5f + dy2 * 0.5f;
            pulsante[4].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            pulsante[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            pulsante_testo[4].GetComponent<TextMeshProUGUI>().fontSize = font_size;
        }

        for (int n = 0; n <= 2; n++) {
            pulsante[n + 2].GetComponent<Image>().color = new Color(1, 1, 1, 1);

            if (pagina == -1) {
                pulsante[2].GetComponent<Image>().color = newColor;
            }

            if (pagina == 0) {
                pulsante[3].GetComponent<Image>().color = newColor;
            }

            if (pagina == 1) {
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

            float dx2 = dy * .8f / 2;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * .5f - dx2 * 2;
            pos_y = risoluzione_y * 0.45f;

            grafica[11].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[11].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

        }

        if (grafica[12] != null)  //coin  txt
        {

            float dx2 = dy * .8f;
            float dy2 = dx2 * (76f / 72f);
            pos_x = risoluzione_x * .5f - dx2;
            pos_y = risoluzione_y * 0.5f - dy2 * .9f;

            grafica[12].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
            grafica[12].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x, pos_y);

            grafica_testo[12].GetComponent<TextMeshProUGUI>().fontSize = font_size;
            grafica_testo[12].GetComponent<TextMeshProUGUI>().text = "" + script_struttura_dati.monete;

        }

        if (grafica[13] != null)  //gem
        {

            float dx2 = dy * .8f / 2;
            float dy2 = dx2 * (84f / 70f);
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
            grafica_testo[14].GetComponent<TextMeshProUGUI>().text = "" + script_struttura_dati.gemme;


        }

    }


    void aggiorna_menu_centrale() {


        float dy = risoluzione_y * .125f;
        float dx = risoluzione_x * .7f;

        float pos_x = 0;
        float pos_y = risoluzione_y * .3f;

        if (pulsante[0] != null) {
            pos_y = risoluzione_y * -.2f;

            pulsante[0].GetComponent<RectTransform>().sizeDelta = new Vector2(dx, dy);
            pulsante[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y);

            pulsante_testo[0].GetComponent<TextMeshProUGUI>().fontSize = font_size;

        }

        if (pulsante[30] != null) {

            pos_y = risoluzione_y * 0.28f;

            pulsante[30].GetComponent<RectTransform>().sizeDelta = new Vector2(dx, dy);
            pulsante[30].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y);
            pulsante_testo[30].GetComponent<TextMeshProUGUI>().fontSize = font_size;
        }

        if (pulsante[31] != null) {

            float dx2 = risoluzione_x * 0.2f;
            float dy2 = risoluzione_y * 0.1f;
            pos_y = risoluzione_y * -0.33f;
            pulsante[31].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2, dy2);
            pulsante[31].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y);
        }

        if (grafica[20] != null) {
            pos_y = risoluzione_y * 0.15f;

            grafica[20].GetComponent<RectTransform>().sizeDelta = new Vector2(dx, dy);
            grafica[20].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y);

            grafica_testo[20].GetComponent<TextMeshProUGUI>().fontSize = font_size * 3f;

        }
        if (pulsante[11] != null) {

            float dx2 = risoluzione_x * 0.8f * 0.4f;
            float dy2 = risoluzione_y * 0.8f * 0.45f * 0.535f;

            pos_y = 0;

            pulsante[11].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 , dy2 );
            pulsante[11].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_x + spostamento_x, pos_y);


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

        canvas_popup.SetActive(false);

        for (int n = 200; n < 300; n++) {
            if (pulsante[n] != null) {
                DestroyImmediate(pulsante[n]);
            }

        }

        for (int n = 200; n < 286; n++) {
            if (grafica[n] != null) {
                DestroyImmediate(grafica[n]);
            }

        }


    }



    void crea_menu() {

        distruggi_menu();

        //layout top fisso


        // sfondi

        crea_grafica_text(0, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/sfondo_menu");
        crea_grafica_text(1, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/sfondo_menu");
        crea_grafica_text(2, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/sfondo_menu");
        crea_grafica_text(5, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/sfondo_menu"); //battlepass sfondo





        // pagina sinistra

        for (int n = 0; n < 10; n++) {


            crea_button_text(100 + n, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/frame_carta_shop " + (n + 1));
            crea_grafica_text(100 + n, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");
            crea_grafica_text(120 + n, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");

            if (n == 0) {
                pulsante[100].AddComponent<timer_reward>();
            }


            grafica[100 + n].GetComponent<Image>().raycastTarget = false;
            grafica_testo[100 + n].GetComponent<TextMeshProUGUI>().raycastTarget = false;
            grafica[120 + n].GetComponent<Image>().raycastTarget = false;
            grafica_testo[120 + n].GetComponent<TextMeshProUGUI>().raycastTarget = false;
        }

        //pagina battlepass 
        int stelle_acquisite = script_struttura_dati.stelle_battle_pass;

        crea_grafica_text(297, new Color(1, 1, 1, 0), "", canvas, "Canvas", ""); //text premium battlepass
        crea_grafica_text(298, new Color(1, 1, 1, 0), "", canvas, "Canvas", ""); //text stelle acquisite
        crea_grafica_text(299, new Color(1, 1, 1, 0), "", canvas, "Canvas", ""); //text free battlepass


        for (int n = 0; n < 100; n++) {
            if (stelle_acquisite > 0) {
                crea_grafica_text(300 + n, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/stella 1");
                stelle_acquisite--;
            }
            else
                crea_grafica_text(300 + n, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/stella 0");
        }

        for (int n = 0; n < 100; n++) {
            crea_button_text(300 + n, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/frame_carta_shop 1");
        }

        for (int n = 0; n < 100; n++) {
            crea_button_text(400 + n, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/frame_carta_shop 6");
        }


        // pagina a destra
        //   crea_button_text(20, "DESTRA", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_Blue");
        //   crea_button_text(21, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Icon_PictoIcon_Setting");

        // agilità 0
        // barriera 1
        // energia 2
        // spari 3
        // scafo 4
        // calamita 5

        upgrade_titolo[1] = "AGILITY";
        upgrade_titolo[2] = "BARRIER";
        upgrade_titolo[3] = "ENERGY";
        upgrade_titolo[4] = "AMMO";
        upgrade_titolo[5] = "SHIELD";
        upgrade_titolo[6] = "CALAMITY";

        shop_quantita_monete[1] = 100;
        shop_quantita_monete[2] = 200;
        shop_quantita_monete[3] = 1000;
        shop_quantita_monete[4] = 2000;
        shop_quantita_monete[5] = 5000;
        shop_quantita_monete[6] = 10;
        shop_quantita_monete[7] = 20;
        shop_quantita_monete[8] = 40;
        shop_quantita_monete[9] = 80;
        shop_quantita_monete[10] = 120;


        for (int n = 0; n < 6; n++) {
            crea_button_text(150 + n, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/frame_carta_upgrade_1");
            crea_grafica_text(150 + n, new Color(1, 1, 1, 0), "" + upgrade_titolo[n + 1], canvas, "Canvas", "");
            crea_grafica_text(160 + n, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/upgrade_carta " + (n + 1));
            crea_grafica_text(170 + n, new Color(1, 1, 1, 0), "" + script_struttura_dati.livello_upgrade[n + 1], canvas, "Canvas", "");


            grafica[150 + n].GetComponent<Image>().raycastTarget = false;
            grafica_testo[150 + n].GetComponent<TextMeshProUGUI>().raycastTarget = false;


            grafica[160 + n].GetComponent<Image>().raycastTarget = false;
            grafica_testo[160 + n].GetComponent<TextMeshProUGUI>().raycastTarget = false;

            grafica[170 + n].GetComponent<Image>().raycastTarget = false;
            grafica_testo[170 + n].GetComponent<TextMeshProUGUI>().raycastTarget = false;


        }


        // pagina centrale
        crea_grafica_text(3, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");
        crea_grafica_text(4, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/Frame_BarFrame_Top02_Navy");



        crea_grafica_text(11, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/StatusBarIcon_Gold");
        crea_grafica_text(12, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");
        crea_button_text(1, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Icon_PictoIcon_Setting");


        crea_button_text(0, "PLAY", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_Blue");
        crea_button_text(30, "BATTLEPASS", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/battlepass");
        crea_button_text(31, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/chest_main_page_1");
        pulsante[31].AddComponent<timer_reward>();
        crea_grafica_text(20, new Color(1, 1, 1, 0), "Level " + script_struttura_dati.livello_in_uso, canvas, "Canvas", "");
        crea_button_text(11, "", new Color(1, 1, 1, 1), canvas, "Canvas", "UI/grafica_UI/immagine_centrale_1"); //pulsante seleziona livelli

        crea_button_text(2, "SHOP", new Color(0, 0, 0, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_White");
        crea_button_text(3, "MAIN", new Color(0, 0, 0, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_White");
        crea_button_text(4, "UPGRADE", new Color(0, 0, 0, 1), canvas, "Canvas", "UI/grafica_UI/Btn_MainButton_White");
        crea_grafica_text(13, new Color(1, 1, 1, 1), "", canvas, "Canvas", "UI/grafica_UI/StatusBarIcon_Gem");
        crea_grafica_text(14, new Color(1, 1, 1, 0), "", canvas, "Canvas", "");




    }

    void crea_popup(int num = 1) {

        attivo_popup = num;

        distruggi_menu_popup();

        canvas_popup.SetActive(true);

        crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/sfondo_popUP"); //pannello shop
        crea_grafica_text(201, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", $"UI/grafica_UI/carta_shop {indice_shop_corrente}");  //testo/titolo oggetto shop
        crea_grafica_text(202, new Color(1, 1, 1, 0), $"{shop_quantita_monete[indice_shop_corrente]}", canvas_popup, "Canvas_popup/Panel", ""); //testo/titolo oggetto upgrade
        if (indice_shop_corrente == 1) {
            crea_button_text(200, "FREE", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White");  //tasto COMPRA SHOP
        }
        else if (indice_shop_corrente == 2) {
            crea_button_text(200, "AD", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White");  //tasto COMPRA SHOP
        }
        else {
            crea_button_text(200, "9.99€", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White");  //tasto COMPRA SHOP
        }

        crea_button_text(201, "", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ExitButton");

    }
    void crea_popup_seleziona_livelli(int num = 4) {

        attivo_popup = num;

        distruggi_menu_popup();

        canvas_popup.SetActive(true);

        crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/sfondo_popUP"); //pannello selezione livelli
        crea_grafica_text(201, new Color(1, 1, 1, 0), $"Chapter {indice_pagina_livello_corrente}", canvas_popup, "Canvas_popup/Panel", ""); //titolo capitolo selezionato

        for (int n = 0; n < 5; n++) {
            crea_grafica_text(202 + n, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", $"UI/grafica_UI/stella_{script_struttura_dati.stelle_livello[(indice_pagina_livello_corrente * 5 - 5) + n + 1]}");  //testo/titolo oggetto shop
        }

        crea_button_text(201, "", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ExitButton"); //exit button

        for (int n = 0; n < 5; n++) {
            crea_button_text(202 + n, $"Level {n + 1}", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White");
            //    pulsante[202 + n].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            //     pulsante_testo[202 + n].GetComponent<TextMeshProUGUI>().alignment = (TextAlignmentOptions)TextAlignment.Left;
        }

        crea_button_text(215, "", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/freccia_destra");//pulsante freccia cambio capitolo
        crea_button_text(216, "", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/freccia_sinistra"); //pulsante freccia cambio capitolo
        crea_button_text(217, "PLAY", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //pulsante gioca dentro il popup seleziona

        aggiorna_nome_livello();
    }

    void crea_popup_upgrade(int num = 2) {

        attivo_popup = num;

        distruggi_menu_popup();

        canvas_popup.SetActive(true);

        crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/frame_carta_upgrade_popUP_1"); //pannello upgrade
        crea_grafica_text(201, new Color(1, 1, 1, 0), "titolo", canvas_popup, "Canvas_popup/Panel", ""); //testo/titolo oggetto upgrade
        crea_grafica_text(202, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/upgrade_carta " + indice_upgrade_corrente); //immagine upgrade
        crea_grafica_text(203, new Color(1, 1, 1, 0), "prezzo", canvas_popup, "Canvas_popup/Panel", ""); //testo/prezzo oggetto upgrade
        crea_grafica_text(204, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/upgrade_popUP_barra " + script_struttura_dati.livello_upgrade[indice_upgrade_corrente]); //immagine valuta

        if (script_struttura_dati.livello_upgrade[indice_upgrade_corrente] == 10) {
            crea_button_text(200, "MAXED", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto UPGRADE
            pulsante[200].GetComponent<Button>().interactable = false;
        }
        else {
            crea_button_text(200, "UPGRADE", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto UPGRADE
        }
        crea_button_text(201, "", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ExitButton"); //Exitbutton

    }
    void crea_popup_opzioni(int num = 3) { //TODO FIX

        attivo_popup = num;

        distruggi_menu_popup();

        canvas_popup.SetActive(true);

        crea_grafica_text(200, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/sfondo_popUP"); //pannello OPZIONI
        crea_grafica_text(201, new Color(1, 1, 1, 0), "", canvas_popup, "Canvas_popup/Panel", ""); //testo/titolo musica
                                                                                                   //   crea_button_text(200, "ON", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto MUSICA
        crea_grafica_text(202, new Color(1, 1, 1, 0), "", canvas_popup, "Canvas_popup/Panel", ""); //testo/titolo sfx
                                                                                                   //   crea_button_text(201, "ON", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto SFX

        crea_grafica_text(203, new Color(1, 1, 1, 0), "", canvas_popup, "Canvas_popup/Panel", ""); //testo/titolo facebook
                                                                                                   // crea_button_text(202, "FacebookLogin", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto facebook


        crea_grafica_text(204, new Color(1, 1, 1, 0), "", canvas_popup, "Canvas_popup/Panel", ""); //testo/titolo credits
        crea_button_text(203, "Credits", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto credits

        crea_button_text(201, "", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/ExitButton"); //Exitbutton

        crea_button_text(205, "FacebookLogin", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto credits
        crea_button_text(206, "ON", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto new music
        cambio_colore_musiche_tasto();

        crea_button_text(207, "ON", new Color(0, 0, 0, 1), canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/Btn_MainButton_White"); //tasto new SFX

        cambio_colore_sfx_tasto();
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


        if (num == 0 || num == 217) {
            SceneManager.LoadScene("gioco");
        }

        if (num == 31) {

            script_struttura_dati.monete += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
            if (pulsante[31].GetComponent<timer_reward>() != null) {
                pulsante[31].GetComponent<timer_reward>().Click();
            }

        }


        if (num == 1) {
            crea_popup_opzioni(3);


        }

        if (num == 20) {
            crea_popup_opzioni(3);
        }

        if (num == 2) {
            pagina = -1;
        }

        if (num == 3) {
            pagina = 0;
        }

        if (num == 4) {
            pagina = 1;
        }

        //pulsante battlepass
        if (num == 30) {
            pagina = 2;
        }

        if (num == 11) {

            crea_popup_seleziona_livelli(4);
        }


        if (num == 100) {
            indice_shop_corrente = 1;
            crea_popup(1);
        }
        if (num == 101) {
            indice_shop_corrente = 2;
            crea_popup(1);
        }
        if (num == 102) {
            indice_shop_corrente = 3;
            crea_popup(1);
        }
        if (num == 103) {
            indice_shop_corrente = 4;
            crea_popup(1);
        }

        if (num == 104) {
            indice_shop_corrente = 5;
            crea_popup(1);
        }
        if (num == 105) {
            indice_shop_corrente = 6;
            crea_popup(1);
        }
        if (num == 106) {
            indice_shop_corrente = 7;
            crea_popup(1);
        }
        if (num == 107) {
            indice_shop_corrente = 8;
            crea_popup(1);
        }
        if (num == 108) {
            indice_shop_corrente = 9;
            crea_popup(1);
        }
        if (num == 109) {
            indice_shop_corrente = 10;
            crea_popup(1);
        }

        if (num == 150 || num == 160) {
            indice_upgrade_corrente = 1;
            crea_popup_upgrade(2);
        }
        if (num == 151 || num == 161) {
            indice_upgrade_corrente = 2;
            crea_popup_upgrade(2);
        }
        if (num == 152 || num == 162) {
            indice_upgrade_corrente = 3;
            crea_popup_upgrade(2);
        }

        if (num == 153 || num == 163) {
            indice_upgrade_corrente = 4;
            crea_popup_upgrade(2);
        }
        if (num == 154 || num == 164) {
            indice_upgrade_corrente = 5;
            crea_popup_upgrade(2);
        }

        if (num == 155 || num == 165) {
            indice_upgrade_corrente = 6;
            crea_popup_upgrade(2);
        }


        if (num == 200) {
            if (attivo_popup == 1) {
                acquista_shop();
            }
            else if (attivo_popup == 2) {
                if (script_struttura_dati.livello_upgrade[indice_upgrade_corrente] != 10) {
                    acquista_upgrade(script_struttura_dati.costo_livello[script_struttura_dati.livello_upgrade[indice_upgrade_corrente]]);
                }
            }

        }

        if (num == 201) {
            distruggi_menu_popup();
        }



        if (script_struttura_dati != null && attivo_popup == 3) {
            if (num == 206) {
                script_struttura_dati.disattiva_musica = 1 - script_struttura_dati.disattiva_musica;
                cambio_colore_musiche_tasto();
            }

            if (num == 207) {
                script_struttura_dati.disattiva_effetti = 1 - script_struttura_dati.disattiva_effetti;
                cambio_colore_sfx_tasto();
            }

        }





        for (int n = 0; n < 100; n++) {
            if (num == 300 + n) {
                riscatta_premio_battle_pass_free(n);
            }
        }

        for (int n = 0; n < 100; n++) {
            if (num == 400 + n) {
                riscatta_premio_battle_pass_premium(n);
            }
        }

        if (attivo_popup == 4) {
            for (int n = 0; n < 5; n++) {
                if (script_struttura_dati.livello_massimo_raggiunto >= (indice_pagina_livello_corrente * 5 - (5 - (n + 1)))) {
                    if (num == 202 + n) {
                        script_struttura_dati.livello_in_uso = indice_pagina_livello_corrente * 5 - (5 - (n + 1));
                        PlayerPrefs.SetInt("livello_in_uso", script_struttura_dati.livello_in_uso);
                        grafica_testo[20].GetComponent<TextMeshProUGUI>().text = "Level " + script_struttura_dati.livello_in_uso;
                        reset_colori_testo_selezionati();
                        pulsante[202 + n].GetComponent<Image>().color = Color.green;
                    }
                }
                else {
                    pulsante_testo[202 + n].GetComponent<TextMeshProUGUI>().color = Color.gray;
                    pulsante[202 + n].GetComponent<Button>().interactable = false;
                }


            }
        }

        if (num == 215) { //pulsante seleziona livello destra
            if (indice_pagina_livello_corrente != 40) {
                indice_pagina_livello_corrente++;
                aggiorna_grafica_testo_menu_selezionato();
                aggiorna_grafica_stelle();
                aggiorna_nome_livello();
            }
        }

        if (num == 216) {//pulsante seleziona livello sinistra
            if (indice_pagina_livello_corrente != 1) {
                indice_pagina_livello_corrente--;
                aggiorna_grafica_testo_menu_selezionato();
                aggiorna_grafica_stelle();
                aggiorna_nome_livello();
            }
        }


    }

    private void cambio_colore_sfx_tasto() {
        if (script_struttura_dati.disattiva_effetti == 1) {
            pulsante_testo[207].GetComponent<TextMeshProUGUI>().text = "OFF";
            pulsante[207].GetComponent<Image>().color = Color.red;
        }
        else if (script_struttura_dati.disattiva_effetti == 0) {
            pulsante_testo[207].GetComponent<TextMeshProUGUI>().text = "ON";
            pulsante[207].GetComponent<Image>().color = Color.green;
        }
    }

    private void cambio_colore_musiche_tasto() {
        if (script_struttura_dati.disattiva_musica == 1) {
            pulsante_testo[206].GetComponent<TextMeshProUGUI>().text = "OFF";
            pulsante[206].GetComponent<Image>().color = Color.red;
        }
        else if (script_struttura_dati.disattiva_musica == 0) {
            pulsante_testo[206].GetComponent<TextMeshProUGUI>().text = "ON";
            pulsante[206].GetComponent<Image>().color = Color.green;
        }
    }

    private void aggiorna_nome_livello() {

        for (int n = 0; n < 5; n++) {
            pulsante_testo[202 + n].GetComponent<TextMeshProUGUI>().text = "Level " + (indice_pagina_livello_corrente * 5 - (5 - (n + 1)));
        }
    }

    private void reset_colori_testo_selezionati() {
        for (int n = 0; n < 5; n++) {
            pulsante[202 + n].GetComponent<Image>().color = Color.white;
            pulsante_testo[202 + n].GetComponent<TextMeshProUGUI>().color = Color.black;
        }
    }
    private void aggiorna_grafica_testo_menu_selezionato() {
        for (int n = 0; n < 5; n++) {
            if (script_struttura_dati.livello_massimo_raggiunto >= (indice_pagina_livello_corrente * 5 - (5 - (n + 1)))) {

                pulsante_testo[202 + n].GetComponent<TextMeshProUGUI>().color = Color.black;
                pulsante[202 + n].GetComponent<Button>().interactable = true;
                pulsante[202 + n].GetComponent<Image>().color = Color.white;
            }
            else {
                pulsante[202 + n].GetComponent<Image>().color = Color.white;
                pulsante_testo[202 + n].GetComponent<TextMeshProUGUI>().color = Color.gray;
                pulsante[202 + n].GetComponent<Button>().interactable = false;

            }

        }
    }

    private void aggiorna_grafica_stelle() {
        float dx = risoluzione_x;
        float dy = risoluzione_y;

        float pos_x = 0;
        float pos_y = 0;
        float dx2 = dx * .8f;
        float dy2 = dy * .8f;
        float dime_panel_x = dx2;
        float dime_panel_y = dy2;



        for (int n = 0; n < 5; n++) {

            if (grafica[202 + n] != null) //stelle acquisite
{
                dx2 = dime_panel_x * 0.09f;
                dy2 = dime_panel_y * 0.09f;
                pos_x = dime_panel_x * -0.2f;
                pos_y = dime_panel_y * 0.35f + n * -38f;
                grafica[202 + n].GetComponent<RectTransform>().sizeDelta = new Vector2(dx2 * .97f, dy2);
                grafica[202 + n].GetComponent<RectTransform>().anchoredPosition = new Vector2(-pos_x, pos_y);
                grafica[202 + n].GetComponent<Image>().sprite = Resources.Load<Sprite>($"UI/grafica_UI/stella_{script_struttura_dati.stelle_livello[(indice_pagina_livello_corrente * 5 - 5) + n + 1]}");

            }

        }
    }

    private void riscatta_premio_battle_pass_free(int indice_reward) {
        char[] reward_string = script_struttura_dati.battle_pass_reward_free.ToCharArray();

        reward_string[indice_reward] = '2';

        script_struttura_dati.battle_pass_reward_free = new string(reward_string);
        PlayerPrefs.SetString("battle_pass_reward_free", script_struttura_dati.battle_pass_reward_free);
        script_struttura_dati.monete += 100;
        PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
    }

    private void riscatta_premio_battle_pass_premium(int indice_reward) {
        char[] reward_string = script_struttura_dati.battle_pass_reward_premium.ToCharArray();

        reward_string[indice_reward] = '2';

        script_struttura_dati.battle_pass_reward_premium = new string(reward_string);
        PlayerPrefs.SetString("battle_pass_reward_premium", script_struttura_dati.battle_pass_reward_premium);
        script_struttura_dati.gemme += 10;
        PlayerPrefs.SetInt("gemme", script_struttura_dati.gemme);

    }

    private void acquista_shop() {
        if (indice_shop_corrente == 1) {
            script_struttura_dati.monete += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
            if (pulsante[100].GetComponent<timer_reward>() != null) {
                pulsante[100].GetComponent<timer_reward>().Click();
            }
        }
        if (indice_shop_corrente == 2) {
            script_struttura_dati.monete += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
        }
        if (indice_shop_corrente == 3) {
            script_struttura_dati.monete += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
        }
        if (indice_shop_corrente == 4) {
            script_struttura_dati.monete += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
        }
        if (indice_shop_corrente == 5) {
            script_struttura_dati.monete += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
        }
        if (indice_shop_corrente == 6) {
            script_struttura_dati.gemme += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("gemme", script_struttura_dati.gemme);
        }
        if (indice_shop_corrente == 7) {
            script_struttura_dati.gemme += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("gemme", script_struttura_dati.gemme);
        }
        if (indice_shop_corrente == 8) {
            script_struttura_dati.gemme += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("gemme", script_struttura_dati.gemme);
        }
        if (indice_shop_corrente == 9) {
            script_struttura_dati.gemme += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("gemme", script_struttura_dati.gemme);
        }
        if (indice_shop_corrente == 10) {
            script_struttura_dati.gemme += shop_quantita_monete[indice_shop_corrente];
            PlayerPrefs.SetInt("gemme", script_struttura_dati.gemme);
        }
        distruggi_menu_popup();
    }

    private void acquista_upgrade(int costoUpgrade) {
        if (script_struttura_dati.monete >= costoUpgrade) {

            script_struttura_dati.monete = script_struttura_dati.monete - costoUpgrade;
            PlayerPrefs.SetInt("monete", script_struttura_dati.monete);
            script_struttura_dati.livello_upgrade[indice_upgrade_corrente] = script_struttura_dati.livello_upgrade[indice_upgrade_corrente] + 1;
            PlayerPrefs.SetInt($"LivelloUpgrade{indice_upgrade_corrente}", (script_struttura_dati.livello_upgrade[indice_upgrade_corrente]));

            if (grafica[204] != null) {
                Destroy(grafica[204]);
            }

            crea_grafica_text(204, new Color(1, 1, 1, 1), "", canvas_popup, "Canvas_popup/Panel", "UI/grafica_UI/upgrade_popUP_barra " + script_struttura_dati.livello_upgrade[indice_upgrade_corrente]); //immagine valuta

            grafica_testo[170 + indice_upgrade_corrente - 1].GetComponent<TextMeshProUGUI>().text = "" + script_struttura_dati.livello_upgrade[indice_upgrade_corrente];
            Debug.Log("Oggetto acquistato");
        }

    }

    void pressione_input_text(int num, InputField tog) {


        if (tog.text != null || tog.text != "" && tog.text != "-" && tog.text != ".") {

            Debug.Log("pressione!" + tog.text + "!!");

        }


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


}
