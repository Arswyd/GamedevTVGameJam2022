using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] GameObject graveVFX;
    [SerializeField] GameObject graveStone;
    [SerializeField] GameObject ghost;
    [SerializeField] GameObject ghostVFX;
    [SerializeField] float levelLoadDelay = 0.5f;

    [SerializeField] CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField] CinemachineVirtualCamera idleCamera;
    [SerializeField] CinemachineVirtualCamera runCamera;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    PlayerMovement playerMovement;
    Vector3 startingPosition;
    AudioPlayer audioPlayer;
    [SerializeField] CompositeCollider2D tilemapCollider;

    bool isAlive = true;


    void Awake() 
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        startingPosition = playerMovement.transform.position;
    }

    void Start() 
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    public void StartDeathSequence( )
    {
        if (isAlive)
        {
            isAlive = false;
            Vector3 playerPosition = playerMovement.transform.position;

            //if(!isAlive && myRigidbody.velocity.y < Mathf.Epsilon) 
            Destroy(playerMovement.gameObject);
            GameObject instance = Instantiate(graveVFX, playerPosition, Quaternion.identity);
            Destroy(instance, 1);
            GameObject graveStoneInstance = Instantiate(graveStone, playerPosition, Quaternion.identity);
            audioPlayer.PlayGraveClip();

            StartCoroutine(StartGhostInstantiation(graveStoneInstance.transform.position, new Vector2(0, 1.5f)));
        }
        else
        {
            Destroy(playerMovement.gameObject);
            StartCoroutine(StartGhostInstantiation(startingPosition, new Vector2(0, 0)));

        }
    }

    IEnumerator StartGhostInstantiation(Vector3 deathposition, Vector3 offset)
    {
        yield return new WaitForSeconds(levelLoadDelay);
        GameObject instance = Instantiate(ghostVFX, deathposition + offset, Quaternion.identity);
        Destroy(instance, 1);

        yield return new WaitForSeconds(levelLoadDelay);
        GameObject newPlayer = Instantiate(ghost, deathposition + offset, Quaternion.identity);
        stateDrivenCamera.m_AnimatedTarget = newPlayer.GetComponent<Animator>();
        idleCamera.m_Follow = newPlayer.transform;
        runCamera.m_Follow = newPlayer.transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
}
