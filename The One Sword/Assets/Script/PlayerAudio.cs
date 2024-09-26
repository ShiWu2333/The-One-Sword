using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        playerController.OnPlayerHit += PlayerController_OnPlayerHit;
    }

    private void PlayerController_OnPlayerHit(object sender, System.EventArgs e)
    {
        audioSource.Play();
    }
}
