using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Equipo
{
    public string nombre;
    public int estrellas;
    public Atributos atributos;
    public Poderes[] poderes;
    public Poderes poderActivo; // 1
    public int nivel;
    public int nivel_maximo;
    public int experiencia;
    public int experiencia_maxima;
    public int costo_fragmentos;
    public int fragmentos;
    public string foto_perfil;
    public string imagen_completa;
    public bool esta_equipada;



    public virtual bool Asignar_poder(Poderes poder)
    {

        if(poderActivo != null && poder.nombre == poderActivo.nombre)
        {
            poderActivo = null;
            return false;
        }


        if (poderActivo == null)
        {
            poderActivo = poder;
            return true;
        }
        return false;
    }


    public void Evolucionar()
    {
        switch(estrellas){
            case 0:
                if (fragmentos >= 20){
                    fragmentos -= 20;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 1:
                if (fragmentos >= 40){
                    fragmentos -= 40;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 2:
            if (fragmentos >= 60){
                    fragmentos -= 60;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 3:
            if (fragmentos >= 80){
                    fragmentos -= 80;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 4:
            if (fragmentos >= 100){
                    fragmentos -= 100;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 5:
            if (fragmentos >= 120){
                    fragmentos -= 120;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            case 6:
            if (fragmentos >= 140){
                    fragmentos -= 140;
                    estrellas ++;
                    nivel_maximo += 10;
                }
                break;
            default:
                Debug.Log("estrella erroneas " + estrellas);
                break;
            
        }
    }


}
