using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject moneyText;
    [SerializeField] private GameObject healthText;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private string moneyPrefix = "Money: ";
    [SerializeField] private string healthPrefix = "Health: ";

    [SerializeField] private List<int> cannonCosts;
    [SerializeField] private List<int> mortarCosts;
    [SerializeField] private List<int> flamethrowerCosts;

    [SerializeField] private Dictionary<string, List<int>> costs;

    private SelectionState selectionState = SelectionState.None;

    private void Start()
    {
        costs = new Dictionary<string, List<int>>()
        {
            { "cannon", cannonCosts },
            { "mortar", mortarCosts },
            { "flamethrower", flamethrowerCosts }
        };

        UpdateMoneyText(gameManager.Money);
        UpdateHealthText(gameManager.Health);
    }

    /// <summary>
    /// Updates the text box containing money
    /// </summary>
    /// <param name="money">The current amount of money</param>
    public void UpdateMoneyText(float money)
    {

        moneyText.GetComponent<TextMeshProUGUI>().text = moneyPrefix + money;
    }

    /// <summary>
    /// Updates the text box containing health
    /// </summary>
    /// <param name="health">The current amount of health</param>
    public void UpdateHealthText(float health)
    {
        healthText.GetComponent<TextMeshProUGUI>().text = healthPrefix + health;
    }

    /// <summary>
    /// Cancels the selection
    /// </summary>
    public void Cancel()
    {
        selectionState = SelectionState.None;
    }

    /// <summary>
    /// Sets the selections state to Flamethrower
    /// </summary>
    public void Flamethrower()
    {
        selectionState = SelectionState.Flamethrower;
    }

    /// <summary>
    /// Sets the selection state to Cannon
    /// </summary>
    public void Cannon()
    {
        selectionState = SelectionState.Cannon;
    }

    /// <summary>
    /// Sets the selection state to Mortar
    /// </summary>
    public void Mortar()
    {
        selectionState = SelectionState.Mortar;
    }

    /// <summary>
    /// Called when a slot is clicked
    /// </summary>
    /// <param name="slot">The slot that was clicked</param>
    public void SlotClicked(TowerSlot slot)
    {
        // if there is a tower already in there
        if (slot.IsOccupied)
        {
            // if its already max level
            if (slot.towerScript.Level == 2)
            {
                // ignore
                return;
            }

            // calculates the cost of leveling up
            int cost = costs[slot.towerScript.TowerName][slot.towerScript.Level + 1];

            // if there is enough money level it up
            if (gameManager.Money > cost)
            {
                // remove that amount of money
                gameManager.Money -= cost;

                // level it up
                slot.towerScript.LevelUp();
            }
            // otherwise do nothing
        }
        // otherwise
        else
        {
            // depending on what has been selected
            switch(selectionState)
            {
                case SelectionState.None:
                    // if nothing selected, do nothing
                    break;

                case SelectionState.Flamethrower:
                    // if flamethrower selected and there is enough money, place a flamethrower
                    if (gameManager.Money > costs["flamethrower"][0])
                    {
                        // remove that money
                        gameManager.Money -= costs["flamethrower"][0];

                        // add the flamethrower to the slot
                        slot.FillSlot(gameManager.Towers[SelectionState.Flamethrower]);
                    }
                    break;

                case SelectionState.Cannon:
                    // if flamethrower selected and there is enough money, place a cannon
                    if (gameManager.Money > costs["cannon"][0])
                    {
                        // remove that money
                        gameManager.Money -= costs["cannon"][0];

                        // add the flamethrower to the slot
                        slot.FillSlot(gameManager.Towers[SelectionState.Cannon]);
                    }
                    break;

                case SelectionState.Mortar:
                    // if flamethrower selected and there is enough money, place a mortar
                    if (gameManager.Money > costs["mortar"][0])
                    {
                        // remove that money
                        gameManager.Money -= costs["cannon"][0];

                        // add the flamethrower to the slot
                        slot.FillSlot(gameManager.Towers[SelectionState.Mortar]);
                    }
                    break;
            }
        }
    }
}
