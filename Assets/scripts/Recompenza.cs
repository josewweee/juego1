using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recompenza : MonoBehaviour
{
    //TIPOS DE RECOMPENZA
    Monedas[] monedas;
    Equipo[] equipo;
    public string tipo_combate;
    int nivel_historia;

    //VARIABLES CON DATOS LOCALES
    public Usuario jugador;
    public storage_script storage;

    //TRAEMOS EL CRUD PARA MANEJO DE BD
    private crud CRUD;

    //PREFABS CON ITEMS PARA MOSTRAR EN LA UI
    public GameObject prefab_item;

    public void Awake()
    {
       jugador = Usuario.instancia;
       storage = storage_script.instancia;

       nivel_historia = storage.nivel_historia;
       tipo_combate = storage.tipo_combate;

        //INICIALIZAMOS EL MANEJO DE DB
        CRUD = GameObject.Find("Crud").GetComponent<crud>();
    }



    public void Recompenzas_ganar()
    {   
        if(tipo_combate == "historia")
        {
            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 100
            var rand = new System.Random();
            int oro = rand.Next(0, 101);
            int diamantes = 0;
            float y = 0F;
            //MOSTRAMOS LO GANADO DE ORO EN LA UI
            Mostrar_item_UI(-70F, y, prefab_item, "Oro", oro);

            //SI ESTAMOS EN UN NIVEL 10, 20, 30, ... TENDREMOS MAS PROBABILIDADES DE GANAR DIAMANTES, SI NO SOLO UN 18%
            int prob_diamantes = ( nivel_historia % 10 == 0)? rand.Next(0, 76): rand.Next(0, 101);
            if (prob_diamantes < 18){
                diamantes = prob_diamantes;
                y -= 83F;
                //MOSTRAMOS LO GANADO DE ORO EN LA UI
                Mostrar_item_UI(-70F, y, prefab_item, "Diamantes", diamantes);
            }

            //LE AGREGAMOS LO GANADO AL JUGADOR
            jugador.monedas.oro += oro;
            jugador.monedas.diamantes += diamantes;
            
            //LE DAMOS EXP A LOS PERSONAJES Y AL USUARIO
            int exp = (nivel_historia / 2) + 1;
            jugador.Sumar_experiencia(exp);
            
            foreach(Personajes pj in jugador.personajesFavoritos)
            {
                if (pj != null){
                    int index = jugador.personajes.FindIndex(item => item.nombre == pj.nombre);
                    jugador.personajes[index].Ganar_exp(exp);
                }
            }

            //SUMAMOS EL NIVEL DE HISTORIA
            jugador.SubirNivelHistoria(nivel_historia);

            //DAMOS FRAGMENTOS DE PERSONAJE, SEGUN LOS PERSONAJES QUE TENGA EL USUARIO
            List<Personajes> personajes = jugador.personajes;
            float pos_y = y - 83F;
            for(int i = 0; i < personajes.Count; i++)
            {
                int prob_fragmentos = rand.Next(0, 1000);
                switch(personajes[i].rareza)
                {
                    case "comun":
                        if (prob_fragmentos < 100){
                            jugador.personajes[i].fragmentos += 1;
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                    case "raro":
                        if (prob_fragmentos < 30){
                            jugador.personajes[i].fragmentos += 1;
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                    case "mitico":
                        if (prob_fragmentos < 10){
                            jugador.personajes[i].fragmentos += 1;
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                    case "legendario":
                        if (prob_fragmentos < 5){
                            jugador.personajes[i].fragmentos += 1;  
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                }
            }         
        }
        else if (tipo_combate == "pvp")
        {

            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 100
            var rand = new System.Random();
            int puntos_pvp = rand.Next(0, 101);
            int diamantes = 0;
            float y = 0F;
             //MOSTRAMOS LO GANADO DE PUNTOS PVP EN LA UI
            Mostrar_item_UI(-70F, y, prefab_item, "Puntos Pvp", puntos_pvp);

            //25% DE PROB DE GANAR DIAMANTES EN PVP
            int prob_diamantes = rand.Next(0, 76);
             if (prob_diamantes < 25){
                diamantes = prob_diamantes;
                y -= 83F;
                //MOSTRAMOS LO GANADO DE ORO EN LA UI
                Mostrar_item_UI(-70F, y, prefab_item, "Diamantes", diamantes);
            }
            jugador.monedas.puntos_pvp += puntos_pvp;
            jugador.monedas.diamantes += diamantes;

            //DAMOS FRAGMENTOS DE PERSONAJE, DOBLE DE POSIBILIDAD, SEGUN LOS PERSONAJES QUE TENGA EL USUARIO
            List<Personajes> personajes = jugador.personajes;
            float pos_y = y - 83F;
            for(int i = 0; i < personajes.Count; i++)
            {
                int prob_fragmentos = rand.Next(0, 1000);
                switch(personajes[i].rareza)
                {
                    case "comun":
                        if (prob_fragmentos < 200){
                            jugador.personajes[i].fragmentos += 1;
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                    case "raro":
                        if (prob_fragmentos < 60){
                            jugador.personajes[i].fragmentos += 1;
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                    case "mitico":
                        if (prob_fragmentos < 20){
                            jugador.personajes[i].fragmentos += 1;
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                    case "legendario":
                        if (prob_fragmentos < 10){
                            jugador.personajes[i].fragmentos += 1;  
                            //MOSTRAMOS LO GANADO DE ORO EN LA UI
                            Mostrar_item_UI(-70F, pos_y, prefab_item, "Fragmentos " + jugador.personajes[i].nombre, 1);
                            pos_y -= 83F;
                        }
                        break;
                }
            }            
        }

        //ENVIAMOS LA INFO NUEVA A LA DB
        Guardar_cambios(jugador);
        
    }

    public void Recompenzas_perder()
    {
        if(tipo_combate == "historia")
        {
            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 50
            var rand = new System.Random();
            int oro = rand.Next(0, 51);
            int diamantes = 0;
            float y = 0F;
            //MOSTRAMOS LO GANADO DE ORO EN LA UI
            Mostrar_item_UI(-70F, y, prefab_item, "Oro", oro);

            //SI ESTAMOS EN UN NIVEL 10, 20, 30, ... TENDREMOS MAS PROBABILIDADES DE GANAR DIAMANTES, SI NO SOLO UN 8%
            int prob_diamantes = ( nivel_historia % 10 == 0)? rand.Next(0, 76): rand.Next(0, 101);
            if (prob_diamantes < 8){
                diamantes = prob_diamantes;
                y -= -83F;
                //MOSTRAMOS LO GANADO DE ORO EN LA UI
                Mostrar_item_UI(-70F, y, prefab_item, "Diamantes", diamantes);
            }
            jugador.monedas.oro += oro;
            jugador.monedas.diamantes += diamantes;
            
            //LE DAMOS EXP A LOS PERSONAJES Y AL USUARIO
            int exp = (nivel_historia / 3) + 1;
            jugador.Sumar_experiencia(exp);

            foreach(Personajes pj in jugador.personajesFavoritos)
            {
                if (pj != null){
                    int index = jugador.personajes.FindIndex(item => item.nombre == pj.nombre);
                    jugador.personajes[index].Ganar_exp(exp);
                }
            }

            // for(int i = 0; i < jugador.personajesFavoritos.Count; i++)
            // {
            //     jugador.personajesFavoritos[i].Ganar_exp(exp);
            // }

        }
        else if (tipo_combate == "pvp")
        {

            //MIRAMOS UN VALOR ALEATORIO PARA EL ORO ENTRE 0 - 30
            var rand = new System.Random();
            int puntos_pvp = rand.Next(0, 31);
            int diamantes = 0;
            float y = 0F;
            //MOSTRAMOS LO GANADO DE PUNTOS PVP EN LA UI
            Mostrar_item_UI(-70F, y, prefab_item, "Puntos Pvp", puntos_pvp);


            //15% DE PROB DE GANAR DIAMANTES EN PVP
            int prob_diamantes = rand.Next(0, 76);
            if (prob_diamantes < 8){
                diamantes = prob_diamantes;
                y -= 83F;
                //MOSTRAMOS LO GANADO DE ORO EN LA UI
                Mostrar_item_UI(-70F, y, prefab_item, "Diamantes", diamantes);
            }
            jugador.monedas.puntos_pvp += puntos_pvp;
            jugador.monedas.diamantes += diamantes;
        }

        //ENVIAMOS LA INFO NUEVA A LA DB
        Guardar_cambios(jugador);
    }

    void Mostrar_item_UI(float x, float y, GameObject prefab, string item, int cantidad)
    {
        //INSTANCIAMOS EL ITEM
        GameObject item_ganado = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);

        //PONEMOS SU CANTIDAD
        GameObject txt_cantidad = item_ganado.transform.GetChild(0).gameObject;
        txt_cantidad.GetComponent<UnityEngine.UI.Text>().text = "x " + cantidad.ToString();

        //PONEMOS EL ITEM QUE SALIO
        GameObject txt_item = item_ganado.transform.GetChild(1).gameObject;
        txt_item.GetComponent<UnityEngine.UI.Text>().text = item;

        //PONEMOS LA IMAGEN
        string nombre_img = "";
        switch(item)
        {
            case "Oro":
                nombre_img = "Oro";
                break;
            case "Diamantes":
                nombre_img = "Diamantes";
                break;
            case "Puntos Pvp":
                nombre_img = "pvp";
                break;
            default: //OPCION DE LOS FRAGMENTOS
                nombre_img = "invocacion";
                break;
        }
        GameObject img_item = item_ganado.transform.GetChild(2).gameObject;
        img_item.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load <Sprite>("botones/" + nombre_img);

        //LO PONEMOS COMO HIJO DE LA VENTANA DE FIN JUEGO
        item_ganado.transform.SetParent(GameObject.Find("Fin_juego").transform, false);
    }

    private void Guardar_cambios(Usuario nuevo_val)
    {
        this.CRUD.Guardar_usuario(nuevo_val);
    }


}
