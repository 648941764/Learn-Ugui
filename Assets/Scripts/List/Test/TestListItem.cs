using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class TestListItem : ListItem
{
    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    public void SetText(string content)
    {
        text.text = content;
    }
}