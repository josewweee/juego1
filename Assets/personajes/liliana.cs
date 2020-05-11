using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liliana : Personajes
{
    public liliana()
    {
        nombre = "liliana";
        nivel = 1;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(3f, 120f, 15f, 6f, 8f, 5f, 7f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4];
        elemento = "agua";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "comun";
        Agregar_poderes();
        Activar_poderes();
    }

// nombre, descripcion, atributo, multiplicador, multiplicador_efecto, tipo_poder, tipo_elemento, reutilizacion, duracion_efecto, objetivos, se_puede_usar, habilidades, daño_base
    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("torrente", "liliana ataca al enemigo", "magia", 1f, 0f, "ataque", "agua", 0, 0, "unico", true, new string[1]{"null"}, 10F);
        this.poderes[1] = new Poderes("congelar", "liliana congela a los enemigos", "magia", 0f, 0f, "debuff", "agua", 10, 2, "multiple", true, new string[1]{"congelar"}, 3F);
        this.poderes[2] = new Poderes("vida +", "liliana aumenta la vitalidad del grupo", "magia", 0f, 1f, "buff", "agua", 5, 3, "multiple", true, new string[1]{"aumentar_vitalidad"}, 0F);
        this.poderes[3] = new Poderes("magia +","liliana aumenta la magia del grupo", "magia", 0f, 1f, "buff", "agua", 5, 3, "multiple", true, new string[1]{"aumentar_magia"}, 0F);
        this.poderes[4] = new Poderes("debuff", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[5] = new Poderes("debuff", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[6] = new Poderes("debuff", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[7] = new Poderes("debuff", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
    }

    public void Activar_poderes()
    {
        this.poderesActivos[0] = poderes[0];
        this.poderesActivos[1] = poderes[1];
        this.poderesActivos[2] = poderes[2];
        this.poderesActivos[3] = poderes[3];
    }
}
