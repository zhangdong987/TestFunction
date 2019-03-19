using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AB包配置对象
public class AssetBundleInfo 
{
    public List<AssetBundleBuildInfo> AssetBundles
    {
        get;
        set;
    }
    public AssetBundleInfo()
    {
        AssetBundles = new List<AssetBundleBuildInfo>();
    }

    //是否存在该ab包名
    public bool IsExistName(string name)
    {
        foreach (AssetBundleBuildInfo one in AssetBundles)
        {
            if (one.Name==name)
            {
                return true;
            }
        }
        return false;
    }
}
