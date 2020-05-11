using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personajes
{
    public string nombre;
    public int nivel;
    public int estrellas;
    public int despertadas;
    public Atributos atributos; // FUERZA, VITALIDAD, MAGIA, VELOCIDAD, CRITICO, DEF_FISICA, DEF_MAGICA
    public Equipo[] equipamiento;
    public int experiencia;
    public Poderes[] poderes; // 9
    public Poderes[] poderesActivos; // 4
    public string elemento; // AGUA, FUEGO, TIERRA, TRUENO, LUZ, OSCURIDAD
    public Dictionary<string, float[]> estado_alterado; // NOMBRE EFECTO -> [ DAÑO EFECTO, DURACION EFECTO ]
    public string rareza; // COMUN, RARO, MITICO, LEGENDARIO


    public Personajes Crear_personaje(string personaje)
    {
        Personajes nuevo_personaje;
        switch(personaje){
            case "roger":
                nuevo_personaje = new roger();
                return nuevo_personaje;
            case "alicia":
                nuevo_personaje = new alicia();
                return nuevo_personaje;
            case "liliana":
                nuevo_personaje = new liliana();
                return nuevo_personaje;
            case "martis":
                nuevo_personaje = new martis();
                return nuevo_personaje;
            default:
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
