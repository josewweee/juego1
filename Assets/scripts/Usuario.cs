using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Usuario
{

    public string nombre = "nuevo usuario";
    public int nivel = 1;
    public int experiencia = 0;
    public string hermandad = "";
    public bool tutorial_completo = false;
    public List<Amigos> amigos = new List<Amigos>();
    public List<Personajes> personajes = new List<Personajes>();
    public List<Equipo> equipo = new List<Equipo>();
    public List<Personajes> personajesFavoritos;
    public Monedas monedas = new Monedas(0,0,0,0,0,0,0,0);
    public Configuracion configuracion = new Configuracion();
    //public List<Logros> logros = new List<Logros>();
    public Dictionary<int, Logros> logros = new Dictionary<int, Logros>();
    public string logrosComprimidos = "";
    public int puntos_logro = 0;
    public int energia = 30;
    public int energia_maxima;
    public string metodo_login = "";
    public int energia_pvp = 15;
    public int puntos_pvp = 0;
    public List<Personajes> defensa_pvp;
    public string[] historial_pvp;
    public int posicion_pvp = 99999;
    public int nivel_historia = 1;
    public bool regalo_enviado = false;
    public bool daily_reclamado = false;
    public int login_diarios = 1;

    // CONSTRUCTOR
    public Usuario() { 
        energia_maxima = 30;
        defensa_pvp = new List<Personajes>(){null, null, null, null};
        personajesFavoritos = new List<Personajes>(){null, null, null, null};
    }
    // SINGLETON DEL USUARIO
    public static Usuario _instancia = null;
    public static Usuario instancia
    {
        get
        {
            if (_instancia == null)
                _instancia = new Usuario();
            return _instancia;
        }
    }

    public void Actualizar_usuario(Usuario nuevo)
    {
        nombre = nuevo.nombre;
        nivel = nuevo.nivel;
        experiencia = nuevo.experiencia;
        hermandad = nuevo.hermandad;
        tutorial_completo = nuevo.tutorial_completo;
        amigos = nuevo.amigos;
        personajes = nuevo.personajes;
        equipo = nuevo.equipo;
        personajesFavoritos = nuevo.personajesFavoritos;
        monedas = nuevo.monedas;
        configuracion = nuevo.configuracion;
        logros = nuevo.logros;
        logrosComprimidos = nuevo.logrosComprimidos;
        puntos_logro = nuevo.puntos_logro;
        energia = nuevo.energia;
        energia_maxima = nuevo.energia_maxima;
        metodo_login = nuevo.metodo_login;
        energia_pvp = nuevo.energia_pvp;
        puntos_pvp = nuevo.puntos_pvp;
        defensa_pvp = nuevo.defensa_pvp;
        historial_pvp = nuevo.historial_pvp;
        posicion_pvp = nuevo.posicion_pvp;
        nivel_historia = nuevo.nivel_historia;
        regalo_enviado = nuevo.regalo_enviado;
        daily_reclamado = nuevo.daily_reclamado;
        login_diarios = nuevo.login_diarios;
    }

    // METODOS DE LA CLASE
    public bool Agregar_personaje(Personajes personaje)
    {
        personajes.Add(personaje);
        return true;
    }

    public void Sumar_experiencia(int cantidad)
    {
        experiencia += cantidad;
        if (experiencia >= 50 * nivel) Subir_nivel(1);
    }

    public void Subir_nivel(int niveles)
    {
        nivel += niveles;
        energia_maxima += 10;
        energia = energia_maxima;
        experiencia = 0;
    }

    public void SubirNivelHistoria(int nivel)
    {
        if (nivel > this.nivel_historia)
        {
            this.nivel_historia +=1;
        }
    }

    public void Sumar_monedas(string tipo_moneda, int cantidad)
    {
        switch (tipo_moneda)
        {
            case "oro":
                this.monedas.oro += cantidad;
                break;
            case "diamantes":
                this.monedas.diamantes += cantidad;
                break;
            case "puntos_pvp":
                this.monedas.puntos_pvp += cantidad;
                break;
            case "invocaciones_normales":
                this.monedas.invocaciones_normales += cantidad;
                break;
            case "invocaciones_raras":
                this.monedas.invocaciones_raras += cantidad;
                break;
            case "invocaciones_miticas":
                this.monedas.invocaciones_miticas += cantidad;
                break;
            case "invocaciones_legendarias":
                this.monedas.invocaciones_legendarias += cantidad;
                break;
            default:
                Debug.Log("no existe el tipo de moneda: " + tipo_moneda + ", revisa bien");
                break;
        }
    }

    public bool AgregarAmigos(Amigos amigo)
    {
        amigos.Add(amigo);
        return true;
    }

    //AUMENTAMOS EL PROGRESO ACTUAL DE UN LOGRO EN 1 UNIDAD
    public void AumentarLogro(int codigoLogro, int objetivoLogro)
    {
        if (logros.ContainsKey(codigoLogro))
        {
           Logros logroActual = logros[codigoLogro];
           logroActual.progreso_actual += 1;
           logros[codigoLogro] = logroActual;
        }
        else
        {
            logros[codigoLogro] = new Logros(codigoLogro,1,1,false);
        }

        if(logros[codigoLogro].progreso_actual >= objetivoLogro)
        {
             Debug.Log("Logro #" + codigoLogro +  ", completado, Felicidades");
        }
    }

    //PONEMOS UN LOGRO COMO COMPLETADO
    public void LogroCompletado(int codigoLogro)
    {
        if (logros.ContainsKey(codigoLogro))
        {
           Logros logroActual = logros[codigoLogro];
           logroActual.reclamado = true;
           logros[codigoLogro] = logroActual;
        }
    }

    public void Recargar_energia(string tipo_energia, int cantidad)
    {
        switch (tipo_energia)
        {
            case "energia":
                energia += cantidad;
                break;
            case "energia_pvp":
                energia_pvp += cantidad;
                break;
            default:
                Debug.Log("no existe el tipo de energia" + tipo_energia + ", revisa bien");
                break;
        }
    }

    public void Cambiar_personaje_batalla(string tipo_lista, int posicion_cambio, Personajes personaje)
    {
        switch (tipo_lista)
        {
            case "personajes_favoritos":
                try
                {
                    //RELLENAMOS LA LISTA CON NULLS, PARA EVITAR TENER INDEX OUT OF BOUNDS, ESTO PASA YA QUE
                    //AL TRAER LA LISTA DE LA DB, ESTA VIENE MAS PEQUEÑA
                    if(personajesFavoritos.Count < 4)
                    {
                        while(personajesFavoritos.Count < 4)
                        {
                            personajesFavoritos.Add(null);
                        }
                    }
                    personajesFavoritos[posicion_cambio] = personaje;
                    Debug.Log(personajesFavoritos[posicion_cambio].nombre);
                }
                catch  ( Exception ex)
                {
                    Debug.Log("error ocurrido con el cambio de personaje: " + ex);
                }
                break;
            case "defensa_pvp":
                try
                {
                    //RELLENAMOS LA LISTA CON NULLS, PARA EVITAR TENER INDEX OUT OF BOUNDS, ESTO PASA YA QUE
                    //AL TRAER LA LISTA DE LA DB, ESTA VIENE MAS PEQUEÑA
                    if(defensa_pvp.Count < 4)
                    {
                        while(defensa_pvp.Count < 4)
                        {
                            defensa_pvp.Add(null);
                        }
                    }
                    defensa_pvp[posicion_cambio] = personaje;
                }
                catch ( Exception ex)
                {
                    Debug.Log("error ocurrido con el cambio de personaje:" + ex);
                }
                break;
            default:
                break;
        }
    }

    public void Cambiar_posicion_lista_pvp(int cantidad)
    {
        posicion_pvp = cantidad;
    }

    public void Cambiar_metodo_login(string metodo)
    {

    }

    public void Aumentar_login_reclamados()
    {
        this.login_diarios ++;
        if (this.login_diarios >= 7)
        {
            this.login_diarios = 0;
        }
    }

    public void Reiniciar_daily()
    {
        this.daily_reclamado = false;
    }
}
