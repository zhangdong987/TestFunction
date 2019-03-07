using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomList;
public class ItemData : UIListScrollIndex<ItemData>
{
    public Transform transform;

    private Button button;
    private Text text;

    private GameObject _object;
    public GameObject Object
    {
        get {
            return _object;
        }
        set
        {
            _object = value;
            if (_object == null) return;
            this.transform= _object.transform;
            button = this.transform.Find("Button").GetComponent<Button>();
            text = this.transform.Find("Text").GetComponent<Text>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>
            {
                TestScrollView.testdata.RemoveAt(m_Index);
                _scroller.DelItem(m_Index);
            });
        }
    }
    private int _index;
    public int m_Index
    {
        get
        {
            return _index;
        }
        set
        {
            _index = value;
            this.transform.localPosition = _scroller.GetPosition(_index);
            SetItemInfo(_index);
        }
    }
    private UIListScroller<ItemData> _scroller;
    public UIListScroller<ItemData> Scroller
    {
        set {
            _scroller = value;
        }
    }

   

    private void SetItemInfo(int _index)
    {
        this.text.text = TestScrollView.testdata[_index].ToString();//_index.ToString();

    }
}
