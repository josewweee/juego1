using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alicia : Personajes
{
    public alicia()
    {
        nombre = "Alicia";
        nivel = 1;
        nivel_maximo = 40;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(2f, 70f, 20f, 4f, 15f, 5f, 10f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4];
        elemento = "fuego";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "raro";
        fragmentos = 0;
        imagen_completa = new string[2]{"PackForest01","3"};
        foto_perfil = "alicia_perfil";
        Agregar_poderes();
        Activar_poderes();
    }

// nombre, descripcion, atributo, multiplicador, multiplicador_efecto, tipo_poder, tipo_elemento, reutilizacion, duracion_efecto, objetivos, se_puede_usar, habilidades, daño_base
    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("incinerar", "alicia inciera al enemigo", "magia", 1f, 0f, "ataque", "fuego", 0, 0, "unico", true, new string[1]{"null"}, 10F, "incinerar");
        this.poderes[1] = new Poderes("quemadura", "alicia quema al enemigo", "magia", 1f, 0.5f, "ataque_debuff", "fuego", 5, 4, "unico", true, new string[1]{"quemar"}, 3F, "quemadura");
        this.poderes[2] = new Poderes("vida +", "alicia aumenta las estadisticas del grupo", "magia", 0f, 2f, "buff", "fuego", 5, 4, "multiple", true, new string[1]{"aumentar_estadisticas"}, 0F, "vida +");
        this.poderes[3] = new Poderes("meteoro", "alicia quema a los enemigos", "magia", 0f, 1f, "debuff", "fuego", 3, 3, "multiple", true, new string[1]{"quemar"}, 10F, "meteoro");
        this.poderes[4] = new Poderes("xx", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[5] = new Poderes("xx", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[6] = new Poderes("xx", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "meteoro");
        this.poderes[7] = new Poderes("xx", "alicia quema a los enemigos", "magia", 0f, 0.1f, "debuff", "fuego", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
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
            atributos.fuerza += 0F;
            atributos.vitalidad += 10F;
            atributos.magia += 2F;
            atributos.velocidad += 1F;
            atributos.critico += 0.3F;
            atributos.defensa_fisica += 0.1F;
            atributos.defensa_magica += 0.1F;
        }
    }
}
