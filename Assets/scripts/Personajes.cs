using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personajes
{
    public string nombre;
    public int nivel;
    public int estrellas;
    public int despertadas;
    public Atributos atributos; // FUERZA, VITALIDAD, MAGIA, VELOCIDAD, CRITICO, DEF_FISICA, DEF_MAGICA
    public Equipo[] equipamiento;
    public int experiencia;
    public Poderes[] poderes; // 9
    public Poderes[] poderesActivos; // 4
    public string elemento; // AGUA, FUEGO, TIERRA, TRUENO, LUZ, OSCURIDAD
    public Dictionary<string, float[]> estado_alterado; // NOMBRE EFECTO -> [ DAÑO EFECTO, DURACION EFECTO ]
    public string rareza; // COMUN, RARO, MITICO, LEGENDARIO



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

    public void Subir_nivel(int niveles){
        nivel = niveles;
        experiencia = 0;
        atributos.fuerza += niveles;
        atributos.vitalidad += niveles;
        atributos.magia += niveles;
        atributos.velocidad += niveles;
        atributos.critico += niveles;
        atributos.defensa_fisica += niveles;
        atributos.defensa_magica += niveles;

        // string output = JsonUtility.ToJson(atributos, true);
        // Debug.Log(output);
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

    public void Modificar_atributo(float efecto, string atributo){
        switch(atributo){
            case "vitalidad":
                atributos.vitalidad += efecto;
                break;
            case "fuerza":
                atributos.fuerza += efecto;
                break;
            case "magia":
                atributos.magia += efecto;
                break;
            case "velocidad":
                atributos.velocidad += efecto;
                break;
            case "critico":
                atributos.critico += efecto;
                break;
            case "defensa_fisica":
                atributos.defensa_fisica += efecto;
                break;
            case "defensa_magica":
                atributos.defensa_magica += efecto;
                break;
            default:
                break;
        }
    }

   public void Alteraciones_atributos(string atributo, string nombre_habilidad, float efecto, float duracion_efecto, bool reducir_atributo){
        if (estado_alterado.ContainsKey(nombre_habilidad)){
            estado_alterado.Remove(nombre_habilidad);
            estado_alterado[nombre_habilidad] = new float[]{efecto, duracion_efecto};
            if(estado_alterado[nombre_habilidad][1] <= 0){
                efecto = (reducir_atributo == false) ? -efecto : efecto;
                estado_alterado.Remove(nombre_habilidad);
                Modificar_atributo(efecto, atributo);
            }
        }else{
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
        }
    }


}
