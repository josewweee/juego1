using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Amigos
{
    public string nombre;
    public int nivel;
    public List<Personajes> personajesFavoritos;
    public List<Personajes> defensa_pvp;
    public List<Personajes> personajes;
    public bool regalo_enviado;
    public bool regalo_reclamado = false;

    public Amigos(string nombre, List<Personajes> fav = null, List<Personajes> def = null, List<Personajes> perso = null, bool regalo = false)
    {
        this.nombre = nombre;
        if (fav == null) personajesFavoritos = new List<Personajes>(){null, null, null, null};
        else this.personajesFavoritos = fav;
        if (def == null) defensa_pvp = new List<Personajes>(){null, null, null, null};
        else this.defensa_pvp = def;
        if (perso == null) personajes = new List<Personajes>(){null};
        else this.personajes = perso;
        this.regalo_enviado = regalo;
    }

    public void Set_nivel(int nivel)
    {
        this.nivel = nivel;
    }

    public void Set_favoritos(List<Personajes> favoritos)
    {
        this.personajesFavoritos = favoritos;
    }

    public void Set_defensa(List<Personajes> defensa)
    {
        this.defensa_pvp = defensa;
    }

     public void Set_personajes(List<Personajes> pjs)
    {
        this.personajes = pjs;
    }
}
