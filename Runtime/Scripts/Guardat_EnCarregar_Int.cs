#if UNITY_EDITOR
using UnityEditor;
#endif
public class Guardat_EnCarregar_Int : Guardat_EnCarregarPublic<int> 
{
#if UNITY_EDITOR
    [MenuItem("GameObject/Xido Studio/Guardar/Add EnCarregar_Int")]
    public static void Add()
    {
        if (Selection.activeGameObject == null) 
            return;

        Undo.RecordObject(Selection.activeGameObject, $"Afegir component a trabes d'un script fet per mi");
        Selection.activeGameObject.AddComponent<Guardat_EnCarregar_Int>();
    }
#endif
}
