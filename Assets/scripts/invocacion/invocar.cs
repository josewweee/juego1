using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invocar : MonoBehaviour
{
    // INSTANCIAS DEL PERSONAJE Y DE LA FABRICA DE PERSONAJES
    public Usuario jugador;
    public Personajes fabrica_personajes;
    void Start()
    {
        fabrica_personajes = new Personajes();
        jugador = Usuario.instancia;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void invocacion_normal()
    {
        Personajes personaje_invocado = fabrica_personajes.Crear_personaje("roger");
        jugador.Agregar_personaje(personaje_invocado);
        string output = JsonUtility.ToJson(jugador.personajes[0], true);
        Debug.Log(output);
    }
}
