using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;

public class crear_personaje : MonoBehaviour
{
    
    //KEY DEL USUARIO EN EL LOCALSTORAGE
    private string KEY_JUGADOR ="KEY_JUGADOR";
    private Usuario usuario_nuevo;
    private FirebaseDatabase _database;
    public InputField nombre_usuario;

    private void Start() {
        _database = FirebaseDatabase.DefaultInstance;
    }


   public void Comenzar_tutorial()
    {
        //creamos un usuario nuevo
        usuario_nuevo = new Usuario();

        //ANTES HAY QUE REVISAR QUE EL NOMBRE NO EXISTA EN LA BASE DE DATOS CON UN IF AQUI
        usuario_nuevo.nombre = nombre_usuario.text.ToString();
        //LO GUARDAMOS EN LOCAL STORAGE
        PlayerPrefs.SetString(KEY_JUGADOR, JsonUtility.ToJson(usuario_nuevo)); 
        //PONEMOS LA RUTA CON SU NOMBRE
        KEY_JUGADOR += "/" + usuario_nuevo.nombre;
        //LO GUARDAMOS EN LA BASE DE DATOS
        _database.GetReference(KEY_JUGADOR).SetRawJsonValueAsync(JsonUtility.ToJson(usuario_nuevo));
        //viajamos al menu principal
        SceneManager.LoadScene("menu_principal");
    }
}
