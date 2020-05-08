using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storage_script
{
    public string personaje;
    public string equipo;
    public Personajes[] enemigos = {null, null, null, null};
    public string tipo_combate = "";


    private storage_script() {

     }


    // SINGLETON DE PERSONAJE O EQUIPO PARA REVISAR DE FORMA INDIVIDUAL
    public static storage_script _instancia = null;
    public static storage_script instancia
    {
        get
        {
            if (_instancia == null)
                _instancia = new storage_script();
            return _instancia;
        }
    }


    public void Agregar_enemigo(int index, Personajes personaje){
        if (index < 4 && index >= 0){
            enemigos[index] = personaje;
        }else{
            Debug.Log("Error, Estas intentando agregar un enemigo en la pos: " + index + " y se sale de los limites");
        }
    }
}
