using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alicia : Personajes
{
    public alicia()
    {
        nombre = "Alicia";
        nivel = 1;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(2f, 70f, 20f, 4f, 15f, 5f, 10f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4];
        elemento = "fuego";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "comun";
        Agregar_poderes();
        Activar_poderes();
    }


    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("incinerar", "alicia inciera al enemigo", "magia", 1f, 0f, "ataque", "fuego", 0, 0, "unico", true, new string[1]{"null"}, 10F);
        this.poderes[1] = new Poderes("congelar", "alicia congela a los enemigos", "magia", 0f, 0f, "debuff", "agua", 10, 2, "multiple", true, new string[1]{"congelar"}, 3F);
        this.poderes[2] = new Poderes("quemar", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 5, 3, "multiple", true, new string[1]{"quemar"}, 1F);
        this.poderes[3] = new Poderes("debuff", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[4] = new Poderes("debuff", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[5] = new Poderes("debuff", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[6] = new Poderes("debuff", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[7] = new Poderes("debuff", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
    }

    public void Activar_poderes()
    {
        this.poderesActivos[0] = poderes[0];
        this.poderesActivos[1] = poderes[1];
        this.poderesActivos[2] = poderes[2];
        this.poderesActivos[3] = poderes[3];
    }
}
