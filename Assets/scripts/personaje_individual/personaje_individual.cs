using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class personaje_individual : MonoBehaviour
{
    // INSTANCIA DE PERSONA O EQUIPO A BUSCAR
    public storage_script equipo_personaje_objetivo;

    // TRAEMOS LOS VALORES DE LA UI
    public Text nombre;
    public Text nivel;

    public Text vitalidad;
    public Text fuerza;
    public Text magia;
    public Text velocidad;
    public Text critico;
    public Text def_fisica;
    public Text def_magica;

    //FABRICA DE PERSONAJES
    private Personajes fabrica_personajes;
    void Start()
    {
        //TRAEMOS EL PERSONAJE DEL SINGLETON DE PERSONAJES A BUSCAR Y LO CREAMOS CON LA FABRICA
        equipo_personaje_objetivo = storage_script.instancia;
        string nombre_personaje = equipo_personaje_objetivo.personaje;
        fabrica_personajes = new Personajes();
        Personajes personaje_actual = fabrica_personajes.Crear_personaje(nombre_personaje.ToLower());
        Debug.Log(personaje_actual.nombre);

        //ASIGNAMOS VALORES A LA UI
        //Personajes resultadoQuey = jugador.Personajes.Find(x => x.GetId() == personaje_actual.nombre);
        nombre.text = personaje_actual.nombre;
        nivel.text = personaje_actual.nivel.ToString();
        vitalidad.text = personaje_actual.atributos.vitalidad.ToString();
        fuerza.text = personaje_actual.atributos.fuerza.ToString();
        magia.text = personaje_actual.atributos.magia.ToString();
        velocidad.text = personaje_actual.atributos.velocidad.ToString();
        critico.text = personaje_actual.atributos.critico.ToString();
        def_fisica.text = personaje_actual.atributos.defensa_fisica.ToString();
        def_magica.text = personaje_actual.atributos.defensa_magica.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
