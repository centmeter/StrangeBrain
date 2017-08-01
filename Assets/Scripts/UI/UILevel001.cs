using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UILevel001 : UIBase
{
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
    private const float TEXT_FADEOUT_TIME = 3f;
    public override void RegisterNode(string name, GameObject obj)
    {
        switch (name)
        {
            case TOUCH_BUTTON:
                _touchButton = obj.GetComponent<Button>();
                break;
            case PROMPT_TEXT:
                _promptText = obj.GetComponent<Text>();
                break;
            case CONFIRM_BUTTON:
                _confirmButton = obj.GetComponent<Button>();
                break;
            case NAME_INPUT:_nameInput = obj.GetComponent<InputField>();
                break;
            case NEXT_BUTTON:_nextButton = obj.GetComponent<Button>();
                break;
            default:
                break;
        }
    }
    public override void Init()
    {
        base.Init();
        _nextButton.gameObject.SetActive(false);
        _touchButton.gameObject.SetActive(false);
    }
    public override void OnButtonClick(string name)
    {
        switch (name)
        {
            case CONFIRM_BUTTON:
                string input = _nameInput.text;
                _nameInput.interactable = false;
                _confirmButton.interactable = false;
                if (JudgeInput(input))
                    OnInputRight();
                else
                    OnInputWrong();
                break;
            case NEXT_BUTTON:
                UIManager.Instance.UIEnter<UILevel002>(false);
                UIManager.Instance.UIExit(this);
                break;
            case TOUCH_BUTTON:
                _nextButton.gameObject.SetActive(true);
                _touchButton.gameObject.SetActive(false);
                break;
            default:
                break;
        }
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
        Tweener tweener = _promptText.DOFade(0, TEXT_FADEOUT_TIME);
        tweener.OnComplete(() =>
        {
            _promptText.text = string.Empty;
            _promptText.color = tempColor;
            _nameInput.text = string.Empty;
            _nameInput.interactable = true;
            _confirmButton.interactable = true;
        });
    }
    /// <summary>
    /// 输入正确事件
    /// </summary>
    private void OnInputRight()
    {
        _promptText.text = "HOHOHO 第二关 近在嘴边";
        _touchButton.gameObject.SetActive(true);
    }
}
