using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class mecanicas_logros 
{

    private Personajes[] tempAliados;
    private Personajes[] tempEnemigos;
    private Usuario jugador;
    public mecanicas_logros( Usuario jugador, Personajes[] aliados = null, Personajes[] enemigos = null)
    {
        this.tempAliados = aliados;
        this.tempEnemigos = enemigos;
        this.jugador = jugador;
    }

    public void RevisarLogrosCombate(Personajes[] aliados, Personajes[] enemigos)
    {
        //REVISAMOS SI TODOS ESTAN CONGELADOS Y ANTES NO LO ESTABAN
        if(enemigos.Where(c => c.estado_alterado.ContainsKey("congelar")).ToArray().Length > 3)
        {
            if(tempEnemigos.Where(c => c.estado_alterado.ContainsKey("congelar")).ToArray().Length < 1)
            {
                jugador.AumentarLogro(24,1);
            }
        }

        //REVISAMOS SI TODOS ESTAN ATURDIDOS Y ANTES NO LO ESTABAN
        if(enemigos.Where(c => c.estado_alterado.ContainsKey("aturdir")).ToArray().Length > 3)
        {
            if(tempEnemigos.Where(c => c.estado_alterado.ContainsKey("aturdir")).ToArray().Length < 1)
            {
                jugador.AumentarLogro(27,1);
            }
        }

        //REVISAMOS SI ELIMINAMOS A UN ENEMIGO DE VIT MAX A MUERTO
        for(int i = 0; i < enemigos.Length; i++)
        {
            if(enemigos[i].estado_alterado.ContainsKey("muerto") && tempEnemigos[i].atributos.salud == tempEnemigos[i].atributos.vitalidad)
            {
                jugador.AumentarLogro(29,1);
                break;
            }
        }

        //REVISAMOS SI TODOS ESTAN MUERTOS Y ANTES ESTABAN FULL VIDA
        if(enemigos.Where(c => c.estado_alterado.ContainsKey("muerto")).ToArray().Length > 3)
        {
            if(tempEnemigos.Where(c => c.atributos.salud == c.atributos.vitalidad).ToArray().Length > 3)
            {
                jugador.AumentarLogro(30,1);
            }
        }

        //ACTUALIZAMOS EL ESTADO DEL COMBATE
        this.tempAliados = aliados;
        this.tempEnemigos = enemigos;
    }

    public void RevisarLogrosFinCombate(Personajes[] aliados = null, Personajes[] enemigos = null, int nivelHistoria = 0, string tipoCombate = "")
    {
        if(enemigos != null)
        {
            //REVISAMOS SI ESTAN TODOS QUEMADOS
            if(enemigos.Where(x => x.estado_alterado.ContainsKey("quemar") || x.estado_alterado.ContainsKey("quemar_grave")).ToArray().Length > 3)
            {
                jugador.AumentarLogro(25,1);
            }

            //REVISAMOS SI ESTAN TODOS CONGELADOS
            if(enemigos.Where(x => x.estado_alterado.ContainsKey("congelar")).ToArray().Length > 3)
            {
                jugador.AumentarLogro(26,1);
            }
        }

        if(aliados != null)
        {
            //REVISAMOS SI SOLO QUEDO 1 SUPERVIVIENTE ALIADO
            if((aliados.Where(x => x.estado_alterado.ContainsKey("muerto")).ToArray().Length > 2) && aliados.Length > 3)
            {
                jugador.AumentarLogro(28,1);
            }
        }

        if(nivelHistoria != 0)
        {
            //REVISAMOS SI AVANZAMOS 1 NIVEL
            if(jugador.logros.ContainsKey(2) && !jugador.logros[2].reclamado && nivelHistoria > jugador.nivel_historia)
            {
                jugador.AumentarLogro(2,1);
            }

            //REVISAMOS NIVEL 10 COMPLETADO
            if(nivelHistoria == 10 && (!jugador.logros.ContainsKey(6) || !jugador.logros[6].reclamado))
            {
                jugador.AumentarLogro(6,1);
            }

            //REVISAMOS NIVEL 20 COMPLETADO
            if((nivelHistoria == 20) && (jugador.logros.ContainsKey(6) && jugador.logros[6].reclamado) && (!jugador.logros.ContainsKey(7) || !jugador.logros[7].reclamado))
            {
                jugador.AumentarLogro(7,1);
            }

            //REVISAMOS NIVEL 30 COMPLETADO
            if((nivelHistoria == 30) && (jugador.logros.ContainsKey(7) && jugador.logros[7].reclamado) && (!jugador.logros.ContainsKey(8) || !jugador.logros[8].reclamado))
            {
                jugador.AumentarLogro(8,1);
            }

            //REVISAMOS NIVEL 40 COMPLETADO
            if((nivelHistoria == 40) && (jugador.logros.ContainsKey(8) && jugador.logros[8].reclamado) && (!jugador.logros.ContainsKey(9) || !jugador.logros[9].reclamado))
            {
                jugador.AumentarLogro(9,1);
            }

            //REVISAMOS NIVEL 50 COMPLETADO
            if((nivelHistoria == 50) && (jugador.logros.ContainsKey(9) && jugador.logros[9].reclamado) && (!jugador.logros.ContainsKey(10) || !jugador.logros[10].reclamado))
            {
                jugador.AumentarLogro(10,1);
            }

            //REVISAMOS NIVEL 60 COMPLETADO
            if((nivelHistoria == 60) && (jugador.logros.ContainsKey(10) && jugador.logros[10].reclamado) && (!jugador.logros.ContainsKey(11) || !jugador.logros[11].reclamado))
            {
                jugador.AumentarLogro(11,1);
            }
        }

        if(nivelHistoria != 0 && aliados != null)
        {
            //REVISAMOS NIVEL 10 COMPLETADO SIN MUERTES
            if(nivelHistoria == 10 && (!jugador.logros.ContainsKey(18) || !jugador.logros[18].reclamado))
            {
                if(aliados.Where(x => x.estado_alterado.ContainsKey("muerto")).ToArray().Length < 1)
                {
                    jugador.AumentarLogro(18,1);
                }
            }

            //REVISAMOS NIVEL 20 COMPLETADO SIN MUERTES
            if((nivelHistoria == 20) && (jugador.logros.ContainsKey(18) && jugador.logros[18].reclamado) && (!jugador.logros.ContainsKey(19) || !jugador.logros[19].reclamado))
            {
            if(aliados.Where(x => x.estado_alterado.ContainsKey("muerto")).ToArray().Length < 1)
                {
                    jugador.AumentarLogro(19,1);
                }
            }

            //REVISAMOS NIVEL 30 COMPLETADO SIN MUERTES
            if((nivelHistoria == 30) && (jugador.logros.ContainsKey(19) && jugador.logros[19].reclamado) && (!jugador.logros.ContainsKey(20) || !jugador.logros[20].reclamado))
            {
                if(aliados.Where(x => x.estado_alterado.ContainsKey("muerto")).ToArray().Length < 1)
                {
                    jugador.AumentarLogro(20,1);
                }
            }

            //REVISAMOS NIVEL 40 COMPLETADO SIN MUERTES
            if((nivelHistoria == 40) && (jugador.logros.ContainsKey(20) && jugador.logros[20].reclamado) && (!jugador.logros.ContainsKey(21) || !jugador.logros[21].reclamado))
            {
                if(aliados.Where(x => x.estado_alterado.ContainsKey("muerto")).ToArray().Length < 1)
                {
                    jugador.AumentarLogro(21,1);
                }
            }

            //REVISAMOS NIVEL 50 COMPLETADO SIN MUERTES
            if((nivelHistoria == 50) && (jugador.logros.ContainsKey(21) && jugador.logros[21].reclamado) && (!jugador.logros.ContainsKey(22) || !jugador.logros[22].reclamado))
            {
                if(aliados.Where(x => x.estado_alterado.ContainsKey("muerto")).ToArray().Length < 1)
                {
                    jugador.AumentarLogro(22,1);
                }
            }

            //REVISAMOS NIVEL 60 COMPLETADO SIN MUERTES
            if((nivelHistoria == 60) && (jugador.logros.ContainsKey(22) && jugador.logros[22].reclamado) && (!jugador.logros.ContainsKey(23) || !jugador.logros[23].reclamado))
            {
                if(aliados.Where(x => x.estado_alterado.ContainsKey("muerto")).ToArray().Length < 1)
                {
                    jugador.AumentarLogro(23,1);
                }
            }

            //REVISAMOS NIVEL 10 COMPLETADO CON 1 PERSONAJE
            if(nivelHistoria == 10 && (!jugador.logros.ContainsKey(12) || !jugador.logros[12].reclamado))
            {
                if(aliados.Length < 2)
                {
                    jugador.AumentarLogro(12,1);
                }
            }

            //REVISAMOS NIVEL 20 COMPLETADO CON 1 PERSONAJE
            if((nivelHistoria == 20) && (jugador.logros.ContainsKey(12) && jugador.logros[12].reclamado) && (!jugador.logros.ContainsKey(13) || !jugador.logros[13].reclamado))
            {
            if(aliados.Length < 2)
                {
                    jugador.AumentarLogro(13,1);
                }
            }

            //REVISAMOS NIVEL 30 COMPLETADO CON 1 PERSONAJE
            if((nivelHistoria == 30) && (jugador.logros.ContainsKey(13) && jugador.logros[13].reclamado) && (!jugador.logros.ContainsKey(14) || !jugador.logros[14].reclamado))
            {
            if(aliados.Length < 2)
                {
                    jugador.AumentarLogro(14,1);
                }
            }

            //REVISAMOS NIVEL 40 COMPLETADO CON 1 PERSONAJE
            if((nivelHistoria == 40) && (jugador.logros.ContainsKey(14) && jugador.logros[14].reclamado) && (!jugador.logros.ContainsKey(15) || !jugador.logros[15].reclamado))
            {
                if(aliados.Length < 2)
                {
                    jugador.AumentarLogro(15,1);
                }
            }

            //REVISAMOS NIVEL 50 COMPLETADO CON 1 PERSONAJE
            if((nivelHistoria == 50) && (jugador.logros.ContainsKey(15) && jugador.logros[15].reclamado) && (!jugador.logros.ContainsKey(16) || !jugador.logros[16].reclamado))
            {
            if(aliados.Length < 2)
                {
                    jugador.AumentarLogro(16,1);
                }
            }

            //REVISAMOS NIVEL 60 COMPLETADO CON 1 PERSONAJE
            if((nivelHistoria == 60) && (jugador.logros.ContainsKey(16) && jugador.logros[16].reclamado) && (!jugador.logros.ContainsKey(17) || !jugador.logros[17].reclamado))
            {
                if(aliados.Length < 2)
                {
                    jugador.AumentarLogro(17,1);
                }
            }
        }

        if(tipoCombate != ""){
            //REVISAMOS SI PELEAMOS CONTRA UN AMIGO
            if( (!jugador.logros.ContainsKey(5) || !jugador.logros[5].reclamado) && tipoCombate == "amistoso")
            {
                jugador.AumentarLogro(5,1);
            }

            //REVISAMOS SI PELEAMOS CONTRA UN AMIGO
            if( (!jugador.logros.ContainsKey(0) || !jugador.logros[0].reclamado) && tipoCombate == "pvp")
            {
                jugador.AumentarLogro(0,3);
            }
        }
    }

    public void RevisarOtros(Personajes pjs = null, bool tienda = false)
    {
        //REVISAMOS SI INVOCACMOS ALGO
        if(pjs != null &&(!jugador.logros.ContainsKey(1) || !jugador.logros[1].reclamado))
        {
            jugador.AumentarLogro(1,1);
        }

        if(tienda == true && (!jugador.logros.ContainsKey(4) || !jugador.logros[4].reclamado))
        {
            jugador.AumentarLogro(4,1);
        }

    }

}
