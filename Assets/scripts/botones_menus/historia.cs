using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class historia : MonoBehaviour
{
    private GameObject nivel_1;
    private GameObject nivel_2;
    private GameObject nivel_3;
    private GameObject nivel_4;
    private GameObject nivel_5;
    private GameObject nivel_6;
    private GameObject nivel_7;
    private GameObject nivel_8;
    private GameObject nivel_9;
    private GameObject nivel_10;
    void Start()
    {
        nivel_1 = GameObject.Find("texto_nivel_1");
        nivel_2 = GameObject.Find("texto_nivel_2");
        nivel_3 = GameObject.Find("texto_nivel_3");
        nivel_4 = GameObject.Find("texto_nivel_4");
        nivel_5 = GameObject.Find("texto_nivel_5");
        nivel_6 = GameObject.Find("texto_nivel_6");
        nivel_7 = GameObject.Find("texto_nivel_7");
        nivel_8 = GameObject.Find("texto_nivel_8");
        nivel_9 = GameObject.Find("texto_nivel_9");
        nivel_10 = GameObject.Find("texto_nivel_10");
    }

    public void siguienteMapa(){
        int sigVal = int.Parse(nivel_1.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_1.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_2.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_2.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_3.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_3.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_4.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_4.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_5.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_5.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_6.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_6.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_7.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_7.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_8.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_8.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_9.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_9.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_10.GetComponent<UnityEngine.UI.Text>().text) + 10;
        nivel_10.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

    }

    public void mapaAnterior(){
        int sigVal = int.Parse(nivel_1.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_1.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_2.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_2.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_3.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_3.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_4.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_4.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_5.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_5.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_6.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_6.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_7.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_7.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_8.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_8.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_9.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_9.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        sigVal = int.Parse(nivel_10.GetComponent<UnityEngine.UI.Text>().text) - 10;
        nivel_10.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;
    }
}
