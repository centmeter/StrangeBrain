using UnityEngine;
using System.Collections;

public class TestMain : MonoBehaviour
{
    private void Start()
    {
        UILevel003Key key = new UILevel003Key(UILevel003KeyType.BeforeMain, string.Empty);
        UIManager.Instance.UIEnter<UILevel003>(UIEnterStyle.FromTopToBottom,true,true,key);
    }
}
