using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : BaseUI
{
    [SerializeField] private Button _btnClose;

    protected override void OnOpen()
    {
        base.OnOpen();
    }

    protected override void InitComponents()
    {
        base.InitComponents();
        _btnClose.onClick.AddListener(OnBtnCloseClicked);
    }

    public void OnBtnCloseClicked()
    {
        UIManager.Instance.DestroyPaenl<TestPanel>();
    }
}
