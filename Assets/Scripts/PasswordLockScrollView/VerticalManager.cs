using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VerticalManager : MonoBehaviour {

    public GameObject verticlePrefab;
    private GameObject[] yearsButtons;
    public RectTransform yearsScrollingPanel;
    UIVerticalScroller yearsVerticalScroller;
    public RectTransform yearsCenter;
    public ScrollRect yearsScrollRect;

    public Button downBtn;
    public Button upBtn;
    public Text curText;

    public InputField inputFieldYears;
    public Button setBtn;
    // Use this for initialization
    void Start () {
        InitList();

        yearsVerticalScroller = new UIVerticalScroller(yearsCenter, yearsCenter, yearsScrollRect, yearsButtons, ChangeCurText);
       /* yearsVerticalScroller.scrollDownButton = downBtn;
        yearsVerticalScroller.scrollUpButton = upBtn;*/
        yearsVerticalScroller.Start();

        downBtn.onClick.AddListener(()=>
        {
            yearsVerticalScroller.ScrollDown();
        });
        upBtn.onClick.AddListener(()=>
        {
            yearsVerticalScroller.ScrollUp();
        });
        setBtn.onClick.AddListener(()=>
        {
            SetData();
        });
    }

    private void ChangeCurText(string text)
    {
        curText.text = text;
    }

    public void SetData()
    {
        int yearsSet = int.Parse(inputFieldYears.text) - 1900;
        
        yearsVerticalScroller.SnapToElement(yearsSet);
    }

    private void InitList()
    {
        int currentYear = int.Parse(System.DateTime.Now.ToString("yyyy"));

        int[] arrayYears = new int[currentYear + 1 - 1900];

        yearsButtons = new GameObject[arrayYears.Length];

        for (int i = 0; i < arrayYears.Length; i++)
        {
            arrayYears[i] = 1900 + i;

            GameObject clone = Instantiate(verticlePrefab, yearsScrollingPanel);
            clone.transform.localScale = new Vector3(1, 1, 1);
            clone.GetComponentInChildren<Text>().text = "" + arrayYears[i];
            clone.name = "Year_" + arrayYears[i];
            clone.AddComponent<CanvasGroup>();
            yearsButtons[i] = clone;

        }
    }

    // Update is called once per frame
    void Update () {
        yearsVerticalScroller.Update();
	}
}
