using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class routing : MonoBehaviour
{
    //VALORES PARA EL MENU
    private bool menu_activo;
    private GameObject boton_menu;
    private Text oro;
    private Text diamantes;
    private Text energia_max;
    private Text energia;

    // TRAEMOS LA INSTANCIA DEL USUARIO PARA PRUEBAS
    public Usuario jugador;

    // INSTANCIA DE STORAGE LOCAL
    public storage_script storage_singleton;


    void Start()
    {
        //HALLAMOS EL BOTON DEL MENU Y LO ACTIVAMOS
        boton_menu = GameObject.Find("contenedor");
        boton_menu.SetActive(false);
        menu_activo = false;

        //CREAMOS EL USUARIO Y EL STORAGE LOCAL
        jugador = Usuario.instancia;
        storage_singleton = storage_script.instancia;

        //ASIGNAMOS LOS TEXTOS DEL MENU
        try{
            oro = GameObject.Find("oro_valor").GetComponent<Text>();
            oro.text = jugador.monedas.oro.ToString();

            diamantes = GameObject.Find("diamantes_valor").GetComponent<Text>();
            diamantes.text = jugador.monedas.diamantes.ToString();

            energia = GameObject.Find("energia_valor").GetComponent<Text>();
            energia.text = jugador.energia.ToString();

            energia_max = GameObject.Find("energia_valor_maximo").GetComponent<Text>();
            energia_max.text = " / " + jugador.energia_maxima.ToString();
        }catch{
            
        }
        
    }

    void Update()
    {
        try{
            oro.text = jugador.monedas.oro.ToString();
            diamantes.text = jugador.monedas.diamantes.ToString();
            energia.text = jugador.energia.ToString();
            energia_max.text = " / " + jugador.energia_maxima.ToString();
        }
        catch{
            
        }
    }


    public void mostrar_menu()
    {
        if (menu_activo)
        {
            boton_menu.SetActive(false);
            menu_activo = false;
        }
        else
        {
            boton_menu.SetActive(true);
            menu_activo = true;
        }
    }

    public void ir_principal()
    {
        SceneManager.LoadScene("menu_principal");
    }

    public void ir_configuracion()
    {
        SceneManager.LoadScene("menu_configuracion");
    }

    public void ir_equipamiento()
    {
        SceneManager.LoadScene("menu_equipamiento");
    }

    public void ir_equipamiento_individual()
    {
        SceneManager.LoadScene("menu_equipamiento_individual");
    }

    public void ir_tienda()
    {
        SceneManager.LoadScene("menu_tienda");
    }

    public void ir_historia()
    {
        SceneManager.LoadScene("menu_historia");
    }

    public void ir_logros()
    {
        SceneManager.LoadScene("menu_logros");
    }

    public void ir_mis_personajes()
    {
        SceneManager.LoadScene("menu_mis_personajes");
    }

    public void ir_personaje_individual(Personajes personaje_objetivo)
    {
        storage_singleton.personaje = personaje_objetivo;
        SceneManager.LoadScene("menu_personaje_individual");
    }

    public void ir_pvp()
    {
        SceneManager.LoadScene("menu_pvp");
    }

    public void ir_seleccion_pre_combate()
    {
        SceneManager.LoadScene("menu_seleccion_pre_combate");
    }

    public void ir_invocacion()
    {
        SceneManager.LoadScene("menu_invocacion");
    }

    public void ir_combate(string tipoCombate){
        storage_singleton.tipo_combate = tipoCombate;
        if(tipoCombate == "historia")
        {  
            if(jugador.energia >= 6 )
            {
                jugador.energia -= 6;
                SceneManager.LoadScene("menu_combate");
            }else{
                Debug.Log("No hay energia suficiente");
            }
        }else if(tipoCombate == "pvp"){
            if ( jugador.energia_pvp >= 1)
            {
                jugador.energia_pvp --;
                SceneManager.LoadScene("menu_combate");
            }else{
                 Debug.Log("No hay energia suficiente");
            }
        }
        
    }
}
