using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Update is called once per frame
    void Update()
    {

    }
}
