using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private enum ClickState
    {
        Idle,
        Pressed,
        WaitingSecondClick
    }

    [SerializeField, Min(1)] private int longPressThresholdMs = 500;
    [SerializeField, Min(1)] private int doubleClickWindowMs = 250;

    private SkillButton skillButton;
    private ClickState state = ClickState.Idle;
    private int clickCount;
    private CancellationTokenSource activeTimerCancellationTokenSource;

    private TimeSpan LongPressThreshold => TimeSpan.FromMilliseconds(longPressThresholdMs);
    private TimeSpan DoubleClickWindow => TimeSpan.FromMilliseconds(doubleClickWindowMs);

    public void Initialize(SkillButton sb)
    {
        skillButton = sb;

        skillButton.ButtonPhaseChanged += OnButtonPhaseChanged;
    }
    
    private void OnDestroy()
    {
        if (skillButton != null)
        {
            skillButton.ButtonPhaseChanged -= OnButtonPhaseChanged;
        }

        Reset();
    }

    private void OnButtonPhaseChanged(SkillButton.Phase phase)
    {
        switch (phase)
        {
            case SkillButton.Phase.Pressed:
                HandlePressed();
                break;
            case SkillButton.Phase.Released:
                HandleReleased();
                break;
            case SkillButton.Phase.Canceled:
                Reset();
                break;
        }
    }

    private void HandlePressed()
    {
        if (state == ClickState.Pressed)
        {
            return;
        }
        
        clickCount = state == ClickState.Idle ? 1 : 2;
        state = ClickState.Pressed;
        CancelActiveTimer();
        activeTimerCancellationTokenSource = new CancellationTokenSource();
        LongPress(activeTimerCancellationTokenSource.Token).Forget();
    }

    private void HandleReleased()
    {
        if (state != ClickState.Pressed)
        {
            return;
        }

        CancelActiveTimer();

        if (clickCount == 2)
        {
            Debug.Log("Double Click");
            Reset();
            return;
        }

        state = ClickState.WaitingSecondClick;
        activeTimerCancellationTokenSource = new CancellationTokenSource();
        SecondClickWindow(activeTimerCancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid LongPress(CancellationToken cancellationToken)
    {
        bool isCanceled = await UniTask.Delay(LongPressThreshold, cancellationToken: cancellationToken)
            .SuppressCancellationThrow();

        if (isCanceled || state != ClickState.Pressed)
        {
            return;
        }

        Debug.Log("Long Press");
        Reset();
    }

    private async UniTaskVoid SecondClickWindow(CancellationToken cancellationToken)
    {
        bool isCanceled = await UniTask.Delay(DoubleClickWindow, cancellationToken: cancellationToken)
            .SuppressCancellationThrow();

        if (isCanceled || state != ClickState.WaitingSecondClick || clickCount != 1)
        {
            return;
        }

        Debug.Log("Short Click");
        Reset();
    }

    private void Reset()
    {
        state = ClickState.Idle;
        clickCount = 0;
        CancelActiveTimer();
    }

    private void CancelActiveTimer()
    {
        if (activeTimerCancellationTokenSource == null)
        {
            return;
        }

        if (!activeTimerCancellationTokenSource.IsCancellationRequested)
        {
            activeTimerCancellationTokenSource.Cancel();
        }

        activeTimerCancellationTokenSource.Dispose();
        activeTimerCancellationTokenSource = null;
    }
}
