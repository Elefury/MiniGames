using UnityEngine;

public class CleanScene : MonoBehaviour
{
    private  GameObject[] balls;
    // Update is called once per frame

   
    public void CleanUp()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject b in balls)
        {
            Destroy(b);
        }
    }

}
