using UnityEngine;
using System.Collections.Generic;

public abstract class Mobile : MonoBehaviour
{
	
    public byte level; // Level

    public int str; // Damage
    public int dex; // Max AP
    public int con; // Max Health
	
	public int health;
    public int gold; // Gold
    public int AP;


    public List<Item> inventory;

	public bool checkAP (int amount)
	{
		return (this.AP < amount);
	}
    public int useAP(byte amount)
	{
		return (this.AP - amount);
	}
	
	
	public int getMaxAP()
	{
		return dex * (int)Multipliers.AP_DEX;
	}
 //    public int getSTR()
	// {
	// 	return str;
	// }
 //    public int getDEX()
	// {
	// 	return dex;
	// }
 //    public int getCON()
	// {
	// 	return con;
	// }
	// public byte getLVL()
	// {
	// 	return level;
	// }
	public int Damage(int dmg)
	{
		health -= dmg;	
		return health;
	}
	
	// public int getHealth()
	// {
	// 	return health;
	// }
	
	public abstract int getMaxHealth();
    public abstract float getAC();
    public abstract float getDMG();
	
	// public Mobile(byte level)
	// {
	// 	Initialize(level);
	// }
	// public Mobile(int str, int dex, int con)
	// {
	// 	Initialize(str, dex, con);
	// }

	private void Initialize(byte level)
	{
		this.level = level;
		this.str = level * Multipliers.LEVEL_ENEMYSTATS;
		this.dex = level * Multipliers.LEVEL_ENEMYSTATS;
		this.con = level * Multipliers.LEVEL_ENEMYSTATS;
		this.health = getMaxHealth();
		this.AP = getMaxAP();
		this.gold = level * Multipliers.LEVEL_GOLD;
		
		Debug.Log("===============================");
		Debug.Log("Enemy Initialized!");
		Debug.Log("Level: " + this.level);
		Debug.Log("STR: " + this.str);
		Debug.Log("DEX: " + this.dex);
		Debug.Log("CON: " + this.con);
		Debug.Log("Health: " + this.health);
		Debug.Log("AP: " + this.AP);
		Debug.Log("Gold: " + this.gold);
		Debug.Log("===============================");
	}
	private void Initialize(int str, int dex, int con)
	{
		this.level = 1;
		this.str = str;
		this.dex = dex;
		this.con = con;
		this.health = getMaxHealth();
		this.AP = getMaxAP();
		this.gold = 100;
	}
}
