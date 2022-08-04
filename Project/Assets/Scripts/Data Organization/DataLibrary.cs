using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class DataLibrary : MonoBehaviour
{
    #region Statics
    static DataLibrary instance;
    public static DataLibrary I
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance) { return; }
            else
            {
                instance = value;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Already A DataLibrary in scene. Deleting this one!");
            Destroy(gameObject);
            return;
        }

        Setup();
    }

    #endregion

    public Getter weapons { get; private set; }

    void Setup()
    {
        weapons = new Getter(Resources.LoadAll<ItemData>("Guns"));
    }

    #region Old Method
    //[SerializeField] T[] data;

    //Dictionary<string, DataPrefab> getData;

    //private void OnValidate()
    //{
    //    getData = new Dictionary<string, DataPrefab>();

    //    for (int i = 0; i < data.Length; i++)
    //    {
    //        if (!data[i]) { print("Null Data!"); continue; }
    //        getData.Add(data[i].name, data[i]);
    //    }
    //}

    //public DataPrefab GetData(string n)
    //{
    //    getData.TryGetValue(n, out DataPrefab d);
    //    if (d == null) { Debug.LogError($"couldn't get {n}"); }
    //    return d;
    //} 
    #endregion
}
