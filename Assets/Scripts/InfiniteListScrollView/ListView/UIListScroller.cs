using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomList
{
    public enum Arrangement
    {
        Horizontal,
        Vertical,
    }
    public class UIListScroller<T> where T:new()
    {
     
        public Arrangement _movement = Arrangement.Vertical;

        public int maxPerLine = 4;
        public int cellPadiding = 5;//主要还是用于垂直距离差  不管是何种运动方式
        //如果要用于水平距离差 则在获得GetPosition以及获得左上角起始位置的时候 cellwidth+cellpadiding

        public int cellWidth = 500;
        public int cellHeight = 100;

        //默认显示的行数 一般比可显示行数大2-3行就OK
        public int viewCount = 6;

        public GameObject itemPrefab;
        public RectTransform _content;
        private int _index = -1;
        public int _dataCount;

        private List<UIListScrollIndex<T>> _itemList;
        private Queue<UIListScrollIndex<T>> _unUsedQueue;

        private bool isinit = false;
        private int startLineIndex = 1;//起始行数
        public Type type;

        public UIListScroller(int _maxperline,int _cellpadiding,int _cellwidth,int _cellheight,int _viewCount,GameObject _itemPrefab,
            RectTransform _content, Arrangement _arrangement= Arrangement.Vertical)
        {
            this.maxPerLine = _maxperline;
            this.cellPadiding = _cellpadiding;
            this.cellWidth = _cellwidth;
            this.cellHeight = _cellheight;
            this.viewCount = _viewCount;
            this.itemPrefab = _itemPrefab;
            this._content = _content;
            this._movement = _arrangement;
        }

        //开始
        public void OnStart(int _index=1)
        {
            isinit = true;
            startLineIndex = _index;

            _itemList = new List<UIListScrollIndex<T>>();
            _unUsedQueue = new Queue<UIListScrollIndex<T>>();
            this._index = -1;
            OnInitValuePos(startLineIndex);
        }

        /*public void Update()
        {
            if (isinit == false) return;

            _itemList = new List<UIListScrollIndex>();
            _unUsedQueue = new Queue<UIListScrollIndex>();

            _index = -1;
        }*/
        private void OnInitValuePos(int _index=1)
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    _content.anchoredPosition = new Vector2((_index-1)*cellWidth,0);
                    break;
                case Arrangement.Vertical:
                    _content.anchoredPosition = new Vector2(0,(_index-1)*cellHeight);
                    break;
            }
            OnValueChange();
        }
        private int GetPosIndex()
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    return Mathf.FloorToInt(_content.anchoredPosition.x/-(cellWidth));
                case Arrangement.Vertical:
                    return Mathf.FloorToInt(_content.anchoredPosition.y/(cellHeight+cellPadiding));
            }
            return 0;
        }
        public void OnValueChange()
        {
            int index = GetPosIndex();//左上角第一个 表示第几行 或者第几列（从0开始）
 
            if (_itemList == null) return;

            if (_index != index && index > -1)
            {
                _index = index;
                for (int i = _itemList.Count; i > 0; --i)
                {
                    UIListScrollIndex<T> item = _itemList[i - 1];
                    if (item.m_Index < index * maxPerLine || (item.m_Index >= (index + viewCount) * maxPerLine))//判断Item数据是否还在显示界面范围内
                    {
                        //GameObject.Destroy(item.gameObject);
                        _itemList.Remove(item);
                        _unUsedQueue.Enqueue(item);
                    }
                }
                //i=左上角显示的其实第一个index     <最大的index数值
                for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
                {
                    if (i < 0) continue;
                    if (i > _dataCount - 1) continue;//i的最大值不能超过数据长度
                    bool isOk = false;
                    foreach (UIListScrollIndex<T> item in _itemList)
                    {
                        if (item.m_Index == i) isOk = true;
                    }
                    if (isOk) continue;
                    CreateItem(i);
                }
            }
        }

        public void AddItem(int index)
        {
            if (index > _dataCount)
            {
                Debug.LogError("添加错误:" + index);
                return;
            }
            AddItemIntoPanel(index);
            DataCount += 1;
        }

        public void DelItem(int index)
        {
            if (index < 0 || index > _dataCount - 1)
            {
                Debug.LogError("删除作物");
                return;
            }
            DelItemFromPanel(index);
            DataCount -= 1;
        }

        private void AddItemIntoPanel(int index)
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                UIListScrollIndex<T> item = _itemList[i];
                if (item.m_Index >= index) item.m_Index += 1;
            }
            CreateItem(index);
        }

        private void DelItemFromPanel(int index)
        {
            int maxIndex = -1;
            int minIndex = int.MaxValue;
            for (int i = _itemList.Count; i > 0; i--)
            {
                UIListScrollIndex<T> item = _itemList[i - 1];
                if (item.m_Index == index)
                {
                    GameObject.Destroy(item.Object);
                    _itemList.Remove(item);
                }
                if (item.m_Index > maxIndex)
                {
                    maxIndex = item.m_Index;
                }
                if (item.m_Index < minIndex)
                {
                    minIndex = item.m_Index;
                }
                if (item.m_Index > index)
                {
                    item.m_Index -= 1;
                }
            }
            if (maxIndex < DataCount - 1)
            {
                CreateItem(maxIndex);
            }
        }
        internal static GameObject AddChild(GameObject parent, GameObject item, bool isSetScale = true)
        {
            GameObject go = GameObject.Instantiate(item);

            if (item != null && parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform);
                //t.parent = parent.transform;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                if (isSetScale)
                    t.localScale = Vector3.one;
                go.layer = parent.layer;
                go.SetActive(true);
                return go;
            }
            else
            {
                Debug.LogError("this item or parent is null!");
                return null;
            }
        }
        private void CreateItem(int index)
        {
            UIListScrollIndex<T> itemBase;
           
            if (_unUsedQueue.Count > 0)
            {
                itemBase = _unUsedQueue.Dequeue();
            }
            else
            {
                T t = new T();
                itemBase = t as UIListScrollIndex<T>;
                itemBase.Object = AddChild(_content.gameObject, itemPrefab);//.GetComponent<UIListScrollIndex<T>>();
                itemBase.Object.transform.localScale = Vector3.one;
                itemBase.Object.SetActive(true);
            }
            
            itemBase.Scroller = this;
            itemBase.m_Index = index;
            _itemList.Add(itemBase);
        }


        public Vector3 GetPosition(int i)//根据index更改单独item局部位置坐标(在整个Content里面的局部坐标位置)  当一个item在界面消失的时候 他的局部坐标位置才会发生变化
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    return new Vector3((cellWidth) * (i / maxPerLine), -(cellHeight + cellPadiding) * (i % maxPerLine), 0f);
                case Arrangement.Vertical:
                    return new Vector3(cellWidth * (i % maxPerLine), -(cellHeight + cellPadiding) * (i / maxPerLine), 0f);
            }
            return Vector3.zero;
        }

        public int DataCount
        {
            get { return _dataCount; }
            set
            {
                _dataCount = value;
                UpdateTotalWidth();
            }
        }

        private void UpdateTotalWidth()
        {
            int lineCount = Mathf.CeilToInt((float)_dataCount / maxPerLine);//总数据需要几行几列
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    _content.sizeDelta = new Vector2(cellWidth * lineCount + cellPadiding * (lineCount - 1), _content.sizeDelta.y);
                    break;
                case Arrangement.Vertical:
                    _content.sizeDelta = new Vector2(_content.sizeDelta.x, cellHeight * lineCount + cellPadiding * (lineCount - 1));
                    break;
            }
        }
    }
}


