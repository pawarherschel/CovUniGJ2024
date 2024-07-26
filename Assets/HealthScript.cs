using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3;
    public float health;

    private void Awake()
    {
        health = maxHealth;
    }
}
