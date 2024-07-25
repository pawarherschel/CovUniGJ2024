using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

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
        private bool _prevAttackAnimationEnding;

        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform projectileLocation;
        [SerializeField] private Vector2 projectileVelocity = Vector2.zero;

        // Start is called before the first frame update
        private void Start()
        {
            _animator = this.GetComponent<Animator>();
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            
            Assert.IsNotNull(projectile);
            Assert.IsNotNull(projectileLocation);
        }

        private void Update()
        {
            var animationState = _animator.GetCurrentAnimatorStateInfo(0);

            var attackAnimationEnding = (animationState.IsName("atk") && animationState.normalizedTime > 0.2);
            var attackJustConnected = !_prevAttackAnimationEnding && attackAnimationEnding;

            if (attackJustConnected)
            {
                var projectileInstance = Instantiate(projectile, projectileLocation);
                var projectileScript = projectileInstance.GetComponent<ProjectileScript>();
                projectileScript.SetVelocity(projectileVelocity);
            }

            _prevAttackAnimationEnding = attackAnimationEnding;
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
