using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private PlayerMovement _playerMovement;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _timer.enabled = true;
            _playerMovement.enabled = true;
            Destroy(gameObject);
        }
    }
}
