using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetBundleTool
{
    public static FileType GetFileTypeByExtension(string extension)
    {
        if (extension == ".cs")
        {
            return FileType.InValidFile;
        }
        return FileType.ValidFile;
    }
    //是否是无效的文件夹 true为有效文件夹
    private static bool IsValidFolder(string fileName)
    {
        fileName = fileName.ToLower();
        if (fileName.Contains("streamingassets")||fileName.Contains("editor")||fileName.Contains("plugins"))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 读取资源文件夹下的所有子资源
    /// </summary>
    public static void ReadAssetsInChildren(AssetInfo asset)
    {
        if (asset.AssetFileType!=FileType.Folder)
        {
            return;
        }
        DirectoryInfo di = new DirectoryInfo(asset.AssetFullPath);

        FileSystemInfo[] fileinfo = di.GetFileSystemInfos();

        foreach (FileSystemInfo fi in fileinfo)
        {
            if (fi is DirectoryInfo)
            {
                if (IsValidFolder(fi.Name))
                {
                    AssetInfo ai = new AssetInfo(fi.FullName,fi.Name,false);
                    asset.childAssetInfo.Add(ai);
                    //继续深层遍历这个文件夹
                    ReadAssetsInChildren(ai);
                }
            }
            else
            {
                if (fi.Extension!=".meta")
                {
                    AssetInfo ai = new AssetInfo(fi.FullName,fi.Name,fi.Extension);
                    
                    asset.childAssetInfo.Add(ai);
                    if (ai.AssetFileType!=FileType.InValidFile)
                    {
                        AssetBundleEditor._validAssets.Add(ai);
                    }

                    AssetImporter importer = AssetImporter.GetAtPath(ai.AssetPath);
                    if (importer.assetBundleName!="")
                    {
                        //ai.Bundled = importer.assetBundleName;
                        ai.IsChecked = true;
                        AssetBundleBuildInfo one = AssetBundleEditor._assetBundle.IsExistName(importer.assetBundleName);
                        if (one==null)
                        {
                            one = new AssetBundleBuildInfo(importer.assetBundleName);
                            AssetBundleEditor._assetBundle.AssetBundles.Add(one);
                        }
                        one.AddAsset(ai);
                    }

                }
            }
        }
    }

    public static void CleareAsset(this AssetBundleBuildInfo build)
    {
        if (build == null || build.Assets == null || build.Assets.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < build.Assets.Count; ++i)
        {
            build.Assets[i].Bundled = "";
            AssetImporter importer = AssetImporter.GetAtPath(build.Assets[i].AssetPath);
            importer.assetBundleName = "";
        }
        build.Assets.Clear();
    }

    //删除AB包
    public static void DeleteAssetBundle(this AssetBundleInfo abInfo,int index)
    {
        abInfo.AssetBundles[index].CleareAsset();
        AssetDatabase.RemoveAssetBundleName(abInfo.AssetBundles[index].Name,true);
        abInfo.AssetBundles.RemoveAt(index);
    }

    /// <summary>
    /// 获取所有被选中的有效资源 且没有包名的资源
    /// </summary>
    public static List<AssetInfo> GetCheckedAssets(this List<AssetInfo> validAssetList)
    {
        List<AssetInfo> currentAssets = new List<AssetInfo>();
        for (int i = 0; i < validAssetList.Count; i++)
        {
            if (validAssetList[i].IsChecked&&validAssetList[i].Bundled=="")
            {
                currentAssets.Add(validAssetList[i]);
            }
        }
        return currentAssets;
    }
    public static void BuildAssetBundles()
    {
        string buildPath = EditorPrefs.GetString("BuildPath", "");
        if (!Directory.Exists(buildPath))
        {
            Debug.LogError("Please set build path！");
            return;
        }

        BuildTarget target = (BuildTarget)EditorPrefs.GetInt("BuildTarget", 5);

        BuildPipeline.BuildAssetBundles(buildPath, BuildAssetBundleOptions.None, target);
    }
}
