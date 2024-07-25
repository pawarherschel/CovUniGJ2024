using System;
using UnityEngine;
using UnityEngine.Assertions;

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
    }
}
