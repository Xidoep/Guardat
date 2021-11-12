#if UNITY_EDITOR
using UnityEditor;
#endif
public class Guardat_EnEnable_Float : Guardat_EnEnablePublic<float> 
{
#if UNITY_EDITOR
    [MenuItem("GameObject/Xido Studio/Guardar/Add EnEnable_Float")]
    public static void Add()
    {
        if (Selection.activeGameObject == null) 
            return;

        Undo.RecordObject(Selection.activeGameObject, $"Afegir component a trabes d'un script fet per mi");
        Selection.activeGameObject.AddComponent<Guardat_EnEnable_Float>();
    }
#endif
}
