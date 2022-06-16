using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class struttura_dati : MonoBehaviour
{

    public int livello_in_uso;
    public int monete;
    public int gemme;
    public int[] upgrade= new int[10];
    public int stelle_battle_pass;
    public float caratteristiche_forza;
    public float caratteristiche_velocita;
    public float energia;
    

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        livello_in_uso = PlayerPrefs.GetInt("livello_in_uso");
        monete = PlayerPrefs.GetInt("monete");
        gemme = PlayerPrefs.GetInt("gemme");
        energia = PlayerPrefs.GetInt("energia");

        stelle_battle_pass = PlayerPrefs.GetInt("stelle_battle_pass");
        caratteristiche_forza = PlayerPrefs.GetFloat("caratteristiche_forza");
        caratteristiche_velocita = PlayerPrefs.GetFloat("caratteristiche_velocita");


        for (int i= 0; i < 9; i++) {
            upgrade[i] = PlayerPrefs.GetInt($"Upgrade{i}");
        }
        SceneManager.LoadScene("menu");

    }

   
}
