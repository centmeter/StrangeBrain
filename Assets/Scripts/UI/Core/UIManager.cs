using UnityEngine;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    private List<UIBase> _uiList;
    private GameObject _canvas;
    private const string UI_PATH_ROOT = "UIPrefabs/";
    private UIManager()
    {
        _uiList = new List<UIBase>();
    }
    public T UIEnter<T>(bool isCache) where T : UIBase
    {
        if (_canvas == null)
            _canvas = GameObject.Find("Canvas");
        GameObject obj = ResourceManager.Instance.CreateGameObject(UI_PATH_ROOT + typeof(T).ToString(), isCache);
        if(obj==null)
        {
            Debug.LogError("加载UI界面失败！Type:" + typeof(T).ToString());
            return null;
        }
        obj.transform.SetParent(_canvas.transform);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        T t = obj.AddComponent<T>();
        t.InitNode(t.transform);
        t.Init();
        _uiList.Add(t);
        return t;
    }
    public void UIExit(UIBase ui)
    {
        if(ui==null)
        {
            Debug.LogError("需要退出的UI不存在！");
            return;
        }
        if(!_uiList.Contains(ui))
        {
            Debug.LogError("需要退出的UI不在UI列表中！UI:" + ui.ToString());
            return;
        }
        _uiList.Remove(ui);
        DestroyImmediate(ui.gameObject);
    }
}
