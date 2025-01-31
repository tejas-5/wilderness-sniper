using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100;
    private int playerHp;
    public Slider healthSlider;

    [SerializeField] float maxPlayerMp = 100f;
    [SerializeField] float mpDecreaseRate = 5f;
    private float currentPlayerMp;
    public Slider mpSlider;

    public PopUpController popUpController;
    [SerializeField] float mpIncreaseInterval = 1f;
    [SerializeField] float mpIncreaseAmount = 1f;
    [SerializeField] float popUpChance = 0.01f;

    private bool isPopUpWaiting = false;

    public GameObject gameOverPanel;
    public GameObject errorCodePanel;

    private float popUpCooldown = 20f; // Cooldown time in seconds
    private float lastPopUpTime = 0f;

    public AudioClip clickSound; // Drag and drop your sound in the inspector
    private AudioSource audioSource;

    void Start()
    {
        playerHp = maxPlayerHp;
        currentPlayerMp = maxPlayerMp;
         

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp;
            healthSlider.value = playerHp;
        }


        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp;
            mpSlider.value = currentPlayerMp;
        }

        StartCoroutine(IncreaseMpOverTime());

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
      
        // Player movement logic
        mousePos = Input.mousePosition;
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        transform.position = worldPos;

        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp();
            PlayClickSound();
        }

        if (playerHp <= 0)
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    void PlayClickSound()
    {
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound); // Play the sound once
        }
    }

    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage;


        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);


        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    void ReduceMp()
    {
        if (GameManager.Instance.AnyScreenEnabled())
        {
            return;
        }
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);


        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }


    void IncreaseMp()
    {

        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }


    IEnumerator IncreaseMpOverTime()
    {
        while (true)
        {
            IncreaseMp();

            // Check if a pop-up should be triggered
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance && Time.time > lastPopUpTime + popUpCooldown)
            {
                if (playerHp > 10 && popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true);
                    isPopUpWaiting = true;
                    lastPopUpTime = Time.time; // Update the last pop-up time

                    // Wait for the pop-up to be resolved
                    yield return new WaitUntil(() => !isPopUpWaiting);
                }
            }

            // Wait for the next MP increase interval
            yield return new WaitForSeconds(mpIncreaseInterval);
        }
    }

    public void SetPopUpWaiting(bool waiting)
    {
        isPopUpWaiting = waiting;
    }
}