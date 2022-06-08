using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class struttura_dati : MonoBehaviour
{

    public int livello_in_uso;
    public int monete;
    public float caratteristiche_forza;
    public float caratteristiche_velocita;


   
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        livello_in_uso = PlayerPrefs.GetInt("livello_in_uso");
        monete = PlayerPrefs.GetInt("monete");
        caratteristiche_forza = PlayerPrefs.GetFloat("caratteristiche_forza");
        caratteristiche_velocita = PlayerPrefs.GetFloat("caratteristiche_velocita");

       

        SceneManager.LoadScene("menu");

    }

   
}
