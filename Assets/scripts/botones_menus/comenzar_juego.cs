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
		//PlayerPrefs.DeleteKey(KEY_JUGADOR);
		//Debug.Log("BOrrando key");
    }

    public void ir_principal()
    {
        //UNA HILO SE ENCARGA DE ESPERAR EL RESULTADO DE LA DB Y CAMBIAR DE ESCENA
		if (hilo == null)
		{
			hilo = StartCoroutine(Cargar_escena_hilo());
		}
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
