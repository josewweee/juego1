using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class daily_login : MonoBehaviour
{
    //PANEL DEL DAILY LOGIN
    GameObject panel_daily;

    // TRAEMOS LA INSTANCIA DEL USUARIO LLAMADA SINGLETON, EL CRUD PARA LA BD Y UN JUGADOR PARA ASIGNARLE LO DE LA BD
    public Usuario singleton;
    public Usuario jugador;
    private crud CRUD;

    //ESTRUCTURA CON LOS DATOS DEL DAILY
    private List<crud.daily_item> daily_items;

    public IEnumerator Start()
    {
        //TRAEMOS EL OJETO DEL UI
        panel_daily = GameObject.Find("daily_login_panel");

        //TRAEMOS EL USUARIO DE LA BASE DE DATOS
        CRUD = GameObject.Find("Crud").GetComponent<crud>();
        var jugador_task = CRUD.GetComponent<crud>().Cargar_usuario();
        yield return new WaitUntil( ()=> jugador_task.IsCompleted);
        jugador = jugador_task.Result;

        //TRAEMOS LOS DAILY DE LA BASE DE DATOS
        var daily_task = CRUD.GetComponent<crud>().Cargar_dailys();
        yield return new WaitUntil( ()=> daily_task.IsCompleted);
        daily_items = daily_task.Result;

        //ACTUALIZAMOS EL SINGLETON LOCAL CON LOS DATOS DE LA BD
        this.singleton.Actualizar_usuario(jugador);

        //SI YA RECLAMAMOS EL DAILY DEL DIA, CERRAR LA VENTANA
        if(this.singleton.daily_reclamado == true)
        {
            Cerrar_ventana();
        }

        //ACTUALIZAMOS LOS DAILY CON LA DB
        Actualizar_dailys();

        //ACTIVAMOS LOS DAILY QUE LE CORRESPONDEN AL USUAIRO, SEGUN LOS QUE LLEVE ACOMULADOR
        Activar_botones();
    }

    //CERRAMOS Y ABRIRMOS LA VENTANA DEL DAILY
    public void Cerrar_ventana()
    {
        panel_daily.SetActive(false);
    }

    public void Abrir_ventana()
    {
        panel_daily.SetActive(true);
    }

    //DESACTIVAMOS LOS BOTONES QUE NO CORRESPONDEN AL DAILY DEL USUARIO O SI EL USUARIO YA LO RECLAMO
    private void Activar_botones()
    {
        int dia_login = (this.singleton.login_diarios - 1);
        for(int i = 0; i < 7; i++)
        {
            GameObject item_daily = GameObject.Find("item_daily_" + i);
            if(dia_login == i && this.singleton.daily_reclamado == false)
            {
                item_daily.transform.GetChild(1).GetComponent<Button>().interactable = true;
                break;
            }
        }
    }

    //LO QUE SUCEDERA AL RECLAMAR UN DAILY, LE AGREGAREMOS LO QUE ESTE AL USUARIO
    //AUMENTAMOS LOS DAILY QUE LLEVA Y LO PONDREMOS COMO QUE YA RECLAMO POR HOY
    //GUARDAMOS LOS CAMBIOS EN LA DB
    public void Reclamar_daily()
    {
        int dia_login = (this.singleton.login_diarios - 1);
        GameObject item_daily = GameObject.Find("item_daily_" + dia_login);
        GameObject btn_component = item_daily.transform.GetChild(1).gameObject;
        string item_recompenza = item_daily.transform.GetChild(3).gameObject.name;
        Text txt_daily = btn_component.transform.GetChild(1).GetComponent<Text>();
        string txt_cantidad = txt_daily.text;
        int cantidad_daily = int.Parse(txt_cantidad.Substring(1));

        if(item_recompenza.Contains("oro")){
            this.singleton.monedas.oro += cantidad_daily;
        }
        else if(item_recompenza.Contains("diamante")){
            this.singleton.monedas.diamantes += cantidad_daily;
        }
        else if(item_recompenza.Contains("pvp")){
            this.singleton.monedas.puntos_pvp += cantidad_daily;
        }
        else if(item_recompenza.Contains("amigo")){
            this.singleton.monedas.monedas_amigos += cantidad_daily;
        }
        else if(item_recompenza.Contains("invocar")){
            //this.singleton.monedas.monedas_amigos += cantidad_daily;
        }
        else if (item_recompenza.Contains("fragmentos")){
            string tipo_frag = item_recompenza.Substring(11);
            foreach(Personajes pj in this.singleton.personajes)
            {
                if (pj.nombre.Contains(tipo_frag))
                {
                    pj.fragmentos += cantidad_daily;    
                }
            }
        }
        btn_component.GetComponent<Button>().interactable = false;
        this.singleton.Aumentar_login_reclamados();
        this.singleton.daily_reclamado = true;
        Guardar_cambios(this.singleton);
    }


    //ACTUALIZAMOS LA UI CON LOS VALORES DE LA DB
    public void Actualizar_dailys()
    {
        int i = 0;
        foreach(crud.daily_item d in this.daily_items)
        {
            GameObject item_daily = GameObject.Find("item_daily_" + i);
            GameObject child2 = item_daily.transform.GetChild(1).gameObject;
            Image img_daily = child2.transform.GetChild(0).gameObject.GetComponent<Image>();
            img_daily.sprite =  Resources.Load <Sprite>("botones/" + d.foto);
            Text txt_cantidad = child2.transform.GetChild(1).gameObject.GetComponent<Text>();
            txt_cantidad.text = "x" + d.cantidad.ToString();

            GameObject item_titulo = item_daily.transform.GetChild(3).gameObject;
            item_titulo.name = d.nombre;
            Text txt_titulo = item_titulo.GetComponent<Text>();
            txt_titulo.text = d.titulo;


            i++;
        }
    }

    //GUARDAMOS TODO EN LA DB
    private void Guardar_cambios(Usuario nuevo_val)
    {
        this.CRUD.Guardar_usuario(nuevo_val);
    }
}
