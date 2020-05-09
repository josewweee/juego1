using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poderes
{
    public string nombre;
    public string descripcion;
    public string atributo;
    public float multiplicador;
    public float multiplicador_efecto;
    public string tipo_poder; // ataque, buff, debuff, purgar, ataque debuff, ataque buff
    public string tipo_elemento;
    public int reutilizacion;
    public int duracion_efecto;
    public string objetivos; // multiple, propio, unico
    public bool se_puede_usar;
    public string[] habilidades;
    public float daño_base; // DAÑO BASE DEL PODER

    public Poderes(string nombre, string descripcion, string atributo, float multiplicador, float multiplicador_efecto, string tipo_poder, string tipo_elemento,int reutilizacion, int duracion_efecto, string objetivos, bool se_puede_usar, string[] habilidades, float daño_base)
    {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.atributo = atributo; // cambiar el nombre de tipo de daño a atributo
        this.multiplicador = multiplicador;
        this.multiplicador_efecto = multiplicador_efecto;
        this.tipo_poder = tipo_poder;
        this.tipo_elemento = tipo_elemento;
        this.reutilizacion = reutilizacion;
        this.duracion_efecto = duracion_efecto;
        this.objetivos = objetivos;
        this.se_puede_usar = se_puede_usar;
        this.habilidades = habilidades;
        this.daño_base = daño_base;
    }
}
