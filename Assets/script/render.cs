using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class render : MonoBehaviour
{

    public string nome = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.Space))
        {

            ScreenCapture.CaptureScreenshot("C:/Users/Monzini/Documents/Jam_22_github_03/render"+ nome + ".png",4);


        }



    }
}
