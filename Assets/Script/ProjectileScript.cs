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
        private Vector2 _velocity;
        
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

        private void Update()
        {
            _rigidbody2D.velocity = _velocity;
        }

        internal void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
            
            if (!(velocity.x < 0)) return;
            
            var spriteRenderer = this.GetComponent<SpriteRenderer>();

            if (spriteRenderer)
            {
                spriteRenderer.flipX = true;
            }
        }

        private void OnTriggerEnter2D([NotNull] Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;
            Destroy(other.gameObject);
            Destroy(this.gameObject);

            var levelScript = GameObject.FindObjectOfType<LevelScript>();
            
            Debug.Assert(levelScript != null, nameof(levelScript) + " != null");
            levelScript.noOfRemainingEnemies -= 1;
        }
    }
}
