using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class UILevel001 : UIBase
{
    /// <summary>
    /// 欢迎文本
    /// </summary>
    private Text _welcomeText;
    private const string WELCOME_TEXT = "txtWelcome";
    /// <summary>
    /// 确定输入框内容按钮
    /// </summary>
    private Button _confirmButton;
    private const string CONFIRM_BUTTON = "btnConfirm";
    /// <summary>
    /// 下一关按钮
    /// </summary>
    private Button _nextButton;
    private const string NEXT_BUTTON = "btnNext";
    /// <summary>
    /// 名字输入框
    /// </summary>
    private InputField _nameInput;
    private const string NAME_INPUT = "inputName";
    /// <summary>
    /// 提示区文本
    /// </summary>
    private Text _promptText;
    private const string PROMPT_TEXT = "txtPrompt";
    /// <summary>
    /// 触碰触发区域
    /// </summary>
    private Button _touchButton;
    private const string TOUCH_BUTTON = "btnTouch";
    /// <summary>
    /// 提示区渐隐时间
    /// </summary>
    private float _promptFadeoutTime;
    /// <summary>
    /// 输入框提示语
    /// </summary>
    private string _placeHolderStr;
    /// <summary>
    /// 错误提示语
    /// </summary>
    private string _wrongPromptStr;
    /// <summary>
    /// 正确提示语
    /// </summary>
    private string _rightPromptStr;
    public override void RegisterNode(string name, GameObject obj)
    {
        switch (name)
        {
            case WELCOME_TEXT:
                _welcomeText = obj.GetComponent<Text>();
                break;
            case TOUCH_BUTTON:
                _touchButton = obj.GetComponent<Button>();
                _touchButton.onClick.AddListener(OnTouchButtonClick);
                break;
            case PROMPT_TEXT:
                _promptText = obj.GetComponent<Text>();
                break;
            case CONFIRM_BUTTON:
                _confirmButton = obj.GetComponent<Button>();
                _confirmButton.onClick.AddListener(OnConfirmButtonClick);
                break;
            case NAME_INPUT:
                _nameInput = obj.GetComponent<InputField>();
                break;
            case NEXT_BUTTON:
                _nextButton = obj.GetComponent<Button>();
                _nextButton.onClick.AddListener(OnNextButtonClick);
                break;
            default:
                break;
        }
    }
    public override void Init()
    {
        base.Init();

        InitPromptData();
        InitButtonData();
        InitInputData();

        PlayContentTextAnim();
    }

    #region 输入提示模块相关
    /// <summary>
    /// 初始化输入模块
    /// </summary>
    private void InitInputData()
    {
        SetInputInteractable(false);
    }
    /// <summary>
    /// 初始化各提示区提示语
    /// </summary>
    private void InitPromptData()
    {
        _placeHolderStr = "请输入你的名字";
        _wrongPromptStr = "NONONO 我要你的名字";
        _rightPromptStr = "HOHOHO 第二关近在嘴边了";
        _promptFadeoutTime = 3;
    }
    /// <summary>
    /// 设置输入模块是否可操控
    /// </summary>
    /// <param name="enabled"></param>
    private void SetInputInteractable(bool interactable)
    {
        _nameInput.interactable = interactable;
        _confirmButton.interactable = interactable;
    }
    /// <summary>
    /// 判定输入是否正确
    /// </summary>
    private bool JudgeInput(string text)
    {
        if ("你的名字".Equals(text.Trim()))
            return true;
        return false;
    } 
    /// <summary>
    /// 输入错误内容回调
    /// </summary>
    private void OnInputWrong()
    {
        TextTool.RefreshText(_promptText, _wrongPromptStr);
        Tweener tweener = _promptText.DOFade(0, _promptFadeoutTime);
        tweener.OnComplete(() =>
        {
            TextTool.ClearText(_promptText);
            TextTool.SetTextAlpha(_promptText, 1);
            _nameInput.text = string.Empty;
            SetInputInteractable(true);
        });
    }
    /// <summary>
    /// 输入正确内容回调
    /// </summary>
    private void OnInputRight()
    {
        TextTool.RefreshText(_promptText,_rightPromptStr);
        TextTool.SetTextAlpha(_promptText, 0);
        _promptText.DOFade(1, 2f).onComplete = () => { _touchButton.enabled=true; };

    }
    #endregion

    #region 按钮状态相关
    /// <summary>
    /// 初始化各按钮
    /// </summary>
    private void InitButtonData()
    {
        InitNextButtonData();
        _touchButton.enabled = false;
    }
    /// <summary>
    /// 初始化下一关按钮
    /// </summary>
    private void InitNextButtonData()
    {
        _nextButton.gameObject.SetActive(false);
        RectTransform rect = _nextButton.GetComponent<RectTransform>();
        float x = -rect.anchoredPosition.x;
        rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
        _nextButton.enabled = false;
    }
    /// <summary>
    /// 移动式显示下一关按钮
    /// </summary>
    private void ShowNextButtonByMove()
    {
        _nextButton.gameObject.SetActive(true);
        RectTransform rect = _nextButton.GetComponent<RectTransform>();
        float pathX = -2 * rect.anchoredPosition.x;
        float endX = -rect.anchoredPosition.x;
        Tweener tweener = rect.DOAnchorPosX(pathX, 0.5f);
        tweener.OnComplete(() =>
        {
            rect.DOAnchorPosX(endX, 0.2f).onComplete = () => { _nextButton.enabled = true; };
        });
    }
    #endregion

    #region 点击事件相关
    private void OnTouchButtonClick()
    {
        _touchButton.gameObject.SetActive(false);
        ShowNextButtonByMove();
    }
    private void OnConfirmButtonClick()
    {
        string input = _nameInput.text;
        SetInputInteractable(false);
        if (JudgeInput(input))
            OnInputRight();
        else
            OnInputWrong();
    }
    private void OnNextButtonClick()
    {
        _nextButton.interactable = false;
        UIManager.Instance.UIEnter<UILevel002>(false, UIEnterStyle.FromLeftToRight);
        UIManager.Instance.UIExit(this, UIExitStyle.ToRight);
    }
    #endregion

    #region 其他

    /// <summary>
    /// 开场动画
    /// </summary>
    /// <returns></returns>
    private void PlayContentTextAnim()
    {
        string welcomeStr = _welcomeText.text;
        TextTool.ClearText(_welcomeText);
        TweenCallback callback = () =>
        {
            TypeWriter(_nameInput.placeholder.GetComponent<Text>(), _placeHolderStr, 2.5f, () =>
            {
                SetInputInteractable(true);
            }, ScrambleMode.None);
        };
        TypeWriter(_welcomeText, welcomeStr, 6.5f, callback, ScrambleMode.Numerals);
    }

    /// <summary>
    /// 打字机
    /// </summary>
    private void TypeWriter(Text text, string content, float duration, TweenCallback callback, ScrambleMode mode)
    {
        Tweener tweener = text.DOText(content, duration, true, mode);
        tweener.OnComplete(callback);
    }
    #endregion
}
