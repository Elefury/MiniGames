using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SandEatsBall : MonoBehaviour
{
    public float sinkDuration = 1f; // ¬рем€, за которое м€ч "погрузитс€" в пол
    public float sinkDistance = 1f; // Ќасколько глубоко м€ч погрузитс€
    private float ballPositionX;

    private List<GameObject> activeBalls = new List<GameObject>(); // —писок активных м€чей

    private void OnTriggerEnter(Collider other)
    {
        // ѕровер€ем, что вошли именно м€чом
        if (other.gameObject.CompareTag("Ball") && !activeBalls.Contains(other.gameObject))
        {
            ballPositionX = other.transform.position.x;
            Debug.Log((-41f-ballPositionX) + "m");
            //Debug.Log("ћ€ч вошел в триггер. «апускаем эффект погружени€.");

            // ƒобавл€ем м€ч в список активных
            activeBalls.Add(other.gameObject);

            // «апускаем корутину дл€ эффекта погружени€ и уничтожени€ м€ча
            StartCoroutine(SinkAndDestroyBall(other.gameObject));
        }
    }

    //  орутина дл€ эффекта погружени€ и уничтожени€ м€ча
    private IEnumerator SinkAndDestroyBall(GameObject ball)
    {
        // ќтключаем физику м€ча
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.isKinematic = true; // ƒелаем м€ч кинематическим
        }

        // —охран€ем начальную позицию и масштаб м€ча
        Vector3 startPosition = ball.transform.position;
        Vector3 startScale = ball.transform.localScale;

        // ¬рем€, прошедшее с начала погружени€
        float elapsedTime = 0f;

        while (elapsedTime < sinkDuration)
        {
            // ¬ычисл€ем прогресс погружени€ (от 0 до 1)
            float progress = elapsedTime / sinkDuration;

            // ѕлавно уменьшаем масштаб м€ча
            ball.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);

            // ѕлавно перемещаем м€ч вниз
            ball.transform.position = Vector3.Lerp(startPosition, startPosition - Vector3.up * sinkDistance, progress);

            // ”величиваем прошедшее врем€
            elapsedTime += Time.deltaTime;
            yield return null; // ∆дем следующий кадр
        }

        // ”бедимс€, что м€ч полностью "погрузилс€"
        ball.transform.localScale = Vector3.zero;
        ball.transform.position = startPosition - Vector3.up * sinkDistance;

        // ”ничтожаем м€ч
        if (ball != null)
        {
            //Debug.Log("ћ€ч уничтожен.");
            Destroy(ball);
        }

        // ”дал€ем м€ч из списка активных
        activeBalls.Remove(ball);
    }
}