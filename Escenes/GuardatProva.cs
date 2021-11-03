using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardatProva : MonoBehaviour {

    public Guardat guardat;

    public bool fet;
    public int vida;
    public float temps;
    public string nom;
    public Vector2 direccio;

    //public So so;

    Guardat_EnCarregar<string> Nom;
    Guardat_EnEnable<float> Temps;

    private void OnEnable()
    {
        Nom = new Guardat_EnCarregar<string>(guardat,"nom", "ei", (string arg) => { nom = arg; Debug.Log("prova crear event on the fly"); });
        Temps = new Guardat_EnEnable<float>(guardat, "temps", -1, (float arg) => { temps = arg; Debug.Log("prova agafar info on the fly al enable"); });
    }


    [ContextMenu("Enviar")]
    void Enviar()
    {
        guardat.Set("fet", fet);
        guardat.Set("vida", vida);
        guardat.Set("temps", temps, true);
        guardat.Set("nom", nom);
        //guardat.Set("direccio", direccio);
        //Guardat.Set("So", so);
    }

    [ContextMenu("Agafar")]
    void Agafar()
    {
        fet = (bool)guardat.Get("fet", true);
        vida = (int)guardat.Get("vida", 12);
        temps = (float)guardat.Get("temps", 12.3f);
        nom = (string)guardat.Get("nom", "nom");
        //direccio = (Vector2)guardat.Get("direccio", direccio);
        //so = (So)Guardat.Get("So", null);
    }

    
}
