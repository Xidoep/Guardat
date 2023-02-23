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
}


