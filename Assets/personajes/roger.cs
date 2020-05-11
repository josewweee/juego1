using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roger : Personajes
{
    public roger()
    {
        nombre = "Roger";
        nivel = 1;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(10f, 100f, 2f, 7f, 10f, 10f, 5f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4];
        elemento = "tierra";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "comun";
        Agregar_poderes();
        Activar_poderes();
    }

// nombre, descripcion, atributo, multiplicador, multiplicador_efecto, tipo_poder, tipo_elemento, reutilizacion, duracion_efecto, objetivos, se_puede_usar, habilidades, daño_base
    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("golpe", "roger golpea al enemigo", "fuerza", 1f, 0f, "ataque", "tierra", 0, 0, "unico", true, new string[1]{"null"}, 10F);
        this.poderes[1] = new Poderes("curar", "roger cura a los aliados", "magia", 0f, 0.5f, "buff", "tierra", 2, 0, "multiple", true, new string[1]{"curacion"}, 10F);
        this.poderes[2] = new Poderes("revitalizar", "roger cura en el tiepo a los aliados", "magia", 0f, 1f, "buff", "tierra", 5, 3, "multiple", true, new string[1]{"revitalizar"}, 1F);
        this.poderes[3] = new Poderes("debuff", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[4] = new Poderes("debuff", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[5] = new Poderes("debuff", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[6] = new Poderes("debuff", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[7] = new Poderes("debuff", "roger quema a los enemigos", "magia", 0f, 0.1f, "debuff", "tierra", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
    }

    public void Activar_poderes()
    {
        this.poderesActivos[0] = poderes[0];
        this.poderesActivos[1] = poderes[1];
        this.poderesActivos[2] = poderes[2];
        this.poderesActivos[3] = poderes[3];
    }
}
