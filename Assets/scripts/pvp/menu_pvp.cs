using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu_pvp : MonoBehaviour
{

    private routing _routing;
    public GameObject menu;
    private Usuario jugador;
    private storage_script storage_enemigos;
    public GameObject prefab_lista;
    private List<Usuario> jugadores_DB = new List<Usuario>();
    private Text txt_energia_pvp;
    private Text txt_puntos_pvp;
    private Text txt_posicion_pvp;
    private Personajes fabrica;

    //INSTANCIA DE LA DB
    private crud CRUD;

    private IEnumerator Start()
    {
         _routing = menu.GetComponent<routing>();

         //TRAEMOS LAS INSTANCIAS DEL JUGADOR Y LOS ENEMIGOS
        jugador = Usuario.instancia;
        storage_enemigos = storage_script.instancia;

        //TRAEMOS LOS OTROS JUGADORES DE LA DB
        CRUD = GameObject.Find("Crud").GetComponent<crud>();
        var otros_jugadores = CRUD.GetComponent<crud>().Cargar_jugadores();
        yield return new WaitUntil( ()=> otros_jugadores.IsCompleted);
        jugadores_DB = otros_jugadores.Result;
        Debug.Log(jugadores_DB.Count);

        //CONECTAMOS LA UI
        txt_energia_pvp = GameObject.Find("texto_energia_pvp").GetComponent<Text>();
        txt_energia_pvp.text = jugador.energia_pvp.ToString();
        txt_puntos_pvp = GameObject.Find("texto_numero_puntos").GetComponent<Text>();
        txt_puntos_pvp.text = jugador.puntos_pvp.ToString();
        txt_posicion_pvp = GameObject.Find("texto_numero_posicion").GetComponent<Text>();
        txt_puntos_pvp.text = jugador.posicion_pvp.ToString();


        // fabrica = new Personajes();
        // Usuario u1 = new Usuario();
        // u1.defensa_pvp[0] = fabrica.Crear_personaje("roger");
        // u1.defensa_pvp[1] = fabrica.Crear_personaje("alicia");
        // u1.defensa_pvp[2] = fabrica.Crear_personaje("martis");
        // u1.defensa_pvp[3] = fabrica.Crear_personaje("liliana");
        // jugadores_DB = new Usuario[4]{u1, new Usuario(), new Usuario(), new Usuario()};
        Popular_lista_pvp(prefab_lista);
    }


    private void Popular_lista_pvp(GameObject prefab_lista)
    {
        float x = 2.200012F;
        float y = 120.7798F;
        foreach(Usuario u in jugadores_DB)
        {
            //INSTANCIAMOS EL USUARIO
            GameObject recuadro_pvp = Instantiate(prefab_lista, new Vector3(x, y, 0), Quaternion.identity);
            recuadro_pvp.transform.SetParent(GameObject.Find("contenido_arena").transform, false);

            string output = JsonUtility.ToJson(u, true);
            Debug.Log(output);

            //NOMBRE DEL OPONENTE
            Text nombre = recuadro_pvp.transform.GetChild(0).gameObject.GetComponent<Text>();
            nombre.text = u.nombre;


            //LE ASIGNAMOS LOS PERSONAJES AL SINGLETON PARA SIGUIENTE ESCENA
            Button btn = recuadro_pvp.GetComponent<Button>();
            List<Personajes> defensa_enemiga = u.defensa_pvp;
            btn.onClick.AddListener(delegate { Ir_combate_pvp(u, defensa_enemiga); });

            y -= 48F;
        }
    }


    public void Ir_combate_pvp(Usuario enemigo, List<Personajes> pjs_enemigo)
    {
        storage_enemigos.enemigos_pvp = pjs_enemigo;
        storage_enemigos.jugador_enemigo = enemigo;
        storage_enemigos.tipo_combate = "pvp";
        _routing.ir_seleccion_pre_combate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
