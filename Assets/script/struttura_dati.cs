using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class struttura_dati : MonoBehaviour
{

    public int livello_in_uso;
    public int monete;
    public int gemme;
    public int[] livello_upgrade= new int[10];
    public int[] costo_livello= new int[50];
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


        for (int i= 1; i < 7; i++) {
            livello_upgrade[i] = PlayerPrefs.GetInt($"LivelloUpgrade{i}");
        } 

        for (int i= 1; i <11; i++) {
            costo_livello[i] = PlayerPrefs.GetInt($"Livello{i}");
        } 
     
        SceneManager.LoadScene("menu");

    }

   
}
