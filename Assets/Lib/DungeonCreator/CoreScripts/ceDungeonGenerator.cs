using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class ceDungeonGenerator
	: MonoBehaviour 
{	
	#region Enums
	
		public enum enDungeonSize
		{
			Small   = 8,
			Medium  = 12,
			Large   = 16,
			Huge    = 24,
		};
	
		public enum enDungeonRoomSize
		{
			Small,
			Medium,
			Large,
		};
		
		public enum enCorridors
		{
			Maze 		 = 30,
			Some		 = 60,
			ConnectRooms = 90,		
		}
		
		public enum enDeadEnds
		{
			Allow  = 0,
			Remove = 100,
		}
	
		public enum enTwists
		{
			Straight = 0,
			Minor    = 50,
			Major    = 100,			
		};
	
	#endregion
	
	#region Members
	
		[SerializeField] private enDungeonSize     		m_dungeonSize     = enDungeonSize.Medium;
		[SerializeField] private enDungeonRoomSize 		m_dungeonRoomSize = enDungeonRoomSize.Medium;
		[SerializeField] private enCorridors		  	m_corridors       = enCorridors.Some;
		[SerializeField] private enDeadEnds        		m_deadEnds        = enDeadEnds.Allow;
		[SerializeField] private enTwists 		  		m_twists          = enTwists.Minor;		
	
		public GameObject floorPrefab;
		public GameObject wallsPrefab;
		public GameObject doorsPrefab;	
		public GameObject playerObject;
		public GameObject monsterObject;
	
		public bool monstersInCorridors = true;
		public bool monstersInRooms 	= true;
		public int  monsterAppearChance = 10;
	
		public float cellSpacingFactor = 1.0f;
		
		public float floorHeightOffset;
		public float wallsHeightOffset;
		public float doorsHeightOffset;
		public float roofsHeightOffset;
		
		private int m_maxRoomCount     = 5;		
		public bool GenerateCeiling = false;
	
	#endregion
	
	#region Properties
	
		public enDungeonSize DungeonSize
		{
			get { return m_dungeonSize;  }
			set { m_dungeonSize = value; }
		}
		
		public enDungeonRoomSize DungeonRoomSize
		{
			get { return m_dungeonRoomSize;  }
			set { m_dungeonRoomSize = value; }
		}
	
		public int MaxRoomCount
		{
			get { return m_maxRoomCount; }
			set
			{
				m_maxRoomCount = Mathf.Clamp(value, 1, 25);
			}
		}
		
		public enCorridors Corridors
		{
			get { return m_corridors;  }
			set { m_corridors = value; }
		}
		
		public enDeadEnds DeadEnds
		{
			get { return m_deadEnds;  }
			set { m_deadEnds = value; }
		}
	
		public enTwists Twists
		{
			get { return m_twists;  }
			set { m_twists = value; }
		}
	
	#endregion
	
	#region Functions
	
		public void DestroyDungeon()
		{
			List<GameObject> children 
				= new List<GameObject>();
			
			foreach (Transform child in this.transform)
				children.Add(child.gameObject);
			
			children.ForEach(child => 
				GameObject.DestroyImmediate(child));

 			children.Clear();
       		children.TrimExcess();

		}
	
	public void CreateDungeon()
	{
		CreateDungeon((int)System.DateTime.Now.Millisecond);
	}

	public void CreateDungeon(int seed)
	{
		csRandom.Instance.Seed(seed);
		csDungeonRoomGenerator rG
			= new csDungeonRoomGenerator();
		
		int roomSizeMin = 2;
		int roomSizeMax = 2;
		
		switch (m_dungeonRoomSize)
		{
			case enDungeonRoomSize.Small:
				roomSizeMin = 2;
				roomSizeMax = 2;
				break;
			case enDungeonRoomSize.Medium:
				roomSizeMin = 2;
				roomSizeMax = 3;
				break;
			case enDungeonRoomSize.Large:
				roomSizeMin = 2;
				roomSizeMax = 4;
				break;
		}
		
		rG.Constructor
			(m_maxRoomCount,
			 roomSizeMin,
		     roomSizeMax,
			 roomSizeMin,
			 roomSizeMax);
		
		csDungeonGenerator dG 
			= new csDungeonGenerator();
		
		dG.Constructor
			((int)m_dungeonSize,
			 (int)m_dungeonSize,
			 (int)m_twists,
		     (int)m_corridors,
			 (int)m_deadEnds,
			 rG);		 
		
		csDungeon dungeon 
			= dG.Generate();
		
		int[,] tiles 
			= csDungeonGenerator.ExpandToTiles
				(dungeon);
		
		if (this.floorPrefab && this.wallsPrefab && this.doorsPrefab)
		{			
			for (int k=0;k<tiles.GetUpperBound(0);k++)
			{
				for (int l=0;l<tiles.GetUpperBound(1);l++)
				{	
					float x = k * cellSpacingFactor;
					float y = l * cellSpacingFactor;
					
					if (tiles[k,l] != (int)csDungeonCell.TileType.Rock && 
						tiles[k,l] != (int)csDungeonCell.TileType.Void)
					{
						GameObject tile 
							= (GameObject)Instantiate
								(this.floorPrefab, new Vector3(x, this.floorHeightOffset, y), Quaternion.identity);
						
						tile.transform.parent 
							= this.transform;
						
						if (this.GenerateCeiling)
						{
						
							tile = (GameObject)Instantiate
									   (this.floorPrefab, new Vector3(x, this.roofsHeightOffset, y), Quaternion.identity);
							
							tile.transform.parent 
								= this.transform;
						}
					}	
					
					if (tiles[k,l] == (int)csDungeonCell.TileType.Rock)
					{
						GameObject tile 
							= (GameObject)Instantiate
								(this.wallsPrefab, new Vector3(x, this.wallsHeightOffset ,y), Quaternion.identity);
						
						tile.transform.parent 
							= this.transform;
					}	 
					
					if (tiles[k,l] == (int)csDungeonCell.TileType.DoorNS)
					{
						GameObject tile 
							= (GameObject)Instantiate
								(this.doorsPrefab, new Vector3(x, this.doorsHeightOffset ,y), Quaternion.identity);
						
						tile.transform.parent 
							= this.transform;
					}	
					
					if (tiles[k,l] == (int)csDungeonCell.TileType.DoorEW)
					{
						GameObject tile
							= (GameObject)Instantiate
								(this.doorsPrefab, new Vector3(x, this.doorsHeightOffset ,y), Quaternion.AngleAxis(90.0f, Vector3.up));
						
						tile.transform.parent 
							= this.transform;
					}	
				}
			}
			
			//Find a dead end to place the player in
			if (this.playerObject)
			{
				for (int k=1;k<tiles.GetUpperBound(0)-1;k++)
				{
					for (int l=1;l<tiles.GetUpperBound(1)-1;l++)
					{	
						if (tiles[k,l]==(int)csDungeonCell.TileType.Corridor)
						{
							int deadEndWeight = 0;
							
							if (tiles[k-1, l-1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							if (tiles[k  , l-1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							if (tiles[k+1, l-1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							if (tiles[k-1, l  ] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							if (tiles[k+1, l  ] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							if (tiles[k-1, l+1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							if (tiles[k  , l+1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							if (tiles[k+1, l+1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
							
							if (deadEndWeight == 7)
							{
								playerObject.transform.position = new Vector3(k * cellSpacingFactor, 1.0f , l * cellSpacingFactor);								
							}							
						}
					}
				}				
			}
			
			if (this.monsterObject)
			{
                bool itemGiven = false;

				for (int k=1;k<tiles.GetUpperBound(0)-1;k++)
				{
					for (int l=1;l<tiles.GetUpperBound(1)-1;l++)
					{	
						if (tiles[k,l] == (int)csDungeonCell.TileType.Room && monstersInRooms == true)
						{
							if (Random.Range(0, 100) < monsterAppearChance)
							{
                                GameObject monster 
									= (GameObject)
										Instantiate(monsterObject, new Vector3(k * cellSpacingFactor, 1.05f ,l * cellSpacingFactor), Quaternion.identity);
								
								monster.transform.parent = this.transform;

                                if (!itemGiven)
                                {
                                    monster.GetComponentInChildren<EnemyProps>().hasItem = true;  // Give this monster the item!!!
                                    itemGiven = true;
                                }
							}
						}
						
						if (tiles[k,l] == (int)csDungeonCell.TileType.Corridor && monstersInCorridors == true)
						{
							if (Random.Range(0, 100) < monsterAppearChance)
							{
								GameObject monster 
									= (GameObject)
										Instantiate(monsterObject, new Vector3(k * cellSpacingFactor, 1.05f ,l * cellSpacingFactor), Quaternion.identity);
								
								monster.transform.parent = this.transform;

                                if (!itemGiven)
                                {
                                    monster.GetComponentInChildren<EnemyProps>().hasItem = true;  // Give this monster the item!!!
                                    itemGiven = true;
                                }
							}
						}
					}
				}				
			}
		}
	}
	
	#endregion
}
