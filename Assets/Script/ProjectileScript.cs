using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = System.Diagnostics.Debug;

namespace Script
{
    public sealed class ProjectileScript : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private bool affectedByGravity = false;
        [SerializeField] private float timeToLive = 0f;
        
        private void Start()
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            Assert.IsNotNull(_rigidbody2D);

            _rigidbody2D.gravityScale = affectedByGravity ? _rigidbody2D.gravityScale : 0f;
            
            Assert.IsFalse(timeToLive <= 0);
            
            Destroy(this.gameObject, timeToLive);
        }

        internal void SetVelocity(Vector2 velocity)
        {
            if (velocity == Vector2.zero)
            {
                return;
            }
            
            _rigidbody2D.velocity = velocity;
        }

        private void OnTriggerEnter2D([NotNull] Collider2D other)
        {
            if (!other.CompareTag("Enemy") && !other.CompareTag("Player")) return;
            var remainingHealth = 0f;

            if (other.CompareTag("Enemy")){
                var healthScript = other.GetComponentInParent<HealthScript>();
                if (healthScript)
                {
                    healthScript.health -= 1;
                    remainingHealth = healthScript.health;
                    print(nameof(remainingHealth) + ": " + remainingHealth);
                }
            }
            if (other.CompareTag("Player"))
            {
                var playerController = other.GetComponent<PlayerController>();
                playerController.healthPoints -= 1;
                remainingHealth = playerController.healthPoints;
            }

            if (remainingHealth > 0) return;

            Destroy(other.gameObject); 
            Destroy(this.gameObject);

            var levelScript = GameObject.FindObjectOfType<LevelScript>();

            Debug.Assert(levelScript != null, nameof(levelScript) + " != null");
            levelScript.noOfRemainingEnemies -= 1;
        }
    }
}
