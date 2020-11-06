using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Created by Thuyet Pham
// Edited by Arttu Paldán on 29.10.2020: Mainly I just merged this and PlayerHealthBar into one script. 
public class PlayerHealth : MonoBehaviour
{
    private PlayerCurrency playerCurrency;

    public int PlayerHP, PlayerMaxHP, newHP;

    private GameObject playerUI;
    private Slider hpBar;
    private Image hpFill;
    public Gradient hpGradient;

    [HideInInspector] public Transform spaceTextGrid;
    [HideInInspector] public Text currentSpace;
    [HideInInspector] public Text neededSpace;
    
    private int lostGold;
    private int lostKarma;


    void Awake()
    {
        playerCurrency = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerCurrency>();

        playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        hpBar = playerUI.transform.Find("hpBar").GetComponent<Slider>();
        hpFill = playerUI.transform.Find("hpBar").Find("hpFill").GetComponent<Image>();

        SetHP();

        spaceTextGrid = playerUI.transform.Find("TextGrid");
        currentSpace = spaceTextGrid.GetChild(1).GetComponent<Text>();
        neededSpace = spaceTextGrid.GetChild(3).GetComponent<Text>();
    }

    void SetHP()
    {
        PlayerHP = PlayerMaxHP;
        hpBar.maxValue = PlayerMaxHP;
        hpBar.value = PlayerHP;
        hpFill.color = hpGradient.Evaluate(1f);
    }

    // This can be handled by the UpdateHealth().
    public void PlayerTakeDamage(int EnemyDamage)
    {
        PlayerHP -= EnemyDamage;
        SetCurrentHP(PlayerHP);
        CheckPlayerDeath();
    }

    public void GainHealth(int gain)
    {
        newHP = Mathf.Clamp(PlayerHP + gain, 0, PlayerMaxHP);
        PlayerHP = newHP;
        SetCurrentHP(PlayerHP);
    }

    void SetCurrentHP(float HP)
    {
        hpBar.value = HP;
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
    }

    void CheckPlayerDeath()
    {
        if (PlayerHP <= 0)
        {
            LoseCurrency();
          
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void LoseCurrency()
    {
        lostGold = playerCurrency.gold * 10 / 100;
        lostKarma = playerCurrency.karma * 10 / 100;

        playerCurrency.LoseGold(lostGold);

        playerCurrency.LoseKarma(lostKarma);
        
        SaveManager.SaveCurrency(playerCurrency);
    }

    public void SetCurrentSpace(int count)
    {
        currentSpace.text = count.ToString();
    }

    public void SetNeededSpace(int count)
    {
        neededSpace.text = count.ToString();
    }
}
