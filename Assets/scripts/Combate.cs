using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour
{

    public Personajes[] personajes;
    public Personajes[] enemigos;
    public string tipo_combate;
    public Recompenza[] recompenza;
    public Usuario ganador;
    public Usuario perdedor;
    public GameObject prefab_personaje;

    // INSTANCIAS SINGLETON CON LOS DATOS DE LOS ENEMIGOS Y EL USUARIO
    public storage_script storage_enemigos;
    public Usuario jugador;

    void Start()
    {
        jugador = Usuario.instancia;
        storage_enemigos = storage_script.instancia;
        
        //COPIAMOS LOS PERSONAJES DEL USUARIO Y DE LOS ENEMIGOS LOCALMENTE
        personajes = jugador.personajesFavoritos;
        enemigos = storage_enemigos.enemigos;
        popular_personajes_mapa(personajes, enemigos, prefab_personaje);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void popular_personajes_mapa(Personajes[] personajes_jugador, Personajes[] enemigos, GameObject prefab){

        //INSTANCIAMOS LOS PERSONAJES DEL JUGADOR
        float[] pos_inicial_x = {-5.71F, -7.990039F, -6.790027F, -4.100024F};
        float[] pos_inicial_y = {-3.72F, -1.910015F, 0.3199964F, 0.8399854F};
        float pos_inicial_z = 20F;
        for(int i = 0; i < personajes_jugador.Length; i++){
            GameObject personaje_creado = Instantiate(prefab, new Vector3(pos_inicial_x[i], pos_inicial_y[i], pos_inicial_z), Quaternion.identity);
        }

        //INSTANCIAMOS LOS ENEMIGOS
        float[] pos_inicial_x_enemigos = {7.39F, 5.15F, 4.63F, 6.76F};
        float[] pos_inicial_y_enemigos = {-2.61F, -3.48F, 0.89F, 0.12F};
        for(int i = 0; i < enemigos.Length; i++){
            GameObject personaje_creado = Instantiate(prefab, new Vector3(pos_inicial_x_enemigos[i], pos_inicial_y_enemigos[i], pos_inicial_z), Quaternion.identity);
        }
    }

}
