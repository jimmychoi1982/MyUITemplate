// ========================================================================
// YTweenRectTransformSizeDelta.cs 2016 03 10 masahiro kouno
// ターゲットのRectTransformのsizeDeltaアニメーション設定で値を変化させるクラス
// ========================================================================
using UnityEngine;
using System.Collections;

[System.Serializable]
public class YTweenRectTransformSizeDelta : YTween
{
    /// <summary>
    /// ターゲットのRectTransformの参照　必ずインスペクター設定してください。
    /// this.Transformでトランスフォームを取得できてもRectTransformはキャストまたは
    /// GetComponentしたくてはならないのでインスペクター設定が無難
    /// </summary>
    [SerializeField]
    private RectTransform targetTransform = null;

    [SerializeField]
    protected Vector2 from;

    [SerializeField]
    protected Vector2 to;
	
	// Update is called once per frame
	protected override void Update ()
    {
        if (Animate)
        {
            base.Update();

            float rate = Curve.Evaluate(TimeDelta / Duration);
            targetTransform.sizeDelta = Vector2.LerpUnclamped(from, to, rate);
        }
    }

    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        targetTransform.sizeDelta = Vector2.LerpUnclamped(from, to, rate);
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
        Vector2 Backup = from;
        from = to;
        to = Backup;
    }

    // Fromを現在のステータスにする
    protected override void StartNotFrom()
    {
        from = targetTransform.sizeDelta;
    }
}
