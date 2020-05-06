using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usuarios : MonoBehaviour
{
    public static Usuarios instancia = null;

    public string nombre = "nuevo usuario";
    public int nivel = 1;
    public int experiencia = 0;
    public string hermandad = "";
    public bool tutorial_completo = false;
    public Usuarios[] amigos = null;
    public List<Personajes> personajes = new List<Personajes>();
    public List<Monedas> monedas = new List<Monedas>();
    public Configuracion configuracion = new Configuracion();
    public List<Logros> logros = new List<Logros>();
    public int puntos_logro = 0;
    public int energia = 30;
    public string metodo_login = "";
    public int energia_pvp = 0;
    public int puntos_pvp = 0;
    public Personajes[] defensa_pvp = new Personajes[3];
    public int posicion_pvp = 0;

    void Awake(){
        if(instancia == null){
            instancia = this;
        }else if(instancia != this){
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
