using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableVariable<T>
{
    public SavableVariable(Guardat guardat, string key, bool local, T perDefecte)
    {
        this.guardat = guardat;
        this.key = key;
        this.local = local;
        this.perDefecte = perDefecte;
    }
    public SavableVariable(string key, bool local, T perDefecte)
    {
        guardat = guardat;
        this.key = key;
        this.local = local;
        this.perDefecte = perDefecte;
        Validate();
    }

    [SerializeField] Guardat guardat;
    [SerializeField] string key;
    [SerializeField] bool local;
    [SerializeField] T perDefecte;
    public T Valor
    {
        get
        {
            if(guardat != null) 
                return (T)guardat.Get(key, perDefecte);
            else return perDefecte;
        }
        set
        {
            guardat.Set(key, value, local);
        }
    }

    public T PerDefecte => perDefecte;

    void Validate()
    {
        if (guardat == null) guardat = XS_Utils.XS_Editor.LoadGuardat<Guardat>();
    }
}


