using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Logros
{
    public int codigo_logro;
    public int progreso_actual;
    public int puntos;
    public bool reclamado;

    public Logros(int codigo_logro, int progreso_actual, int puntos, bool reclamado)
    {
        this.codigo_logro = codigo_logro;
        this.progreso_actual = progreso_actual;
        this.puntos = puntos;
        this.reclamado = reclamado;
    }
}
