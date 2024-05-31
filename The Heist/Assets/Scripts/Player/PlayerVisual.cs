using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] protected MeshRenderer playerMainRenderer;
        [SerializeField] protected Animator playerAnimator;

        #region AnimatorMethods
        public void SetAnimatorParameter(string name)
        {
            playerAnimator.SetTrigger(Animator.StringToHash(name));
        }

        public void SetAnimatorParameter(string name, bool value)
        {
            playerAnimator.SetBool(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter(string name, float value)
        {
            playerAnimator.SetFloat(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter(string name, int value)
        {
            playerAnimator.SetInteger(Animator.StringToHash(name), value);
        }

        public void SetAnimatorTimeScale(float timeScale)
        {
            playerAnimator.speed = timeScale;
        }
        #endregion //AnimatorMethods
    }
}
