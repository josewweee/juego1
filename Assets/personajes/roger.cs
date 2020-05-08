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
        atributos = new Atributos(10f, 10f, 10f, 10f, 10f, 10f, 10f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[3];
        elemento = "tierra";
        estado_alterado = "null";
        rareza = "comun";
        Agregar_poderes();
        Activar_poderes();
    }


    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("golpe", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
        this.poderes[1] = new Poderes("golpe_1", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
        this.poderes[2] = new Poderes("golpe_2", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
        this.poderes[3] = new Poderes("golpe_3", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
        this.poderes[4] = new Poderes("golpe_4", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
        this.poderes[5] = new Poderes("golpe_5", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
        this.poderes[6] = new Poderes("golpe_6", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
        this.poderes[7] = new Poderes("golpe_7", "roger golpea al enemigo", "fisico", 1f, "normal", 0, "unico");
    }

    public void Activar_poderes()
    {
        this.poderesActivos[0] = poderes[2];
        this.poderesActivos[1] = poderes[3];
        this.poderesActivos[2] = poderes[0];
    }
}
