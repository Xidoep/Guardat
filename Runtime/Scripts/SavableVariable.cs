using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableVariable<T>
{
    [SerializeField] Guardat guardat;
    [SerializeField] string key;
    [SerializeField] bool local;
    [SerializeField] T valor;
    public T Valor
    {
        get
        {
            if((object)valor != guardat.Get(key, valor))
            {
                valor = (T)guardat.Get(key, valor);
            }
            return valor;
        }
        set
        {
            valor = value;
            if (local) guardat.SetLocal(key, value);
            else guardat.SetCloud(key, value);
        }
    }

    public string Key { get => key; set => key = value; }
    public bool Local => local;
    public Guardat Guardat => guardat;

    /// <summary>
    /// It works as a constructor.
    /// </summary>
    public void Define(Guardat guardat, string key, bool local, T valor)
    {
        this.guardat = guardat;
        this.key = key;
        this.local = local;
        this.valor = valor;
    }
}

public class SavableClass<T> where T : class
{

}


