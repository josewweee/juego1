using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class configuracion : MonoBehaviour
{
    // true = graficos && false = cuenta
    private GameObject menu_graficos;
    private GameObject menu_cuenta;

    void Start() {
        menu_graficos = GameObject.Find("panel_graficos");
        menu_cuenta = GameObject.Find("panel_cuenta");
        menu_cuenta.SetActive(false);
    }
    public void cambiar_menu(bool valor){
        if (!valor){
            menu_graficos.SetActive(false);
            menu_cuenta.SetActive(true);
        }else{
            menu_cuenta.SetActive(false);
            menu_graficos.SetActive(true);
        }
    }
    

}
