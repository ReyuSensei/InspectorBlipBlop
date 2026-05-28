using UnityEngine;
using System.Collections;

public class PlayerAnimationEventDispatcher : MonoBehaviour
{
    public SkinnedMeshRenderer playerMeshRenderer;
    private void Start()
    {

        blinkIndex = playerMeshRenderer.sharedMesh.GetBlendShapeIndex(blinkShapeName);

        if (blinkIndex != -1)
        {
            StartCoroutine(BlinkLoop());
        }
        else
        {
            Debug.LogError("No se encontró el Blend Shape: " + blinkShapeName);
        }
    }
    public void EventStep()
    {
        transform.parent.SendMessage("EventStep");
    }

    public void EventShowGlass() 
    {
        transform.parent.SendMessage("ShowGlass");
    }

    public void EventHideGlass()
    {
        transform.parent.SendMessage("HideGlass");
    }

    public string blinkShapeName = "Ojos cerrados";

    // Tiempo entre pestañeos
    public float minBlinkDelay = 2f;
    public float maxBlinkDelay = 5f;

    // Velocidad del pestañeo
    public float blinkDuration = 0.08f;

    private int blinkIndex;



    IEnumerator BlinkLoop()
    {
        while (true)
        {
            // Espera aleatoria antes del siguiente pestañeo
            yield return new WaitForSeconds(Random.Range(minBlinkDelay, maxBlinkDelay));

            // Cerrar ojos
            yield return StartCoroutine(SetBlink(0f, 100f));

            // Abrir ojos
            yield return StartCoroutine(SetBlink(100f, 0f));
        }
    }

    IEnumerator SetBlink(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;

            float value = Mathf.Lerp(from, to, elapsed / blinkDuration);

            playerMeshRenderer.SetBlendShapeWeight(blinkIndex, value);

            yield return null;
        }

        playerMeshRenderer.SetBlendShapeWeight(blinkIndex, to);
    }
}

