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
        //pasamos los logros a 1 solo string comprimido
        string logrosComprimidos = "";
        foreach(KeyValuePair<int, Logros> l in jugador.logros)
        {
            logrosComprimidos += l.Value.codigo_logro.ToString() + ',';
            logrosComprimidos += l.Value.progreso_actual.ToString() + ',';
            logrosComprimidos += l.Value.puntos.ToString() + ',';
            logrosComprimidos += (l.Value.reclamado == true) ? "1;" : "0;";
        }

        jugador.logrosComprimidos = logrosComprimidos;
        PlayerPrefs.SetString(KEY_JUGADOR, JsonUtility.ToJson(jugador));
        _database.GetReference(KEY_JUGADOR).SetRawJsonValueAsync(JsonUtility.ToJson(jugador));

    }


    public void Guardar_personajes(List<Personajes> pjs)
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
        //descomprimimos los logros de string a un objeto
        Usuario jugador = JsonUtility.FromJson<Usuario>(dataSnapshot.GetRawJsonValue());
        int lastComaIndex = 0;
        int i = 0;
        string l;
        Logros logroDescomprimido;
        int codigo_logro = 0;
        int progreso_actual = 0;
        int puntos = 0;
        bool reclamado = false;
        while(i < jugador.logrosComprimidos.Length - 1)
        {
            int comaPosition = jugador.logrosComprimidos.Substring(i).IndexOf(",");
            if(lastComaIndex == 3) comaPosition = jugador.logrosComprimidos.Substring(i).IndexOf(";");
            l = jugador.logrosComprimidos.Substring(i, comaPosition);
            switch(lastComaIndex)
            {
                case 0:
                    codigo_logro = int.Parse(l);
                    i += l.Length;
                    lastComaIndex ++;
                    break;
                case 1:
                    progreso_actual = int.Parse(l);
                    i += l.Length;
                    lastComaIndex ++;
                    break;
                case 2:
                    puntos = int.Parse(l);
                    i += l.Length;
                    lastComaIndex ++;
                    break;
                case 3:
                    reclamado = (l == "1") ? true : false;
                    i++;
                    lastComaIndex = 0;
                    logroDescomprimido = new Logros(codigo_logro, progreso_actual, puntos, reclamado);
                    jugador.logros[codigo_logro] = logroDescomprimido;
                    break;
                default:
                    break;
            }
            i++;
        }
        jugador.logrosComprimidos = "";
        //return JsonUtility.FromJson<Usuario>(dataSnapshot.GetRawJsonValue());
        return jugador;
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

    //ENVIAMOS UN MENSAJE AL CHAT GLOBAL
    public void EnviarMensaje(string mensaje)
    {
        //ARMAMOS EL MENSAJE CON EL NOMBRE DEL USUARIO
        string nombre = this.usuario.nombre;
        string mensajeCompuesto = nombre + " : " + mensaje;

        //SI EL MENSAJE ESTA EN BLANCO, NO LO ENVIAMOS
        if(mensaje.Length < 2) return;

        //ENVIAMOS EL MENSAJE
        _database.GetReference("CHAT").Child("GLOBAL").Push().SetValueAsync(mensajeCompuesto);
    }

    //MOSTRAMOS MENSAJES DEL CHAT GLOBAL
    public void MostrarMensajes()
    {
        _database.GetReference("CHAT").Child("GLOBAL").ValueChanged += HandleValueChanged;
    }
   
 
    //ACTUALIZAMOS EL CHAT GLOBAL CON CUALQUIER CAMBIO
    void HandleValueChanged(object sender, ValueChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        try{
            //ITERAMOS POR CADA HIJO DEL CHAT Y METEMOS SU VALOR EN UNA LISTA DE STRINGS
            UnityEngine.UI.Text panelText;
            panelText = GameObject.Find("textoChat").GetComponent<UnityEngine.UI.Text>();
            var jsonText = args.Snapshot.Children;
            List<string> mensajes = new List<string>();
            foreach(var h in jsonText)
            {
                mensajes.Add(h.Value.ToString());
            }
            //PONEMOS CADA VALOR DE LA LISTA DE STRINGS EN LA UI
            panelText.text = "";
            for(int i = 0; i < mensajes.Count; i++)
            {
                panelText.text += mensajes[i] + "\n";
            }
           
            //SI TENEMOS MAS DE 30 MENSAJES, BORRAMOS EL ULTIMO
            if(mensajes.Count >= 10)
            {
                BorrarMensajes();
            }
        }
        //UN CATCH PARA CUANDO NO ESTEMOS EN LA PANTALLA PRINCIPAL
        catch (Exception e){
            Debug.Log("Chat no disponible: " +  e);
        }
    }

    //BORRAMOS EL ULTIMO MENSAJE ESCRITO
    public void BorrarMensajes()
    {
        //VAMOS A CHAT / GLOBAL Y SOLO RETORNAMOS EL PRIMER CHILD
        _database.GetReference("CHAT").Child("GLOBAL").LimitToFirst(1).GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                    Debug.Log("ERRORES BORRANDO MENSAJE");
                    }
                    else if (task.IsCompleted) {
                        //BORRAMOS EL PRIMER CHILD Y PARAMOS EL CICLO DE BORRAR
                        DataSnapshot snapshot = task.Result;
                        foreach(var child in snapshot.Children)
                        {
                            _database.GetReference("CHAT").Child("GLOBAL").Child(child.Key).RemoveValueAsync();
                            break;
                        }
                    }
        });
    }
}
