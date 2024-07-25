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
        private bool inWater = false;

        [SerializeField] private float maxHSpeed = 3f;
        [SerializeField] private float horizontalSpeed = 10f;
        [SerializeField] private float jumpForce = 7.5f;
        [SerializeField] private float healthPoints = 3;

        private PlayerPrefabSwitcherOnTimer _switcherOnTimer;
        
        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _switcherOnTimer = this.GetComponent<PlayerPrefabSwitcherOnTimer>();
        }

        // Update is called once per frame
        private void Update()
        {
            var axis = Input.GetAxis("Horizontal");
            var swimAxis = Input.GetAxis("Vertical");
            
            _rigidbody2D.AddForce(Vector2.right * (axis * horizontalSpeed));
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                var didJump = _switcherOnTimer.CurrentInternalController.Jump();
                if (didJump)
                {
                    _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }

            if (inWater && Input.GetAxis("Vertical") != 0)
            {
                _rigidbody2D.AddForce(horizontalSpeed * swimAxis * Vector2.up);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _switcherOnTimer.CurrentInternalController.Attack();
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
                    _switcherOnTimer.CurrentInternalController.FaceLeft();
                }
                else
                {
                    _switcherOnTimer.CurrentInternalController.FaceRight();
                }
                _switcherOnTimer.CurrentInternalController.SetRunning();
                
                _rigidbody2D.velocity = new Vector2(sign * maxHSpeed, _rigidbody2D.velocity.y);
            }
            else
            {
                Debug.Assert(_switcherOnTimer, nameof(_switcherOnTimer) + " != null");
                Debug.Assert(_switcherOnTimer.CurrentInternalController, "_switcherOnTimer.CurrentInternalPlayerController != null");
                _switcherOnTimer.CurrentInternalController.ResetRunning();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                inWater = true;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Lava"))
            {
                healthPoints -= 0.5f;
                if (healthPoints < 0)
                {
                    GameOver();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                inWater = false;
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
