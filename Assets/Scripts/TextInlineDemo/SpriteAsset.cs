using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAsset : ScriptableObject
{
    //图集ID
    public int ID;
    //是否是静态表情
    public bool _IsStatic;
    //图片资源
    public Texture texSource;
    /// <summary>
    /// 所有sprite信息 SpriteAssetinfo类为具体的信息类
    /// </summary>
    public List<SpriteInforGroup> listSpriteGroup;
}

[System.Serializable]
public class SpriteInfor
{
    public int ID;
    public string name;
    public Vector2 pivot;
    public Rect rect;
    public Sprite sprite;
    public string tag;
    public Vector2[] uv;
}


[System.Serializable]
public class SpriteInforGroup
{
    public string tag = "";
    public List<SpriteInfor> listSpriteInfor = new List<SpriteInfor>();
    public float width = 1.0f;
    public float size = 24.0f;
}
