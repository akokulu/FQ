using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
	public enum ItemType
	{ 
		ARMOR, 
		WEAPON, 
		JUNK 
	};
	
	public Texture2D icon;
	public List<Attribute> attributes = new List<Attribute> ();
	public byte level;
	public ItemType type = ItemType.JUNK;
	public int itemPrice;

	public Item ()
	{
		itemPrice = Random.Range(0,6);
		Debug.Log(itemPrice);
		level = 6;
		Initialize (level, 5);
	}
	
	public string getName ()
	{
		return this.type.ToString () + " Lvl" + level;
//		string name = this.type.ToString ();
//		
//		for (int i = 0; i < attributes.Count; i++) {
//			name += " " + attributes[i].type.ToString();
//			
//			if (attributes [i].modifier > 0) 
//			{
//				name += "+";
//			}
//			name += attributes[i].modifier + " ";
//		}
//		
//		return name.TrimEnd ();
	}

	public string getTooltip ()
	{
		return 
			"Item Level: " + level.ToString() + "\n" + "Item Value: " + itemPrice.ToString();
	}

	private void Initialize (byte BaseLevel, byte Scope, ItemType type)
	{
		setRandomLevel (BaseLevel, Scope);
		this.type = type;
		if (this.type == ItemType.ARMOR) {
			addArmorClass (Scope);
		} else if (this.type == ItemType.WEAPON) {
			addDamageModifiers (Scope);
		} else {
			itemPrice = this.level * Multipliers.PRICE_MODIFIER;
		}
		
		for (byte j = 0; j < level; j++) {
			this.addAttribute ();
		}
		

//		Debug.Log("===============================");
//		Debug.Log("Item Initialized!");
//		Debug.Log("Type: " + this.type);
//		Debug.Log("Price: " + this.itemPrice);
//		Debug.Log("Level: " + this.level);
//		foreach (Attribute attr in attributes) 
//		{
//			Debug.Log(attr.getType() + ": " + attr.getModifier());
//		}
//		Debug.Log("===============================");
		
		
		
	}

	private void Initialize (byte BaseLevel, byte Scope)
	{
		setRandomLevel (BaseLevel, Scope);
		int i = Random.Range (0, 100);
		
		if (i <= 33) {
			Initialize (BaseLevel, Scope, ItemType.ARMOR);
		} else if (i <= 33) {
			Initialize (BaseLevel, Scope, ItemType.WEAPON);
		} else {
			Initialize (BaseLevel, Scope, ItemType.JUNK);
		}
	}
	
	// protected Item(byte BaseLevel, byte Scope) //random type item
	// {		
	// 	Initialize(BaseLevel,Scope);
	// }
	// protected Item(byte level, ItemType type)
	// {
	// 	Initialize(level,0,type);
	// }
	// protected Item(byte level)
	// {
	// 	Initialize(level,0);
	// }
	
	
	public void addArmorClass (int Scope)
	{
		byte i = (byte)Random.Range (this.level - (Scope * 2), this.level + (Scope * 2));
		addAttribute (Attribute.Stat.AC, i);
		itemPrice = i * Multipliers.PRICE_MODIFIER;
	}

	public void addDamageModifiers (int Scope)
	{
		byte i = (byte)Random.Range ((this.level - (Scope * 2)), this.level);
		byte j = (byte)Random.Range (this.level, (this.level + (Scope * 2)));
		addAttribute (Attribute.Stat.MINDMG, i);
		addAttribute (Attribute.Stat.MAXDMG, j);
		itemPrice = (i * Multipliers.PRICE_MODIFIER / 2) + (j * Multipliers.PRICE_MODIFIER / 2);
	}
	
	public void addAttribute ()
	{
		int i = Random.Range (0, 2);
		if (i == 0) {
			this.attributes.Add (new Attribute (Attribute.Stat.STR, this.level));
		} else if (i == 1) {
			this.attributes.Add (new Attribute (Attribute.Stat.DEX, this.level));
		} else {
			this.attributes.Add (new Attribute (Attribute.Stat.CON, this.level));
		}
	}
	
	public void addAttribute (Attribute.Stat type)
	{
		this.attributes.Add (new Attribute (type, this.level));
	}

	public void addAttribute (Attribute.Stat type, byte modifier)
	{
		this.attributes.Add (new Attribute (type, modifier));
	}

	public void addAttribute (Attribute mod)
	{
		this.attributes.Add (mod);
	}

	private void setRandomLevel (byte BaseLevel, byte Scope)
	{
		this.level = (byte)(Random.Range (BaseLevel - Scope, BaseLevel + Scope));
	}

	public int getModifier (Attribute.Stat type)
	{
		int val = 0;
		foreach (Attribute attr in attributes) {
			if (attr.getType () == type) {
				val += attr.getModifier ();
			}
		}
		return val;
	}
	
	/*
	public int getItemValue()
	{
		int val = 0;
		foreach (Attribute attr in attributes) 
		{
			val = attr.getModifier() * Multipliers.VALUE_MODIFIER;
		}
		return val;
	}
	*/
		
	
}
