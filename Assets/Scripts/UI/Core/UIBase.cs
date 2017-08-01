using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// 初始化完成标识
    /// </summary>
    private bool _initDone = false;
    /// <summary>
    /// 是否需要缓存(默认不需要)
    /// </summary>
    public bool IsCache { get; protected set; }
    /// <summary>
    /// 遍历节点
    /// </summary>
    public void InitNode(Transform tf)
    {
        if (tf != null)
            RegisterNode(tf.name,tf.gameObject);
        Button btn = tf.GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(() =>
            {
                OnButtonClick(tf.name);
            });
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
    /// 按钮点击事件
    /// </summary>
    public abstract void OnButtonClick(string name);
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        IsCache = false;
    }
}
