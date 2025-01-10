using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI¤Î¤¿¤á¤Î¥é¥¤¥Ö¥é¥E

public class PlayerController : MonoBehaviour
{
    // ¥Ş¥¦¥¹Î»ÖÃ¤È¥E`¥EÉÎ»ÖÃ¤ò±£´æ¤¹¤E¿¤á¤Î‰äÊı
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // ×ûĞóHP
    private int playerHp; // ¬FÔÚ¤ÎHP
    public Slider healthSlider; // UI¤ÎHP¥Ğ©`

    [SerializeField] float maxPlayerMp = 100f; // ×ûĞóMP
    [SerializeField] float mpDecreaseRate = 5f; // MP¤¬œp¤EÙ¶È
    private float currentPlayerMp; // ¬FÔÚ¤ÎMP
    public Slider mpSlider; // UI¤ÎMP¥Ğ©`

    public PopUpController popUpController; // ¥İ¥Ã¥×¥¢¥Ã¥×¤ò±úæ¾¤¹¤E³¥ó¥È¥úÅ`¥é©`
    [SerializeField] float mpIncreaseInterval = 1f; // MP¤¬»ØÍ¤¹¤Erégég¸E
    [SerializeField] int mpIncreaseAmount = 1; // 1»Ø¤ÎMP»ØÍÁ¿
    [SerializeField] float popUpChance = 0.1f; // ¥İ¥Ã¥×¥¢¥Ã¥×¤¬±úæ¾¤µ¤EE_ÂÊ£¨0.0 - 1.0£©

    private bool isPopUpWaiting = false; // ¥İ¥Ã¥×¥¢¥Ã¥×¤¬´ı™CÖĞ¤«¤É¤¦¤«¤Î¥Õ¥é¥°

    // ¥²©`¥àé_Ê¼•r¤Ë1»Ø¤À¤±ŒgĞĞ¤µ¤EE
    void Start()
    {
        playerHp = maxPlayerHp; // ¥×¥E¤¥ä©`¤ÎHP¤ò×ûĞó‚¤ËÔO¶¨
        currentPlayerMp = maxPlayerMp; // ¥×¥E¤¥ä©`¤ÎMP¤ò×ûĞó‚¤ËÔO¶¨

        // HP¥Ğ©`¤ÎÔO¶¨
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HP¥Ğ©`¤Î×ûĞó‚¤òÔO¶¨
            healthSlider.value = playerHp; // ¬FÔÚ¤ÎHP¤ò¥Ğ©`¤ËÔO¶¨
        }

        // MP¥Ğ©`¤ÎÔO¶¨
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MP¥Ğ©`¤Î×ûĞó‚¤òÔO¶¨
            mpSlider.value = currentPlayerMp; // ¬FÔÚ¤ÎMP¤ò¥Ğ©`¤ËÔO¶¨
        }

        // Ò»¶¨•rég¤´¤È¤ËMP¤ò»ØÍ¤¹¤EIÀúÀòé_Ê¼
        StartCoroutine(IncreaseMpOverTime());
    }

    // š°¥Õ¥E`¥àŒgĞĞ¤µ¤EE
    void Update()
    {
        // ¥Ş¥¦¥¹¤ÎÎ»ÖÃ¤òÈ¡µÃ
        mousePos = Input.mousePosition;

        // ¥Ş¥¦¥¹¤Î¥¹¥¯¥E`¥ó×ù˜Ë¤ò¥E`¥EÉ×ù˜Ë¤Ë‰ä“Q
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // ¥×¥E¤¥ä©`¤ÎÎ»ÖÃ¤ò¥Ş¥¦¥¹¤ÎÎ»ÖÃ¤ËºÏ¤E»¤E
        transform.position = worldPos;

        // ¥Ş¥¦¥¹¤Î×ó¥¯¥EÃ¥¯¤ÇMP¤òœp¤é¤¹
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MP¤òœp¤é¤¹
        }
    }

    // ¥À¥á©`¥¸¤òÊÜ¤±¤¿¤È¤­¤Ëºô¤Ğ¤EE
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // HP¤òœp¤é¤¹

        // HP¤¬0ÒÔÏÂ¤Ë¤Ê¤é¤Ê¤¤¤è¤¦¤ËÖÆÏŞ
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HP¥Ğ©`¤ò¸EÂ
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MP¤òœp¤é¤¹„IÀE
    void ReduceMp()
    {
        // MP¤¬0ÒÔÏÂ¤Ë¤Ê¤é¤Ê¤¤¤è¤¦¤ËÖÆÏŞ
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MP¥Ğ©`¤ò¸EÂ
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MP¤ò»ØÍ¤¹¤EIÀE
    void IncreaseMp()
    {
        // MP¤¬×ûĞó‚¤ò³¬¤¨¤Ê¤¤¤è¤¦¤ËÖÆÏŞ
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MP¥Ğ©`¤ò¸EÂ
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // Ò»¶¨•rég¤´¤È¤ËMP¤ò»ØÍ¤¹¤E³¥E`¥Á¥E
    IEnumerator IncreaseMpOverTime()
    {
        Debug.Log("MP Recovery Coroutine Started!");
        while (true) // ŸoÏŞ¥E`¥×¤ÇMP¤ò»ØÍ
        {
            // MP¤ò»ØÍ
            IncreaseMp();
            yield return new WaitForSeconds(mpIncreaseInterval);

            // MP¤¬×ûĞó¤Ç¤Ï¤Ê¤¯¡¢¥İ¥Ã¥×¥¢¥Ã¥×¤Î´_ÂÊ¤Ë»ù¤Å¤¤¤Æ¥İ¥Ã¥×¥¢¥Ã¥×¤ò±úæ¾
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
            {
                // ¥İ¥Ã¥×¥¢¥Ã¥×¤ò±úæ¾
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // ¥İ¥Ã¥×¥¢¥Ã¥×¤ò±úæ¾
                    isPopUpWaiting = true;
                }
            }

            // ´Î¤Î»ØÍ¤Ş¤Ç´ı™C
            yield return new WaitForSeconds(mpIncreaseInterval);

            // ¥İ¥Ã¥×¥¢¥Ã¥×´ı™CÖĞ¤Î¥Õ¥é¥°¤¬Á¢¤Ã¤Æ¤¤¤EĞ¡¢20ÃEá¤Ë¥E»¥Ã¥È
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // 20ÃEı¤Ä
                isPopUpWaiting = false; // ¥Õ¥é¥°¤ò¥E»¥Ã¥È¤·¤Æ¡¢´Î¤Î¥İ¥Ã¥×¥¢¥Ã¥×¤òÔS¿É
            }
        }
    }
}