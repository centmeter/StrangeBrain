using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
    private float _textFadeoutTime = 3f;
    /// <summary>
    /// 输入框提示语
    /// </summary>
    private string _placeHolder = "请输入 你的名字";
    public override void RegisterNode(string name, GameObject obj)
    {
        switch (name)
        {
            case WELCOME_TEXT:
                _welcomeText = obj.GetComponent<Text>();
                break;
            case TOUCH_BUTTON:
                _touchButton = obj.GetComponent<Button>();
                break;
            case PROMPT_TEXT:
                _promptText = obj.GetComponent<Text>();
                break;
            case CONFIRM_BUTTON:
                _confirmButton = obj.GetComponent<Button>();
                break;
            case NAME_INPUT:
                _nameInput = obj.GetComponent<InputField>();
                break;
            case NEXT_BUTTON:
                _nextButton = obj.GetComponent<Button>();
                break;
            default:
                break;
        }
    }
    public override void Init()
    {
        base.Init();
        InitNextButton();
        _touchButton.gameObject.SetActive(false);
        SetInputInteractable(false);
        TweenCallback callback = () =>
          {
              TypeWriter(_nameInput.placeholder.GetComponent<Text>(), _placeHolder, 2.5f, () =>
                 {
                     SetInputInteractable(true);
                 }, ScrambleMode.None);
          };
        string welcomeStr = _welcomeText.text;
        _welcomeText.text = string.Empty;
        TypeWriter(_welcomeText, welcomeStr, 6.5f, callback, ScrambleMode.Numerals);
    }
    public override void OnButtonClick(string name)
    {
        switch (name)
        {
            case CONFIRM_BUTTON:
                string input = _nameInput.text;
                SetInputInteractable(false);
                if (JudgeInput(input))
                    OnInputRight();
                else
                    OnInputWrong();
                break;
            case NEXT_BUTTON:
                _nextButton.interactable = false;
                UIManager.Instance.UIEnter<UILevel002>(false, UIEnterStyle.FromLeftToRight);
                UIManager.Instance.UIExit(this, UIExitStyle.ToRight);
                break;
            case TOUCH_BUTTON:
                _touchButton.gameObject.SetActive(false);
                ShowNextButtonByMove();
                break;
            default:
                break;
        }
    }
    private void TypeWriter(Text text, string content, float duration, TweenCallback callback, ScrambleMode mode)
    {
        Tweener tweener = text.DOText(content, duration, true, mode);
        tweener.OnComplete(callback);
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
    /// 输入错误事件
    /// </summary>
    private void OnInputWrong()
    {
        _promptText.text = "NONONO 我要 你的名字";
        Color tempColor = _promptText.color;
        Tweener tweener = _promptText.DOFade(0, _textFadeoutTime);
        tweener.OnComplete(() =>
        {
            _promptText.text = string.Empty;
            _promptText.color = tempColor;
            _nameInput.text = string.Empty;
            SetInputInteractable(true);
        });
    }
    /// <summary>
    /// 输入正确事件
    /// </summary>
    private void OnInputRight()
    {
        _promptText.text = "HOHOHO 第二关 近在嘴边了";
        _touchButton.gameObject.SetActive(true);
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
    /// 移动式显示NEXT按钮
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
            rect.DOAnchorPosX(endX, 0.2f).onComplete=()=> { _nextButton.enabled = true; };
        });
    }
    /// <summary>
    /// 初始化下一关按钮
    /// </summary>
    private void InitNextButton()
    {
        _nextButton.gameObject.SetActive(false);
        RectTransform rect = _nextButton.GetComponent<RectTransform>();
        float x = -rect.anchoredPosition.x;
        rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
        _nextButton.enabled = false;
    }
}
