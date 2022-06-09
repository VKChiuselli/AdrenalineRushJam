

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class cilindro
{

    public float raggio=20;

}



[Serializable]
public class blocco
{
   
    public GameObject mesh;
    public GameObject mesh_dissolve;

    public Renderer mesh_renderer;

    public string struttura_procedurale;
    public string struttura_procedurale_dz;


    public int tipo;

    public int attivo;
    public int arrivo;
    public int disattiva_coll;
    public int distruzione_oggetto;

    public float valore_dissolve;

    public float altezza;
    public float rad;
    public float pos;

}


[Serializable]
public class bonus
{

    public GameObject mesh;


    public int tipo;

    public float altezza;
    public float rad;
    public float pos;

}


[Serializable]
public class malus
{

    public GameObject mesh;


    public int tipo;

    public float altezza;
    public float rad;
    public float pos;

    public int attivo;

    public int inversion_controller;

}

[Serializable]
public class gemma
{

    public GameObject mesh;


    public int tipo;

    public float altezza;
    public float rad;
    public float pos;

}

[Serializable]
public class moneta
{

    public GameObject mesh;


    public int tipo;

    public float altezza;
    public float rad;
    public float pos;

}