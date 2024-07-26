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
        private Rigidbody2D _rigidbody2D;

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

            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            Debug.Assert(_rigidbody2D != null, nameof(_rigidbody2D) + " != null");
        }

        // Update is called once per frame
        private void Update()
        {
            var canAttack = CanAttack();
            var canChase = CanChase();

            bool? attackSuccess = null;
            bool? chaseSuccess = null;

            if (canAttack)
            {
                _rigidbody2D.velocity = Vector2.zero;
                _switcherOnTimer.CurrentInternalController.StopChase();
                attackSuccess = _switcherOnTimer.CurrentInternalController.Attack();
            }
            if (canChase && !canAttack)
            {
                chaseSuccess = _switcherOnTimer.CurrentInternalController.Chase();
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
                _switcherOnTimer.CurrentInternalController.StopChase();
            }

            if (!(chaseSuccess ?? false)) return;
            var direction = ((_player.transform.position.x - this.transform.position.x) < 0 ? -1 : 1);
            _rigidbody2D.velocity = Vector2.right * (hSpeed * direction);
        }

        private bool CanChase()
        {
            var position = chasePosition.position;
            var playerInRange = _player.transform != null && Vector2.Distance(_player.transform.position, position) <
                Vector2.Distance(Vector2.zero, position);

            return playerInRange;
        }

        private bool CanAttack()
        {
            var position = attackPosition.position;
            var playerInRange = _player.transform != null && Vector2.Distance(_player.transform.position, position) <
                Vector2.Distance(Vector2.zero, position);

            return playerInRange;
        }
    }
}
