using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
/// 文本相关工具
/// </summary>
public class TextTool
{
    /// <summary>
    /// 设置文本透明度
    /// </summary>
    /// <param name="text">文本对象</param>
    /// <param name="alpha">透明度值</param>
    /// <returns></returns>返回是否改变透明度成功
    public static bool SetTextAlpha(Text text, float alpha)
    {
        if (text == null)
            return false;
        Color color = text.color;
        if (color.a == alpha)
            return false;
        text.color = new Color(color.r, color.g, color.b, alpha);
        return true;
    }
    /// <summary>
    /// 更新文本内容
    /// </summary>
    /// <param name="text">文本对象</param>
    /// <param name="content">更新内容</param>
    /// <returns></returns>返回是否成功更新文本
    public static bool RefreshText(Text text,string content)
    {
        if (text == null || content == null)
            return false;
        if (text.text == content)
            return false;
        text.text = content;
        return true;
    }
    /// <summary>
    /// 清空文本
    /// </summary>
    /// <param name="text">文本对象</param>
    /// <param name="onChangeComplete">文本清空成功回调</param>
    /// <returns></returns>返回是否成功清空文本
    public static bool ClearText(Text text)
    {
        if (text == null)
            return false;
        return RefreshText(text, string.Empty);
    }
}
