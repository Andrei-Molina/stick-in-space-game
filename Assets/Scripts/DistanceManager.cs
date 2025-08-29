using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceManager : MonoBehaviour 
{ 
    [SerializeField] private TextMeshProUGUI distanceText; 
    [SerializeField] private Transform startingDistance; 
    [SerializeField] private Transform playerTransform; 
    private float distance = 0; public float Distance => distance; 
    void Update() 
    { 
        if (playerTransform != null && startingDistance != null) 
        { 
            distance = Vector3.Distance(playerTransform.position, startingDistance.position); 
            distanceText.text = $"Distance: {distance.ToString("F2")}m"; 
        } 
    } 
}