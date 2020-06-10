using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invocar : MonoBehaviour
{
    // INSTANCIAS DEL PERSONAJE Y DE LA FABRICA DE PERSONAJES
    public Usuario jugador;
    public Usuario singleton;
    public Personajes fabrica_personajes;
    Personajes personaje_invocado;

    //AGREGAMOS LOS NOMBRES DE LOS PERSONAJES QUE SE PUEDEN INVOCAR
    List<string> comunes = new List<string>();
    List<string> raros = new List<string>();
    List<string> miticos = new List<string>();
    List<string> legendarios = new List<string>();

    //AGREGAMOS LA IMAGEN DEL PERSONAJE INVOCADO
    GameObject item_personaje_invocado;
    GameObject txt_personaje_invocado;
    GameObject img_personaje_invocado;

    //TRAEMOS INSTANCIA DEL CRUD PARA LAS OPERACIONES DE LA BASE DE DATOS
    private crud CRUD;

    //TRAEMOS LA MECANICA DE LOS LOGROS
    private mecanicas_logros LOGROS;

    private IEnumerator Start()
    {
        singleton = Usuario.instancia;
        //TRAEMOS EL USUARIO DE LA BASE DE DATOS
        CRUD = GameObject.Find("Crud").GetComponent<crud>();
        var jugador_task = CRUD.GetComponent<crud>().Cargar_usuario();
        yield return new WaitUntil( ()=> jugador_task.IsCompleted);
        jugador = jugador_task.Result;

        //FABRICA PARA INVOCAR PERSONAJES Y SINGLETON USUARIO PARA AGREGARLE LOS PERSONAJES
        fabrica_personajes = new Personajes();
        
        //LOGROS
        LOGROS = new mecanicas_logros(jugador);

        //COMENZAMOS A AGREGAR LOS PERSONAJES QUE SE PUEDEN INVOCAR
        comunes.Add("roger");
        raros.Add("alicia");
        miticos.Add("martis");
        legendarios.Add("liliana");
        
        //ASIGNAMOS Y ELIMINAMOS TEMPORALMENTE LA IMG_PERSONAJE INVOCADO
        item_personaje_invocado = GameObject.Find("personaje_invocado");
        img_personaje_invocado = item_personaje_invocado.transform.GetChild(1).gameObject;
        txt_personaje_invocado = item_personaje_invocado.transform.GetChild(2).gameObject;
        item_personaje_invocado.SetActive(false);


        
    }


    public void Invocacion_normal()
    {
        if (jugador.monedas.invocaciones_normales < 1 && jugador.monedas.oro - 1000 <= 0) return;
        if (jugador.monedas.invocaciones_normales >= 1)
        {
            jugador.monedas.invocaciones_normales -= 1;
        }
        else
        {
            jugador.monedas.oro -= 1000;
        }

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

        //REVISAMOS LOGROS DE INVOCACIONES
        LOGROS.RevisarOtros(personaje_invocado, false);

        //REVISAMOS SI EL JUGADOR YA TIENE EL PERSONAJE
        Personajes ya_obtenido = null;
        ya_obtenido = jugador.personajes.Find( x => x.nombre == personaje_invocado.nombre );
        if(ya_obtenido == null)
        {
            //AGREGAMOS EL PERSONAJE AL JUGADOR
            jugador.Agregar_personaje(personaje_invocado);

            //ACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
            Abrir_item_personaje_invocado(false);

        
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

            //ACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
            Abrir_item_personaje_invocado(true);
        }
        Guardar_cambios(jugador);
    }

    public void Invocacion_especial()
    {
        if (jugador.monedas.invocaciones_normales < 1 && jugador.monedas.diamantes - 100 <= 0) return;
        if (jugador.monedas.invocaciones_normales >= 1)
        {
            jugador.monedas.invocaciones_raras -= 1;
        }
        else
        {
            jugador.monedas.diamantes -= 100;
        }
        
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

        //REVISAMOS LOGROS DE INVOCACIONES
        LOGROS.RevisarOtros(personaje_invocado, false);

         //AGREGAMOS EL PERSONAJE INVOCADO AL JUGADOR
        Personajes ya_obtenido = null;
        ya_obtenido = jugador.personajes.Find( x => x.nombre == personaje_invocado.nombre );
        if(ya_obtenido == null)
        {
            jugador.Agregar_personaje(personaje_invocado);
            
            //ACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
            Abrir_item_personaje_invocado(false);
        
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

            //ACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
            Abrir_item_personaje_invocado(true);
        }
        

        Debug.Log(personaje_invocado.nombre);
        Guardar_cambios(jugador);
    }

    public void Invocacion_Legendaria()
    {
        if (jugador.monedas.invocaciones_normales < 1 && jugador.monedas.diamantes - 1000 <= 0) return;
        if (jugador.monedas.invocaciones_legendarias >= 1)
        {
            jugador.monedas.invocaciones_legendarias -= 1;
        }
        else
        {
            jugador.monedas.diamantes -= 1000;
        }

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

        //REVISAMOS LOGROS DE INVOCACIONES
        LOGROS.RevisarOtros(personaje_invocado, false);

         //AGREGAMOS EL PERSONAJE INVOCADO AL JUGADOR
        Personajes ya_obtenido = null;
        ya_obtenido = jugador.personajes.Find( x => x.nombre == personaje_invocado.nombre );
        if(ya_obtenido == null)
        {
            jugador.Agregar_personaje(personaje_invocado);

            //ACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
            Abrir_item_personaje_invocado(false);
        
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

            //ACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
            Abrir_item_personaje_invocado(true);
        }
        

        Debug.Log(personaje_invocado.nombre);
        Guardar_cambios(jugador);
    }

    public void Abrir_item_personaje_invocado(bool fragmentos)
    {
        //ACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
        item_personaje_invocado.SetActive(true);

        //NOMBRE DEL INVOCADO
        if (!fragmentos) txt_personaje_invocado.GetComponent<Text>().text = personaje_invocado.nombre;
        else txt_personaje_invocado.GetComponent<Text>().text = "Fragmentos de, " + personaje_invocado.nombre;

        //IMAGEN DEL INVOCADO
        Sprite[] sprites = Resources.LoadAll<Sprite>("img_personajes/" + personaje_invocado.imagen_completa[0]);
        int index_imagen = int.Parse(personaje_invocado.imagen_completa[1]);
        img_personaje_invocado.GetComponent<Image>().sprite = sprites[index_imagen];

    }

    public void Cerrar_item_personaje_invocado()
    {
        //DESACTIVAMOS EL ITEM DEL PERSONAJE INVOCADO
        item_personaje_invocado.SetActive(false);
    }

    private void Guardar_cambios(Usuario nuevo_val)
    {
        this.CRUD.Guardar_usuario(nuevo_val);
        this.singleton.Actualizar_usuario(nuevo_val);
    }
    
}
