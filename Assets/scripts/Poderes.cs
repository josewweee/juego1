using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poderes
{
    public string nombre;
    public string descripcion;
    public string tipo_daño;
    public float multiplicador_daño;
    public string tipo_poder;
    public int reutilizacion;
    public bool esta_en_reutilizacion;

    public Poderes(string nombre, string descripcion, string tipo_daño, float multiplicador_daño, string tipo_poder, int reutilizacion)
    {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.tipo_daño = tipo_daño;
        this.multiplicador_daño = multiplicador_daño;
        this.tipo_poder = tipo_poder;
        this.reutilizacion = reutilizacion;
        this.esta_en_reutilizacion = false;
    }
}
