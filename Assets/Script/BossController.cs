using UnityEngine;

namespace Script
{
    public sealed class BossController : MonoBehaviour
    {
        [SerializeField] private Transform attackPosition;
        [SerializeField] private Transform chasePosition;
        [SerializeField] private float hSpeed;
        
        private GameObject _player;
        private BossPrefabSwitcherOnTimer _switcherOnTimer;

        // Start is called before the first frame update
        private void Start()
        {
            Debug.Assert(attackPosition != null, nameof(attackPosition) + " != null");
            Debug.Assert(chasePosition != null, nameof(chasePosition) + " != null");
            Debug.Assert(hSpeed != 0);
            
            _player = GameObject.FindWithTag("Player");
            Debug.Assert(_player != null, nameof(_player) + " != null");

            _switcherOnTimer = this.GetComponent<BossPrefabSwitcherOnTimer>();
            Debug.Assert(_switcherOnTimer != null, nameof(_switcherOnTimer) + " != null");

            var internalBossController = _switcherOnTimer.CurrentInternalController;
            Debug.Assert(internalBossController != null, nameof(internalBossController) + " != null");
        }

        // Update is called once per frame
        private void Update()
        {
            var canAttack = CanAttack();
            var canChase = CanChase();


            if (canAttack)
            {
                var attackSuccess = _switcherOnTimer.CurrentInternalController.Attack();
            }
            if (canChase)
            {
                var chaseSuccess = _switcherOnTimer.CurrentInternalController.Chase();
            }

            if (canChase && canAttack)
            {
                _switcherOnTimer.CurrentInternalController.StopChase();
                _switcherOnTimer.CurrentInternalController.Attack();
            }
        }

        private bool CanChase()
        {
            var chasePositionX = chasePosition.position.x;
            var playerInRange = Mathf.Abs(_player.transform.position.x - chasePositionX) <
                                chasePositionX;

            return playerInRange;
        }

        private bool CanAttack()
        {
            var attackPositionX = attackPosition.position.x;
            var playerInRange = Mathf.Abs(_player.transform.position.x - attackPositionX) < attackPositionX;

            return playerInRange;
        }
    }
}
