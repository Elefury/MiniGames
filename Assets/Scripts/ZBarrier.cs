using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;


public class ZBarrier : MonoBehaviour
{
    public float barrierX = -41f;  // Позиция барьера по оси Z

    // Обработчик столкновений с триггером
    void OnTriggerStay(Collider other)
    {
        // Проверка, если это игрок
        if (other.CompareTag("Player"))
        {
            // Если игрок находится ниже Z=12, ограничиваем его движение
            if (other.transform.position.x < barrierX)
            {
                
                other.transform.position = new Vector3(barrierX, other.transform.position.y, other.transform.position.z);
            }
        }
    }
}

