using System.Collections;
using UnityEngine;
using Cinemachine;

public class DeathHandler : MonoBehaviour
{
    [Header("Grave")]
    [SerializeField] GameObject graveVFX;
    [SerializeField] GameObject graveStone;

    [Header("Ghost")]
    [SerializeField] GameObject ghost;
    [SerializeField] GameObject ghostVFX;
    [SerializeField] float levelLoadDelay = 0.5f;

    [Header("Camera")]
    [SerializeField] CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField] CinemachineVirtualCamera idleCamera;
    [SerializeField] CinemachineVirtualCamera runCamera;

    [Header("Win VFX")]
    [SerializeField] GameObject[] winVFXs;

    PlayerMovement playerMovement;
    Vector3 startingPosition;
    AudioPlayer audioPlayer;

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

    public void Win()
    {
        if (winVFXs != null)
        {
            foreach (GameObject winVFX in winVFXs)
            {
                winVFX.SetActive(true);
                playerMovement.SetWon();
            }
        }
    }
}
