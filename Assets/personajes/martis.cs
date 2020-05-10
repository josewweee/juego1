using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class martis : Personajes
{
    public martis()
    {
        nombre = "martis";
        nivel = 1;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(16f, 60f, 2f, 12f, 12f, 5f, 7f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4];
        elemento = "trueno";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "comun";
        Agregar_poderes();
        Activar_poderes();
    }


    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("rayo", "martis lanza un rayo al enemigo", "fuerza", 1f, 0f, "ataque", "trueno", 0, 0, "unico", true, new string[1]{"null"}, 10F);
        this.poderes[1] = new Poderes("electrocutar", "martis electrocuta a los enemigos", "fuerza", 0f, 0.3f, "debuff", "trueno", 10, 2, "multiple", true, new string[1]{"congelar"}, 3F);
        this.poderes[2] = new Poderes("quemar", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 5, 3, "multiple", true, new string[1]{"quemar"}, 1F);
        this.poderes[3] = new Poderes("debuff", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[4] = new Poderes("debuff", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[5] = new Poderes("debuff", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[6] = new Poderes("debuff", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
        this.poderes[7] = new Poderes("debuff", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"curar"}, 0F);
    }

    public void Activar_poderes()
    {
        this.poderesActivos[0] = poderes[0];
        this.poderesActivos[1] = poderes[1];
        this.poderesActivos[2] = poderes[2];
        this.poderesActivos[3] = poderes[3];
    }
}
