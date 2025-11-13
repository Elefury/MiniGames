using UnityEngine;
using UnityEngine.UI;

public class CubeRoom : MonoBehaviour
{
    public Button readyButton; // Кнопка "Готов"
    //public GameObject cubePrefab;
    //public int gridSize = 10;
    //public float spacing = 1.1f;
    public Color activeColor = Color.yellow; // Цвет активных квадратиков

    private int activeCubeCount; // Количество активных квадратиков
    private GameObject[] cubes;

    private void Start()
    {
        //readyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    private void GameStarting()
    {
        Debug.Log("Игрок готов! Начинаем игру.");
        //StartGame();
    }



}