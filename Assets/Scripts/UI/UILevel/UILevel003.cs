using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;
public class UILevel003 : UIBase
{
    /// <summary>
    /// 界面主Key
    /// </summary>
    private UILevel003Key _mainKey;
    
    private Button _nextUpButton;
    private const string _nextUpButtonStr = "btnNextUp";
    private Button _nextDownButton;
    private const string _nextDownButtonStr = "btnNextDown";
    private Button _nextLeftButton;
    private const string _nextLeftButtonStr = "btnNextLeft";
    private Button _nextRightButton;
    private const string _nextRightButtonStr = "btnNextRight";

    private Button _touchUpButton;
    private const string _touchUpButtonStr = "btnTouchUp";
    private Button _touchDownButton;
    private const string _touchDownButtonStr = "btnTouchDown";
    private Button _touchLeftButton;
    private const string _touchLeftButtonStr = "btnTouchLeft";
    private Button _touchRightButton;
    private const string _touchRightButtonStr = "btnTouchRight";

    private Button[] _nextButtonArr = new Button[4];
    private Text _promptText;
    private const string _promptTextStr = "txtPrompt";
    private static string _keys = "2468642";
    public override void RegisterNode(string name, GameObject obj)
    {
        switch (name)
        {
            case _promptTextStr:
                _promptText = obj.GetComponent<Text>();
                break;
            case _nextUpButtonStr:
                _nextUpButton = obj.GetComponent<Button>();
                _nextButtonArr[0] = _nextUpButton;
                break;
            case _nextDownButtonStr:
                _nextDownButton = obj.GetComponent<Button>();
                _nextButtonArr[1] = _nextDownButton;
                break;
            case _nextLeftButtonStr:
                _nextLeftButton = obj.GetComponent<Button>();
                _nextButtonArr[2] = _nextLeftButton;
                break;
            case _nextRightButtonStr:
                _nextRightButton = obj.GetComponent<Button>();
                _nextButtonArr[3] = _nextRightButton;
                break;
            case _touchUpButtonStr:
                _touchUpButton = obj.GetComponent<Button>();
                break;
            case _touchDownButtonStr:
                _touchDownButton = obj.GetComponent<Button>();
                break;
            case _touchLeftButtonStr:
                _touchLeftButton = obj.GetComponent<Button>();
                break;
            case _touchRightButtonStr:
                _touchRightButton = obj.GetComponent<Button>();
                break;
        }
    }
    public override void Init(bool hasKeys, params object[] keys)
    {
        if(hasKeys)
        {
            this._mainKey = keys[0] as UILevel003Key;
            InitDataFromKeys(_mainKey);
        }
        InitOnButtonClick();
        InitButtonData();
    }

    #region 按钮状态相关
    /// <summary>
    /// 初始化按钮状态
    /// </summary>
    private void InitButtonData()
    {
        for (int i = 0; i < _nextButtonArr.Length; i++)
        {
            Button btn = _nextButtonArr[i];
            InitButtonPosition(btn, (ButtonDirection)i);
            btn.gameObject.SetActive(false);
            btn.enabled = false;
        }
    }
    /// <summary>
    /// 初始化next按钮位置
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="dir"></param>
    private void InitButtonPosition(Button btn, ButtonDirection dir)
    {
        RectTransform rect = btn.GetComponent<RectTransform>();
        float x = rect.anchoredPosition.x;
        float y = rect.anchoredPosition.y;
        switch (dir)
        {
            case ButtonDirection.Left:
                x = -rect.anchoredPosition.x;
                break;
            case ButtonDirection.Right:
                x = -rect.anchoredPosition.x;
                break;
            case ButtonDirection.Up:
                y = -rect.anchoredPosition.y;
                break;
            case ButtonDirection.Down:
                y = -rect.anchoredPosition.y;
                break;
            default:
                break;
        }
        rect.anchoredPosition = new Vector2(x, y);
    }
    /// <summary>
    /// 移动式显示NEXT按钮
    /// </summary>
    private void ShowNextButtonByMove(ButtonDirection dir)
    {
        Button btn = _nextButtonArr[(int)dir];
        btn.gameObject.SetActive(true);
        RectTransform rect = btn.GetComponent<RectTransform>();
        float pathX = 0;
        float endX = 0;
        float pathY = 0;
        float endY = 0;
        switch (dir)
        {
            case ButtonDirection.Up:
                pathY = -2 * rect.anchoredPosition.y;
                endY = -rect.anchoredPosition.y;
                rect.DOAnchorPosY(pathY, 0.5f).onComplete = () =>
                   {
                       rect.DOAnchorPosY(endY, 0.2f).onComplete = () =>
                        {
                            btn.enabled = true;
                        };
                   };
                break;
            case ButtonDirection.Down:
                pathY = -2 * rect.anchoredPosition.y;
                endY = -rect.anchoredPosition.y;
                rect.DOAnchorPosY(pathY, 0.5f).onComplete = () =>
                {
                    rect.DOAnchorPosY(endY, 0.2f).onComplete = () =>
                    {
                        btn.enabled = true;
                    };
                };
                break;
            case ButtonDirection.Left:
                pathX = -2 * rect.anchoredPosition.x;
                endX = -rect.anchoredPosition.x;
                rect.DOAnchorPosX(pathX, 0.5f).onComplete = () =>
                {
                    rect.DOAnchorPosX(endX, 0.2f).onComplete = () =>
                    {
                        btn.enabled = true;
                    };
                };
                break;
            case ButtonDirection.Right:
                pathX = -2 * rect.anchoredPosition.x;
                endX = -rect.anchoredPosition.x;
                rect.DOAnchorPosX(pathX, 0.5f).onComplete = () =>
                {
                    rect.DOAnchorPosX(endX, 0.2f).onComplete = () =>
                    {
                        btn.enabled = true;
                    };
                };
                break;
            default:
                break;
        }
    }
    #endregion

    #region 点击事件
    /// <summary>
    /// 监听点击事件
    /// </summary>
    private void InitOnButtonClick()
    {
        for (int i = 0; i < _nextButtonArr.Length; i++)
        {
            ButtonDirection dir = (ButtonDirection)i;
            _nextButtonArr[i].onClick.AddListener(()=> { OnNextButtonClick(dir); });
        }
        _touchUpButton.onClick.AddListener(() => { OnTouchButtonClick(ButtonDirection.Up, _touchUpButton); });
        _touchDownButton.onClick.AddListener(() => { OnTouchButtonClick(ButtonDirection.Down, _touchDownButton); });
        _touchLeftButton.onClick.AddListener(() => { OnTouchButtonClick(ButtonDirection.Left, _touchLeftButton); });
        _touchRightButton.onClick.AddListener(() => { OnTouchButtonClick(ButtonDirection.Right, _touchRightButton); });
    }
    private void OnNextButtonClick(ButtonDirection dir)
    {
        _nextButtonArr[(int)dir].interactable = false;
        UILevel003InitKey key = null;
        if(_initKey.initKeyType==UILevel003InitKey.InitKeyType.BeforeMain)
        {
            key = CreateLevel003InitKey(this._initKey, UILevel003InitKey.InitKeyType.Main);
        }
        else if(_initKey.initKeyType==UILevel003InitKey.InitKeyType.Main)
        {
            UILevel003InitKey.ClickButtonType type = _initKey.needToClickButtonList[0];
            if (JudgeButtonClick(dir, type))
            {
                key = CreateLevel003InitKey(this._initKey, UILevel003InitKey.InitKeyType.Right);
            }
            else
                key = CreateLevel003InitKey(this._initKey, UILevel003InitKey.InitKeyType.Wrong);
        }
        if (key == null)
            return;
        if (key.keys == string.Empty)
        {
            UIChangeByButtonDir<UILevel004>(dir);
            return;
        }
        UIChangeByButtonDir<UILevel003>(dir, true, key);
        
        

    }
    private void OnTouchButtonClick(ButtonDirection dir,Button touchBtn)
    {
        ShowNextButtonByMove(dir);
        touchBtn.gameObject.SetActive(false);
    }
    #endregion

    #region InitKey相关
    private void InitDataFromKeys(UILevel003Key key)
    {
        TextTool.RefreshText(_promptText, key.promptStr);
    }
    private static UILevel003Key CreateLevel003InitKey(UILevel003Key fromKey, UILevel003KeyType toType)
    {
        String keys = string.Empty;
        switch (toType)
        {
            case UILevel003KeyType.Main:
                keys = _keys;
                break;
            case UILevel003InitKey.InitKeyType.Wrong:
                keys = _keys;
                break;
            case UILevel003InitKey.InitKeyType.Right:
                if (fromKey == null)
                    return null;
                List<UILevel003InitKey.ClickButtonType> list = fromKey.needToClickButtonList;
                list.RemoveAt(0);
                keys = UILevel003InitKey.ListToKeys(list);
                break;
            default:
                break;
        }
        UILevel003InitKey initKey = UILevel003InitKey.GetUILevel003InitKey(toType, keys);
        return initKey;
    }
    #endregion

    #region 其他
    private void UIChangeByButtonDir<T>(ButtonDirection dir,bool hasInitData=false,params object[] initKeys) where T:UIBase
    {
        switch (dir)
        {
            case ButtonDirection.Up:
                UIManager.Instance.UIEnter<T>(UIEnterStyle.FromBottomToTop, true, hasInitData,initKeys);
                UIManager.Instance.UIExit(this, UIExitStyle.ToTop);
                break;
            case ButtonDirection.Down:
                UIManager.Instance.UIEnter<T>(UIEnterStyle.FromTopToBottom, true, hasInitData, initKeys);
                UIManager.Instance.UIExit(this, UIExitStyle.ToBottom);
                break;
            case ButtonDirection.Left:
                UIManager.Instance.UIEnter<T>(UIEnterStyle.FromRightToLeft, true, hasInitData, initKeys);
                UIManager.Instance.UIExit(this, UIExitStyle.ToLeft);
                break;
            case ButtonDirection.Right:
                UIManager.Instance.UIEnter<T>(UIEnterStyle.FromLeftToRight, true, hasInitData, initKeys);
                UIManager.Instance.UIExit(this, UIExitStyle.ToRight);
                break;
            default:
                break;
        }
    }
    #endregion
}
public class UILevel003Key
{
    /// <summary>
    /// 界面初始化类型
    /// </summary>
    public UILevel003KeyType keyType;
    /// <summary>
    /// 还需输入的密码
    /// </summary>
    public string keys;
    /// <summary>
    /// 还需点击按钮的按钮类型
    /// </summary>
    public List<ButtonDirection> needClickButtonList;
    public string promptStr
    {
        get
        {
            string str = string.Empty;
            switch (keyType)
            {
                case UILevel003KeyType.BeforeMain:
                    str = string.Empty;
                    break;
                case UILevel003KeyType.Main:
                    str = "有那么容易就过吗？不可能的好吧~.~.\n看在你来到这也不容易，就偷偷的告诉你通往下关的密码吧，请一定一定一定要记住哦！！\n2468642\n在小键盘上它们可是有特殊的魔力哦，加油吧，我在下一关等你";
                    break;
                case UILevel003KeyType.Wrong:
                    str = "啧啧啧 恭喜你又回到了原点。。。";
                    break;
                case UILevel003KeyType.Right:
                    str = "HOHOHO 又近一步了。。。";
                    break;
            }
            return str;
        }
    }
    public static UILevel003Key GetUILevel003Key(UILevel003KeyType type,string keys)
    {
        UILevel003Key initKey = new UILevel003Key();
        initKey.keyType = type;
        initKey.keys = keys;
        initKey.needClickButtonList = KeysToList(keys);
        return initKey;
    } 
    /// <summary>
    /// 将剩下所需密码转化为所要点击的按钮类型
    /// </summary>
    /// <returns></returns>
    public static List<ButtonDirection> KeysToList(string keys)
    {
        List<ButtonDirection> list = new List<ButtonDirection>();
        keys = keys.Trim();
        for (int i = 0; i < keys.Length; i++)
        {
            switch (keys[i])
            {
                case '8':
                    list.Add(ButtonDirection.Up);
                    break;
                case '2':
                    list.Add(ButtonDirection.Down);
                    break;
                case '4':
                    list.Add(ButtonDirection.Left);
                    break;
                case '6':
                    list.Add(ButtonDirection.Right);
                    break; 
                default:
                    break;
            }
        }
        return list;
    }
    public static string ListToKeys(List<ButtonDirection> list)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append(((int)list[i]).ToString());
        }
        return sb.ToString();
    }
}
public enum UILevel003KeyType
{
    BeforeMain,    //主界面之前的界面
    Main,          //主界面
    Wrong,         //错误提示界面
    Right          //正确提示界面
}
public enum ButtonDirection
{
    Up=0,               //8
    Down,               //2
    Left,               //4
    Right               //6
}