using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barra_vida : MonoBehaviour
{
    //SLIDER PARA MOVER EL RELLENO DE VIDA (UNA IMAGEN)
    public Slider slider;

    //GRADIENTE PARA CAMBIAR EL COLOR DEL RELLNO DE VIDA
    public Gradient gradient;

    //IMAGEN PARA SER EL RELLENO DE VIDA EN EL SLIDER
    public Image relleno;


    //PONEMOS EL SLIDER AL MAXIMO
    public void Set_salud_maxima(float vitalidad)
    {
        slider.maxValue = vitalidad;
        slider.value = vitalidad;
        relleno.color = gradient.Evaluate(1f);
    }

    //PONEMOS EL SLIDER SEGUN EL VALOR DE VIDA QUE NOS ENVIEN
    public void Set_salud(float salud)
    {
        slider.value = salud;
        relleno.color = gradient.Evaluate(slider.normalizedValue);
    }
}
