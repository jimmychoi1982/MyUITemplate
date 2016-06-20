using UnityEngine;
using System.Collections;

/// <summary>YTweenPosition の拡張Tween</summary>
public class YTweenPositionExt : YTweenPosition
{

	/// <summary>From座標の設定</summary>
	public void SetPositionFrom(Vector3 _vec)
	{
		From = _vec;
	}

	/// <summary>To座標の設定</summary>
	public void SetPositionTo(Vector3 _vec)
	{
		To = _vec;
	}

	/// <summary>所要時間の設定</summary>
	public void SetDuration(float _dur)
	{
		Duration = _dur;
	}

	/// <summary>待機時間の設定</summary>
	public void SetDelay(float _del)
	{
		Delay = _del;
	}

}
