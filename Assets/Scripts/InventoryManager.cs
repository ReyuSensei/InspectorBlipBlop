using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<string> items = new List<string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Add (string id)
    {
        items.Add(id);
    }

    public bool Has(string id)
    {
        return items.Contains(id);
    }
}