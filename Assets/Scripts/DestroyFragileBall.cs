using UnityEngine;

public class DestroyFragileBall : MonoBehaviour
{
    private float ballPositionX;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошли именно мячом
        if (other.gameObject.CompareTag("Ball")) // Или: other.gameObject.name == "Ball"
        {
            ballPositionX = other.transform.position.x;
            Debug.Log((-41f-ballPositionX) + "m");
            Destroy(other.gameObject);
        }
    }
}
