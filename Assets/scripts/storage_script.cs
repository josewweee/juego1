using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storage_script
{
    public Personajes personaje = null;
    public Usuario jugador_enemigo = null;
    public Equipo equipo;
    public Personajes[] enemigos = {null, null, null, null};
    public List<Personajes> enemigos_pvp;
    public string tipo_combate = "";
    public int nivel_historia = 1;


    private storage_script() {
        enemigos_pvp = new List<Personajes>{null, null, null, null};
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

    public void Cambiar_nivel(int nivel)
    {   
        this.nivel_historia = nivel;
    }
}
