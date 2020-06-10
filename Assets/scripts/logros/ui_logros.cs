using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_logros : MonoBehaviour
{
    //PANELES DE LA TIENDA
    public GameObject prefab_logro;
    private GameObject panel_diarias;
    private GameObject panel_eventos;
    private GameObject panel_mensuales;
    private GameObject panel_generales;
    private GameObject panel_pvp;
    private GameObject panel_historia;

    //ESTRUCTURA DE LA RECOMPENZA DEL LOGRO
    public struct Recompenza_logro{
        public int codigo;
        public int puntos;
        public int cantidad;
    }

    //POSICIONES DE LAS INSTANCIAS QUE SE CREARAN DE LOS ITEMS DE LA TIENDA
    float pos_inicial_x;
    float pos_inicial_y;

    //VARIABLES DEL USUARIO
    private Usuario jugador;
    private Usuario singleton;
    private crud CRUD;
    //VARIABLE PARA CAMBIAR DE PANTALLAS
    private routing menuCtrl;


    private IEnumerator Start()
    {
        //TRAEMOS EL USUARIO DE LA BASE DE DATOS Y DE SU SINGLETON
        singleton = Usuario.instancia;
        CRUD = GameObject.Find("Crud").GetComponent<crud>();
        var jugador_task = CRUD.GetComponent<crud>().Cargar_usuario();
        yield return new WaitUntil( ()=> jugador_task.IsCompleted);
        jugador = jugador_task.Result;

        //TRAEMOS EL MENU
        menuCtrl = GameObject.Find("menu").gameObject.GetComponent<routing>();

        //POSICIONES DE LAS INSTANCIAS QUE SE CREARAN DE LOS ITEMS DE LA TIENDA
        pos_inicial_x = 10F;
        pos_inicial_y = -192.689F;

        //BUSCAMOS LOS PANELES DE LOGROS
        panel_diarias = GameObject.Find("panel_diarias");
        panel_eventos = GameObject.Find("panel_eventos");
        panel_mensuales = GameObject.Find("panel_mensuales");
        panel_generales = GameObject.Find("panel_generales");
        panel_pvp = GameObject.Find("panel_pvp");
        panel_historia = GameObject.Find("panel_historia");

        //DESACTIVAMOS LOS PANELES MENOS EL DE DIARIAS
        Importar_datos_logros("diarias");
        panel_eventos.SetActive(false);
        panel_mensuales.SetActive(false);
        panel_generales.SetActive(false);
        panel_pvp.SetActive(false);
        panel_historia.SetActive(false);
    }

    public void Abrir_panel(string panel)
    {
        Cerrar_panel_activo();
        switch(panel)
        {
            case "diarias":
                panel_diarias.SetActive(true);
                Importar_datos_logros("diarias");
                break;
            case "eventos":
                panel_eventos.SetActive(true);
                Importar_datos_logros("eventos");
                break;
            case "mensuales":
                panel_mensuales.SetActive(true);
                Importar_datos_logros("mensuales");
                break;
            case "generales":
                panel_generales.SetActive(true);
                Importar_datos_logros("generales");
                break;
            case "pvp":
                panel_pvp.SetActive(true);
                Importar_datos_logros("pvp");
                break;
            case "historia":
                panel_historia.SetActive(true);
                Importar_datos_logros("historia");
                break;
            default:
                break;
        }
    }

    void Cerrar_panel_activo()
    {
        if(panel_diarias.activeSelf) panel_diarias.SetActive(false);destruirItemsInternos(panel_diarias);
        if(panel_eventos.activeSelf) panel_eventos.SetActive(false);destruirItemsInternos(panel_eventos);
        if(panel_mensuales.activeSelf) panel_mensuales.SetActive(false);destruirItemsInternos(panel_mensuales);
        if(panel_generales.activeSelf) panel_generales.SetActive(false);destruirItemsInternos(panel_generales);
        if(panel_pvp.activeSelf) panel_pvp.SetActive(false);destruirItemsInternos(panel_pvp);
        if(panel_historia.activeSelf) panel_historia.SetActive(false);destruirItemsInternos(panel_historia);
    }

    void destruirItemsInternos(GameObject panel)
    {
        foreach (Transform child in panel.transform.GetChild(0).GetChild(0)) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void Importar_datos_logros(string panel)
    {
        pos_inicial_y = 23F;
        //titulo, codigoLogro, [img recom]
        //[cantidad recomp], [codigo recom], objetivo
        switch(panel)
        {
            case "diarias":
                if ( !jugador.logros.ContainsKey(0) || (jugador.logros.ContainsKey(0) && jugador.logros[0].reclamado == false)){
                    this.crearItemLogro("Juega 3 combates Pvp", 0, new string[]{"Oro"}, new int[] {100}, new int[] {0}, 3);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(1) || (jugador.logros.ContainsKey(1) && jugador.logros[1].reclamado == false)){
                    this.crearItemLogro("Realiza 1 invocacion", 1, new string[] {"Oro"}, new int[] {100}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(2) || (jugador.logros.ContainsKey(2) && jugador.logros[2].reclamado == false)){
                    this.crearItemLogro("Avanza 1 nivel en modo historia", 2, new string[] {"Diamantes"}, new int[] {20}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(3) || (jugador.logros.ContainsKey(3) && jugador.logros[3].reclamado == false)){
                    this.crearItemLogro("Envia regalos a tus amigos", 3, new string[] {"Oro"}, new int[] {100}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(4) || (jugador.logros.ContainsKey(4) && jugador.logros[4].reclamado == false)){
                    this.crearItemLogro("Compra 1 item en la tienda", 4, new string[] {"Diamantes"}, new int[] {30}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(5) || (jugador.logros.ContainsKey(5) && jugador.logros[5].reclamado == false)){
                    this.crearItemLogro("Juega 1 combate contra 1 amigo", 5, new string[] {"Oro"}, new int[] {100}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                break;
            case "eventos":
                break;
            case "mensuales":
                break;
            case "generales":
                if ( !jugador.logros.ContainsKey(24) || (jugador.logros.ContainsKey(24) && jugador.logros[24].reclamado == false)){
                    this.crearItemLogro("Congela al equipo enemigo de 1 solo poder", 24, new string[]{"Diamantes"}, new int[] {100}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(25) || (jugador.logros.ContainsKey(25) && jugador.logros[25].reclamado == false)){
                    this.crearItemLogro("Gana 1 juego con el equipo enemigo estando quemado", 25, new string[] {"Diamantes"}, new int[] {50}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(26) || (jugador.logros.ContainsKey(26) && jugador.logros[26].reclamado == false)){
                    this.crearItemLogro("Gana 1 juego con el equipo enemigo estando congelado", 26, new string[] {"Diamantes"}, new int[] {50}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(27) || (jugador.logros.ContainsKey(27) && jugador.logros[27].reclamado == false)){
                    this.crearItemLogro("Aturde al equipo enemigo de 1 solo poder", 27, new string[] {"Diamantes"}, new int[] {100}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(28) || (jugador.logros.ContainsKey(28) && jugador.logros[28].reclamado == false)){
                    this.crearItemLogro("Gana 1 juego con un unico superviviente", 28, new string[] {"Oro"}, new int[] {500}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(29) || (jugador.logros.ContainsKey(29) && jugador.logros[29].reclamado == false)){
                    this.crearItemLogro("Elimina a 1 enemigo de 1 solo golpe", 29, new string[] {"Diamantes"}, new int[] {100}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ( !jugador.logros.ContainsKey(30) || (jugador.logros.ContainsKey(30) && jugador.logros[30].reclamado == false)){
                    this.crearItemLogro("Elimina al equipo enemigo de 1 solo golpe", 30, new string[] {"Diamantes"}, new int[] {200}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                break;
            case "pvp":
                break;
            case "historia":
                if ( !jugador.logros.ContainsKey(6) || (jugador.logros.ContainsKey(6) && jugador.logros[6].reclamado == false)){
                    this.crearItemLogro("Completa el nivel 10", 6, new string[]{"Oro"}, new int[] {1000}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(6) && jugador.logros[6].reclamado) && (!jugador.logros.ContainsKey(7) || (jugador.logros.ContainsKey(7) && jugador.logros[7].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 20", 7, new string[] {"Diamantes"}, new int[] {500}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(7) && jugador.logros[7].reclamado) && (!jugador.logros.ContainsKey(8) || (jugador.logros.ContainsKey(8) && jugador.logros[8].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 30", 8, new string[] {"Oro"}, new int[] {4000}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(8) && jugador.logros[8].reclamado) && (!jugador.logros.ContainsKey(9) || (jugador.logros.ContainsKey(9) && jugador.logros[9].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 40", 9, new string[] {"Oro", "Diamantes"}, new int[] {6000, 500}, new int[] {0,1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(9) && jugador.logros[9].reclamado) && (!jugador.logros.ContainsKey(10) || (jugador.logros.ContainsKey(10) && jugador.logros[10].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 50", 10, new string[] {"Diamantes"}, new int[] {1000}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(10) && jugador.logros[10].reclamado) && (!jugador.logros.ContainsKey(11) || (jugador.logros.ContainsKey(11) && jugador.logros[11].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 60", 11, new string[] {"Oro", "Diamantes"}, new int[] {10000, 500}, new int[] {0,1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((!jugador.logros.ContainsKey(18) || (jugador.logros.ContainsKey(18) && jugador.logros[18].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 10 sin que nadie muera", 18, new string[]{"Oro"}, new int[] {1000}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(18) && jugador.logros[18].reclamado) && (!jugador.logros.ContainsKey(19) || (jugador.logros.ContainsKey(19) && jugador.logros[19].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 20 sin que nadie muera", 19, new string[] {"Diamantes"}, new int[] {500}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(19) && jugador.logros[19].reclamado) && (!jugador.logros.ContainsKey(20) || (jugador.logros.ContainsKey(20) && jugador.logros[20].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 30 sin que nadie muera", 20, new string[] {"Oro"}, new int[] {4000}, new int[] {0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(20) && jugador.logros[20].reclamado) && (!jugador.logros.ContainsKey(21) || (jugador.logros.ContainsKey(21) && jugador.logros[21].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 40 sin que nadie muera", 21, new string[] {"Oro", "Diamantes"}, new int[] {6000, 500}, new int[] {0,1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(21) && jugador.logros[21].reclamado) && (!jugador.logros.ContainsKey(22) || (jugador.logros.ContainsKey(22) && jugador.logros[22].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 50 sin que nadie muera", 22, new string[] {"Diamantes"}, new int[] {1000}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(22) && jugador.logros[22].reclamado) && (!jugador.logros.ContainsKey(23) || (jugador.logros.ContainsKey(23) && jugador.logros[23].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 60 sin que nadie muera", 23, new string[] {"Oro", "Diamantes"}, new int[] {10000, 500}, new int[] {0,1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((!jugador.logros.ContainsKey(12) || (jugador.logros.ContainsKey(12) && jugador.logros[12].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 10, con solo 1 personaje", 12, new string[]{"Diamantes"}, new int[] {500}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(12) && jugador.logros[12].reclamado) && (!jugador.logros.ContainsKey(13) || (jugador.logros.ContainsKey(13) && jugador.logros[13].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 20, con solo 1 personaje", 13, new string[]{"Diamantes"}, new int[] {800}, new int[] {1}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(13) && jugador.logros[13].reclamado) && (!jugador.logros.ContainsKey(14) || (jugador.logros.ContainsKey(14) && jugador.logros[14].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 30, con solo 1 personaje", 14, new string[]{"Diamantes", "Oro"}, new int[] {1000, 3000}, new int[] {1,0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(14) && jugador.logros[14].reclamado) && (!jugador.logros.ContainsKey(15) || (jugador.logros.ContainsKey(15) && jugador.logros[15].reclamado == false))){
                    this.crearItemLogro("Completa el nivel 40, con solo 1 personaje", 15, new string[]{"Diamantes", "Oro"}, new int[] {1500, 5000}, new int[] {1,0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(15) && jugador.logros[15].reclamado) && (!jugador.logros.ContainsKey(16) || (jugador.logros.ContainsKey(16) && jugador.logros[16].reclamado == false))){          
                    this.crearItemLogro("Completa el nivel 50, con solo 1 personaje", 16, new string[]{"Diamantes", "Oro"}, new int[] {1800, 5000}, new int[] {1,0}, 1);
                    pos_inicial_y -= 56.6908F;
                }
                if ((jugador.logros.ContainsKey(16) && jugador.logros[16].reclamado) && (!jugador.logros.ContainsKey(17) || (jugador.logros.ContainsKey(17) && jugador.logros[17].reclamado == false))){          
                    this.crearItemLogro("Completa el nivel 60, con solo 1 personaje", 17, new string[]{"Diamantes", "Oro"}, new int[] {2000, 5000}, new int[] {1,0}, 1);
                    pos_inicial_y -= 56.6908F;                      
                }
                break;                                                            
        }
    }

    private void crearItemLogro(string titulo, int codigoLogro, string[] imgRecompenza, int[] cantidadRecompenza, int[] codigoRecompenza, int objetivo)
    {
        //INSTANCIAMOS CADA ITEM
        GameObject item = Instantiate(this.prefab_logro, new Vector3(this.pos_inicial_x, this.pos_inicial_y, 0), Quaternion.identity);
        item.transform.SetParent(GameObject.Find("contenido_scroll").transform, false);

        //titulo
        item.transform.GetChild(3).GetComponent<Text>().text = titulo;
        //estado actual del logro del usuario
        //Logros logroActual = this.jugador.logros.Find(x => x.codigo_logro == codigoLogro);
        int actual = (jugador.logros.ContainsKey(codigoLogro)) ? jugador.logros[codigoLogro].progreso_actual : 0;
        item.transform.GetChild(2).GetComponent<Text>().text = actual.ToString();
        //objetivo del logro
        item.transform.GetChild(1).GetComponent<Text>().text = "/ " + objetivo.ToString();
        //imagen y cantidad de las recompenzas
        if (codigoRecompenza.Length >= 2){
            item.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = Resources.Load <Sprite>("botones/" + imgRecompenza[0]);
            item.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "x"+cantidadRecompenza[0].ToString();
            item.transform.GetChild(5).GetChild(0).GetComponent<Image>().sprite = Resources.Load <Sprite>("botones/" + imgRecompenza[1]);
            item.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = "x"+cantidadRecompenza[1].ToString();
        }
        else
        {
            item.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = Resources.Load <Sprite>("botones/" + imgRecompenza[0]);
            item.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = "x"+cantidadRecompenza[0].ToString();
            GameObject recomp2 = item.transform.GetChild(5).gameObject;
            recomp2.SetActive(false);
        }

        
        //estructura del item
        List<Recompenza_logro> itemsCreado = new List<Recompenza_logro>();
        int i = 0;
        foreach(int codigo in codigoRecompenza)
        {
           Recompenza_logro itemNuevo = new Recompenza_logro();
           itemNuevo.codigo = codigo;
           itemNuevo.puntos = 1;
           itemNuevo.cantidad = cantidadRecompenza[i];
           itemsCreado.Add(itemNuevo);
           i++;
        }
        //desactivamos el boton de reclamar si aun no esta completo, si no, le enviamos el codigo de la recompenza
        //y la cantidad a reclamar
        GameObject objeto_boton = item.transform.GetChild(0).gameObject;
        Button btn = objeto_boton.GetComponent<Button>();
        if (actual < objetivo) btn.interactable = false;
        btn.onClick.AddListener(delegate { Completar(itemsCreado, codigoLogro); });
    }

    void Completar(List<Recompenza_logro> recompenzasLogro, int codigoLogro)
    {
        int contadorRecompenzasReclamadas = 0; // PARA VER CUANTAS RECOMPENZAS PUDIMOS RECLAMAR
        foreach(Recompenza_logro logro in recompenzasLogro)
        {
            switch(logro.codigo)
            {
                case 0: // oro
                    jugador.Sumar_monedas("oro",logro.cantidad);
                    contadorRecompenzasReclamadas ++;
                    break;
                case 1: //diamantes
                    jugador.Sumar_monedas("diamantes",logro.cantidad);
                    contadorRecompenzasReclamadas ++;
                    break;
                case 2: //invocaciones normales
                    jugador.Sumar_monedas("invocaciones_normales",logro.cantidad);
                    contadorRecompenzasReclamadas ++;
                    break;
                case 3: //invocaciones raras
                    jugador.Sumar_monedas("invocaciones_raras",logro.cantidad);
                    contadorRecompenzasReclamadas ++;
                    break;
                case 4: //invocaciones miticas
                    jugador.Sumar_monedas("invocaciones_miticas",logro.cantidad);
                    contadorRecompenzasReclamadas ++;
                    break;
                case 5: //invocaciones legendarias
                    jugador.Sumar_monedas("invocaciones_legendarias",logro.cantidad);
                    contadorRecompenzasReclamadas ++;
                    break;
                case 6: //energia
                    jugador.energia += logro.cantidad;
                    contadorRecompenzasReclamadas ++;
                    break;
                case 7: //puntos pvp
                    jugador.energia_pvp += logro.cantidad;
                    contadorRecompenzasReclamadas ++;
                    break;
                case 8: //fragmentos roger
                    if (jugador.personajes.Find(x => x.nombre == "roger") != null){
                        jugador.personajes.Find(x => x.nombre == "roger").fragmentos += logro.cantidad;
                        contadorRecompenzasReclamadas ++;
                    }
                    break;
                case 9: //fragmentos liliana
                    if (jugador.personajes.Find(x => x.nombre == "liliana") != null){
                        jugador.personajes.Find(x => x.nombre == "liliana").fragmentos += logro.cantidad;
                        contadorRecompenzasReclamadas ++;
                    }
                    break;
                case 10: //fragmentos martis
                    if (jugador.personajes.Find(x => x.nombre == "martis") != null){
                        jugador.personajes.Find(x => x.nombre == "martis").fragmentos += logro.cantidad;
                        contadorRecompenzasReclamadas ++;
                    }
                    break;
                case 11: //fragmentos alicia
                    if (jugador.personajes.Find(x => x.nombre == "alicia") != null){
                        jugador.personajes.Find(x => x.nombre == "alicia").fragmentos += logro.cantidad;
                        contadorRecompenzasReclamadas ++;
                    }
                    break;
            }
        }

        //SOLO GUARDAMOS SI PUDIMOS RECLAMAR TODAS LAS RECOMPENZAS
        if(contadorRecompenzasReclamadas == recompenzasLogro.Count)
        {
            jugador.LogroCompletado(codigoLogro);
            Guardar_cambios(jugador);
            menuCtrl.ir_logros();
        }
        else
        {
            Debug.Log("No puedes reclamar este logro aun, no tendrias las recompenzas completas");
        }
       

    }

    private void Guardar_cambios(Usuario nuevo_val)
    {
        this.CRUD.Guardar_usuario(nuevo_val);
        this.singleton.Actualizar_usuario(nuevo_val);
    }
}
