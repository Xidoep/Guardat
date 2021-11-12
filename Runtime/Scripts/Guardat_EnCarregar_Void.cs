using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Guardat_EnCarregar_Void : Guardat_EnCarregar
{
    public UnityEvent esdeveniment;
    public override void Actualitzar() => esdeveniment.Invoke();

#if UNITY_EDITOR
    [MenuItem("GameObject/Xido Studio/Guardar/Add EnCarregar_Void")]
    public static void Add()
    {
        if (Selection.activeGameObject == null) 
            return;

        Undo.RecordObject(Selection.activeGameObject, $"Afegir component a trabes d'un script fet per mi");
        Selection.activeGameObject.AddComponent<Guardat_EnCarregar_Void>();
    }
#endif
}
