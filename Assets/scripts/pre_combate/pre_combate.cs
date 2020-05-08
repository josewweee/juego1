using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pre_combate : MonoBehaviour
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
        Popular_personajes_favoritos(jugador, prefab_recuadro_personaje);
    }

    public void Popular_lista_personajes(int numero_personajes, GameObject prefab){
        float pos_inicial_x = -85.4F; // POSICION DEL CUADRADO 1 EN X
        float pos_inicial_y = 70F; // POSICION DEL CUADRADO 1 EN Y
        for (int i = 0; i < numero_personajes; i++){
            // NOS MOVEMOS 17F A LA DERECHA CADA PREFAB Y PONEMOS EL OBJETO COMO CHILD DEL CANVAS - CONTENT_SCROLL
            GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
            recuadro_personaje.transform.SetParent(GameObject.Find("Content_scroll").transform, false);

            Button btn = recuadro_personaje.GetComponent<Button>();
            int index = i; // NECESARIO PARA EVITAR CLOUSURE CON LA i
            btn.onClick.AddListener(delegate { Agregar_personaje(recuadro_personaje, index); });

            //ENTRAMOS EN EL CHILD #0 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
            GameObject texto_nivel = recuadro_personaje.transform.GetChild(1).gameObject;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = this.jugador.personajes[i].nivel.ToString();
            pos_inicial_x += 17F;
        }
    }

    public void Popular_personajes_favoritos(Usuario jugador, GameObject prefab){
        Personajes[] personajes_favoritos = jugador.personajesFavoritos; //LISTA DE PERSONAJES FAVORITOS
        float pos_inicial_x = 0.00001525879F; // POSICION DEL CUADRADO 1 EN X
        float pos_inicial_y = 7.441475F; // POSICION DEL CUADRADO 1 EN Y
        if (personajes_favoritos[0] != null || personajes_favoritos[1] != null || personajes_favoritos[2] != null || personajes_favoritos[3] != null){
            for(int i = 0; i < personajes_favoritos.Length; i++){
                if (personajes_favoritos[i] != null){
                    //...
                GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
                recuadro_personaje.transform.SetParent(GameObject.Find("Panel_personaje_"+i).transform, false);
                recuadro_personaje.transform.localScale = new Vector3(4.386893F, 14.56672F, 5.55976F);

                // AGREGAMOS UN BOTON PARA ELIMINAR EL PERSONAJE DE FAVORITOS
                Button btn = recuadro_personaje.GetComponent<Button>();
                int index = i; // NECESARIO PARA EVITAR CLOUSURE
                btn.onClick.AddListener(delegate { Borrar_personaje(recuadro_personaje, index); });

                //ENTRAMOS EN EL CHILD #0 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
                GameObject texto_nivel = recuadro_personaje.transform.GetChild(1).gameObject;
                texto_nivel.GetComponent<UnityEngine.UI.Text>().text = jugador.personajes[i].nivel.ToString();
                }
            }
        }else{
            //...
            GameObject recuadro_personaje = Instantiate(prefab, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
            recuadro_personaje.transform.SetParent(GameObject.Find("Panel_personaje_0").transform, false);
            recuadro_personaje.transform.localScale = new Vector3(4.386893F, 14.56672F, 5.55976F);

            //AGREGAMOS EL PERSONAJE A FAVORITOS
            jugador.Cambiar_personaje_batalla("personajes_favoritos", 0, jugador.personajes[0]);

            // AGREGAMOS UN BOTON PARA ELIMINAR EL PERSONAJE DE FAVORITOS
            Button btn = recuadro_personaje.GetComponent<Button>();
            btn.onClick.AddListener(delegate { Borrar_personaje(recuadro_personaje, 0); });

            //ENTRAMOS EN EL CHILD #0 DEL PREFAB Y CAMBIAMOS EL VALOR DE SU TEXTO
            GameObject texto_nivel = recuadro_personaje.transform.GetChild(1).gameObject;
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = jugador.personajes[0].nivel.ToString();
        }
    }

    public void Borrar_personaje(GameObject personaje, int index)
    {
        //BORRAMOS EL PERSONAJE DE FAVORITOS TANTO EN LA UI, COMO DEL USUARIO
        Destroy(personaje);
        this.jugador.Cambiar_personaje_batalla("personajes_favoritos", index,  null);
    }

    void Agregar_personaje(GameObject personaje, int index){
        //MIRAMOS SI EN LA LISTA DE FAVORITOS DEL JUGADOR HAY UN ESPACIO LIBRE Y TOMAMOS ESE INDEX
        Personajes[] personajes_favoritos = this.jugador.personajesFavoritos;
        int i = 0;
        bool espacio_libre = true;
        while (personajes_favoritos[i] != null){
            i++;
            if (i >= 4 ){
                 espacio_libre = false;
                 break;
            }
        }

        //COLOCAMOS EN EL ESPACIO VACIO TANTO EN LA UI COMO EN LA LISTA DE FAVORITOS DEL JUGADOR, ESE PERSONAJE
        if (espacio_libre){
            //...
            float pos_inicial_x = 0.00001525879F;
            float pos_inicial_y = 7.441475F;
            GameObject recuadro_personaje = Instantiate(personaje, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
            recuadro_personaje.transform.SetParent(GameObject.Find("Panel_personaje_"+i).transform, false);
            recuadro_personaje.transform.localScale = new Vector3(4.386893F, 14.56672F, 5.55976F);

            // AGREGAMOS UN BOTON PARA ELIMINAR EL PERSONAJE DE FAVORITOS
            Button btn = recuadro_personaje.GetComponent<Button>();
            btn.onClick.AddListener(delegate { Borrar_personaje(recuadro_personaje, i); });
            this.jugador.Cambiar_personaje_batalla("personajes_favoritos", i, this.jugador.personajes[index]);
        }
    }

}
