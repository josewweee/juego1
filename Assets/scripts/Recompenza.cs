using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recompenza
{
    Monedas[] monedas;
    Equipo[] equipo;
    string tipo_combate;
    int nivel_historia;
    //VARIABLES CON DATOS LOCALES
    public Usuario jugador;
    public storage_script storage;

    public Recompenza()
    {
       jugador = Usuario.instancia;
       storage = storage_script.instancia;

       nivel_historia = storage.nivel_historia;
       tipo_combate = storage.tipo_combate;
    }



    public void Recompenzas_ganar()
    {   

        if(tipo_combate == "historia")
        {
            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 100
            var rand = new System.Random();
            int oro = rand.Next(0, 101);
            int diamantes = 0;

            //SI ESTAMOS EN UN NIVEL 10, 20, 30, ... TENDREMOS MAS PROBABILIDADES DE GANAR DIAMANTES, SI NO SOLO UN 25%
            int prob_diamantes = ( nivel_historia % 10 == 0)? rand.Next(0, 76): rand.Next(0, 101);
            if (prob_diamantes < 25)  diamantes = prob_diamantes;
            jugador.monedas.oro += oro;
            jugador.monedas.diamantes += diamantes;
            
            //LE DAMOS EXP A LOS PERSONAJES Y AL USUARIO
            int exp = (nivel_historia / 2) + 1;
            jugador.Sumar_experiencia(exp);
            
            Personajes[] pjs = jugador.personajesFavoritos;
            for(int i = 0; i < pjs.Length; i++)
            {
                if (pjs[i] != null)
                {
                    pjs[i].Ganar_exp(exp);
                }
                
            }            
        }
        else if (tipo_combate == "pvp")
        {

            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 100
            var rand = new System.Random();
            int puntos_pvp = rand.Next(0, 101);
            int diamantes = 0;

            //25% DE PROB DE GANAR DIAMANTES EN PVP
            int prob_diamantes = rand.Next(0, 76);
            if (prob_diamantes < 25)  diamantes = prob_diamantes;
            jugador.monedas.puntos_pvp += puntos_pvp;
            jugador.monedas.diamantes += diamantes;
        }
        
    }

    public void Recompenzas_perder()
    {
        if(tipo_combate == "historia")
        {
            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 50
            var rand = new System.Random();
            int oro = rand.Next(0, 51);
            int diamantes = 0;

            //SI ESTAMOS EN UN NIVEL 10, 20, 30, ... TENDREMOS MAS PROBABILIDADES DE GANAR DIAMANTES, SI NO SOLO UN 15%
            int prob_diamantes = ( nivel_historia % 10 == 0)? rand.Next(0, 76): rand.Next(0, 101);
            if (prob_diamantes < 15)  diamantes = prob_diamantes;
            jugador.monedas.oro += oro;
            jugador.monedas.diamantes += diamantes;
            
            //LE DAMOS EXP A LOS PERSONAJES Y AL USUARIO
            int exp = (nivel_historia / 3) + 1;
            jugador.Sumar_experiencia(exp);

            Personajes[] pjs = jugador.personajesFavoritos;
            for(int i = 0; i < pjs.Length; i++)
            {
                pjs[i].Ganar_exp(exp);
            }

        }
        else if (tipo_combate == "pvp")
        {

            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 30
            var rand = new System.Random();
            int puntos_pvp = rand.Next(0, 31);
            int diamantes = 0;

            //15% DE PROB DE GANAR DIAMANTES EN PVP
            int prob_diamantes = rand.Next(0, 76);
            if (prob_diamantes < 15)  diamantes = prob_diamantes;
            jugador.monedas.puntos_pvp += puntos_pvp;
            jugador.monedas.diamantes += diamantes;
        }
    }
}
