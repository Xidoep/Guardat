using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableVariable<T>
{
    public SavableVariable(string key, Guardat.Direccio direccio, T perDefecte)
    {
        if (guardat == null) guardat = XS_Utils.XS_Editor.LoadGuardat<Guardat>();
        this.key = key;
        this.direccio = direccio;
        this.perDefecte = perDefecte;
    }

    [SerializeField] Guardat guardat;
    [SerializeField] string key;
    [SerializeField] Guardat.Direccio direccio;
    [SerializeField] T perDefecte;



    public T Valor
    {
        get => guardat.Get(key, perDefecte);
        set => guardat.Set(key, value, direccio);
    }
    public T Reset() 
    {
        Valor = perDefecte;
        return Valor;
    }



}


