using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
public class UILevel002 : UIBase
{
    /// <summary>
    /// 标题文本
    /// </summary>
    private Text _titleText;
    private const string TITLE_TEXT = "txtTitle";
    /// <summary>
    /// 内容文本
    /// </summary>
    private Text _contentText;
    private const string CONTENT_TEXT = "txtContent";
    /// <summary>
    /// 触发区域
    /// </summary>
    private GameObject _touchObj;
    private const string TOUCH_OBJ = "objTouch";
    /// <summary>
    /// 下一关按钮
    /// </summary>
    private Button _nextButton;
    private const string NEXT_BUTTON = "btnNext";
    /// <summary>
    /// 产生折线按钮
    /// </summary>
    private Button _lineButton;
    private const string LINE_BUTTON = "btnLine";
    public override void RegisterNode(string name, GameObject obj)
    {
        switch (name)
        {
            case TITLE_TEXT:
                _titleText = obj.GetComponent<Text>();
                break;
            case CONTENT_TEXT:
                _contentText = obj.GetComponent<Text>();
                break;
            case TOUCH_OBJ:
                _touchObj = obj;
                break;
            case NEXT_BUTTON:
                _nextButton = obj.GetComponent<Button>();
                break;
            case LINE_BUTTON:
                _lineButton = obj.GetComponent<Button>();
                break;
            default:
                break;
        }
    }
    public override void Init()
    {
        base.Init();
        InitNextButton();
        DragItem item = AddDragItem(_titleText.gameObject,null,null, OnTitleTextDragEnd);
    }
    public override void OnButtonClick(string name)
    {
        switch (name)
        {
            case NEXT_BUTTON:
                UIManager.Instance.UIEnter<UILevel003>(false, UIEnterStyle.FromTopToBottom);
                UIManager.Instance.UIExit(this, UIExitStyle.ToBottom);
                break;
            case LINE_BUTTON:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 添加可拖拽组件
    /// </summary>
    private DragItem AddDragItem(GameObject obj,UnityAction onBeginDrag=null,UnityAction onDrag=null,UnityAction onEndDrag=null)
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
    /// <summary>
    /// 比较标题与触发区的相对位置
    /// </summary>
    /// <returns></returns>
    private bool JudgeItemTextPos()
    {
        RectTransform titleTxtRect = _titleText.GetComponent<RectTransform>();
        RectTransform touchObjRect = _touchObj.GetComponent<RectTransform>();
        if (titleTxtRect.anchoredPosition.x >= touchObjRect.anchoredPosition.x && titleTxtRect.anchoredPosition.y <= touchObjRect.anchoredPosition.y)
            return true;
        return false;
    }
    /// <summary>
    /// 标题拖拽结束事件
    /// </summary>
    private void OnTitleTextDragEnd()
    {
        if (JudgeItemTextPos())
        {
            _titleText.GetComponent<DragItem>().enabled=false;
            ShowNextButtonByMove();
        }
    }
    /// <summary>
    /// 移动式显示NEXT按钮
    /// </summary>
    private void ShowNextButtonByMove()
    {
        _nextButton.gameObject.SetActive(true);
        RectTransform rect = _nextButton.GetComponent<RectTransform>();
        float pathY = -2 * rect.anchoredPosition.y;
        float endY = -rect.anchoredPosition.y;
        Tweener tweener = rect.DOAnchorPosY(pathY, 0.5f);
        tweener.OnComplete(() =>
        {
            rect.DOAnchorPosY(endY, 0.2f).onComplete=()=> { _nextButton.enabled = true; };
        });
    }
    /// <summary>
    /// 初始化下一关按钮
    /// </summary>
    private void InitNextButton()
    {
        RectTransform rect = _nextButton.GetComponent<RectTransform>();
        float y = -rect.anchoredPosition.y;
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
        _nextButton.gameObject.SetActive(false);
        _nextButton.enabled = false;
    }
}
