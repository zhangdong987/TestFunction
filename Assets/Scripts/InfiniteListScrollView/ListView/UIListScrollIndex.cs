using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomList
{
    public interface UIListScrollIndex<T> where T:new()
    {
        GameObject Object { get; set; }
        int m_Index { get; set; }
        UIListScroller<T> Scroller { set; }
    }
}

