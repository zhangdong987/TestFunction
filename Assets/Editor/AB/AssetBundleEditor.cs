using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//AB资源编辑器
public class AssetBundleEditor :EditorWindow 
{
    [MenuItem("Window/AssetBundle编辑器 %#O")]
    private static void OpenAssetBundleWindow()
    {
        AssetBundleEditor ABEditor = GetWindow<AssetBundleEditor>("AssetBundles");
        ABEditor.Show();
    }

    private void OnGUI()
    {
        TitleGUI();//标题栏
        AssetBundlesGUI();
        CurrentAssetBundlesGUI();
        AssetsGUI();
    }
    #region 标题栏区域
    //标记 ，用于标记当前选中的AB包索引
    private int _currentAB = -1;
    //一种系统样式  使用它就可以使按钮展示为矩形黑框样式
    private GUIStyle _preButton = new GUIStyle("PreButton");
    private GUIStyle _preDropDown = new GUIStyle("PreDropDown");
    //是否隐藏无效资源
    private bool _hideInvalidAsset = false;
    //是否隐藏已绑定资源
    private bool _hideBundleAsset = false;
    //打包路径
    private string _buildPath = "";
    //打包平台
    private BuildTarget _buildTarget = BuildTarget.StandaloneWindows;
    private void TitleGUI()
    {
        if (GUI.Button(new Rect(5,5,60,15),"Create",_preButton))
        {

        }
        //当前未选中任何一个AB包的话  禁用之后的所有UI控件
        GUI.enabled = _currentAB == -1 ? false : true;
        //重命名当前选中的AB包（必须选中任意一个AB包后方可生效）； 
        if (GUI.Button(new Rect(65,5,60,15),"Rename",_preButton))
        {

        }
        //清空当前选中的AB包中的所有资源（必须选中任意一个AB包后方可生效）； 
        if (GUI.Button(new Rect(125,5,60,15),"Clear",_preButton))
        {

        }
        //删除当前的AB包，同时会自动执行Clear操作（必须选中任意一个AB包后方可生效）； 
        if (GUI.Button(new Rect(185,5,60,15),"Delete",_preButton))
        {

        }
        //被勾选的资源添加到当前选中的AB包（必须选中任意一个AB包后方可生效）； 
        if (GUI.Button(new Rect(250,5,100,15),"Add Assets",_preButton))
        {

        }
        GUI.enabled = true;

        _hideInvalidAsset = GUI.Toggle(new Rect(360, 5, 100, 15), _hideInvalidAsset, "Hide Invalid");
        _hideBundleAsset = GUI.Toggle(new Rect(460, 5, 100, 15), _hideBundleAsset, "Hide Bundled");

        if (GUI.Button(new Rect(250,25,60,15),"Open",_preButton))
        {

        }
        if (GUI.Button(new Rect(310,25,60,15),"Browse",_preButton))
        {

        }
        GUI.Label(new Rect(370,25,70,15),"Build Path:");
        _buildPath = GUI.TextField(new Rect(440,25,300,15),_buildPath);

        BuildTarget buildTarget = (BuildTarget)EditorGUI.EnumPopup(new Rect((int)position.width-205,5,150,15),_buildTarget,_preDropDown);

        if (GUI.Button(new Rect((int)position.width-55,5,50,15),"Build",_preButton))
        {

        }
    }

    #endregion

    #region AB包列表区域
    //区域视图的范围
    private Rect _ABViewRect;
    //区域视图滚动的范围
    private Rect _ABScrollRect;
    //区域视图滚动的位置
    private Vector2 _ABScroll;
    //区域高度标记，这里不用管它 ， 是后续用来控制视图滚动量的
    private int _ABViewHeight = 0;
    //一种系统样式，使用他可以使控件周围表现为一个Box的模样
    private GUIStyle box = new GUIStyle("Box");
    private void AssetBundlesGUI()
    {
        //区域的视图范围 左上角位置固定 宽度固定240  
        _ABViewRect = new Rect(5,25,240,(int)position.height/2-20);//显示区域

        // //滚动的区域是根据当前显示的控件数量来确定的，如果显示的控件（AB包）太少，
        //则滚动区域小于视图范围，则不生效，_ABViewHeight会根据AB包数量累加
        _ABScrollRect = new Rect(5, 25, 240, _ABViewHeight);//可滚动的最大区域

        _ABScroll = GUI.BeginScrollView(_ABViewRect,_ABScroll,_ABScrollRect);
        GUI.BeginGroup(_ABScrollRect,box);
        if (_ABViewHeight<_ABViewRect.height)
        {
            _ABViewHeight = (int)_ABViewRect.height;
        }
        GUI.EndGroup();
        GUI.EndScrollView();
    }
    #endregion

    #region 当前AB包资源列表区域
    //区域视图的范围
    private Rect _currentABViewRect;
    //区域视图滚动的范围
    private Rect _currentABScollRect;
    //区域视图滚动的位置
    private Vector2 _currentABScroll;
    //区域高度标记，是后续用来控制视图滚动量的
    private int _currentABViewHeight = 0;

    private void CurrentAssetBundlesGUI()
    {
        //  //区域的视图范围：左上角位置固定在上一个区域的底部，宽度固定（240），高度为窗口高度的一半再减去空隙（15），上下都有空隙
        _currentABViewRect = new Rect(5,(int)position.height/2+10,240,(int)position.height/2-15);
        _currentABScollRect = new Rect(5,(int)position.height/2+10,240,_currentABViewHeight);

        _currentABScroll = GUI.BeginScrollView(_currentABViewRect,_currentABScroll,_currentABScollRect);
        GUI.BeginGroup(_currentABScollRect,box);
        if (_currentABViewHeight<_currentABViewRect.height)
        {
            _currentABViewHeight = (int)_currentABViewRect.height;
        }
        GUI.EndGroup();
        GUI.EndScrollView();
    }
    #endregion

    #region 所有资源列表区域
    private Rect _assetViewRect;
    private Rect _assetScrollRect;
    private Vector2 _assetScroll;
    private int _assetViewHeight = 0;

    private void AssetsGUI()
    {
        _assetViewRect = new Rect(250,45,(int)position.width-255,(int)position.height-50);
        _assetScrollRect = new Rect(250, 45, (int)position.width - 255, _assetViewHeight);

        _assetScroll = GUI.BeginScrollView(_assetViewRect,_assetScroll,_assetScrollRect);
        GUI.BeginGroup(_assetScrollRect,box);
        if (_assetViewHeight<_assetViewRect.height)
        {
            _assetViewHeight = (int)_assetViewRect.height;
        }
        GUI.EndGroup();
        GUI.EndScrollView();
    }
    #endregion
}
