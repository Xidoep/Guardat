using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class EditorGuardat : EditorWindow
{
    [SerializeField] VisualTreeAsset tree;
    StyleSheet styleSheet;
    Guardat guardat;

    //public bool scriptables;
    //SerializedObject sco;

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

        //scriptables = guardat.scriptables;

        tree.CloneTree(rootVisualElement);

        // Each editor window contains a root VisualElement object
        //VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C# asd");
        //root.Add(label);

        //SerializedObject serializedObject = new SerializedObject(guardat);
        //serializedObject.FindProperty("scriptables");
        //PropertyField property = rootVisualElement.Q<PropertyField>("scriptables");

        Foldout foldout_dc = rootVisualElement.Q<Foldout>("dades");
        SerializedProperty dc = new SerializedObject(guardat).FindProperty("dadesCloud");
        for (int i = 0; i < dc.arraySize; i++)
        {
            Debug.Log(dc.GetArrayElementAtIndex(i).objectReferenceValue);

            ObjectField of = new ObjectField();
            of.objectType = guardat.dadesCloud[i].dada.GetType();
            of.label = guardat.dadesCloud[i].key;
            of.value = (Object)guardat.dadesCloud[i].dada;
            of.styleSheets.Add(styleSheet);
            foldout_dc.Add(of);
        }

        Foldout foldout_dl = rootVisualElement.Q<Foldout>("dadesLocals");
        SerializedProperty dl = new SerializedObject(guardat).FindProperty("dadesLocals");
        for (int i = 0; i < dl.arraySize; i++)
        {
            TextField tf = new TextField(guardat.dadesLocals[i].key);
            tf.value = guardat.dadesLocals[i].dada.ToString();

            tf.styleSheets.Add(styleSheet);
            foldout_dl.Add(tf);
        }

        Foldout foldout = rootVisualElement.Q<Foldout>("Scriptables");
        SerializedProperty sp = new SerializedObject(guardat).FindProperty("scriptablesCloud");
        for (int i = 0; i < sp.arraySize; i++)
        {
            //VisualElement prova = new PropertyField(sp.GetArrayElementAtIndex(i));
            ObjectField of = new ObjectField();
            of.objectType = typeof(ScriptableObject);
            of.value = sp.GetArrayElementAtIndex(i).objectReferenceValue;
            of.RegisterValueChangedCallback(evt => 
            {
                Debug.Log((ScriptableObject)evt.newValue);
                Debug.Log(i);
                guardat.scriptablesCloud[i - 1] = (ScriptableObject)evt.newValue; 
            });
            of.styleSheets.Add(styleSheet);
            foldout.Add(of);
        }

        Foldout foldout2 = rootVisualElement.Q<Foldout>("ScriptablesLocals");
        SerializedProperty sp2 = new SerializedObject(guardat).FindProperty("scriptablesLocals");
        for (int i = 0; i < sp2.arraySize; i++)
        {
            //VisualElement prova = new PropertyField(sp.GetArrayElementAtIndex(i));
            ObjectField of = new ObjectField();
            of.objectType = typeof(ScriptableObject);
            of.value = sp2.GetArrayElementAtIndex(i).objectReferenceValue;
            of.RegisterValueChangedCallback(evt =>
            {
                Debug.Log((ScriptableObject)evt.newValue);
                Debug.Log(i);
                guardat.scriptablesCloud[i - 1] = (ScriptableObject)evt.newValue;
            });
            of.styleSheets.Add(styleSheet);
            foldout2.Add(of);
        }
        

        Button button = rootVisualElement.Q<Button>("Guardar");
        button.clicked += guardat.Guardar;

        Button button1 = rootVisualElement.Q<Button>("Carregar");
        button1.clicked += guardat.Carregar;
            

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