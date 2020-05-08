using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class routing : MonoBehaviour
{
    private bool menu_activo;
    private GameObject boton_menu;
    // TRAEMOS LA INSTANCIA DEL USUARIO PARA PRUEBAS
    public Usuario usuario_nuevo;
    // INSTANCIA DE PERSONA O EQUIPO A BUSCAR
    public storage_script equipo_personaje_objetivo;

    void Start()
    {
        boton_menu = GameObject.Find("contenedor");
        menu_activo = true;
        usuario_nuevo = Usuario.instancia;
        equipo_personaje_objetivo = storage_script.instancia;
        Debug.Log(usuario_nuevo.personajesFavoritos); // IMPRIMIMOS EL NOMBRE PARA PRUEBAS
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

    public void ir_personaje_individual(string personaje_objetivo)
    {
        equipo_personaje_objetivo.personaje = personaje_objetivo;
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
}
