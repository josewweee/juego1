using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roger : Personajes
{
    public roger()
    {
        nombre = "Roger";
        nivel = 1;
        nivel_maximo = 40;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(7f, 200f, 7f, 7f, 10f, 10f, 5f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4]{null, null, null, null};
        elemento = "tierra";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "comun";
        fragmentos = 0;
        imagen_completa = new string[2]{"PackForest01","7"};
        Agregar_poderes();
        Activar_poderes();
    }

// nombre, descripcion, atributo, multiplicador, multiplicador_efecto, tipo_poder, tipo_elemento, reutilizacion, duracion_efecto, objetivos, se_puede_usar, habilidades, daño_base
    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("golpe", "roger golpea al enemigo", "fuerza", 1f, 0f, "ataque", "tierra", 0, 0, "unico", true, new string[1]{"null"}, 10F, "golpe");
        this.poderes[1] = new Poderes("curar", "roger cura a los aliados", "magia", 0f, 1f, "buff", "tierra", 2, 0, "multiple", true, new string[1]{"curacion"}, 10F, "curar");
        this.poderes[2] = new Poderes("noquear", "roger noquea el objetivo", "fuerza", 1f, 0f, "ataque_debuff", "tierra", 5, 3, "unico", true, new string[1]{"aturdir"}, 10F, "noquear");
        this.poderes[3] = new Poderes("locura", "roger aumenta mucho su daño y baja su defensa", "fuerza", 0f, 2f, "buff", "tierra", 6, 3, "propio", true, new string[1]{"locura"}, 0F, "locura");
        this.poderes[4] = new Poderes("x1", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[5] = new Poderes("x2", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[6] = new Poderes("x3", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[7] = new Poderes("x4", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
    }

    public void Activar_poderes()
    {
        this.poderesActivos[0] = poderes[0];
        this.poderesActivos[1] = poderes[1];
        this.poderesActivos[2] = poderes[2];
        this.poderesActivos[3] = poderes[3];
    }


    public override void Subir_nivel(int niveles){
        nivel += niveles;
         for(int i = 0; i < niveles; i++)
        {
            experiencia = 0;
            atributos.fuerza += 1F;
            atributos.vitalidad += 10F;
            atributos.magia += 0.8F;
            atributos.velocidad += 1F;
            atributos.critico += 0.3F;
            atributos.defensa_fisica += 0.3F;
            atributos.defensa_magica += 0.3F;
        }
    }
}
