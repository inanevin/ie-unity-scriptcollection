using UnityEngine;
using System.Collections;
using System;

public abstract class AutoSingletonManager : MonoBehaviour { }

public abstract class AutoSingletonManager<T> : AutoSingletonManager where T : AutoSingletonManager
{
    private static bool Compare<T>(T x, T y) where T : class
    {
        return x == y;
    }

    #region Singleton

    private static T _instance = default(T);

    public static T Instance
    {
        get
        {
            if (!Compare<T>(default(T), _instance)) return _instance;

            InitInstance(true);
            return _instance;
        }
    }

    #endregion

    public void Awake()
    {
        InitInstance(false);
    }

    public static void InitInstance(bool shouldInitManager)
    {
        Type thisType = typeof(T);

        _instance = FindObjectOfType<T>();

        if (Compare<T>(default(T), _instance))
        {
            _instance = new GameObject(thisType.Name).AddComponent<T>();
        }

        //Won't call InitManager from Awake
        if (shouldInitManager)
        {
            (_instance as AutoSingletonManager<T>).InitManager();
        }
    }

    public virtual void InitManager() { }
}