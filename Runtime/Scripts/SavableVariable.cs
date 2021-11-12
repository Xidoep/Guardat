using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableVariable<T> 
{
    [System.Serializable]
    public struct Settings
    {
        public Guardat guardat;
        public bool local;
    }
    [SerializeField] Settings settings;
    [SerializeField] string key;
    [SerializeField] T valor;
    public T Valor
    {
        get
        {
            if((object)valor != settings.guardat.Get(key, valor))
            {
                valor = (T)settings.guardat.Get(key, valor);
            }
            return valor;
        }
        set
        {
            valor = value;
            settings.guardat.Set(key, value, settings.local);
        }
    }
}

public class SavableClass<T> where T : class
{

}


