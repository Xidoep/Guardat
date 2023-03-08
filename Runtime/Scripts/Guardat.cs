using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using XS_Utils;

[DefaultExecutionOrder(-10)]
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

    //public static Guardat Instance;

    [SerializeField] public List<Dada> dadesCloud = new List<Dada>();
    [SerializeField] public List<Dada> dadesLocals = new List<Dada>();
    [SerializeField] public ScriptableObject[] scriptablesCloud;
    [SerializeField] public ScriptableObject[] scriptablesLocals;
    //[SerializeField] public List<Scriptable> scriptables = new List<Scriptable>();

    public float tempsDeJoc;

    public Action onLoad = null;
    public Action onSave = null;

    public Action saveingQueue;
    string Path(string arxiu) => $"{Application.persistentDataPath}{arxiu}";





    /// <summary>
    /// Funcio pensada per ser cridada al RuntimeEsdeveniments
    /// </summary>
    public void Iniciar() => tempsDeJoc = 0;
    //********************
    //FUNCIONS STANDARS
    //********************
    private void OnEnable() 
    {
        //Instance = this;
        Debugar.Log("[Guardat] OnEnable => Carregar()");
        tempsDeJoc = 0;
        Carregar();
    }

    //object tmp;
    bool trobada;
    Dada dada;

    //FUNCIONS PUBLIQUES


    public T Get<T>(string _key, T perDefecte, Direccio direccio = Direccio.Local)
    {
        T tmp = perDefecte;
        trobada = false;
        for (int i = 0; i < dadesCloud.Count; i++)
        {
            if (dadesCloud[i].key == _key)
            {
                tmp = (T)dadesCloud[i].dada;
                trobada = true;
                direccio = Direccio.Cloud;
            }
        }
        if (!trobada)
        {
            for (int i = 0; i < dadesLocals.Count; i++)
            {
                if (dadesLocals[i].key == _key)
                {
                    tmp = (T)dadesLocals[i].dada;
                    trobada = true;
                    direccio = Direccio.Local;
                }
            }
        }

        if (!trobada) //No hi es ni a Cloud ni a Locals
        {
            dada = new Dada(_key, perDefecte);
            if (direccio == Direccio.Cloud)
                dadesCloud.Add(dada);
            else dadesLocals.Add(dada);
            tmp = perDefecte;
        }
        return tmp;
    }


    public void Set(string _key, object _data, Direccio direccio)
    {
        if (direccio == Direccio.Cloud)
            SetCloud(_key, _data);
        else SetLocal(_key, _data);
    }
    public void SetLocal(string _key, object _dada)
    {
        trobada = false;

        for (int i = 0; i < dadesLocals.Count; i++)
        {
            if (dadesLocals[i].key == _key)
            {
                dadesLocals[i].dada = _dada;
                trobada = true;
                return;
            }
        }
        if (trobada)
            return;

        dadesLocals.Add(new Dada(_key, _dada));
    }

    public void SetCloud(string _key, object _dada)
    {
        bool _dadaTrobada = false;

        for (int i = 0; i < dadesCloud.Count; i++)
        {
            if (dadesCloud[i].key == _key)
            {
                dadesCloud[i].dada = _dada;
                _dadaTrobada = true;
            }
        }
        if (_dadaTrobada)
            return;

        dadesCloud.Add(new Dada(_key, _dada));
    }




    /// <summary>
    /// Guarda les dades al dispositiu.
    /// </summary>
    [ContextMenu("Guardar")]
    public void Guardar()
    {
        if (!SuficientTempsDesdeUltimGuardat())
        {
            Debugar.Log("Guardat temporalment bloquejat per prevenir multiples guardats");
            if(saveingQueue == null)
            {
                saveingQueue = Guardar;
                XS_Coroutine.StartCoroutine_Ending(SuficientTempsDesdeUltimGuardat, Guardar);
            }
            return;
        }

        FormategarDades(dadesCloud, ARXIU_DADES);
        FormategarDades(dadesLocals, ARXIU_DADES_LOCALS);

        EscriureJsonScriptables(scriptablesCloud, ARXIU_SCRIPTABLES);
        EscriureJsonScriptables(scriptablesLocals, ARXIU_SCRIPTABLES_LOCALS);

        tempsDeJoc = Time.realtimeSinceStartup;

        if (onSave != null) onSave.Invoke();

        saveingQueue = null;
        Debugar.Log("Guardar");


        return;
    }
    bool SuficientTempsDesdeUltimGuardat() => Time.realtimeSinceStartup - tempsDeJoc > 10;
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


    /// <summary>
    /// Carrega les dades del dispositiu.
    /// </summary>
    [ContextMenu("Carregar")]
    public void Carregar()
    {
        if (File.Exists(Path(ARXIU_DADES)))
        {
            Debugar.Log($"[Guardat] Carregar() => Path dades cloud = {Path(ARXIU_DADES)}");
            //Carregar les dades individuals
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(Path(ARXIU_DADES), FileMode.Open);
            GuardatDades _dades = _formatter.Deserialize(_stream) as GuardatDades;
            _stream.Close();
            
            dadesCloud.Clear();
            for (int i = 0; i < _dades.dades.Length; i++)
            {
                dadesCloud.Add(new Dada());
                dadesCloud[i].key = _dades.dades[i].key;
                dadesCloud[i].dada = _dades.dades[i].dada;
            }

        }

        if (File.Exists(Path(ARXIU_DADES_LOCALS)))
        {
            Debugar.Log($"[Guardat] Carregar() => Path dades locals = {Path(ARXIU_DADES_LOCALS)}");
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

        if (File.Exists(Path(ARXIU_SCRIPTABLES)))
        {
            Debugar.Log($"[Guardat] Carregar() => Path scriptables clouds = {Path(ARXIU_SCRIPTABLES)}");
            string[] _guardats = File.ReadAllText(Path(ARXIU_SCRIPTABLES)).Split(SEPARADOR);
            for (int i = 0; i < scriptablesCloud.Length; i++)
            {
                JsonUtility.FromJsonOverwrite(_guardats[i], scriptablesCloud[i]);
            }
        }
        if (File.Exists(Path(ARXIU_SCRIPTABLES_LOCALS)))
        {
            Debugar.Log($"[Guardat] Carregar() => Path scriptables locals = {Path(ARXIU_SCRIPTABLES)}");
            string[] _guardats = File.ReadAllText(Path(ARXIU_SCRIPTABLES_LOCALS)).Split(SEPARADOR);
            for (int i = 0; i < scriptablesLocals.Length; i++)
            {
                JsonUtility.FromJsonOverwrite(_guardats[i], scriptablesLocals[i]);
            }
        }

        tempsDeJoc = Time.realtimeSinceStartup;

        if(onLoad != null) onLoad.Invoke();

        Debugar.Log("[Guardat] Carregar()");
    }

    /// <summary>
    /// Destrueix totes les dades guardades.
    /// </summary>
    [ContextMenu("Destruir")]
    public void Destruir()
    {
        dadesCloud.Clear();
        dadesLocals.Clear();
        Guardar();
    }

    /// <summary>
    /// Destrueix la Dada amb el nom que l'hi passes.
    /// </summary>
    /// <param name="_key"></param>
    public void Borrar(string _key)
    {
        for (int i = 0; i < dadesCloud.Count; i++)
        {
            if(dadesCloud[i].key == _key)
            {
                dadesCloud.RemoveAt(i);
            }
        }
    }










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

    public enum Direccio
    {
        Local,
        Cloud
    }
}


