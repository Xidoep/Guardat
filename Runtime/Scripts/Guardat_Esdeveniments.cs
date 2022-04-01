using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Guardat_EnCarregar : MonoBehaviour
{
    public Guardat guardat;
    public abstract void Actualitzar();

    private void OnEnable() => guardat.onLoad += Actualitzar;
    private void OnDisable() => guardat.onLoad -= Actualitzar;

    private void OnValidate()
    {
        guardat = XS_Utils.XS_Editor.LoadGuardat<Guardat>();
    }
}

public abstract class Guardat_EnEnable : MonoBehaviour
{
    public Guardat guardat;
    public abstract void Actualitzar();
    private void OnEnable() => Actualitzar();
}


public class Guardat_EnCarregarPublic<T> : Guardat_EnCarregar
{
    public Guardat_EnCarregar<T> esdeveniment;
    public override void Actualitzar() => esdeveniment.Actualitzar(guardat);
}
public class Guardat_EnEnablePublic<T> : Guardat_EnEnable
{
    public Guardat_EnEnable<T> esdeveniment;
    public override void Actualitzar() => esdeveniment.Actualitzar(guardat);
}



[System.Serializable]
public class Guardat_Esdeveniment<T>
{
    internal Guardat guardat;
    [SerializeField] internal string key;
    [SerializeField] internal T perDefecte;
    [SerializeField] internal UnityEvent<T> esdeveniment;
    public void Actualitzar() => esdeveniment?.Invoke((T)guardat.Get(key, perDefecte));
    public void Actualitzar(Guardat guardat) => esdeveniment?.Invoke((T)guardat.Get(key, perDefecte));

    internal void Set(Guardat guardat, string key, T perDefecte, UnityAction<T> esdeveniment)
    {
        this.guardat = guardat;
        this.key = key;
        this.perDefecte = perDefecte;
        this.esdeveniment = new UnityEvent<T>();
        this.esdeveniment.AddListener(esdeveniment);
    }
}

[System.Serializable]
public class Guardat_EnCarregar<T> : Guardat_Esdeveniment<T>
{
    public Guardat_EnCarregar() { }
    public Guardat_EnCarregar(Guardat guardat, string key, T perDefecte, UnityAction<T> esdeveniment)
    {
        base.Set(guardat, key, perDefecte, esdeveniment);
        guardat.onLoad += Actualitzar;
    }
}



[System.Serializable]
public class Guardat_EnEnable<T> : Guardat_Esdeveniment<T>
{
    public Guardat_EnEnable() { }
    public Guardat_EnEnable(Guardat guardat, string key, T perDefecte, UnityAction<T> esdeveniment)
    {
        base.Set(guardat, key, perDefecte, esdeveniment);
        Actualitzar();
        //guardat.onLoad += Actualitzar;
    }
}

