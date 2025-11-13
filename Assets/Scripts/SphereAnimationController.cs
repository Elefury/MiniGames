using UnityEngine;

public class SphereAnimationController : MonoBehaviour
{
    private Animator sphereAnimator; // Ссылка на Animator сферы
    private bool isMoving = false; // Состояние анимации

    private void Start()
    {
        // Получаем компонент Animator
        //sphereAnimator = GetComponent<Animator>();

        //if (sphereAnimator == null)
        //{
        //    Debug.LogError("Animator component not found on the sphere!");
        //}
        //else
        //{
            // Устанавливаем начальное состояние анимации
        //    SetAnimationState(isMoving);
        //}
    }

    // Метод для включения/выключения анимации
    public void SetAnimationState(bool state)
    {
        isMoving = state;
        if (sphereAnimator != null)
        {
            sphereAnimator.SetBool("IsMoving", isMoving);
        }
    }

    // Метод для остановки анимации
    public void StopAnimation()
    {
        SetAnimationState(false);
    }

    // Метод для запуска анимации
    public void StartAnimation()
    {
        SetAnimationState(true);
    }
}
