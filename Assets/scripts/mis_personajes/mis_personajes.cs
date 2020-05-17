using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mis_personajes : MonoBehaviour
{
    //TRAEMOS INSTANCIA DEL USUARIO, PREFAB DEL RECUADRO DE LOS PERSONAJES, OBJETO DEL MENU Y SCRIPT DE ROUTING
    public Usuario jugador;
    public GameObject prefab_recuadro_personaje;
    private routing _routing;
    public GameObject menu;

    //LISTA DE PERSONAJES A MOSTRAR Y SU CANTIDAD
    public List<Personajes> _personajes;
    private int cantidad_personajes;

    //ELEMENTOS TOGGLE DE LA UI
    public Toggle toggle_todos_personajes;
    public Toggle toggle_todos_elementos;
    public Toggle toggle_elemento_agua;
    public Toggle toggle_elemento_fuego;
    public Toggle toggle_elemento_tierra;
    public Toggle toggle_elemento_trueno;
    public Toggle toggle_elemento_luz;
    public Toggle toggle_elemento_oscuridad;


    // CREAMOS UNA VARIABLE CON EL SCRIPT ROUTING, ESTA ESTA DENTRO DEL GAMEOBJECT MENU
    void Awake()
    {
        _routing = menu.GetComponent<routing>();
    }

    void Start()
    {
        // INICIALIZAMOS EL USUARIO Y MIRAMOS CUANTOS PERSONAJES TIENE
        jugador = Usuario.instancia;
        _personajes = jugador.personajes;
        cantidad_personajes = _personajes.Count;

        //POPULAMOS LA UI CON LOS PREFABS
        Popular_lista_mis_personajes(cantidad_personajes, prefab_recuadro_personaje);

        //INICIALIZAMOS LOS TOGGLE
        toggle_todos_personajes = GameObject.Find("toggle_todos_personajes").GetComponent<Toggle>();
        toggle_todos_elementos = GameObject.Find("toggle_todos_elementos").GetComponent<Toggle>();
        toggle_elemento_agua = GameObject.Find("toggle_elemento_agua").GetComponent<Toggle>();
        toggle_elemento_fuego = GameObject.Find("toggle_elemento_fuego").GetComponent<Toggle>();
        toggle_elemento_tierra = GameObject.Find("toggle_elemento_tierra").GetComponent<Toggle>();
        toggle_elemento_trueno = GameObject.Find("toggle_elemento_trueno").GetComponent<Toggle>();
        toggle_elemento_luz = GameObject.Find("toggle_elemento_luz").GetComponent<Toggle>();
        toggle_elemento_oscuridad = GameObject.Find("toggle_elemento_oscuridad").GetComponent<Toggle>();
    }


    public void Popular_lista_mis_personajes(int numero_personajes, GameObject prefab)
    {
        float pos_inicial_x = -59.7F; // POSICION DEL CUADRADO 1 EN X
        float pos_inicial_y = 89F; // POSICION DEL CUADRADO 1 EN Y
        int filaActual = 0;
        for (int i = 0; i < numero_personajes; i++)
        {
            if (filaActual < 6) // CADA 6 FILAS BAJAMOS 1 COLUMNA
            {
                // NOS MOVEMOS 30F A LA DERECHA CADA PREFAB Y PONEMOS EL OBJETO COMO CHILD DEL CANVAS - CONTENIDO_SCROLL
                GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
                recuadro_personaje.transform.SetParent(GameObject.Find("contenido_scroll").transform, false);

                // AGREGAMOS UN ATRIBUTO DE BOTON AL PREFAB Y LE ENVIAMOS UN PARAMETRO CON EL NOMBRE DEL PERSONAJE DEL JUGADOR
                Personajes param = _personajes[i];
                Button btn = recuadro_personaje.GetComponent<Button>();
                btn.onClick.AddListener(delegate { btnClicked(param); });

                //ENTRAMOS EN EL CHILD #1 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
                GameObject texto_nivel = recuadro_personaje.transform.GetChild(2).gameObject;
                GameObject texto_nombre = recuadro_personaje.transform.GetChild(1).gameObject;
                GameObject img_perfil = recuadro_personaje.transform.GetChild(0).gameObject;
                img_perfil.GetComponent<Image>().sprite = Resources.Load <Sprite>("img_personajes/perfiles/" +  _personajes[i].foto_perfil);
                texto_nombre.GetComponent<Text>().text = _personajes[i].nombre;
                texto_nivel.GetComponent<Text>().text = _personajes[i].nivel.ToString();
                pos_inicial_x += 30F;
                filaActual++;


            }
            else // CADA 6 FILAS BAJAMOS 1 COLUMNA
            {
                // NOS MOVEMOS 87F HACIA ABAJO, RESETEAMOS X Y CADA PREFAB Y PONEMOS EL OBJETO COMO CHILD DEL CANVAS - CONTENIDO_SCROLL
                filaActual = 0;
                pos_inicial_x = -59.7F;
                pos_inicial_y -= 87F;
                GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
                recuadro_personaje.transform.SetParent(GameObject.Find("contenido_scroll").transform, false);

                // AGREGAMOS UN ATRIBUTO DE BOTON AL PREFAB Y LE ENVIAMOS UN PARAMETRO CON EL NOMBRE DEL PERSONAJE DEL JUGADOR
                Personajes param = _personajes[i];
                Button btn = recuadro_personaje.GetComponent<Button>();
                btn.onClick.AddListener(delegate { btnClicked(param); });

                //ENTRAMOS EN EL CHILD #1 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
                GameObject texto_nivel = recuadro_personaje.transform.GetChild(2).gameObject;
                GameObject texto_nombre = recuadro_personaje.transform.GetChild(1).gameObject;
                GameObject img_perfil = recuadro_personaje.transform.GetChild(0).gameObject;
                img_perfil.GetComponent<Image>().sprite = Resources.Load <Sprite>("img_personajes/perfiles/" +  _personajes[i].foto_perfil);
                texto_nombre.GetComponent<Text>().text = _personajes[i].nombre;
                texto_nivel.GetComponent<Text>().text = _personajes[i].nivel.ToString();
            }
        }
    }

    //FUNCION DEL BOTON, PARA IR A OTRA VENTANA Y ENVIAR UN PARAMETRO
    public void btnClicked(Personajes personaje)
    {
        _routing.ir_personaje_individual(personaje);
    }

    //FUNCION PARA CAMBIAR LA LISTA DE PERSONAJES SEGUN EL FILTRO
    public void Filtrar_personajes(string filtro)
    {
        Borrar_lista_personajes();
        Personajes pjs = new Personajes();

        switch(filtro)
        {
            case "todos_personajes":
                if(toggle_todos_personajes.isOn == true)
                {
                    _personajes = pjs.Crear_todos_personajes();
                }else{
                    _personajes = jugador.personajes;
                }
                break;
            case "toggle_todos_elementos":
                if(toggle_todos_elementos.isOn == true)
                {
                    toggle_elemento_agua.isOn = true;
                    toggle_elemento_fuego.isOn = true;
                    toggle_elemento_tierra.isOn = true;
                    toggle_elemento_trueno.isOn = true;
                    toggle_elemento_luz.isOn = true;
                    toggle_elemento_oscuridad.isOn = true;
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                }else{
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                    if(!toggle_elemento_agua.isOn){
                        _personajes = _personajes.FindAll(x => x.elemento != "agua");
                    }
                    if(!toggle_elemento_fuego.isOn){
                        _personajes = _personajes.FindAll(x => x.elemento != "fuego");
                    }
                    if(!toggle_elemento_tierra.isOn){
                        _personajes = _personajes.FindAll(x => x.elemento != "tierra");
                    }
                    if(!toggle_elemento_trueno.isOn){
                        _personajes = _personajes.FindAll(x => x.elemento != "trueno");
                    }
                    if(!toggle_elemento_luz.isOn){
                        _personajes = _personajes.FindAll(x => x.elemento != "luz");
                    }
                    if(!toggle_elemento_oscuridad.isOn){
                        _personajes = _personajes.FindAll(x => x.elemento != "oscuridad");
                    }
                }
                break;
            case "toggle_elemento_agua":
                if(toggle_elemento_agua.isOn == true)
                {
                    toggle_elemento_fuego.isOn = false;
                    toggle_elemento_tierra.isOn = false;
                    toggle_elemento_trueno.isOn = false;
                    toggle_elemento_luz.isOn = false;
                    toggle_elemento_oscuridad.isOn = false;
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                    _personajes = _personajes.FindAll(x => x.elemento == "agua");
                }else{
                    toggle_elemento_agua.isOn = false;
                    toggle_todos_elementos.isOn = false;
                    Filtrar_personajes("toggle_todos_elementos");
                }
                break;

             case "toggle_elemento_fuego":
                if(toggle_elemento_agua.isOn == true)
                {
                    toggle_elemento_agua.isOn = false;
                    toggle_elemento_tierra.isOn = false;
                    toggle_elemento_trueno.isOn = false;
                    toggle_elemento_luz.isOn = false;
                    toggle_elemento_oscuridad.isOn = false;
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                    _personajes = _personajes.FindAll(x => x.elemento == "fuego");
                }else{
                    toggle_elemento_fuego.isOn = false;
                    toggle_todos_elementos.isOn = false;
                    Filtrar_personajes("toggle_todos_elementos");
                }
                break;

            case "toggle_elemento_tierra":
                if(toggle_elemento_tierra.isOn == true)
                {
                    toggle_elemento_fuego.isOn = false;
                    toggle_elemento_agua.isOn = false;
                    toggle_elemento_trueno.isOn = false;
                    toggle_elemento_luz.isOn = false;
                    toggle_elemento_oscuridad.isOn = false;
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                    _personajes = _personajes.FindAll(x => x.elemento == "tierra");
                }else{
                    toggle_elemento_tierra.isOn = false;
                    toggle_todos_elementos.isOn = false;
                    Filtrar_personajes("toggle_todos_elementos");
                }
                break;

             case "toggle_elemento_trueno":
                if(toggle_elemento_trueno.isOn == true)
                {
                    toggle_elemento_fuego.isOn = false;
                    toggle_elemento_tierra.isOn = false;
                    toggle_elemento_agua.isOn = false;
                    toggle_elemento_luz.isOn = false;
                    toggle_elemento_oscuridad.isOn = false;
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                    _personajes = _personajes.FindAll(x => x.elemento == "trueno");
                }else{
                    toggle_elemento_trueno.isOn = false;
                    toggle_todos_elementos.isOn = false;
                    Filtrar_personajes("toggle_todos_elementos");
                }
                break;

             case "toggle_elemento_luz":
                if(toggle_elemento_luz.isOn == true)
                {
                    toggle_elemento_fuego.isOn = false;
                    toggle_elemento_tierra.isOn = false;
                    toggle_elemento_trueno.isOn = false;
                    toggle_elemento_agua.isOn = false;
                    toggle_elemento_oscuridad.isOn = false;
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                    _personajes = _personajes.FindAll(x => x.elemento == "luz");
                }else{
                    toggle_elemento_luz.isOn = false;
                    toggle_todos_elementos.isOn = false;
                    Filtrar_personajes("toggle_todos_elementos");
                }
                break;

             case "toggle_elemento_oscuridad":
                if(toggle_elemento_oscuridad.isOn == true)
                {
                    toggle_elemento_fuego.isOn = false;
                    toggle_elemento_tierra.isOn = false;
                    toggle_elemento_trueno.isOn = false;
                    toggle_elemento_luz.isOn = false;
                    toggle_elemento_agua.isOn = false;
                    _personajes = (toggle_todos_personajes.isOn == true) ? pjs.Crear_todos_personajes() : jugador.personajes;
                    _personajes = _personajes.FindAll(x => x.elemento == "oscuridad");
                }else{
                    toggle_elemento_oscuridad.isOn = false;
                    toggle_todos_elementos.isOn = false;
                    Filtrar_personajes("toggle_todos_elementos");
                }
                break;
        }
    
        cantidad_personajes = _personajes.Count;
        Popular_lista_mis_personajes(cantidad_personajes, prefab_recuadro_personaje);
    }

    public void Borrar_lista_personajes()
    {
        GameObject content_personajes = GameObject.Find("contenido_scroll");
        foreach (Transform child in content_personajes.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}
