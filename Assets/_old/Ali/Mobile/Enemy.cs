using UnityEngine;
using System.Collections;

public class Enemy : Mobile {
	

	// Enemy(byte level) : base(level){}	
	
    public override float getAC()
	{
		//extract Armor Class from DEX
		return 0;
	}
    public override float getDMG()
	{
		//extract Damage from STR
		return 0;
	}
	public override int getMaxHealth()
	{
		return base.con * Multipliers.HEALTH_CON;
	}
	
}
