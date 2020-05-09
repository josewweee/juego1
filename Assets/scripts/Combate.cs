using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        //ASIGNAMOS LOS ATRIBUTOS INICIALES
        /**
        atributos_iniciales_personajes = new Atributos[4]{};
        estados_alterados_iniciales_personajes = new string[4]{};
        atributos_iniciales_enemigos = new Atributos[4]{};
        estados_alterados_iniciales_enemigos = new string[4]{};
        **/

        //LLENAMOS EL CAMPO CON LOS PERSONAJES COPIADOS
        popular_personajes_mapa(personajes, enemigos, prefab_personaje);

        //LLENAMOS EL PRIMER ORDEN DE TURNOS SEGUN LA VELOCIDAD DE LOS PERSONAJES
        for(int i = 0; i < personajes.Length; i++){
            turno.Add(personajes[i]);
        }
        for(int i = 0; i < enemigos.Length; i++){
            turno.Add(enemigos[i]);
        }
        turno.Sort((x, y) => x.atributos.velocidad.CompareTo(y.atributos.velocidad));
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
        
    }


    void Update()
    {   
        //SI ACABAMOS TURNO, EL SIGUIENTE SERA EL PRIMERO EN LA COLA
        if (turno_finalizado) personaje_en_turno = turno[0];
        Debug.Log("personaje en turno: " + personaje_en_turno.nombre);

        //SI YA ACABAMOS TURNO, ASIGNAMOS A LOS BOTONES LOS VALORES DEL NUEVO PERSONAJE EN TURNO
        if (cambiar_UI_poderes) Asignar_botones_turno(personaje_en_turno);
    }

    void Asignar_botones_turno(Personajes actual){
        //CAMBIAMOS LA OPCION DE CAMBIAR UI HASTA NO FINALIZAR EL TURNO
        cambiar_UI_poderes = false;

        //CAMBIAMOS TEXTOS EN LOS BOTONES
        GameObject texto_nivel;
        for(int i = 1; i < 5; i++){
            texto_nivel = GameObject.Find("texto_poder_"+i);
            texto_nivel.GetComponent<UnityEngine.UI.Text>().text = actual.poderesActivos[i-1].nombre;
        }

        //AGREGAMOS UN LISTENER A CADA BOTON
        btn_poder_1.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[0], actual); });
        btn_poder_2.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[1], actual); });
        btn_poder_3.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[2], actual); });
        btn_poder_4.onClick.AddListener(delegate { AsignarPoder(actual.poderesActivos[3], actual); });
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
        }
    }

    void AsignarPoder(Poderes poder, Personajes actual){
        switch(poder.tipo_poder){
            case "ataque":
                ataque();
                break;
            case "buff":
                buff();
                break;
            case "purgar":
                purgar();
                break;
            case "ataque_debuff":
                ataque();
                debuff();
                break;
            case "ataque_buff":
                ataque();
                buff();
                break;
            default:
                break;
        }
    }

    void ataque(){}
    void buff(){}
    void purgar(){}
    void debuff(){}
    void seleccionarUnico(){}
        
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

ataque(daño_base, atributo, multiplicador, tipo_elemento, reutilizacion, objetivos, sePuedeUsar, enemigos):
    if objetivo == unico:
        if (sePuedeUsar && reutilizacion == 0):
            Personajes[] target = new Personajes[1]{seleccionarUnico()}
            daño = (atributo * multiplicador) + daño_base;
            hacerDaño(target, daño, elemento, atributo)
                
    else:
        if (sePuedeUsar && reutilizacion == 0):
            daño = atributo * multiplicador
            hacerDaño(enemigos, daño, elemento, atributo)

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



hacerDaño(personajes[] target, daño, elementoAtacante, atributo):
    elementoDefensor = target.elemento
    for (i < target.Length){
        if inmunidad.magico
         --> x6
            if elementoAtacante = agua:
                if elementoDefensor = agua:
                    daño = Critico(daño, actual.atributos.critico)
                    daño += daño * 1
                    if (atributo == "magia"): daño -= (daño * (actual.atributo.defensa_magica / 100))
                    else(): daño -= (daño * (actual.atributo.defensa_fisica / 100))
                    
                if elementoDefensor = fuego:
                    daño = Critico(daño, actual.atributos.critico)
                    daño += daño * 1.5
                    if (atributo == "magia"): daño -= (daño * (actual.atributo.defensa_magica / 100))
                    else(): daño -= (daño * (actual.atributo.defensa_fisica / 100))
                if elementoDefensor = tierra:
                    daño = Critico(daño, actual.atributos.critico)
                    daño += daño * 1
                    if (atributo == "magia"): daño -= (daño * (actual.atributo.defensa_magica / 100))
                    else(): daño -= (daño * (actual.atributo.defensa_fisica / 100))
                if elementoDefensor = trueno:
                    daño = Critico(daño, actual.atributos.critico)
                    daño += daño * 0.5
                    if (atributo == "magia"): daño -= (daño * (actual.atributo.defensa_magica / 100))
                    else(): daño -= (daño * (actual.atributo.defensa_fisica / 100))
                if elementoDefensor = oscuridad:
                    daño = Critico(daño, actual.atributos.critico)
                    daño += daño * 1
                    if (atributo == "magia"): daño -= (daño * (actual.atributo.defensa_magica / 100))
                    else(): daño -= (daño * (actual.atributo.defensa_fisica / 100))
                if elementoDefensor = luz:
                    daño = Critico(daño, actual.atributos.critico)
                    daño += daño * 1
                    if (atributo == "magia"): daño -= (daño * (actual.atributo.defensa_magica / 100))
                    else(): daño -= (daño * (actual.atributo.defensa_fisica / 100))


            target.vitalidad -= daño
            if (target.vitalidad <= 0):
                destroy(target)
        
    }
**/