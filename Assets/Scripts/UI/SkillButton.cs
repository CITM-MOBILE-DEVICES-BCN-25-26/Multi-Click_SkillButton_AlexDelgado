using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public enum Phase
    {
        Pressed,
        Released,
        Canceled
    }

    public event Action<Phase> ButtonPhaseChanged;
    
    private bool isButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
        ButtonPhaseChanged?.Invoke(Phase.Pressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isButtonPressed)
        {
            return;
        }

        isButtonPressed = false;
        ButtonPhaseChanged?.Invoke(Phase.Released);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isButtonPressed)
        {
            return;
        }

        isButtonPressed = false;
        ButtonPhaseChanged?.Invoke(Phase.Canceled);
    }

    private void OnDisable()
    {
        isButtonPressed = false;
    }
}
