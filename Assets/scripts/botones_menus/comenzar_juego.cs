using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class comenzar_juego : MonoBehaviour
{

    //instancia de un usuario totalmente nuevo
    //public Usuario usuario_nuevo;

	//key de los usuarios en el localStorage
	private const string KEY_JUGADOR ="KEY_JUGADOR";
	[SerializeField] private crud CRUD;

	//CREAMOS UN HILO PARA PASAR DE ESCENA CUANDO TENGAMOS EL PERSONAJE DE LA BASE DE DATOS
	private Coroutine hilo;
    void Start()
    {
	
    }

    public void ir_principal()
    {
        
		if (hilo == null)
		{
			hilo = StartCoroutine(Cargar_escena_hilo());
		}

		//creamos un usuario nuevo
        // usuario_nuevo = Usuario.instancia;

		// //si el jugador existe en el localstorage significa que ya ha jugado antes y lo enviamos al menu principal, si no
		// //lo enviamos a crear un usuario
		// if (PlayerPrefs.HasKey(KEY_JUGADOR)){
		// 	//viajamos al menu principal
        // 	SceneManager.LoadScene("menu_principal");
		// }else{
		// 	SceneManager.LoadScene("crear_personaje");
		// }
    }

	private IEnumerator Cargar_escena_hilo()
	{
		var existe_usuario_task = CRUD.Existe_jugador();

		yield return new WaitUntil( () => existe_usuario_task.IsCompleted );

		if(existe_usuario_task.Result)
		{
			SceneManager.LoadScene("menu_principal");
		}
		else
		{
			SceneManager.LoadScene("crear_personaje");
		}
		hilo = null;
	}
}
