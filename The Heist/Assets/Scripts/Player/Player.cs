using System.Collections;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IDamageble
    {
        private const string hitStringParameter = "Hitted";

        #region SerializeFields
        [SerializeField]
        private HealthModule healthModule;
        [SerializeField]
        private float postDamageInvulnerabilityTime;
        [SerializeField]
        private PlayerController playerController;
        [SerializeField]
        private PlayerVisual playerVisual;
        #endregion
        private Coroutine invulnerabilityCoroutine;

        private static Player player;

        public static Player Get()
        {
            if (player != null) return player;
            player = GameObject.FindObjectOfType<Player>();
            return player;
        }

        #region Mono
        private void Awake()
        {
            if (player != null && player != this)
            {
                Destroy(player);
                return;
            }
            player = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (player != this) return; 
            ResetHealth();
            healthModule.OnDamageTaken += InternalOnDamageTaken;
            healthModule.OnDeath += InternalOnDeath;
        }
        #endregion

        #region Interface: IDamageable
        public void TakeDamage(DamageContainer damage)
        {
            healthModule.TakeDamage(damage);
        }
        #endregion

        #region HealthModule
        public void InternalOnDamageTaken(DamageContainer damageContainer)  //richiamata dall'health module tramite l'action OnDamageTaken
        {
            NotifyHealthUpdatedGlobal();
            playerController.OnDamageTaken?.Invoke(damageContainer);
            playerVisual.SetAnimatorParameter(hitStringParameter);
            SetInvulnerable(postDamageInvulnerabilityTime);
        }
        public void InternalOnDeath()       //richiamata dall'health module tramite l'action OnDeath
        {
            playerController.IsDeath = true;
            playerController.OnDeath?.Invoke();
        }

        private IEnumerator InvulnerabilityCoroutine(float invulnerabilityTime)
        {
            healthModule.SetInvulnerable(true);
            yield return new WaitForSeconds(invulnerabilityTime);
            healthModule.SetInvulnerable(false);
        }
        private void SetInvulnerable(float invulnerabilityTime)
        {
            if (invulnerabilityCoroutine != null)
            {
                StopCoroutine(invulnerabilityCoroutine);
            }
            invulnerabilityCoroutine = StartCoroutine(InvulnerabilityCoroutine(invulnerabilityTime));
        }

        private void NotifyHealthUpdatedGlobal()
        {
            Debug.Log("Notify Health Update: Health = " + healthModule.CurrentHP);
            //GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated,
            //    EventArgsFactory.PlayerHealthUpdatedFactory((int)healthModule.MaxHP, (int)healthModule.CurrentHP));
        }

        public void ResetHealth()
        {
            healthModule.Reset();
            NotifyHealthUpdatedGlobal();
            playerController.IsDeath = false;
        }
        #endregion
    }
}
