
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
   public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private void Start() {
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread( task => {
            if (task.Exception != null){
                Debug.Log("Error para inicializar Firebase: " + task.Exception);
                return;
            }
            OnFirebaseInitialized.Invoke();
        });
    }
}