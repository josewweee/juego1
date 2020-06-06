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
    private routing _routing;
    public GameObject menu;

    // INSTANCIA SINGLETON PARA GUARDAR LOS ENEMIGOS
    public storage_script storage_enemigos;

    //FABRICA DE PERSONAJES
    private Personajes fabrica_personajes;

    // CREAMOS UNA VARIABLE CON EL SCRIPT ROUTING, ESTA ESTA DENTRO DEL GAMEOBJECT MENU
    void Awake()
    {
        _routing = menu.GetComponent<routing>();
    }

    void Start()
    {
        //SINGLETON DONDE GUARDAREMOS LOS ENEMIGOS
        storage_enemigos = storage_script.instancia;

        //FABRICA PARA CREAR PERSONAJES
        fabrica_personajes = new Personajes();

        //UI DE LOS NIVELES
        nivel_1 = GameObject.Find("nivel_1");
        nivel_2 = GameObject.Find("nivel_2");
        nivel_3 = GameObject.Find("nivel_3");
        nivel_4 = GameObject.Find("nivel_4");
        nivel_5 = GameObject.Find("nivel_5");
        nivel_6 = GameObject.Find("nivel_6");
        nivel_7 = GameObject.Find("nivel_7");
        nivel_8 = GameObject.Find("nivel_8");
        nivel_9 = GameObject.Find("nivel_9");
        nivel_10 = GameObject.Find("nivel_10");

        //ASIGNAMOS BOTONES PARA IR AL NIVEL A CADA UI DE NIVEL
        GameObject[] arr = { nivel_1, nivel_2, nivel_3, nivel_4, nivel_5, nivel_6, nivel_7, nivel_8, nivel_9, nivel_10 };
        Asignar_botones(arr, false);
    }

    public void siguienteMapa(){
        // TOMAMOS EL HIJO DEL NIVEL
        GameObject texto_nivel = nivel_1.transform.GetChild(0).gameObject;
        // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        int sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_2.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_3.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_4.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_5.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_6.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_7.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_8.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_9.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        // TOMAMOS EL HIJO DEL NIVEL
        texto_nivel = nivel_10.transform.GetChild(0).gameObject;
         // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
        sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) + 10;
        texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

        GameObject[] arr = { nivel_1, nivel_2, nivel_3, nivel_4, nivel_5, nivel_6, nivel_7, nivel_8, nivel_9, nivel_10 };
        Asignar_botones(arr, true);
        Asignar_botones(arr, false);

    }

    public void mapaAnterior(){
        // TOMAMOS EL HIJO DEL NIVEL Y REVISAMOS SI ES MAYOR QUE 0
        GameObject texto_nivel = nivel_1.transform.GetChild(0).gameObject;
        int sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;

        if (sigVal > 0){
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_2.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_3.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_4.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_5.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_6.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_7.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_8.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_9.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            // TOMAMOS EL HIJO DEL NIVEL
            texto_nivel = nivel_10.transform.GetChild(0).gameObject;
            // ASIGNAMOS UN NUEVO TEXTO AL NIVEL
            sigVal = int.Parse(texto_nivel.GetComponent<UnityEngine.UI.Text>().text) - 10;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = "" + sigVal;

            GameObject[] arr = { nivel_1, nivel_2, nivel_3, nivel_4, nivel_5, nivel_6, nivel_7, nivel_8, nivel_9, nivel_10 };
            Asignar_botones(arr, true);
            Asignar_botones(arr, false);
        }
    }

    void Asignar_botones(GameObject[] niveles, bool borrarListener){
        for (int i = 0; i < niveles.Length; i++){
            //TOMAMOS EL OBJECTO ACTUAL Y LE HALLAMOS EL HIJO #0 (TEXT) CON SU VALOR
            GameObject actual = niveles[i];
            //SI DEBEMOS AGREGAR BOTONES
            if (!borrarListener){
                GameObject child_nivel = actual.transform.GetChild(0).gameObject;
                string nivel = child_nivel.GetComponent<UnityEngine.UI.Text>().text;
                //LE AGREGAMOS UN BOTON QUE ENVIE EL NIVEL (VALOR TEXTO HIJO)
                Button btn = actual.GetComponent<Button>();
                btn.onClick.AddListener(delegate { Ir_nivel(nivel); });
            }else // SI DEBEMOS BORRAR BOTONES
            {
                //TOMAMOS EL BOTON Y LE BORRAMOS LOS LISTENERS
                Button btn = actual.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
            }
            
        }
    }

    public void Ir_nivel(string nivel){
        Crear_personajes_nivel(nivel);
         storage_enemigos.tipo_combate = "historia";
        _routing.ir_seleccion_pre_combate();
    }

    public void Crear_personajes_nivel(string nivel_){
        int nivel = int.Parse(nivel_);
        switch(nivel){
            case 1:
                Fabricar_personajes_con_nivel("roger", 0, 0);
                Fabricar_personajes_con_nivel("alicia", 0, 1);
                Fabricar_personajes_con_nivel("martis", 0, 2);
                Fabricar_personajes_con_nivel("liliana", 0, 3);
                storage_enemigos.Cambiar_nivel(1);
                break;
            case 2:
                Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(2);
                break;
            case 3:
                Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(3);
                break;
            case 4:
                  Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(4);
                break;
            case 5:
                 Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(5);
                break;
            case 6:
                 Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(6);
                break;
            case 7:
                  Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(7);
                break;
            case 8:
                  Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(8);
                break;
            case 9:
                  Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(9);
                break;
            case 10:
                  Fabricar_personajes_con_nivel("roger", nivel, 0);
                Fabricar_personajes_con_nivel("alicia", nivel, 1);
                Fabricar_personajes_con_nivel("martis", nivel, 2);
                Fabricar_personajes_con_nivel("liliana", nivel, 3);
                storage_enemigos.Cambiar_nivel(10);
                break;
            default:
                break;
        }
    }

    void Fabricar_personajes_con_nivel(string personaje, int nivel, int index){
        int i = index;
        Personajes enemigo = fabrica_personajes.Crear_personaje(personaje);
        enemigo.Subir_nivel(nivel);

        storage_enemigos.Agregar_enemigo(i, enemigo);
        i++;
    }
}
