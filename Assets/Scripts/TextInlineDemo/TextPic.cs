using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class TextPic : Text
{
    private readonly List<Image> m_ImagePool = new List<Image>();

    //图片的最后一个顶点的索引
    private readonly List<int> m_ImageVertexIndex = new List<int>();

    private static readonly Regex s_Regex = new Regex(@"<quad name=(.+?) size=(\d*\.?\d+%?) width=(\d*\.?\d+%?) />", RegexOptions.Singleline);

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        UpdateQuadImage();
    }


    protected void UpdateQuadImage()
    {
        m_ImageVertexIndex.Clear();
        foreach (Match match in s_Regex.Matches(text))
        {
            var picIndex = match.Index + match.Length - 1;
            var endIndex = picIndex * 4 + 3;
            m_ImageVertexIndex.Add(endIndex);


            m_ImagePool.RemoveAll(image => image == null);
            if (m_ImagePool.Count==0)
            {
                GetComponentsInChildren<Image>(m_ImagePool);
            }
            if (m_ImageVertexIndex.Count>m_ImagePool.Count)
            {
                var resources = new DefaultControls.Resources();
                var go = DefaultControls.CreateImage(resources);
                go.layer = gameObject.layer;
                var rt = go.transform as RectTransform;
                if (rt)
                {
                    rt.SetParent(rectTransform);
                    rt.localPosition = Vector3.zero;
                    rt.localRotation = Quaternion.identity;
                    rt.localScale = Vector3.one;
                }
                m_ImagePool.Add(go.GetComponent<Image>());
            }
            var spriteName = match.Groups[1].Value;
            var size = float.Parse(match.Groups[2].Value);
            var img = m_ImagePool[m_ImageVertexIndex.Count - 1];
            if (img.sprite == null || img.sprite.name != spriteName)
            {
                img.sprite = Resources.Load<Sprite>(spriteName);
            }
            img.rectTransform.sizeDelta = new Vector2(size, size);
            img.enabled = true;
        }

        for (var i = m_ImageVertexIndex.Count; i < m_ImageVertexIndex.Count; i++)
        {
            if (m_ImagePool[i])
            {
                m_ImagePool[i].enabled = false;
            }
        }
        
    }




    protected override void OnPopulateMesh(Mesh toFill)
    {
        base.OnPopulateMesh(toFill);
        var verts = toFill.vertices;

        for (var i = 0; i < m_ImageVertexIndex.Count; i++)
        {
            var endIndex = m_ImageVertexIndex[i];
            var rt = m_ImagePool[i].rectTransform;
            var size = rt.sizeDelta;
            if (endIndex < verts.Length)
            {
                rt.anchoredPosition = new Vector2(verts[endIndex].x + size.x / 2, verts[endIndex].y + size.y / 2);

                // 抹掉左下角的小黑点
                for (int j = endIndex, m = endIndex - 3; j > m; j--)
                {
                    verts[j] = verts[m];
                }
            }
        }

        if (m_ImageVertexIndex.Count != 0)
        {
            toFill.vertices = verts;
            m_ImageVertexIndex.Clear();
        }
    }
}
