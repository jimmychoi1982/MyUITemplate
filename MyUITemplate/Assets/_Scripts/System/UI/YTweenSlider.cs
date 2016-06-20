// ===================================================================================
// YTweenSlider.cs 2016 masahiro kouno
// スライダー対応バージョンに拡張
// ===================================================================================
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>スライダー用のYTweenスクリプト</summary>
public class YTweenSlider : YTween
{
    /// <summary>ターゲットとなるスライダー参照　必ずインスペクター設定をして下さい。</summary>
    [SerializeField]
    private Slider targetSlider = null;

    /// <summary>ターゲットとなるスライダー参照　プロパティ</summary>
    public Slider TargetSlider
    {
        get
        {
            return targetSlider;
        }
    }

    /// <summary>スライダーの値を表示するテキスト参照　必要であればインスペクター設定してください。</summary>
    [SerializeField]
    private Text targetText = null;

    /// <summary>スライダーの値を表示するテキスト参照 プロパティ</summary>
    public Text TargetText
    {
        get
        {
            return targetText;
        }
    }

    /// <summary>
    /// テキストに表示する浮動小数点数
    /// ０の場合はintで設定されますが、浮動小数点を使用する場合は
    /// string.Formatが使用されます。
    /// </summary>
    [SerializeField]
    private int textFloatingPointNumber = 0;

    /// <summary>テキストに表示する浮動小数点数 プロパティ</summary>
    public int TextFloatingPointNumber
    {
        get
        {
            return textFloatingPointNumber;
        }

        set
        {
            textFloatingPointNumber = value;
        }
    }

    /// <summary>スライダーの現在の値</summary>
    protected float From
    {
        get
        {
            return targetSlider.value;
        }
    }

    /// <summary>目標の値</summary>
    [SerializeField]
    private float to = 0.0f;

    /// <summary>目標の値　プロパティ</summary>
    protected float To
    {
        get
        {
            return to;
        }
        set
        {
            to = value;
        }
    }


	/// <summary>初期化メソッド</summary>
    protected override void Start ()
    {
	    if(targetSlider == null)
        {
            Debug.LogError(this.name + " : ターゲットスライダーがnullです。");
        }
	}

    /// <summary>更新メソッド</summary>
    // Update is called once per frame
    protected override void Update()
    {
        if (Animate)
        {
            base.Update();
            float rate = Curve.Evaluate(TimeDelta / Duration);
            targetSlider.value = Mathf.Lerp(From, To, rate);

            if(targetText != null)
            {
                if (textFloatingPointNumber == 0)
                {
                    targetText.text = ((int)targetSlider.value).ToString();
                }
                else
                {
                    string resultFloatingPointNumber = "";

                    for (int i = 0; i < textFloatingPointNumber; i++)
                    {
                        resultFloatingPointNumber += "0";
                    }

                    targetText.text = string.Format("{0:0.0" + resultFloatingPointNumber + "}\r", targetSlider.value);
                }
            }
        }
    }

    /// <summary>アニメーションをやり直すメソッド</summary>
    public override void ResetTween()
    {
        base.ResetTween();
        float rate = Curve.Evaluate(0);
        targetSlider.value = Mathf.Lerp(From, To, rate);
    }

    /// <summary>
    /// toの設定及びアニメーションのやり直しするメソッド
    /// </summary>
    public void SetTo_and_ResetTween(float _to)
    {
        if(to != _to)
        {
            to = _to;
        }

        ResetTween();
    }
}
