using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class YTween : AniMoveBase {

    public enum LoopType
    {
        Once,
        Loop,
        PingPong
    }

    [SerializeField]
    protected AnimationCurve Curve;

    [SerializeField]
    protected LoopType Loop;

    [SerializeField]
    protected float Delay = 0f;

    [SerializeField]
    protected float Duration = 1f;

    [SerializeField]
    protected int ID = 0;

    //  現在位置から開始する
    [SerializeField]
    protected bool StartThisPosition = false;

    protected float WaitingDelta;   // 待ち時間
    protected float TimeDelta;      // アニメーション経過時間

    protected bool Animate;         // アニメーション中

    protected bool PingPong;        // バウンド中

    /// <summary>
    /// イネーブル時のイベント内処理を行うかのフラグ 2016 03 16 kouno
    /// </summary>
    protected bool isOnEnableEvent = true;

    /// <summary>
    /// イネーブル時のイベント内処理を行うかのフラグ プロパティ 2016 03 16 kouno
    /// </summary>
    public bool IsOnEnableEvent { get { return isOnEnableEvent; } set { isOnEnableEvent = value; } }

    protected int ToCount;          // 複数アニメーション登録していたら現在の選択

    public UnityEvent FinishEvent = new UnityEvent();
    UnityEvent FinishEventFromOverMethod = new UnityEvent();

	/// <summary>イネーブル時のイベント</summary>
	protected virtual void OnEnable()
	{
        if(!isOnEnableEvent)
        {
            return;
        }

		ResetTween();
	}

	// Use this for initialization
	protected virtual void Start () {
        Animate = true;
        PingPong = false;

        if (StartThisPosition)
        {
            StartNotFrom();
        }
	}

    void OnDestroy()
    {
        if (FinishEvent != null)
        {
            FinishEvent.Invoke();
            FinishEvent.RemoveAllListeners();
            FinishEvent = null;
        }

        if (FinishEventFromOverMethod != null)
        {
            FinishEventFromOverMethod.Invoke();
            FinishEventFromOverMethod.RemoveAllListeners();
            FinishEventFromOverMethod = null;
        }

    }

	// Update is called once per frame
    protected virtual void Update()
    {
        if (Animate)
        {
            WaitingDelta += Time.deltaTime;
            if (WaitingDelta > Delay)
            {
                if (!PingPong)
                {
                    TimeDelta += Time.deltaTime;
                }
                else
                {
                    TimeDelta -= Time.deltaTime;
                }
            }
            if (TimeDelta >= Duration)
            {
                switch (Loop)
                {
                    case LoopType.Loop:
                        {
                            TimeDelta = 0.0f;
                            break;
                        }

                    case LoopType.Once:
                        {
                            TimeDelta = Duration;
                            Animate = false;
                            FinishEventAct();
                            break;
                        }
                    case LoopType.PingPong:
                        {
                            TimeDelta = Duration;
                            PingPong = true;
                            break;
                        }
                }
            }

            else if (TimeDelta <= 0.0f)
            {
                TimeDelta = 0.0f;
                PingPong = false;
            }
        }
	}

    // アニメーションをやり直す
    public virtual void ResetTween()
    {
        WaitingDelta = 0.0f;
        TimeDelta = 0.0f;
        Animate = true;
        PingPong = false;
        if (StartThisPosition)
        {
            StartNotFrom();
        }
    }

    // デリゲートを呼ぶ
    protected virtual void FinishEventAct()
    {
        if (FinishEvent != null)
        {
            FinishEvent.Invoke();
//            FinishEvent.RemoveAllListeners();
//            FinishEvent = null;
        }

        if (FinishEventFromOverMethod != null)
        {
            FinishEventFromOverMethod.Invoke();
            FinishEventFromOverMethod.RemoveAllListeners();
        }
    }

    // 指定されたIDといって指定たら再生
    public bool PlayFromID(int _id, UnityAction _ev = null)
    {
        if (ID == _id)
        {
            enabled = true;
            ResetTween();

            if (_ev != null &&
                Loop == LoopType.Once)
            {
                FinishEventFromOverMethod.AddListener(_ev);
                
                return true;
            }
            return false;
        }
        else
        {
//            enabled = false;
            return false;
        }
    }

	// 指定されたIDといって指定たら再生
	public bool PlayFromID_FalseDisable(int _id, UnityAction _ev = null)
	{
		if (ID == _id)
		{
			enabled = true;
			ResetTween();

			if (_ev != null &&
				Loop == LoopType.Once)
			{
				FinishEventFromOverMethod.AddListener(_ev);

				return true;
			}
			return false;
		}
		else
		{
			enabled = false;
			return false;
		}
	}

	protected virtual void StartNotFrom()
    {
    }

    public void DestroyCall()
    {
        Destroy(gameObject);
    }

	public void DisactiveCall()
	{
		this.gameObject.SetActive(false);
	}


    /// <summary>
    /// アニメーション中フラグの設定メソッド
    /// 初回のみ自動でアニメーション動作をしないようにする等の使用用途で追加　2016 03 11 kouno
    /// </summary>
    public void SetAnimate(bool isAnimate)
    {
        Animate = isAnimate;
    }

    /// <summary>
    /// 逆アニメーションメソッド
    /// </summary>
    public virtual void ReverseTween()
    {

    }

    /// <summary>
    /// アニメーションの値を逆するメソッド 2016 03 14
    /// </summary>
    public virtual void Reverse()
    {

    }

    /// <summary>指定のゲームオブジェクトのスケールを反転するメソッド</summary>
    public void TartgetReversalScale(Transform _transform)
    {
        _transform.localScale = -_transform.localScale;
    }

    /// <summary>指定のオブジェクトのアクティブ状態を反転するメソッド</summary>
    public void TargetReversalSetActive(GameObject _gameObject)
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }

    /// <summary>指定のMonoBehaviour.enabledを反転するメソッド</summary>
    public void TargetReversalMonoBehaviourEnabled(MonoBehaviour _monoBehaviour)
    {
        _monoBehaviour.enabled = !_monoBehaviour.enabled;
    }
}
