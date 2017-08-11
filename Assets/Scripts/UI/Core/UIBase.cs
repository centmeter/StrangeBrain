using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// 遍历节点
    /// </summary>
    public void InitNode(Transform tf)
    {
        if (tf != null)
            RegisterNode(tf.name,tf.gameObject);
        for (int i = 0; i < tf.childCount; i++)
        {
            InitNode(tf.GetChild(i));
        }
    }
    /// <summary>
    /// 注册节点
    /// </summary>
    public abstract void RegisterNode(string name, GameObject obj);
    /// <summary>
    /// 初始化
    /// </summary>
    public abstract void Init(bool hasKeys, params object[] keys);
}
