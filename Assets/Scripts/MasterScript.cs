using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    private static MasterScript instance;
    public static MasterScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MasterScript>();
                if (instance == null)
                {
                    GameObject go = new GameObject("LogicManager");
                    instance = go.AddComponent<MasterScript>();
                }
            }
            return instance;
        }
    }


    void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
