﻿using UnityEngine;
using System.Collections;

public class YTweenScale : YTween
{
    public Transform Trans
    {
        get
        {
            if (__Trans == null) __Trans = this.transform;
            return __Trans;
        }
    }
    private Transform __Trans;

    [SerializeField]
    private Vector3 From;

    [SerializeField]
    private Vector3 To;

    [SerializeField]
    private Vector3[] ToAfter;

	// Update is called once per frame
	protected override void Update () {

        if (Animate)
        {
            base.Update();

            float rate = Curve.Evaluate(TimeDelta / Duration);
            Trans.localScale = Vector3.LerpUnclamped(From, To, rate);
        }
	}

    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        Trans.localScale = Vector3.LerpUnclamped(From, To, rate);
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
        From = Trans.localScale;
    }
}
