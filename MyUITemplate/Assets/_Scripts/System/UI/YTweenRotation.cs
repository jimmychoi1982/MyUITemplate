using UnityEngine;
using System.Collections;

public class YTweenRotation : YTween
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

	// Update is called once per frame
	protected override void Update () {

        if (Animate)
        {
            base.Update();

            float rate = Curve.Evaluate(TimeDelta / Duration);
            Trans.localRotation = Quaternion.Euler(Mathf.Lerp(From.x, To.x, rate),
                Mathf.Lerp(From.y, To.y, rate),
                Mathf.Lerp(From.z, To.z, rate));
        }
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
        From = Trans.localRotation.eulerAngles;
    }


    public void SetRotationFrom(Vector3 _from)
    {
        From = _from;
    }

    public void SetRotationTo(Vector3 _to)
    {
        To = _to;
    }

}
