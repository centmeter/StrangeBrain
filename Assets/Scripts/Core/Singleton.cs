using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance==null)
                _instance=new GameObject("(Singleton)"+typeof(T).ToString()).AddComponent<T>();
            return _instance;
        }
    }
}
