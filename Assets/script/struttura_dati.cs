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
    public int astronave_skin;

    public int[] astronave_skin_comprate = new int[20];

    public float energia;
    public string battle_pass_reward_free;
    public string battle_pass_reward_premium;

    public int disattiva_musica = 0;
    public int disattiva_effetti = 0;

    void Start() {
        DontDestroyOnLoad(this.gameObject);

       if (!PlayerPrefs.HasKey("first_login_ever")) {
            PlayerPrefs.SetInt("first_login_ever", 1);
            primo_login();
        }

        livello_in_uso = PlayerPrefs.GetInt("livello_in_uso");
        monete = PlayerPrefs.GetInt("monete");
        gemme = PlayerPrefs.GetInt("gemme");
        energia = PlayerPrefs.GetInt("energia");
        astronave_skin = PlayerPrefs.GetInt("astronave_skin");



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
        for (int i = 0; i < 10; i++)
        {
            astronave_skin_comprate[i] = PlayerPrefs.GetInt($"astronave_skin_comprate{i}");
        }
        astronave_skin_comprate[0] = 1;
        

        SceneManager.LoadScene("menu");

    }

    public void primo_login() {
        inizializzazione_dati();
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

    private static void inizializzazione_dati() {

        PlayerPrefs.SetInt("Livello1", 100);
        PlayerPrefs.SetInt("Livello2", 200);
        PlayerPrefs.SetInt("Livello3", 400);
        PlayerPrefs.SetInt("Livello4", 800);
        PlayerPrefs.SetInt("Livello5", 1600);
        PlayerPrefs.SetInt("Livello6", 3200);
        PlayerPrefs.SetInt("Livello7", 6400);
        PlayerPrefs.SetInt("Livello8", 10000);
        PlayerPrefs.SetInt("Livello9", 11000);
        PlayerPrefs.SetInt("Livello10", 12000);

        for (int i = 1; i < 7; i++) {
            PlayerPrefs.SetInt($"LivelloUpgrade{i}", 1);
            PlayerPrefs.SetString($"path_sprite{i}", $"UI/grafica_UI/upgrade_carta {i}");
            PlayerPrefs.SetInt($"Costo_Upgrade{i}", PlayerPrefs.GetInt($"Livello{PlayerPrefs.GetInt($"LivelloUpgrade{i}")}"));
        }

        PlayerPrefs.SetString("UpgradeTitolo1", "agilita");
        PlayerPrefs.SetString("UpgradeTitolo2", "barriera");
        PlayerPrefs.SetString("UpgradeTitolo3", "energia");
        PlayerPrefs.SetString("UpgradeTitolo4", "spari");
        PlayerPrefs.SetString("UpgradeTitolo5", "scafo");
        PlayerPrefs.SetString("UpgradeTitolo6", "calamita");

        PlayerPrefs.SetString("UpgradeDescrizione1", "Descrizione!agilita");
        PlayerPrefs.SetString("UpgradeDescrizione2", "Descrizione!barriera");
        PlayerPrefs.SetString("UpgradeDescrizione3", "Descrizione!energia");
        PlayerPrefs.SetString("UpgradeDescrizione4", "Descrizione!spari");
        PlayerPrefs.SetString("UpgradeDescrizione5", "Descrizione!scafo");
        PlayerPrefs.SetString("UpgradeDescrizione6", "Descrizione!calamita");

        //ammontare di quanto cambia l'effetto del potere, ognuno avrà una sua singolarità
        PlayerPrefs.SetInt("UpgradeEffetto1", 1);
        PlayerPrefs.SetInt("UpgradeEffetto2", 1);
        PlayerPrefs.SetInt("UpgradeEffetto3", 1);
        PlayerPrefs.SetInt("UpgradeEffetto4", 1);
        PlayerPrefs.SetInt("UpgradeEffetto5", 1);
        PlayerPrefs.SetInt("UpgradeEffetto6", 1);

        //TBD
        PlayerPrefs.SetInt("UpgradeTipologia1", 1);
        PlayerPrefs.SetInt("UpgradeTipologia2", 1);
        PlayerPrefs.SetInt("UpgradeTipologia3", 1);
        PlayerPrefs.SetInt("UpgradeTipologia4", 1);
        PlayerPrefs.SetInt("UpgradeTipologia5", 1);
        PlayerPrefs.SetInt("UpgradeTipologia6", 1);

    }
}


//IF (stella ottenuta <=   0: Non riscattabile 1: Riscattabile 2: Riscattato
//fare funzione allo start che legge le stelle  QUI, 