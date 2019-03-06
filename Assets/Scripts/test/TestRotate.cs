using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{

    public Transform ptransform;
    public Transform MousePosFlag;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool start = false;
    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            this.ptransform.localEulerAngles = new Vector3(0,0,this.ptransform.localEulerAngles.z+0.5f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 uiPos = this.transform.InverseTransformPoint(worldPos);
            MousePosFlag.transform.localPosition = uiPos;
            Vector2 dir = new Vector2(uiPos.x-this.ptransform.localPosition.x,uiPos.y-this.ptransform.localPosition.y).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
            this.ptransform.localEulerAngles = new Vector3(0,0,angle);

        }
     
    }
    private void OnGUI()
    {
        if (GUILayout.Button("开始旋转"))
        {
            start = true;
        }
        if (GUILayout.Button("关闭旋转"))
        {
           // start = false;
        }
    }
}
