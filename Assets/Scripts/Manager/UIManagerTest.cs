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
        //检查是否已经打开
        if (_panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError($"<color=red>界面已经打开</color>");
            return null;
        }

        //检查路径配置是否有问题
        string path = "";
        if (!_pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("界面名称错误，或未配置路径" + name);
            return null;
        }

        GameObject panelPrefab = null;
        if (!_prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realpath = "Prefab/panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realpath) as GameObject;
            _prefabDict.Add(name, panelPrefab);
        }

        //打开界面
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
            Debug.LogError("用户界面未打开" + name);
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
