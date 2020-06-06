using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Combate : MonoBehaviour
{
    //PERSONAJES EN EL COMBATE
    public Personajes[] personajes;
    public Personajes[] enemigos;
    public string tipo_combate;

    //MECANICAS PERSONAJES
    private mecanicas_combate mecanicas;
    public Poderes poder_a_ser_lanzado;
    public inteligencia_artificial maquina;
    private Personajes[,] matrix_envio_personajes;

    //VELOCIDADES INICIALES DE LOS ALIADOS Y LOS ENEMIGOS
    public float[] velocidades_personajes;
    public float[] velocidades_enemigos;

    //RECOMPENZAS
    public Recompenza recompenza;
    public Usuario ganador;
    public Usuario perdedor;
    private GameObject fin_juego;
    private float fixedDeltaTime;


    //PREFAB QUE INSTANCIAREMOS COMO PERSONAJE
    public GameObject prefab_personaje;

    //TURNOS DE PERSONAJES
    public Dictionary<string, Personajes> turno = new Dictionary<string, Personajes>();
    private Personajes personaje_en_turno;
    public string key_personaje_turno;
    public bool turno_finalizado = false;
    int index_personaje_en_turno = 0;
    public GameObject imagen_puntero;
    public Sprite puntero_verde;
    public Sprite puntero_rojo;

    //de pruebas locales
    private Personajes fabrica;

    // INSTANCIAS SINGLETON CON LOS DATOS DE LOS ENEMIGOS Y EL USUARIO
    public storage_script storage_enemigos;
    public Usuario jugador;

    //OBJETOS PARA PODER IR A OTRAS ESCENAS EN EL MENU Y MENU CONFIGURACIONES
    public GameObject menu;
    public routing _routing;
    private GameObject menu_configuracion;

    //UI DE LOS PODERES PARA MAXIMO 4 PERSONAJES
    public bool cambiar_UI_poderes = true;
    private GameObject poder_1;
    private GameObject poder_2;
    private GameObject poder_3;
    private GameObject poder_4;

    public Button btn_poder_1;
    public Button btn_poder_2;
    public Button btn_poder_3;
    public Button btn_poder_4;

    private Sprite[] img_poderes_personaje_1;
    private Sprite[] img_poderes_personaje_2;
    private Sprite[] img_poderes_personaje_3;
    private Sprite[] img_poderes_personaje_4;

    //UI CON TEXTO DE SELECCIONAR OBJETIVO / OBJETIVOS
    public GameObject objetivo_unico_txt;
    public GameObject multiple_objetivo_txt;
    public GameObject propio_objetivo_txt;
    

    //VARIABLES DE ANIMACION
    Animator animator;
    private IEnumerator corrutina;
    GameObject mostrar_poder; //poder que sale en la pantalla;

    //VARIABLES DE BARRA DE VIDA
    private List <GameObject> Objs_barras_vidas = new List <GameObject>();



    void Awake(){
        //PONEMOS LA VELOCIDAD DEL TIEMPO INICIAL EN EL JUEGO
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Start()
    {
        
        //TRAEMOS LAS INSTANCIAS DEL JUGADOR Y LOS ENEMIGOS
        jugador = Usuario.instancia;
        storage_enemigos = storage_script.instancia;
        tipo_combate = storage_enemigos.tipo_combate;

        //DESACTIVAMOS EL UI DE FIN DEL JUEGO
        fin_juego = GameObject.Find("Fin_juego");
        recompenza = fin_juego.GetComponent<Recompenza>();
        fin_juego.SetActive(false);

        //INSTANCIAMOS EL SCRIPT QUE NOS DEJARA CAMBIAR DE ESCENAS, ASIGNAMOS Y DESACTIVAMOS EL MENU DE CONFIGURACION
        this.menu = GameObject.Find("menu");
        _routing = menu.GetComponent<routing>();
        this.menu_configuracion = GameObject.Find("ventana_configuracion");
        this.menu_configuracion.SetActive(false);
        
        //de prueba
        // fabrica = new Personajes();
        // personajes = new Personajes[4]{fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("liliana"), fabrica.Crear_personaje("martis")};
        // enemigos = new Personajes[4]{fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("martis"), fabrica.Crear_personaje("liliana")};

        //COPIAMOS LOS PERSONAJES DEL USUARIO Y DE LOS ENEMIGOS LOCALMENTE
        if (tipo_combate == "historia")
        {
            personajes = jugador.personajesFavoritos.ToArray();
            enemigos = storage_enemigos.enemigos.ToArray();
        }
        else if (tipo_combate == "pvp" || tipo_combate == "amistoso")
        {
            personajes = jugador.defensa_pvp.ToArray();
            enemigos = storage_enemigos.enemigos_pvp.ToArray();
        }


        //INSTANCIAMOS LAS MECANICAS Y LA IA
        mecanicas = new mecanicas_combate();
        maquina = new inteligencia_artificial(enemigos, personajes);
        matrix_envio_personajes = new Personajes[2, personajes.Length];

        //ASIGNAMOS LAS VELOCIDADES INCIALES DE LOS PERSONAJES Y ENEMIGOS
        velocidades_personajes = new float[personajes.Length];
        velocidades_enemigos = new float[enemigos.Length];
        for(int i = 0; i < personajes.Length; i++){
            velocidades_personajes[i] = personajes[i].atributos.velocidad;
        }
        for(int i = 0; i < enemigos.Length; i++){
            velocidades_enemigos[i] = enemigos[i].atributos.velocidad;
        }

        //ASIGNAMOS EL UI DE LOS TEXTOS DE OBJETIVOS Y LOS DESHABILITAMOS POR AHORA
        objetivo_unico_txt = GameObject.Find("texto_seleccion_objetivo");
        multiple_objetivo_txt = GameObject.Find("texto_seleccion_objetivos");
        propio_objetivo_txt = GameObject.Find("texto_seleccion_propia");
        propio_objetivo_txt.SetActive(false);
        objetivo_unico_txt.SetActive(false);
        multiple_objetivo_txt.SetActive(false);

        //DESACTIVAMOS TODAS LAS BARRAS DE VIDA PARA NO USAR LAS QUE NO NECESITAMOS, SON 8 EN TOTAL ( 4 DE ENEMIGOS Y 4 DE ALIADOS )
        for (int w = 0; w < 4; w++) {
            GameObject b_vida = GameObject.Find("barra_vida_"+w);
            GameObject b_vida_enemigo = GameObject.Find("barra_vida_enemigo_"+w);
            b_vida.SetActive(false);
            b_vida_enemigo.SetActive(false);
            Objs_barras_vidas.Add(b_vida);
            Objs_barras_vidas.Add(b_vida_enemigo); 
        }

        
        //LLENAMOS EL CAMPO CON LOS PERSONAJES COPIADOS
        popular_personajes_mapa(personajes, enemigos);

        //LLENAMOS EL PRIMER ORDEN DE TURNOS SEGUN LA VELOCIDAD DE LOS PERSONAJES
        for(int i = 0; i < personajes.Length; i++){
            turno["aliado"+i] = personajes[i];
        }
        for(int i = 0; i < enemigos.Length; i++){
            turno["enemigo"+i] = enemigos[i];
        }


        turno = turno.OrderByDescending(key => key.Value.atributos.velocidad).ToDictionary(x => x.Key, x => x.Value);
        
        foreach (KeyValuePair<string, Personajes> pj in turno)
        {
            personaje_en_turno = pj.Value;
            key_personaje_turno = pj.Key;
            break;
        }

        //INICIALIZAMOS LOS BOTONES
        poder_1 = GameObject.Find("poder_1");
        poder_2 = GameObject.Find("poder_2");
        poder_3 = GameObject.Find("poder_3");
        poder_4 = GameObject.Find("poder_4");

        btn_poder_1 = poder_1.GetComponent<Button>();
        btn_poder_2 = poder_2.GetComponent<Button>();
        btn_poder_3 = poder_3.GetComponent<Button>();
        btn_poder_4 = poder_4.GetComponent<Button>();

        //INICIALIZAMOS LA IMAGEN DEL PODER QUE SE MUESTRA EN LA PANTALLA
        mostrar_poder = GameObject.Find("poder_lanzado");

        //IMAGENES DE LOS PODERES DE MAXIMO 4 PERSONAJES
        if (personajes.Length >= 1){
                img_poderes_personaje_1 = new Sprite[4]{
                    Resources.Load <Sprite>("poderes/" + personajes[0].poderes[0].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[0].poderes[1].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[0].poderes[2].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[0].poderes[3].imagen)
                };
        }

        if (personajes.Length >= 2){
                img_poderes_personaje_2 = new Sprite[4]{
                    Resources.Load <Sprite>("poderes/" + personajes[1].poderes[0].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[1].poderes[1].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[1].poderes[2].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[1].poderes[3].imagen)
                };
        }

        if (personajes.Length >= 3){
                img_poderes_personaje_3 = new Sprite[4]{
                    Resources.Load <Sprite>("poderes/" + personajes[2].poderes[0].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[2].poderes[1].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[2].poderes[2].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[2].poderes[3].imagen)
                };
        }

        if (personajes.Length >= 4){
                img_poderes_personaje_4 = new Sprite[4]{
                    Resources.Load <Sprite>("poderes/" + personajes[3].poderes[0].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[3].poderes[1].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[3].poderes[2].imagen),
                    Resources.Load <Sprite>("poderes/" + personajes[3].poderes[3].imagen)
                };
        }


        imagen_puntero = GameObject.Find("puntero_turno_alidado");
        // ASIGNAMOS LOS BOTONES AL PRIMER PERSONAJE
        for(int j = 0; j < personajes.Length; j++){
            if(personaje_en_turno.nombre.Contains(personajes[j].nombre)){
                index_personaje_en_turno = j;
                break;
            }
        }

        GameObject prefab_pj_turno = null;
        
         if(key_personaje_turno.Contains("enemigo")){

            //ASIGNAMOS EL PUNTERO
            Mover_puntero_personaje(index_personaje_en_turno);
            Debug.Log("personaje en turno: " + personaje_en_turno.nombre);

            //EJECUTAMOS UNA ACCION DE LA IA DE ATAQUE Y RECIBIMOS UNA MATRIZ CON LAS LISTAS PERSONAJES Y ENEMIGOS
            matrix_envio_personajes = maquina.Ejecutar(enemigos, personajes, personaje_en_turno);

            //INICIALIZAMOS LA ANIMACION DEL ENEMIGO
            GameObject personaje = GameObject.Find(personaje_en_turno.nombre + "_enemigo");
            Poderes poder_lanzado_enemigo = maquina.GetPoderLanzado();
            int index_objetivo_lanzado_enemigo = maquina.GetIndexObjetivo();
            
            //CORREMOS LA ANIMACION, CUANDO ACABE ACTUALIZAREMOS LAS BARRAS DE VIDA Y PASAREMOS TURNO
            Activar_animacion(personaje, poder_lanzado_enemigo, true, index_personaje_en_turno, index_objetivo_lanzado_enemigo.ToString());

            //LOS PASAMOS DE LA MATRIZ A NUESTRAS LISTAS GLOBALES DE PERSONAJES Y ENEMIGOS
            Matrix_to_array(matrix_envio_personajes);

         }else{
            Mover_puntero_personaje(index_personaje_en_turno);
            Asignar_botones_turno(personaje_en_turno);

            prefab_pj_turno = GameObject.Find(personaje_en_turno.nombre + "_aliado");
            animator = prefab_pj_turno.GetComponent<Animator>();

            Debug.Log("personaje en turno: " + personaje_en_turno.nombre);
         }

    }


    void Update()
    {  

        //SI ACABAMOS TURNO
        if (turno_finalizado){
            //SI ES ENEMIGO REVISAMOS BUFFOS, DEBUFOS Y LLAMAMOS A LA IA PARA QUE EJECUTE UNA ACCION
            if(key_personaje_turno.Contains("enemigo")){
                Debug.Log("enemigo jugando: " + personaje_en_turno.nombre);
                personaje_en_turno.Reducir_cooldown();
                Personajes[] personaje_para_revisar_buffos = new Personajes[1]{personaje_en_turno};
                for(int j = 0; j < enemigos.Length; j++){
                    if(personaje_en_turno.nombre.Contains(enemigos[j].nombre)){
                        index_personaje_en_turno = j;
                        break;
                    }
                }
                //Y REVISAMOS SI TOCA MODIFICARLE LOS BUFFOS / DEBUFFOS
                mecanicas_combate.buffear(personaje_para_revisar_buffos, index_personaje_en_turno, enemigos);
                mecanicas_combate.debuffear(personaje_para_revisar_buffos, index_personaje_en_turno, personajes, enemigos);

                //EN CASO DE ESTAR NOQUEADO, PASAR TURNO
                if(personaje_en_turno.estado_alterado.ContainsKey("dormir") || personaje_en_turno.estado_alterado.ContainsKey("congelar") || personaje_en_turno.estado_alterado.ContainsKey("aturdir")){
                    Debug.Log(personaje_en_turno.nombre +  " esta con estado alterado o muerto y pasara turno");
                    pasar_turno();
                } else if ( personaje_en_turno.estado_alterado.ContainsKey("muerto")){
                    Debug.Log(personaje_en_turno.nombre +  " esta muerto");
                    pasar_turno();
                } else{

                    //ASIGNAMOS EL PUNTERO
                    Mover_puntero_personaje(index_personaje_en_turno);

                    //LO PONEMOS EN TURNO, QUITAMOS EL BOOLEAN DE PASAR TURNO
                    turno_finalizado = false;

                    //RECIBIMOS UNA MATRIZ CON LAS LISTAS PERSONAJES Y ENEMIGOS
                    matrix_envio_personajes = maquina.Ejecutar(enemigos, personajes, personaje_en_turno);

                    //RECIBIMOS EL PODER QUE FUE LANZADO Y EL INDEX HACIA QUIEN FUE
                    Poderes poder_lanzado_enemigo = maquina.GetPoderLanzado();
                    int index_objetivo_lanzado_enemigo = maquina.GetIndexObjetivo();

                    //INICIALIZAMOS LA ANIMACION DEL ENEMIGO
                    GameObject personaje = GameObject.Find(personaje_en_turno.nombre + "_enemigo");

                    //CORREMOS LA ANIMACION, CUANDO ACABE ACTUALIZAREMOS LAS BARRAS DE VIDA Y PASAREMOS TURNO
                    Activar_animacion(personaje, poder_lanzado_enemigo, true, index_personaje_en_turno, index_objetivo_lanzado_enemigo.ToString());

                    //LOS PASAMOS DE LA MATRIZ A NUESTRAS LISTAS GLOBALES DE PERSONAJES Y ENEMIGOS
                    Matrix_to_array(matrix_envio_personajes);
                
                }
            }else{
                //SI ES ALIADO, REVISAMOS QUIEN ES
                Debug.Log("personaje en turno: " + personaje_en_turno.nombre);
                personaje_en_turno.Reducir_cooldown();
                Personajes[] personaje_para_revisar_buffos = new Personajes[1]{personaje_en_turno};
                for(int j = 0; j < personajes.Length; j++){
                    if(personaje_en_turno.nombre.Contains(personajes[j].nombre)){
                        index_personaje_en_turno = j;
                        break;
                    }
                }
                //Y REVISAMOS SI TOCA MODIFICARLE LOS BUFFOS / DEBUFFOS
                mecanicas_combate.buffear(personaje_para_revisar_buffos, index_personaje_en_turno, personajes);
                mecanicas_combate.debuffear(personaje_para_revisar_buffos, index_personaje_en_turno, enemigos, personajes);

                //EN CASO DE ESTAR NOQUEADO, PASAR TURNO
                if(personaje_en_turno.estado_alterado.ContainsKey("dormir") || personaje_en_turno.estado_alterado.ContainsKey("congelar") || personaje_en_turno.estado_alterado.ContainsKey("aturdir") ){
                    Debug.Log(personaje_en_turno.nombre +  " esta con estado alterado y pasara turno");
                    pasar_turno();
                }else if (personaje_en_turno.estado_alterado.ContainsKey("muerto") )
                {
                    Debug.Log(personaje_en_turno.nombre +  " esta muerto y pasara turno");
                    pasar_turno();
                }else{
                    //ASIGNAMOS BOTONES AL JUGADOR EN TURNO
                    Asignar_botones_turno(personaje_en_turno);
                    Mover_puntero_personaje(index_personaje_en_turno);
                    turno_finalizado = false;
                } 
            }
        }


        int contador_muertos = 0;
        for(int k = 0; k < personajes.Length; k++)
        {
            if (personajes[k].estado_alterado.ContainsKey("muerto")) contador_muertos ++;
            else break;
            if (contador_muertos >= personajes.Length)
            {
                //DEJAMOS EL TURNO SIN AVANZAR
                turno_finalizado = false;

               //BORRAMOS LOS PREFABS DEL JUEGO
               Limpiar_mapa_gameobjects();

                //DAMOS RECOMPENZAS POR PERDER
                if ( !fin_juego.activeSelf )Agregar_recompenzas(false);    

                //PONEMOS EL TEXTO DE PERDER
                Text txt_fin_juego = GameObject.Find("txt_fin_del_juego").GetComponent<Text>();
                txt_fin_juego.text = "Partida Perdida";
            }
        }

        contador_muertos = 0;
        for(int k = 0; k < enemigos.Length; k++)
        {
            if (enemigos[k].estado_alterado.ContainsKey("muerto")) contador_muertos ++;
            else break;
            if (contador_muertos >= enemigos.Length)
            {
                //DEJAMOS EL TURNO SIN AVANZAR
                turno_finalizado = false;

                //BORRAMOS LOS PREFABS DEL JUEGO
                Limpiar_mapa_gameobjects();

                //DAMOS RECOMPENZAS POR GANAR
                if ( !fin_juego.activeSelf )Agregar_recompenzas(true);

                //PONEMOS EL TEXTO DE GANAR
                Text txt_fin_juego = GameObject.Find("txt_fin_del_juego").GetComponent<Text>();
                txt_fin_juego.text = "Partida Ganada";

            }
        }

    }


    void Asignar_botones_turno(Personajes actual){
        //CAMBIAMOS TEXTOS EN LOS BOTONES
        GameObject texto_nivel;
        for(int i = 1; i < 5; i++){
            texto_nivel = GameObject.Find("texto_poder_"+i);
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = actual.poderesActivos[i-1].nombre;
        }

        //AGREGAMOS UN LISTENER A CADA BOTON
        btn_poder_1.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[0]); });
        btn_poder_2.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[1]); });
        btn_poder_3.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[2]); });
        btn_poder_4.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[3]); });

        //AGREGAMOS UN TEXTO DESCRIPTIVO A CADA BOTON
        btn_poder_1.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = actual.poderesActivos[0].descripcion;
        btn_poder_2.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = actual.poderesActivos[1].descripcion;
        btn_poder_3.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = actual.poderesActivos[2].descripcion;
        btn_poder_4.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = actual.poderesActivos[3].descripcion;

        //ACTIVAMOS EL UI DE COOLDOWN EN LOS PODERES QUE LO TIENEN
        Button btn_iteracion;
        for(int i = 0; i < 4; i++){
            btn_iteracion = (i == 0)? btn_poder_1 : (i == 1) ? btn_poder_2 : (i == 2) ? btn_poder_3 : btn_poder_4;
            if(actual.poderesActivos[i].se_puede_usar == false){
                btn_iteracion.transform.GetChild(2).gameObject.SetActive(true);
                btn_iteracion.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Text>().text = actual.poderesActivos[i].reutilizacion_actual.ToString();
            }else{
                btn_iteracion.transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        //BUSCAMOS EL ID DEL PERSONAJE ACTUAL
        int index = 0;
        for(int i = 0; i < personajes.Length; i++)
        {
            if (personajes[i] != null)
            {
                if (actual.nombre == personajes[i].nombre)
                {
                    index = i;
                    break;
                }
            } 
        }
        
        //ASIGNAMOS LAS IMAGENES A SUS PODERES
        switch(index)
        {
            case 0:
                poder_1.GetComponent<Image>().sprite = img_poderes_personaje_1[0];
                poder_2.GetComponent<Image>().sprite = img_poderes_personaje_1[1];
                poder_3.GetComponent<Image>().sprite = img_poderes_personaje_1[2];
                poder_4.GetComponent<Image>().sprite = img_poderes_personaje_1[3];
                break;
            case 1:
                poder_1.GetComponent<Image>().sprite = img_poderes_personaje_2[0];
                poder_2.GetComponent<Image>().sprite = img_poderes_personaje_2[1];
                poder_3.GetComponent<Image>().sprite = img_poderes_personaje_2[2];
                poder_4.GetComponent<Image>().sprite = img_poderes_personaje_2[3];
                break;
            case 2:
                poder_1.GetComponent<Image>().sprite = img_poderes_personaje_3[0];
                poder_2.GetComponent<Image>().sprite = img_poderes_personaje_3[1];
                poder_3.GetComponent<Image>().sprite = img_poderes_personaje_3[2];
                poder_4.GetComponent<Image>().sprite = img_poderes_personaje_3[3];
                break;
            case 3:
                poder_1.GetComponent<Image>().sprite = img_poderes_personaje_4[0];
                poder_2.GetComponent<Image>().sprite = img_poderes_personaje_4[1];
                poder_3.GetComponent<Image>().sprite = img_poderes_personaje_4[2];
                poder_4.GetComponent<Image>().sprite = img_poderes_personaje_4[3];
                break;
            default:
                break;
        }
    }

    // BORRAMOS LOS LISTENERS DE LOS UI DE BOTONES AL ACABAR UN TURNO
    void Limpiar_botones_turno(){
        btn_poder_1.onClick.RemoveAllListeners();
        btn_poder_2.onClick.RemoveAllListeners();
        btn_poder_3.onClick.RemoveAllListeners();
        btn_poder_4.onClick.RemoveAllListeners();
    }

    void popular_personajes_mapa(Personajes[] personajes_jugador, Personajes[] enemigos){

        //INSTANCIAMOS LOS PERSONAJES DEL JUGADOR
        float[] pos_inicial_x = {-5.22F, -6.84F, -4.74F, -2.14F};
        float[] pos_inicial_y = {-1.74F, 0.46F, 1.6F, 0.14F};
        float pos_inicial_z = -0F;
        for(int i = 0; i < personajes_jugador.Length; i++){
            if (personajes_jugador[i] != null){
                GameObject personaje_creado = Instantiate( Resources.Load("prefabs_personajes/" + personajes_jugador[i].nombre), new Vector3(pos_inicial_x[i], pos_inicial_y[i], pos_inicial_z), Quaternion.identity) as GameObject;
                personaje_creado.transform.SetParent(GameObject.Find("personajes").transform, false);
                personaje_creado.transform.rotation = Quaternion.Euler(0, 180f, 0);

                //LE CAMBIAMOS EL NOMBRE
                personaje_creado.name = personajes_jugador[i].nombre + "_aliado";

                //ACITVAMOS LA BARRA DE VIDA CORRESPONDIENTE Y LE SETEAMOS LA SALUD MAXIMA
                GameObject objeto_barra_vida = this.Objs_barras_vidas.Find(x => x.name == "barra_vida_"+i);
                objeto_barra_vida.SetActive(true);

                Barra_vida barra_vida = objeto_barra_vida.GetComponent<Barra_vida>();
                barra_vida.Set_salud_maxima(personajes_jugador[i].atributos.vitalidad);

                //MOSTRAMOS LA VIDA DEL PERSONAJE ARRIBA EN PANTALLA
                Text txt_vida = GameObject.Find("vida_"+i).GetComponent<Text>();
                txt_vida.text = personajes_jugador[i].atributos.salud.ToString() + " / " + personajes_jugador[i].atributos.vitalidad.ToString();


                //AGREGAMOS UN EVENTO CUANDO SE SELECCIONE EL EPRSONAJE
                int index = i;
                EventTrigger trigger = personaje_creado.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener(delegate { Confirmar_poder(index.ToString()); });
                trigger.triggers.Add(entry);
            }
            
        }

        //INSTANCIAMOS LOS ENEMIGOS
        float[] pos_inicial_x_enemigos = {2.78F, 4.66F, 7.41F, 6.43F};
        float[] pos_inicial_y_enemigos = {-0.42F, 1.82F, 1.22F, -1.89F};
        for(int i = 0; i < enemigos.Length; i++){
            if (enemigos[i] != null){
                GameObject personaje_creado = Instantiate(Resources.Load("prefabs_personajes/" + enemigos[i].nombre), new Vector3(pos_inicial_x_enemigos[i], pos_inicial_y_enemigos[i], pos_inicial_z), Quaternion.identity) as GameObject;
                personaje_creado.transform.SetParent(GameObject.Find("personajes").transform, false);

                //LE CAMBIAMOS EL NOMBRE
                personaje_creado.name = enemigos[i].nombre + "_enemigo";

                //ACITVAMOS LA BARRA DE VIDA CORRESPONDIENTE Y LE SETEAMOS LA SALUD MAXIMA
                GameObject objeto_barra_vida = this.Objs_barras_vidas.Find(x => x.name == "barra_vida_enemigo_"+i);
                objeto_barra_vida.SetActive(true);

                Barra_vida barra_vida = objeto_barra_vida.GetComponent<Barra_vida>();
                barra_vida.Set_salud_maxima(enemigos[i].atributos.vitalidad);
                
                //MOSTRAMOS LA VIDA DEL PERSONAJE ARRIBA EN PANTALLA
                Text txt_vida = GameObject.Find("vida_enemigo_"+i).GetComponent<Text>();
                txt_vida.text = enemigos[i].atributos.salud.ToString() + " / " + enemigos[i].atributos.vitalidad.ToString();
                
                //AGREGAMOS UN EVENTO CUANDO SE SELECCIONE EL EPRSONAJE
                int index = i;
                EventTrigger trigger = personaje_creado.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener(delegate { Confirmar_poder(index.ToString()); });
                trigger.triggers.Add(entry);
            }
        }
    }

    void AsignarPoder(Poderes poder){
        //ACTIVAMOS LOS UI DE TEXTO OBJETIVO
        string objetivos = poder.objetivos;
        switch(objetivos){
            case "unico":
                multiple_objetivo_txt.SetActive(false);
                objetivo_unico_txt.SetActive(true);
                break;
            case "multiple":
                objetivo_unico_txt.SetActive(false);
                multiple_objetivo_txt.SetActive(true);
                break;
            case "propio":
                objetivo_unico_txt.SetActive(false);
                multiple_objetivo_txt.SetActive(false);
                propio_objetivo_txt.SetActive(true);
                break;
        }

        //ASIGNAMOS DE FORMA GLOBAL EL PODER QUE LANZAREMOS
        poder_a_ser_lanzado = poder;
    }

    void Confirmar_poder(string index){
         GameObject personaje = null;
        if (poder_a_ser_lanzado != null && poder_a_ser_lanzado.se_puede_usar){
            mecanicas_combate.Lanzar_poder(index, poder_a_ser_lanzado, personajes, enemigos, personaje_en_turno);

            //INICIALIZAMOS LA ANIMACION
            personaje = GameObject.Find(personaje_en_turno.nombre + "_aliado");

            //CORREMOS LA ANIMACION, CUANDO ACABE ACTUALIZAREMOS LAS BARRAS DE VIDA Y PASAREMOS TURNO
            Activar_animacion(personaje, poder_a_ser_lanzado, false, index_personaje_en_turno, index);

        }else{
            return;
        }

        //DESACTIVAMOS LOS UI DE TEXTO OBJETIVO
        string objetivos = poder_a_ser_lanzado.objetivos; // UNICO = FALSE, MULTIPLE = TRUE
        switch(objetivos){
            case "unico":
                objetivo_unico_txt.SetActive(false);
                break;
            case "multiple":
                multiple_objetivo_txt.SetActive(false);
                break;
            case "propio":
                propio_objetivo_txt.SetActive(false);
                break;

        }

        //PONEMOS NULL EL PODER A SER LANZADO DE FORMA GLOBAL
        poder_a_ser_lanzado = null;
    }


    //FUNCION ENCARGADA DE INICIAR UNA ANIMACION DE COMBATE
    public void Activar_animacion(GameObject personaje, Poderes poder_lanzado, bool es_enemigo, int index_personaje, string index_objetivo)
    {       
            //SACAMOS EL COMPONENTE DE ANIMACION, EL NOMBRE DEL CLIP Y DEL TRIGGER QUE ACTIVARA LA ANIMACION
            this.animator = personaje.GetComponent<Animator>();
            string  nombre_animacion = "";
            string nombre_clip = "";
            string bando = (es_enemigo) ? "enemigo" : "aliado";

            //CORREMOS LA ANIMACION DEL PODER MOSTRADO EN PANTALLA
            mostrar_poder.GetComponent<Image>().sprite = Resources.Load <Sprite>("poderes/" + poder_lanzado.imagen);
            Animator animator_poder = mostrar_poder.GetComponent<Animator>();
            animator_poder.SetBool("mostrar_poder", true); // se activa la animacion del poder en pantalla
            AnimationClip[] clips = animator_poder.runtimeAnimatorController.animationClips;
            corrutina = Terminar_animacion("mostrar_poder", clips[0].length - 0.5F, "ninguno", animator_poder, "");
            StartCoroutine(corrutina);

            //VEMOS SI EL TRIGER QUE ACTIVARA LA ANIMACION ES PARA LA MAGIA O PARA EL DAÑO NORMAL
            if (poder_lanzado.atributo == "magia" || poder_lanzado.objetivos != "unico" || poder_lanzado.atributo == "defensa_magia")
            {
                nombre_animacion = index_personaje + "_magia";
                nombre_clip = index_personaje + "_basico_magia";
            }else{
                nombre_animacion = index_personaje + "_melee_" + index_objetivo;
                nombre_clip = index_personaje + "_melee_" + index_objetivo;
            }
            if(es_enemigo) this.animator.SetBool("enemigo", true); // especificamos que es un enemigo para asegurar la animacion de enemigo
            this.animator.SetBool(nombre_animacion, true); // justo aqui se activa la animacion

            //TOMAMOS TODOS LOS CLIPS Y BUSCAMOS EL ACTUAL PARA HALLAR SU TIEMPO DE DURACION
            clips = this.animator.runtimeAnimatorController.animationClips;
            int index_clip_actual = 0;
            foreach(AnimationClip clip in clips)
             {
                 if (clip.name == nombre_clip){
                     break;
                 }
                 index_clip_actual++;
            }
            Debug.Log(clips[index_clip_actual].name);

            //SI TENEMOS QUE EL PODER ES PARA VARIOS OBJETIVOS, CAMBIAMOS EL INDEX A TODOS
            if (poder_lanzado.objetivos == "multiple") index_objetivo = "multiple";
            if (poder_lanzado.objetivos == "propio") index_objetivo = "propio";
            if (poder_lanzado.tipo_poder == "buff") index_objetivo = "buff";

            //ENVIAMOS EL ELEMENTO DEL PERSONAJE QUE LANZO EL PODER
            string elemento = "";
            switch(this.personaje_en_turno.elemento)
            {
                case "fuego":
                    elemento = "_fuego";
                    break;
                case "agua":
                    elemento = "_agua";
                    break;
                case "tierra":
                    elemento = "_tierra";
                    break;
                case "trueno":
                    elemento = "_trueno";
                    break;
                case "luz":
                    elemento = "_luz";
                    break;
                case "oscuridad":
                    elemento = "_oscuridad";
                    break;
                default:
                    break;
            }
            //CORREMOS UNA SUB RUTINA PARA CERRAR LA ANIMACION, QUE SE EJECUTARA LUEGO DEL TIEMPO DE DURACION DE LA ANIMACION
            corrutina = Terminar_animacion(nombre_animacion, clips[index_clip_actual].length - 0.5F, bando, null, index_objetivo, elemento);
            StartCoroutine(corrutina); // un hilo para terminar la animacion
    }



    //FUNCION ENCARGADA DE CORTAR LA ANIMACION CUANDO TERMINE
     private IEnumerator Terminar_animacion(string nombre_animacion, float tiempo_espera, string bando, Animator anim, string index_objetivo, string elemento = "")
     {
         //SI ANIM = NULL ESTAMOS ANIMANDO EL PODER QUE SALE EN PANTALLA, SI NO UN PERSONAJE
        if (anim == null) anim = this.animator;
        //ESPERAMOS EL TIEMPO DEL CLIP - 0.5 SEGUNDOS PARA DESACTIVARLO, 0.5 MENOS PARA ASEGURAR QUE EVITAMOS UNA REPETICION DEL CLIP
        yield return new WaitForSeconds(tiempo_espera);
        try{

            //SI TENEMOS UN ENEMIGO O UN ALIADO HACIENDO ANIMACION, ESTO NO SE EJECUTA SI ESTAMOS ANIMANDO LA IMAGEN DEL PODER EN EL MAPA
            if (bando == "enemigo" || bando == "aliado")
            {
                anim.SetBool("enemigo", false); //PONEMOS LA VARIABLE DE ENEMIGO SIEMPRE EN FALSO AL TERMINAR, NO IMPORTA SI ESTABA EN FALSO.
        
                //ACTIVAMOS LAS PARTICULAS A LOS OBJETIVOS AFECTADOS POR EL PODER TIRADO
                Activar_particulas(index_objetivo, bando, elemento);

                //ACTUALIZAMOS LAS VIDAS DE TODOS EN EL MAPA
                Cambiar_texto_vida(null, 99, true);
                Cambiar_texto_vida(null, 99, false);
            }

            //DETENEMOS LA ANIMACION
            anim.SetBool(nombre_animacion, false);
        }catch{
                Debug.Log("acabamos el juego");
        }   

         //ESPERAMOS 0,7 SEGUNDOS MAS, PARA QUE TEMRINE LA ANIMACION SI NO LO HA HECHO Y PASAMOS TURNO
         //ESTO NO SE EJECUTA SI ESTAMOS ANIMANDO UN PODER LANZADO
        if (bando == "enemigo" || bando == "aliado")
        {
            yield return new WaitForSeconds(0.7F);
            pasar_turno();
        }
     }

     void Activar_particulas(string index_objetivo, string bando, string elemento)
     {
         Color color = new Color(0,0,0,0);
         if (elemento.Contains("fuego")) color = new Color(1f, 0f, 0f, 1f);
         if (elemento.Contains("agua")) color = new Color(0f, 1f, 1f, 1f);
         if (elemento.Contains("tierra")) color = new Color(0.78f, 0.55f, 0.25f, 1f);
         if (elemento.Contains("trueno")) color = new Color(1f, 0.92f, 0.016f, 1f);
         if (elemento.Contains("luz")) color = new Color(1f, 1f, 1f, 1f);
         if (elemento.Contains("oscuridad")) color = new Color(0f, 0f, 0f, 1f);

        //ACTIVAMOS LAS PARTICULAS PARA SIMULAR UN GOLPE
        string objetivo_particulas_nombre = (bando == "enemigo") ? "Particulas_" : "Particulas_enemigo_";
        if (index_objetivo.Contains("multiple")){
            int cantidad_objetivos = (bando == "enemigo") ? this.personajes.Length : this.enemigos.Length;
            for(int i = 0; i < this.personajes.Length; i++)
            {
                GameObject particulas_golpe = GameObject.Find(objetivo_particulas_nombre + i);

                //CAMBIAMOS EL COLOR AL ELEMENTO DEL PERSONAJE
                ParticleSystem.MainModule settings = particulas_golpe.GetComponent<ParticleSystem>().main;
                settings.startColor = color;

                particulas_golpe.GetComponent <ParticleSystem>().Play();
            }
        }else if (index_objetivo.Contains("buff")){
            int cantidad_objetivos = (bando == "enemigo") ? this.enemigos.Length : this.personajes.Length;
            for(int i = 0; i < this.personajes.Length; i++)
            {
                objetivo_particulas_nombre = (bando == "enemigo") ? "Particulas_enemigo_" : "Particulas_";
                GameObject particulas_golpe = GameObject.Find(objetivo_particulas_nombre + i);

                //CAMBIAMOS EL COLOR A VERDE SI ES UN BUFF
                ParticleSystem.MainModule settings = particulas_golpe.GetComponent<ParticleSystem>().main;
                settings.startColor = new Color(0f, 1f, 0f, 1f);

                particulas_golpe.GetComponent <ParticleSystem>().Play();
            }
        }else{
            GameObject particulas_golpe = GameObject.Find(objetivo_particulas_nombre + index_objetivo);

            //CAMBIAMOS EL COLOR AL ELEMENTO DEL PERSONAJE
            ParticleSystem.MainModule settings = particulas_golpe.GetComponent<ParticleSystem>().main;
            settings.startColor = color;

            particulas_golpe.GetComponent <ParticleSystem>().Play();
        }
     }


        void Cambiar_texto_vida(Personajes target, int index_objetivo, bool aliado){

        if (aliado == false){
            //MOSIFICAMOS LA VIDA DE TODOS LOS ENEMIGO SI NOS LLEGA (99 = TODOS LOS OBJETIVOS)
            if (index_objetivo == 99){
                for(int i = 0; i < enemigos.Length; i++){
                    GameObject canvas_texto = GameObject.Find("vida_enemigo_"+i);
                    Barra_vida barra_vida = GameObject.Find("barra_vida_enemigo_"+i).GetComponent<Barra_vida>();
                    //FLOAT PARA TENER UN MEJOR CONTROL DEL SLIDER
                    float saludFloat = (enemigos[i].atributos.salud > 0F) ? enemigos[i].atributos.salud : 0F;
                    barra_vida.Set_salud(saludFloat);
                    //INT PARA NO VER NUMEROS LARGOS EN LA PANTALLA
                    string salud = (Convert.ToInt32(enemigos[i].atributos.salud) > 0) ? Convert.ToInt32(enemigos[i].atributos.salud).ToString() : "0";
                    canvas_texto.GetComponent<Text>().text = salud + " / " + enemigos[i].atributos.vitalidad.ToString();

                }
            }else{
                //MODIFICAMOS LA VIDA DE 1 ENEMIGO ARRIBA EN PANTALLA
                GameObject canvas_texto = GameObject.Find("vida_enemigo_"+index_objetivo);
                Barra_vida barra_vida = GameObject.Find("barra_vida_enemigo_"+index_objetivo).GetComponent<Barra_vida>();
                float saludFloat = (target.atributos.salud > 0F) ? target.atributos.salud : 0F;
                barra_vida.Set_salud(saludFloat);
                string salud = (Convert.ToInt32(target.atributos.salud) > 0) ? Convert.ToInt32(target.atributos.salud).ToString() : "0";
                canvas_texto.GetComponent<Text>().text = salud + " / " + target.atributos.vitalidad.ToString();
            }
        }else{
            //MOSIFICAMOS LA VIDA DE TODOS LOS PERSONAJES SI NOS LLEGA (99 = TODOS LOS OBJETIVOS)
            if (index_objetivo == 99){
                for(int i = 0; i < personajes.Length; i++){
                    GameObject canvas_texto = GameObject.Find("vida_"+i);
                    Barra_vida barra_vida = GameObject.Find("barra_vida_"+i).GetComponent<Barra_vida>();
                    float saludFloat = (personajes[i].atributos.salud > 0F) ? personajes[i].atributos.salud : 0F;
                    barra_vida.Set_salud(saludFloat);
                    string salud = (Convert.ToInt32(personajes[i].atributos.salud) > 0) ? Convert.ToInt32(personajes[i].atributos.salud).ToString() : "0";
                    canvas_texto.GetComponent<Text>().text = salud + " / " + personajes[i].atributos.vitalidad.ToString();
                }
            }else{
                //MODIFICAMOS LA VIDA DE 1 PERSONAJE ARRIBA EN PANTALLA
                GameObject canvas_texto = GameObject.Find("vida_"+index_objetivo);
                Barra_vida barra_vida = GameObject.Find("barra_vida_"+index_objetivo).GetComponent<Barra_vida>();
                float saludFloat = (target.atributos.salud > 0F) ? target.atributos.salud : 0F;
                barra_vida.Set_salud(saludFloat);
                string salud = (Convert.ToInt32(target.atributos.salud) > 0) ? Convert.ToInt32(target.atributos.salud).ToString() : "0";
                canvas_texto.GetComponent<Text>().text = salud + " / " + target.atributos.vitalidad.ToString();
            }
        }
        
    }

    void pasar_turno(){
        //AUMENTAMOS TODAS LAS VELOCIDADES SEGUN EL ARREGLO DE VELOCIDADES INICIALES
        for(int i = 0; i < personajes.Length; i++){ 
            personajes[i].atributos.velocidad += velocidades_personajes[i];
        }

        for(int i = 0; i < enemigos.Length; i++){
             enemigos[i].atributos.velocidad += velocidades_enemigos[i];
        }
        //VEMOS SI TENEMOS UN ALIADO O UN ENEMIGO COMO JUGADOR EN TURNO
        bool aliado = (key_personaje_turno.Contains("aliado"))? true: false; //TRUE = ALIADO, FALSE = ENEMIGO
        string nombre_personaje_turno = personaje_en_turno.nombre;
        //BUSCAMOS SU NOMBRE EN EL ARREGLO CORRESPONDIENTE Y REDUCIMOS SU VELOCIDAD A 0
        int j = 0;
        bool velocidad_cambiada = false;

        if (aliado == true){
            while(j < personajes.Length && !velocidad_cambiada){
                if(nombre_personaje_turno.Contains(personajes[j].nombre)){
                    velocidad_cambiada = true;
                    personajes[j].atributos.velocidad = 0;
                }
                j++;
            }
        }else{
            while(j < enemigos.Length && !velocidad_cambiada){
                if(nombre_personaje_turno.Contains(enemigos[j].nombre)){
                    velocidad_cambiada = true;
                    enemigos[j].atributos.velocidad = 0;
                }
                j++;
            }
        }
        //LIMPIAMOS EL MAPA
        turno.Clear();
        //AGREGAMOS TODOS LOS ALIADOS Y ENEMIGOS AL MAPA
        for(int i = 0; i < personajes.Length; i++){
            turno["aliado"+i] = personajes[i];
        }

        for(int i = 0; i < enemigos.Length; i++){   
            turno["enemigo"+i] = enemigos[i];
        }
        //ORDENAMOS EL MAPA EN ORDEN DE VELOCIDADES
        turno = turno.OrderByDescending(key => key.Value.atributos.velocidad).ToDictionary(x => x.Key, x => x.Value);
        //ACTIVAMOS EL BOOLEANO DE TURNO FINALIZADO
        turno_finalizado = true;
        //LIMPIAMOS LOS BOTONES DE TURNO
        Limpiar_botones_turno();
        //retornamos el nuevo personaje en turno
        foreach (KeyValuePair<string, Personajes> pj in turno)
        {
            personaje_en_turno = pj.Value;
            key_personaje_turno = pj.Key;
            break;
        } 
    }

    void Mover_puntero_personaje(int index_personaje_en_turno){
        RectTransform dragArea;
        if (imagen_puntero != null){
            if (key_personaje_turno.Contains("enemigo")){
                imagen_puntero. GetComponent<Image>().sprite = puntero_rojo;
                switch(index_personaje_en_turno){
                    case 0:
                        dragArea = imagen_puntero.GetComponent<RectTransform>();
                        dragArea.localPosition = new Vector3(201f, 35f, 0f);
                        break;
                    case 1:
                        dragArea = imagen_puntero.GetComponent<RectTransform>();
                        dragArea.localPosition = new Vector3(340F, 207F, 0f);
                        break;
                    case 2:
                        dragArea = imagen_puntero.GetComponent<RectTransform>();
                        dragArea.localPosition = new Vector3(539f, 158F, 0f);
                        break;
                    case 3:
                        dragArea = imagen_puntero.GetComponent<RectTransform>();
                        dragArea.localPosition = new Vector3(451f, 262f, 0f);
                        break;
                    default:
                        break;
                }
            }else{
                imagen_puntero. GetComponent<Image>().sprite = puntero_verde;
                switch(index_personaje_en_turno){
                    case 0:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(-382f, -56f, 0f);
                    break;
                    case 1:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(-490F, 109F, 0f);
                    break;
                    case 2:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(-349f, 190F, 0f);
                    break;
                    case 3:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(-157f, 84f, 0f);
                    break;
                    default:
                    break;
                }
            }
        }
    }

    //OBTENEMOS LA MATRIX DE PERSONAJES Y ENEMIGOS QUE LLEGO DE LA INTELEGENCIA ARTIFICIAL.
    private void Matrix_to_array(Personajes[,] matrix){
        for(int i = 0; i < matrix.GetLength(1); i++)
        {
            //UN TRY CATCH, YA QUE ALGUNO DE LOS 2 ARREGLOS DE PERSONAJE PUEDE SER MAS PEQUEÑO QUE EL OTRO
            try
            {
                enemigos[i] = matrix[0,i];
            }
            catch
            {
                Debug.Log("menos enemigos");
            }
        }

        for(int i = 0; i < matrix.GetLength(1); i++)
        {
            try
            {
                personajes[i] = matrix[1,i];
            }
            catch
            {
                Debug.Log("menos personajes");
            }
        }
    }

    //FUNCIONES AL TERMINAR EL JUEGO Y IR A LA VENTANA DE RECOMPENZAS
    public void Agregar_recompenzas(bool Gane)
    {
        //RESETEAMOS EL JUGADOR Y LOS ENEMIGOS
        Resetear_jugador();

        //ACTIVAMOS LA UI DE FIN DEL JUEGO
        fin_juego.SetActive(true);

        //ACTIVAMOS LA FUNCION DE RECOMPENZA CORRESPONDIENTE
        if (Gane) recompenza.Recompenzas_ganar();
        else recompenza.Recompenzas_perder();
    }

    //BORRAMOS LOS PREFABS DEL JUEGO
    public void Limpiar_mapa_gameobjects()
    {
        Destroy(GameObject.Find("personajes"));
        Destroy(GameObject.Find("barra_vidas"));
    }


    //DEVOLVEMOS LOS PERSONAJES Y ENEMIGOS DE FAVORITOS Y DEFENSA PVP A SUS ATRIBUTOS INICIALES
    public void Resetear_jugador(){
        switch(this.tipo_combate)
        {
            case "historia":
                foreach(Personajes pj in jugador.personajesFavoritos)
                {
                    pj.Resetear_personaje();
                }

                foreach(Personajes pj in storage_enemigos.enemigos)
                {
                    pj.Resetear_personaje();
                }
                break;
            case "pvp":
                foreach(Personajes pj in jugador.defensa_pvp)
                {
                    pj.Resetear_personaje();
                }

                foreach(Personajes pj in storage_enemigos.enemigos_pvp)
                {
                    pj.Resetear_personaje();
                }
                break;
            case "amistoso":
                foreach(Personajes pj in jugador.defensa_pvp)
                {
                    pj.Resetear_personaje();
                }

                foreach(Personajes pj in storage_enemigos.enemigos_pvp)
                {
                    pj.Resetear_personaje();
                }
                break;
        }
    }



// FUNCIONES DEL MENU
    public void Opciones_menu(string opcion)
    {
        //OPCIONES DENTRO DEL MENU
        switch(opcion)
        {
            case "salir":
                Resetear_jugador();
                _routing.ir_principal();
                break;
            case "configuracion":
                this.menu_configuracion.SetActive(true);
                break;
        }
    }

    //CERRAMOS EL BLOQUE NEGRO CON LAS OPCIONES DEL MENU
    public void Cerrar_menu_configuracion()
    {
        this.menu_configuracion.SetActive(false);
    }

}





