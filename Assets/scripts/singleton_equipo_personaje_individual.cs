using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleton_equipo_personaje_individual
{
    public string personaje;
    public string equipo;
    private singleton_equipo_personaje_individual() { }
    // SINGLETON DE PERSONAJE O EQUIPO PARA REVISAR DE FORMA INDIVIDUAL
    public static singleton_equipo_personaje_individual _instancia = null;
    public static singleton_equipo_personaje_individual instancia
    {
        get
        {
            if (_instancia == null)
                _instancia = new singleton_equipo_personaje_individual();
            return _instancia;
        }
    }

}
