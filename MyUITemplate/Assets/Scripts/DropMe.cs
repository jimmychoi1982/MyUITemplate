/*
 * 这是一个接受拖动icon的Script
 * 该类是装备在拖动平面的Drop Area上面，
 */
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image containerImage; // Drop框框的图片，初始值apha设定为· 50%
	public Image receivingImage; // 接受框的图片
	private Color normalColor; // 一般状态的颜色，半透明
	public Color highlightColor = Color.yellow; // 如果icon和接受框框交错的时候的颜色，Highline
	
    /// <summary>
    /// 在被激活的时候
    /// </summary>
	public void OnEnable ()
	{
		if (containerImage != null)
            // 一般时候的颜色设定位半透明
			normalColor = containerImage.color;
	}
	
    /// <summary>
    /// 产生Drop动作的时候
    /// </summary>
    /// <param name="data"></param>
	public void OnDrop(PointerEventData data)
	{
        // 框框变回半透明
		containerImage.color = normalColor;
		
        // error对应
		if (receivingImage == null)
			return;
		
        // 设定图片为icon的图片
		Sprite dropSprite = GetDropSprite (data);
		if (dropSprite != null)
			receivingImage.overrideSprite = dropSprite;
	}

    /// <summary>
    /// icon与Drop Area接触画面的时候
    /// </summary>
    /// <param name="data"></param>
	public void OnPointerEnter(PointerEventData data)
	{
        // error对应
		if (containerImage == null)
			return;
		
        // 框框显示Highline颜色
		Sprite dropSprite = GetDropSprite (data);
		if (dropSprite != null)
			containerImage.color = highlightColor;
	}

    /// <summary>
    /// icon离开Drop Area时候
    /// </summary>
    /// <param name="data"></param>
	public void OnPointerExit(PointerEventData data)
	{
        // 错误对应
		if (containerImage == null)
			return;
		
        // 框框颜色回复初始值，半透明
		containerImage.color = normalColor;
	}
	
    /// <summary>
    /// 获得Drop的图片Sprite
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
	private Sprite GetDropSprite(PointerEventData data)
	{
        // 获得拖动状态的GameObject
		var originalObj = data.pointerDrag;
		if (originalObj == null)
			return null;

		// 获取拖动状态GameObject的DragMe Script
		var dragMe = originalObj.GetComponent<DragMe>();
		if (dragMe == null) // 没有这个Component的话就什么都不用说了
			return null;
		
        // 获得拖动状态GameObject的Image
		var srcImage = originalObj.GetComponent<Image>();
		if (srcImage == null) // 没有这个Component的话就什么都不用说了
            return null;
		
        // 返回这个图片的Sprite
		return srcImage.sprite;
	}
}
