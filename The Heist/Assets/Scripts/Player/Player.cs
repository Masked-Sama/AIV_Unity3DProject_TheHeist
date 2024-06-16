using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
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
        }
        #endregion
    }
}
