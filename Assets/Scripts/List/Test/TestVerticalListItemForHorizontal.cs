using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestVerticalListItemForHorizontal : MonoBehaviour
{
    [SerializeField] HorizontaVirtualList list;
    [SerializeField] Button refreshListBtn;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    public int num = 10;
    private int _chooseItemSprite;
    Sprite[] ss;

    List<int> nums = new List<int>();

    private void Awake()
    {
        list.listRefresh += OnListRender;
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
        HorizontalListItem testItem = listItem as HorizontalListItem;
        testItem.SetText(ss[index].name);
        testItem.currentindex = index;
        //testItem.openImgAction += OnRefreshImg;
        //更新图片的显示，该显示显示，不该显示就把那给关掉
        //if (testItem.recordIndex != index)
        //{
        //    testItem.image.sprite = null;
        //}
        //else
        //{
        //    testItem.image.sprite = ss[index];
        //}

    }

    public void OnRefreshImg(int index, TestListItem testListItem)
    {
        //点击按钮的时候就把其他打开过的物体给关了
        for (int i = -1; ++i < list.Items.Length;)
        {
            if (i != index)
            {
                list.Items[i].GetComponent<Image>().sprite = null;
                testListItem.image.sprite = ss[index];
                list.Items[i].recordIndex = -1;
            }
        }
    }
}
