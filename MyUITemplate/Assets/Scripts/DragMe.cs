/*
 * Command by JimmyChoi 2016/06/17
 * 这是一个拖动icon的Script,请装备在想要拖动的icon GameObject上面
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool dragOnSurfaces = true;
	
	private Dictionary<int,GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
	private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

	public void OnBeginDrag(PointerEventData eventData)
	{
        // 找canvas，没有的话就什么都不用说了
		var canvas = FindInParents<Canvas>(gameObject);
		if (canvas == null)
			return;

        // 当开始拖动icon的时候，新生成一个icon的GameObject
		m_DraggingIcons[eventData.pointerId] = new GameObject("icon");

        // 设定icon的parent到canvas
		m_DraggingIcons[eventData.pointerId].transform.SetParent (canvas.transform, false);
        // 把拖动的icon放到画面最前面
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();
		
        // 追加Image component
		var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        // 设置icon的图片
        image.sprite = GetComponent<Image>().sprite;
        image.SetNativeSize();

        // The icon will be under the cursor.
        // We want it to be ignored by the event system.
        // 我们需要把icon弄到被event system忽视
        var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
		group.blocksRaycasts = false;

        // 设定m_DraggingPlanes：如果在拖动的画面上拖动的话 
        // m_DraggingPlanes的pointerId和本icon的pointerId相同
        if (dragOnSurfaces)
			m_DraggingPlanes[eventData.pointerId] = transform as RectTransform;
		else
			m_DraggingPlanes[eventData.pointerId]  = canvas.transform as RectTransform;
		
		SetDraggedPosition(eventData);
	}

    /// <summary>
    /// 被拖动的时候
    /// </summary>
    /// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData)
	{
		if (m_DraggingIcons[eventData.pointerId] != null)
			SetDraggedPosition(eventData); // 设定被拖动icon的位置
	}

    /// <summary>
    /// 设定被拖动的icon的位置和拖动点同步
    /// </summary>
    /// <param name="eventData"></param>
	private void SetDraggedPosition(PointerEventData eventData)
	{
        if (dragOnSurfaces &&
            eventData.pointerEnter != null &&
            eventData.pointerEnter.transform as RectTransform != null)
        {
            // 设定被拖动的平面
            m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;
        }
		
        // 将拖动的icon和拖动的手指位置同步
		var iconRectTransform = m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
            // 设定icon的位置，和手指、鼠标同步
			iconRectTransform.position = globalMousePos;

            // 设定icon的倾斜度和当前的画面一样
			iconRectTransform.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
		}
	}

    /// <summary>
    /// 拖动结束时的处理
    /// </summary>
    /// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
    {
        // 把自己从画面中移除
        if (m_DraggingIcons[eventData.pointerId] != null)
			Destroy(m_DraggingIcons[eventData.pointerId]);

		m_DraggingIcons[eventData.pointerId] = null;
	}

    /// <summary>
    /// 寻找Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetGameObject"></param>
    /// <returns></returns>
	static public T FindInParents<T>(GameObject targetGameObject) where T : Component
	{
		if (targetGameObject == null)
            return null;

        var targetComponent = targetGameObject.GetComponent<T>();

		if (targetComponent != null)
			return targetComponent;
		
		var myParent = targetGameObject.transform.parent;
		while (myParent != null && targetComponent == null)
		{
			targetComponent = myParent.gameObject.GetComponent<T>();
			myParent = myParent.parent;
		}
		return targetComponent;
	}
}
