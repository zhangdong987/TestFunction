using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum FileType
{
    ValidFile,//有效的文件资源
    Folder,//文件夹
    InValidFile,//无效的文件资源
}

public class AssetInfo 
{
    //资源文件在硬盘中的完整路径
    public string AssetFullPath
    {
        get;
        private set;
    }
    //因为资源最终的目的是要打入AB包中，所以我们还要保留资源文件的Assets路径（也即是从当前项目的Assets目录开始的路径)
    public string AssetPath
    {
        get;
        private set;
    }
    //资源名称
    public string AssetName
    {
        get;
        private set;
    }
    //资源的GUID
    public string GUID
    {
        get;
        private set;
    }
    //资源类型
    public Type AssetType
    {
        get;
        private set;
    }
    //资源文件类型
    public FileType AssetFileType
    {
        get;
        private set;
    }

    //资源是否勾选
    public bool IsChecked
    {
        get;
        set;
    }
    //文件夹是否展开(资源无效)
    public bool IsExpanding
    {
        get;
        set;
    }
    //所属的AB包（文件夹无效）
    public string Bundled
    {
        get;
        set;
    }
    //文件夹的子资源（资源无效）
    public List<AssetInfo> childAssetInfo
    {
        get;
        set;
    }
    //文件类型资源
    public AssetInfo(string fullePath,string name,string extension)
    {
        AssetFullPath = fullePath;
        AssetPath = "Assets" + fullePath.Replace(Application.dataPath.Replace("/","\\"),"");
        AssetName = name;
        GUID = AssetDatabase.AssetPathToGUID(AssetPath);
        AssetFileType = AssetBundleTool.GetFileTypeByExtension(extension);
        AssetType = AssetDatabase.GetMainAssetTypeAtPath(AssetPath);
        IsChecked = false;
        IsExpanding = false;
        Bundled = "";
        childAssetInfo = null;
    }
    //文件夹类型资源
    public AssetInfo(string fullPath,string name,bool isExpanding)
    {
        AssetFullPath = fullPath;
        AssetPath = "Assets" + fullPath.Replace(Application.dataPath.Replace("/","\\"),"");
        AssetName = name;
        GUID = "";
        AssetFileType = FileType.Folder;
        AssetType = null;
        IsChecked = false;
        IsExpanding = isExpanding;
        Bundled = "";
        childAssetInfo = new List<AssetInfo>();
    }
    

}
