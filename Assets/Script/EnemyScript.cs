using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Script
{
    public sealed class EnemyScript : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _circleCollider2D;

        [SerializeField] private float hSpeed;
        [SerializeField] private bool startLeft;

        private void Start()
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _circleCollider2D = this.GetComponent<CircleCollider2D>();
            
            
            Debug.Assert(_rigidbody2D != null, nameof(_rigidbody2D) + " != null");
            Debug.Assert(_spriteRenderer != null, nameof(_spriteRenderer) + " != null");
            Debug.Assert(_circleCollider2D != null, nameof(_circleCollider2D) + " != null");
            
            if (hSpeed != 0f)
            {
                _rigidbody2D.velocity = Vector2.right * hSpeed;
            }
            
            if (startLeft)
            {
                this.Flip();
            }

            // var isGrounded = false;
            //
            // while (!isGrounded)
            // {
            //     // this.transform.position.y -= 0.019;
            //     var transform1 = this.transform;
            //     var vector3 = transform1.position;
            //     vector3.y = 0.019f;
            //     transform1.position = vector3;
            // }
        }

        private void OnTriggerExit2D([NotNull] Collider2D other)
        {
            this.Flip();
        }

        private void Flip()
        {
            if (_rigidbody2D.velocity == Vector2.zero)
            {
                print("Velocity is 0");
                return;
            }
            
            _rigidbody2D.velocity *= -1;
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
            // _circleCollider2D.offset.x = 0 - _circleCollider2D.offset.x;
            var offset1 = _circleCollider2D.offset;
            var offset = offset1;
            offset.x = 0 - offset1.x;
            offset1 = offset;
            _circleCollider2D.offset = offset1;
        }
    }
}
