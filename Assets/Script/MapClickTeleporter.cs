using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapClickTeleporter : MonoBehaviour, IPointerClickHandler
{
    public Camera mapCamera; // CameraOpenMap
    public RenderTexture renderTexture;

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Chuyển tọa độ chuột sang tọa độ UV (0-1)
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            return;

        Vector2 size = rectTransform.rect.size;
        Vector2 normalizedPoint = (localPoint + size * 0.5f) / size;

        // Chuyển UV sang pixel trong RenderTexture
        int texX = Mathf.RoundToInt(normalizedPoint.x * renderTexture.width);
        int texY = Mathf.RoundToInt(normalizedPoint.y * renderTexture.height);

        // Bắn ray từ mapCamera theo tọa độ đó
        Ray ray = mapCamera.ScreenPointToRay(new Vector3(texX, texY, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tele tele = hit.collider.GetComponent<Tele>();
            if (tele != null)
            {
                tele.ShowButtonTele();
            }
        }
    }
}
