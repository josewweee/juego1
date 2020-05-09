using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Combate : MonoBehaviour
{
    //PERSONAJES EN EL COMBATE
    public Personajes[] personajes;
    public Personajes[] enemigos;

    //ESTADOS INICIALES DE LOS ATRIBUTOS Y ESTADOS ALTERADOS DE LOS PERSONAJES Y ENEMIGOS
    private Atributos[] atributos_iniciales_personajes;
    private string[] estados_alterados_iniciales_personajes;
    private Atributos[] atributos_iniciales_enemigos;
    private string[] estados_alterados_iniciales_enemigos;

    //¿HISTORIA, PVP?
    public string tipo_combate;
    public Recompenza[] recompenza;
    public Usuario ganador;
    public Usuario perdedor;

    //TABLA DEFENSAS ELEMENTALES;
    private Dictionary<string, float[]> defensas_elementales = new Dictionary<string, float[]>(){
        {"agua", new float[] {1F, 1.5F, 1F, 0.5F, 1F, 1F} },
        {"fuego", new float[] {0.5F, 1F, 1.5F, 1F, 1F, 1F} },
        {"tierra", new float[] {1F, 0.5F, 1F, 1.5F, 1F, 1F} },
        {"trueno", new float[] {1.5F, 1F, 0.5F, 1F, 1F, 1F} },
        {"oscuridad", new float[] {1F, 1F, 1F, 1F, 0.5F, 1.5F} },
        {"luz", new float[] {1F, 1F, 1F, 1F, 1.5F, 0.5F} },
    };

    //PREFAB QUE INSTANCIAREMOS COMO PERSONAJE
    public GameObject prefab_personaje;

    //TURNOS DE PERSONAJES
    public List<Personajes> turno = new List<Personajes>();
    private Personajes personaje_en_turno;
    public bool turno_finalizado = false;

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

    public Poderes poder_a_ser_lanzado;

    void Start()
    {
        //TRAEMOS LAS INSTANCIAS DEL JUGADOR Y LOS ENEMIGOS
        jugador = Usuario.instancia;
        storage_enemigos = storage_script.instancia;
        
        //de prueba
        fabrica = new Personajes();
        personajes = new Personajes[4]{fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("roger")};
        enemigos = new Personajes[4]{fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("roger"), fabrica.Crear_personaje("alicia"), fabrica.Crear_personaje("roger")};

        //COPIAMOS LOS PERSONAJES DEL USUARIO Y DE LOS ENEMIGOS LOCALMENTE
        //personajes = jugador.personajesFavoritos;
        //enemigos = storage_enemigos.enemigos;

        //ASIGNAMOS LOS ATRIBUTOS INICIALES
        /**
        atributos_iniciales_personajes = new Atributos[4]{};
        estados_alterados_iniciales_personajes = new string[4]{};
        atributos_iniciales_enemigos = new Atributos[4]{};
        estados_alterados_iniciales_enemigos = new string[4]{};
        **/

        //ASIGNAMOS EL UI DE LOS TEXTOS DE OBJETIVOS Y LOS DESHABILITAMOS POR AHORA
        objetivo_unico_txt = GameObject.Find("texto_seleccion_objetivo");
        multiple_objetivo_txt = GameObject.Find("texto_seleccion_objetivos");
        objetivo_unico_txt.SetActive(false);
        multiple_objetivo_txt.SetActive(false);

        //LLENAMOS EL CAMPO CON LOS PERSONAJES COPIADOS
        popular_personajes_mapa(personajes, enemigos, prefab_personaje);

        //LLENAMOS EL PRIMER ORDEN DE TURNOS SEGUN LA VELOCIDAD DE LOS PERSONAJES
        for(int i = 0; i < personajes.Length; i++){
            turno.Add(personajes[i]);
        }
        for(int i = 0; i < enemigos.Length; i++){
            turno.Add(enemigos[i]);
        }
        turno.Sort((x, y) => y.atributos.velocidad.CompareTo(x.atributos.velocidad));
        personaje_en_turno = turno[0];

        //IMPRIMIMOS LOS TURNOS
        for(int i = 0; i < turno.Count; i++){
            Debug.Log(turno[i]);
        }

        //NICIALIZAMOS LOS BOTONES
        poder_1 = GameObject.Find("poder_1");
        poder_2 = GameObject.Find("poder_2");
        poder_3 = GameObject.Find("poder_3");
        poder_4 = GameObject.Find("poder_4");

        btn_poder_1 = poder_1.GetComponent<Button>();
        btn_poder_2 = poder_2.GetComponent<Button>();
        btn_poder_3 = poder_3.GetComponent<Button>();
        btn_poder_4 = poder_4.GetComponent<Button>();

        // ASIGNAMOS LOS BOTONES AL PRIMER PERSONAJE
        Asignar_botones_turno(personaje_en_turno);
         Debug.Log("personaje en turno: " + personaje_en_turno.nombre);
        
    }


    void Update()
    {   
        //SI ACABAMOS TURNO, EL SIGUIENTE SERA EL PRIMERO EN LA COLA y ASIGNAMOS A LOS BOTONES LOS VALORES DEL NUEVO PERSONAJE EN TURNO
        if (turno_finalizado){
            //turno.Sort((x, y) => x.atributos.velocidad.CompareTo(y.atributos.velocidad)); 
            personaje_en_turno = turno[0];
            Asignar_botones_turno(personaje_en_turno);
            turno_finalizado = false;
            Debug.Log("personaje en turno: " + personaje_en_turno.nombre);
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
        float[] pos_inicial_x_enemigos = {7.39F, 5.15F, 4.63F, 6.76F};
        float[] pos_inicial_y_enemigos = {-2.61F, -3.48F, 0.89F, 0.12F};
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

    void ataque(Poderes poder, string index_objetivo){
        int index = int.Parse(index_objetivo);
        Personajes[] target;
        float atributo_;

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
            HacerDaño(target, daño, personaje_en_turno.elemento, poder.atributo, index);
        }else{
            target = enemigos;
            float daño = (atributo_ * poder.multiplicador) + poder.daño_base;
            // 99 PARA INDICAR QUE AFECTAREMOS A TODOS
            HacerDaño(target, daño, personaje_en_turno.elemento, poder.atributo, 99);
        }

    }


    void buff(){}
    void purgar(){}
    void debuff(){}

    public void Confirmar_poder(string index){
        if(poder_a_ser_lanzado == null) return;
        Poderes poder = poder_a_ser_lanzado;
        bool bando_objetivo; // TRUE = AMIGOS, FALSE = ENEMIGOS

        //MIRAMOS QUE PODER FUE LANZADO Y LLAMAMOS LA FUNCION CORRESPONDIENTE
        //MIRAMOS SI ES POSIBLE TIRAR SEGUN EL TIEMPO DE COOLDOWN
        if (poder.se_puede_usar){
            switch(poder.tipo_poder){ 
                case "ataque":
                    ataque(poder, index);
                    break;
                case "buff":
                    buff();
                    break;
                case "purgar":
                    purgar();
                    break;
                case "ataque_debuff":
                    ataque(poder, index);
                    debuff();
                    break;
                case "ataque_buff":
                    ataque(poder, index);
                    buff();
                    break;
                default:
                    break;
            }

            //PASAMOS TURNO
            turno_finalizado = true;
            turno.RemoveAt(0);
            turno.Add(personaje_en_turno);
            Limpiar_botones_turno();

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


    void HacerDaño(Personajes[] target, float daño, string elementoAtacante, string atributo, int index_objetivo){
        for (int i= 0; i < target.Length; i++){
            string elementoDefensor = target[i].elemento;
            if (elementoDefensor == "agua"){
                daño = Critico(daño, personaje_en_turno.atributos.critico);
                daño += daño * defensas_elementales[elementoAtacante][0];
                if (atributo == "magia") daño -= (daño * (target[i].atributos.defensa_magica / 100));
                else daño -= (daño * (target[i].atributos.defensa_fisica / 100));        
            }else if (elementoDefensor == "fuego"){
                daño = Critico(daño, personaje_en_turno.atributos.critico);
                daño += daño * defensas_elementales[elementoAtacante][1];
                if (atributo == "magia") daño -= (daño * (target[i].atributos.defensa_magica / 100));
                else daño -= (daño * (target[i].atributos.defensa_fisica / 100));
            }else if (elementoDefensor == "tierra"){
                daño = Critico(daño, personaje_en_turno.atributos.critico);
                daño += daño * defensas_elementales[elementoAtacante][2];
                if (atributo == "magia") daño -= (daño * (target[i].atributos.defensa_magica / 100));
                else daño -= (daño * (target[i].atributos.defensa_fisica / 100));
            }else if (elementoDefensor == "trueno"){
                daño = Critico(daño, personaje_en_turno.atributos.critico);
                daño += daño * defensas_elementales[elementoAtacante][3];
                if (atributo == "magia") daño -= (daño * (target[i].atributos.defensa_magica / 100));
                else daño -= (daño * (target[i].atributos.defensa_fisica / 100));
            }else if (elementoDefensor == "oscuridad"){
                daño = Critico(daño, personaje_en_turno.atributos.critico);
                daño += daño * defensas_elementales[elementoAtacante][4];
                if (atributo == "magia") daño -= (daño * (target[i].atributos.defensa_magica / 100));
                else daño -= (daño * (target[i].atributos.defensa_fisica / 100));
            }else if (elementoDefensor == "luz"){
                daño = Critico(daño, personaje_en_turno.atributos.critico);
                daño += daño * defensas_elementales[elementoAtacante][6];
                if (atributo == "magia") daño -= (daño * (target[i].atributos.defensa_magica / 100));
                else daño -= (daño * (target[i].atributos.defensa_fisica / 100));
            }else{
                Debug.Log("elemento defensor erroneo");
            }


            target[i].atributos.vitalidad -= daño;
            if (target[i].atributos.vitalidad <= 0){
                target[i].estado_alterado["muerto"] = new float[]{0F, 9999F};
            }
            Cambiar_texto_vida(target[i], index_objetivo);
        }
        
    }
    
    float Critico(float daño, float pro_critico){
        return daño;
    }

    void Cambiar_texto_vida(Personajes target, int index_objetivo){

        //MOSIFICAMOS LA VIDA DE TODOS LOS PERSONAJES SI NOS LLEGA 99 -> TODOS LOS OBJETIVOS
        if (index_objetivo == 99){
            for(int i = 0; i < enemigos.Length; i++){
                GameObject canvas_texto = GameObject.Find("vida_enemigo_"+i);
                canvas_texto.GetComponent<Text>().text = enemigos[i].atributos.vitalidad.ToString();
            }
        }else{
            //MODIFICAMOS LA VIDA DE 1 PERSONAJE ARRIBA EN PANTALLA
            GameObject canvas_texto = GameObject.Find("vida_enemigo_"+index_objetivo);
            canvas_texto.GetComponent<Text>().text = target.atributos.vitalidad.ToString();
        }
    }



        
}

/**
actual = personaje_en_turno;

if actual. ataque:
    ataque()
if buff:
    buff()
if debuff:
    debuff()
if purgar:
    purgar()
if ataque_debuff:
    ataque()
    debuff()
if ataque_buff:
    ataque()
    buff()



buff(daño_base, atributo, multiplicador_efecto, tipo_elemento, reutilizacion, duracion_efecto, objetivos, sePuedeUsar, aliados )
    if objetivo == unico:
        if (sePuedeUsar && reutilizacion == 0):
            Personajes[] target = new Personajes[1]{seleccionarUnico()}
            buffear(target, daño_base, aributo, multiplicador_efecto, duracion_efecto)
    else objetivo == multiple:
        if (sePuedeUsar && reutilizacion == 0):
            buffear(aliados,  daño_base, aributo, multiplicador_efecto, duracion_efecto)
    else:
        if (sePuedeUsar && reutilizacion == 0):
            Personajes[] target = new Personajes[1]{actual}
            buffear(target,  daño_base, aributo, multiplicador_efecto, duracion_efecto)



buffear(Personajes[] target, daño_base,aributo, multiplicador_efecto, duracion_efecto):
    for(i < target.Length):
        "curacion":
            efecto = (atributo * multiplicador_efecto) + daño_base;
            target[i].vitalidad += efecto;
        "revivir"
            efecto = 0.5F
            if(target[i].estado_alterado.has("muerto")):
                target[i].estado_alterado["muerto"] = new float[]{efecto, duracion_efecto};
        "revitalizar"
            efecto = (atributo * multiplicador_efecto) + daño_base;
            target[i].vitalidad += efecto;
            target[i].estado_alterado["revitalizar"] = new float[]{efecto, duracion_efecto};
        "aumentar_fuerza"
        "aumentar_vitalidad"
        "aumentar_magia"
        "aumentar_velocidad"
        "aumentar_critico"
        "aumentar_defensa_fisica"
        "aumentar_defensa_magica"
        "aumentar_fuerza_magia"
        "aumentar_defensas"
        "aumentar_velocidad_critico"
        "inmunidad_magica"
        "inmunidad_fisica"
        "escudo_fisico"
        "escudo_magico"
        "inmunidad"
        "escudo"
        "robo_vida"
        "aumentar_estadisticas"
        "ignorar_defensa_fisica"
        "ignorar_defensa_magica"
        "ignorar_defensas"


debuff():
    "heridas_graves":
    "sangrado"
    "reducir_fuerza"
    "reducir_vitalidad"
    "reducir_magia"
    "reducir_velocidad"
    "reducir_critico"
    "reducir_defensa_fisica"
    "reducir_defensa_magica"
    "reducir_fuerza_magia"
    "reducir_defensas"
    "reducir_velocidad_critico"
    "reducir_estadisticas"
    "quemar"
    "congelar"
    "aturdir"
    "dormir"
    "silenciar"
    **/