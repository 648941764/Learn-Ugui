using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItemPure : MonoBehaviour
{
    public VerticalVirtualList list;
    public int num;
    public readonly List<int> datas = new List<int>();

    private void OnEnable()
    {
        list.listRenderer += RenderItem;
    }

    private void OnDisable()
    {
        list.listRenderer -= RenderItem;
    }

    public void Refresh()
    {
        num = 13; // Random.Range(10, 21);
        datas.Clear();
        int i = 0;
        while (++i <= num)
        {
            datas.Add(i);
        }
        list.ItemDataNum = num;
    }

    private void RenderItem(int index, ListItem listItem)
    {
        var content = listItem.transform.GetComponentInChildren<Text>();
        content.text = datas[index].ToString();
    }
}