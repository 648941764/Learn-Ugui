using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(RectTransform))]
public class BaseUI : MonoBehaviour
{

    [SerializeField] private UILayer _uILayer = UILayer.DeafutLayer;
    private void Awake()
    {
        SetLayer();
        InitComponents();
    }

    protected virtual void OnOpen() { OnReFresh(); /*这里需要添加一个设置父节点的字段*/ }
    protected virtual void OnClose() { }
    protected virtual void OnReFresh() { }
    protected virtual void InitComponents() { }

    public void Enable() { gameObject.SetActive(true); OnOpen(); }
    public void Disable() { gameObject.SetActive(false); OnClose(); }
    public void Destroy() { Destroy(gameObject); }

    private void SetLayer()
    {
        this.GetComponent<Canvas>().sortingOrder = (int)_uILayer;
    }
}

public enum UILayer
{
    DeafutLayer = 0,
    BackLayer = 10,
    SceneLayer = 20,
    MiddleLayer = 30,
    TopLayer = 40,
}
