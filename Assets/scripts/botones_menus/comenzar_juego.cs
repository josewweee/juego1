using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class comenzar_juego : MonoBehaviour
{

    //instancia de un usuario totalmente nuevo
    public Usuario usuario_nuevo;

	//key de los usuarios en el localStorage
	private const string KEY_JUGADOR ="KEY_JUGADOR";

    void Start()
    {
	Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
	  var dependencyStatus = task.Result;
	  if (dependencyStatus == Firebase.DependencyStatus.Available) {
	    // Create and hold a reference to your FirebaseApp,
	    // where app is a Firebase.FirebaseApp property of your application class.
	    //   app = Firebase.FirebaseApp.DefaultInstance;

	    // Set a flag here to indicate whether Firebase is ready to use by your app.
	  } else {
	    UnityEngine.Debug.LogError(System.String.Format(
	      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
	    // Firebase Unity SDK is not safe to use here.
	  }
	});
    }

    public void ir_principal()
    {
        //creamos un usuario nuevo
        usuario_nuevo = Usuario.instancia;

		//si el jugador existe en el localstorage significa que ya ha jugado antes y lo enviamos al menu principal, si no
		//lo enviamos a crear un usuario
		if (PlayerPrefs.HasKey(KEY_JUGADOR)){
			//viajamos al menu principal
        	SceneManager.LoadScene("menu_principal");
		}else{
			SceneManager.LoadScene("crear_usuario");
		}
    }
}
