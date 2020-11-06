using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyBehaviour : EnemyBehaviourNotUsing
{
    // Collision with player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentState == EnemyState.Attack)
            {
                rb.AddForce((Vector2)transform.right * (-forcePower / 1.2f), ForceMode2D.Impulse);

                if (!isPlayerGetKnocked)
                {
                    isPlayerGetKnocked = true;
                    StartCoroutine("PlayerStun");
                }

                // playerStat.PlayerTakeDamage(enemyDamage);
                // Debug.Log(playerStat.PlayerHP);
            }
        }
    }

    IEnumerator PlayerStun()
    {
        if (player != null)
        {
            //playerRigid.bodyType = RigidbodyType2D.Static;
            playerRigid.drag = 15;
            playerRigid.mass = 100;
            playerMovement.enabled = false;
            playerAttack.enabled = false;
            playerRenderer.color = Color.red;

            yield return new WaitForSeconds(1.5f);

            //playerRigid.bodyType = RigidbodyType2D.Dynamic;
            playerRigid.drag = 1;
            playerRigid.mass = 1;
            playerMovement.enabled = true;
            playerAttack.enabled = true;
            playerRenderer.color = Color.white;

            isPlayerGetKnocked = false;
        }
    }
}
