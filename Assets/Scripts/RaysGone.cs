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



public class RaysGone : MonoBehaviour
{


    public XRRayInteractor m_RayInteractor_left; // Левый луч
    public XRRayInteractor m_RayInteractor_right; // Правый луч




    public void RaysBack ()
        {
            
            m_RayInteractor_left.gameObject.SetActive(true);
            m_RayInteractor_right.gameObject.SetActive(true);

        }
}
