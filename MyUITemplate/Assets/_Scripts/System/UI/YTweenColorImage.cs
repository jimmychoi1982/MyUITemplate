﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YTweenColorImage : YTween
{
    public Image OutPut;

    [SerializeField]
    private Color From;

    [SerializeField]
    private Color To;
	
	// Update is called once per frame
	protected override void Update () {

        if (Animate)
        {
            base.Update();

            float rate = Curve.Evaluate(TimeDelta / Duration);
            OutPut.color = Color.LerpUnclamped(From, To, rate);
        }
	}

    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        OutPut.color = Color.LerpUnclamped(From, To, rate);
    }
    
    // アニメーションを逆にする
    public override void ReverseTween()
    {
        base.ResetTween();

        Color Backup = From;
        From = To;
        To = Backup;
    }

    // Fromを現在のステータスにする
    protected override void StartNotFrom()
    {
        From = OutPut.color;
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
}
