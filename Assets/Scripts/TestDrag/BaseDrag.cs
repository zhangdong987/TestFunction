using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public bool dropAll = false;

    public abstract void OnBeginDrag(PointerEventData eventData);


    public abstract void OnDrag(PointerEventData eventData);

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (dropAll)
            PassEvent(eventData,ExecuteEvents.dropHandler);
    }

    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(data,results);

        GameObject current = data.pointerDrag;

        for (int i=0;i<results.Count;++i)
        {
            if (current!=results[i].gameObject)
            {
                data.pointerPressRaycast = results[i];
                ExecuteEvents.Execute(results[i].gameObject, data, function);
                //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
            }
        }
    }
}
