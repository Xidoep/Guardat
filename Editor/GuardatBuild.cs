using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
class GuardatBuild
{
    static GuardatBuild() 
    {
        BuildingExtraCheckings.checkings -= Test;
        BuildingExtraCheckings.checkings += Test;
    }
    static Guardat guardat;

    static void Test()
    {
        Debug.Log("...start checking [GUARDAT]");

        guardat = (Guardat)AssetDatabase.LoadAssetAtPath("Assets/XidoStudio/Guardat/Runtime/Guardat.asset", typeof(Guardat));

        Debug.Log("Checking scriptables...");
        if (guardat.scriptablesCloud.Length > 0)
        {
            for (int i = 0; i < guardat.scriptablesCloud.Length; i++)
            {
                if (guardat.scriptablesCloud[i] == null)
                {
                    throw new System.NotImplementedException($"[GUARDAT] The scriptable object with the index [{i}] is empty!!! It could create crashes on build. FIX IT!");
                }
            }
            Debug.Log("... scriptables checked!!!");
        }
        else Debug.Log("...there are no scriptables.");

        Debug.Log("Checking scriptablesLocals...");
        if (guardat.scriptablesLocals.Length > 0)
        {
            for (int i = 0; i < guardat.scriptablesLocals.Length; i++)
            {
                if (guardat.scriptablesLocals[i] == null)
                {
                    throw new System.NotImplementedException($"[GUARDAT] The scriptableLocal object with the index [{i}] is empty!!! It could create crashes on build. FIX IT!");
                }
            }
            Debug.Log("... scriptablesLocals checked!!!");
        }
        else Debug.Log("...there are no scriptablesLocals.");

        Debug.Log("...end checking [GUARDAT]");
        Debug.Log("-----------------------------------------------");
    }
}
