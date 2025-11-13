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

public class TeleportPlayer : MonoBehaviour
{
    public Transform player; // Ссылка на игрока (XR Origin)
    public XRRayInteractor m_RayInteractor_left; // Левый луч
    public XRRayInteractor m_RayInteractor_right; // Правый луч
    public Transform lobby; // Точка телепортации в лобби
    public Transform game; // Точка телепортации в игру
    public Transform gameBallThrowEasy;
    public Transform gameBallThrowMedium;
    public Transform gameBallThrowHard;
    public Transform gameBallThrowExtreme;
    public Transform gameParkourEasy;

    private Jumpability jumpAbility; // Ссылка на скрипт Jumpability
    private bool isTeleported = false;

    private void Start()
    {
        // Находим компонент Jumpability на игроке
        jumpAbility = player.GetComponent<Jumpability>();

        Debug.Log("Started");

        // Если компонент не найден, выводим предупреждение
        if (jumpAbility == null)
        {
            Debug.LogWarning("Jumpability script not found on the player!");
        }
    }

    public void TeleportToParkourGameEasy() {
        if (jumpAbility != null && gameParkourEasy != null && player != null)
        {
            jumpAbility.enabled = false;
            player.position = gameParkourEasy.position;
        }

        StartCoroutine(TeleportAndCheck(gameParkourEasy, () =>
        {
            // Включаем Jumpability после успешной телепортации
            
            jumpAbility.enabled = true;
            

            // Отключаем лучи
            if (isTeleported) {
            m_RayInteractor_left.gameObject.SetActive(false);
            m_RayInteractor_right.gameObject.SetActive(false);
            }

            Debug.Log("Teleported to Game successfully!");
        }));
        
    }


    public void TeleportToBallThrowGame() {
        if (jumpAbility != null && gameBallThrowEasy != null && player != null)
        {
            jumpAbility.enabled = false;
            player.position = gameBallThrowEasy.position;
        }

        StartCoroutine(TeleportAndCheck(gameBallThrowEasy, () =>
        {
            // Включаем Jumpability после успешной телепортации
            
            jumpAbility.enabled = true;
            

            // Отключаем лучи
            if (isTeleported) {
            m_RayInteractor_left.gameObject.SetActive(false);
            m_RayInteractor_right.gameObject.SetActive(false);
            }

            Debug.Log("Teleported to Game successfully!");
        }));
        
    }

        public void TeleportToBallThrowGameMedium() {
        if (jumpAbility != null && gameBallThrowMedium != null && player != null)
        {
            jumpAbility.enabled = false;
            player.position = gameBallThrowMedium.position;
        }

        StartCoroutine(TeleportAndCheck(gameBallThrowMedium, () =>
        {
            // Включаем Jumpability после успешной телепортации
            
            jumpAbility.enabled = true;
            

            // Отключаем лучи
            if (isTeleported) {
            m_RayInteractor_left.gameObject.SetActive(false);
            m_RayInteractor_right.gameObject.SetActive(false);
            }

            Debug.Log("Teleported to Game successfully!");
        }));
        
        }

        public void TeleportToBallThrowGameHard() {
        if (jumpAbility != null && gameBallThrowHard != null && player != null)
        {
            jumpAbility.enabled = false;
            player.position = gameBallThrowHard.position;
        }

        StartCoroutine(TeleportAndCheck(gameBallThrowHard, () =>
        {
            // Включаем Jumpability после успешной телепортации
            
                jumpAbility.enabled = true;
            

            // Отключаем лучи
            if (isTeleported) {
            m_RayInteractor_left.gameObject.SetActive(false);
            m_RayInteractor_right.gameObject.SetActive(false);
            }

            Debug.Log("Teleported to Game successfully!");
        }));
    }

    public void TeleportToBallThrowGameExtreme() {
        if (jumpAbility != null && gameBallThrowExtreme != null && player != null)
        {
            jumpAbility.enabled = false;
            player.position = gameBallThrowExtreme.position;
        }

        StartCoroutine(TeleportAndCheck(gameBallThrowExtreme, () =>
        {
            // Включаем Jumpability после успешной телепортации
            
                jumpAbility.enabled = true;
            

            // Отключаем лучи
            if (isTeleported) {
            m_RayInteractor_left.gameObject.SetActive(false);
            m_RayInteractor_right.gameObject.SetActive(false);
            }

            Debug.Log("Teleported to Game successfully!");
        }));
    }

    public void TeleportToLobby()
    {
        // Отключаем Jumpability перед телепортацией
        if (jumpAbility != null && lobby != null && player != null)
        {
            jumpAbility.enabled = false;
            player.position = lobby.position;
        }

        // Запускаем корутину для телепортации и проверки
        StartCoroutine(TeleportAndCheck(lobby, () =>
        {
            // Включаем Jumpability после успешной телепортации
            
            jumpAbility.enabled = true;
            

            // Включаем лучи
            if (isTeleported) {
            m_RayInteractor_left.gameObject.SetActive(true);
            m_RayInteractor_right.gameObject.SetActive(true);
            }
            Debug.Log("Teleported to Lobby successfully!");
        }));
    }



    private IEnumerator TeleportAndCheck(Transform targetPosition, System.Action onComplete)
    {
        
        
        isTeleported = false;

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
                isTeleported = true;
                Debug.Log("Teleported successfully!");
                break;
            }
        }

        // Если телепортация успешна, вызываем колбэк
        if (isTeleported)
        {
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogWarning("Failed to teleport player after multiple attempts.");
        }
    }
}