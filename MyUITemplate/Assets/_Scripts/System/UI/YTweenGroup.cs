using UnityEngine;
using System.Collections;

public class YTweenGroup : MonoBehaviour {

    YTween[] YTweens;

    public delegate void FinishEvent(); // アニメーション終了時に呼び出すデリゲート
    public event FinishEvent Finishes;

    int MovingCount;
    int FinishCount;

	[HideInInspector]
	public bool Animate = true;

	// Use this for initialization
	void OnEnable () {
        YTweens = GetComponentsInChildren<YTween>(true);
        TweenPlayStart();
	}

    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        if (Finishes != null)
        {

            Finishes = null;
        }
    }
    // IDを指定して保存されているTweenを選択
    public void TweenPlaySelect(int _ID,FinishEvent _ev)
    {
        MovingCount = 0;
        FinishCount = 0;
        Finishes += _ev;
        if (YTweens.Length > 0)
        {
            foreach (YTween Tween in YTweens)
            {
                if (_ev != null)
                {
                    if (Tween.PlayFromID(_ID, PartsFinished))
                    {
                        MovingCount++;
                    }
                }
                else
                {
                    Tween.PlayFromID(_ID);
                }
            }
        }
    }

	// IDを指定して保存されているTweenを選択(Inspector用)
	public void TweenPlaySelect(int _ID)
	{
		MovingCount = 0;
		FinishCount = 0;
		if (YTweens.Length > 0)
		{
			foreach (YTween Tween in YTweens)
			{

				Tween.PlayFromID(_ID);

			}
		}
	}

	// IDを指定して保存されているTweenを選択(Inspector用)
	public void TweenPlaySelect_FalseDisable(int _ID)
	{
		MovingCount = 0;
		FinishCount = 0;
		if (YTweens.Length > 0)
		{
			foreach (YTween Tween in YTweens)
			{

				Tween.PlayFromID_FalseDisable(_ID);

			}
		}
	}

	// Tweenの再アクティブ化時の再生処理
    void TweenPlayStart()
    {
		Animate = true;
        MovingCount = 0;
        FinishCount = 0;
        if (YTweens.Length > 0)
        {
            foreach (YTween Tween in YTweens)
            {
                Tween.enabled = false;
                Tween.ResetTween();
                Tween.PlayFromID(0);

            }
        }
    }

	public void TweenPlay()
	{
		Animate = true;
		if (YTweens.Length > 0)
		{
			foreach (YTween Tween in YTweens)
			{
				Tween.SetAnimate(true);
			}
		}
	}
	public void TweenStop()
	{
		Animate = false;
		if (YTweens.Length > 0)
		{
			foreach (YTween Tween in YTweens)
			{
				Tween.SetAnimate(false);
			}
		}
	}

    public void EventInvoke()
    {
        if (Finishes != null)
        {

            Finishes();
            Finishes = null;
        }
    }

    public void PartsFinished()
    {
        FinishCount++;
        if (FinishCount >= MovingCount)
        {
            EventInvoke();
        }
    }

    public void DestroyCall()
    {
        Destroy(gameObject);
    }
}
