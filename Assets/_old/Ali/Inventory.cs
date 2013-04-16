using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	public GUISkin inventorySkin;
	public List<Item> inventory;
	
	
	// Use this for initialization
	void Start () {
	inventory = new List<Item>();
		for (int i = 0; i < 5; i++)
		{
			inventory.Add(new Item());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI ()
	{
		//Background Box
		GUI.Box(
			new Rect(10,10,120,((inventory.Count) * 22) + 50),
			"Inventory");
		
		//Populate Inventory Buttons
		for (int i = 0; i < inventory.Count; i++)
		{			
			if(GUI.Button(
					new Rect(
						        20, 
					 40 + (i * 22),
							    100,
						        20), 
					new GUIContent(
						inventory[i].getName(),
						inventory[i].getTooltip()))) 
			{
				//TODO: Click
				Debug.Log ("ButtonClick: " + inventory[i].getName() + " (" + i + ")");
			}	
		}
		
		//Tooltip label position
		GUI.Label (
			new Rect (
				120,
				 10,
				100,
				 100), 
			GUI.tooltip);
	}
}
/*
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
	public List<Attribute> attributes = new List<Attribute>();
	public byte level = 1;
	public ItemType type = ItemType.JUNK;
	public int itemPrice = 1;

	void Start()
	{
		Initialize(level, 0);
	}
	
	private void Initialize(byte BaseLevel, byte Scope, ItemType type)
	{
		setRandomLevel(BaseLevel,Scope);
		this.type = type;
		if (this.type == ItemType.ARMOR) 
		{
			byte i = (byte)Random.Range(this.level - (Scope*2),this.level + (Scope*2));
			addAttribute(Attribute.Stat.AC,i);
			itemPrice = i * Multipliers.PRICE_MODIFIER;
		}
		else if (this.type == ItemType.WEAPON) 
		{
			byte i = (byte)Random.Range((this.level - (Scope*2)),this.level);
			byte j = (byte)Random.Range(this.level,(this.level + (Scope*2)));
			addAttribute(Attribute.Stat.MINDMG,i);
			addAttribute(Attribute.Stat.MAXDMG,j);
			itemPrice = (i * Multipliers.PRICE_MODIFIER / 2) + (j * Multipliers.PRICE_MODIFIER / 2);
		}
		else
		{
			itemPrice = this.level * Multipliers.PRICE_MODIFIER;
		}
		

		Debug.Log("===============================");
		Debug.Log("Item Initialized!");
		Debug.Log("Type: " + this.type);
		Debug.Log("Price: " + this.itemPrice);
		Debug.Log("Level: " + this.level);
		foreach (Attribute attr in attributes) 
		{
			Debug.Log(attr.getType() + ": " + attr.getModifier());
		}
		Debug.Log("===============================");

	}
	
	private void Initialize(byte BaseLevel, byte Scope)
	{
		setRandomLevel(BaseLevel,Scope);
		int i = Random.Range(0,100);
		
		if (i <= 15)
		{
			Initialize(BaseLevel,Scope,ItemType.ARMOR);
		}
		else if (i <= 30)
		{
			Initialize(BaseLevel,Scope,ItemType.WEAPON);
		}
		else 
		{
			Initialize(BaseLevel,Scope,ItemType.JUNK);
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
	
		
	public void addAttribute(Attribute.Stat type)
	{
		this.attributes.Add(new Attribute(type, this.level));
	}
	public void addAttribute(Attribute.Stat type, byte modifier)
	{
		this.attributes.Add(new Attribute(type,modifier));
	}
	public void addAttribute(Attribute mod)
	{
		this.attributes.Add(mod);
	}
	
//	public int getItemValue()
//	{
//		int val = 0;
//		foreach (Attribute attr in attributes) 
//		{
//			val = attr.getModifier() * Multipliers.VALUE_MODIFIER;
//		}
//		return val;
//	}
	
	public int getItemPrice()
	{
		return itemPrice;
	}
	
	private void setRandomLevel(byte BaseLevel, byte Scope)
	{
		this.level = (byte) (Random.Range(BaseLevel - Scope, BaseLevel + Scope));
	}
	
	public int getModifier(Attribute.Stat type)
	{
		int val = 0;
		foreach (Attribute attr in attributes) 
		{
			if (attr.getType() == type) 
			{
				val += attr.getModifier();
			}
		}
		return val;
	}
	
}

*/