using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manejadorChat : MonoBehaviour
{
   private crud Crud;
   public UnityEngine.UI.InputField mensaje;

    void Start()
    {
        Crud = GameObject.Find("Crud").GetComponent<crud>();
        Crud.MostrarMensajes();
    }

   public void EnviarMensaje()
   {    
        Crud.EnviarMensaje(mensaje.text);
        mensaje.text = "";
   }
}
