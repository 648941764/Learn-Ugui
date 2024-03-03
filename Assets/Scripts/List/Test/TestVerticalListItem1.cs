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

    List<int> nums = new List<int>();

    private void Awake()
    {
        list.listRenderer += OnListRender;
        refreshListBtn.onClick.AddListener(RefreshList);
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
        list.ItemDataNum = nums.Count;
    }

    private void OnListRender(int index, ListItem listItem)
    {
        TestListItem testItem = listItem as TestListItem;
        testItem.SetText(nums[index].ToString(), $"етЪЧ{nums[index]}", sprites[index]);
    }
}
