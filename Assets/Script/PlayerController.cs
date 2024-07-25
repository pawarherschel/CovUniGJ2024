using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = System.Diagnostics.Debug;

namespace Script
{
    public sealed class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [SerializeField] private float maxHSpeed = 3f;
        [SerializeField] private float horizontalSpeed = 10f;
        [SerializeField] private float jumpForce = 7.5f;
        [SerializeField] private int healthPoints = 2;

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
            var axis = Input.GetAxis("Horizontal");
            
            _rigidbody2D.AddForce(Vector2.right * (axis * horizontalSpeed));
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
            
            
            if (Mathf.Abs(axis) != 0f)
            {
                var sign = axis switch
                {
                    // 0 => 0,
                    > 0 => +1,
                    _ => -1
                };

                if (sign == -1)
                {
                    _switcherOnTimer.CurrentInternalPlayerController.FaceLeft();
                }
                else
                {
                    _switcherOnTimer.CurrentInternalPlayerController.FaceRight();
                }
                _switcherOnTimer.CurrentInternalPlayerController.SetRunning();
                
                _rigidbody2D.velocity = new Vector2(sign * maxHSpeed, _rigidbody2D.velocity.y);
            }
            else
            {
                Debug.Assert(_switcherOnTimer != null, nameof(_switcherOnTimer) + " != null");
                Debug.Assert(_switcherOnTimer.CurrentInternalPlayerController != null, "_switcherOnTimer.CurrentInternalPlayerController != null");
                _switcherOnTimer.CurrentInternalPlayerController.ResetRunning();
            }
        }

        private void OnCollisionEnter2D([NotNull] Collision2D other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            
            healthPoints--;
            if (healthPoints < 0)
            {
                GameOver();
            }
        }

        private static void GameOver()
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
