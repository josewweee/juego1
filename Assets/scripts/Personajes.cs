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
