using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Monedas
{
    public int oro;
    public int diamantes;
    public int puntos_pvp;
    public int monedas_amigos;
    public int invocaciones_normales;
    public int invocaciones_raras;
    public int invocaciones_miticas;
    public int invocaciones_legendarias;

    public Monedas(int oro, int diamantes, int puntos_pvp, int monedas_amigos, int iNormales, int iRaras, int iMiticas, int iLegendarias){
        this.oro = oro;
        this.diamantes = diamantes;
        this.puntos_pvp = puntos_pvp;
        this.monedas_amigos = monedas_amigos;
        this.invocaciones_normales = iNormales;
        this.invocaciones_raras = iRaras;
        this.invocaciones_miticas = iMiticas;
        this.invocaciones_legendarias = iLegendarias;
    }
    
}
