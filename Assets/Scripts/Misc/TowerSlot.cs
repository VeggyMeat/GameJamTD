using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    internal GameObject tower = null;
    internal Tower towerScript;

    [SerializeField] private bool blockedSlot;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private int unblockCost;

    /// <summary>
    /// Returns whether the slot is occupied or not
    /// </summary>
    public bool IsOccupied
    {
        get
        {
            return tower is not null;
        }
    }

    /// <summary>
    /// Returns whether the slot is blocked and needs to be unlocked
    /// </summary>
    public bool IsBlocked
    {
        get
        {
            return blockedSlot;
        }
    }

    /// <summary>
    /// Fills the slot with a tower
    /// </summary>
    /// <param name="towerTemplate">The prefab for that tower</param>
    internal void FillSlot(GameObject towerTemplate)
    {
        // spawns in the tower at the spot
        tower = Instantiate(towerTemplate, new Vector3 (transform.position.x, transform.position.y, 0), transform.rotation);
        towerScript = tower.GetComponent<Tower>();

        // sets up the tower, in this slots' position
        towerScript.Setup(this, gameManager);
    }

    /// <summary>
    /// Called when clicked by the player
    /// </summary>
    public void Clicked()
    {
        // if its blocked
        if (IsBlocked)
        {
            // if the player can afford to unblock
            if (gameManager.Money > unblockCost)
            {
                blockedSlot = false;
                gameManager.Money -= unblockCost;
            }
            // otherwise ignore
        }
        // if its not blocked
        else
        {
            // tell the UIManager that this has been clicked
            gameManager.uIManager.SlotClicked(this);
        }
    }
}
