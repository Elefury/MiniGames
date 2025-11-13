
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Transform pos;

    // Update is called once per frame
    public void CreateObj(GameObject prefab)
    {
        Instantiate(prefab, pos.position, pos.rotation);
    }
}
