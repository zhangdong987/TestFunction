using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleTool
{
    public static FileType GetFileTypeByExtension(string extension)
    {
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
                }
            }
        }
    }
}
