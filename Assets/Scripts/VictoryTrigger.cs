using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject victoryPanel; // Ссылка на UI Panel с сообщением о победе
    public Transform lobbyPosition; // Позиция лобби для телепортации
    public Transform player; // Ссылка на игрока
    public float fadeDuration = 1f; // Длительность появления/исчезновения панели
    public float displayDuration = 5f; // Время отображения панели
    public XRRayInteractor m_RayInteractor_left; // Левый луч
    public XRRayInteractor m_RayInteractor_right; // Правый луч
    public GameObject trophyPrefab; // Префаб трофея
    public InteractionManager interactionManager; // Ссылка на InteractionManager
    public Animator sphereAnimator;
    public MusicManager musicManager;
    public AudioSource soundEffect;

    private Jumpability jumpAbility;
    private CanvasGroup victoryCanvasGroup; // Для управления прозрачностью панели

    private void Start()
    {
        jumpAbility = player.GetComponent<Jumpability>();
        // Получаем компонент CanvasGroup для управления прозрачностью
        if (victoryPanel != null)
        {
            victoryCanvasGroup = victoryPanel.GetComponent<CanvasGroup>();
            if (victoryCanvasGroup == null)
            {
                victoryCanvasGroup = victoryPanel.AddComponent<CanvasGroup>();
            }
            victoryCanvasGroup.alpha = 0; // Начинаем с полностью прозрачной панели
            victoryPanel.SetActive(false); // Скрываем панель в начале
        }
        else
        {
            Debug.LogError("Victory Panel не назначен в VictoryTrigger скрипте!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошли именно мячом
        if (other.gameObject.CompareTag("Ball")) // Или: other.gameObject.name == "Ball"
        {
            // Запускаем корутину для отображения панели победы и последующих действий
            StartCoroutine(ShowVictoryAndTeleport(other.gameObject));
        }
    }

    private IEnumerator ShowVictoryAndTeleport(GameObject ball)
    {
        // Останавливаем мяч
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.linearVelocity = Vector3.zero;
            ballRigidbody.isKinematic = true;
            if (sphereAnimator != null) {
            sphereAnimator.speed = 0f;
            }
        }
            else
        {
            Debug.LogError("Rigidbody на мяче не назначен или SphereAnimationController не назначен в VictoryTrigger скрипте!");
        }
        if (soundEffect!=null) {
        musicManager.ActivateSoundEffect(soundEffect);
        }
        // Показываем панель победы с анимацией появления
        victoryPanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(victoryCanvasGroup, 0, 1, fadeDuration)); // Появление

        // Ждем указанное время (минус время затухания)
        yield return new WaitForSeconds(displayDuration - fadeDuration * 2);

        // Скрываем панель победы с анимацией исчезновения
        yield return StartCoroutine(FadeCanvasGroup(victoryCanvasGroup, 1, 0, fadeDuration)); // Исчезновение
        victoryPanel.SetActive(false);

        // Телепортируем игрока в лобби
        if (player != null && lobbyPosition != null && jumpAbility != null)
        {
            
            jumpAbility.enabled = false;
            player.position = lobbyPosition.position;
        }
        else
        {
            Debug.LogError("Player, Lobby Position или JumpAbility не назначены в VictoryTrigger скрипте!");
        }

        StartCoroutine(TeleportAndCheck(lobbyPosition, () =>
        {
            // Включаем Jumpability после успешной телепортации
            
            jumpAbility.enabled = true;
            

            // Включаем лучи
            
            m_RayInteractor_left.gameObject.SetActive(true);
            m_RayInteractor_right.gameObject.SetActive(true);

            if (musicManager != null) {
                musicManager.SwitchToLobbyMusic();
            }

            // Создаем трофей
            if (interactionManager != null && trophyPrefab != null)
            {
                interactionManager.CreateObj(trophyPrefab);
                Destroy(interactionManager);
            }
            
            
        }));

        // Удаляем все мячи
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject b in balls)
        {
            Destroy(b);
        }
    }

     private IEnumerator TeleportAndCheck(Transform targetPosition, System.Action onComplete)
    {
        // Телепортируем игрока
        player.position = targetPosition.position;
        

        // Ждем один кадр, чтобы Unity успел обновить позицию
        yield return null;

        // Проверяем, находится ли игрок в целевой позиции
        for (int i = 0; i < 10; i++)
        {
            if (Vector3.Distance(player.position, targetPosition.position) > 1f)
            {
                Debug.Log("Player is not at the target position. Retrying...");
                player.position = targetPosition.position;
                yield return null; // Ждем следующий кадр
            }
            else
            {
                
                Debug.Log("Teleported successfully!");
                break;
            }
        }

        onComplete?.Invoke();

        
    }

    // Корутина для плавного изменения прозрачности CanvasGroup
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha; // Убедимся, что значение точно установлено
    }
}

