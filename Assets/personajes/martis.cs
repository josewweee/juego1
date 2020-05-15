using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class martis : Personajes
{
    public martis()
    {
        nombre = "martis";
        nivel = 1;
        nivel_maximo = 40;
        estrellas = 0;
        despertadas = 0;
        atributos = new Atributos(16f, 60f, 2f, 8f, 12f, 5f, 7f);
        equipamiento = new Equipo[1];
        experiencia = 0;
        poderes = new Poderes[8];
        poderesActivos = new Poderes[4];
        elemento = "trueno";
        estado_alterado = new Dictionary<string, float[]>();
        rareza = "mitico";
        fragmentos = 0;
        imagen_completa = "51";
        Agregar_poderes();
        Activar_poderes();
    }

// nombre, descripcion, atributo, multiplicador, multiplicador_efecto, tipo_poder, tipo_elemento, reutilizacion, duracion_efecto, objetivos, se_puede_usar, habilidades, daño_base
    public void Agregar_poderes()
    {
        this.poderes[0] = new Poderes("rayo", "martis lanza un rayo al enemigo", "fuerza", 1f, 0f, "ataque", "trueno", 0, 0, "unico", true, new string[1]{"null"}, 10F, "rayo");
        this.poderes[1] = new Poderes("tormenta", "martis electrocuta a los enemigos", "fuerza", 0f, 1f, "debuff", "trueno", 10, 3, "multiple", true, new string[1]{"quemar"}, 3F, "tormenta");
        this.poderes[2] = new Poderes("veneno", "martis envenena a los enemigos", "fuerza", 0f, 1f, "debuff", "trueno", 5, 3, "multiple", true, new string[1]{"quemar"}, 1F, "veneno");
        this.poderes[3] = new Poderes("emboscar", "martis ataca a los enemigos", "fuerza", 1.2f, 0f, "ataque", "trueno", 3, 0, "unico", true, new string[1]{"null"}, 0F, "emboscar");
        this.poderes[4] = new Poderes("xx", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[5] = new Poderes("xx", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[6] = new Poderes("xx", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
        this.poderes[7] = new Poderes("xx", "martis quema a los enemigos", "magia", 0f, 0.1f, "debuff", "trueno", 3, 2, "multiple", true, new string[1]{"a"}, 0F, "revivir");
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
        experiencia = 0;
         for(int i = 0; i < niveles; i++)
        {
            atributos.fuerza += 1F;
            atributos.vitalidad += 10F;
            atributos.magia += 0F;      
            atributos.velocidad += 1.3F;      
            atributos.critico += 0.6F;
            atributos.defensa_fisica += 0.1F;
            atributos.defensa_magica += 0.1F;
        }
    }
}
