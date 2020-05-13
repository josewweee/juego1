using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liliana : Personajes
{
    public liliana()
    {
        nombre = "liliana";
        nivel = 1;
        nivel_maximo = 40;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(3f, 100f, 15f, 6f, 8f, 5f, 7f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4];
        elemento = "agua";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "legendario";
        fragmentos = 0;
        Agregar_poderes();
        Activar_poderes();
    }

// nombre, descripcion, atributo, multiplicador, multiplicador_efecto, tipo_poder, tipo_elemento, reutilizacion, duracion_efecto, objetivos, se_puede_usar, habilidades, daño_base
    // LA DURACION DE LOS PODERES, VA CON 1 VALOR DEMAS DEL QUE DEBE DURAR
    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("torrente", "liliana ataca al enemigo", "magia", 1f, 0f, "ataque", "agua", 0, 0, "unico", true, new string[1]{"null"}, 10F);
        this.poderes[1] = new Poderes("congelar", "liliana congela a los enemigos", "magia", 0f, 0f, "debuff", "agua", 10, 3, "multiple", true, new string[1]{"congelar"}, 3F);
        this.poderes[2] = new Poderes("ola", "liliana lanza olas al enemigo", "magia", 1.2f, 0f, "ataque", "agua", 5, 0, "multiple", true, new string[1]{"null"}, 0F);
        this.poderes[3] = new Poderes("vida +","liliana aumenta la vida del grupo", "magia", 0f, 1f, "buff", "agua", 5, 3, "multiple", true, new string[1]{"aumentar_vitalidad"}, 0F);
        this.poderes[4] = new Poderes("xx", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"a"}, 0F);
        this.poderes[5] = new Poderes("xx", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"a"}, 0F);
        this.poderes[6] = new Poderes("xx", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"a"}, 0F);
        this.poderes[7] = new Poderes("xx", "liliana quema a los enemigos", "magia", 0f, 0.1f, "debuff", "agua", 3, 2, "multiple", true, new string[1]{"a"}, 0F);
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
