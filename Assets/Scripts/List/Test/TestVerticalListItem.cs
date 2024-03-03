using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class TestVerticalListItem : MonoBehaviour
{
    [SerializeField] VerticalVirtualList list;
    [SerializeField] Button refreshListBtn;

    public int num = 10;

    List<int> nums = new List<int>();

    private void Awake()
    {
        list.listRenderer += OnListRender;
        refreshListBtn.onClick.AddListener(RefreshList);
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
        testItem.SetText(nums[index].ToString());
    }
}