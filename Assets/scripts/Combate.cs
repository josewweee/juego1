﻿using System.Collections;
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

    //MECANICAS PERSONAJES
    private mecanicas_combate mecanicas;
    public Poderes poder_a_ser_lanzado;
    public inteligencia_artificial maquina;
    private Personajes[,] matrix_envio_personajes;

    //VELOCIDADES INICIALES DE LOS ALIADOS Y LOS ENEMIGOS
    public float[] velocidades_personajes;
    public float[] velocidades_enemigos;

    //¿HISTORIA, PVP?
    public string tipo_combate;
    public Recompenza[] recompenza;
    public Usuario ganador;
    public Usuario perdedor;

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

    void Start()
    {
       
        //TRAEMOS LAS INSTANCIAS DEL JUGADOR Y LOS ENEMIGOS
        jugador = Usuario.instancia;
        storage_enemigos = storage_script.instancia;

        
        //de prueba
        fabrica = new Personajes();
        personajes = new Personajes[4]{fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("liliana"), fabrica.Crear_personaje("martis")};
        enemigos = new Personajes[4]{fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("martis"), fabrica.Crear_personaje("liliana")};

        //COPIAMOS LOS PERSONAJES DEL USUARIO Y DE LOS ENEMIGOS LOCALMENTE
        //personajes = jugador.personajesFavoritos;
        //enemigos = storage_enemigos.enemigos;

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
        Mover_puntero_personaje(index_personaje_en_turno);
        Asignar_botones_turno(personaje_en_turno);
        Debug.Log("personaje en turno: " + personaje_en_turno.nombre);
        
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
                turno_finalizado = false;
                Debug.Log("Juego perdido");
            }
        }

        contador_muertos = 0;
        for(int k = 0; k < enemigos.Length; k++)
        {
            if (enemigos[k].estado_alterado.ContainsKey("muerto")) contador_muertos ++;
            else break;
            if (contador_muertos >= 4)
            {
                turno_finalizado = false;
                Debug.Log("Juego Ganado");
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
                
                
                //MOSTRAMOS LA VIDA DEL PERSONAJE ARRIBA EN PANTALLA
                GameObject canvas_texto = GameObject.Find("vida_"+i);
                Text texto_vida = canvas_texto.AddComponent<Text>();
                texto_vida.text = personajes_jugador[i].atributos.vitalidad.ToString();
                texto_vida.fontSize = 20;
                texto_vida.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                texto_vida.color = Color.black;

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
            
            //MOSTRAMOS LA VIDA DEL PERSONAJE ARRIBA EN PANTALLA
            GameObject canvas_texto = GameObject.Find("vida_enemigo_"+i);
            Text texto_vida = canvas_texto.AddComponent<Text>();
            texto_vida.text = enemigos[i].atributos.vitalidad.ToString();
            texto_vida.fontSize = 20;
            texto_vida.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            texto_vida.color = Color.black;
            
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
            enemigos[i] = matrix[0,i];
        }

        for(int i = 0; i < matrix.GetLength(1); i++)
        {
            personajes[i] = matrix[1,i];
            // string output = JsonUtility.ToJson(personajes[i].atributos, true);
            // Debug.Log(output);
        }
    }

    /**
    public void Lanzar_poder(string index, Poderes poder_a_ser_lanzado, Personajes[] personajes, Personajes[] enemigos, Personajes personaje_en_turno){
        if(poder_a_ser_lanzado == null) return;
        Poderes poder = poder_a_ser_lanzado;

        // TOMAMOS EL INDEX DEL PERSONAJE EN TURNO PARA MODIFICARLO
        int index_personaje_en_turno = 0;
        for(int j = 0; j < personajes.Length; j++){
            if(personaje_en_turno.nombre.Contains(personajes[j].nombre)){
                index_personaje_en_turno = j;
                break;
            }
        }
        // string output = JsonUtility.ToJson(personajes[Int32.Parse(index)].atributos, true);
        // Debug.Log(output);
        //MIRAMOS QUE PODER FUE LANZADO Y LLAMAMOS LA FUNCION CORRESPONDIENTE
        //MIRAMOS SI ES POSIBLE TIRAR SEGUN EL TIEMPO DE COOLDOWN
        if (poder.se_puede_usar){
            switch(poder.tipo_poder){ 
                case "ataque":
                    ataque(poder, index, index_personaje_en_turno, personajes, enemigos);
                    break;
                case "buff":
                    buff(poder, index, index_personaje_en_turno, personajes, enemigos);
                    break;
                case "purgar":
                    purgar();
                    break;
                case "debuff":
                    debuff(poder, index, index_personaje_en_turno, personajes, enemigos);
                    break;
                case "ataque_debuff":
                    ataque(poder, index, index_personaje_en_turno, personajes, enemigos);
                    debuff(poder, index, index_personaje_en_turno, personajes, enemigos);
                    break;
                case "ataque_buff":
                    ataque(poder, index, index_personaje_en_turno, personajes, enemigos);
                    buff(poder, index, index_personaje_en_turno, personajes, enemigos);
                    break;
                default:
                    break;
            }
            // output = JsonUtility.ToJson(personajes[Int32.Parse(index)].atributos, true);
            // Debug.Log(output);
            //PASAMOS TURNO
            pasar_turno();
            //DESACTIVAMOS LOS UI DE TEXTO OBJETIVO
            bool multi_objetivo = (poder.objetivos == "unico")? false : true; // UNICO = FALSE, MULTIPLE = TRUE
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
        }
    }

    void ataque(Poderes poder, string index_objetivo, int index_personaje_en_turno, Personajes[] personajes, Personajes[] enemigos){
        int index = int.Parse(index_objetivo);
        Personajes[] target;
        float atributo_;
        Personajes personaje_en_turno = personajes[index_personaje_en_turno];

        // MIRAMOS CON QUE ATRIBUTO SE HARA EL DAÑO
        switch(poder.atributo){
            case "fuerza":
                atributo_ = personaje_en_turno.atributos.fuerza;
                break;
            case "magia":
                atributo_ = personaje_en_turno.atributos.magia;
                break;
            case "vitalidad":
                atributo_ = personaje_en_turno.atributos.vitalidad;
                break;
            case "defensa_fisica":
                atributo_ = personaje_en_turno.atributos.defensa_fisica;
                break;
            case "defensa_magica":
                atributo_ = personaje_en_turno.atributos.defensa_magica;
                break;
            case "velocidad":
                atributo_ = personaje_en_turno.atributos.velocidad;
                break;
            default:
                atributo_ = 1;
                break;
        }
        // TOMAMOS EL OBJETIVO DEPENDIENDO DE QUIEN HAYA SIDO SELECIONADO
        if(poder.objetivos == "unico"){
            target = new Personajes[1]{enemigos[index]};
            float daño = (atributo_ * poder.multiplicador) + poder.daño_base;
            HacerDaño(target, daño, personaje_en_turno.elemento, poder.atributo, index, index_personaje_en_turno, enemigos);
        }else{
            float daño = (atributo_ * poder.multiplicador) + poder.daño_base;
            // 99 PARA INDICAR QUE AFECTAREMOS A TODOS
            HacerDaño(enemigos, daño, personaje_en_turno.elemento, poder.atributo, 99, index_personaje_en_turno, enemigos);
        }

    }


    void buff(Poderes poder, string index_objetivo, int index_personaje_en_turno, Personajes[] personajes, Personajes[] enemigos){
        
        int index = int.Parse(index_objetivo);
        Personajes[] target;
        float atributo_;
        string[] habilidades = poder.habilidades;
        Personajes personaje_en_turno = personajes[index_personaje_en_turno];

        // MIRAMOS CON QUE ATRIBUTO SE HARA EL DAÑO
        switch(poder.atributo){
            case "fuerza":
                atributo_ = personaje_en_turno.atributos.fuerza;
                break;
            case "magia":
                atributo_ = personaje_en_turno.atributos.magia;
                break;
            case "vitalidad":
                atributo_ = personaje_en_turno.atributos.vitalidad;
                break;
            case "defensa_fisica":
                atributo_ = personaje_en_turno.atributos.defensa_fisica;
                break;
            case "defensa_magica":
                atributo_ = personaje_en_turno.atributos.defensa_magica;
                break;
            case "velocidad":
                atributo_ = personaje_en_turno.atributos.velocidad;
                break;
            default:
                atributo_ = 1;
                break;
        }
        // TOMAMOS EL OBJETIVO DEPENDIENDO DE QUIEN HAYA SIDO SELECIONADO
        if(poder.objetivos == "unico"){
            target = new Personajes[1]{personajes[index]};
            //HacerDaño(target, daño, personaje_en_turno.elemento, poder.atributo, index, index_personaje_en_turno);
            buffear(target, index, personajes, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else if (poder.objetivos == "multiple"){
            // 99 PARA INDICAR QUE AFECTAREMOS A TODOS
            //HacerDaño(enemigos, daño, personaje_en_turno.elemento, poder.atributo, 99, index_personaje_en_turno);
            buffear(personajes, 99, personajes, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else{
            target = new Personajes[1]{personajes[index]};
            buffear(target, index_personaje_en_turno, personajes, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }
    }
    

    void purgar(){}
    void debuff(Poderes poder, string index_objetivo, int index_personaje_en_turno, Personajes[] personajes, Personajes[] enemigos){
        
        int index = int.Parse(index_objetivo);
        Personajes[] target;
        float atributo_;
        string[] habilidades = poder.habilidades;
        Personajes personaje_en_turno = personajes[index_personaje_en_turno];

        // MIRAMOS CON QUE ATRIBUTO SE HARA EL DAÑO
        switch(poder.atributo){
            case "fuerza":
                atributo_ = personaje_en_turno.atributos.fuerza;
                break;
            case "magia":
                atributo_ = personaje_en_turno.atributos.magia;
                break;
            case "vitalidad":
                atributo_ = personaje_en_turno.atributos.vitalidad;
                break;
            case "defensa_fisica":
                atributo_ = personaje_en_turno.atributos.defensa_fisica;
                break;
            case "defensa_magica":
                atributo_ = personaje_en_turno.atributos.defensa_magica;
                break;
            case "velocidad":
                atributo_ = personaje_en_turno.atributos.velocidad;
                break;
            default:
                atributo_ = 1;
                break;
        }
        // TOMAMOS EL OBJETIVO DEPENDIENDO DE QUIEN HAYA SIDO SELECIONADO
        if(poder.objetivos == "unico"){
            target = new Personajes[1]{enemigos[index]};
            debuffear(target, index, enemigos, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else if (poder.objetivos == "multiple"){
            // 99 PARA INDICAR QUE AFECTAREMOS A TODOS
            Debug.Log(enemigos[0].atributos.salud);
            debuffear(enemigos, 99, enemigos, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }else{
            target = new Personajes[1]{enemigos[index]};
            debuffear(target, index_personaje_en_turno, enemigos, habilidades, poder.daño_base, atributo_, poder.multiplicador_efecto, poder.duracion_efecto);
        }
    }


    void HacerDaño(Personajes[] target, float daño, string elementoAtacante, string atributo, int index_objetivo, int index_personaje_en_turno, Personajes[] enemigos){

        //REVISAMOS TODOS LOS ENEMIGOS O SOLO UNO
        for (int i= 0; i < target.Length; i++){
            string elementoDefensor = target[i].elemento;
            bool detener_daño = false;
            // REVISAMOS SI EL GOLPE SERA CRITICO
            daño = Critico(daño, personaje_en_turno.atributos.critico);
            
            // REDUCIMOS DEFENSAS
            if (atributo == "magia"){
                if (!(target[i].estado_alterado.ContainsKey("inmunidad_magica")) && !(target[i].estado_alterado.ContainsKey("inmunidad"))){
                        float porcentaje_reduccion_magia =  (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensa_magica")? personaje_en_turno.estado_alterado["ignorar_defensa_magica"][0] : 0F);
                        float porcentaje_reduccion_general = (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensas")? personaje_en_turno.estado_alterado["ignorar_defensas"][0] : 0F);
                        float reduccion_defensas = (porcentaje_reduccion_magia >= porcentaje_reduccion_general)? porcentaje_reduccion_magia : porcentaje_reduccion_general;
                        if (reduccion_defensas <= target[i].atributos.defensa_magica){
                            daño -= daño * ((target[i].atributos.defensa_magica / 100) - (reduccion_defensas / 100));
                        }
                }else{
                    detener_daño = true;
                }
            }else{
                if (!(target[i].estado_alterado.ContainsKey("inmunidad_fisica")) && !(target[i].estado_alterado.ContainsKey("inmunidad"))){
                    float porcentaje_reduccion_magia =  (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensa_fisica")? personaje_en_turno.estado_alterado["ignorar_defensa_fisica"][0] : 0F);
                    float porcentaje_reduccion_general = (personaje_en_turno.estado_alterado.ContainsKey("ignorar_defensas")? personaje_en_turno.estado_alterado["ignorar_defensas"][0] : 0F);
                    float reduccion_defensas = (porcentaje_reduccion_magia >= porcentaje_reduccion_general)? porcentaje_reduccion_magia : porcentaje_reduccion_general;
                    if (reduccion_defensas <= target[i].atributos.defensa_fisica){
                        daño -= daño * ((target[i].atributos.defensa_fisica / 100) - (reduccion_defensas / 100));
                    }
                }else{
                    detener_daño = true;
                }
            }
            // SI NO HAY INMUNIDADES, REVISAMOS CADA ELEMENTO PARA VER SU MULTIPLICADOR DE DAÑO
            if (!detener_daño){
                if (elementoDefensor == "agua"){
                    daño += daño * defensas_elementales[elementoAtacante][0];
                }else if (elementoDefensor == "fuego"){
                    daño += daño * defensas_elementales[elementoAtacante][1];
                }else if (elementoDefensor == "tierra"){
                    daño += daño * defensas_elementales[elementoAtacante][2];
                }else if (elementoDefensor == "trueno"){
                    daño += daño * defensas_elementales[elementoAtacante][3];
                }else if (elementoDefensor == "oscuridad"){
                    daño += daño * defensas_elementales[elementoAtacante][4];
                }else if (elementoDefensor == "luz"){
                    daño += daño * defensas_elementales[elementoAtacante][6];
                }else{
                    Debug.Log("elemento defensor erroneo");
                }
            }else{
                daño = 0;
            }
            
            // SI TENEMOS ROBO DE VIDA, NOS ENCARGAMOS DE CURARNOS
            target[i].Daño(daño);

            if (personaje_en_turno.estado_alterado.ContainsKey("robo_vida")){
                float robo_vida = daño * personaje_en_turno.estado_alterado["robo_vida"][0];
                personajes[index_personaje_en_turno].Curar(robo_vida);
            } 
            
            //SI TENEMOS UN SOLO OBJETIVO, MODIFICAMOS EL ARREGLO DE ENEMIGOS DIRECTAMENTE
            if (target.Length < 2) enemigos[index_objetivo] = target[i];

            // //SI LLEGA A 0 DE VIDA, TENDRA ESTADO MUERTO POR 999 TURNOS
            // if (target[i].atributos.vitalidad <= 0){
            //     target[i].estado_alterado["muerto"] = new float[]{0F, 9999F};
            // }
            Cambiar_texto_vida(target[i], index_objetivo, false);
        }
        
    }
    //LOS PRIMEROS DOS PARAMETROS SON OBLIGATORIOS, EL RESTO SON OPCIONALES
    void buffear(Personajes[] target, int index_objetivo, Personajes[] personajes, string[] habilidades = null, float daño_base = 0f, float atributo = 0f, float multiplicador_efecto = 0f, int duracion_efecto_int = 0){
        float efecto = 0F;
        float duracion_efecto = 0F;
        bool chequeando_bufos = false;

        //SI NOS PASAN TODOS LOS PARAMETROS, REVISAMOS CUANTO SERA EL EFECTO DEL BUFF
        if (habilidades != null){
            efecto = (atributo * multiplicador_efecto) + daño_base;
            duracion_efecto = Convert.ToSingle(duracion_efecto_int);
            duracion_efecto -= 1;
        }
        //REVISAMOS TODOS LOS PERSONAJES QUE NOS LLEGUEN
        for(int i = 0; i < target.Length; i++){
            // SI NO NOS LLEGAN TODOS LOS PARAMETROS, SIGNIFICA QUE ESTAMOS CHEQUEANDO BUFFOS, TOMAMOS UNA LISTA DE BUFFOS EN EL TARGET
            if (habilidades == null){
                habilidades = new string[target[i].estado_alterado.Count];
                int k = 0;
                foreach (KeyValuePair<string, float[]> estado in target[i].estado_alterado){
                    habilidades[k] = estado.Key;
                    k++;
                }
                chequeando_bufos = true;
            }
            for(int j = 0; j < habilidades.Length; j++){
                string habilidad = habilidades[j];
                // SI ESTAMOS CHEQUEANDO BUFFOS AGARRAMOS EL EFECTO Y LA DURACION DEL TARGET
                if (chequeando_bufos == true){
                    efecto = target[i].estado_alterado[habilidades[j]][0];
                    duracion_efecto = (target[i].estado_alterado[habilidades[j]][1]) - 1;
                }
                switch (habilidad){
                    case "curacion":
                    //MODIFICACION NORMAL A LA VIDA
                        target[i].Curar(efecto);
                        break;
                    case "revivir":
                    //REMOVEMOS UN ESTADO NEGATIVO
                        if(target[i].estado_alterado.ContainsKey("muerto")){
                            target[i].estado_alterado.Remove("muerto");
                            target[i].atributos.vitalidad = efecto;
                        }
                        break;
                    case "revitalizar":
                    //CONTANTE AUMENTO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0
                        if (target[i].estado_alterado.ContainsKey("revitalizar")) target[i].estado_alterado.Remove("revitalizar");
                        target[i].estado_alterado["revitalizar"] = new float[]{efecto, duracion_efecto};
                        target[i].Curar(efecto);
                        if(target[i].estado_alterado["revitalizar"][1] <= 0) target[i].estado_alterado.Remove("revitalizar");
                        break;
                    //AUMENTO UNICO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                    case "aumentar_fuerza":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("fuerza", "aumentar_fuerza", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_vitalidad":
                         // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("vitalidad", "aumentar_vitalidad", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_magia":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("magia", "aumentar_magia", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_velocidad":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("velocidad", "aumentar_velocidad", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_critico":
                         // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("critico", "aumentar_critico", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_defensa_fisica":
                         // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("defensa_fisica", "aumentar_defensa_fisica", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_defensa_magica":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("defensa_magica", "aumentar_defensa_magica", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_fuerza_magia":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("fuerza", "aumentar_fuerza", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("magia", "aumentar_magia", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_defensas":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("defensa_fisica", "aumentar_defensa_fisica", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("defensa_magica", "aumentar_defensa_magica", efecto, duracion_efecto, false);
                        break;
                    case "aumentar_velocidad_critico":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("velocidad", "aumentar_velocidad", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("critico", "aumentar_critico", efecto, duracion_efecto, false);
                        break;
                    //PONEMOS UN ESTADO QUE NOS DARA CIERTOS BENEFICIOS EN OTRAS FUNCIONES DEL JUEGO
                    case "inmunidad_magica":
                        if (target[i].estado_alterado.ContainsKey("inmunidad_magica")) target[i].estado_alterado.Remove("inmunidad_magica");
                        target[i].estado_alterado["inmunidad_magica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["inmunidad_magica"][1] <= 0){
                            target[i].estado_alterado.Remove("inmunidad_magica");
                        }
                        break;
                    case "inmunidad_fisica":
                        if (target[i].estado_alterado.ContainsKey("inmunidad_fisica")) target[i].estado_alterado.Remove("inmunidad_fisica");
                        target[i].estado_alterado["inmunidad_fisica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["inmunidad_fisica"][1] <= 0){
                            target[i].estado_alterado.Remove("inmunidad_fisica");
                        }
                        break;
                    case "inmunidad":
                        if (target[i].estado_alterado.ContainsKey("inmunidad")) target[i].estado_alterado.Remove("inmunidad");
                        target[i].estado_alterado["inmunidad"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["inmunidad"][1] <= 0){
                            target[i].estado_alterado.Remove("inmunidad");
                        }
                        break;
                    case "robo_vida":
                        if (target[i].estado_alterado.ContainsKey("robo_vida")) target[i].estado_alterado.Remove("robo_vida");
                        target[i].estado_alterado["robo_vida"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["robo_vida"][1] <= 0){
                            target[i].estado_alterado.Remove("robo_vida");
                        }
                        break;
                    case "aumentar_estadisticas":
                     //AUMENTO UNICO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                        target[i].Alteraciones_atributos("velocidad", "aumentar_velocidad", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("critico", "aumentar_critico", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("fuerza", "aumentar_fuerza", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("magia", "aumentar_magia", efecto, duracion_efecto, false);
                        target[i].Alteraciones_atributos("vitalidad", "aumentar_vitalidad", efecto, duracion_efecto, false);
                        break;
                    case "ignorar_defensa_fisica":
                        if (target[i].estado_alterado.ContainsKey("ignorar_defensa_fisica")) target[i].estado_alterado.Remove("ignorar_defensa_fisica");
                        // efecto = porcentaje de ignorar
                        target[i].estado_alterado["ignorar_defensa_fisica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["ignorar_defensa_fisica"][1] <= 0){
                            target[i].estado_alterado.Remove("ignorar_defensa_fisica");
                        }
                        break;
                    case "ignorar_defensa_magica":
                        if (target[i].estado_alterado.ContainsKey("ignorar_defensa_magica")) target[i].estado_alterado.Remove("ignorar_defensa_magica");
                        // efecto = porcentaje de ignorar
                        target[i].estado_alterado["ignorar_defensa_magica"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["ignorar_defensa_magica"][1] <= 0){
                            target[i].estado_alterado.Remove("ignorar_defensa_magica");
                        }
                        break;
                    case "ignorar_defensas":
                        if (target[i].estado_alterado.ContainsKey("ignorar_defensas")) target[i].estado_alterado.Remove("ignorar_defensas");
                        // efecto = porcentaje de ignorar
                        target[i].estado_alterado["ignorar_defensas"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["ignorar_defensas"][1] <= 0){
                            target[i].estado_alterado.Remove("ignorar_defensas");
                        }
                        break;
                    default:
                        break;
                }
            }
            //MODIFICAMOS LA VIDA DE LOS OBJETIVOS AFECTADOS
            Cambiar_texto_vida(target[i], index_objetivo, true);
        }

        //SI ES PARA SOLO UN OJETIVO LO MODIFICAMOS EN EL ARREGLO DE PERSONAJES
        if(target.Length < 2) personajes[index_objetivo] = target[0];

    }



public void debuffear(Personajes[] target, int index_objetivo, Personajes[] enemigos, string[] habilidades = null, float daño_base = 0f, float atributo = 0f, float multiplicador_efecto = 0f, int duracion_efecto_int = 0){
    float efecto = 0F;
    float duracion_efecto = 0F;
    bool chequeando_debuffos = false;

    //SI NOS PASAN TODOS LOS PARAMETROS, REVISAMOS CUANTO SERA EL EFECTO DEL BUFF
    if (habilidades != null){
        efecto = (atributo * multiplicador_efecto) + daño_base;
        duracion_efecto = Convert.ToSingle(duracion_efecto_int);
        duracion_efecto -= 1;
    }
    //REVISAMOS TODOS LOS PERSONAJES QUE NOS LLEGUEN
    for(int i = 0; i < target.Length; i++){
        // SI NO NOS LLEGAN TODOS LOS PARAMETROS, SIGNIFICA QUE ESTAMOS CHEQUEANDO BUFFOS, TOMAMOS UNA LISTA DE BUFFOS EN EL TARGET
        if (habilidades == null){
            habilidades = new string[target[i].estado_alterado.Count];
            int k = 0;
            foreach (KeyValuePair<string, float[]> estado in target[i].estado_alterado){
                habilidades[k] = estado.Key;
                k++;
            }
            chequeando_debuffos = true;
        }
        for(int j = 0; j < habilidades.Length; j++){
            string habilidad = habilidades[j];
            // SI ESTAMOS CHEQUEANDO BUFFOS AGARRAMOS EL EFECTO Y LA DURACION DEL TARGET
            if (chequeando_debuffos == true){
                efecto = target[i].estado_alterado[habilidades[j]][0];
                duracion_efecto = (target[i].estado_alterado[habilidades[j]][1]) - 1;
            }
            switch (habilidad){
                case "heridas_graves":
                    if (target[i].estado_alterado.ContainsKey("heridas_graves")) target[i].estado_alterado.Remove("heridas_graves");
                    target[i].estado_alterado["heridas_graves"] = new float[]{efecto, duracion_efecto};
                    if(target[i].estado_alterado["heridas_graves"][1] <= 0){
                        target[i].estado_alterado.Remove("heridas_graves");
                    }
                    break;
                case "sangrado":
                    if (target[i].estado_alterado.ContainsKey("sangrado")) target[i].estado_alterado.Remove("sangrado");
                        target[i].estado_alterado["sangrado"] = new float[]{efecto, duracion_efecto};
                        target[i].Daño(efecto);
                        if(target[i].estado_alterado["sangrado"][1] <= 0) target[i].estado_alterado.Remove("revitalizar");
                        break;
                //REDUCCION UNICA DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                case "reducir_fuerza":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("fuerza", "reducir_fuerza", efecto, duracion_efecto, true);
                    break;
                case "reducir_vitalidad":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("vitalidad", "reducir_vitalidad", efecto, duracion_efecto, true);
                    break;
                case "reducir_magia":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("magia", "reducir_magia", efecto, duracion_efecto, true);
                    break;
                case "reducir_velocidad":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("velocidad", "reducir_velocidad", efecto, duracion_efecto, true);
                    break;
                case "reducir_critico":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("critico", "reducir_critico", efecto, duracion_efecto, true);
                    break;
                case "reducir_defensa_fisica":
                        // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("defensa_fisica", "reducir_defensa_fisica", efecto, duracion_efecto, true);
                    break;
                case "reducir_defensa_magica":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("defensa_magica", "reducir_defensa_magica", efecto, duracion_efecto, true);
                    break;
                case "reducir_fuerza_magia":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("fuerza", "reducir_fuerza", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("magia", "reducir_magia", efecto, duracion_efecto, true);
                    break;
                case "reducir_defensas":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("defensa_fisica", "reducir_defensa_fisica", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("defensa_magica", "reducir_defensa_magica", efecto, duracion_efecto, true);
                    break;
                case "reducir_velocidad_critico":
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("velocidad", "reducir_velocidad", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("critico", "reducir_critico", efecto, duracion_efecto, true);
                    break;
                case "reducir_estadisticas":
                    //AUMENTO UNICO DE UN ATRIBUTO MIENTRAS LA DURACION SEA > 0, LUEGO PONEMOS EL ATRIBUTO COMO ESTABA ANTES
                    // ATRIBUTO - HABILIDAD - EFECTO - DURACION - REDUCIR ATRIBUTO
                    target[i].Alteraciones_atributos("velocidad", "reducir_velocidad", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("critico", "reducir_critico", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("fuerza", "reducir_fuerza", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("magia", "reducir_magia", efecto, duracion_efecto, true);
                    target[i].Alteraciones_atributos("vitalidad", "reducir_vitalidad", efecto, duracion_efecto, true);
                    break;

                case "quemar":
                    if (target[i].estado_alterado.ContainsKey("quemar")) target[i].estado_alterado.Remove("quemar");
                        target[i].estado_alterado["quemar"] = new float[]{efecto, duracion_efecto};
                        target[i].Daño(efecto);
                        if(target[i].estado_alterado["quemar"][1] <= 0) target[i].estado_alterado.Remove("quemar");
                        break;
                case "quemar_grave":
                    if (target[i].estado_alterado.ContainsKey("quemar_grave")) target[i].estado_alterado.Remove("quemar_grave");
                        target[i].estado_alterado["quemar_grave"] = new float[]{efecto, duracion_efecto};
                        target[i].estado_alterado["heridas_graves"] = new float[]{efecto, duracion_efecto};
                        target[i].Daño(efecto);
                        if(target[i].estado_alterado["quemar_grave"][1] <= 0) target[i].estado_alterado.Remove("quemar_grave");
                        break;
                case "congelar":
                    if (target[i].estado_alterado.ContainsKey("congelar")){
                        target[i].estado_alterado.Remove("congelar");
                        target[i].estado_alterado["congelar"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["congelar"][1] <= 0){
                            target[i].estado_alterado.Remove("congelar");
                        }
                    }else{
                        target[i].Daño(efecto);
                        target[i].estado_alterado["congelar"] = new float[]{efecto, duracion_efecto};
                    }  
                    break;
                case "aturdir":
                    if (target[i].estado_alterado.ContainsKey("aturdir")){
                        target[i].estado_alterado.Remove("aturdir");
                        target[i].estado_alterado["aturdir"] = new float[]{efecto, duracion_efecto};
                        if(target[i].estado_alterado["aturdir"][1] <= 0){
                            target[i].estado_alterado.Remove("aturdir");
                        }
                    }else{
                        target[i].Daño(efecto);
                        target[i].estado_alterado["aturdir"] = new float[]{efecto, duracion_efecto};
                    }  
                    break;
                case "dormir":
                    if (target[i].estado_alterado.ContainsKey("dormir")){
                        // SI EL OBJETIVO RECIBE DAÑO EN ALGUN MOMENTO, LO DESPERTAMOS, REMOVIENDO EL EFECTO
                        if(efecto == target[i].atributos.salud){
                            target[i].estado_alterado.Remove("dormir");
                            target[i].estado_alterado["dormir"] = new float[]{target[i].atributos.salud, duracion_efecto};
                        }else{
                            target[i].estado_alterado.Remove("dormir");
                        }
                        if(target[i].estado_alterado["dormir"][1] <= 0){
                            target[i].estado_alterado.Remove("dormir");
                        }
                    }else{
                        target[i].estado_alterado["dormir"] = new float[]{target[i].atributos.salud, duracion_efecto};
                    }  
                    break;
                case "silenciar":
                    break;
                default:
                    break;
            }
        }
        //MODIFICAMOS LA VIDA DE LOS OBJETIVOS AFECTADOS
        Cambiar_texto_vida(target[i], index_objetivo, false);
    }

    //SI ES PARA SOLO UN OJETIVO LO MODIFICAMOS EN EL ARREGLO DE PERSONAJES
    if(target.Length < 2) enemigos[index_objetivo] = target[0];
}





    float Critico(float daño, float pro_critico){
        var rand = new System.Random();
        float comparador_critico = rand.Next(0, 101);
        if (comparador_critico < pro_critico){
            daño = daño * 1.5F;
            Debug.Log("GOLPE CRITICO");
            return daño;
        }
        return daño;
    }
    **/
        
}





