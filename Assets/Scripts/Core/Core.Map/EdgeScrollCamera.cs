using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeScrollCamera : MonoBehaviour
{

    public float ScrollSpeed = 5f;
    public int BorderThickness = 20; // 画面端から何ピクセル以内で反応するか
    public int TopBarHeight = 50;
    public Vector2 MinPosition = new Vector2(0, 0);
    public Vector2 MaxPosition = new Vector2(0, 20); // マップに合わせて調整

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 pos = transform.position;

        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x >= Screen.width - BorderThickness)
            pos.x += ScrollSpeed * Time.deltaTime;
        if (mousePos.x <= BorderThickness)
            pos.x -= ScrollSpeed * Time.deltaTime;
        if (mousePos.y >= Screen.height - BorderThickness - TopBarHeight)
            pos.y += ScrollSpeed * Time.deltaTime;
        if (mousePos.y <= BorderThickness)
            pos.y -= ScrollSpeed * Time.deltaTime;

        // 範囲内に制限
        pos.x = Mathf.Clamp(pos.x, MinPosition.x, MaxPosition.x);
        pos.y = Mathf.Clamp(pos.y, MinPosition.y, MaxPosition.y);

        transform.position = pos;
    }
}
