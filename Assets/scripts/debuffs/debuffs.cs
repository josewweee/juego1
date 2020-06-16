using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debuffs 
{
   public Dictionary<string, string[]> mapaDebuffs;
    // 72 ES DE PRUEBA, CAMBIAR NUMERO
   public debuffs()
   {
       mapaDebuffs = new Dictionary<string, string[]>();
       mapaDebuffs.Add("revitalizar", new string[] {"Te curas cada turno.", "72"});
       mapaDebuffs.Add("aumentar_fuerza", new string[] {"Te vuelves mas fuerte.", "65"});
       mapaDebuffs.Add("aumentar_vitalidad", new string[] {"Obtienes mas vida.", "25"});
       mapaDebuffs.Add("aumentar_magia", new string[] {"Mas poder magico.", "72"});
       mapaDebuffs.Add("aumentar_velocidad", new string[] {"Atacas mas frecuentemente.", "27"});
       mapaDebuffs.Add("aumentar_critico", new string[] {"Mas % de golpe crit.", "72"});
       mapaDebuffs.Add("aumentar_defensa_fisica", new string[] {"Reduces el daño fisico.", "66"});
       mapaDebuffs.Add("aumentar_defensa_magica", new string[] {"Reduces el daño magico.", "28"});
       mapaDebuffs.Add("aumentar_fuerza_magia", new string[] {"Eres mas fuerte y mas magico.", "72"});
       mapaDebuffs.Add("aumentar_defensas", new string[] {"Recibes menos daño de todas las fuentes.", "30"});
       mapaDebuffs.Add("aumentar_velocidad_critico", new string[] {"Atacas mas frecuentemente y con mas % de crit.", "72"});
       mapaDebuffs.Add("inmunidad_magica", new string[] {"Inmune a ataques magicos.", "72"});
       mapaDebuffs.Add("inmunidad_fisica", new string[] {"Inmune a ataques fisicos.", "72"});
       mapaDebuffs.Add("inmunidad", new string[] {"Inmune a todas las fuentes.", "72"});
       mapaDebuffs.Add("robo_vida", new string[] {"Te curas un % de tu daño.", "72"});
       mapaDebuffs.Add("aumentar_estadisticas", new string[] {"Aumentas todas tus estadisticas.", "72"});
       mapaDebuffs.Add("ignorar_defensa_fisica", new string[] {"ignoras un % de la defensa fisica enemiga.", "72"});
       mapaDebuffs.Add("ignorar_defensa_magica", new string[] {"ignoras un % de la defensa magica enemiga.", "72"});
       mapaDebuffs.Add("ignorar_defensas", new string[] {"ignoras un % de las defensas enemigas.", "72"});
       mapaDebuffs.Add("locura", new string[] {"Aumentas tu daño pero reduces tu defensa.", "29"});
       mapaDebuffs.Add("heridas_graves", new string[] {"Te curaras un % menos.", "102"});
       mapaDebuffs.Add("sangrado", new string[] {"Recibes daño cada turno.", "72"});
       mapaDebuffs.Add("reducir_fuerza", new string[] {"Haces menos daño fisico.", "72"});
       mapaDebuffs.Add("reducir_vitalidad", new string[] {"Tienes menos vitalidad max.", "72"});
       mapaDebuffs.Add("reducir_magia", new string[] {"Haces menos daño magico.", "72"});
       mapaDebuffs.Add("reducir_velocidad", new string[] {"Atacas con menor frecuencia.", "72"});
       mapaDebuffs.Add("reducir_critico", new string[] {"Menos % de golpe critico.", "72"});
       mapaDebuffs.Add("reducir_defensa_fisica", new string[] {"Tienes menos defensa fisica.", "72"});
       mapaDebuffs.Add("reducir_defensa_magica", new string[] {"Tienes menos defensa magica.", "72"});
       mapaDebuffs.Add("reducir_fuerza_magia", new string[] {"Haces menos daño fisico y magico.", "72"});
       mapaDebuffs.Add("reducir_defensas", new string[] {"Defensas reducidas.", "72"});
       mapaDebuffs.Add("reducir_velocidad_critico", new string[] {"Atacas con menor frecuencia y menor % de crit.", "72"});
       mapaDebuffs.Add("reducir_estadisticas", new string[] {"Estadisticas reducidas.", "72"});
       mapaDebuffs.Add("quemar", new string[] {"Daño de fuego cada turno.", "95"});
       mapaDebuffs.Add("veneno", new string[] {"Daño de naturaleza cada turno.", "17"});
       mapaDebuffs.Add("quemar_grave", new string[] {"Daño de fuego cada turno y curacion reducida.", "72"});
       mapaDebuffs.Add("congelar", new string[] {"Congelado, no puedes actuar.", "96"});
       mapaDebuffs.Add("aturdir", new string[] {"Aturdido, no puedes actuar.", "20"});
       mapaDebuffs.Add("dormir", new string[] {"Dormido, no puedes actuar.", "21"});
       mapaDebuffs.Add("silenciar", new string[] {"Dormido, no puedes lanzar poderes.", "19"});
       mapaDebuffs.Add("muerto", new string[] {"Muerto, no puedes actuar.", "16"});
       
       
   }
}
