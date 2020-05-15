using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class personaje_individual : MonoBehaviour
{
    // INSTANCIA DE PERSONA O EQUIPO A BUSCAR Y DEL USUARIO DUEÑO
    public storage_script storage;
    public Usuario jugador;
    Personajes personaje_actual;

    // TRAEMOS LOS VALORES DE LA UI
    public Text nombre;
    public Text nivel;
    public Text nivel_max;
    public Text vitalidad;
    public Text fuerza;
    public Text magia;
    public Text velocidad;
    public Text critico;
    public Text def_fisica;
    public Text def_magica;
    public Text costo_fragmentos;
    public Text fragmentos_poseidos;

    public GameObject img_personaje;

    public GameObject poder_1;
    public GameObject poder_2;
    public GameObject poder_3;
    public GameObject poder_4;
    public GameObject poder_5;
    public GameObject poder_6;
    public GameObject poder_7;
    public GameObject poder_8;

    public Button btn_poder_1;
    public Button btn_poder_2;
    public Button btn_poder_3;
    public Button btn_poder_4;
    public Button btn_poder_5;
    public Button btn_poder_6;
    public Button btn_poder_7;
    public Button btn_poder_8;

    GameObject marco_poder_1;
    GameObject marco_poder_2;
    GameObject marco_poder_3;
    GameObject marco_poder_4;
    GameObject marco_poder_5;
    GameObject marco_poder_6;
    GameObject marco_poder_7;
    GameObject marco_poder_8;


    void Start()
    {


        //TRAEMOS EL PERSONAJE DEL SINGLETON DE PERSONAJES A BUSCAR Y EL JUGADOR
        storage = storage_script.instancia;
        jugador = Usuario.instancia;
        personaje_actual = storage.personaje;


        //ASIGNAMOS VALORES A LA UI
        nombre.text = personaje_actual.nombre;
        nivel.text = personaje_actual.nivel.ToString();
        nivel_max.text = " / " +  personaje_actual.nivel_maximo.ToString();
        vitalidad.text = personaje_actual.atributos.vitalidad.ToString();
        fuerza.text = personaje_actual.atributos.fuerza.ToString();
        magia.text = personaje_actual.atributos.magia.ToString();
        velocidad.text = personaje_actual.atributos.velocidad.ToString();
        critico.text = personaje_actual.atributos.critico.ToString();
        def_fisica.text = personaje_actual.atributos.defensa_fisica.ToString();
        def_magica.text = personaje_actual.atributos.defensa_magica.ToString();
        costo_fragmentos.text = "x " + ((personaje_actual.estrellas + 1) * 20).ToString();
        fragmentos_poseidos.text = personaje_actual.fragmentos.ToString();

        //ASIGNAMOS IMAGENES
        img_personaje = GameObject.Find("img_personaje");
        Sprite[] sprites = Resources.LoadAll<Sprite>("img_personajes/" + personaje_actual.imagen_completa[0]);
        int index_imagen = int.Parse(personaje_actual.imagen_completa[1]);
        img_personaje.GetComponent<Image>().sprite = sprites[index_imagen];


        //ASIGNAMOS LOS BOTONES DE LOS PODERES
        poder_1 = GameObject.Find("boton_poder 1");
        btn_poder_1 = poder_1.GetComponent<Button>();
        marco_poder_1 = GameObject.Find("marco_poder 1");
        poder_1.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[0].imagen);
        btn_poder_1.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[0].nombre;
        btn_poder_1.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[0], marco_poder_1) );

        poder_2 = GameObject.Find("boton_poder 2");
        btn_poder_2 = poder_2.GetComponent<Button>();
        marco_poder_2 = GameObject.Find("marco_poder 2");
        poder_2.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[1].imagen);
        btn_poder_2.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[1].nombre;
        btn_poder_2.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[1], marco_poder_2) );

        poder_3 = GameObject.Find("boton_poder 3");
        btn_poder_3 = poder_3.GetComponent<Button>();
        marco_poder_3 = GameObject.Find("marco_poder 3");
        poder_3.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[2].imagen);
        btn_poder_3.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[2].nombre;
        btn_poder_3.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[2], marco_poder_3) );

        poder_4 = GameObject.Find("boton_poder 4");
        btn_poder_4 = poder_4.GetComponent<Button>();
        marco_poder_4 = GameObject.Find("marco_poder 4");
        poder_4.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[3].imagen);
        btn_poder_4.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[3].nombre;
        btn_poder_4.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[3], marco_poder_4) );

        poder_5 = GameObject.Find("boton_poder 5");
        btn_poder_5 = poder_5.GetComponent<Button>();
        marco_poder_5 = GameObject.Find("marco_poder 5");
        poder_5.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[4].imagen);
        btn_poder_5.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[4].nombre;
        btn_poder_5.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[4], marco_poder_5) );

        poder_6 = GameObject.Find("boton_poder 6");
        btn_poder_6 = poder_6.GetComponent<Button>();
        marco_poder_6 = GameObject.Find("marco_poder 6");
        poder_6.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[5].imagen);
        btn_poder_6.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[5].nombre;
        btn_poder_6.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[5], marco_poder_6) );

        poder_7 = GameObject.Find("boton_poder 7");
        btn_poder_7 = poder_7.GetComponent<Button>();
        marco_poder_7 = GameObject.Find("marco_poder 7");
        poder_7.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[6].imagen);
        btn_poder_7.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[6].nombre;
        btn_poder_7.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[6], marco_poder_7) );

        poder_8 = GameObject.Find("boton_poder 8");
        btn_poder_8 = poder_8.GetComponent<Button>();
        marco_poder_8 = GameObject.Find("marco_poder 8");
        poder_8.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + personaje_actual.poderes[7].imagen);
        btn_poder_8.transform.GetChild(0).gameObject.GetComponent<Text>().text = personaje_actual.poderes[7].nombre;
        btn_poder_8.onClick.AddListener( () => Asignar_poder(personaje_actual.poderes[7], marco_poder_8) );


        Marcos_poderes_elegidos(personaje_actual);

    }



    public void Evolucionar()
    {
        //ENCONTRAMOS EL PERSONAJE EN EL USUARIO
        List<Personajes> personajes = jugador.personajes;
        for(int i = 0; i < personajes.Count; i++)
        {
            if(personajes[i].nombre ==  personaje_actual.nombre)
            {
                jugador.personajes[i].Evolucionar();

                //MODIFICAMOS LA UI PARA ACTUALIZAR LOS VALORES
                nivel_max.text = " / " +  jugador.personajes[i].nivel_maximo.ToString();
                costo_fragmentos.text = "x " + ((jugador.personajes[i].estrellas + 1) * 20).ToString();
                fragmentos_poseidos.text = jugador.personajes[i].fragmentos.ToString();
            }
        }
    }

    public void Asignar_poder(Poderes poder, GameObject marco_poder)
    {
        bool func = personaje_actual.Asignar_poder(poder);
        if (func)
        {
            Debug.Log("poder agregado");
            int index = 0;
            for(int i = 0; i < personaje_actual.poderesActivos.Length; i++){
                if(personaje_actual.poderesActivos[i] != null && personaje_actual.poderesActivos[i].nombre == poder.nombre){
                     index = i; 
                     break;
                }
            }
            marco_poder.SetActive(true);
            GameObject txt_pos_poder = marco_poder.transform.GetChild(0).gameObject;
            txt_pos_poder.GetComponent<Text>().text = (index+1).ToString();

        }else{
            Debug.Log("poder quitado");
            marco_poder.SetActive(false);
        }
    }

    void Marcos_poderes_elegidos(Personajes p_actual)
    {
        Poderes[] poderes_activos = p_actual.poderesActivos;
        Poderes[] lista_poderes = p_actual.poderes;

        for(int i = 0; i < lista_poderes.Length; i++)
        {
            bool poder_encontrado = false;
            for(int j = 0; j < poderes_activos.Length; j++)
            {
                if(poderes_activos[j] != null && poderes_activos[j].nombre == lista_poderes[i].nombre)
                {
                    int index = i + 1;
                    GameObject marco_poder = GameObject.Find("marco_poder " + index);
                    if(!marco_poder.activeSelf) marco_poder.SetActive(true);
                    GameObject txt_pos_poder = marco_poder.transform.GetChild(0).gameObject;
                    txt_pos_poder.GetComponent<UnityEngine.UI.Text>().text = (j+1).ToString();
                    poder_encontrado = true;
                    break;
                }
            }
            if (!poder_encontrado)
            {
                GameObject marco_poder = GameObject.Find("marco_poder " + (i+1));
                if(marco_poder.activeSelf) marco_poder.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
