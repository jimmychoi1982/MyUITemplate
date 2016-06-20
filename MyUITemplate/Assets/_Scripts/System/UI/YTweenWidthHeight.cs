using UnityEngine;
using System.Collections;

public class YTweenWidthHeight : YTween
{
    public RectTransform Trans
    {
        get
        {
            if (__Trans == null)
            {
                __Trans = this.GetComponent<RectTransform>();
                if (null == __Trans)
                {
                    Debug.LogError("This object has no RectTransform");
                }
            }
            return __Trans;
        }
    }
    private RectTransform __Trans;

    [SerializeField]
    private Vector2 From;

    [SerializeField]
    private Vector2 To;

	// Update is called once per frame
	protected override void Update () {

        if (Animate)
        {
            base.Update();

            float rate = Curve.Evaluate(TimeDelta / Duration);
            Trans.sizeDelta = Vector2.LerpUnclamped(From, To, rate);
        }
	}

    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        Trans.sizeDelta = Vector2.LerpUnclamped(From, To, rate);
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
        Vector3 Backup = From;
        From = To;
        To = Backup;
    }

    // Fromを現在のステータスにする
    protected override void StartNotFrom()
    {
        From = Trans.sizeDelta;
    }
}
