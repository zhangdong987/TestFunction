using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerl : MonoBehaviour
{
    private static GameManagerl instance = null;
    public static GameManagerl Instance
    {
        get
        {
            return instance;
        }
    }

    private int[] Array;
    private int[] Order;
    public GameObject table;

    float starttime = 0f;
    public Text timetext;
    bool isstart = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Array = new int[] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16};
        Order = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }; 
        isstart = false;
    }

    public void GameStart()
    {
        A_random(Array,16);
        p_squence(table,Array);

        starttime = 0f;
        isstart = true;
        
    }
    private void Update()
    {
        if (isstart)
        {
            starttime = (starttime + Time.deltaTime);
            timetext.text = TransTimeSecondIntToString((long)starttime);
        }
    }

    void A_random(int[] a,int temptimes)
    {
        for (int i=0;i<temptimes;++i)
        {
            int r = Random.Range(0,a.Length);
            int temp = a[i];
            a[i] = a[r];
            a[r] = temp;
        }

    }
    void p_squence(GameObject table,int[] a)
    {
        foreach (int num in a)
        {
            string name = "pice" + num;
            table.transform.Find(name).SetSiblingIndex(-1);

        }
    }
    /// <summary> 
    /// 获取数字num对应Array的索引值和table子物体的索引值 
    /// </summary> 
    /// <param name="num"></param> 
    public void Getsiblingindex(int num)
    {
        int s = table.transform.Find("pice" + num).GetSiblingIndex();
        A_exchange(s + 1);
    }
    void A_exchange(int x)
    {
        int index = table.transform.Find("pice16").GetSiblingIndex() + 1;
        if (((index == x - 1) && !(index % 4 == 0)) || ((index == x + 1) && !(index % 4 == 1)) || (index + 4 == x) || (index - 4 == x))
        {
            int temp = Array[x - 1];
            Array[x - 1] = Array[index - 1];
            Array[index - 1] = temp;
            p_squence(table, Array);
        }
        if (Array == Order)
        {
            Debug.Log("匹配成功");
            isstart = false;
        }
    }

    string TransTimeSecondIntToString(long second)
    {
        string str = "";
        long hour = second / 3600;
        long min = second % 3600 / 60;
        long sec = second % 60;
        if (hour < 10)
        {
            str += "0" + hour.ToString();
        }
        else
        {
            str += hour.ToString();
        }
        str += ":";
        if (min < 10)
        {
            str += "0" + min.ToString();
        }
        else
        {
            str += min.ToString();
        }
        str += ":";
        if (sec < 10)
        {
            str += "0" + sec.ToString();
        }
        else
        {
            str += sec.ToString();
        }
        return str;
    }

    }
