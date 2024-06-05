using UnityEngine;

namespace Player
{
    public abstract class PlayerAbilityBase : MonoBehaviour
    {
        #region References
        protected PlayerController playerController;
        protected PlayerVisual playerVisual;
        #endregion

        #region ProtectedMembers
        protected bool isPrevented;
        #endregion

        #region VirtualMembers
        public virtual void Init(PlayerController playerController, PlayerVisual playerVisual)
        {
            this.playerController = playerController;
            this.playerVisual = playerVisual;
        }
        #endregion

        #region AbstractMembers
        public abstract void OnInputDisabled();
        public abstract void OnInputEnabled();
        public abstract void StopAbility();
        #endregion
    }
}
