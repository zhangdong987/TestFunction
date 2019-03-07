using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomList;
using UnityEngine.UI;
public class TestScrollView : MonoBehaviour
{
    public GameObject Scroller;
    public ScrollRect ScrollRect;
    public RectTransform Content;

    private UIListScroller<ItemData> _listscroller;

    public GameObject ItemPrefab;

    public static List<int> testdata = new List<int>();

    private void Awake()
    {
        _listscroller = new UIListScroller<ItemData>(4,10,75,58,8,ItemPrefab,Content,Arrangement.Vertical);
        for (int i=0;i<1000;++i)
        {
            testdata.Add(i);
        }
    }

    public void Start()
    {
        _listscroller.DataCount = 900;
        ScrollRect.onValueChanged.RemoveAllListeners();
        ScrollRect.onValueChanged.AddListener((v)=>
        {
            _listscroller.OnValueChange();
        });
        _listscroller.OnStart();
    }
}
