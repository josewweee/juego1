using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class crear_personaje : MonoBehaviour
{
    
    //KEY DEL USUARIO EN EL LOCALSTORAGE
    private const string KEY_JUGADOR ="KEY_JUGADOR";
    private Usuario usuario_nuevo;

    public InputField nombre_usuario;



   public void Comenzar_tutorial()
    {
        //creamos un usuario nuevo y lo guardamos en el localStorage como un json
        usuario_nuevo = Usuario.instancia;

        //ANTES HAY QUE REVISAR QUE EL NOMBRE NO EXISTA EN LA BASE DE DATOS CON UN IF AQUI
        usuario_nuevo.nombre = nombre_usuario.text.ToString();
        PlayerPrefs.SetString(KEY_JUGADOR, JsonUtility.ToJson(usuario_nuevo));

        //viajamos al menu principal
        SceneManager.LoadScene("menu_principal");
    }
}
