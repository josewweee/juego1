using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu_amigos : MonoBehaviour
{
    //PARA MOVERNOS ENTRE VENTANAS
    private routing _routing;
    public GameObject menu;

    //PARA VER QUE USUARIOS SALDRAN EN PANTALLA Y CONTRA QUIEN PELEAREMOS
    private Usuario jugador;
    private storage_script storage_enemigos;
    public GameObject prefab_lista;
    private List<Amigos> jugadores_DB = new List<Amigos>();

    //private Personajes fabrica;

    //INSTANCIA DE LA DB Y DATOS NECESARIOS PARA USAR LA DB
    private crud CRUD;
    private string[] nombres_amigos;

    private IEnumerator Start()
    {
         _routing = menu.GetComponent<routing>();

         //TRAEMOS LAS INSTANCIAS DEL JUGADOR Y LOS ENEMIGOS
        jugador = Usuario.instancia;
        storage_enemigos = storage_script.instancia;

        //TRAEMOS LOS OTROS JUGADORES DE LA DB
        nombres_amigos = new string[jugador.amigos.Count];
        int i = 0;
        foreach(Amigos _amigo in jugador.amigos)
        {
            nombres_amigos[i] = _amigo.nombre;
            i++;
        }

        CRUD = GameObject.Find("Crud").GetComponent<crud>();
        var otros_jugadores = CRUD.GetComponent<crud>().Cargar_amigos(nombres_amigos);
        yield return new WaitUntil( ()=> otros_jugadores.IsCompleted);
        jugadores_DB = otros_jugadores.Result;
        Debug.Log(jugadores_DB.Count);


        //FUNCIONES DE LA VENTANA
        Actualizar_regalos_recibidos();
        Popular_lista_amigos(prefab_lista);
    }


    private void Popular_lista_amigos(GameObject prefab_lista)
    {
        float x = 2.200012F;
        float y = 120.7798F;
        foreach(Amigos u in jugadores_DB)
        {
            //INSTANCIAMOS EL USUARIO
            GameObject recuadro_amigo = Instantiate(prefab_lista, new Vector3(x, y, 0), Quaternion.identity);
            recuadro_amigo.transform.SetParent(GameObject.Find("contenido_amigos").transform, false);


            //NOMBRE DEL AMIGO
            Text nombre = recuadro_amigo.transform.GetChild(0).gameObject.GetComponent<Text>();
            nombre.text = u.nombre;


            //LE ASIGNAMOS LOS PERSONAJES AL SINGLETON PARA SIGUIENTE ESCENA, Y VAMOS AL COMBATE PVP
            Button btn = recuadro_amigo.transform.GetChild(4).gameObject.GetComponent<Button>();
            List<Personajes> defensa_amigo= u.defensa_pvp;
            btn.onClick.AddListener(delegate { Ir_combate_pvp(defensa_amigo); });

            //VEMOS LOS PERSONAJES DEL AMIGO
            Button btn_agregar_amigo = recuadro_amigo.transform.GetChild(3).gameObject.GetComponent<Button>();
             List<Personajes> pjs_amigo = u.personajes;
            btn.onClick.AddListener(delegate { Ver_personajes(pjs_amigo); });

            y -= 48F;
        }
    }

    //ENVIAMOS LA DEFENSA DEL ENEMIGO A UN SINGLETON LOCAL, MODIFICAMOS EL TIPO DE COMBATE Y VAMOS A PRE COMBATE
    public void Ir_combate_pvp(List<Personajes> pjs_enemigo)
    {
        storage_enemigos.enemigos_pvp = pjs_enemigo;
        storage_enemigos.tipo_combate = "amistoso";
        _routing.ir_seleccion_pre_combate();
    }

    public void Ver_personajes(List<Personajes> pjs)
    {
        //jugador.AgregarAmigos(amigo);
        //Guardar_DB(jugador);
    }

    //ENVIAMOS UN REGALO A TODOS LOS AMIGOS, ELLOS AHORA LO PUEDEN RECLAMAR
    public void Enviar_regalo(){
        jugador.regalo_enviado = true;
        Guardar_DB(jugador);
    }

    //RECLAMAMOS EL REGALO DE TODOS LOS AMIGOS QUE LO HAYAN ENVIADO
    public void Recibir_regalo(){
        foreach(Amigos a in jugadores_DB)
        {
            if(a.regalo_enviado == true && a.regalo_reclamado == false){
                jugador.monedas.monedas_amigos += 1;
                a.regalo_enviado = false;
                a.regalo_reclamado = true;
            }
        }
        Guardar_DB(jugador);
    }

    //ACTUALIZAMOS A LOS AMIGOS A LOS QUE YA LES RECLAME EL REGALO, YA QUE CUANDO LLEGAN DE LA DB, ESTE VALOR LLEGA COMO NO RECLAMADO
    public void Actualizar_regalos_recibidos(){
        for(int i = 0; i < this.jugadores_DB.Count; i++)
        {
            if(this.jugadores_DB[i].nombre == this.jugador.amigos[i].nombre)
            {
                this.jugadores_DB[i].regalo_reclamado = this.jugador.amigos[i].regalo_reclamado;
            }
        }
        this.jugador.amigos = jugadores_DB;
    }

    //ACTUALIZAMOS EL USUARIO EN LA DB
    public void Guardar_DB(Usuario nuevo_val){
        this.CRUD.Guardar_usuario(nuevo_val);
    }


}