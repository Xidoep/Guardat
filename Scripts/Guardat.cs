using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[CreateAssetMenu(menuName = "Xido Studio/Guardat/Guardat variables", fileName = "Guardat", order = 200)]
public class Guardat : ScriptableObject {

    const string ARXIU_DADES = "/guar.dat";
    const string ARXIU_SCRIPTABLES = "/scriptables.dat";
    const string ARXIU_DADES_LOCALS = "/guar.loc";
    const string ARXIU_SCRIPTABLES_LOCALS = "/scriptables.loc";
    const char SEPARADOR = ';';
    const char OPEN = '{';
    const char CLOSE = '}';
    const char COMETES = '"';

    [SerializeField] public List<Dada> dades = new List<Dada>();
    [SerializeField] public List<Dada> dadesLocals = new List<Dada>();
    [SerializeField] public ScriptableObject[] scriptables;
    [SerializeField] public ScriptableObject[] scriptablesLocals;
    //[SerializeField] public List<Scriptable> scriptables = new List<Scriptable>();

    public float tempsDeJoc;

    public Action onLoad = null;
    public Action onSave = null;


    string Path(string arxiu) => $"{Application.persistentDataPath}/{arxiu}";



    //********************
    //CLASSES
    //********************
    [System.Serializable]
    public class Dada : System.Object
    {
        public Dada() { }
        public Dada(string _key, object _dada)
        {
            key = _key;
            dada = _dada;
        }

        public string key;
        public object dada;
    }
    [Serializable]
    public class Scriptable : System.Object
    {
        public Scriptable() { }
        public Scriptable(ScriptableObject _scriptableObject)
        {
            scriptableObject = _scriptableObject;
        }
        public string nom;
        public ScriptableObject scriptableObject;
    }

    [System.Serializable]
    public class GuardatDades
    {
        public Dada[] dades;

        public GuardatDades(Dada[] _dades)
        {
            dades = _dades;
        }

    }

    [System.Serializable]
    public class Noms
    {
        public string[] noms;

        public Noms(List<string> _noms)
        {
            noms = _noms.ToArray();
        }

    }

    /// <summary>
    /// Funcio pensada per ser cridada al RuntimeEsdeveniments
    /// </summary>
    public void Iniciar()
    {
        tempsDeJoc = 0;
    }
    //********************
    //FUNCIONS STANDARS
    //********************
    /*private void OnEnable()
    {
        Instance = this;
        Carregar();
    }

    private void OnDisable()
    {
        Instance = null;
    }*/





    //FUNCIONS PUBLIQUES

    /// <summary>
    /// Agafa una Dada guardada, i retorna un "objecte" que s'ha de transformar enel tipus que necessiets
    /// </summary>
    /// <param name="_key">El nom de la Dada guardada</param>
    /// <returns>Retorna un "object" que s'ha de transformar en el tipus que necessites</returns>
    public object Get(string _key, object _default)
    {
        object _tmp = _default;
        //if (Guardat.Instance)
        //{
        bool _trobat = false;
        bool _local = false;
        for (int i = 0; i < dades.Count; i++)
        {
            if (dades[i].key == _key)
            {
                _tmp = dades[i].dada;
                _trobat = true;
            }
        }
        if (!_trobat)
        {
            for (int i = 0; i < dadesLocals.Count; i++)
            {
                if (dadesLocals[i].key == _key)
                {
                    _tmp = dadesLocals[i].dada;
                    _trobat = true;
                    _local = true;
                }
            }
        }

        if (!_trobat)//Si no troba la dada, la crea. (Si la busco aquí és que hauria de ser-hi)
        {

            Dada _dada = new Dada(_key, _default);
            if (!_local)
                dades.Add(_dada);
            else dadesLocals.Add(_dada);
            _tmp = _dada.dada;
        }
        else
        {
            //Ha trobat data pero aquesta es nulla
            if (_tmp == null) _tmp = _default;
        }
        //}

        return _tmp;
    }

    /// <summary>
    /// Guarda una Dada.
    /// </summary>
    /// <param name="_key">El nom de la Dada guardada</param>
    /// <param name="_dada">El valor de la Dada</param>
    public void Set(string _key, object _dada, bool local = false)
    {
        bool _dadaTrobada = false;
        if (!local)
        {
            for (int i = 0; i < dades.Count; i++)
            {
                if (dades[i].key == _key)
                {
                    dades[i].dada = _dada;
                    _dadaTrobada = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < dadesLocals.Count; i++)
            {
                if (dadesLocals[i].key == _key)
                {
                    dadesLocals[i].dada = _dada;
                    _dadaTrobada = true;
                }
            }
        }
       
        if (_dadaTrobada)
            return;

        if (!local)
            dades.Add(new Dada(_key, _dada));
        else dadesLocals.Add(new Dada(_key, _dada));
    }



    /// <summary>
    /// Guarda les dades al dispositiu.
    /// </summary>
    [ContextMenu("Guardar")]
    public void Guardar()
    {
        if (Time.realtimeSinceStartup - tempsDeJoc < 10)
        {
            XS_Utils.Debugar.Log("Guardat temporalment bloquejat per prevenir multiples guardats");
            return;
        }

        XS_Utils.Debugar.Log("Guardar");

        //Guardat de les dades individuals en format Binari
        FormategarDades(dades, ARXIU_DADES);
        FormategarDades(dadesLocals, ARXIU_DADES_LOCALS);


        //Guardat dels scriptableObjects i els seus noms.
        /*List<string> _noms = new List<string>();
        string json = "";
        for (int i = 0; i < scriptables.Count; i++)
        {
            _noms.Add(scriptables[i].name);
            json += JsonUtility.ToJson(scriptables[i], true);
            if (i != scriptables.Count - 1)
            {
                json += SEPARADOR;
            }
        }
        noms = new Noms(_noms);
        File.WriteAllText(Path(ARXIU_SCRIPTABLES_NOM), JsonUtility.ToJson(noms, true));
        File.WriteAllText(Path(ARXIU_SCRIPTABLES), json);
        */
        EscriureJsonScriptables(scriptables, ARXIU_SCRIPTABLES);
        EscriureJsonScriptables(scriptablesLocals, ARXIU_SCRIPTABLES_LOCALS);

        tempsDeJoc = Time.realtimeSinceStartup;

        if (onSave != null) onSave.Invoke();

        return;
    }
    void FormategarDades(List<Dada> dades, string nomArxiu)
    {
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(Path(nomArxiu), FileMode.Create);
        GuardatDades _dades = new GuardatDades(dades.ToArray());
        _formatter.Serialize(_stream, _dades);
        _stream.Close();
    }
    void EscriureJsonScriptables(ScriptableObject[] scriptableObjects, string nomArxiu)
    {
        string json = "";
        for (int i = 0; i < scriptableObjects.Length; i++)
        {
            json += JsonUtility.ToJson(scriptableObjects[i], true);
            if (i != scriptableObjects.Length - 1)
            {
                json += SEPARADOR;
            }
        }
        File.WriteAllText(Path(nomArxiu), json);
    }

    void DesformategarDades()
    {

    }

    /// <summary>
    /// Carrega les dades del dispositiu.
    /// </summary>
    [ContextMenu("Carregar")]
    public void Carregar()
    {
        if (File.Exists(Path(ARXIU_DADES)))
        {

            //Carregar les dades individuals
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(Path(ARXIU_DADES), FileMode.Open);
            GuardatDades _dades = _formatter.Deserialize(_stream) as GuardatDades;
            _stream.Close();
            
            dades.Clear();
            for (int i = 0; i < _dades.dades.Length; i++)
            {
                dades.Add(new Dada());
                dades[i].key = _dades.dades[i].key;
                dades[i].dada = _dades.dades[i].dada;
            }

        }

        if (File.Exists(Path(ARXIU_DADES_LOCALS)))
        {

            //Carregar les dades individuals
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(Path(ARXIU_DADES_LOCALS), FileMode.Open);
            GuardatDades _dades = _formatter.Deserialize(_stream) as GuardatDades;
            _stream.Close();

            dadesLocals.Clear();
            for (int i = 0; i < _dades.dades.Length; i++)
            {
                dadesLocals.Add(new Dada());
                dadesLocals[i].key = _dades.dades[i].key;
                dadesLocals[i].dada = _dades.dades[i].dada;
            }

        }

        //Carregat dels ScriptableObjects segons el seu nom si estan guardats.
        /*if (File.Exists(Path(ARXIU_SCRIPTABLES)))
        {
            string[] _guardats = File.ReadAllText(Path(ARXIU_SCRIPTABLES)).Split(SEPARADOR);
            if (File.Exists(Path(ARXIU_SCRIPTABLES_NOM)))
            {
                noms = JsonUtility.FromJson<Noms>(File.ReadAllText(Path(ARXIU_SCRIPTABLES_NOM)));

                if (_guardats.Length > 0 && noms != null)
                {
                    for (int i = 0; i < noms.noms.Length; i++)
                    {
                        for (int z = 0; z < scriptables.Count; z++)
                        {
                            if (noms.noms[i] == scriptables[z].name)
                            {
                                JsonUtility.FromJsonOverwrite(_guardats[i], scriptables[z]);
                                XS_Utils.Debugar.Log(_guardats[i]);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _guardats.Length; i++)
                {
                    JsonUtility.FromJsonOverwrite(_guardats[i], scriptables[i]);
                    XS_Utils.Debugar.Log(_guardats[i]);
                }
            }
        }*/
        if (File.Exists(Path(ARXIU_SCRIPTABLES)))
        {
            string[] _guardats = File.ReadAllText(Path(ARXIU_SCRIPTABLES)).Split(SEPARADOR);
            for (int i = 0; i < scriptables.Length; i++)
            {
                JsonUtility.FromJsonOverwrite(_guardats[i], scriptables[i]);
            }
        }
        if (File.Exists(Path(ARXIU_SCRIPTABLES_LOCALS)))
        {
            string[] _guardats = File.ReadAllText(Path(ARXIU_SCRIPTABLES_LOCALS)).Split(SEPARADOR);
            for (int i = 0; i < scriptablesLocals.Length; i++)
            {
                JsonUtility.FromJsonOverwrite(_guardats[i], scriptablesLocals[i]);
            }
        }

        tempsDeJoc = Time.realtimeSinceStartup;

        if(onLoad != null) onLoad.Invoke();

        XS_Utils.Debugar.Log("Carregar");
    }

    /// <summary>
    /// Destrueix totes les dades guardades.
    /// </summary>
    public void Destruir()
    {
        dades.Clear();
        Guardar();
    }

    /// <summary>
    /// Destrueix la Dada amb el nom que l'hi passes.
    /// </summary>
    /// <param name="_key"></param>
    public void Borrar(string _key)
    {
        for (int i = 0; i < dades.Count; i++)
        {
            if(dades[i].key == _key)
            {
                dades.RemoveAt(i);
            }
        }
    }

}


