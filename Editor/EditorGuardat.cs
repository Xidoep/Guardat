using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class EditorGuardat : EditorWindow
{
    [SerializeField] VisualTreeAsset tree;
    StyleSheet styleSheet;
    Guardat guardat;

    [MenuItem("Guardat/Open")]
    public static void ShowExample()
    {
        EditorGuardat wnd = GetWindow<EditorGuardat>();
        wnd.titleContent = new GUIContent("EditorGuardat");
    }

    public void CreateGUI()
    {
        guardat = AssetDatabase.LoadAssetAtPath<Guardat>("Assets/XidoStudio/Guardat/Runtime/Guardat.asset");
        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/XidoStudio/Guardat/Editor/EditorGuardat.uss");

        tree.CloneTree(rootVisualElement);

        // Each editor window contains a root VisualElement object
        //VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C# asd");
        //root.Add(label);

        //SerializedObject serializedObject = new SerializedObject(guardat);
        //serializedObject.FindProperty("scriptables");

        Foldout foldout = rootVisualElement.Q<Foldout>("Scriptables");

        SerializedProperty sp = new SerializedObject(guardat).FindProperty("scriptables");
        for (int i = 0; i < sp.arraySize; i++)
        {
            Debug.Log(sp.GetArrayElementAtIndex(i).objectReferenceValue);
            //VisualElement prova = new PropertyField(sp.GetArrayElementAtIndex(i));
            ObjectField of = new ObjectField();
            of.objectType = typeof(ScriptableObject);
            of.value = sp.GetArrayElementAtIndex(i).objectReferenceValue;
            of.RegisterValueChangedCallback(evt => 
            {
                Debug.Log((ScriptableObject)evt.newValue);
                Debug.Log(i);
                guardat.scriptables[i - 1] = (ScriptableObject)evt.newValue; 
            });
            of.styleSheets.Add(styleSheet);
            foldout.Add(of);
        }
        

        


        // Import UXML
        /*var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/XidoStudio/Guardat/Editor/EditorGuardat.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        Button guardar = root.Q<Button>("Guardar");
        Button carregar = root.Q<Button>("Carregar");

        guardar.clicked += guardat.Guardar;
        carregar.clicked += guardat.Carregar;

        PropertyField pf = new PropertyField(sp);
        root.Add(pf);*/

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/XidoStudio/Guardat/Editor/EditorGuardat.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        //root.Add(labelWithStyle);
    }
}