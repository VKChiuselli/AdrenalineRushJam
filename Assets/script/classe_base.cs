

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class cilindro {

    public float raggio = 20;
    public float spostamento_z = 1;
    public float spostamento_z2 = .75f;

    public Vector3 pos_finale;
}



[Serializable]
public class blocco {

    public GameObject mesh;
    public GameObject mesh_dissolve;

    public Renderer mesh_renderer;

    public string struttura_procedurale;
    public string struttura_procedurale_dz;

    public Vector3 punto_impatto;

    public float forza_impatto;

    public int tipo;

    public int attivo;
    public int arrivo;
    public int disattiva_coll;
    public int distruzione_oggetto;

    public float valore_dissolve;

    public float altezza;
    public float rad;
    public float pos;
    public float rotazione;

}


[Serializable]
public class bonus {

    public GameObject mesh;


    public int tipo;

    public float altezza;
    public float rad;
    public float pos;

    public int attivo;

}


[Serializable]
public class malus {

    public GameObject mesh;


    public int tipo;

    public float altezza;
    public float rad;
    public float pos;

    public int attivo;

    public int inversion_controller;

}

[Serializable]
public class gemma {

    public GameObject mesh;

    public int presa;

    public int tipo;

    public float altezza;
    public float rad;
    public float pos;

}

[Serializable]
public class moneta {

    public GameObject mesh;


    public int tipo;

    public int presa;

    public float altezza;
    public float rad;
    public float pos;

}

[Serializable]
public class carte_shop {
    string path_sprite;
    string titolo;
    string descrizione;

    string costo;

    public int effetto;

}


[Serializable]
public class carte_upgrade {

    public string path_sprite;
    public string titolo;
    public string descrizione;
    public int Costo_Upgrade;


    public int effetto;

    public int livello;
    public int tipologia;

}