using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giocatore : MonoBehaviour
{

    GameObject protagonista;

    public float sposta_camera_finale = 2;
    public float sposta_camera_iniziale = 2;

    public float sposta_camera_verticale = 1;
    public float sposta_visione_cam = 1.0f;

    float sposta_camera = 0;// sposta_camera_iniziale;


    struttura_dati script_struttura_dati;

    GameObject canvas;


    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 120.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;



    GameObject cam0;

    GameObject ogg_struttura_dati;

    float altezza_blocco_calcolo = 0;

    int blocco_numero = -1;

    int sconfitta = 0;

    float[] altezza_blocco = new float[50];


    // Start is called before the first frame update
    void Start()
    {

        cam0 = GameObject.Find("Main Camera");
        characterController = GetComponent<CharacterController>();

        sposta_camera = sposta_camera_iniziale;

        crea_livello();
       // carica_livello();

        canvas = GameObject.Find("Canvas");

        ogg_struttura_dati = GameObject.Find("base_struttura");

        if (ogg_struttura_dati != null)
        {
            script_struttura_dati = ogg_struttura_dati.GetComponent<struttura_dati>();

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (sconfitta == 0) {
            controllo_personaggio();
        }

    }


    void controllo_personaggio() { 

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(-Input.GetAxis("Horizontal"), 0.0f, 1);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        
        moveDirection.y -= gravity * Time.deltaTime;

       
        characterController.Move(moveDirection * Time.deltaTime);

        controllo_camera();


        controllo_caduta();


    }


    void controllo_caduta()
    {



        Ray ray = new Ray(characterController.transform.position, new Vector3(0, -300, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5000))
        {


        }
        else
        {
         //   sconfitta = 1;

        }


    }




    void controllo_camera()
    {

        Vector3 pos = characterController.transform.position;

        float rad = characterController.transform.localEulerAngles.y;

        sposta_camera = Mathf.Lerp(sposta_camera, sposta_camera_finale, Time.deltaTime*.2f);

        float xx = pos.x - Mathf.Sin(rad) * sposta_camera;
        float zz = pos.z- Mathf.Cos(rad) * sposta_camera;

        float yy = pos.y + sposta_camera_verticale;


        cam0.transform.position = new Vector3(xx, yy, zz);

        Vector3 looat = new Vector3(pos.x,pos.y+ sposta_visione_cam, pos.z);

        cam0.transform.LookAt(looat);



    }


    void crea_livello()
    {

        altezza_blocco[5] = -1;
        altezza_blocco[6] = -2;

        altezza_blocco[7] = 1;
        altezza_blocco[8] = 2;

        altezza_blocco[10] = 2;


        for (int n = 0; n < 8; n++)
        {
            crea_blocco(0);
        }

            for (int n = 0; n < 50; n++)
        {

            int rnd_int = (int)(UnityEngine.Random.Range(0, 6.99f));

            crea_blocco(rnd_int);

        }



    }


    void crea_blocco(int tipo)
    {
        blocco_numero = blocco_numero + 1;

        GameObject blocco = Instantiate(Resources.Load("grafica_3d/Prefabs/blocco_" + tipo )) as GameObject;

        blocco.name = "blocco"+ blocco_numero+" tipo "+tipo;


        blocco.transform.position = new Vector3(0, altezza_blocco_calcolo, blocco_numero * 6);


        altezza_blocco_calcolo = altezza_blocco_calcolo + altezza_blocco[tipo];


        float rnd = UnityEngine.Random.Range(.0f, 10);

       
        if (blocco_numero >2 && rnd>0)
        {

            if (tipo == 0)
            {

                StartCoroutine(crea_bonus_gemma(blocco.transform.position));

            }

            if (tipo == 3)
            {

                StartCoroutine(crea_bonus_moneta(blocco.transform.position, tipo));

            }

            if (tipo == 5 || tipo == 6 || tipo == 7 || tipo == 8 || tipo == 10)
            {

             StartCoroutine( crea_bonus_moneta(blocco.transform.position, tipo)) ;

            }


        }




    }


    IEnumerator crea_bonus_gemma(Vector3 pos)
    {

        yield return .1f;

        float xx = UnityEngine.Random.Range(-2.5f,2.5f);


        int num = (int)(UnityEngine.Random.Range(1, 3.99f));


        float moltiplicatore = 4.0f / num;

        for (int n = 0; n <= num; n++)
        {


            GameObject bonus_gemma = Instantiate(Resources.Load("grafica_3d/Prefabs/gemma")) as GameObject;

            bonus_gemma.name = "bonus_gemma";

            float zz = .5f + n * moltiplicatore;


            Vector3 pos2 = new Vector3(xx, 0, zz) + pos;


            bonus_gemma.transform.position = new Vector3(pos2.x, calcolo_altezza(pos2),pos2.z);


        }




    }






    IEnumerator crea_bonus_moneta(Vector3 pos, int tipo_blocco)
    {
        yield return .1f;

        float direzione = UnityEngine.Random.Range(-2.5f, 2.5f);

        float xx = 2.5f*Mathf.Sign(direzione);

        if (tipo_blocco == 5 || tipo_blocco == 6 || tipo_blocco == 7 || tipo_blocco == 8 || tipo_blocco == 10)
        {

            xx = UnityEngine.Random.Range(-2.5f, 2.5f);
        }
      


        int num = (int)(UnityEngine.Random.Range(1, 3.99f));


        float moltiplicatore = 4.0f / num;




        for (int n = 0; n <= num; n++)
        {


            GameObject bonus_moneta = Instantiate(Resources.Load("grafica_3d/Prefabs/moneta")) as GameObject;

            bonus_moneta.name = "bonus_moneta";

            float zz = .5f + n * moltiplicatore;




            Vector3 pos2 = new Vector3(xx, 0, zz) + pos;

          //  crea_sfera_test(pos2);

            bonus_moneta.transform.position = new Vector3(pos2.x, calcolo_altezza(pos2), pos2.z);


        }


    

    }



    void crea_sfera_test(Vector3 pos)
    {
        GameObject sfera = Instantiate(Resources.Load("grafica_3d/Prefabs/Sphere")) as GameObject;

        sfera.transform.position = pos;

        float dx=.2f;

        sfera.transform.localScale = new Vector3(dx,dx,dx);
    }


    float calcolo_altezza(Vector3 pos)
    {

        float altezza=0;

        Ray ray = new Ray(new Vector3(pos.x,pos.y+2,pos.z), new Vector3(0, -300, 0));
        RaycastHit hit;

     //   Debug.DrawLine(new Vector3(pos.x, pos.y + 10, pos.z), new Vector3(pos.x, -300, pos.z),new Color(1,0,0,1),25);


        if (Physics.Raycast(ray, out hit, 5000))
        {
            altezza = hit.point.y;

            Debug.Log("altezza");

        }


        return altezza;

    }



    void OnCollisionEnter(Collision collision)
    {


        Debug.Log("collision "+collision.collider.name);
       

    }


    private void OnTriggerEnter(Collider other)
    {
        //  Debug.Log(""+other.name);


        if (other.name == "moneta")
        {
            script_struttura_dati.monete = script_struttura_dati.monete + 1;

            Debug.Log(" script_struttura_dati.monete " + script_struttura_dati.monete);
        }


        Destroy(other.gameObject);
       


    }


}
