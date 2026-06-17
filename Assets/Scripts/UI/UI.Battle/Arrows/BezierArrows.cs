using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BezierArrows : MonoBehaviour
{
    #region Public Fields
    public GameObject ArrowHeadPrefab;
    public GameObject ArrowNodePrefab;
    public int arrowNodeNum;
    public float scaleFactor = 1.0f;

    public static BezierArrows Instance { get; private set; }

    #endregion

    #region Private Fields
    Vector3 origin;
    Vector3 top;
    List<RectTransform> arrowNodes = new List<RectTransform>();

    List<Vector2> controlPoints = new List<Vector2>();

    readonly List<Vector2> controlPointFactors = new List<Vector2>()
    {
        new Vector2(-0.3f, 0.8f),
        new Vector2(0.1f, 1.4f),
    };
    #endregion

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < arrowNodeNum; i++)
        {
            // Nodeを生成して、リストに追加
            GameObject arrowNode = Instantiate(ArrowNodePrefab, transform);
            arrowNodes.Add(arrowNode.GetComponent<RectTransform>());
        }

        // Headを生成
        GameObject arrowHead = Instantiate(ArrowHeadPrefab, transform);
        arrowNodes.Add(arrowHead.GetComponent<RectTransform>());

        // 初期位置を設定(遠くに)
        arrowNodes.ForEach(node => node.GetComponent<RectTransform>().position = new Vector2(-1000, 1000));

        for (int i = 0; i < 4; i++)
        {
            controlPoints.Add(Vector2.zero);
        }

        SetColor(new Color32(100, 100, 100, 255));
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        controlPoints[0] = origin;
        controlPoints[3] = top;

        // 制御点の計算
        controlPoints[1] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[0];
        controlPoints[2] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[1];

        for (int i = 0; i < arrowNodes.Count; i++)
        {
            float t = (float)i / (this.arrowNodes.Count - 1);

            // 1. 位置の計算 (localPosition)
            this.arrowNodes[i].localPosition =
                Mathf.Pow(1 - t, 3) * controlPoints[0] +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1] +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2] +
                Mathf.Pow(t, 3) * controlPoints[3];

            // 2. 回転の計算 (localPositionベース)
            if (i > 0)
            {
                var diff = arrowNodes[i].localPosition - arrowNodes[i - 1].localPosition;
                if (diff != Vector3.zero) // ゼロ除算防止
                {
                    var angle = Vector2.SignedAngle(Vector2.up, diff);
                    arrowNodes[i].localRotation = Quaternion.Euler(0, 0, angle);
                }
            }

            // 3. スケールの計算
            var scale = scaleFactor * (1f - 0.03f * (arrowNodes.Count - 1 - i));
            arrowNodes[i].localScale = new Vector3(scale, scale, 1);
        }

        // 4. 根元の向きを2番目のノードに合わせる
        if (arrowNodes.Count > 1)
        {
            arrowNodes[0].localRotation = arrowNodes[1].localRotation;
        }
    }

    public void SetOriginPos(Vector3 pos)
    {
        origin = pos;
    }

    public void SetTopPos(Vector3 pos)
    {
        top = pos;
    }

    // 色を変える
    public void SetColor(Color color)
    {
        foreach (var node in arrowNodes)
        {
            node.GetComponent<Image>().color = color;
        }
    }
}