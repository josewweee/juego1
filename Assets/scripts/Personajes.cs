﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Personajes
{
    public string nombre;
    public int nivel;
    public int nivel_maximo;
    public int estrellas;
    public int despertadas;
    public Atributos atributos; // FUERZA, VITALIDAD, MAGIA, VELOCIDAD, CRITICO, DEF_FISICA, DEF_MAGICA
    public Equipo[] equipamiento;
    public int experiencia;
    public Poderes[] poderes; // 9
    public Poderes[] poderesActivos; // 4
    public string elemento; // AGUA, FUEGO, TIERRA, TRUENO, LUZ, OSCURIDAD
    public DiccionarioStringFloatArray estado_alterado;
    //public Dictionary<string, float[]> estado_alterado; // NOMBRE EFECTO -> [ DAÑO EFECTO, DURACION EFECTO ]
    public string rareza; // COMUN, RARO, MITICO, LEGENDARIO
    public int fragmentos;
    public string[] imagen_completa; // UBICACION SPRITE, POSICION SPRITE DENTRO DE LA LISTA DE SPRITES
    public string foto_perfil;

        // string output = JsonUtility.ToJson(atributos, true);
        // Debug.Log(output);

    public Personajes Crear_personaje(string personaje)
    {
        Personajes nuevo_personaje;
        switch(personaje){
            case "roger":
                nuevo_personaje = new roger();
                return nuevo_personaje;
            case "alicia":
                nuevo_personaje = new alicia();
                return nuevo_personaje;
            case "liliana":
                nuevo_personaje = new liliana();
                return nuevo_personaje;
            case "martis":
                nuevo_personaje = new martis();
                return nuevo_personaje;
            default:
                return null;
        }
    }

    public List<Personajes> Crear_todos_personajes()
    {
        //ARREGLO EN DONDE SE INSTANCIAN TODOS LOS PERSONAJES QUE EXITEN
        List<Personajes> lista_personajes = new List<Personajes>(){
            new roger(),
            new alicia(),
            new liliana(),
            new martis()
        };

        return lista_personajes;
    }

    public virtual void Subir_nivel(int niveles)
    {
       nivel += niveles;
         for(int i = 0; i < niveles; i++)
        {
            experiencia = 0;
            atributos.fuerza += 1F;
            atributos.vitalidad += 10F;
            atributos.magia += 1F;
            atributos.velocidad += 1F;
            atributos.critico += 0.5F;
            atributos.defensa_fisica += 0.5F;
            atributos.defensa_magica += 0.5F;
        }
    }

    public virtual void Ganar_exp(int exp)
    {
        experiencia += exp;
        if(experiencia >= Math.Pow(nivel, 2) && nivel < nivel_maximo) Subir_nivel(1);
    }

    public virtual void Resetear_personaje()
    {
        this.atributos.Resetear_atributos();
        this.estado_alterado.Clear();
        foreach(Poderes p in poderesActivos)
        {
            p.Resetear_poder();
        }

        Subir_nivel(this.nivel - 1);
        //Aumentar_estadisticas_equipamiento();
    }

    public virtual bool Asignar_poder(Poderes poder)
    {
        for(int i = 0; i < poderesActivos.Length; i++)
        {
            if(poderesActivos[i] != null && poder.nombre == poderesActivos[i].nombre)
            {
                poderesActivos[i] = null;
                return false;
            }
        }

         for(int i = 0; i < poderesActivos.Length; i++)
        {
            if (poderesActivos[i] == null)
            {
                poderesActivos[i] = poder;
                return true;
            }
        }
        return false;
    }

    public void Curar(float efecto){
        if(atributos.salud <= 0) return;
        if (estado_alterado.ContainsKey("heridas_graves")) efecto -= efecto * (estado_alterado["heridas_graves"][0] / 100);
        float salud_final = efecto + atributos.salud;
        if(salud_final <= atributos.vitalidad){
            atributos.salud = salud_final;
        }
        else{
            atributos.salud = atributos.vitalidad;
        }
    }

    public void Evolucionar()
    {
        switch(estrellas){
            case 0:
                if (fragmentos >= 20){
                    fragmentos -= 20;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 1:
                if (fragmentos >= 40){
                    fragmentos -= 40;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 2:
            if (fragmentos >= 60){
                    fragmentos -= 60;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 3:
            if (fragmentos >= 80){
                    fragmentos -= 80;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 4:
            if (fragmentos >= 100){
                    fragmentos -= 100;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 5:
            if (fragmentos >= 120){
                    fragmentos -= 120;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 6:
            if (fragmentos >= 140){
                    fragmentos -= 140;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            default:
                Debug.Log("estrella erroneas " + estrellas);
                break;
            
        }
    }

    public void Modificar_atributo(float efecto, string atributo){
        switch(atributo){
            case "vitalidad":
                if((atributos.vitalidad += efecto) < 0){atributos.vitalidad = 0; break;}
                atributos.vitalidad += efecto;
                if(atributos.vitalidad < atributos.salud) atributos.salud = atributos.vitalidad;
                break;
            case "fuerza":
                if((atributos.fuerza += efecto) < 0){atributos.fuerza = 0; break;}
                atributos.fuerza += efecto;
                break;
            case "magia":
                if((atributos.magia += efecto) < 0){atributos.magia = 0; break;}
                atributos.magia += efecto;
                break;
            case "velocidad":
                if((atributos.velocidad += efecto) < 0){atributos.velocidad = 0; break;}
                atributos.velocidad += efecto;
                break;
            case "critico":
                if((atributos.critico += efecto) < 0){atributos.critico = 0; break;}
                atributos.critico += efecto;
                break;
            case "defensa_fisica":
                if((atributos.defensa_fisica += efecto) < 0){atributos.defensa_fisica = 0; break;}
                atributos.defensa_fisica += efecto;
                break;
            case "defensa_magica":
                if((atributos.defensa_magica += efecto) < 0){atributos.defensa_magica = 0; break;}
                atributos.defensa_magica += efecto;
                break;
            default:
                break;
        }
    }

   public void Alteraciones_atributos(string atributo, string nombre_habilidad, float efecto, float duracion_efecto, bool reducir_atributo){
       //REVISAMOS SI YA TENEMOS EL BUFF PUESTO Y REDUCIMOS EL TIEMPO
        if (estado_alterado.ContainsKey(nombre_habilidad)){
            estado_alterado.Remove(nombre_habilidad);
            estado_alterado[nombre_habilidad] = new float[]{efecto, duracion_efecto};
            //SI EL TIEMPO LLEGA A CERO, QUITAMOS EL BUFF Y BAJAMOS EL ATRIBUTO
            if(estado_alterado[nombre_habilidad][1] <= 0){
                efecto = (reducir_atributo == false) ? -efecto : efecto;
                estado_alterado.Remove(nombre_habilidad);
                Modificar_atributo(efecto, atributo);
            }
        }else{
            //SI ES NUESTRA PRIMERA VEZ CON EL BUFF, SUBIMOS EL ATRIBUTO
            efecto = (reducir_atributo == true) ? -efecto : efecto;
            Modificar_atributo(efecto, atributo);
            estado_alterado[nombre_habilidad] = new float[]{efecto, duracion_efecto};
        }  
    }

    public void Revivir(){

    }

    public void Daño(float daño){
        //HACEMOS EL DAÑO EN LA SALUD
        atributos.salud -= daño;

        //SI LLEGA A 0 DE VIDA, TENDRA ESTADO MUERTO POR 999 TURNOS
        if (atributos.salud <= 0){
            estado_alterado["muerto"] = new float[]{0F, 9999F};
            Debug.Log(nombre + ", ha muerto");
        }
    }

    public float Hallar_daño_poder(Poderes poder, string tipo_ataque){
        float atributo_ = 0F;
        float daño;

        switch(poder.atributo){
            case "fuerza":
                atributo_ = atributos.fuerza;
                break;
            case "magia":
                atributo_ = atributos.magia;
                break;
            case "vitalidad":
                atributo_ = atributos.vitalidad;
                break;
            case "defensa_fisica":
                atributo_ = atributos.defensa_fisica;
                break;
            case "defensa_magica":
                atributo_ = atributos.defensa_magica;
                break;
            case "velocidad":
                atributo_ = atributos.velocidad;
                break;
            default:
                atributo_ = 1F;
                break;
        }

        if (tipo_ataque.Contains("ataque")){
            daño = (atributo_ * poder.multiplicador) + poder.daño_base;
        }else{
            daño = (atributo_ * poder.multiplicador_efecto) + poder.daño_base;
        }
        return daño;
    }

    public void Poder_usado(Poderes poder){
        poder.Usado();
    }

    public void Reducir_cooldown(){
        for(int i = 0; i < poderesActivos.Length; i++){
            poderesActivos[i].Reducir_reutilizacion();
        }
    }

}
