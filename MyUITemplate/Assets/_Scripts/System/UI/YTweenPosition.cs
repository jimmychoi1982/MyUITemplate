using UnityEngine;
using System.Collections;

public class YTweenPosition : YTween
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
    protected Vector3 From;

    [SerializeField]
	protected Vector3 To;

    bool originSaved = false;
    Vector3 originFrom;
    Vector3 originTo;

    //  相対座標として扱う
    [SerializeField]
    protected bool RelativePosition = false;

    public bool ignoreX = false;
    public bool ignoreY = false;
    public bool ignoreZ = false;


    // Update is called once per frame
    protected override void Update () {

        if (Animate)
        {
            base.Update();

            float rate = Curve.Evaluate(TimeDelta / Duration);
            SetPosition(Vector3.LerpUnclamped(From, To, rate));
        }
	}
    // アニメーションをやり直す
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        Vector3 lerped = Vector3.LerpUnclamped(From, To, rate);
        SetPosition(lerped);
    }

    /// <summary>
    /// ローカル座標に実際に値を代入する
    /// </summary>
    void SetPosition(Vector3 _pos)
    {
        Vector3 oldPos = Trans.localPosition;

        if (ignoreX == true)
        {
            _pos.x = oldPos.x;
        }
        if (ignoreY == true)
        {
            _pos.y = oldPos.y;
        }
        if (ignoreZ == true)
        {
            _pos.z = oldPos.z;
        }
        Trans.localPosition = _pos;
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
        if (RelativePosition)
        {
            if (!originSaved)
            {
                originFrom = From;
                originTo = To;
                originSaved = true;
            }
            //  相対座標として扱う
            From = Trans.localPosition + originFrom;
            To = Trans.localPosition + originTo;
        }
        else
        {
            //  現在位置からスタート
            From = Trans.localPosition;
        }

    }
}
