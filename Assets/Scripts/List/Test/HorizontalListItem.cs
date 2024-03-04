using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalListItem : ListItem
{
    private Text _txtDescribe;
    public int currentindex;

    //public Action<int, HorizontalListItem>

    private void Awake()
    {
        recordIndex = -1;
        _txtDescribe = GetComponentInChildren<Text>();
    }

    public void SetText(string describe)
    {
        _txtDescribe.text = describe;
    }
}
