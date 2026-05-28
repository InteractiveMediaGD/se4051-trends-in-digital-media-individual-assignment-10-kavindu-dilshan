using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [SerializeField] private string levelName;

        public string lastPlayedLevel { get; set; }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            instance.lastPlayedLevel = levelName;
        }
    }
}