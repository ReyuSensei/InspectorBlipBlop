using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    public bool isActive = true;

    Material mat;

    private void Start()
    {
        targetRenderer = this.GetComponent<Renderer>();
        Material[] mats = targetRenderer.materials;
        mat = mats[mats.Length - 1];
        mat.SetFloat("_IsActive", 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

            if (other.CompareTag("Player"))
            {
                mat.SetFloat("_IsActive", 1);
            }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive) return;

            if (other.CompareTag("Player"))
            {
                mat.SetFloat("_IsActive", 0);
            }

    }

    public void OFF()
    {
        mat.SetFloat("_IsActive", 0);
    }
}