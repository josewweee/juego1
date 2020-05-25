using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading.Tasks;


public class crud : MonoBehaviour {

    //KEY DEL USUARIO EN EL LOCALSTORAGE
    private string KEY_JUGADOR ="KEY_JUGADOR";
    
    //TRAEMOS LA BASE DE DATOS
    private FirebaseDatabase _database;
    

    private void Start() {
        _database = FirebaseDatabase.DefaultInstance;
        Usuario usuario;
        try{
            usuario = JsonUtility.FromJson<Usuario>(PlayerPrefs.GetString(KEY_JUGADOR));
            KEY_JUGADOR = "KEY_JUGADOR/"+ usuario.nombre.ToString();
        }catch{
            Debug.Log("nada en el localStorage");
            Debug.Log(KEY_JUGADOR);
             KEY_JUGADOR = "null";
        }
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
        Debug.Log(KEY_JUGADOR);
        var dataSnapshot = await _database.GetReference(KEY_JUGADOR).GetValueAsync();
        if(!dataSnapshot.Exists)
        {
            Debug.Log("NO ENCONTRAMOS NADA");
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
}
