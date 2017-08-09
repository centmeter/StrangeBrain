using UnityEngine;
using System.Collections;

public class TestMain : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.UIEnter<UILevel002>(false,UIEnterStyle.FromLeftToRight);
    }
}
