using UnityEngine;
using System.Collections;

public class TestMain : MonoBehaviour
{
    private void Start()
    {
        UILevel003InitKey key = UILevel003InitKey.GetUILevel003InitKey(UILevel003InitKey.InitKeyType.BeforeMain, "");
        UIManager.Instance.UIEnter<UILevel003>(UIEnterStyle.FromTopToBottom,true,true,key);
    }
}
