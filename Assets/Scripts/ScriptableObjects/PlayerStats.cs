using UnityEngine;

// permite crear archivos asset desde el menu en proyecto
[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Stats/PVP/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float maxHealth = 100f; 
    public float currentHealth; 
    public float walkSpeed = 3.5f;
    public float runSpeed = 5f;
    public float damage;

    public int currentScore; 
    public int currentLevel = 1;


    public MeshRenderer skin;
    //public Asset prefab;

    // metodo para reiniciar valores al iniciar una nueva partida
    public void ResetStats() 
    { 
        currentHealth = maxHealth; 
        currentScore = 0; 
        currentLevel = 1; 
    } 
}
