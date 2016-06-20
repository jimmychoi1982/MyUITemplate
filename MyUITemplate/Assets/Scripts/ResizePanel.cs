/*
 * 拖动，以改变大小的按钮
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler {
	
    // 最小尺寸
	public Vector2 minSize = new Vector2 (100, 100);
    // 最大尺寸
    public Vector2 maxSize = new Vector2 (400, 400);
	
    [Header("需要拖动的窗口")]
	public RectTransform panelRectTransform;

    // 触屏的
	private Vector2 originalLocalPointerPosition;
    
    // 当前窗口的大小
	private Vector2 originalSizeDelta;
	
    /// <summary>
    /// 在被tap的时候
    /// </summary>
    /// <param name="data"></param>
	public void OnPointerDown (PointerEventData data) {

        // 获取元素现在的大小
        originalSizeDelta = panelRectTransform.sizeDelta;

        // UI元素的世界坐标和canvas的RectTrasform再加上UI摄像机，换算出元素在Canvas的2D坐标。
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out pos);
        originalLocalPointerPosition = pos;
	}
	
    /// <summary>
    /// 被拖动的时候
    /// </summary>
    /// <param name="data"></param>
	public void OnDrag (PointerEventData data) {
        
        // 如果连Panel都没有，就什么都不用说了
        if (panelRectTransform == null)
			return;
		
        // 取得拖动时触摸点的坐标
		Vector2 localPointerPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out localPointerPosition);

        // 计算出拖动时的坐标差，用来设定窗口大小
        Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
		
        // 利用拖动时的坐标差，计算大小差，从而设定大小
		Vector2 sizeDelta = originalSizeDelta + new Vector2 (offsetToOriginal.x, -offsetToOriginal.y);

        // 利用Mathf.Clamp来限制大小
        sizeDelta = new Vector2 (
			Mathf.Clamp (sizeDelta.x, minSize.x, maxSize.x),
			Mathf.Clamp (sizeDelta.y, minSize.y, maxSize.y)
		);
		
        // 拖动后的大小反应在窗口上
		panelRectTransform.sizeDelta = sizeDelta;
	}
}