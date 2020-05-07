using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personajes
{
    public string nombre;
    public int nivel;
    public int estrellas;
    public int despertadas;
    public Atributos atributos;
    public Equipo[] equipamiento;
    public int experiencia;
    public Poderes[] poderes;
    public Poderes[] poderesActivos;
    public string elemento;
    public string estado_alterado;
    public string rareza;


    public Personajes Crear_personaje(string personaje)
    {
        Personajes nuevo_personaje;

        if (personaje == "roger")
        {
            nuevo_personaje = new roger();
            return nuevo_personaje;
        }
        else
        {
            return null;
        }
    }
}
