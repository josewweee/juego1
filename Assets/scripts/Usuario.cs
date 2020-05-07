﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usuario
{

    public string nombre = "nuevo usuario";
    public int nivel = 1;
    public int experiencia = 0;
    public string hermandad = "";
    public bool tutorial_completo = false;
    public List<Usuario> amigos = new List<Usuario>();
    public List<Personajes> personajes = new List<Personajes>();
    public Personajes[] personajesFavoritos = new Personajes[3];
    public Monedas monedas;
    public Configuracion configuracion = new Configuracion();
    public List<Logros> logros = new List<Logros>();
    public int puntos_logro = 0;
    public int energia = 30;
    public string metodo_login = "";
    public int energia_pvp = 0;
    public int puntos_pvp = 0;
    public Personajes[] defensa_pvp = new Personajes[3];
    public int posicion_pvp = 0;
    public int nivel_historia = 1;

    // CONSTRUCTOR
    private Usuario() { }
    // SINGLETON DEL USUARIO
    public static Usuario _instancia = null;
    public static Usuario instancia
    {
        get
        {
            if (_instancia == null)
                _instancia = new Usuario();
            return _instancia;
        }
    }


    // METODOS DE LA CLASE
    public bool Agregar_personaje(Personajes personaje)
    {
        personajes.Add(personaje);
        return true;
    }

    public void Sumar_experiencia(int cantidad)
    {
        experiencia += cantidad;
    }

    public void Sumar_monedas(string tipo_moneda, int cantidad)
    {
        switch (tipo_moneda)
        {
            case "oro":
                monedas.oro += cantidad;
                break;
            case "diamantes":
                monedas.diamantes += cantidad;
                break;
            case "puntos_pvp":
                monedas.puntos_pvp += cantidad;
                break;
            default:
                Debug.Log("no existe el tipo de moneda: " + tipo_moneda + ", revisa bien");
                break;
        }
    }

    public bool AgregarAmigos(Usuario usuario)
    {
        amigos.Add(usuario);
        return true;
    }

    public void Logro_completado(Logros logro)
    {
        logros.Add(logro);
        puntos_logro += logro.puntos;
    }

    public void Recargar_energia(string tipo_energia, int cantidad)
    {
        switch (tipo_energia)
        {
            case "energia":
                energia += cantidad;
                break;
            case "energia_pvp":
                energia_pvp += cantidad;
                break;
            default:
                Debug.Log("no existe el tipo de energia" + tipo_energia + ", revisa bien");
                break;
        }
    }

    public void Cambiar_personaje_batalla(string tipo_lista, int posicion_cambio, Personajes personaje)
    {
        switch (tipo_lista)
        {
            case "personajes_favoritos":
                try
                {
                    personajesFavoritos[posicion_cambio] = personaje;
                }
                catch
                {
                    Debug.Log("error ocurrido con el cambio de personaje");
                }
                break;
            case "defensa_pvp":
                try
                {
                    defensa_pvp[posicion_cambio] = personaje;
                }
                catch
                {
                    Debug.Log("error ocurrido con el cambio de personaje");
                }
                break;
            default:
                break;
        }
    }

    public void Cambiar_posicion_lista_pvp(int cantidad)
    {
        posicion_pvp = cantidad;
    }

    public void Cambiar_metodo_login(string metodo)
    {

    }
}
