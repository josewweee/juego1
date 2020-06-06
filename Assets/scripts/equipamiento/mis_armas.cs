using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mis_armas : MonoBehaviour
{
     //TRAEMOS INSTANCIA DEL USUARIO, PREFAB DEL RECUADRO DE LOS PERSONAJES, OBJETO DEL MENU Y SCRIPT DE ROUTING
    public Usuario jugador;
    public GameObject prefab_recuadro_personaje;
    private routing _routing;
    public GameObject menu;

    //LISTA DE PERSONAJES A MOSTRAR Y SU CANTIDAD
    public List<Equipo> _equipo;
    private int cantidad_armas;

    //ELEMENTOS TOGGLE DE LA UI
    private Toggle toggle_todas_armas;

    // CREAMOS UNA VARIABLE CON EL SCRIPT ROUTING, ESTA ESTA DENTRO DEL GAMEOBJECT MENU
    void Awake()
    {
        _routing = menu.GetComponent<routing>();
    }

    void Start()
    {
        // INICIALIZAMOS EL USUARIO Y MIRAMOS CUANTOS PERSONAJES TIENE
        jugador = Usuario.instancia;
        _equipo = jugador.equipo;
        cantidad_armas = _equipo.Count;

        //POPULAMOS LA UI CON LOS PREFABS
        Popular_lista_mis_armas(cantidad_armas, prefab_recuadro_personaje);

        //INICIALIZAMOS LOS TOGGLE
        toggle_todas_armas = GameObject.Find("toggle_todas_armas").GetComponent<Toggle>();
    
    }


    public void Popular_lista_mis_armas(int numero_armas, GameObject prefab)
    {
        float pos_inicial_x = -59.7F; // POSICION DEL CUADRADO 1 EN X
        float pos_inicial_y = 89F; // POSICION DEL CUADRADO 1 EN Y
        int filaActual = 0;
        for (int i = 0; i < numero_armas; i++)
        {
            if (filaActual < 6) // CADA 6 FILAS BAJAMOS 1 COLUMNA
            {
                // NOS MOVEMOS 30F A LA DERECHA CADA PREFAB Y PONEMOS EL OBJETO COMO CHILD DEL CANVAS - CONTENIDO_SCROLL
                GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
                recuadro_personaje.transform.SetParent(GameObject.Find("contenido_scroll").transform, false);

                // AGREGAMOS UN ATRIBUTO DE BOTON AL PREFAB Y LE ENVIAMOS UN PARAMETRO CON EL NOMBRE DEL PERSONAJE DEL JUGADOR
                Equipo param = _equipo[i];
                Button btn = recuadro_personaje.GetComponent<Button>();
                btn.onClick.AddListener(delegate { btnClicked(param); });

                //ENTRAMOS EN EL CHILD #1 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
                GameObject texto_nivel = recuadro_personaje.transform.GetChild(2).gameObject;
                GameObject texto_nombre = recuadro_personaje.transform.GetChild(1).gameObject;
                GameObject img_perfil = recuadro_personaje.transform.GetChild(0).gameObject;
                img_perfil.GetComponent<Image>().sprite = Resources.Load <Sprite>("img_armas/perfiles/" +  _equipo[i].foto_perfil);
                texto_nombre.GetComponent<Text>().text = _equipo[i].nombre;
                texto_nivel.GetComponent<Text>().text = _equipo[i].nivel.ToString();
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
                Equipo param = _equipo[i];
                Button btn = recuadro_personaje.GetComponent<Button>();
                btn.onClick.AddListener(delegate { btnClicked(param); });

                //ENTRAMOS EN EL CHILD #1 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
                GameObject texto_nivel = recuadro_personaje.transform.GetChild(2).gameObject;
                GameObject texto_nombre = recuadro_personaje.transform.GetChild(1).gameObject;
                GameObject img_perfil = recuadro_personaje.transform.GetChild(0).gameObject;
                img_perfil.GetComponent<Image>().sprite = Resources.Load <Sprite>("img_armas/perfiles/" +  _equipo[i].foto_perfil);
                texto_nombre.GetComponent<Text>().text = _equipo[i].nombre;
                texto_nivel.GetComponent<Text>().text = _equipo[i].nivel.ToString();
            }
        }
    }

    //FUNCION DEL BOTON, PARA IR A OTRA VENTANA Y ENVIAR UN PARAMETRO
    public void btnClicked(Equipo equipo)
    {
        _routing.ir_equipamiento_individual(equipo);
    }
}
