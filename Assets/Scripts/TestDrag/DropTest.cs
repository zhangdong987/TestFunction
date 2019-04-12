using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//给3D摄像机要挂载  PhysicsRaycaster 组件
public class DropTest : EventTrigger
{
    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("我收到一个拖入者:" + eventData.lastPress);
        this.transform.position = new Vector3(eventData.pointerCurrentRaycast.worldPosition.x, eventData.pointerCurrentRaycast.worldPosition.y,this.transform.position.z);
        Debug.Log("position==="+eventData.pointerCurrentRaycast.worldPosition);
        base.OnDrag(eventData);
    }
}
