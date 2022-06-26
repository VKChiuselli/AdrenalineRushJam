using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class struttura_dati : MonoBehaviour {

    public int livello_in_uso;
    public int monete;
    public int gemme;
    public int[] livello_upgrade = new int[20];
    public int[] costo_livello = new int[50];
    public string[] stelle_livello = new string[400];
    public int stelle_battle_pass;
    public float caratteristiche_forza;
    public float caratteristiche_velocita;
    public float energia;
    public string battle_pass_reward_free;
    public string battle_pass_reward_premium;


    void Start() {
        DontDestroyOnLoad(this.gameObject);

        livello_in_uso = PlayerPrefs.GetInt("livello_in_uso");
        monete = PlayerPrefs.GetInt("monete");
        gemme = PlayerPrefs.GetInt("gemme");
        energia = PlayerPrefs.GetInt("energia");

        stelle_battle_pass = PlayerPrefs.GetInt("stelle_battle_pass");

        if (!PlayerPrefs.HasKey("battle_pass_reward_free")) {
            resetto_battle_pass("free");
        }
        if (!PlayerPrefs.HasKey("battle_pass_reward_premium")) {
            resetto_battle_pass("premium");
        }
        if (!PlayerPrefs.HasKey("stelle_livello")) {
            inizio_stelle_livello();
        }

        battle_pass_reward_free = PlayerPrefs.GetString("battle_pass_reward_free");
        battle_pass_reward_premium = PlayerPrefs.GetString("battle_pass_reward_premium");
        caratteristiche_forza = PlayerPrefs.GetFloat("caratteristiche_forza");
        caratteristiche_velocita = PlayerPrefs.GetFloat("caratteristiche_velocita");


        for (int i = 1; i < 7; i++) {
            livello_upgrade[i] = PlayerPrefs.GetInt($"LivelloUpgrade{i}");
        }

        for (int i = 1; i < 11; i++) {
            costo_livello[i] = PlayerPrefs.GetInt($"Livello{i}");
        }
        for (int i = 0; i < 400; i++) {
            stelle_livello[i] = PlayerPrefs.GetString($"stelle_livello{i}");
        }

        SceneManager.LoadScene("menu");

    }

    private void inizio_stelle_livello() {
        for (int i = 0; i < 400; i++) {
            PlayerPrefs.SetString($"stelle_livello{i}", new string('0', 3));
        }
    }

    private void resetto_battle_pass(string tipo_battle_pass) {
        PlayerPrefs.SetString($"battle_pass_reward_{tipo_battle_pass}", new string('0', 200));
    }
}


//IF (stella ottenuta <=   0: Non riscattabile 1: Riscattabile 2: Riscattato
//fare funzione allo start che legge le stelle  QUI, 