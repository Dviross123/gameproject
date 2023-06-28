using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;


    public Transform GetDestinatrion() 
    {
        return destination;
    }
}
