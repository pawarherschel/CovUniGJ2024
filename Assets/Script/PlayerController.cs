using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Script
{
    public sealed class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [SerializeField] private float maxHSpeed = 3f;
        [SerializeField] private float horizontalSpeed = 10f;
        private bool _canJump = true;
        [SerializeField] private float jumpForce = 7.5f;

        private PrefabSwitcherOnTimer _switcherOnTimer;
        
        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _switcherOnTimer = this.GetComponent<PrefabSwitcherOnTimer>();
        }

        // Update is called once per frame
        private void Update()
        {
            _rigidbody2D.AddForce(Vector2.right * (Input.GetAxis("Horizontal") * horizontalSpeed));
            if (Input.GetKeyDown(KeyCode.Space) && _canJump)
            {
                // var animator = _switcherOnTimer.CurrentSprite.GetComponent<Animator>();
                // animator.Play("jump");
                _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                _canJump = false;
            }
            
            if (_rigidbody2D.velocity.x < -maxHSpeed || _rigidbody2D.velocity.x > maxHSpeed)
            {
                var sign = _rigidbody2D.velocity.x >= 0 ? 1 : -1;
                _rigidbody2D.velocity = new Vector2(sign * maxHSpeed, _rigidbody2D.velocity.y);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _canJump = true;
        }
    }
}
