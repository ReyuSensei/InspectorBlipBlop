using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.gameplayUI = this; //  LE DIGO AL GAMEMANAGER QUE EL GESTOR DE GAMEPLAYUI SOY YO, CREANDO UNA REFERENCIA INDIRECTA
    }

    public void UpdateHealthBar(float barFillAmount)
    {
        healthBar.fillAmount = barFillAmount;
    }
}
