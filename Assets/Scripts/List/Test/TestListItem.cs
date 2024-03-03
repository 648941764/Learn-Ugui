using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class TestListItem : ListItem
{
    private Text _txtName;
    private Text _txtdescribe;
    private Image _image;

    private void Awake()
    {
        _txtName = GetComponentInChildren<Text>();
        _txtName = transform.GetChild(0).GetComponent<Text>();
        _txtdescribe = transform.GetChild(1).GetComponent<Text>();
        _image = gameObject.GetComponent<Image>();
    }

    public void SetText(string name, string describle, Sprite sprite)
    {
        _txtName.text = name;
        _txtdescribe.text = describle;
        _image.sprite = sprite;
    }
}