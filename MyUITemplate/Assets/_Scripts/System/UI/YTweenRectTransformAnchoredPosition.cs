// ===========================================================================
// YTweenRectTransformAnchoredPosition.cs 2016 05 17 masahiro kouno
// RectTransformAnchoredPositionを制御するスクリプト　
// 急ぎで合わせた捨てコードなので不具合があるかもしれません。
// ===========================================================================

using UnityEngine;
using System.Collections;

public class YTweenRectTransformAnchoredPosition : YTween
{
    [SerializeField]
    protected Vector2 from;

    [SerializeField]
    protected Vector2 to;

    bool originSaved = false;
    Vector3 originFrom;
    Vector3 originTo;

    //  相対座標として扱う
    [SerializeField]
    protected bool RelativePosition = false;

    public bool ignoreX = false;
    public bool ignoreY = false;


    protected override void OnEnable()
    {
        // 邪魔なので処理を消す
        //base.OnEnable();
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (Animate)
        {
            base.Update();

            float rate = Curve.Evaluate(TimeDelta / Duration);
            SetPosition(Vector2.LerpUnclamped(from, to, rate));
        }
    }
    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        Vector3 lerped = Vector2.LerpUnclamped(from, to, rate);
        SetPosition(lerped);
    }

    /// <summary>
    /// ローカル座標に実際に値を代入する
    /// </summary>
    void SetPosition(Vector2 _pos)
    {
        Vector2 oldPos = this.transform.localPosition;

        if (ignoreX == true)
        {
            _pos.x = oldPos.x;
        }
        if (ignoreY == true)
        {
            _pos.y = oldPos.y;
        }

        this.transform.localPosition = _pos;
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
        if (RelativePosition)
        {
            if (!originSaved)
            {
                originFrom = from;
                originTo = to;
                originSaved = true;
            }
            //  相対座標として扱う
            from = this.transform.localPosition + originFrom;
            to = this.transform.localPosition + originTo;
        }
        else
        {
            //  現在位置からスタート
            from = this.transform.localPosition;
        }
    }

    /// <summary></summary>
    public void FromStart()
    {
        //  現在位置からスタート
        from = this.transform.localPosition;
        ResetTween();
    }
}
