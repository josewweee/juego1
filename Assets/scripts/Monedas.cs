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

    public Monedas(int oro, int diamantes, int puntos_pvp){
        this.oro = oro;
        this.diamantes = diamantes;
        this.puntos_pvp = puntos_pvp;
    }
    
}
