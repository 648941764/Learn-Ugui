using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool _isRemove = false;
    protected string _panelName;
    protected virtual void OnOpen(string name) 
    {
        this._panelName = name;
        gameObject.SetActive(true);
    }
    public virtual void OnClose() 
    {
        _isRemove = true;
        gameObject.SetActive(false);
        Destroy(gameObject);

        //������棬��ʾ����û�д�
        if (UIManagerTest.Instance.PanelDict.ContainsKey(name))
        {
            UIManagerTest.Instance.PanelDict.Remove(name);
        }
    }
    public virtual void OnFresh() { }
}
