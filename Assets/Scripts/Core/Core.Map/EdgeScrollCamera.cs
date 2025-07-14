using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeScrollCamera : MonoBehaviour
{

    public float scrollSpeed = 5f;
    public int borderThickness = 20; // 画面端から何ピクセル以内で反応するか
    public int topBarHeight = 50;
    public Vector2 minPosition = new Vector2(0, 0);
    public Vector2 maxPosition = new Vector2(0, 20); // マップに合わせて調整

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 pos = transform.position;

        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x >= Screen.width - borderThickness)
            pos.x += scrollSpeed * Time.deltaTime;
        if (mousePos.x <= borderThickness)
            pos.x -= scrollSpeed * Time.deltaTime;
        if (mousePos.y >= Screen.height - borderThickness - topBarHeight)
            pos.y += scrollSpeed * Time.deltaTime;
        if (mousePos.y <= borderThickness)
            pos.y -= scrollSpeed * Time.deltaTime;

        // 範囲内に制限
        pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);

        transform.position = pos;
    }
}
