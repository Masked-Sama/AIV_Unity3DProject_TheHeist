using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private static Player player;

        [SerializeField]
        private Transform startingPoint;


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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (GameObject.Find("PlayerStartingPosition") == null) return;

            transform.position = GameObject.Find("PlayerStartingPosition").transform.position;
            transform.rotation = GameObject.Find("PlayerStartingPosition").transform.rotation;

        }

        private void OnEnable()
        {
        }

        private void Start()
        {
            if (player != this) return;

        }
        #endregion
    }
}
