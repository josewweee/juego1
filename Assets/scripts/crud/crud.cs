using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class crud : MonoBehaviour {

    //KEY DEL USUARIO EN EL LOCALSTORAGE
    private const string KEY_JUGADOR ="KEY_JUGADOR";

    //ENVIAMOS EL USUARIO QUE NOS MANDEN AL LOCAL STORAGE
    public void Guardar_usuario(Usuario jugador)
    {
        PlayerPrefs.SetString(KEY_JUGADOR, JsonUtility.ToJson(jugador));
    }

    // REVISAMOS SI EL USUARIO EXISTE Y LO RETORNAMOS DEL LOCALSTORAGE PARSEAMOS DE JSON A TIPO USUARIO, SI NO EXISTE RETORNAMOS NULL
    public Usuario Cargar_usuario()
    {
        if (Existe_jugador()){
            return JsonUtility.FromJson<Usuario>(PlayerPrefs.GetString(KEY_JUGADOR));
        }

        return null;
    }

    //MIRAMOS SI LA KEY DEL USUARIO EXISTE
    public bool Existe_jugador()
    {
        return PlayerPrefs.HasKey(KEY_JUGADOR);
    }
}
