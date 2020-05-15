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

    //TABLA DEFENSAS ELEMENTALES;
    // private Dictionary<string, float[]> defensas_elementales = new Dictionary<string, float[]>(){
    //     {"agua", new float[] {1F, 1.5F, 1F, 0.5F, 1F, 1F} },
    //     {"fuego", new float[] {0.5F, 1F, 1.5F, 1F, 1F, 1F} },
    //     {"tierra", new float[] {1F, 0.5F, 1F, 1.5F, 1F, 1F} },
    //     {"trueno", new float[] {1.5F, 1F, 0.5F, 1F, 1F, 1F} },
    //     {"oscuridad", new float[] {1F, 1F, 1F, 1F, 0.5F, 1.5F} },
    //     {"luz", new float[] {1F, 1F, 1F, 1F, 1.5F, 0.5F} },
    // };

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

    //UI DE LOS PODERES
    public bool cambiar_UI_poderes = true;
    private GameObject poder_1;
    private GameObject poder_2;
    private GameObject poder_3;
    private GameObject poder_4;

    public Button btn_poder_1;
    public Button btn_poder_2;
    public Button btn_poder_3;
    public Button btn_poder_4;

    //UI CON TEXTO DE SELECCIONAR OBJETIVO / OBJETIVOS
    public GameObject objetivo_unico_txt;
    public GameObject multiple_objetivo_txt;




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
        
        //de prueba
        //fabrica = new Personajes();
        //personajes = new Personajes[4]{fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("liliana"), fabrica.Crear_personaje("martis")};
        //enemigos = new Personajes[4]{fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("martis"), fabrica.Crear_personaje("liliana")};

        //COPIAMOS LOS PERSONAJES DEL USUARIO Y DE LOS ENEMIGOS LOCALMENTE
        if (tipo_combate == "historia")
        {
            personajes = jugador.personajesFavoritos.ToArray();
            enemigos = storage_enemigos.enemigos.ToArray();
        }
        else if (tipo_combate == "pvp")
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
            Debug.Log("cantidad: " + personajes.Length);
            Debug.Log(personajes[i].nombre);
            Debug.Log(personajes[i].atributos);
            Debug.Log(personajes[i].atributos.velocidad);
            velocidades_personajes[i] = personajes[i].atributos.velocidad;
        }
        for(int i = 0; i < enemigos.Length; i++){
            velocidades_enemigos[i] = enemigos[i].atributos.velocidad;
        }

        //ASIGNAMOS EL UI DE LOS TEXTOS DE OBJETIVOS Y LOS DESHABILITAMOS POR AHORA
        objetivo_unico_txt = GameObject.Find("texto_seleccion_objetivo");
        multiple_objetivo_txt = GameObject.Find("texto_seleccion_objetivos");
        objetivo_unico_txt.SetActive(false);
        multiple_objetivo_txt.SetActive(false);
        
        //LLENAMOS EL CAMPO CON LOS PERSONAJES COPIADOS
        popular_personajes_mapa(personajes, enemigos, prefab_personaje);

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

        imagen_puntero = GameObject.Find("puntero_turno_alidado");
        // ASIGNAMOS LOS BOTONES AL PRIMER PERSONAJE
        for(int j = 0; j < personajes.Length; j++){
            if(personaje_en_turno.nombre.Contains(personajes[j].nombre)){
                index_personaje_en_turno = j;
                break;
            }
        }
        
         if(key_personaje_turno.Contains("enemigo")){
            //ASIGNAMOS EL PUNTERO
            Mover_puntero_personaje(index_personaje_en_turno);

            //RECIBIMOS UNA MATRIZ CON LAS LISTAS PERSONAJES Y ENEMIGOS
            matrix_envio_personajes = maquina.Ejecutar(enemigos, personajes, personaje_en_turno);

            //LOS PASAMOS DE LA MATRIZ A NUESTRAS LISTAS GLOBALES DE PERSONAJES Y ENEMIGOS
            Matrix_to_array(matrix_envio_personajes);
            
            //ACTUALIZAMOS LOS VALORES DE LA VIDA Y PASAMOS TURNO
            Cambiar_texto_vida(null, 99, true);
            Cambiar_texto_vida(null, 99, false);
            pasar_turno();
         }else{
            Mover_puntero_personaje(index_personaje_en_turno);
            Asignar_botones_turno(personaje_en_turno);
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
                if(personaje_en_turno.estado_alterado.ContainsKey("dormir") || personaje_en_turno.estado_alterado.ContainsKey("congelar") || personaje_en_turno.estado_alterado.ContainsKey("aturdir") || personaje_en_turno.estado_alterado.ContainsKey("muerto")){
                    Debug.Log(personaje_en_turno.nombre +  " esta con estado alterado o muerto y pasara turno");
                    pasar_turno();
                } else{

                    //ASIGNAMOS EL PUNTERO
                    Mover_puntero_personaje(index_personaje_en_turno);

                    //RECIBIMOS UNA MATRIZ CON LAS LISTAS PERSONAJES Y ENEMIGOS
                    matrix_envio_personajes = maquina.Ejecutar(enemigos, personajes, personaje_en_turno);

                    //LOS PASAMOS DE LA MATRIZ A NUESTRAS LISTAS GLOBALES DE PERSONAJES Y ENEMIGOS
                    Matrix_to_array(matrix_envio_personajes);
                    
                    //ACTUALIZAMOS LOS VALORES DE LA VIDA Y PASAMOS TURNO
                    Cambiar_texto_vida(null, 99, true);
                    Cambiar_texto_vida(null, 99, false);
                    pasar_turno();
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
            if (contador_muertos >= 4)
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
            if (contador_muertos >= 4)
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
    }

    // BORRAMOS LOS LISTENERS DE LOS UI DE BOTONES AL ACABAR UN TURNO
    void Limpiar_botones_turno(){
        btn_poder_1.onClick.RemoveAllListeners();
        btn_poder_2.onClick.RemoveAllListeners();
        btn_poder_3.onClick.RemoveAllListeners();
        btn_poder_4.onClick.RemoveAllListeners();
    }

    void popular_personajes_mapa(Personajes[] personajes_jugador, Personajes[] enemigos, GameObject prefab){

        //INSTANCIAMOS LOS PERSONAJES DEL JUGADOR
        float[] pos_inicial_x = {-5.71F, -7.990039F, -6.790027F, -4.100024F};
        float[] pos_inicial_y = {-3.72F, -1.910015F, 0.3199964F, 0.8399854F};
        float pos_inicial_z = 20F;
        for(int i = 0; i < personajes_jugador.Length; i++){
            if (personajes_jugador[i] != null){
                GameObject personaje_creado = Instantiate(prefab, new Vector3(pos_inicial_x[i], pos_inicial_y[i], pos_inicial_z), Quaternion.identity);
                personaje_creado.transform.SetParent(GameObject.Find("personajes").transform, false);
                
                //MOSTRAMOS LA VIDA DEL PERSONAJE ARRIBA EN PANTALLA
                Text txt_vida = GameObject.Find("vida_"+i).GetComponent<Text>();
                txt_vida.text = personajes_jugador[i].atributos.vitalidad.ToString();

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
        float[] pos_inicial_x_enemigos = {5.15F, 7.39F, 6.76F, 4.63F};
        float[] pos_inicial_y_enemigos = {-3.48F, -2.61F, 0.12F, 0.89F};
        for(int i = 0; i < enemigos.Length; i++){
            GameObject personaje_creado = Instantiate(prefab, new Vector3(pos_inicial_x_enemigos[i], pos_inicial_y_enemigos[i], pos_inicial_z), Quaternion.identity);
            personaje_creado.transform.SetParent(GameObject.Find("personajes").transform, false);
            //MOSTRAMOS LA VIDA DEL PERSONAJE ARRIBA EN PANTALLA
            Text txt_vida = GameObject.Find("vida_enemigo_"+i).GetComponent<Text>();
            txt_vida.text = enemigos[i].atributos.vitalidad.ToString();
            
            //AGREGAMOS UN EVENTO CUANDO SE SELECCIONE EL EPRSONAJE
            int index = i;
            EventTrigger trigger = personaje_creado.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(delegate { Confirmar_poder(index.ToString()); });
            trigger.triggers.Add(entry);
        }
    }

    void AsignarPoder(Poderes poder){
        //ACTIVAMOS LOS UI DE TEXTO OBJETIVO
        bool multi_objetivo = (poder.objetivos == "unico")? false : true; // UNICO = FALSE, MULTIPLE = TRUE
        switch(multi_objetivo){
            case false:
                multiple_objetivo_txt.SetActive(false);
                objetivo_unico_txt.SetActive(true);
                break;
            case true:
                objetivo_unico_txt.SetActive(false);
                multiple_objetivo_txt.SetActive(true);
                break;
        }

        //ASIGNAMOS DE FORMA GLOBAL EL PODER QUE LANZAREMOS
        poder_a_ser_lanzado = poder;
    }

    void Confirmar_poder(string index){
        if (poder_a_ser_lanzado != null && poder_a_ser_lanzado.se_puede_usar){
            mecanicas_combate.Lanzar_poder(index, poder_a_ser_lanzado, personajes, enemigos, personaje_en_turno);
            pasar_turno();
        }else{
            return;
        }
        
        //DESACTIVAMOS LOS UI DE TEXTO OBJETIVO
        bool multi_objetivo = (poder_a_ser_lanzado.objetivos == "unico")? false : true; // UNICO = FALSE, MULTIPLE = TRUE
        switch(multi_objetivo){
            case false:
                objetivo_unico_txt.SetActive(false);
                break;
            case true:
                multiple_objetivo_txt.SetActive(false);
                break;
        }

        //PONEMOS NULL EL PODER A SER LANZADO DE FORMA GLOBAL
        poder_a_ser_lanzado = null;

        Cambiar_texto_vida(null, 99, true);
        Cambiar_texto_vida(null, 99, false);
    }


        void Cambiar_texto_vida(Personajes target, int index_objetivo, bool aliado){

        if (aliado == false){
            //MOSIFICAMOS LA VIDA DE TODOS LOS PERSONAJES SI NOS LLEGA (99 = TODOS LOS OBJETIVOS)
            if (index_objetivo == 99){
                for(int i = 0; i < enemigos.Length; i++){
                    GameObject canvas_texto = GameObject.Find("vida_enemigo_"+i);
                    canvas_texto.GetComponent<Text>().text = Convert.ToInt32(enemigos[i].atributos.salud).ToString();
                }
            }else{
                //MODIFICAMOS LA VIDA DE 1 PERSONAJE ARRIBA EN PANTALLA
                GameObject canvas_texto = GameObject.Find("vida_enemigo_"+index_objetivo);
                canvas_texto.GetComponent<Text>().text = Convert.ToInt32(target.atributos.salud).ToString();
            }
        }else{
            //MOSIFICAMOS LA VIDA DE TODOS LOS PERSONAJES SI NOS LLEGA (99 = TODOS LOS OBJETIVOS)
            if (index_objetivo == 99){
                for(int i = 0; i < personajes.Length; i++){
                    GameObject canvas_texto = GameObject.Find("vida_"+i);
                    canvas_texto.GetComponent<Text>().text = Convert.ToInt32(personajes[i].atributos.salud).ToString();
                }
            }else{
                //MODIFICAMOS LA VIDA DE 1 PERSONAJE ARRIBA EN PANTALLA
                GameObject canvas_texto = GameObject.Find("vida_"+index_objetivo);
                canvas_texto.GetComponent<Text>().text = Convert.ToInt32(target.atributos.salud).ToString();
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
        if (key_personaje_turno.Contains("enemigo")){
            imagen_puntero. GetComponent<Image>().sprite = puntero_rojo;
            switch(index_personaje_en_turno){
                case 0:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(368f, -51f, 0f);
                    break;
                case 1:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(552F, -22F, 0f);
                    break;
                case 2:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(483f, 205F, 0f);
                    break;
                case 3:
                    dragArea = imagen_puntero.GetComponent<RectTransform>();
                    dragArea.localPosition = new Vector3(330f, 262f, 0f);
                    break;
                default:
                    break;
            }
        }else{
            imagen_puntero. GetComponent<Image>().sprite = puntero_verde;
            switch(index_personaje_en_turno){
                case 0:
                dragArea = imagen_puntero.GetComponent<RectTransform>();
                dragArea.localPosition = new Vector3(-399f, -54f, 0f);
                break;
                case 1:
                dragArea = imagen_puntero.GetComponent<RectTransform>();
                dragArea.localPosition = new Vector3(-574F, 61F, 0f);
                break;
                case 2:
                dragArea = imagen_puntero.GetComponent<RectTransform>();
                dragArea.localPosition = new Vector3(-483f, 217F, 0f);
                break;
                case 3:
                dragArea = imagen_puntero.GetComponent<RectTransform>();
                dragArea.localPosition = new Vector3(-291f, 248f, 0f);
                break;
                default:
                break;
            }
        }
        
    }


    private void Matrix_to_array(Personajes[,] matrix){
        for(int i = 0; i < matrix.GetLength(1); i++)
        {
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


    public void Agregar_recompenzas(bool Gane)
    {
        //RESETEAMOS EL PERSONAJE
        Resetear_jugador();

        //ACTIVAMOS LA UI DE FIN DEL JUEGO
        fin_juego.SetActive(true);
        if (Gane) recompenza.Recompenzas_ganar();
        else recompenza.Recompenzas_perder();
    }

    public void Limpiar_mapa_gameobjects()
    {
        //BORRAMOS LOS PREFABS DEL JUEGO
        Destroy(GameObject.Find("personajes"));
        Destroy(GameObject.Find("txt_vidas"));
    }

    public void Resetear_jugador(){
        //DEVOLVEMOS LOS PERSONAJES A SUS ATRIBUTOS INICIALES
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
        }
    }
}





