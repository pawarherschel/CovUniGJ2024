using System;
using UnityEngine;

namespace Script
{
    public sealed class InternalPlayerController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int AttackTrigger = Animator.StringToHash("attack");
        private bool _canJump = true;
        private static readonly int JumpTrigger = Animator.StringToHash("jump");
        private SpriteRenderer _spriteRenderer;
        private static readonly int RunBool = Animator.StringToHash("run");

        // Start is called before the first frame update
        private void Start()
        {
            _animator = this.GetComponent<Animator>();
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        internal void SetRunning()
        {
            _animator.SetBool(RunBool, true);
        }

        internal void ResetRunning()
        {
            _animator.SetBool(RunBool, false);
        }

        internal void Attack()
        {
            _animator.SetTrigger(AttackTrigger);
        }

        internal bool Jump()
        {
            if (!_canJump)
            {
                return false;
            }
            
            _canJump = false;
            _animator.SetTrigger(JumpTrigger);

            return true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _canJump = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _canJump = true;
        }

        internal void FaceRight()
        {
            _spriteRenderer.flipX = false;
        }

        internal void FaceLeft()
        {
            _spriteRenderer.flipX = true;
        }
    }
}
