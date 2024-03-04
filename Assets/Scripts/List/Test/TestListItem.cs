using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

public class TestListItem : ListItem
{
    private Text _txtName;
    private Text _txtDescribe;
    private Button _btnOpenSprite;
    public Image image;
    public int currentindex;

    //private List<int> ints = new List<int>();
    public Action<int, TestListItem> openImgAction;

    //public List<int> Ints => ints;


    private void Awake()
    {
        recordIndex = -1;
        _txtName = GetComponentInChildren<Text>();
        _txtName = transform.GetChild(0).GetComponent<Text>();
        _txtDescribe = transform.GetChild(1).GetComponent<Text>();
        image = gameObject.GetComponent<Image>();
        _btnOpenSprite = transform.GetChild(2).GetComponent<Button>();
        _btnOpenSprite.onClick.AddListener(OnBtnOpenSpriteClicked);
    }

    public void SetText(string name, string describle)
    {
        _txtName.text = name;
        _txtDescribe.text = describle;
    }

    public void OnBtnOpenSpriteClicked()
    {
        openImgAction?.Invoke(currentindex, this);
        recordIndex = currentindex;
    }
}