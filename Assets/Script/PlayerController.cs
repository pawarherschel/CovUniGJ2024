using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public sealed class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [SerializeField] private float maxHSpeed = 3f;
        [SerializeField] private float horizontalSpeed = 10f;
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
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                var didJump = _switcherOnTimer.CurrentInternalPlayerController.Jump();
                if (didJump)
                {
                    _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.J))
            {
                _switcherOnTimer.CurrentInternalPlayerController.Attack();
            }
            
            if (_rigidbody2D.velocity.x < -maxHSpeed || _rigidbody2D.velocity.x > maxHSpeed)
            {
                var sign = _rigidbody2D.velocity.x >= 0 ? 1 : -1;
                if (sign == -1)
                {
                    _switcherOnTimer.CurrentInternalPlayerController.FaceLeft();
                }
                else
                {
                    _switcherOnTimer.CurrentInternalPlayerController.FaceRight();
                }
                _rigidbody2D.velocity = new Vector2(sign * maxHSpeed, _rigidbody2D.velocity.y);
            }
            
            if (_rigidbody2D.velocity.Abs().x > 0.000007)
            {
                _switcherOnTimer.CurrentInternalPlayerController.SetRunning();
            }
            else
            {
                _switcherOnTimer.CurrentInternalPlayerController.ResetRunning();
            }
        }
    }
}
