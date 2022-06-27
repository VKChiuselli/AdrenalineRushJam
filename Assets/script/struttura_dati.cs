using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class struttura_dati : MonoBehaviour {

    public int livello_in_uso;
    public int monete;
    public int gemme;
    public int livello_massimo_raggiunto;
    public int[] livello_upgrade = new int[20];
    public int[] costo_livello = new int[50];
    public string[] stelle_livello = new string[400];
    public int stelle_battle_pass;

    public float energia;
    public string battle_pass_reward_free;
    public string battle_pass_reward_premium;

    public int disattiva_musica = 0;
    public int disattiva_effetti = 0;

    void Start() {
        DontDestroyOnLoad(this.gameObject);

       if (!PlayerPrefs.HasKey("1_login")) {
            PlayerPrefs.SetInt("1_login", 1);
            primo_login();
        }

        livello_in_uso = PlayerPrefs.GetInt("livello_in_uso");
        monete = PlayerPrefs.GetInt("monete");
        gemme = PlayerPrefs.GetInt("gemme");
        energia = PlayerPrefs.GetInt("energia");

        stelle_battle_pass = PlayerPrefs.GetInt("stelle_battle_pass");
        livello_massimo_raggiunto = PlayerPrefs.GetInt("livello_massimo_raggiunto");
        battle_pass_reward_free = PlayerPrefs.GetString("battle_pass_reward_free");
        battle_pass_reward_premium = PlayerPrefs.GetString("battle_pass_reward_premium");


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

    private void primo_login() {

        PlayerPrefs.SetInt("livello_in_uso", 1);
        PlayerPrefs.SetInt("livello_massimo_raggiunto", 1);
        PlayerPrefs.SetInt("monete", 0);
        PlayerPrefs.SetInt("gemme", 0);
        PlayerPrefs.SetInt("energia", 100);
        PlayerPrefs.SetInt("stelle_battle_pass", 0);
        for (int i = 1; i < 7; i++) {
         PlayerPrefs.SetInt($"LivelloUpgrade{i}", 1);
        }
        resetto_battle_pass("free");
        resetto_battle_pass("premium");
        inizio_stelle_livello();
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