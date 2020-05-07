using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class personaje_individual : MonoBehaviour
{
    // INSTANCIA DE PERSONA O EQUIPO A BUSCAR
    public singleton_equipo_personaje_individual equipo_personaje_objetivo;

    //FABRICA DE PERSONAJES
    private Personajes fabrica_personajes;
    void Start()
    {
        equipo_personaje_objetivo = singleton_equipo_personaje_individual.instancia;
        string nombre_personaje = equipo_personaje_objetivo.personaje;
        Debug.Log(nombre_personaje);
        fabrica_personajes = new Personajes();
        Personajes personaje_actual = fabrica_personajes.Crear_personaje(nombre_personaje.ToLower());
        Debug.Log(personaje_actual.nombre);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
