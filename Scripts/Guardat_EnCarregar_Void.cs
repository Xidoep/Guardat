using UnityEngine.Events;

public class Guardat_EnCarregar_Void : Guardat_EnCarregar
{
    public UnityEvent esdeveniment;
    public override void Actualitzar() => esdeveniment.Invoke();
}
