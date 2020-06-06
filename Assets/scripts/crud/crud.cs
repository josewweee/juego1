using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading.Tasks;
using System;


public class crud : MonoBehaviour {

    //KEY DEL USUARIO EN EL LOCALSTORAGE
    private string KEY_JUGADOR ="KEY_JUGADOR";
    
    //TRAEMOS LA BASE DE DATOS
    private FirebaseDatabase _database;

    //CREAMOS LA VARIABLE DONDE ALOJAREMOS EL USUARIO
    Usuario usuario;

    //PARA CREAR USUARIOS
    private bool hilo = false;

    private void Start() {
        //TRAEMOS EL NOMBRE DEL USUARIO EN EL DISPOSITIVO ACTUAL
        _database = FirebaseDatabase.DefaultInstance;
        KEY_JUGADOR ="KEY_JUGADOR";
        Debug.Log("Comenzando el crud, buscando key del jugador...");
        try{
            Debug.Log("key..");
            usuario = JsonUtility.FromJson<Usuario>(PlayerPrefs.GetString(KEY_JUGADOR));
            KEY_JUGADOR = "KEY_JUGADOR/"+ usuario.nombre.ToString();
            Debug.Log("Tu Key es: " + KEY_JUGADOR);
        }catch (Exception e){
            Debug.Log("nada en el localStorage, " + e);
            Debug.Log(KEY_JUGADOR);
            KEY_JUGADOR = "null";
        }
        //PARA NO DESTRUIR ESTO ENTRE ESCENAS
        DontDestroyOnLoad(this.gameObject);
    }

    //ENVIAMOS EL USUARIO QUE NOS MANDEN AL LOCAL STORAGE Y A LA DATABASE COMO UN JSON
    public void Guardar_usuario(Usuario jugador)
    {
        PlayerPrefs.SetString(KEY_JUGADOR, JsonUtility.ToJson(jugador));
        _database.GetReference(KEY_JUGADOR).SetRawJsonValueAsync(JsonUtility.ToJson(jugador));

    }


    public void Guardar_personajes(Personajes pjs)
    {
        Debug.Log("enviando: " + JsonUtility.ToJson(pjs) );
        _database.GetReference(KEY_JUGADOR).Child("personajes").SetRawJsonValueAsync(JsonUtility.ToJson(pjs));

    }

    // REVISAMOS SI EL USUARIO EXISTE Y LO RETORNAMOS DEL LOCALSTORAGE PARSEAMOS DE JSON A TIPO USUARIO, SI NO EXISTE RETORNAMOS NULL
    public async Task<Usuario> Cargar_usuario()
    {
        var dataSnapshot = await _database.GetReference(KEY_JUGADOR).GetValueAsync();
        if(!dataSnapshot.Exists)
        {
            Debug.Log("No encontramos el usuario " +  KEY_JUGADOR +  ", en la DB");
            return null;
        }
        return JsonUtility.FromJson<Usuario>(dataSnapshot.GetRawJsonValue());
        // if (Existe_jugador()){
        //     return JsonUtility.FromJson<Usuario>(PlayerPrefs.GetString(KEY_JUGADOR));
        // }

        // return null;
    }

    //TRAEMOS LA LISTA DE JUGADORES PARA LLENAR LA LISTA DE PVP
    public async Task<List<Usuario>> Cargar_jugadores()
    {
        var dataSnapshot = await _database.GetReference("KEY_JUGADOR").GetValueAsync();
        if(!dataSnapshot.Exists)
        {
            Debug.Log("NO ENCONTRAMOS NADA");
            return null;
        }
        List<Usuario> usuarios = new List<Usuario>();
        foreach(var u in dataSnapshot.Children){
            Debug.LogFormat("Key = {0}", u.Key);
            usuarios.Add( JsonUtility.FromJson<Usuario>(u.GetRawJsonValue()));
        }
        //ELIMINAMOS EL USUARIO QUE SE LLAMA IGUAL QUE EL USUARIO ACTUAL
        try{
            usuarios.RemoveAll(item => item.nombre == usuario.nombre);
        }
        catch{
            Start();
            usuarios.RemoveAll(item => item.nombre == usuario.nombre);
        }

        return usuarios;
    }


    //MIRAMOS SI LA KEY DEL USUARIO EXISTE
    public async Task<bool> Existe_jugador()
    {
        var dataSnapshot = await _database.GetReference(KEY_JUGADOR).GetValueAsync();
        Debug.Log(KEY_JUGADOR);
        return dataSnapshot.Exists;
        //return PlayerPrefs.HasKey(KEY_JUGADOR);
    }

    //BORRAMOS UN USUARIO
    public void Borrar_usuario()
    {
        //PlayerPrefs.DeleteKey(KEY_JUGADOR);
        _database.GetReference(KEY_JUGADOR).RemoveValueAsync();
    }

    //CARGAMOS TODOS LOS USUARIOS AMIGOS CON EL STRING ENVIADO ( ESTE VIENE DE LA LISTA DE AMIGOS DEL USUARIO ACTUAL)
    public async Task<List<Amigos>> Cargar_amigos(string[] nombres)
    {
        List<Amigos> amigos = new List<Amigos>();
        for(int i = 0; i < nombres.Length; i++){
            string key = "KEY_JUGADOR/"+nombres[i];
            var dataSnapshot = await _database.GetReference(key).GetValueAsync();
            Usuario usuario_tomado = JsonUtility.FromJson<Usuario>(dataSnapshot.GetRawJsonValue());
            List<Personajes> pjs = usuario_tomado.personajes;
            List<Personajes> favoritos = usuario_tomado.personajesFavoritos;
            List<Personajes> def_pvp = usuario_tomado.defensa_pvp;
            bool regalo = usuario_tomado.regalo_enviado;
            Amigos amigo_nuevo = new Amigos(nombres[i], favoritos, def_pvp, pjs, regalo);
            amigos.Add(amigo_nuevo);
        }
        return amigos;  
    }

    //CREAMOS UN USUARIO NUEVO
    public void Crear_usuario(string key, Usuario usuario_nuevo)
    {
        UnityEngine.UI.Button btn_continuar = GameObject.Find("btn_continuar").GetComponent<UnityEngine.UI.Button>();
        if (hilo == false){
            hilo = true;
            _database.GetReference(key).SetRawJsonValueAsync(JsonUtility.ToJson(usuario_nuevo)).ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Errores creando el usuario");
                }
                else if (task.IsCompleted) 
                {
                    Debug.Log("Usuario creado");
                    this.KEY_JUGADOR = "KEY_JUGADOR/" + usuario_nuevo.nombre;
                    btn_continuar.interactable = true;
                    //this.GetComponent<crear_personaje>().ActivarCambioEscena();
                }
            });
        }
    }


    //ESTRUCTURA PARA TRAER LOS ITEMS DE LA RECOMEPNZA DEL DAILY LOGIN
    public struct daily_item{
        public string titulo;
        public string nombre;
        public string foto;
        public int cantidad;
    }

    //TRAEMOS LA RECOMPENZA DEL DAILY LOGIN
    public async Task<List<daily_item>> Cargar_dailys()
    {
        var dataSnapshot = await _database.GetReference("DAILY").GetValueAsync();
        if(!dataSnapshot.Exists)
        {
            Debug.Log("NO ENCONTRAMOS NADA");
            return null;
        }
        List<daily_item> daily_items = new List<daily_item>();
        foreach(var d in dataSnapshot.Children){
            daily_items.Add( JsonUtility.FromJson<daily_item>(d.GetRawJsonValue()));
        }
        //ELIMINAMOS EL USUARIO QUE SE LLAMA IGUAL QUE EL USUARIO ACTUAL
        return daily_items;
    }
}
