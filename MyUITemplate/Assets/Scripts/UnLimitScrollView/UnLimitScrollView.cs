/*
 * Create by JimmyChoi 2016/06/17
 * 这是一个无限Scroll的基础类
 * 现在只对应3页或者以上的scroll
 * 
 * 用法：
 * 这是一个无限Scroll的UI系统
 * 依赖iTween来做移动处理（当然也可以用其他，或者自己去写）
 * 把设置好的GameObject序列放进Init（）里，就会自动生成一个无限Scroll View
 * 
 * 基本原理：
 * 真正移动的只有左中右的Panel
 * Content原始状态 左(index = max) 中(index = 0) 右(index = 1)
 * 
 * 左 中(0, 0, 0) 右
 * 
 * 左滑动
 * 左（里）放回仓库，右（里）→ 中（里），中（里）→ 左（里），设定右（里），
 * 最后把content恢复到原始状态左 中(0, 0, 0) 右
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnLimitScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
 {
    public ScrollRect myScrollRect; // 自己的ScrollRect

    public RectTransform NodeRectTransformLeft; // 左边的Panel
    public RectTransform NodeRectTransformCenter; // 中间的Panel
    public RectTransform NodeRectTransformRight; // 右边的Panel

    public Transform NodeStack;

    private int currentNodeIndexLeft; // 现在处于左边panel Left里面Node的Index
    private int currentNodeIndexCenter; // 现在处于左边panel Center里面Node的Index
    private int currentNodeIndexRight; // 现在处于左边panel Right里面Node的Index

    [SerializeField]
    private List<GameObject> nodeGameObjectListForTest; // 测试用

    private List<GameObject> nodeGameObjectList = new List<GameObject>(); // 想在Scroll里显示的GameObject
    public List<GameObject> NodeGameObjectList { get { return nodeGameObjectList; } }

    public float CanSwipeDistance; // 滑动多大距离可以形成Swipe事件
    private Vector2 onBeginDragPostion; // 开始滑动时，手指的Position

    private bool onMove = false; // Scroll View正在滑动中的Flag
    private bool onDrag = false; // 正在滑动的Flag

	private bool onMoveDrag = false; // 正在滑动时Drag

    /// <summary>
    /// 滑动方向
    /// </summary>
    private enum SwipeDirection
    {
        Right,
        Left
    }

    private void Start()
    {
        // Test
        Init(nodeGameObjectListForTest);
    }

    /// <summary>
    /// 初始化,把设定好的GameObject序列放在里面就可以用了！
    /// </summary>
    /// <param name="gameObjectList"></param>
    public void Init(List<GameObject> gameObjectList)
    {
        for(int i = 0; i < gameObjectList.Count; ++i)
        {
            GameObject node = Instantiate(gameObjectList[i]) as GameObject;
            node.transform.SetParent(NodeStack); // 将Node都存放在Node仓库
            nodeGameObjectList.Add(node);
        }

        // 将前三个元素放到Scroll的三个JoinGameObject里面
        nodeGameObjectList[gameObjectList.Count - 1].transform.SetParent(NodeRectTransformLeft.transform);
        nodeGameObjectList[gameObjectList.Count - 1].transform.localPosition = Vector3.zero;

        nodeGameObjectList[0].transform.SetParent(NodeRectTransformCenter.transform);
        nodeGameObjectList[0].transform.localPosition = Vector3.zero;

        nodeGameObjectList[1].transform.SetParent(NodeRectTransformRight.transform);
        nodeGameObjectList[1].transform.localPosition = Vector3.zero;

        currentNodeIndexLeft = gameObjectList.Count - 1;
        currentNodeIndexCenter = 0;
        currentNodeIndexRight = 1;
    }

    /// <summary>
    /// 重置Scroll View里Content的位置
    /// </summary>
    private void resetContentPostion()
    {
        myScrollRect.content.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 开始滑动
    /// </summary>
    /// <param name="direction"></param>
    private void swipe(SwipeDirection direction)
    {
        onMove = true;
        myScrollRect.enabled = false;
        switch (direction)
        {
            case SwipeDirection.Right:
                rightSwipe();
                break;
            case SwipeDirection.Left:
                leftSwipe();
                break;
        }
    }

    /// <summary>
    /// 向右滑动
    /// </summary>
    private void rightSwipe()
    {
        iTween.MoveTo(myScrollRect.content.gameObject, iTween.Hash(
            "x", 336f,
            "time", 1.0f,
            "isLocal", true,                       
            "oncomplete", "moveOnCompleted",
            "oncompleteparams", SwipeDirection.Right,
            "oncompletetarget", gameObject
            ));
    }

    /// <summary>
    /// 向左滑动
    /// </summary>
    private void leftSwipe()
    {
        iTween.MoveTo(myScrollRect.content.gameObject, iTween.Hash(
            "x", -336f,
            "time", 1.0f,
            "islocal", true,
            "oncomplete", "moveOnCompleted",
            "oncompleteparams", SwipeDirection.Left,
            "oncompletetarget", gameObject
            ));
    }

    /// <summary>
    /// 滑动停止时的处理
    /// </summary>
    /// <param name="swipeDirection"></param>
    private void moveOnCompleted(SwipeDirection swipeDirection)
    {
        resetContentPostion();
        updateScrollViewNode(swipeDirection);
        
        onMove = false;
        myScrollRect.enabled = true;
    }

    /// <summary>
    /// 更新Scroll View Node的内容
    /// 例如：如果Swipe Right 中-> 右， 左 -> 中，右（更新：currentNodeIndexLeft - 1） -> 左 
    /// </summary>
    /// <param name="swipeDirection"></param>
    private void updateScrollViewNode(SwipeDirection swipeDirection)
    {
        switch (swipeDirection)
        {
            case SwipeDirection.Right:
                {
                    // 把右边的Node放回去NodeStack
                    nodeGameObjectList[currentNodeIndexRight].transform.SetParent(NodeStack);
                    nodeGameObjectList[currentNodeIndexRight].transform.localPosition = Vector3.zero;

                    // 移动中间，左边Node GameObject去右边一格
                    nodeGameObjectList[currentNodeIndexCenter].transform.SetParent(NodeRectTransformRight.transform);
                    nodeGameObjectList[currentNodeIndexCenter].transform.localPosition = Vector3.zero;

                    nodeGameObjectList[currentNodeIndexLeft].transform.SetParent(NodeRectTransformCenter.transform);
                    nodeGameObjectList[currentNodeIndexLeft].transform.localPosition = Vector3.zero;

                    // CurrentIndex更新
                    currentNodeIndexRight = currentNodeIndexCenter;
                    currentNodeIndexCenter = currentNodeIndexLeft;

                    // 设定左边的Node GameObject和Index
                    int nodeIndexLeft = 0;
                    if (currentNodeIndexLeft - 1 < 0)
                    {
                        nodeIndexLeft = nodeGameObjectList.Count - 1;
                    }
                    else
                    {
                        nodeIndexLeft = currentNodeIndexLeft - 1;
                    }
                    nodeGameObjectList[nodeIndexLeft].transform.SetParent(NodeRectTransformLeft.transform);
                    nodeGameObjectList[nodeIndexLeft].transform.localPosition = Vector3.zero;

                    currentNodeIndexLeft = nodeIndexLeft;
                }
                break;
            case SwipeDirection.Left:
                {
                    // 把左边的Node放回去NodeStack
                    nodeGameObjectList[currentNodeIndexLeft].transform.SetParent(NodeStack);
                    nodeGameObjectList[currentNodeIndexLeft].transform.localPosition = Vector3.zero;

                    // 移动中间，右边的Node GameObject去左边边一格
                    nodeGameObjectList[currentNodeIndexCenter].transform.SetParent(NodeRectTransformLeft.transform);
                    nodeGameObjectList[currentNodeIndexCenter].transform.localPosition = Vector3.zero;

                    nodeGameObjectList[currentNodeIndexRight].transform.SetParent(NodeRectTransformCenter.transform);
                    nodeGameObjectList[currentNodeIndexRight].transform.localPosition = Vector3.zero;

                    Debug.Log(nodeGameObjectList[currentNodeIndexRight].transform.parent);

                    // CurrentIndex更新
                    //int tempLeftIndex = currentNodeIndexCenter;
                    currentNodeIndexLeft = currentNodeIndexCenter;
                    currentNodeIndexCenter = currentNodeIndexRight;

                    // 设定右边的Node GameObject和Index
                    int nodeIndexRight = 0;
                    if (currentNodeIndexRight + 1 == nodeGameObjectList.Count)
                    {
                        nodeIndexRight = 0;
                    }
                    else
                    {
                        nodeIndexRight = currentNodeIndexRight + 1;
                    }
                    nodeGameObjectList[nodeIndexRight].transform.SetParent(NodeRectTransformRight.transform);
                    nodeGameObjectList[nodeIndexRight].transform.localPosition = Vector3.zero;
                    currentNodeIndexRight = nodeIndexRight;
                }
                break;
        }
    }

	/// <summary>
	/// 移動中Dragする時、スクロールビューがスクロールできないように
	/// </summary>
	private void FixedUpdate()
	{
		if (onMoveDrag == false)
		{
			myScrollRect.enabled = true;
		}
		else
		{
			myScrollRect.enabled = false;
		}
	}

    #region 判定event
    /// <summary>
    /// 开始滑动的判定
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        onDrag = true;

		if (this.onMove) {
		
			onDrag = false;
			onMoveDrag = true;
			return;
		}

        onBeginDragPostion = eventData.position;
    }

    /// <summary>
    /// 正在滑动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
    }

    /// <summary>
    /// 滑动结束
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
		if (onDrag) {
			Vector3 onEndDragtouchPos = eventData.position;

			var distance = onBeginDragPostion.x + onEndDragtouchPos.x;
			distance = distance > 0 ? distance : -distance;

			if (distance >= CanSwipeDistance) {
				Debug.Log ("Will Swipe");

				if (onBeginDragPostion.x > onEndDragtouchPos.x) {
					swipe (SwipeDirection.Left);
				} else {
					swipe (SwipeDirection.Right);
				}
			}
		}

        onDrag = false;
		onMoveDrag = false;
    }
    #endregion
}
