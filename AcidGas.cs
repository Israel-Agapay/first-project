using UnityEngine;

public class AcidGas : MonoBehaviour
{
    public float damageInterval = 2f; // Time between each damage tick
    public int damageAmount = 2; // Damage dealt per tick
    public Color gasEffectColor = new Color(0f, 1f, 0f, 0.3f); // Green overlay effect

    private bool playerInGas = false; // Track if the player is inside the gas
    private Coroutine damageCoroutine; // Coroutine reference
    private PlayerHealth playerHealth; // Reference to the player's health
    private ScreenOverlay screenOverlay; // Reference to screen overlay (for green effect)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInGas = true;

            // Get references to the player's components
            playerHealth = collision.GetComponent<PlayerHealth>();
            screenOverlay = collision.GetComponent<ScreenOverlay>();

            // Start green screen effect
            if (screenOverlay != null)
            {
                screenOverlay.EnableOverlay(gasEffectColor);
            }

            // Start damage coroutine if not already running
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDamageOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInGas = false;

            // Stop green screen effect if the player leaves the gas
            if (screenOverlay != null)
            {
                screenOverlay.DisableOverlay();
            }

            // Stop the coroutine if the player is no longer in the gas
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private System.Collections.IEnumerator ApplyDamageOverTime()
    {
        // Initial delay of 2 seconds before starting damage
        yield return new WaitForSeconds(2f);

        while (playerInGas)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamge(1);
                Debug.Log($"Player damaged! Current health: {playerHealth.CurrentHealth}");
            }

            yield return new WaitForSeconds(damageInterval);
        }

        // Reset coroutine reference when it stops
        damageCoroutine = null;
    }
}
