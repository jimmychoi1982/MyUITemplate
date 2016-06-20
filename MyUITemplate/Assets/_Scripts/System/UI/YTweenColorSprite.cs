using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YTweenColorSprite : YTween
{
	protected SpriteRenderer OutPut;

	[SerializeField]
	private Color From;

	[SerializeField]
	private Color To;

	protected override void Start()
	{
		base.Start();
		OutPut = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	protected override void Update()
	{

		if (Animate && OutPut!=null )
		{
			base.Update();

			float rate = Curve.Evaluate(TimeDelta / Duration);
			OutPut.color = Color.Lerp(From, To, rate);
		}
	}

    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        if (OutPut != null)
        {

			OutPut.color = Color.Lerp(From, To, rate);
        }
        else
        {
			OutPut = GetComponent<SpriteRenderer>();
			OutPut.color = Color.Lerp(From, To, rate);
        }
    }

	// アニメーションを逆にする
	public override void ReverseTween()
	{
		base.ResetTween();

        Reverse();
	}

	// Fromを現在のステータスにする
	protected override void StartNotFrom()
	{
		if (OutPut != null)
		{
			From = OutPut.color;
		}
	}

    /// <summary>
    /// アニメーションの値を逆するメソッド 2016 03 14
    /// </summary>
    public override void Reverse()
    {
        Color Backup = From;
        From = To;
        To = Backup;
    }

	/// <summary>
	/// 色変更
	/// </summary>
	public void SetColorFrom(Color _col)
	{
		From = _col;
	}

	/// <summary>
	/// 色変更
	/// </summary>
	public void SetColorTo(Color _col)
	{
		To = _col;
	}

}
