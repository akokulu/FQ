using UnityEngine;
using System.Collections;

public class Player : Mobile {
	
	
    private int exp; 
    private Armor armor = null;
    private Weapon weapon = null;
	
	// Player(int str, int dex, int con) : base(str, dex, con){}	
	
	public void setArmor(Armor armor)
	{
		this.inventory.Add(this.armor);
		this.armor = armor;
	}
    public void setWeapon(Weapon weapon)
	{
		this.inventory.Add(this.weapon);
		this.weapon = weapon;
	}
	
    public void addEXP(int xp)
	{
		exp += xp;
	}
    public int getEXP()
	{
		return exp;
	}
	
    public override float getAC()
	{
		//extract Armor Class from Armor & DEX(?)
		return armor.getModifier(Attribute.Stat.AC);
	}
    public override float getDMG()
	{
		//extract Damage from Weapon & STR
		return base.str + Random.Range(weapon.getModifier(Attribute.Stat.MINDMG),weapon.getModifier(Attribute.Stat.MAXDMG)) ;
	}
	
	public override int getMaxHealth()
	{
		return (base.con + armor.getModifier(Attribute.Stat.CON)) * Multipliers.HEALTH_CON;
	}
	
	
	
}
