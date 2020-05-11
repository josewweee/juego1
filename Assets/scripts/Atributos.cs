using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atributos
{
    public float fuerza;
    public float vitalidad;
    public float magia;
    public float velocidad;
    public float critico;
    public float defensa_fisica;
    public float defensa_magica;
    public float salud;

    public Atributos(float fuerza, float vitalidad, float magia, float velocidad, float critico, float defensa_fisica, float defensa_magica)
    {
        this.fuerza = fuerza;
        this.vitalidad = vitalidad;
        this.magia = magia;
        this.velocidad = velocidad;
        this.critico = critico;
        this.defensa_fisica = defensa_fisica;
        this.defensa_magica = defensa_magica;
        this.salud = vitalidad;
    }

}
