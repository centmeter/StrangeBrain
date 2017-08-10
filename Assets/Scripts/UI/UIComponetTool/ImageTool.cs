using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 图片相关工具
/// </summary>
public class ImageTool
{
    /// <summary>
    /// 设置图片透明度
    /// </summary>
    /// <param name="image">图片对象</param>
    /// <param name="alpha">透明度值</param>
    /// <returns></returns>返回是否成功改变透明度
    public static bool SetImageAlpha(Image image, float alpha)
    {
        if (image == null)
            return false;
        Color color = image.color;
        if (color.a == alpha)
            return false;
        image.color = new Color(color.r, color.g, color.b, alpha);
        return true;
    }
    /// <summary>
    /// 设置图片数组颜色
    /// </summary>
    /// <param name="images">图片数组</param>
    /// <param name="color">设置的单色</param>
    /// <param name="colors">设置的多色</param>
    /// <param name="imagesToColors">图片数组与多色相对应</param>
    /// <returns></returns>返回是否成功改变图片数组的颜色
    public static bool SetImagesColor(Image[] images,Color color,Color[] colors=null,int[] imagesToColors=null)
    {
        if (colors == null && imagesToColors == null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = color;
            }
            return true;
        }
        if (colors != null && imagesToColors == null)
        {
            if (images.Length != colors.Length)
                return false;
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = colors[i];
            }
        }
        if (colors != null && imagesToColors != null)
        {
            if (images.Length != imagesToColors.Length)
                return false;
            for (int i = 0; i < images.Length; i++)
            {
                if (imagesToColors[i] > colors.Length - 1)
                    return false;
                images[i].color = colors[imagesToColors[i]];
            }
            return true;
        }
        return false;
    }
}
