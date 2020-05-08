using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combate : MonoBehaviour
{

    public Personajes[] personajes;
    public Personajes[] enemigos;
    public string tipo_combate;
    public Recompenza[] recompenza;
    public Usuario ganador;
    public Usuario perdedor;
    public GameObject prefab_personaje;
    public List<Personajes> turno = new List<Personajes>();
    private Personajes personaje_en_turno;

    //de pruebas locales
    private Personajes fabrica;

    // INSTANCIAS SINGLETON CON LOS DATOS DE LOS ENEMIGOS Y EL USUARIO
    public storage_script storage_enemigos;
    public Usuario jugador;

    void Start()
    {
        jugador = Usuario.instancia;
        storage_enemigos = storage_script.instancia;
        
        //de prueba
        fabrica = new Personajes();
        personajes = new Personajes[4]{fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("roger")};
        enemigos = new Personajes[4]{fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("roger")};

        //COPIAMOS LOS PERSONAJES DEL USUARIO Y DE LOS ENEMIGOS LOCALMENTE
        //personajes = jugador.personajesFavoritos;
        //enemigos = storage_enemigos.enemigos;
        popular_personajes_mapa(personajes, enemigos, prefab_personaje);

        //LLENAMOS EL PRIMER ORDEN DE TURNOS SEGUN LA VELOCIDAD DE LOS PERSONAJES
        for(int i = 0; i < personajes.Length; i++){
            turno.Add(personajes[i]);
        }
        for(int i = 0; i < enemigos.Length; i++){
            turno.Add(enemigos[i]);
        }
        turno.Sort((x, y) => x.atributos.velocidad.CompareTo(y.atributos.velocidad));
        for(int i = 0; i < turno.Count; i++){
            Debug.Log(turno[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        personaje_en_turno = turno[0];
        Debug.Log("personaje en turno: " + personaje_en_turno.nombre);
    }

    void popular_personajes_mapa(Personajes[] personajes_jugador, Personajes[] enemigos, GameObject prefab){

        //INSTANCIAMOS LOS PERSONAJES DEL JUGADOR
        float[] pos_inicial_x = {-5.71F, -7.990039F, -6.790027F, -4.100024F};
        float[] pos_inicial_y = {-3.72F, -1.910015F, 0.3199964F, 0.8399854F};
        float pos_inicial_z = 20F;
        for(int i = 0; i < personajes_jugador.Length; i++){
            if (personajes_jugador[i] != null){
                GameObject personaje_creado = Instantiate(prefab, new Vector3(pos_inicial_x[i], pos_inicial_y[i], pos_inicial_z), Quaternion.identity);
                GameObject canvas_texto = GameObject.Find("vida_"+i);
                Text texto_vida = canvas_texto.AddComponent<Text>();
                texto_vida.text = personajes_jugador[i].atributos.vitalidad.ToString();
                texto_vida.fontSize = 20;
                texto_vida.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                texto_vida.color = Color.black;
            }
            
        }

        //INSTANCIAMOS LOS ENEMIGOS
        float[] pos_inicial_x_enemigos = {7.39F, 5.15F, 4.63F, 6.76F};
        float[] pos_inicial_y_enemigos = {-2.61F, -3.48F, 0.89F, 0.12F};
        for(int i = 0; i < enemigos.Length; i++){
            GameObject personaje_creado = Instantiate(prefab, new Vector3(pos_inicial_x_enemigos[i], pos_inicial_y_enemigos[i], pos_inicial_z), Quaternion.identity);
            GameObject canvas_texto = GameObject.Find("vida_enemigo_"+i);
            Text texto_vida = canvas_texto.AddComponent<Text>();
            texto_vida.text = enemigos[i].atributos.vitalidad.ToString();
            texto_vida.fontSize = 20;
            texto_vida.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            texto_vida.color = Color.black;
        }
    }

}
