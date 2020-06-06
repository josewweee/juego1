using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class equipamiento_individual : MonoBehaviour
{
     // INSTANCIA DE PERSONA O EQUIPO A BUSCAR Y DEL USUARIO DUEÑO
    public storage_script storage;
    public Usuario jugador;
    Equipo equipo_actual;

    // TRAEMOS LOS VALORES DE LA UI
    public Text nombre;
    public Text nivel;
    public Text nivel_max;
    public Text exp;
    public Text exp_max;
    public Text vitalidad;
    public Text fuerza;
    public Text magia;
    public Text velocidad;
    public Text critico;
    public Text def_fisica;
    public Text def_magica;
    public Text costo_fragmentos;
    public Text fragmentos_poseidos;

    private GameObject img_equipo;

    private GameObject poder_1;
    private GameObject poder_2;


    private Button btn_poder_1;
    private Button btn_poder_2;

    GameObject marco_poder_1;
    GameObject marco_poder_2;



    void Start()
    {


        //TRAEMOS EL PERSONAJE DEL SINGLETON DE PERSONAJES A BUSCAR Y EL JUGADOR
        storage = storage_script.instancia;
        jugador = Usuario.instancia;
        equipo_actual = storage.equipo;


        //ASIGNAMOS VALORES A LA UI
        nombre.text = equipo_actual.nombre;
        nivel.text = equipo_actual.nivel.ToString();
        nivel_max.text = " / " +  equipo_actual.nivel_maximo.ToString();
        exp.text = equipo_actual.experiencia.ToString();
        exp_max.text = "/ " +  Math.Pow(equipo_actual.nivel, 2).ToString();
        vitalidad.text = equipo_actual.atributos.vitalidad.ToString();
        fuerza.text = equipo_actual.atributos.fuerza.ToString();
        magia.text = equipo_actual.atributos.magia.ToString();
        velocidad.text = equipo_actual.atributos.velocidad.ToString();
        critico.text = equipo_actual.atributos.critico.ToString();
        def_fisica.text = equipo_actual.atributos.defensa_fisica.ToString();
        def_magica.text = equipo_actual.atributos.defensa_magica.ToString();
        costo_fragmentos.text = "x " + ((equipo_actual.estrellas + 1) * 20).ToString();
        fragmentos_poseidos.text = equipo_actual.fragmentos.ToString();

        //ASIGNAMOS IMAGENES
        img_equipo = GameObject.Find("img_equipo");
        // Sprite[] sprites = Resources.LoadAll<Sprite>("img_equipos/" + equipo_actual.imagen_completa[0]);
        // int index_imagen = int.Parse(equipo_actual.imagen_completa[1]);
        img_equipo.GetComponent<Image>().sprite = Resources.Load<Sprite>("img_equipos/img_completa" + equipo_actual.imagen_completa);


        //ASIGNAMOS LOS BOTONES DE LOS PODERES
        poder_1 = GameObject.Find("boton_poder 1");
        btn_poder_1 = poder_1.GetComponent<Button>();
        marco_poder_1 = GameObject.Find("marco_poder 1");
        poder_1.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + equipo_actual.poderes[0].imagen);
        btn_poder_1.transform.GetChild(0).gameObject.GetComponent<Text>().text = equipo_actual.poderes[0].nombre;
        btn_poder_1.onClick.AddListener( () => Asignar_poder(equipo_actual.poderes[0], marco_poder_1) );

        poder_2 = GameObject.Find("boton_poder 2");
        btn_poder_2 = poder_2.GetComponent<Button>();
        marco_poder_2 = GameObject.Find("marco_poder 2");
        poder_2.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + equipo_actual.poderes[1].imagen);
        btn_poder_2.transform.GetChild(0).gameObject.GetComponent<Text>().text = equipo_actual.poderes[1].nombre;
        btn_poder_2.onClick.AddListener( () => Asignar_poder(equipo_actual.poderes[1], marco_poder_2) );

        Marcos_poderes_elegidos(equipo_actual);

    }



    public void Evolucionar()
    {
        //ENCONTRAMOS EL PERSONAJE EN EL USUARIO
        List<Equipo> equipo = jugador.equipo;
        for(int i = 0; i < equipo.Count; i++)
        {
            if(equipo[i].nombre ==  equipo_actual.nombre)
            {
                jugador.equipo[i].Evolucionar();

                //MODIFICAMOS LA UI PARA ACTUALIZAR LOS VALORES
                nivel_max.text = " / " +  jugador.equipo[i].nivel_maximo.ToString();
                costo_fragmentos.text = "x " + ((jugador.equipo[i].estrellas + 1) * 20).ToString();
                fragmentos_poseidos.text = jugador.equipo[i].fragmentos.ToString();
            }
        }
    }

    public void Asignar_poder(Poderes poder, GameObject marco_poder)
    {
        bool func = equipo_actual.Asignar_poder(poder);
        if (func)
        {
            Debug.Log("poder agregado");
            int index = 0;
            if(equipo_actual.poderActivo != null && equipo_actual.poderActivo.nombre == poder.nombre){
            }
            marco_poder.SetActive(true);
            GameObject txt_pos_poder = marco_poder.transform.GetChild(0).gameObject;
            txt_pos_poder.GetComponent<Text>().text = "0";

        }else{
            Debug.Log("poder quitado");
            marco_poder.SetActive(false);
        }
    }

    void Marcos_poderes_elegidos(Equipo p_actual)
    {
        Poderes poder_activo = p_actual.poderActivo;
        Poderes[] lista_poderes = p_actual.poderes;

        for(int i = 0; i < lista_poderes.Length; i++)
        {
            bool poder_encontrado = false;
            if(poder_activo != null && poder_activo.nombre == lista_poderes[i].nombre)
            {
                int index = i + 1;
                GameObject marco_poder = GameObject.Find("marco_poder " + index);
                if(!marco_poder.activeSelf) marco_poder.SetActive(true);
                GameObject txt_pos_poder = marco_poder.transform.GetChild(0).gameObject;
                txt_pos_poder.GetComponent<Text>().text = "0";
                poder_encontrado = true;
                break;
            }
            if (!poder_encontrado)
            {
                GameObject marco_poder = GameObject.Find("marco_poder " + (i+1));
                if(marco_poder.activeSelf) marco_poder.SetActive(false);
            }
        }
    }
}
