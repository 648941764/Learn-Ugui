using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManagerTest : MonoBehaviour
{
    private static UIManagerTest _instance;
    private Transform _uiRoot;
    private Dictionary<string, string> _pathDict;
    private Dictionary<string, GameObject> _prefabDict;
    private Dictionary<string, BasePanel> _panelDict;

    public Dictionary<string, BasePanel> PanelDict => _panelDict;


    public static UIManagerTest Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindAnyObjectByType<UIManagerTest>();
                if (_instance == null )
                {
                    _instance = new UIManagerTest();
                }
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        DontDestroyOnLoad(_instance.gameObject);
    }

    public Transform UIRoot
    {
        get
        {
            if ( _uiRoot == null )
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    private UIManagerTest() 
    {
        InitDict();

    }

    private void InitDict()
    {
        _pathDict = new Dictionary<string, string>()
        {
            {UIConst.mainMenuPanel, "Menu/MainMenuPanel" },
            {UIConst.userPanel, "Menu/UserPanel" },
            {UIConst.newUserPanel, "Menu/NewUserPanel" }
        };
        _panelDict = new Dictionary<string, BasePanel>();
        _prefabDict = new Dictionary<string, GameObject>();

    }

    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        //����Ƿ��Ѿ���
        if (_panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError($"<color=red>�����Ѿ���</color>");
            return null;
        }

        //���·�������Ƿ�������
        string path = "";
        if (!_pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("�������ƴ��󣬻�δ����·��" + name);
            return null;
        }

        GameObject panelPrefab = null;
        if (!_prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realpath = "Prefab/panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realpath) as GameObject;
            _prefabDict.Add(name, panelPrefab);
        }

        //�򿪽���
        GameObject panelObject = Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        _panelDict.Add(name, panel);
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!_panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("�û�����δ��" + name);
            return false;
        }
        panel.OnClose();
        return true;
    }
}

public class UIConst
{
    public const string mainMenuPanel = "MainMenuPanel" ;
    public const string userPanel = "UserPanel";
    public const string newUserPanel = "NewUserPanel";
}
