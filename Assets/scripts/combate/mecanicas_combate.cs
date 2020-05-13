using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class mecanicas_combate {
    
    public mecanicas_combate(){}

    //TABLA DEFENSAS ELEMENTALES;
    private static Dictionary<string, float[]> defensas_elementales = new Dictionary<string, float[]>(){
        {"agua", new float[] {1F, 1.5F, 1F, 0.5F, 1F, 1F} },
        {"fuego", new float[] {0.5F, 1F, 1.5F, 1F, 1F, 1F} },
        {"tierra", new float[] {1F, 0.5F, 1F, 1.5F, 1F, 1F} },
        {"trueno", new float[] {1.5F, 1F, 0.5F, 1F, 1F, 1F} },
        {"oscuridad", new float[] {1F, 1F, 1F, 1F, 0.5F, 1.5F} },
        {"luz", new float[] {1F, 1F, 1F, 1F, 1.5F, 0.5F} },
    };
    
    
    public static void Lanzar_poder(string index, Poderes poder_a_ser_lanzado, Personajes[] personajes, Personajes[] enemigos, Personajes personaje_en_turno){
        if(poder_a_ser_lanzado == null) return;
        Poderes poder = poder_a_ser_lanzado;

        // TOMAMOS EL INDEX DEL PERSONAJE EN TURNO PARA MODIFICARLO
        int index_personaje_en_turno = 0;
        for(int j = 0; j < personajes.Length; j++){
            if(personaje_en_turno.nombre.Contains(personajes[j].nombre)){
                index_personaje_en_turno = j;
                break;
            }
        }
        // string output = JsonUtility.ToJson(personajes[Int32.Parse(index)].atributos, true);
        // Debug.Log(output);
        //MIRAMOS QUE PODER FUE LANZADO Y LLAMAMOS LA FUNCION CORRESPONDIENTE
        personaje_en_turno.Poder_usado(poder_a_ser_lanzado);
        switch(poder.tipo_poder){ 
            case "ataque":
                ataque(poder, index, index_personaje_en_turno, personajes, enemigos);
                break;
            case "buff":
                buff(poder, index, index_personaje_en_turno, personajes, enemigos);
                break;
            case "purgar":
                purgar();
                break;
            case "debuff":
                debuff(poder, index, index_personaje_en_turno, personajes, enemigos);
                break;
            case "ataque_debuff":
                ataque(poder, index, index_personaje_en_turno, personajes, enemigos);
                debuff(poder, index, index_personaje_en_turno, personajes, enemigos);
                break;
            case "ataque_buff":
                ataque(poder, index, index_personaje_en_turno, personajes, enemigos);
                buff(poder, index, index_personaje_en_turno, personajes, enemigos);
                break;
            default:
                break;
        }
    }

    public static void ataque(Poderes poder, string index_objetivo, int index_personaje_en_turno, Personajes[] personajes, Personajes[] enemigos){
        int index = int.Parse(index_objetivo);
        Personajes[] target;
        float atributo_;
        Personajes personaje_en_turno = personajes[index_personaje_en_turno];

        // MIRAMOS CON QUE ATRIBUTO SE HARA EL DAÑO
        switch(poder.atributo){
            case "fuerza":
                atributo_ = personaje_en_turno.atributos.fuerza;
                break;
            case "magia":
                atributo_ = personaje_en_turno.atributos.magia;
                break;
            case "vitalidad":
                atributo_ = personaje_en_turno.atributos.vitalidad;
                break;
            case "defensa_fisica":
                atributo_ = personaje_en_turno.atributos.defensa_fisica;
                break;
            case "defensa_magica":
                atributo_ = personaje_en_turno.atributos.defensa_magica;
                break;
            case "velocidad":
                atributo_ = personaje_en_turno.atributos.velocidad;
                break;
            default:
                atributo_ = 1;
                break;
        }
        // TOMAMOS EL OBJETIVO DEPENDIENDO DE QUIEN HAYA SIDO SELECIONADO
        if(poder.objetivos == "unico"){
            target = new Personajes[1]{enemigos[index]};
            float daño = (atributo_ * poder.multiplicador) + poder.daño_base;
            HacerDaño(target, daño, personaje_en_turno.elemento, poder.atributo, index, index_personaje_en_turno, enemigos, personajes);
        }else{
            float daño = (atributo_ * poder.multiplicador) + poder.daño_base;
            // 99 PARA INDICAR QUE AFECTAREMOS A TODOS
            HacerDaño(enemigos, daño, personaje_en_turno.elemento, poder.atributo, 99, index_personaje_en_turno, enemigos, personajes);
        }

    }


    public static void buff(Poderes poder, string index_objetivo, int index_personaje_en_turno, Personajes[] personajes, Personajes[] enemigos){
        
        int index = int.Parse(index_objetivo);
        Personajes[] target;
        float atributo_;
        string[] habilidades = poder.habilidades;
        Personajes personaje_en_turno = personajes[index_personaje_en_turno];

        // MIRAMOS CON QUE ATRIBUTO SE HARA EL DAÑO
        switch(poder.atributo){
            case "fuerza":
                atributo_ = personaje_en_turno.atributos.fuerza;
                break;
            case "magia":
                atributo_ = personaje_en_turno.atributos.magia;
                break;
            case "vitalidad":
                atributo_ = personaje_en_turno.atributos.vitalidad;
                break;
            case "defensa_fisica":
                atributo_ = personaje_en_turno.atributos.defensa_fisica;
                break;
            case "defensa_magica":
                atributo_ = personaje_en_turno.atributos.defensa_magica;
                break;
            case "velocidad":
                atributo_ = personaje_en_turno.atributos.velocidad;
                break;
            default:
                atributo_ = 1;
                break;
        }
        // TOMAMOS EL OBJETIVO DEPENDIENDO DE QUIEN HAYA SIDO SELECIONADO
        if(poder.objetivos == "unico"){
            target = new Personajes[1]{personajes[index]};
            //HacerDaño(target, daño, personaje_en_turno.elemento, poder.atributo, index, index_personaje_en_turno);
            buffear(target, index, personajes, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else if (poder.objetivos == "multiple"){
            // 99 PARA INDICAR QUE AFECTAREMOS A TODOS
            //HacerDaño(enemigos, daño, personaje_en_turno.elemento, poder.atributo, 99, index_personaje_en_turno);
            buffear(personajes, 99, personajes, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else{
            target = new Personajes[1]{personajes[index_personaje_en_turno]};
            Debug.Log("tirando buff propio");
            buffear(target, index_personaje_en_turno, personajes, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }
    }
    

    public static void purgar(){}
    public static void debuff(Poderes poder, string index_objetivo, int index_personaje_en_turno, Personajes[] personajes, Personajes[] enemigos){
        
        int index = int.Parse(index_objetivo);
        Personajes[] target;
        float atributo_;
        string[] habilidades = poder.habilidades;
        Personajes personaje_en_turno = personajes[index_personaje_en_turno];

        // MIRAMOS CON QUE ATRIBUTO SE HARA EL DAÑO
        switch(poder.atributo){
            case "fuerza":
                atributo_ = personaje_en_turno.atributos.fuerza;
                break;
            case "magia":
                atributo_ = personaje_en_turno.atributos.magia;
                break;
            case "vitalidad":
                atributo_ = personaje_en_turno.atributos.vitalidad;
                break;
            case "defensa_fisica":
                atributo_ = personaje_en_turno.atributos.defensa_fisica;
                break;
            case "defensa_magica":
                atributo_ = personaje_en_turno.atributos.defensa_magica;
                break;
            case "velocidad":
                atributo_ = personaje_en_turno.atributos.velocidad;
                break;
            default:
                atributo_ = 1;
                break;
        }
        // TOMAMOS EL OBJETIVO DEPENDIENDO DE QUIEN HAYA SIDO SELECIONADO
        if(poder.objetivos == "unico"){
            target = new Personajes[1]{enemigos[index]};
            debuffear(target, index, enemigos, personajes,habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else if (poder.objetivos == "multiple"){
            // 99 PARA INDICAR QUE AFECTAREMOS A TODOS
            debuffear(enemigos, 99, enemigos, personajes,habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else{
            target = new Personajes[1]{enemigos[index]};
            debuffear(target, index_personaje_en_turno, enemigos, personajes,habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }
    }



    public static void HacerDaño(Personajes[] target, float daño, string elementoAtacante, string atributo, int index_objetivo, int index_personaje_en_turno, Personajes[] enemigos, Personajes[] personajes){

        Personajes personaje_en_turno = personajes[index_personaje_en_turno];

        //REVISAMOS TODOS LOS ENEMIGOS O SOLO UNO
        for (int i= 0; i < target.Length; i++){
            string elementoDefensor = target[i].elemento;
            bool detener_daño = false;
            // REVISAMOS SI EL GOLPE SERA CRITICO
            daño = Critico(daño, personaje_en_turno.atributos.critico);
            
            // REDUCIMOS DEFENSAS
            if (atributo == "magia"){
                if (!(target[i].estado_alterado.ContainsKey("inmunidad_magica")) && !(target[i].estado_alterado.ContainsKey("inmunidad"))){
                        float porcentaje_reduccion_magia =  (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensa_magica")? personaje_en_turno.estado_alterado["ignorar_defensa_magica"][0] : 0F);
                        float porcentaje_reduccion_general = (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensas")? personaje_en_turno.estado_alterado["ignorar_defensas"][0] : 0F);
                        float reduccion_defensas = (porcentaje_reduccion_magia >= porcentaje_reduccion_general)? porcentaje_reduccion_magia : porcentaje_reduccion_general;
                        if (reduccion_defensas <= target[i].atributos.defensa_magica){
                            daño -= daño * ((target[i].atributos.defensa_magica / 100) - (reduccion_defensas / 100));
                        }
                }else{
                    detener_daño = true;
                }
            }else{
                if (!(target[i].estado_alterado.ContainsKey("inmunidad_fisica")) && !(target[i].estado_alterado.ContainsKey("inmunidad"))){
                    float porcentaje_reduccion_magia =  (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensa_fisica")? personaje_en_turno.estado_alterado["ignorar_defensa_fisica"][0] : 0F);
                    float porcentaje_reduccion_general = (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensas")? personaje_en_turno.estado_alterado["ignorar_defensas"][0] : 0F);
                    float reduccion_defensas = (porcentaje_reduccion_magia >= porcentaje_reduccion_general)? porcentaje_reduccion_magia : porcentaje_reduccion_general;
                    if (reduccion_defensas <= target[i].atributos.defensa_fisica){
                        daño -= daño * ((target[i].atributos.defensa_fisica / 100) - (reduccion_defensas / 100));
                    }
                }else{
                    detener_daño = true;
                }
            }
            // SI NO HAY INMUNIDADES, REVISAMOS CADA ELEMENTO PARA VER SU MULTIPLICADOR DE DAÑO
            if (!detener_daño){
                if (elementoDefensor == "agua"){
                    daño += daño * defensas_elementales[elementoAtacante][0];
                }else if (elementoDefensor == "fuego"){
                    daño += daño * defensas_elementales[elementoAtacante][1];
                }else if (elementoDefensor == "tierra"){
                    daño += daño * defensas_elementales[elementoAtacante][2];
                }else if (elementoDefensor == "trueno"){
                    daño += daño * defensas_elementales[elementoAtacante][3];
                }else if (elementoDefensor == "oscuridad"){
                    daño += daño * defensas_elementales[elementoAtacante][4];
                }else if (elementoDefensor == "luz"){
                    daño += daño * defensas_elementales[elementoAtacante][6];
                }else{
                    Debug.Log("elemento defensor erroneo");
                }
            }else{
                daño = 0;
            }
            
            // SI TENEMOS ROBO DE VIDA, NOS ENCARGAMOS DE CURARNOS
            target[i].Daño(daño);

            if (personaje_en_turno.estado_alterado.ContainsKey("robo_vida")){
                float robo_vida = daño * personaje_en_turno.estado_alterado["robo_vida"][0];
                personajes[index_personaje_en_turno].Curar(robo_vida);
            } 
            
            //SI TENEMOS UN SOLO OBJETIVO, MODIFICAMOS EL ARREGLO DE ENEMIGOS DIRECTAMENTE
            if (target.Length < 2) enemigos[index_objetivo] = target[i];
        }
        
    }
    //LOS PRIMEROS DOS PARAMETROS SON OBLIGATORIOS, EL RESTO SON OPCIONALES
    public static void buffear(Personajes[] target, int index_objetivo, Personajes[] personajes, string[] habilidades = null, float daño_base = 0f, float atributo = 0f, float multiplicador_efecto = 0f, int duracion_efecto_int = 0){
        float efecto = 0F;
        float duracion_efecto = 0F;
        bool chequeando_bufos = false;

        //SI NOS PASAN TODOS LOS PARAMETROS, REVISAMOS CUANTO SERA EL EFECTO DEL BUFF
        if (habilidades != null){
            efecto = (atributo * multiplicador_efecto) + daño_base;
            duracion_efecto = Convert.ToSingle(duracion_efecto_int);
            duracion_efecto -= 1;
        }
        //REVISAMOS TODOS LOS PERSONAJES QUE NOS LLEGUEN
        for(int i = 0; i < target.Length; i++){
            // SI NO NOS LLEGAN TODOS LOS PARAMETROS, SIGNIFICA QUE ESTAMOS CHEQUEANDO BUFFOS, TOMAMOS UNA LISTA DE BUFFOS EN EL TARGET
            if (habilidades == null){
                habilidades = new string[target[i].estado_alterado.Count];
                int k = 0;
                foreach (KeyValuePair<string, float[]> estado in target[i].estado_alterado){
                    habilidades[k] = estado.Key;
                    k++;
                }
                chequeando_bufos = true;
            }
            for(int j = 0; j < habilidades.Length; j++){
                string habilidad = habilidades[j];
                // SI ESTAMOS CHEQUEANDO BUFFOS AGARRAMOS EL EFECTO Y LA DURACION DEL TARGET
                if (chequeando_bufos == true){
                    efecto = target[i].estado_alterado[habilidades[j]][0];
                    duracion_efecto = (target[i].estado_alterado[habilidades[j]][1]) - 1;
                }
                switch (habilidad){
                    case "curacion":
                    //MODIFICACION NORMAL A LA VIDA
                        target[i].Curar(efecto);
                        break;
                    case "revivir":
                    //REMOVEMOS UN ESTADO NEGATIVO
                        if(target[i].estado_alterado.ContainsKey("muerto")){
                            target[i].estado_alterado.Remove("muerto");
                            target[i].atributos.vitalidad = efecto;
                        }
                        break;
                    case "revitalizar":
                    //CONTANTE AUMENTO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0
                        if (target[i].estado_alterado.ContainsKey("revitalizar")) target[i].estado_alterado.Remove("revitalizar");
                        target[i].estado_alterado["revitalizar"] = new float[]{efecto, duracion_efecto};
                        target[i].Curar(efecto);
                        if(target[i].estado_alterado["revitalizar"][1] <= 0) target[i].estado_alterado.Remove("revitalizar");
                        break;
                    //AUMENTO UNICO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                    case "aumentar_fuerza":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("fuerza", "aumentar_fuerza", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_vitalidad":
                         // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("vitalidad", "aumentar_vitalidad", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_magia":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("magia", "aumentar_magia", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_velocidad":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("velocidad", "aumentar_velocidad", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_critico":
                         // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("critico", "aumentar_critico", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_defensa_fisica":
                         // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("defensa_fisica", "aumentar_defensa_fisica", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_defensa_magica":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("defensa_magica", "aumentar_defensa_magica", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_fuerza_magia":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("fuerza", "aumentar_fuerza", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("magia", "aumentar_magia", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_defensas":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("defensa_fisica", "aumentar_defensa_fisica", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("defensa_magica", "aumentar_defensa_magica", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_velocidad_critico":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("velocidad", "aumentar_velocidad", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("critico", "aumentar_critico", efecto, duracion_efecto, false);
                        break;
                    //PONEMOS UN ESTADO QUE NOS DARA CIERTOS BENEFICIOS EN OTRAS FUNCIONES DEL JUEGO
                    case "inmunidad_magica":
                        if (target[i].estado_alterado.ContainsKey("inmunidad_magica")) target[i].estado_alterado.Remove("inmunidad_magica");
                        target[i].estado_alterado["inmunidad_magica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["inmunidad_magica"][1] <= 0){
                            target[i].estado_alterado.Remove("inmunidad_magica");
                        }
                        break;
                    case "inmunidad_fisica":
                        if (target[i].estado_alterado.ContainsKey("inmunidad_fisica")) target[i].estado_alterado.Remove("inmunidad_fisica");
                        target[i].estado_alterado["inmunidad_fisica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["inmunidad_fisica"][1] <= 0){
                            target[i].estado_alterado.Remove("inmunidad_fisica");
                        }
                        break;
                    case "inmunidad":
                        if (target[i].estado_alterado.ContainsKey("inmunidad")) target[i].estado_alterado.Remove("inmunidad");
                        target[i].estado_alterado["inmunidad"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["inmunidad"][1] <= 0){
                            target[i].estado_alterado.Remove("inmunidad");
                        }
                        break;
                    case "robo_vida":
                        if (target[i].estado_alterado.ContainsKey("robo_vida")) target[i].estado_alterado.Remove("robo_vida");
                        target[i].estado_alterado["robo_vida"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["robo_vida"][1] <= 0){
                            target[i].estado_alterado.Remove("robo_vida");
                        }
                        break;
                    case "aumentar_estadisticas":
                     //AUMENTO UNICO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("velocidad", "aumentar_velocidad", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("critico", "aumentar_critico", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("fuerza", "aumentar_fuerza", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("magia", "aumentar_magia", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("vitalidad", "aumentar_vitalidad", efecto, duracion_efecto, false);
                        break;
                    case "ignorar_defensa_fisica":
                        if (target[i].estado_alterado.ContainsKey("ignorar_defensa_fisica")) target[i].estado_alterado.Remove("ignorar_defensa_fisica");
                        // efecto = porcentaje de ignorar
                        target[i].estado_alterado["ignorar_defensa_fisica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["ignorar_defensa_fisica"][1] <= 0){
                            target[i].estado_alterado.Remove("ignorar_defensa_fisica");
                        }
                        break;
                    case "ignorar_defensa_magica":
                        if (target[i].estado_alterado.ContainsKey("ignorar_defensa_magica")) target[i].estado_alterado.Remove("ignorar_defensa_magica");
                        // efecto = porcentaje de ignorar
                        target[i].estado_alterado["ignorar_defensa_magica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["ignorar_defensa_magica"][1] <= 0){
                            target[i].estado_alterado.Remove("ignorar_defensa_magica");
                        }
                        break;
                    case "ignorar_defensas":
                        if (target[i].estado_alterado.ContainsKey("ignorar_defensas")) target[i].estado_alterado.Remove("ignorar_defensas");
                        // efecto = porcentaje de ignorar
                        target[i].estado_alterado["ignorar_defensas"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["ignorar_defensas"][1] <= 0){
                            target[i].estado_alterado.Remove("ignorar_defensas");
                        }
                        break;
                     case "locura":
                        target[i].Alteraciones_atributos("fuerza", "locura", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("defensa_fisica", "locura", efecto, duracion_efecto, true);
                        target[i].Alteraciones_atributos("defensa_magica", "locura", efecto, duracion_efecto, true);
                        break;
                    default:
                        break;
                }
            }
        }

        //SI ES PARA SOLO UN OJETIVO LO MODIFICAMOS EN EL ARREGLO DE PERSONAJES
        if(target.Length < 2) personajes[index_objetivo] = target[0];

    }



public static void debuffear(Personajes[] target, int index_objetivo, Personajes[] enemigos, Personajes[] personajes, string[] habilidades = null, float daño_base = 0f, float atributo = 0f, float multiplicador_efecto = 0f, int duracion_efecto_int = 0){
    float efecto = 0F;
    float duracion_efecto = 0F;
    bool chequeando_debuffos = false;

    //SI NOS PASAN TODOS LOS PARAMETROS, REVISAMOS CUANTO SERA EL EFECTO DEL BUFF
    if (habilidades != null){
        efecto = (atributo * multiplicador_efecto) + daño_base;
        duracion_efecto = Convert.ToSingle(duracion_efecto_int);
        duracion_efecto -= 1;
    }
    //REVISAMOS TODOS LOS PERSONAJES QUE NOS LLEGUEN
    for(int i = 0; i < target.Length; i++){
        // SI NO NOS LLEGAN TODOS LOS PARAMETROS, SIGNIFICA QUE ESTAMOS CHEQUEANDO BUFFOS, TOMAMOS UNA LISTA DE BUFFOS EN EL TARGET
        if (habilidades == null){
            habilidades = new string[target[i].estado_alterado.Count];
            int k = 0;
            foreach (KeyValuePair<string, float[]> estado in target[i].estado_alterado){
                habilidades[k] = estado.Key;
                k++;
            }
            chequeando_debuffos = true;
        }
        for(int j = 0; j < habilidades.Length; j++){
            string habilidad = habilidades[j];
            // SI ESTAMOS CHEQUEANDO BUFFOS AGARRAMOS EL EFECTO Y LA DURACION DEL TARGET
            if (chequeando_debuffos == true){
                efecto = target[i].estado_alterado[habilidades[j]][0];
                duracion_efecto = (target[i].estado_alterado[habilidades[j]][1]) - 1;
            }
            switch (habilidad){
                case "heridas_graves":
                    if (target[i].estado_alterado.ContainsKey("heridas_graves")) target[i].estado_alterado.Remove("heridas_graves");
                    target[i].estado_alterado["heridas_graves"] = new float[]{efecto, duracion_efecto};
                    if(target[i].estado_alterado["heridas_graves"][1] <= 0){
                        target[i].estado_alterado.Remove("heridas_graves");
                    }
                    break;
                case "sangrado":
                    if (target[i].estado_alterado.ContainsKey("sangrado")) target[i].estado_alterado.Remove("sangrado");
                        target[i].estado_alterado["sangrado"] = new float[]{efecto, duracion_efecto};
                        target[i].Daño(efecto);
                        if(target[i].estado_alterado["sangrado"][1] <= 0) target[i].estado_alterado.Remove("revitalizar");
                        break;
                //REDUCCION UNICA DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                case "reducir_fuerza":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("fuerza", "reducir_fuerza", efecto, duracion_efecto, true);
                    break;
                case "reducir_vitalidad":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("vitalidad", "reducir_vitalidad", efecto, duracion_efecto, true);
                    break;
                case "reducir_magia":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("magia", "reducir_magia", efecto, duracion_efecto, true);
                    break;
                case "reducir_velocidad":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("velocidad", "reducir_velocidad", efecto, duracion_efecto, true);
                    break;
                case "reducir_critico":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("critico", "reducir_critico", efecto, duracion_efecto, true);
                    break;
                case "reducir_defensa_fisica":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("defensa_fisica", "reducir_defensa_fisica", efecto, duracion_efecto, true);
                    break;
                case "reducir_defensa_magica":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("defensa_magica", "reducir_defensa_magica", efecto, duracion_efecto, true);
                    break;
                case "reducir_fuerza_magia":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("fuerza", "reducir_fuerza", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("magia", "reducir_magia", efecto, duracion_efecto, true);
                    break;
                case "reducir_defensas":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("defensa_fisica", "reducir_defensa_fisica", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("defensa_magica", "reducir_defensa_magica", efecto, duracion_efecto, true);
                    break;
                case "reducir_velocidad_critico":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("velocidad", "reducir_velocidad", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("critico", "reducir_critico", efecto, duracion_efecto, true);
                    break;
                case "reducir_estadisticas":
                    //AUMENTO UNICO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("velocidad", "reducir_velocidad", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("critico", "reducir_critico", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("fuerza", "reducir_fuerza", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("magia", "reducir_magia", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("vitalidad", "reducir_vitalidad", efecto, duracion_efecto, true);
                    break;

                case "quemar":
                    if (target[i].estado_alterado.ContainsKey("quemar")) target[i].estado_alterado.Remove("quemar");
                        target[i].estado_alterado["quemar"] = new float[]{efecto, duracion_efecto};
                        target[i].Daño(efecto);
                        if(target[i].estado_alterado["quemar"][1] <= 0) target[i].estado_alterado.Remove("quemar");
                        break;
                case "quemar_grave":
                    if (target[i].estado_alterado.ContainsKey("quemar_grave")) target[i].estado_alterado.Remove("quemar_grave");
                        target[i].estado_alterado["quemar_grave"] = new float[]{efecto, duracion_efecto};
                        target[i].estado_alterado["heridas_graves"] = new float[]{efecto, duracion_efecto};
                        target[i].Daño(efecto);
                        if(target[i].estado_alterado["quemar_grave"][1] <= 0) target[i].estado_alterado.Remove("quemar_grave");
                        break;
                case "congelar":
                    if (target[i].estado_alterado.ContainsKey("congelar")){
                        target[i].estado_alterado.Remove("congelar");
                        target[i].estado_alterado["congelar"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["congelar"][1] <= 0){
                            target[i].estado_alterado.Remove("congelar");
                        }
                    }else{
                        target[i].Daño(efecto);
                        target[i].estado_alterado["congelar"] = new float[]{efecto, duracion_efecto};
                    }  
                    break;
                case "aturdir":
                    if (target[i].estado_alterado.ContainsKey("aturdir")){
                        target[i].estado_alterado.Remove("aturdir");
                        target[i].estado_alterado["aturdir"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["aturdir"][1] <= 0){
                            target[i].estado_alterado.Remove("aturdir");
                        }
                    }else{
                        target[i].Daño(efecto);
                        target[i].estado_alterado["aturdir"] = new float[]{efecto, duracion_efecto};
                    }  
                    break;
                case "dormir":
                    if (target[i].estado_alterado.ContainsKey("dormir")){
                        // SI EL OBJETIVO RECIBE DAÑO EN ALGUN MOMENTO, LO DESPERTAMOS, REMOVIENDO EL EFECTO
                        if(efecto == target[i].atributos.salud){
                            target[i].estado_alterado.Remove("dormir");
                            target[i].estado_alterado["dormir"] = new float[]{target[i].atributos.salud, duracion_efecto};
                        }else{
                            target[i].estado_alterado.Remove("dormir");
                        }
                        if(target[i].estado_alterado["dormir"][1] <= 0){
                            target[i].estado_alterado.Remove("dormir");
                        }
                    }else{
                        target[i].estado_alterado["dormir"] = new float[]{target[i].atributos.salud, duracion_efecto};
                    }  
                    break;
                case "silenciar":
                    break;
                default:
                    break;
            }
        }
    }
    //SI ES PARA SOLO UN OJETIVO LO MODIFICAMOS EN EL ARREGLO DE PERSONAJES
    if(target.Length < 2 && chequeando_debuffos == false) enemigos[index_objetivo] = target[0];
    else if (target.Length < 2 && chequeando_debuffos == true){
        personajes[index_objetivo] = target[0];
    } 
}





    public static float Critico(float daño, float pro_critico){
        var rand = new System.Random();
        float comparador_critico = rand.Next(0, 101);
        if (comparador_critico < pro_critico){
            daño = daño * 1.5F;
            Debug.Log("GOLPE CRITICO");
            return daño;
        }
        return daño;
    }


}
