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
    private int _chooseItemSprite;
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
        list.ItemDataNum = 11; //ss.Length;
    }

    private void OnListRender(int index, ListItem listItem)
    {
        TestListItem testItem = listItem as TestListItem;
        testItem.SetText(ss[index].name, $"����{index + 1}");
        testItem.currentindex = index;
        testItem.openImgAction += OnRefreshImg;
        //����ͼƬ����ʾ������ʾ��ʾ��������ʾ�Ͱ��Ǹ��ص�
        if (testItem.recordIndex != index)
        {
            testItem.image.sprite = null;
        }
        else
        {
            testItem.image.sprite = ss[index];
        }
    }

    public void OnRefreshImg(int index, TestListItem testListItem)
    {
        //�����ť��ʱ��Ͱ������򿪹������������
        for (int i = -1; ++i < list.Items.Length;)
        {
            if (i != index)
            {
                list.Items[i].GetComponent<Image>().sprite = null;
                list.Items[i].IsOpen = false;
                testListItem.image.sprite = ss[index];
                list.Items[i].recordIndex = -1;
            }
        }

    }

}
