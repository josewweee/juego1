using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class comenzar_juego : MonoBehaviour
{

    //instancia de un usuario totalmente nuevo
    public GameObject usuario_nuevo;
    
    void Start()
    {
        
    }

    public void ir_principal(){
        //creamos un usuario nuevo
        Instantiate(usuario_nuevo, new Vector3(2.0F, 0, 0), Quaternion.identity);

        //viajamos al menu principal
        SceneManager.LoadScene("menu_principal");
    }
}
