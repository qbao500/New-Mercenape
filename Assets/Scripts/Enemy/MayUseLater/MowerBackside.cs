using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowerBackside : MonoBehaviour
{
    [SerializeField] private EnemyStats stat;

    public enum GeneratorState { Inactive, Generating, Active }

    private GeneratorState currentState;

    private SpriteRenderer sprite;

    private MowerBehaviour mowerParent;

    private void Start()
    {     
        currentState = GeneratorState.Inactive;

        sprite = GetComponent<SpriteRenderer>();

        mowerParent = gameObject.GetComponentInParent<MowerBehaviour>();
    }

    void Update()
    {
        switch (currentState)
        {
            case GeneratorState.Inactive:
                {
                    sprite.color = Color.white;

                    break;
                }
            case GeneratorState.Generating:
                {
                    sprite.color = Color.yellow;

                    break;
                }
            case GeneratorState.Active:
                {
                    sprite.color = Color.red;

                    break;
                }
        }
    }

    public void DamagingBackside(int playerDmg)
    {
        print("yesssssss");
        if (currentState == GeneratorState.Inactive)
        {
            //mowerParent.currentHP -= playerDmg;

          
        }

        if (currentState == GeneratorState.Generating)
        {
            
        }

        if (currentState == GeneratorState.Active)
        {           
            // Field Generator will damage back the player and push upward
            //mowerParent.playerStat.PlayerTakeDamage(stat.damage);
        }

    }

    public void DamagingGenerator(int playerDmg)
    {
        print("circleeeee");
    }


}
