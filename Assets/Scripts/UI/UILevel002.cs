using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
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

    #region 与拖拽折线图相关变量
    private RectTransform _lineButtonRect;
    private RectTransform _contentTextRect;
    /// <summary>
    /// 折线图在内容文本外面折线的颜色
    /// </summary>
    private Color _outColor = Color.black;
    /// <summary>
    /// 折线图在内容文本里面折线的颜色
    /// </summary>
    private Color _inColor = Color.yellow;
    /// <summary>
    /// 折线图是否在内容文本内
    /// </summary>
    private bool _isIn=false;
    #endregion
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
        _lineButtonRect = _lineButton.GetComponent<RectTransform>();
        _contentTextRect = _contentText.GetComponent<RectTransform>();
        UIManager.Instance.AddDragItem(_lineButton.gameObject, null, OnLineButtonDrag, OnLineButtonDragEnd).enabled = false ;
        UIManager.Instance.AddDragItem(_titleText.gameObject, null, null, OnTitleTextDragEnd).enabled = false;
        InitNextButton();
        _lineButton.enabled = false;
        HideLines();
        StartCoroutine(PlayContentTextAnim());

        
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
                TweenCallback callback = () =>
                  {
                      _lineButton.GetComponent<DragItem>().enabled = true;
                  };
                _lineButton.enabled = false;
                Image[] images = GetImageArr();
                Image image = _lineButton.GetComponent<Image>();
                image.DOFade(0.05f, 1.5f).onComplete = () => { AllLineDoFade(images, images.Length, 1, callback); };
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 内容开场出现动画效果
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayContentTextAnim()
    {
        string str = _contentText.text;
        _contentText.text = string.Empty;
        string[] strArr = str.Split('\n');
        Text[] txtArr = new Text[strArr.Length];
        for (int i = 0; i < txtArr.Length; i++)
        {
            txtArr[i] = _contentText.transform.GetChild(i).GetComponent<Text>();
            txtArr[i].text = strArr[i];
            Color color = txtArr[i].color;
            txtArr[i].color = new Color(color.r, color.g, color.b, 0);
        }
        yield return new WaitForSeconds(1.5f);
        TweenCallback callback = () =>
          {
              _lineButton.enabled = true;
              
          };
        AllTextDoFade(txtArr, txtArr.Length, 2, callback);

        
    }
    /// <summary>
    /// 内容文档接连渐现
    /// </summary>
    private void AllTextDoFade(Text[] textArr,int index,float time=2, TweenCallback allCompleteCallback=null)
    {
        if (textArr.Length == 0 || textArr == null)
            return;
        int i = textArr.Length - index;
        index--;
        Tweener tweener = textArr[i].DOFade(1, time);
        if (index > 0)
            tweener.onComplete = () => { AllTextDoFade(textArr, index, time,allCompleteCallback); };
        else if (index == 0 && allCompleteCallback == null)
            return;
        else if (index == 0 && allCompleteCallback != null)
            tweener.onComplete = allCompleteCallback;

    }
    /// <summary>
    /// 隐藏折线
    /// </summary>
    private void HideLines()
    {
        Image[] images = GetImageArr();
        for (int i = 0; i < images.Length; i++)
        {
            Color color = images[i].color;
            images[i].color = new Color(color.r, color.g, color.b, 0);
        }
    }
    /// <summary>
    /// 得到折线数组
    /// </summary>
    private Image[] GetImageArr()
    {
        Image[] images = new Image[_lineButton.transform.childCount];
        for (int i = 0; i < _lineButton.transform.childCount; i++)
        {
            images[i] = _lineButton.transform.GetChild(i).GetComponent<Image>();
        }
        return images;
    }
    /// <summary>
    /// 折线接连渐现
    /// </summary>
    private void AllLineDoFade(Image[] imageArr,int index,float time=1,TweenCallback allCompleteCallback=null)
    {
        if (imageArr.Length == 0 || imageArr == null)
            return;
        int i = imageArr.Length - index;
        index--;
        Tweener tweener = imageArr[i].DOFade(1, time);
        if (index > 0)
            tweener.onComplete = () => { AllLineDoFade(imageArr, index, time, allCompleteCallback); };
        else if (index == 0 && allCompleteCallback == null)
            return;
        else if (index == 0 && allCompleteCallback != null)
            tweener.onComplete = allCompleteCallback;
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
            _titleText.GetComponent<DragItem>().enabled = false;
            ShowNextButtonByMove();
        }
    }
    /// <summary>
    /// 折线图拖拽
    /// </summary>
    private void OnLineButtonDrag()
    {
        if(JudgeLineInContent()&&!_isIn)
        {
            _isIn = true;
            Image[] imageArr = GetImageArr();
            for (int i = 0; i < imageArr.Length; i++)
            {
                imageArr[i].color = _inColor;
            }
        }
        else if(!JudgeLineInContent()&&_isIn)
        {
            _isIn = false;
            Image[] imageArr = GetImageArr();
            for (int i = 0; i < imageArr.Length; i++)
            {
                imageArr[i].color = _outColor;
            }
        }
    }
    /// <summary>
    /// 判定折线图是否在内容文本里面
    /// </summary>
    /// <returns></returns>
    private bool JudgeLineInContent()
    {
        float deltaX = (_contentTextRect.rect.width - _lineButtonRect.rect.width) * 0.5f;
        float deltaY= (_contentTextRect.rect.height - _lineButtonRect.rect.height) * 0.5f;
        bool x = _lineButtonRect.anchoredPosition.x <= _contentTextRect.anchoredPosition.x + deltaX && _lineButtonRect.anchoredPosition.x >= _contentTextRect.anchoredPosition.x - deltaX;
        bool y = _lineButtonRect.anchoredPosition.y <= _contentTextRect.anchoredPosition.y + deltaY && _lineButtonRect.anchoredPosition.y >= _contentTextRect.anchoredPosition.y - deltaY;
        if (x && y)
            return true;
        return false;
    }
    /// <summary>
    /// 折线图拖拽结束事件
    /// </summary>
    private void OnLineButtonDragEnd()
    {
        if(_isIn)
        {
            _lineButton.GetComponent<DragItem>().enabled = false;
            _lineButtonRect.anchoredPosition = _contentTextRect.anchoredPosition;
            _titleText.GetComponent<DragItem>().enabled = true;
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
