using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Guardat))]
public class GuardatInspector : Editor
{
    GUIStyle _texte;
    GUIStyle _bold;

    Guardat guardat;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(guardat == null) guardat = (Guardat)target;

        if (_texte == null) _texte = new GUIStyle(GUI.skin.label) {fixedWidth = 90 };
        if (_bold == null) _bold = new GUIStyle(GUI.skin.label) {fixedWidth = 90, fontStyle = FontStyle.Bold };

        EditorGUILayout.LabelField("INFORMACIÓ", _bold);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField("KEY", _bold);
        EditorGUILayout.TextField("VALOR", _texte);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        for (int i = 0; i < guardat.dadesCloud.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(guardat.dadesCloud[i].key, _bold);
            if (guardat.dadesCloud[i].dada != null)
            {
                EditorGUILayout.TextField(guardat.dadesCloud[i].dada.ToString(), _texte);

                if (GUILayout.Button("X") && EditorUtility.DisplayDialog("Borrar?", "Segur que vols borrar la dada?", "BORRAR!", "no no no"))
                {
                    Undo.RecordObject(guardat, "guardar guardat...");
                    guardat.dadesCloud.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        for (int i = 0; i < guardat.dadesLocals.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(guardat.dadesLocals[i].key, _bold);
            if (guardat.dadesLocals[i].dada != null)
            {
                EditorGUILayout.TextField(guardat.dadesLocals[i].dada.ToString(), _texte);

                if (GUILayout.Button("X") && EditorUtility.DisplayDialog("Borrar?", "Segur que vols borrar la dada?", "BORRAR!", "no no no"))
                {
                    Undo.RecordObject(guardat, "guardar guardat...");
                    guardat.dadesLocals.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("CARPETA", _bold);
        EditorGUILayout.LabelField($"Windows:   %USERPROFILE%/AppData/LocalLow/{Application.companyName}/{Application.productName}/guar.dat");
        EditorGUILayout.LabelField($"Mac OS:   ~/Library/Application Suport/unity.{Application.companyName}.{Application.productName}/guar.dat");
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField($"Windows:   %USERPROFILE%/AppData/LocalLow/{Application.companyName}/{Application.productName}/scriptab.les");
        EditorGUILayout.LabelField($"Mac OS:   ~/Library/Application Suport/unity.{Application.companyName}.{Application.productName}/scriptab.les");
    }

    
}
