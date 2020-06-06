using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class inteligencia_artificial 
{
    private mecanicas_combate mecanicas;

    // ( IMPORTANTE -> 88 SIGNIFICA "NO SE ENCONTRO UN RESULTADO", SI SE MODIFICA EL 88, SIGNIFICA QUE SI PODEMOS ATACAR, ES BASICAMENTE UN BOOLEANO)
    private static int index_objetivo = 88;
    private static Poderes poder_lanzar = null;
    //VALORES QUE USAREMOS EN ESTA INSTANCIA Y ENVIAREMOS A MECANICAS
    public Personajes[] personajes;
    public Personajes[] enemigos;

    //VALORES A RETORNAR A LA INSTANCIA DE COMBATE
    public Personajes[,] personajes_retorno;
    public Poderes poder_retorno; 
    int index_objetivo_retorno;


    public inteligencia_artificial(Personajes[] personajes, Personajes[] enemigos){
        
        this.personajes = personajes;
        this.enemigos = enemigos;

        //INSTANCIAMOS LAS MECANICAS
        mecanicas = new mecanicas_combate();
    }

    //MORAMOS LAS POSIBLES OPCIONES PARA ATACAR
    // ( IMPORTANTE -> 99 SIGNIFICA A TODOS LOS OBJETIVOS)

    public Personajes[,] Ejecutar(Personajes[] personajes, Personajes[] enemigos, Personajes personaje_en_turno){
        
        int numero_personajes = personajes.Length;
        personajes_retorno = new Personajes[2,numero_personajes];

        Revisar_matar_ataque(personajes, enemigos, personaje_en_turno);
        //SI ENCONTRAMOS A ALGUIEN Y EL PODER QUE LANZAREMOS NO ES NULO
        if(index_objetivo != 88 && poder_lanzar != null)
        {
            //SI NO ES HACIA TODOS LOS OBJETIVOS
            if(index_objetivo != 99)  Debug.Log("hacia/: " +  enemigos[index_objetivo]);
            else Debug.Log("hacia todos");

            //LANZAMOS EL PODER HACIA EL OBJETIVO INDICADO
            mecanicas_combate.Lanzar_poder(index_objetivo.ToString(), poder_lanzar, personajes, enemigos, personaje_en_turno);

            //ASIGNAMOS LA MATRIZ QUE ENVIARA LA INFORMACION DE COMO QUEDARON LOS PRSONAJES A LA INSTANCIA DE COMBATE
            personajes_retorno = matriz_envio_personajes(personajes, enemigos);

            //ASIGNAMOS EL PDOER Y EL INDEX QUE SE LANZO A LA INSTANCIA DE COMBATE
            poder_retorno = poder_lanzar;
            index_objetivo_retorno = index_objetivo;

            index_objetivo = 88;
            poder_lanzar = null;
            return personajes_retorno;
        }

        Revisar_si_curar(personajes, personaje_en_turno);
        if(index_objetivo != 88 && poder_lanzar != null)
        {
             if(index_objetivo != 99)  Debug.Log("hacia: " +  enemigos[index_objetivo]);
            else Debug.Log("hacia todos");
            mecanicas_combate.Lanzar_poder(index_objetivo.ToString(), poder_lanzar, personajes, enemigos, personaje_en_turno);
            personajes_retorno = matriz_envio_personajes(personajes, enemigos);
            //ASIGNAMOS EL PDOER Y EL INDEX QUE SE LANZO A LA INSTANCIA DE COMBATE
            poder_retorno = poder_lanzar;
            index_objetivo_retorno = index_objetivo;

            index_objetivo = 88;
            poder_lanzar = null;
            return personajes_retorno;
        }
        Buffear(personajes, enemigos, personaje_en_turno);
        if(index_objetivo != 88 && poder_lanzar != null)
        {
            if(index_objetivo != 99)  Debug.Log("hacia: " +  enemigos[index_objetivo]);
            else Debug.Log("hacia todos");
            mecanicas_combate.Lanzar_poder(index_objetivo.ToString(), poder_lanzar, personajes, enemigos, personaje_en_turno);
            personajes_retorno = matriz_envio_personajes(personajes, enemigos);
            //ASIGNAMOS EL PDOER Y EL INDEX QUE SE LANZO A LA INSTANCIA DE COMBATE
            poder_retorno = poder_lanzar;
            index_objetivo_retorno = index_objetivo;

            index_objetivo = 88;
            poder_lanzar = null;
            return personajes_retorno;
        }
        Debuffear(personajes, enemigos, personaje_en_turno);
        if(index_objetivo != 88 && poder_lanzar != null)
        {
            if(index_objetivo != 99)  Debug.Log("hacia: " +  enemigos[index_objetivo]);
            else Debug.Log("hacia todos");
            mecanicas_combate.Lanzar_poder(index_objetivo.ToString(), poder_lanzar, personajes, enemigos, personaje_en_turno);
            personajes_retorno = matriz_envio_personajes(personajes, enemigos);
            //ASIGNAMOS EL PDOER Y EL INDEX QUE SE LANZO A LA INSTANCIA DE COMBATE
            poder_retorno = poder_lanzar;
            index_objetivo_retorno = index_objetivo;

            index_objetivo = 88;
            poder_lanzar = null;
            return personajes_retorno;
        }
        
        // EN CASO DE QUE NO HAYA PODERES PARA TIRAR
        for(int i = 0; i < personaje_en_turno.poderesActivos.Length; i++)
        {
            if ( personaje_en_turno.poderesActivos[i].se_puede_usar)
            {
                poder_lanzar = personaje_en_turno.poderesActivos[i];
                index_objetivo = Enemigo_mayor_recibidor_daño(enemigos, poder_lanzar, personaje_en_turno);
                i = personaje_en_turno.poderesActivos.Length + 1;
            }
        }
                if(index_objetivo != 99 && index_objetivo != 88)  Debug.Log("hacia: " +  enemigos[index_objetivo]);
        else Debug.Log("hacia todos");
        mecanicas_combate.Lanzar_poder(index_objetivo.ToString(), poder_lanzar, personajes, enemigos, personaje_en_turno);
        personajes_retorno = matriz_envio_personajes(personajes, enemigos);
        //ASIGNAMOS EL PDOER Y EL INDEX QUE SE LANZO A LA INSTANCIA DE COMBATE
        poder_retorno = poder_lanzar;
        index_objetivo_retorno = index_objetivo;

        index_objetivo = 88;
        poder_lanzar = null;
        return personajes_retorno;
    }






















// FUNCIONES QUE EJECUTAREMOS PARA VER QUE ACCION TOMAR
    static void Revisar_matar_ataque(Personajes[] personajes, Personajes[] enemigos, Personajes personaje_en_turno){
        Poderes[] poderes = personaje_en_turno.poderesActivos;
        for(int i = 0; i < poderes.Length; i++){
            Poderes poder_iteracion = poderes[i];
            if (poder_iteracion.se_puede_usar && (poder_iteracion.tipo_poder.Contains("ataque") || poder_iteracion.tipo_poder.Contains("debuff")) )
            {
                float daño;
                daño = personaje_en_turno.Hallar_daño_poder(poder_iteracion, poder_iteracion.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if (poder_iteracion.atributo != "magia" || (poder_iteracion.atributo != "defensa_magica"))
                    {
                        daño -= daño * (enemigos[j].atributos.defensa_fisica / 100);
                    }else{
                        daño -= daño * (enemigos[j].atributos.defensa_magica / 100);
                    }
                    if(enemigos[j].atributos.salud - daño <= 0 && !(enemigos[j].estado_alterado.ContainsKey("muerto")) ){
                        index_objetivo = (poder_iteracion.objetivos != "multiple") ? j : 99;
                        poder_lanzar = poder_iteracion;
                    }
                }
            }
        }
    }
    
    static void Buffear(Personajes[] personajes, Personajes[] enemigos, Personajes personaje_en_turno){
        Poderes[] poderes = personaje_en_turno.poderesActivos;
        Array.Sort(poderes, delegate(Poderes poder1, Poderes poder2) {
                    return poder1.reutilizacion.CompareTo(poder2.reutilizacion); // (user1.Age - user2.Age)
        });
        Array.Reverse(poderes);
        for(int i = 0; i < poderes.Length; i++){
            Poderes poder_iteracion = poderes[i];
            if (poder_iteracion.se_puede_usar && poder_iteracion.tipo_poder != "debuff" && poder_iteracion.tipo_poder.Contains("buff"))
            {
                if (poder_iteracion.objetivos != "multiple")
                {
                    for(int j = 0; j < poder_iteracion.habilidades.Length; j++)
                    {
                        string habilidad = poder_iteracion.habilidades[j];
                        switch(habilidad){
                            case "aumentar_fuerza":
                                index_objetivo = Personaje_mayor_atributo("fuerza", personajes);
                                break;
                            case "aumentar_magia":
                                index_objetivo = Personaje_mayor_atributo("magia", personajes);
                                break;
                            case "aumentar_vitalidad":
                                index_objetivo = Personaje_mayor_atributo("vitalidad", personajes);
                                break;
                            case "aumentar_velocidad":
                                index_objetivo = Personaje_mayor_atributo("velocidad", personajes);
                                break;
                            case "aumentar_critico":
                                index_objetivo = Personaje_mayor_atributo("critico", personajes);
                                break;
                            case "aumentar_defensa_fisica":
                                index_objetivo = Personaje_mayor_atributo("defensa_fisica", personajes);
                                break;
                            case "aumentar_defensa_magica":
                                index_objetivo = Personaje_mayor_atributo("defensa_magica", personajes);
                                break;
                            case "aumentar_estadisticas":
                                index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "inmunidad":
                                if (Personaje_a_punto_morir(personajes, 25) != 88){ // 88 SIGNIFICA QUE NADIE VA A MORIR
                                    index_objetivo = Personaje_mayor_daño(personajes);
                                }else{
                                    index_objetivo = Personaje_a_punto_morir(personajes, 25);
                                }
                                break;
                            case "ignorar_defensa_fisica":
                                int op1 = Personaje_mayor_atributo("fuerza", personajes);
                                int op2 = Personaje_mayor_atributo("critico", personajes);
                                index_objetivo = (op1 > op2) ? op1 : op2;
                                break;
                            case "ignorar_defensa_magica":
                                index_objetivo = Personaje_mayor_atributo("magia", personajes);
                                break;
                            case "robo_vida":
                                int op3 = Personaje_mayor_atributo("fuerza", personajes);
                                int op4 = Personaje_mayor_atributo("critico", personajes);
                                index_objetivo = (op3 > op4) ? op3 : op4;
                                break;
                            case "ignorar_defensas":
                                index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            default:
                                break;
                        }
                    }
                }else{
                    index_objetivo = 99; // LANZAR A TODOS
                }
                i = poderes.Length +1; // SALIMOS DEL CICLO
                poder_lanzar = poder_iteracion;
            }
            
        }
    }

    static void Debuffear(Personajes[] personajes, Personajes[] enemigos, Personajes personaje_en_turno){
        Poderes[] poderes = personaje_en_turno.poderesActivos;
        Array.Sort(poderes, delegate(Poderes poder1, Poderes poder2) {
                    return poder1.reutilizacion.CompareTo(poder2.reutilizacion); // (user1.Age - user2.Age)
        });
        Array.Reverse(poderes);
        for(int i = 0; i < poderes.Length; i++){
            Poderes poder_iteracion = poderes[i];
            if (poder_iteracion.se_puede_usar && poder_iteracion.tipo_poder.Contains("debuff"))
            {
                if (poder_iteracion.objetivos != "multiple")
                {
                    for(int j = 0; j < poder_iteracion.habilidades.Length; j++)
                    {
                        string habilidad = poder_iteracion.habilidades[j];
                        switch(habilidad){
                            case "reducir_fuerza":
                                index_objetivo = Personaje_mayor_atributo("fuerza", personajes);
                                break;
                            case "reducir_magia":
                                index_objetivo = Personaje_mayor_atributo("magia", personajes);
                                break;
                            case "reducir_vitalidad":
                                index_objetivo = Personaje_mayor_atributo("vitalidad", personajes);
                                break;
                            case "reducir_velocidad":
                                index_objetivo = Personaje_mayor_atributo("velocidad", personajes);
                                break;
                            case "reducir_critico":
                                index_objetivo = Personaje_mayor_atributo("critico", personajes);
                                break;
                            case "reducir_defensa_fisica":
                                index_objetivo = Personaje_mayor_atributo("defensa_fisica", personajes);
                                break;
                            case "reducir_defensa_magica":
                                index_objetivo = Personaje_mayor_atributo("defensa_magica", personajes);
                                break;
                            case "reducir_estadisticas":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "congelar":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "heridas_graves":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "sangrado":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "quemar":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "quemar_grave":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "aturdir":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            case "dormir":
                                 index_objetivo = Personaje_mayor_daño(personajes);
                                break;
                            default:
                                break;
                        }
                    }
                }else{
                    index_objetivo = 99; // LANZAR A TODOS
                }
                i = poderes.Length +1; // SALIMOS DEL CICLO
                poder_lanzar = poder_iteracion;
            }
            
        }
    }


    static void Revisar_si_curar(Personajes[] personajes, Personajes personaje_en_turno){
        //REVISAMOS SI ALGUIEN TIENE MENOS DEL 25% DE SALUD
        int index_objetivo_local = Personaje_a_punto_morir(personajes, 25);
        if (index_objetivo_local != 88) // SI ALGUIEN SE VA A MORIR
        {
            //BUSCAMOS EN LOS PODERES POR CURAR
            for(int i = 0; i < personaje_en_turno.poderesActivos.Length; i++)
            {   if(personaje_en_turno.poderesActivos[i].se_puede_usar)
                {
                    for(int j = 0; j < personaje_en_turno.poderesActivos[i].habilidades.Length; j++)
                    {
                        if (personaje_en_turno.poderesActivos[i].habilidades[j].Contains("curar"))
                        {
                            //CURAMOS AL PRIMER PERSONAJE CON MENOS DE 25% DE VIDA
                            index_objetivo = Personaje_a_punto_morir(personajes, 25);
                            poder_lanzar = personaje_en_turno.poderesActivos[i];
                        }
                    }   
                }
            }
        }

        //REVISAMOS SI ALGUIEN TIENE MENOS DEL 50% DE SALUD
        index_objetivo = Personaje_a_punto_morir(personajes, 50);
        if (index_objetivo_local != 88) // SI ALGUIEN SE VA A MORIR
        {
            //BUSCAMOS EN LOS PODERES POR REVITALIZAR
            for(int i = 0; i < personaje_en_turno.poderesActivos.Length; i++)
            {   if(personaje_en_turno.poderesActivos[i].se_puede_usar)
                {
                    for(int j = 0; j < personaje_en_turno.poderesActivos[i].habilidades.Length; j++)
                    {
                        if (personaje_en_turno.poderesActivos[i].habilidades[j].Contains("revitalizar"))
                        {
                            //REVITALIZAMOS AL PERSONAJE CON MENOS DEL 50% DE VIDA
                            index_objetivo = Personaje_a_punto_morir(personajes, 50);
                            poder_lanzar = personaje_en_turno.poderesActivos[i];
                        }
                    }   
                }
            }
        }
        
    }









// FUNCIONES PARA VER A CUAL ENEMIGO ATACAR
    static int Enemigo_menos_vida(Personajes[] enemigos){
        float menor_salud = 9999999;
        int enemigo_ganador = 88;

        //BUSCAMOS EN LOS ENEMIGOS EL DE MENOS VIDA QUE NO ESTE MUERTO
        for(int j = 0; j < enemigos.Length; j++)
        {
            if(enemigos[j].atributos.salud < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
            {
                menor_salud = enemigos[j].atributos.salud;
                enemigo_ganador = j;
            }
        }
        return enemigo_ganador;
    }

    static int Enemigo_menos_defensa(Personajes[] enemigos, Poderes poder){
        float menor_salud = 9999999;
        int enemigo_ganador = 88;

        //DEPENDIENDO DEL ATAQUE QUE HAGA, MIRAMOS QUIEN TIENE MENOR DEFENSA CONTRA ESE ATAQUE
        switch(poder.atributo)
        {
            case "fuerza":
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if(enemigos[j].atributos.defensa_fisica < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        menor_salud = enemigos[j].atributos.defensa_fisica;
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "critico":
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if(enemigos[j].atributos.defensa_fisica < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        menor_salud = enemigos[j].atributos.defensa_fisica;
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "velocidad":
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if(enemigos[j].atributos.defensa_fisica < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        menor_salud = enemigos[j].atributos.defensa_fisica;
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "defensa_fisica":
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if(enemigos[j].atributos.defensa_fisica < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        menor_salud = enemigos[j].atributos.defensa_fisica;
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "vitalidad":
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if(enemigos[j].atributos.defensa_fisica < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        menor_salud = enemigos[j].atributos.defensa_fisica;
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "magia":   
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if(enemigos[j].atributos.defensa_magica < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        menor_salud = enemigos[j].atributos.defensa_magica;
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "defensa_magica":
                for(int j = 0; j < enemigos.Length; j++)
                {
                    if(enemigos[j].atributos.defensa_magica < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        menor_salud = enemigos[j].atributos.defensa_magica;
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            default:
                break;
        }
        return enemigo_ganador; // 88 NINGUN ENEMIGO
    }


    static int Enemigo_mayor_recibidor_daño(Personajes[] enemigos, Poderes poder, Personajes personaje_en_turno){
        float menor_salud = 9999999;
        int enemigo_ganador = 88;
        float daño = 0;

        // MIRAMOS QUE ENEMIGO RECIBE MAS DAÑO, LUEGO DE QUITAR LA DEFENSA Y AUMENTAR EL CRITICO
        switch(poder.atributo)
        {
            case "fuerza":
                daño = personaje_en_turno.Hallar_daño_poder(poder, poder.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);

                for(int j = 0; j < enemigos.Length; j++)
                {
                    daño -= daño * (enemigos[j].atributos.defensa_fisica / 100);
                    if ( enemigos[j].atributos.salud - daño < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "critico":
                daño = personaje_en_turno.Hallar_daño_poder(poder, poder.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);

                for(int j = 0; j < enemigos.Length; j++)
                {
                    daño -= daño * (enemigos[j].atributos.defensa_fisica / 100);
                    if ( enemigos[j].atributos.salud - daño < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "velocidad":
                daño = personaje_en_turno.Hallar_daño_poder(poder, poder.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);

                for(int j = 0; j < enemigos.Length; j++)
                {
                    daño -= daño * (enemigos[j].atributos.defensa_fisica / 100);
                    if ( enemigos[j].atributos.salud - daño < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "defensa_fisica":
                daño = personaje_en_turno.Hallar_daño_poder(poder, poder.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);

                for(int j = 0; j < enemigos.Length; j++)
                {
                    daño -= daño * (enemigos[j].atributos.defensa_fisica / 100);
                    if ( enemigos[j].atributos.salud - daño < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "vitalidad":
                daño = personaje_en_turno.Hallar_daño_poder(poder, poder.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);

                for(int j = 0; j < enemigos.Length; j++)
                {
                    daño -= daño * (enemigos[j].atributos.defensa_fisica / 100);
                    if ( enemigos[j].atributos.salud - daño < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "magia":
                daño = personaje_en_turno.Hallar_daño_poder(poder, poder.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);

                for(int j = 0; j < enemigos.Length; j++)
                {
                    daño -= daño * (enemigos[j].atributos.defensa_magica / 100);
                    if ( enemigos[j].atributos.salud - daño < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            case "defensa_magica":
                daño = personaje_en_turno.Hallar_daño_poder(poder, poder.atributo);
                daño = mecanicas_combate.Critico(daño, personaje_en_turno.atributos.critico);

                for(int j = 0; j < enemigos.Length; j++)
                {
                    daño -= daño * (enemigos[j].atributos.defensa_magica / 100);
                    if ( enemigos[j].atributos.salud - daño < menor_salud && !(enemigos[j].estado_alterado.ContainsKey("muerto")))
                    {
                        enemigo_ganador = j;
                    }
                }
                return enemigo_ganador;
            default:
                break;
        }
        return enemigo_ganador; // 88 NINGUN ENEMIGO
    }


    static int Enemigo_dps(Personajes[] enemigos){
        return Personaje_mayor_daño(enemigos);
    }











// FUNCIONES DE AYUDA PARA ENCONTRAR VALORES EN PERSONAJES
    static int Personaje_mayor_atributo(string atributo, Personajes[] personajes){
        int i = 0;
        float mayor_atributo = 0;
        int personaje_ganador = 0;
        switch(atributo){
            case "fuerza":
                for(i = 0; i < personajes.Length; i++){
                    if(personajes[i].atributos.fuerza > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                        mayor_atributo = personajes[i].atributos.fuerza;
                        personaje_ganador = i;
                    }
                }
                break;
            case "magia":
            for(i = 0;i < personajes.Length; i++){
                    if(personajes[i].atributos.magia > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                        mayor_atributo = personajes[i].atributos.magia;
                        personaje_ganador = i;
                    }
                }
                break;
            case "vitalidad":
            for(i = 0;i < personajes.Length; i++){
                    if(personajes[i].atributos.vitalidad > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                        mayor_atributo = personajes[i].atributos.vitalidad;
                        personaje_ganador = i;
                    }
                }
                break;
            case "velocidad":
            for(i = 0;i < personajes.Length; i++){
                    if(personajes[i].atributos.velocidad > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                        mayor_atributo = personajes[i].atributos.velocidad;
                        personaje_ganador = i;
                    }
                }
                break;
            case "critico":
            for(i = 0;i < personajes.Length; i++){
                    if(personajes[i].atributos.critico > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                        mayor_atributo = personajes[i].atributos.critico;
                        personaje_ganador = i;
                    }
                }
                break;
            case "defensa_fisica":
            for(i = 0;i < personajes.Length; i++){
                    if(personajes[i].atributos.defensa_fisica > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                        mayor_atributo = personajes[i].atributos.defensa_fisica;
                        personaje_ganador = i;
                    }
                }
                break;
            case "defensa_magica":
            for(i = 0;i < personajes.Length; i++){
                    if(personajes[i].atributos.defensa_magica > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                        mayor_atributo = personajes[i].atributos.defensa_magica;
                        personaje_ganador = i;
                    }
                }
                break;
            default:
            break;
        }
        return i;
    }

    static int Personaje_mayor_daño(Personajes[] personajes){
        float mayor_atributo = 0;
        int personaje_ganador = 0;

        for(int i = 0; i < personajes.Length; i++){
            if(personajes[i].atributos.fuerza > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                mayor_atributo = personajes[i].atributos.fuerza;
                personaje_ganador = i;
            }

            if(personajes[i].atributos.magia > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                mayor_atributo = personajes[i].atributos.magia;
                personaje_ganador = i;
            }

            if(personajes[i].atributos.critico > mayor_atributo && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                mayor_atributo = personajes[i].atributos.critico;
                personaje_ganador = i;
            }
        }
        return personaje_ganador;
    }

    static int Personaje_a_punto_morir(Personajes[] personajes, int porcentaje_riesgo){
        float riesgo = (porcentaje_riesgo / 100);
        int personaje_ganador = 88; // NINGUNO VA A MORIR
        for(int i = 0; i < personajes.Length; i++){
            float vitalidad = personajes[i].atributos.vitalidad;
            float salud = personajes[i].atributos.salud;
            if( salud <= (vitalidad * riesgo)  && !(personajes[i].estado_alterado.ContainsKey("muerto"))){
                personaje_ganador = i;
                return personaje_ganador;
            }
        }
        return personaje_ganador;
    }

    //GUARDAMOS LOS ENEMIGOS Y ALIADOS QUE MODIFICAMOS EN UNA MATRIX 2, TAMAÑO DE PERSONAJES
    static Personajes[,] matriz_envio_personajes(Personajes[] personajes, Personajes[] enemigos)
    {
    
        Personajes[,] matrix = new Personajes[2, 4];
        //ALIADOS POS 0
        for(int i = 0; i < personajes.Length; i++)
        {
            matrix[0,i] = personajes[i];
        }

        //ENEMIGOS POS 1
        for(int i = 0; i < enemigos.Length; i++)
        {
            matrix[1,i] = enemigos[i];
        }
        return matrix;
    }

    //NO ES STATIC POR QUE NO DA ERROR ASI
    public Poderes GetPoderLanzado()
    {
        return poder_retorno;
    }

    public int GetIndexObjetivo()
    {
        return index_objetivo_retorno;
    }

}
