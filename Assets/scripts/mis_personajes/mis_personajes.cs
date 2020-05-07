using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mis_personajes : MonoBehaviour
{
    //TRAEMOS INSTANCIA DEL USUARIO, PREFAB DEL RECUADRO DE LOS PERSONAJES, OBJETO DEL MENU Y SCRIPT DE ROUTING
    public Usuario jugador;
    public GameObject prefab_recuadro_personaje;
    private int cantidad_personajes;
    private routing _routing;
    public GameObject menu;

    // CREAMOS UNA VARIABLE CON EL SCRIPT ROUTING, ESTA ESTA DENTRO DEL GAMEOBJECT MENU
    void Awake()
    {
        _routing = menu.GetComponent<routing>();
    }

    void Start()
    {
        // INICIALIZAMOS EL USUARIO, MIRAMOS CUANTOS PERSONAJES TIENE Y POPULAMOS LA UI CON LOS PREFABS
        jugador = Usuario.instancia;
        cantidad_personajes = jugador.personajes.Count;
        Popular_lista_personajes(cantidad_personajes, prefab_recuadro_personaje);
    }


    public void Popular_lista_personajes(int numero_personajes, GameObject prefab)
    {
        float pos_inicial_x = -73F; // POSICION DEL CUADRADO 1 EN X
        float pos_inicial_y = 106.63F; // POSICION DEL CUADRADO 1 EN Y
        int filaActual = 0;
        for (int i = 0; i < numero_personajes; i++)
        {
            if (filaActual < 6) // CADA 6 FILAS BAJAMOS 1 COLUMNA
            {
                // NOS MOVEMOS 30F A LA DERECHA CADA PREFAB Y PONEMOS EL OBJETO COMO CHILD DEL CANVAS - CONTENIDO_SCROLL
                GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
                recuadro_personaje.transform.SetParent(GameObject.Find("contenido_scroll").transform, false);

                // AGREGAMOS UN ATRIBUTO DE BOTON AL PREFAB Y LE ENVIAMOS UN PARAMETRO CON EL NOMBRE DEL PERSONAJE DEL JUGADOR
                string param = jugador.personajes[i].nombre;
                Button btn = recuadro_personaje.GetComponent<Button>();
                btn.onClick.AddListener(delegate { btnClicked(param); });

                //ENTRAMOS EN EL CHILD #1 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
                GameObject texto_nivel = recuadro_personaje.transform.GetChild(1).gameObject;
                texto_nivel.GetComponent<UnityEngine.UI.Text>().text = jugador.personajes[i].nivel.ToString();
                pos_inicial_x += 30F;
                filaActual++;


            }
            else // CADA 6 FILAS BAJAMOS 1 COLUMNA
            {
                // NOS MOVEMOS 87F HACIA ABAJO, RESETEAMOS X Y CADA PREFAB Y PONEMOS EL OBJETO COMO CHILD DEL CANVAS - CONTENIDO_SCROLL
                filaActual = 0;
                pos_inicial_y -= 87F;
                pos_inicial_x = -73F;
                GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
                recuadro_personaje.transform.SetParent(GameObject.Find("contenido_scroll").transform, false);

                // AGREGAMOS UN ATRIBUTO DE BOTON AL PREFAB Y LE ENVIAMOS UN PARAMETRO CON EL NOMBRE DEL PERSONAJE DEL JUGADOR
                string param = jugador.personajes[i].nombre;
                Button btn = recuadro_personaje.GetComponent<Button>();
                btn.onClick.AddListener(delegate { btnClicked(param); });

                //ENTRAMOS EN EL CHILD #1 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
                GameObject texto_nivel = recuadro_personaje.transform.GetChild(1).gameObject;
                texto_nivel.GetComponent<UnityEngine.UI.Text>().text = jugador.personajes[i].nivel.ToString();
            }
        }
    }

    //FUNCION DEL BOTON, PARA IR A OTRA VENTANA Y ENVIAR UN PARAMETRO
    public void btnClicked(string nombre_personaje)
    {
        Debug.Log(nombre_personaje);
        _routing.ir_personaje_individual(nombre_personaje);
    }
}
