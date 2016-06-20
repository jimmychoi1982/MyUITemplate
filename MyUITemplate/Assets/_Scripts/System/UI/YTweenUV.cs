using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YTweenUV : YTween
{
    [SerializeField]
	private RawImage OutPut;

	[SerializeField]
	private Vector2 From;

	[SerializeField]
    private Vector2 To;

	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{

		if (Animate && OutPut!=null )
		{
			base.Update();

			float rate = Curve.Evaluate(TimeDelta / Duration);
            OutPut.uvRect = new Rect(Vector2.LerpUnclamped(From, To, rate), OutPut.uvRect.size);
		}
	}

    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        OutPut.uvRect = new Rect(Vector2.LerpUnclamped(From, To, rate), OutPut.uvRect.size);
    }

	// アニメーションを逆にする
	public override void ReverseTween()
	{
		base.ResetTween();

        Reverse();
	}

    /// <summary>
    /// アニメーションの値を逆するメソッド 2016 03 14
    /// </summary>
    public override void Reverse()
    {
        Vector2 Backup = From;
        From = To;
        To = Backup;
    }

    // Fromを現在のステータスにする
    protected override void StartNotFrom()
	{
		if (OutPut != null)
		{
			From = OutPut.uvRect.position;
		}
	}
}
