using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invocar : MonoBehaviour
{
    // INSTANCIAS DEL PERSONAJE Y DE LA FABRICA DE PERSONAJES
    public Usuario jugador;
    public Personajes fabrica_personajes;
    Personajes personaje_invocado;
    List<string> comunes = new List<string>();
    List<string> raros = new List<string>();
    List<string> miticos = new List<string>();
    List<string> legendarios = new List<string>();

    void Start()
    {
        fabrica_personajes = new Personajes();
        jugador = Usuario.instancia;
        comunes.Add("roger");
        raros.Add("alicia");
        miticos.Add("martis");
        legendarios.Add("liliana");
    }


    public void Invocacion_normal()
    {
        var rand = new System.Random();
        int tipo_invocacion = rand.Next(1001);
        
        //MIRAMOS QUE RAREZA DE PERSONAJE SALDRA Y ESCOGEMOS UNO ALEATORIO DEL ARREGLO DE PERSONAJES
        if (tipo_invocacion < 5)
        {   
            int num_pj_rareza = legendarios.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(legendarios[pj_invocacion]);
        }else if(tipo_invocacion < 10){
            int num_pj_rareza = miticos.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(miticos[pj_invocacion]);
        }else if(tipo_invocacion < 80){
            int num_pj_rareza = raros.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(raros[pj_invocacion]);
        }else{
            int num_pj_rareza = comunes.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(comunes[pj_invocacion]);
        }

        //AGREGAMOS EL PERSONAJE INVOCADO AL JUGADOR
        Personajes ya_obtenido = null;
        ya_obtenido = jugador.personajes.Find( x => x.nombre == personaje_invocado.nombre );
        if(ya_obtenido == null)
        {
            jugador.Agregar_personaje(personaje_invocado);
        
        // SI YA LO TIENE, LE AGREGAMOS FRAGMENTOS
        }else{
            int fragmentos = 0;
            switch(personaje_invocado.rareza)
            {
                case "comun":
                    fragmentos += 10;
                    break;
                case "raro":
                    fragmentos += 20;
                    break;
                case "mitico":
                    fragmentos += 30;
                    break;
                case "legendario":
                    fragmentos += 40;
                    break;
            }

            int index = 0;
            foreach(Personajes pj in jugador.personajes)
            {
                if (pj.nombre == personaje_invocado.nombre) break;
                index ++;
            }

            jugador.personajes[index].fragmentos += fragmentos;
        }
        

        Debug.Log(personaje_invocado.nombre);
    }

    public void Invocacion_especial()
    {
        var rand = new System.Random();
        int tipo_invocacion = rand.Next(1001);
        
        //MIRAMOS QUE RAREZA DE PERSONAJE SALDRA Y ESCOGEMOS UNO ALEATORIO DEL ARREGLO DE PERSONAJES
        if (tipo_invocacion < 10)
        {   
            int num_pj_rareza = legendarios.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(legendarios[pj_invocacion]);
        }else if(tipo_invocacion < 80){
            int num_pj_rareza = miticos.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(miticos[pj_invocacion]);
        }else{
            int num_pj_rareza = raros.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(raros[pj_invocacion]);
        }
         //AGREGAMOS EL PERSONAJE INVOCADO AL JUGADOR
        Personajes ya_obtenido = null;
        ya_obtenido = jugador.personajes.Find( x => x.nombre == personaje_invocado.nombre );
        if(ya_obtenido == null)
        {
            jugador.Agregar_personaje(personaje_invocado);
        
        // SI YA LO TIENE, LE AGREGAMOS FRAGMENTOS
        }else{
            int fragmentos = 0;
            switch(personaje_invocado.rareza)
            {
                case "comun":
                    fragmentos += 10;
                    break;
                case "raro":
                    fragmentos += 20;
                    break;
                case "mitico":
                    fragmentos += 30;
                    break;
                case "legendario":
                    fragmentos += 40;
                    break;
            }

            int index = 0;
            foreach(Personajes pj in jugador.personajes)
            {
                if (pj.nombre == personaje_invocado.nombre) break;
                index ++;
            }

            jugador.personajes[index].fragmentos += fragmentos;
        }
        

        Debug.Log(personaje_invocado.nombre);
    }

    public void Invocacion_Legendaria()
    {
        var rand = new System.Random();
        int tipo_invocacion = rand.Next(1001);
        
        //MIRAMOS QUE RAREZA DE PERSONAJE SALDRA Y ESCOGEMOS UNO ALEATORIO DEL ARREGLO DE PERSONAJES
        if (tipo_invocacion < 80)
        {   
            int num_pj_rareza = legendarios.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(legendarios[pj_invocacion]);
        }else{
            int num_pj_rareza = miticos.Count;
            int pj_invocacion = rand.Next(num_pj_rareza);
            personaje_invocado = fabrica_personajes.Crear_personaje(miticos[pj_invocacion]);
        }

         //AGREGAMOS EL PERSONAJE INVOCADO AL JUGADOR
        Personajes ya_obtenido = null;
        ya_obtenido = jugador.personajes.Find( x => x.nombre == personaje_invocado.nombre );
        if(ya_obtenido == null)
        {
            jugador.Agregar_personaje(personaje_invocado);
        
        // SI YA LO TIENE, LE AGREGAMOS FRAGMENTOS
        }else{
            int fragmentos = 0;
            switch(personaje_invocado.rareza)
            {
                case "comun":
                    fragmentos += 10;
                    break;
                case "raro":
                    fragmentos += 20;
                    break;
                case "mitico":
                    fragmentos += 30;
                    break;
                case "legendario":
                    fragmentos += 40;
                    break;
            }

            int index = 0;
            foreach(Personajes pj in jugador.personajes)
            {
                if (pj.nombre == personaje_invocado.nombre) break;
                index ++;
            }

            jugador.personajes[index].fragmentos += fragmentos;
        }
        

        Debug.Log(personaje_invocado.nombre);
    }
}
