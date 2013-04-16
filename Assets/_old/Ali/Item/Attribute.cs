using UnityEngine;

[System.Serializable]
public class Attribute
{

	public enum Stat
	{
		STR,
		DEX,
		CON,
		MINDMG,
		MAXDMG,
		AC
	};
	public byte modifier;
	public Stat type;
	public byte level;

	public Attribute (byte level, Stat type, byte modifier) //Manually created modifier
	{
		Initialize (level, type, modifier);
	}

	public Attribute (byte level) //creates random type modifier based on the level
	{
		Initialize (level);
	}

	public Attribute (byte minLevel, byte maxLevel) //creates random type modifier based on the level
	{
		Initialize ((byte)Random.Range (minLevel, maxLevel));
	}

	public Attribute (Stat type, byte level)  //Create modifier of given type based on the specified level
	{
		Initialize (type, level);
	}
	
	private void Initialize (byte level, Stat type, byte modifier)
	{
		this.level = level;
		this.type = type;
		this.modifier = modifier;
	}

	private void Initialize (Stat type, byte level)
	{
		this.level = level;
		this.type = type;
		setRandomModifier ();
	}

	private void Initialize (byte level)
	{
		this.level = level;
		setRandomType ();
		setRandomModifier ();
	}
		
	private void setRandomType () //Sets a random Stat
	{
		int i = Random.Range (0, 2);
		
		if (i == 0) {
			this.type = Stat.CON;
		} else if (i == 1) {
			this.type = Stat.DEX;
		} else {
			this.type = Stat.STR;
		}
	}

	private void setRandomModifier ()
	{
		this.modifier = (byte)(Random.Range ((this.level - 1) * Multipliers.MODIFIER_ITEMLEVEL, this.level * Multipliers.MODIFIER_ITEMLEVEL));
	}
	
	public byte getModifier ()
	{
		return modifier;
	}

	public Stat getType ()
	{
		return this.type;
	}
}
