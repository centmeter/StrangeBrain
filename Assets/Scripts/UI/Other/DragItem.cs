using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DragItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    /// <summary>
    /// RectTransform组件
    /// </summary>
    private RectTransform _rectTrans;
    /// <summary>
    /// Canvas的RectTransform组件
    /// </summary>
    private RectTransform _canvasRect;
    /// <summary>
    /// Canvas在X方向上的最大值（边界）
    /// </summary>
    private float _canvasMaxX;
    /// <summary>
    /// Canvas在Y方向上的最大值（边界）
    /// </summary>
    private float _canvasMaxY;
    /// <summary>
    /// Canvas在X方向上的最小值（边界）
    /// </summary>
    private float _canvasMinX;
    /// <summary>
    /// Canvas在Y方向上的最小值（边界）
    /// </summary>
    private float _canvasMinY;
    /// <summary>
    /// Item位置的X坐标最大允许值
    /// </summary>
    private float _itemMaxX;
    /// <summary>
    /// Item位置的Y坐标最大允许值
    /// </summary>
    private float _itemMaxY;
    /// <summary>
    /// Item位置的X坐标最小允许值
    /// </summary>
    private float _itemMinX;
    /// <summary>
    /// Item位置的Y坐标最小允许值
    /// </summary>
    private float _itemMinY;
    /// <summary>
    /// 鼠标点击点与Item中心点的偏移
    /// </summary>
    private Vector2 _offset;

    public UnityEvent onBeginDrag;
    public UnityEvent onDrag;
    public UnityEvent onEndDrag;
    private void Awake()
    {
        _rectTrans = this.GetComponent<RectTransform>();
        _canvasRect = UIManager.Instance.Canvas.GetComponent<RectTransform>();
        _canvasMaxX = _canvasRect.rect.width * 0.5f;
        _canvasMaxY = _canvasRect.rect.height * 0.5f;
        _canvasMinX = -_canvasRect.rect.width * 0.5f;
        _canvasMinY = -_canvasRect.rect.height * 0.5f;

        onBeginDrag = new UnityEvent();
        onDrag = new UnityEvent();
        onEndDrag = new UnityEvent();
    }
    /// <summary>
    /// 开始拖拽回调
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //在开始拖拽的时候赋予Item极限值
        _itemMaxX = _canvasMaxX - _rectTrans.rect.width * 0.5f;
        _itemMaxY = _canvasMaxY - _rectTrans.rect.height * 0.5f;
        _itemMinX = _canvasMinX + _rectTrans.rect.width * 0.5f;
        _itemMinY = _canvasMinY + _rectTrans.rect.height * 0.5f;

        Vector2 mousePos = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, eventData.position, eventData.enterEventCamera, out mousePos))
        {
            _offset = _rectTrans.anchoredPosition - mousePos;
        }
        if (onBeginDrag != null)
            onBeginDrag.Invoke();
    }
    /// <summary>
    /// 拖拽过程中调用
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, eventData.position, eventData.enterEventCamera, out mousePos))
        {
            _rectTrans.anchoredPosition = CorrectPos(mousePos + _offset);
        }
        if (onDrag != null)
            onDrag.Invoke();
    }
    /// <summary>
    /// 结束拖拽回调
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        _offset = Vector2.zero;
        if (onEndDrag != null)
            onEndDrag.Invoke();
    }
    /// <summary>
    /// Item若超出边界修正Item坐标位置
    /// </summary>
    private Vector2 CorrectPos(Vector2 originPos)
    {
        Vector2 correctPos = originPos;
        switch (GetOutType(originPos))
        {
            case ItemOutRangeType.NULL:
                break;
            case ItemOutRangeType.LeftOnly:
                correctPos = new Vector2(_itemMinX, originPos.y);
                break;
            case ItemOutRangeType.RightOnly:
                correctPos = new Vector2(_itemMaxX, originPos.y);
                break;
            case ItemOutRangeType.TopOnly:
                correctPos = new Vector2(originPos.x, _itemMaxY);
                break;
            case ItemOutRangeType.BottomOnly:
                correctPos = new Vector2(originPos.x, _itemMinY);
                break;
            case ItemOutRangeType.LeftTop:
                correctPos = new Vector2(_itemMinX, _itemMaxY);
                break;
            case ItemOutRangeType.LeftBottom:
                correctPos = new Vector2(_itemMinX, _itemMinY);
                break;
            case ItemOutRangeType.RightTop:
                correctPos = new Vector2(_itemMaxX, _itemMaxY);
                break;
            case ItemOutRangeType.RightBottom:
                correctPos = new Vector2(_itemMaxX, _itemMinY);
                break;
            default:
                break;
        }
        return correctPos;
    }
    /// <summary>
    /// 根据位置信息判定Item什么方向超出屏幕范围
    /// </summary>
    private ItemOutRangeType GetOutType(Vector2 pos)
    {
        if (pos.x <= _itemMinX && pos.y <= _itemMaxY && pos.y >= _itemMinY)
            return ItemOutRangeType.LeftOnly;
        if (pos.x >= _itemMaxX && pos.y <= _itemMaxY && pos.y >= _itemMinY)
            return ItemOutRangeType.RightOnly;
        if (pos.y >= _itemMaxY && pos.x >= _itemMinX && pos.x <= _itemMaxX)
            return ItemOutRangeType.TopOnly;
        if (pos.y <= _itemMinY && pos.x >= _itemMinX && pos.x <= _itemMaxX)
            return ItemOutRangeType.BottomOnly;
        if (pos.x <= _itemMinX && pos.y >= _itemMaxY)
            return ItemOutRangeType.LeftTop;
        if (pos.x <= _itemMinX && pos.y <= _itemMinY)
            return ItemOutRangeType.LeftBottom;
        if (pos.x >= _itemMaxX && pos.y >= _itemMaxY)
            return ItemOutRangeType.RightTop;
        if (pos.x >= _itemMaxX && pos.y <= _itemMinY)
            return ItemOutRangeType.RightBottom;
        return ItemOutRangeType.NULL;
    }
    /// <summary>
    /// 添加可拖拽组件
    /// </summary>
    public static DragItem AddDragItem(GameObject obj, UnityAction onBeginDrag = null, UnityAction onDrag = null, UnityAction onEndDrag = null)
    {
        DragItem dragItem = obj.AddComponent<DragItem>();
        if (onBeginDrag != null)
            dragItem.onBeginDrag.AddListener(onBeginDrag);
        if (onDrag != null)
            dragItem.onDrag.AddListener(onDrag);
        if (onEndDrag != null)
            dragItem.onEndDrag.AddListener(onEndDrag);
        return dragItem;
    }
}
/// <summary>
/// Item超出边界类型
/// </summary>
public enum ItemOutRangeType
{
    NULL,            //没有超出边界
    LeftOnly,        //只有左边
    RightOnly,       //只有右边
    TopOnly,         //只有上面
    BottomOnly,      //只有下面
    LeftTop,         //左上方
    LeftBottom,      //左下方
    RightTop,        //右上方
    RightBottom      //右下方
}