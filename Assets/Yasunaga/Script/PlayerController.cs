using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

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
    [SerializeField] int mpIncreaseAmount = 1; 
    [SerializeField] float popUpChance = 0.1f; 

    private bool isPopUpWaiting = false; 

    
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
    }

    
    void Update()
    {
        
        mousePos = Input.mousePosition;

        
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        
        transform.position = worldPos;
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); 
        }

        
        if (playerHp <= 0)
        {
            
            FindObjectOfType<GameManager>().GameOver();
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
        Debug.Log("MP Recovery Coroutine Started!");
        while (true) 
        {
            
            IncreaseMp();
            yield return new WaitForSeconds(mpIncreaseInterval);

            
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
            {
                
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true);
                    isPopUpWaiting = true;
                }
            }

            
            yield return new WaitForSeconds(mpIncreaseInterval);

            
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); 
                isPopUpWaiting = false; 
            }
        }
    }
}