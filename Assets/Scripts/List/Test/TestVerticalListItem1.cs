using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestVerticalListItem1 : MonoBehaviour
{
    [SerializeField] VerticalVirtualList list;
    [SerializeField] Button refreshListBtn;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    public int num = 10;

    Sprite[] ss;

    List<int> nums = new List<int>();

    private void Awake()
    {
        list.listRenderer += OnListRender;
        refreshListBtn.onClick.AddListener(RefreshList);
        ss = Resources.LoadAll<Sprite>("GUI");
        //atlas = Resources.Load<Sprite>("GUI");
    }

    private void Start()
    {
        RefreshList();
    }

    private void RefreshList()
    {
        nums.Clear();
        int i = 0;
        while (++i <= Random.Range(num, num + 10))
        {
            nums.Add(i);
        }
        Debug.Log($"{nums.Count}");
        list.ItemDataNum = ss.Length;
    }

    private void OnListRender(int index, ListItem listItem)
    {
        TestListItem testItem = listItem as TestListItem;
        testItem.SetText(ss[index].name, $"етЪЧ{index + 1}", sprites[index]);
    }
}
