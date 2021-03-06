﻿using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    private List<UIBase> _uiList;
    private GameObject _canvas;
    public GameObject Canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = GameObject.Find("Canvas");
            return _canvas;
        }
    }
    private const string UI_PATH_ROOT = "UIPrefabs/";
    private UIManager()
    {
        _uiList = new List<UIBase>();
    }
    public T UIEnter<T>(UIEnterStyle style, bool isCache=false, bool hasKeys=false, params object[] keys) where T : UIBase
    {
        GameObject obj = ResourceManager.Instance.CreateGameObject(UI_PATH_ROOT + typeof(T).ToString(), isCache);
        if (obj == null)
        {
            Debug.LogWarning("加载UI界面失败！Type:" + typeof(T).ToString());
            return null;
        }
        obj.transform.SetParent(Canvas.transform);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        OnUIEnterStyle(rect, style);
        T t = obj.AddComponent<T>();
        t.InitNode(t.transform);
        t.Init(hasKeys, keys);
        _uiList.Add(t);
        return t;
    }
    public void UIExit(UIBase ui,UIExitStyle style)
    {
        if (ui == null)
        {
            Debug.LogWarning("需要退出的UI不存在！");
            return;
        }
        if (!_uiList.Contains(ui))
        {
            Debug.LogWarning("需要退出的UI不在UI列表中！UI:" + ui.ToString());
            return;
        }
        _uiList.Remove(ui);
        OnUIExitStyle(ui.GetComponent<RectTransform>(), style,()=> { Destroy(ui.gameObject); });
        
    }
    /// <summary>
    /// 加载UI的方式
    /// </summary>
    private void OnUIEnterStyle(RectTransform rect, UIEnterStyle style, TweenCallback onComplete = null,float time=1)
    {
        Vector2 endPos = rect.anchoredPosition;
        Vector2 startPos = Vector2.zero;
        switch (style)
        {
            case UIEnterStyle.Null:
                return;
            case UIEnterStyle.FromRightToLeft:
                float startX_frl = endPos.x + rect.rect.width;
                startPos = new Vector2(startX_frl, endPos.y);
                break;
            case UIEnterStyle.FromLeftToRight:
                float startX_flr = endPos.x - rect.rect.width;
                startPos = new Vector2(startX_flr, endPos.y);
                break;
            case UIEnterStyle.FromTopToBottom:
                float startY_ftb = endPos.y + rect.rect.height;
                startPos = new Vector2(endPos.x, startY_ftb);
                break;
            case UIEnterStyle.FromBottomToTop:
                float startY_fbt = endPos.y - rect.rect.height;
                startPos = new Vector2(endPos.x, startY_fbt);
                break;
            default:
                break;
        }
        rect.anchoredPosition = startPos;
        Tweener tweener = rect.DOAnchorPos(endPos, time);
        if (onComplete != null)
            tweener.OnComplete(onComplete);
    }
    /// <summary>
    /// 移除UI的方式
    /// </summary>
    /// <param name="style"></param>
    private void OnUIExitStyle(RectTransform rect, UIExitStyle style, TweenCallback onComplete = null, float time=1)
    {
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = Vector2.zero;
        switch (style)
        {
            case UIExitStyle.Null:
                return;
            case UIExitStyle.ToLeft:
                float endX_l = startPos.x - rect.rect.width;
                endPos = new Vector2(endX_l, startPos.y);
                break;
            case UIExitStyle.ToRight:
                float endX_r = startPos.x + rect.rect.width;
                endPos = new Vector2(endX_r, startPos.y);
                break;
            case UIExitStyle.ToTop:
                float endY_t = startPos.y + rect.rect.height;
                endPos = new Vector2(startPos.x, endY_t);
                break;
            case UIExitStyle.ToBottom:
                float endY_b = startPos.y - rect.rect.height;
                endPos = new Vector2(startPos.x, endY_b);
                break;
            default:
                break;
        }
        rect.anchoredPosition = startPos;
        Tweener tweener = rect.DOAnchorPos(endPos, time);
        if (onComplete != null)
            tweener.OnComplete(onComplete);
    }
    
}
public enum UIEnterStyle
{
    Null,             //无
    FromRightToLeft,  //从右边渐入
    FromLeftToRight,  //从左边渐入
    FromTopToBottom,  //从上边渐入
    FromBottomToTop,  //从下边渐入

}
public enum UIExitStyle
{
    Null,    //无
    ToLeft,  //移向左边
    ToRight, //移向右边
    ToTop,   //移向上边
    ToBottom,//移向下边
}