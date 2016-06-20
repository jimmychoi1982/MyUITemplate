using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YTweenCanvasGroup : YTween
{
	protected CanvasGroup OutPut;

	[SerializeField]
	private float From;

	[SerializeField]
	private float To;

	protected override void Start()
	{
		base.Start();
		OutPut = GetComponent<CanvasGroup>();
	}

	// Update is called once per frame
	protected override void Update()
	{

		if (Animate && OutPut != null)
		{
			base.Update();

			float rate = Curve.Evaluate(TimeDelta / Duration);
			OutPut.alpha = Mathf.Clamp01(Mathf.Lerp(From, To, rate));
		}
	}

    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        if (OutPut != null)
        {
            OutPut.alpha = Mathf.Clamp01(Mathf.Lerp(From, To, rate));
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(Mathf.Lerp(From, To, rate));
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
			From = OutPut.alpha;
		}
	}

    /// <summary>
    /// アニメーションの値を逆するメソッド 2016 03 14
    /// </summary>
    public override void Reverse()
    {
        float Backup = From;
        From = To;
        To = Backup;
    }
}
