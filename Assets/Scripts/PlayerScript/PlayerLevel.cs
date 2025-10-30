using UnityEngine;
using UnityEngine.Events;

public class PlayerLevel : MonoBehaviour
{
    public int currentLevel = 1;    
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public ThanhXP xpBar; // Reference to the XP bar UI
    public UnityEvent onLevelUp;
    public void Start()
    {
        GameObject xpBarObject = GameObject.Find("ThanhXP");
        xpBar = xpBarObject.GetComponent<ThanhXP>();
        if (xpBar != null)
        {
            xpBar.SetXP(currentXP, xpToNextLevel);
        }
    }
    public void AddXP(int amount)
    {
        currentXP += amount;
        xpBar.SetXP(currentXP, xpToNextLevel);
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.1f); // Increase XP needed for next level
        Debug.Log("Leveled Up! New Level: " + currentLevel);
        onLevelUp.Invoke();
    }

}
