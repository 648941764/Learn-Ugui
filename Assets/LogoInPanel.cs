using System.Collections;
using System.Collections.Generic;
using TestGoogleBuffer;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using System;

public class LogoInPanel : BaseUI
{
    [Header("Main")]
    [SerializeField] private Button _btnOpenLogoInPanel;
    [SerializeField] private Button _btnOpenRegisterPanel;
    private GameObject _mainObj;

    [Header("LogoIn")]
    [SerializeField] private InputField _logoInAccount;
    [SerializeField] private InputField _logoInPassword;
    [SerializeField] private Button _btnEnter;
    private GameObject _logoInObj;

    [Header("Register")]
    [SerializeField] private InputField _registerAccount;
    [SerializeField] private InputField _registerPassword;
    [SerializeField] private Button _btnRegister;
    [SerializeField] private Button _btnCloseRegister;
    private GameObject _registerObj;
    private string _inputTextForAccount;
    private string _inputTextForPassword;
    private Dictionary<string, UserInfo> _dictUser = new Dictionary<string, UserInfo>();
    public event Action UpdateHandle;
    protected override void InitComponents()
    {
        base.InitComponents();
        _registerObj = GameObject.Find("Register");
        _logoInObj = GameObject.Find("LogoIn");
        _mainObj = GameObject.Find("Main");
        _registerObj.SetActive(false);
        _logoInObj.SetActive(false);

        _btnOpenLogoInPanel.onClick.AddListener(OnBtnOpenLoGoInPanelClicked);
        _btnOpenRegisterPanel.onClick.AddListener(OnBtnOpenRegisterPanelClicked);
        _btnEnter.onClick.AddListener(OnBtnEnterClicked);
        _btnRegister.onClick.AddListener(OnBtnRegisterCliked);
        _btnCloseRegister.onClick.AddListener(OnBtnCloseRegister);

        _logoInAccount.onEndEdit.AddListener(OnLogoInAccountEndEdit);
        _logoInPassword.onValueChanged.AddListener(OnLogoInPasswordEndit);
        _registerAccount.onEndEdit.AddListener(OnRegisterAccountEndEdit);
        _registerPassword.onValueChanged.AddListener(OnRegisterPasswordEndEdit);

        _btnEnter.interactable = false;
        UpdateHandle += LogoInUpdate;
    }

    private void Update()
    {
        UpdateHandle?.Invoke();

    }

    private void LogoInUpdate()
    {
        if (_inputTextForAccount != null)
        {
            _btnEnter.interactable = true;
        }
    }

    #region 按钮事件

    private void OnBtnOpenLoGoInPanelClicked()
    {
        _logoInObj.SetActive(true);
        _mainObj.SetActive(false);
    }

    private void OnBtnOpenRegisterPanelClicked()
    {
        _registerObj.SetActive(true);
        _mainObj.SetActive(false);
    }

    private void OnBtnEnterClicked()
    {
        if (!_dictUser.ContainsKey(_inputTextForAccount) || _dictUser[_inputTextForAccount].Password != _inputTextForPassword)
        {
            Debug.LogWarning($"{_inputTextForAccount}账号或者密码错误");
            return;
        }

        //if (_dictUser[_inputTextForAccount].Password != _inputTextForPassword)
        //{
        //    Debug.LogWarning($"{_inputTextForPassword}密码错误");
        //    return;
        //}

        Debug.Log($"<color=red>登录成功</color>");
    }

    private void OnBtnRegisterCliked()
    {
        if (_inputTextForPassword == null || _inputTextForAccount == null)
        {
            Debug.LogError("账号密码不能为空");
            return;
        }

        UserInfo user = new UserInfo()
        {
            Password = _inputTextForPassword,
        };

        _dictUser.Add(_inputTextForAccount, user);
        Debug.Log($"<color=red>注册成功</color>");
    }

    private void OnBtnCloseRegister() 
    {
        _registerObj.SetActive(false);
        _mainObj.SetActive(true);
        _inputTextForPassword = "";
        _inputTextForAccount = "";
        
    }

    #endregion

    #region 输入框的事件

    private void OnLogoInAccountEndEdit(string userInput)
    {
        _inputTextForAccount = "";
        _inputTextForAccount = _logoInAccount.text;
        if (_inputTextForAccount == "")
        {
            _inputTextForAccount = null;
            _btnEnter.interactable = false;
        }
    }

    private void OnLogoInPasswordEndit(string userInput)
    {
        _inputTextForPassword = "";
        _inputTextForPassword = _logoInPassword.text;
    }

    private void OnRegisterAccountEndEdit(string userInput)
    {
        _inputTextForAccount = "";
        _inputTextForAccount = _registerAccount.text;
    }

    private void OnRegisterPasswordEndEdit(string userInput)
    {
        _inputTextForPassword = "";
        _inputTextForPassword = _registerPassword.text;
    }

    #endregion
}
