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
    public Dictionary<string, float[]> estado_alterado;
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

    public void Subir_nivel(int niveles){
        nivel = niveles;
        experiencia = 0;
        atributos.fuerza += niveles;
        atributos.vitalidad += niveles;
        atributos.magia += niveles;
        atributos.velocidad += niveles;
        atributos.critico += niveles;
        atributos.defensa_fisica += niveles;
        atributos.defensa_magica += niveles;

        // string output = JsonUtility.ToJson(atributos, true);
        // Debug.Log(output);
    }
}
