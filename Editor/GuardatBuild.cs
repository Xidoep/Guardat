using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class GuardatBuild
{
    static Guardat guardat;
    static GuardatBuild() => BuildPlayerWindow.RegisterBuildPlayerHandler(Test);

    static void Test(BuildPlayerOptions obj)
    {

        if (guardat == null) guardat = (Guardat)AssetDatabase.LoadAssetAtPath("Assets/XidoStudio/Guardat/Guardat.asset", typeof(Guardat));
        Debug.Log("Checking Guardat Scriptable objectes...");
        if (guardat.scriptables.Length > 0)
        {
            for (int i = 0; i < guardat.scriptables.Length; i++)
            {
                if (guardat.scriptables[i] == null)
                {
                    throw new System.NotImplementedException($"The scriptable object with the index [{i}] is empty!!! It could create crashes on build. FIX IT!");
                    Debug.LogError($"The scriptable object with the index [{i}] is empty!!! It could create crashes on build. FIX IT!");
                }
            }
        }

        if (guardat.scriptablesLocals.Length > 0)
        {
            for (int i = 0; i < guardat.scriptablesLocals.Length; i++)
            {
                if (guardat.scriptablesLocals[i] == null)
                {
                    throw new System.NotImplementedException($"The scriptableLocal object with the index [{i}] is empty!!! It could create crashes on build. FIX IT!");
                    Debug.LogError($"The scriptable object with the index [{i}] is empty!!! It could create crashes on build. FIX IT!");
                }
            }
        }
    }
}
