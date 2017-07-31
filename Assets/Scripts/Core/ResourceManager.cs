using UnityEngine;
using System.Collections;

public class ResourceManager:Singleton<ResourceManager>
{
    /// <summary>
    /// 缓存用哈希表
    /// </summary>
    private Hashtable _hashTable;
    private ResourceManager()
    {
        _hashTable = new Hashtable();
    }
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="isCache">资源是否缓存</param>
    public T Load<T>(string path,bool isCache) where T: Object
    {
        if(_hashTable.Contains(path))
        {
            return (T)_hashTable[path];
        }
        T t = Resources.Load<T>(path);
        if(t==null)
        {
            Debug.LogError("加载资源失败！path=" + path);
            return null;
        }
        if(isCache)
        {
            _hashTable.Add(path, t);
        }
        return t;
        
    }
    /// <summary>
    /// 创建对象
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="isCache">是否缓存</param>
    public GameObject CreateGameObject(string path,bool isCache)
    {
        GameObject obj = Load<GameObject>(path, isCache);
        if(obj==null)
        {
            return null;
        }
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
        return obj;
          
    }
}
