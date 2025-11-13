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

public class ParkourVictoryTrigger : MonoBehaviour
{
    public Transform player;
    public float barrierY = 64f;  // Позиция барьера по оси Z
    public GameObject victoryPanel; // Ссылка на UI Panel с сообщением о победе
    public Transform lobbyPosition;
    public float fadeDuration = 1f; // Длительность появления/исчезновения панели
    public float displayDuration = 5f; // Время отображения панели
    public GameObject trophyPrefab; // Префаб трофея
    public InteractionManager interactionManager; // Ссылка на InteractionManager
    public XRRayInteractor m_RayInteractor_left; // Левый луч
    public XRRayInteractor m_RayInteractor_right; // Правый луч

    private CanvasGroup victoryCanvasGroup;
    private Jumpability jumpAbility;

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
            Debug.LogError("victory Panel не назначен в victoryTrigger скрипте!");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Если игрок находится ниже Z=12, ограничиваем его движение
            if (other.transform.position.y > barrierY)
            {
                StartCoroutine(ShowvictoryAndTeleport());
            }
        }
    }

    
    private IEnumerator ShowvictoryAndTeleport()
    {
        if (interactionManager != null && trophyPrefab != null)
        {
            interactionManager.CreateObj(trophyPrefab);
            Destroy(interactionManager);
        }

        if (player != null && lobbyPosition != null && jumpAbility != null)
        {
            
            jumpAbility.enabled = false;
            player.position = lobbyPosition.position;
        }
        
        StartCoroutine(TeleportAndCheck(lobbyPosition, () =>
        {
            jumpAbility.enabled = true;
            m_RayInteractor_left.gameObject.SetActive(true);
            m_RayInteractor_right.gameObject.SetActive(true);
                       
        }));

        victoryPanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(victoryCanvasGroup, 0, 1, fadeDuration)); // Появление

        // Ждем указанное время (минус время затухания)
        yield return new WaitForSeconds(displayDuration - fadeDuration * 2);

        // Скрываем панель победы с анимацией исчезновения
        yield return StartCoroutine(FadeCanvasGroup(victoryCanvasGroup, 1, 0, fadeDuration)); // Исчезновение
        victoryPanel.SetActive(false);


    }

     private IEnumerator TeleportAndCheck(Transform targetPosition, System.Action onComplete)
    {
     
        player.position = targetPosition.position;
       
        yield return null;

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
        canvasGroup.alpha = endAlpha;
    }

}

