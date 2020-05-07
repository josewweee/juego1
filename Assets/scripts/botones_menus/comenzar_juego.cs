using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class comenzar_juego : MonoBehaviour
{

    //instancia de un usuario totalmente nuevo
    public Usuario usuario_nuevo;

    void Start()
    {

    }

    public void ir_principal()
    {
        //creamos un usuario nuevo
        usuario_nuevo = Usuario.instancia;

        //viajamos al menu principal
        SceneManager.LoadScene("menu_principal");
    }
}
