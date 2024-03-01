using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf.Collections;
using Google.Protobuf;

public class CountPanel : BaseUI
{
    [SerializeField] private Text _txtCount;
    [SerializeField] private Button _btnAddCount, _btnReduceCount, _btnExit;
    private int _count = 0;

    protected override void OnOpen()
    {
        base.OnOpen();
        AddEvent(OnTxtCountChange);
    }

    protected override void OnClose()
    {
        base.OnClose();
        DelEvent(OnTxtCountChange);
        Debug.Log($"<color=green>取消注册事件</color>");
    }

    protected override void InitComponents()
    {
        base.InitComponents();
        _btnAddCount.onClick.AddListener(OnBtnAddClicked);
        _btnReduceCount.onClick.AddListener(OnReduceClicked);
        _btnExit.onClick.AddListener(OnExitBtnClicked);
        //AddEvent(OnTxtCountChange);
    }

    protected override void OnReFresh()
    {
        base.OnReFresh();
        _txtCount.text = _count.ToString();
    }

    public void OnTxtCountChange(EventParam eventParam)
    {
        if (eventParam.eventName != EventType.OnTextChange) 
        {
            return;
        }
        OnReFresh();
    }

    private void OnBtnAddClicked()
    {
        _count++;
        EventManager.Instance.BroadCast(EventParam.Get(EventType.OnTextChange));
    }

    private void OnReduceClicked()
    {
        _count--;
        EventManager.Instance.BroadCast(EventParam.Get(EventType.OnTextChange));
    }

    private void OnExitBtnClicked()
    {
        UIManager.Instance.ClosePanel<CountPanel>();
    }

}
