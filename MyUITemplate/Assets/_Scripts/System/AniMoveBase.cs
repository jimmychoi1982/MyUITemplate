using UnityEngine;
using System.Collections;

public delegate void AniMoveOpenCallBack();
public delegate void AniMoveCloseCallback();

public class AniMoveBase : MonoBehaviour
{
	[System.NonSerialized]
	public AniMoveOpenCallBack openCallback;
	[System.NonSerialized]
	public AniMoveCloseCallback closeCallback;


	[System.NonSerialized]
	public bool isAnimFlg = false;

	public bool IsAnim()
	{
		return isAnimFlg;
	}

	public virtual void Open(AniMoveOpenCallBack _callback = null)
	{
		openCallback = _callback;
	}
	public virtual void Close(AniMoveCloseCallback _callback = null)
	{
		closeCallback = _callback;
	}
}
