using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{
    //PANELES DE LA TIENDA
    public GameObject prefab_item;
    private GameObject panel_promociones;
    private GameObject panel_personajes;
    private GameObject panel_energia;
    private GameObject panel_despertar;
    private GameObject panel_diamantes;
    private GameObject panel_armas;
    private GameObject panel_oro;

    //POSICIONES DE LAS INSTANCIAS QUE SE CREARAN DE LOS ITEMS DE LA TIENDA
    float pos_inicial_x;
    float pos_inicial_y;

    //VARIABLES DEL USUARIO
    private Usuario jugador;
    private Usuario singleton;
    private crud CRUD;

    //ITEM CREADO
    public struct itemTienda{
        public string titulo;
        public int codigoItem;
        public string img;
        public string precioAntes;
        public string precio;
        public int codigoPrecio; // 0 gratis, 1 oro, 2 diamantes, 3 dinero
        public string descripcion;
    }

    //MENU PARA NAVEGAR
    routing menuCtrl;

    //TRAEMOS LOS LOGROS
    mecanicas_logros LOGROS;

    private IEnumerator Start()
    {
        //TRAEMOS EL USUARIO DE LA BASE DE DATOS Y DE SU SINGLETON
        singleton = Usuario.instancia;
        CRUD = GameObject.Find("Crud").GetComponent<crud>();
        var jugador_task = CRUD.GetComponent<crud>().Cargar_usuario();
        yield return new WaitUntil( ()=> jugador_task.IsCompleted);
        jugador = jugador_task.Result;

        //INICIALIZAMOS LOS LOGROS
        LOGROS = new mecanicas_logros(jugador);

        //TRAEMOS EL MENU
        menuCtrl = GameObject.Find("menu").gameObject.GetComponent<routing>();

        //POS INICIALES
        pos_inicial_x = 0F;
        pos_inicial_y = 0F;
        //BUSCAMOS LOS PANELES DE LA TIENDA
        panel_promociones = GameObject.Find("panel_promociones");
        panel_personajes = GameObject.Find("panel_personajes");
        panel_energia = GameObject.Find("panel_energia");
        panel_despertar = GameObject.Find("panel_despertar");
        panel_diamantes = GameObject.Find("panel_diamantes");
        panel_armas = GameObject.Find("panel_armas");
        panel_oro = GameObject.Find("panel_oro");

        //DESACTIVAMOS LOS PANELES MENOS EL DE PROMOCIONES
        Importar_datos_tienda("promociones");
        panel_personajes.SetActive(false);
        panel_energia.SetActive(false);
        panel_despertar.SetActive(false);
        panel_diamantes.SetActive(false);
        panel_armas.SetActive(false);
        panel_oro.SetActive(false);
    }

    public void Abrir_panel(string panel)
    {
        Cerrar_panel_activo();
        switch(panel)
        {
            case "promociones":
                panel_promociones.SetActive(true);
                Importar_datos_tienda("promociones");
                break;
            case "personajes":
                panel_personajes.SetActive(true);
                Importar_datos_tienda("personajes");
                break;
            case "energia":
                panel_energia.SetActive(true);
                Importar_datos_tienda("energia");
                break;
            case "despertar":
                panel_despertar.SetActive(true);
                Importar_datos_tienda("despertar");
                break;
            case "diamantes":
                panel_diamantes.SetActive(true);
                Importar_datos_tienda("diamantes");
                break;
            case "armas":
                panel_armas.SetActive(true);
                Importar_datos_tienda("armas");
                break;
            case "oro":
                panel_oro.SetActive(true);
                Importar_datos_tienda("oro");
                break;
            default:
                break;
        }
    }

    void Cerrar_panel_activo()
    {
        if(panel_promociones.activeSelf) panel_promociones.SetActive(false);destruirItemsInternos(panel_promociones);
        if(panel_personajes.activeSelf) panel_personajes.SetActive(false);destruirItemsInternos(panel_personajes);
        if(panel_energia.activeSelf) panel_energia.SetActive(false);destruirItemsInternos(panel_energia);
        if(panel_despertar.activeSelf) panel_despertar.SetActive(false);destruirItemsInternos(panel_despertar);
        if(panel_diamantes.activeSelf) panel_diamantes.SetActive(false);destruirItemsInternos(panel_diamantes);
        if(panel_armas.activeSelf) panel_armas.SetActive(false);destruirItemsInternos(panel_armas);
        if(panel_oro.activeSelf) panel_oro.SetActive(false);destruirItemsInternos(panel_oro);
    }

    void destruirItemsInternos(GameObject panel)
    {
        foreach (Transform child in panel.transform.GetChild(0).GetChild(0).GetChild(0)) {
            GameObject.Destroy(child.gameObject);
        }
    }

    void Importar_datos_tienda(string panel)
    {
        //DEBEMOS DE SACAR TODOS LOS DATOS DEPENDIENDO DEL PANEL, BUSCANDO EN LA DB.
        pos_inicial_y = 0F;

        switch(panel){
            case "promociones":
                //img[0] = "Shopbotton";
                this.crearItemTienda("Combo primeros 7 dias", 0, "Shopbotton", "25 usd", "20 usd", 3, "20.000 de oro, 1.000 diamantes, 10 invocaciones ( 5 normales, 3 raras, 1 mitica, 1 legendaria ).");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Combo Armas", 1,  "Shopbotton", "26 usd", "20 usd", 3, "5 armas aleatorias de rareza mitica +");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Combo Intermedio", 2, "Shopbotton", "75 usd", "50 usd", 3, "40.000 de oro, 2.000 diamantes, 10 invocaciones ( 5 raras, 3 raras, 2 legendarias ).");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Combo avanzado", 3, "Shopbotton", "110 usd", "80 usd", 3, "100.000 de oro, 3.000 diamantes, 10 invocaciones ( 6 miticas, 4 legendarias ).");
                break;
            case "personajes":
                //img[0] = "invocacion";
                this.crearItemTienda("Invocacion normal diaria", 4, "carta_normal", "", "Gratis", 0, "Gratis lo que sea, aprovecha ahora, nunca sabes cuando se acabara la promocion.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Pack invocaciones normales", 5, "carta_normal", "5 usd", "3 usd", 3, "Pack de 5 invocaciones normales. Adquiere una ventja temprana.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Pack invocaciones Raras", 6, "invocacion", "10 usd", "8 usd", 3, "Pack de 5 invocaciones raras. Olviadate de perder en el modo historia");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Pack invocaciones Miticas", 7, "carta_rara", "20 usd", "18 usd", 3, "Pack de 5 invocaciones miticas. Sube rapidamente en la lista pvp");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Pack invocaciones Legendarias", 8, "invocacion", "40 usd", "35 usd", 3, "Pack de 5 invocaciones legendarias. Adquiere los mejores personajes");
                break;
            case "energia":
                this.crearItemTienda("Recargar energia", 9, "Cristalorange", "", "30 diamantes", 2, "Recarga tu energia y no pares de jugar.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Recargar energia gratis", 10, "Cristalorange", "", "Gratis", 0, "Solo 1 vez al dia recara topda tu energia totalmente gratis.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Recargar energia pvp", 11, "pvp", "", "30 diamantes", 2, "Recarga tus puntos pvp y no pares de ganar en la arena.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("Energia ilimitada 1h", 12, "Cristalorange", "", "200 diamantes", 2, "Recarga tu energia por 1h y aprovecha al maximo.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("P.pvp ilimitados 1h", 13, "pvp", "", "200 diamantes", 2, "Recarga tu energia pvp por 1h y acaba con tus enemigos en la arena.");
                break;
            case "despertar":
                //img[0] = "Questbotton";
                break;
            case "diamantes":
                this.crearItemTienda("100 diamantes", 14, "Diamantes", "5 usd", "3 usd", 3, "Adquiere ventaja sobre tus oponentes ahora mismo.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("600 diamantes", 15, "Diamantes", "18 usd", "10 usd", 3, "Adquiere este combo y consigu la maxima ventaja en el juego.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("3000 diamantes", 16, "Diamantes", "50 usd", "20 usd", 3, "Se el mejor entre tus amigos con este combo de diamantes.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("5000 diamantes", 17, "Diamantes", "80 usd usd", "40 usd", 3, "Arrasa con todos con este combo inigualable.");
                break;
            case "armas":
                //img[0] = "Itembotton";
                break;
            case "oro":
                //img[0] = "Oro";
                this.crearItemTienda("5.000 Oro", 18, "Oro", "5 usd", "3 usd", 3, "Una pequeña ventaja adicional.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("10.000 Oro", 19, "Oro", "10 usd", "5 usd", 3, "Adquiere mas oro y mejora tu equipo.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("30.000 Oro", 20, "Oro", "15 usd", "10 usd", 3, "Adquiere mas oro de lo que tus bolsillos pueden guardar.");
                pos_inicial_y -= 100F;
                this.crearItemTienda("60.000 Oro", 21, "Oro", "30 usd", "20 usd", 3, "Con esta ventaja, no sera duro que tu equipo haga una mejora instantanea");
                pos_inicial_y -= 100F;
                this.crearItemTienda("100.000 Oro", 22, "Oro", "50 usd usd", "30 usd", 3, "Adquiere ese combo y se la envidia de los demas, conseguiras el titulo de banquero con el.");
                break;
            default:
                //img[0] = "Shopbotton";
                break;
        }
    }

    //COMPRAR EN LA STORE;
    void Comprar(itemTienda item)
    {   
        int precio;
        precio = (item.codigoPrecio != 0) ? int.Parse(item.precio.Substring(0, item.precio.IndexOf(" "))) : 0;

        if (item.codigoPrecio == 0){

        }
        else if (item.codigoPrecio == 1)
        {
            if (jugador.monedas.oro - precio <= 0) return;
            jugador.Sumar_monedas("oro", -precio);
        }
        else if(item.codigoPrecio == 2)
        {
            if (jugador.monedas.diamantes - precio <= 0) return;
            jugador.Sumar_monedas("diamantes", -precio);
            LOGROS.RevisarOtros(null, true);
        }
        else
        {
             //PAGO POR GOOGLE PLAY
            LOGROS.RevisarOtros(null, true);
        }

        switch(item.codigoItem)
        {
            case 0:
                jugador.Sumar_monedas("oro",20000);
                jugador.Sumar_monedas("diamantes",1000);
                jugador.Sumar_monedas("invocaciones_normales", 5);
                jugador.Sumar_monedas("invocaciones_raras", 3);
                jugador.Sumar_monedas("invocaciones_miticas", 1);
                jugador.Sumar_monedas("invocaciones_legendarias", 1);
                break;
            case 1:
                //jugador.monedas.invocacion_e_mitico += 5;
                break;
            case 2:
                jugador.Sumar_monedas("oro", 40000);
                jugador.Sumar_monedas("diamantes", 2000);
                jugador.Sumar_monedas("invocaciones_raras", 5);
                jugador.Sumar_monedas("invocaciones_miticas", 3);
                jugador.Sumar_monedas("invocaciones_legendarias", 2);
                break;
            case 3:
                jugador.Sumar_monedas("oro", 100000);
                jugador.Sumar_monedas("diamantes", 3000);
                jugador.Sumar_monedas("invocaciones_miticas", 6);
                jugador.Sumar_monedas("invocaciones_legendarias", 4);
                break;
            case 4:
                jugador.Sumar_monedas("invocaciones_normales", 1);
                break;
            case 5:
                jugador.Sumar_monedas("invocaciones_normales", 5);
                break;
            case 6:
                jugador.Sumar_monedas("invocaciones_raras", 5);
                break;
            case 7:
                jugador.Sumar_monedas("invocaciones_miticas", 5);
                break;
            case 8:
                jugador.Sumar_monedas("invocaciones_legendarias", 5);
                break;
            case 9:
                jugador.energia = jugador.energia_maxima;
                break;
            case 10:
                jugador.energia = jugador.energia_maxima;
                break;
            case 11:
                jugador.energia_pvp += 15;
                break;
            case 12:
                //jugador.energia = 99999;
                break;
            case 13:
                //jugador.energia_pvp = 99999;
                break;
            case 14:
                jugador.Sumar_monedas("diamantes",100);
                break;
            case 15:
                jugador.Sumar_monedas("diamantes",600);
                break;
            case 16:
                jugador.Sumar_monedas("diamantes",3000);
                break;
            case 17:
                jugador.Sumar_monedas("diamantes",5000);
                break;
            case 18:
                jugador.Sumar_monedas("oro",5000);
                break;
            case 19:
                jugador.Sumar_monedas("oro",10000);
                break;
            case 20:
                jugador.Sumar_monedas("oro",30000);
                break;
            case 21:
                jugador.Sumar_monedas("oro",60000);
                break;
            case 22:
                jugador.Sumar_monedas("oro",100000);
                break;
            default:
                break;
        }
        this.Guardar_cambios(jugador);
        this.menuCtrl.ir_tienda();
    }

    //CREAMOS UN ITEM, EL STRING DE LA IMAGEN ES EL NOMBRE DEL ARCHIVO EN LA CARPETA BOTONES
    //LOS CAMBIOS DE LINEA EN LA DESCRIPCION SON UN ; CADA 27 CHARS
    private void crearItemTienda(string titulo, int codigoItem, string img, string precioAntes, string precio, int codigoPrecio, string descripcion)
    {
        //INSTANCIAMOS CADA ITEM
        GameObject item = Instantiate(this.prefab_item, new Vector3(this.pos_inicial_x, this.pos_inicial_y, 0), Quaternion.identity);
        item.transform.SetParent(GameObject.Find("Content").transform, false);

        //CAMBIAMOS SU TEXTO, IMAGEN, DESCRIPCION, PRECIO Y LE ASIGNAMOS UN LISTENER AL BOTON PARA SER COMPRADO
        GameObject txt_titulo = item.transform.GetChild(1).gameObject;
        txt_titulo.GetComponent<Text>().text = titulo;

        GameObject img_item = item.transform.GetChild(2).gameObject;
        img_item.GetComponent<Image>().sprite = Resources.Load <Sprite>("botones/" + img);//INFO DE LA BD

        GameObject objeto_precios = item.transform.GetChild(3).gameObject;
        GameObject txt_precio = objeto_precios.transform.GetChild(0).gameObject;
        GameObject objeto_precio_antes = objeto_precios.transform.GetChild(1).gameObject;
        GameObject txt_precio_antes = objeto_precio_antes.transform.GetChild(1).gameObject;
        txt_precio.GetComponent<Text>().text = precio;//INFO DE LA BD
        if(precioAntes != "") txt_precio_antes.GetComponent<Text>().text = precioAntes;

        GameObject txt_explicacion = item.transform.GetChild(4).gameObject;
        string _descripcion = descripcion.Replace(";", "\n");
        txt_explicacion.GetComponent<Text>().text = _descripcion;

        GameObject objeto_boton = item.transform.GetChild(5).gameObject;
        Button btn = objeto_boton.GetComponent<Button>();
        //estructura del item
        itemTienda itemCreado = new itemTienda();
        itemCreado.titulo = titulo;
        itemCreado.codigoItem = codigoItem;
        itemCreado.img = img;
        itemCreado.precioAntes = precioAntes;
        itemCreado.precio = precio;
        itemCreado.codigoPrecio = codigoPrecio;
        itemCreado.descripcion = descripcion;
        btn.onClick.AddListener(delegate { Comprar(itemCreado); });
        
    }

    private void Guardar_cambios(Usuario nuevo_val)
    {
        this.CRUD.Guardar_usuario(nuevo_val);
        this.singleton.Actualizar_usuario(nuevo_val);
    }
}
