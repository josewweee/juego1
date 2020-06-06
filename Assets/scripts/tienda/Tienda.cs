using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{
    public GameObject prefab_item;
    private GameObject panel_promociones;
    private GameObject panel_personajes;
    private GameObject panel_energia;
    private GameObject panel_despertar;
    private GameObject panel_diamantes;
    private GameObject panel_armas;
    private GameObject panel_oro;

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
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
        if(panel_promociones.activeSelf) panel_promociones.SetActive(false);
        if(panel_personajes.activeSelf) panel_personajes.SetActive(false);
        if(panel_energia.activeSelf) panel_energia.SetActive(false);
        if(panel_despertar.activeSelf) panel_despertar.SetActive(false);
        if(panel_diamantes.activeSelf) panel_diamantes.SetActive(false);
        if(panel_armas.activeSelf) panel_armas.SetActive(false);
        if(panel_oro.activeSelf) panel_oro.SetActive(false);
    }

    void Importar_datos_tienda(string panel)
    {
        float pos_inicial_x = 0F;
        float pos_inicial_y = 0F;

        //DEBEMOS DE SACAR TODOS LOS DATOS DEPENDIENDO DEL PANEL, BUSCANDO EN LA DB.
        string img = "";
        int cantidad;
        switch(panel){
            case "promociones":
                img = "Shopbotton";
                break;
            case "personajes":
                img = "invocacion";
                break;
            case "energia":
                img = "Cristalorange";
                break;
            case "despertar":
                img = "Questbotton";
                break;
            case "diamantes":
                img = "Diamantes";
                break;
            case "armas":
                img = "Itembotton";
                break;
            case "oro":
                img = "Oro";
                break;
            default:
                img = "Shopbotton";
                break;
        }
        
        cantidad = 3; //SACARLA DE LA BD



        for(int i = 0; i < cantidad; i++)
        {
            //INSTANCIAMOS CADA ITEM
            GameObject item = Instantiate(this.prefab_item, new Vector3(pos_inicial_x, pos_inicial_y, 0), Quaternion.identity);
            item.transform.SetParent(GameObject.Find("Content").transform, false);

            //CAMBIAMOS SU TEXTO, IMAGEN, DESCRIPCION, PRECIO Y LE ASIGNAMOS UN LISTENER AL BOTON PARA SER COMPRADO
            GameObject txt_titulo = item.transform.GetChild(1).gameObject;
            txt_titulo.GetComponent<Text>().text = "10,000 De Oro";//INFO DE LA BD

            GameObject img_item = item.transform.GetChild(2).gameObject;
            img_item.GetComponent<Image>().sprite = Resources.Load <Sprite>("botones/" + img);//INFO DE LA BD

            GameObject objeto_precios = item.transform.GetChild(3).gameObject;
            GameObject txt_precio = objeto_precios.transform.GetChild(0).gameObject;
            GameObject objeto_precio_antes = objeto_precios.transform.GetChild(1).gameObject;
            GameObject txt_precio_antes = objeto_precio_antes.transform.GetChild(1).gameObject;
            txt_precio.GetComponent<Text>().text = "10 usd";//INFO DE LA BD
            txt_precio_antes.GetComponent<Text>().text = "25 usd";

            GameObject txt_explicacion = item.transform.GetChild(4).gameObject;
            string descripcion = "Consigue esta promocion de ;oro unica y nunca te adquiere ;mas ventaja que los demas."; //INFO DB
            descripcion = descripcion.Replace(";", "\n");
            txt_explicacion.GetComponent<Text>().text = descripcion;

            GameObject objeto_boton = item.transform.GetChild(5).gameObject;
            Button btn = objeto_boton.GetComponent<Button>();
            btn.onClick.AddListener(delegate { Comprar(item); });

            
            pos_inicial_y -= 100F;
        }
    }

    void Comprar(GameObject item)
    {
        //COMPRAR EN LA STORE;
    }
}
