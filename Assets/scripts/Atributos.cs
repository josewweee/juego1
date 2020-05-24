using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
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
    public float[] atributos_base;

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
        this.atributos_base = new float[8]{this.fuerza, this.vitalidad, this.magia, this.velocidad, this.critico, this.defensa_fisica, this.defensa_magica, this.salud};
    }

    public void Resetear_atributos()
    {
        this.fuerza = this.atributos_base[0];
        this.vitalidad = this.atributos_base[1];
        this.magia = this.atributos_base[2];
        this.velocidad = this.atributos_base[3];
        this.critico = this.atributos_base[4];
        this.defensa_fisica = this.atributos_base[5];
        this.defensa_magica = this.atributos_base[6];
        this.salud = this.atributos_base[1];
    }



}
