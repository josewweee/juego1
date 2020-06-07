using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barra_exp : MonoBehaviour
{
    //SLIDER PARA MOVER EL RELLENO DE VIDA (UNA IMAGEN)
    public Slider slider;

    //GRADIENTE PARA CAMBIAR EL COLOR DEL RELLNO DE VIDA
    public Gradient gradient;

    //IMAGEN PARA SER EL RELLENO DE VIDA EN EL SLIDER
    public Image relleno;


    //PONEMOS EL SLIDER AL MAXIMO
    public void Set_exp_maxima(int expMax)
    {
        slider.maxValue = expMax;
    }

    //PONEMOS EL SLIDER SEGUN EL VALOR DE VIDA QUE NOS ENVIEN
    public void Set_exp(int exp)
    {
        slider.value = exp;
        relleno.color = gradient.Evaluate(slider.normalizedValue);
    }
}
