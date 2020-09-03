using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class User  {
    
    private static User mInstance = null;
    public long overallScore = 1000000;
	public long powerLevel = 1;
	public long speedLevel = 1;
    public long hightScore = 0;

    public string namePlayer = "Player_00";                 
    public int countShotPlayer = 1;

    public string nameBall = "Ball_0";                   
    public int colorBall = 0;


    private Dictionary<string, TypeOwn> ownedCharacters= new Dictionary<string, TypeOwn>();
    private Dictionary<int, TypeOwn> ownedGuns = new Dictionary<int, TypeOwn>();

    private Dictionary<string, TypeOwn> ownedBalls = new Dictionary<string, TypeOwn>();
    private Dictionary<int, TypeOwn> ownedColorToBalls = new Dictionary<int, TypeOwn>();

    public List<CharacterToBuy> listPlayer;

    public TypeShop typeShopSelect = TypeShop.player;

    public TypeOwn getCharacterType(string name) {
        
        if (ownedCharacters.ContainsKey(name))  
        {
            TypeOwn? _char = ownedCharacters[name];
            return _char.Value;
            
        }
        else {
            return TypeOwn.ToBuy;
        }
    }

    public void setCharacterType(string name, TypeOwn type) 
    {
        ownedCharacters[name] = type;
    }

    public TypeOwn getBallType(string name)
    {
        if (ownedBalls.ContainsKey(name))
        {
            TypeOwn? _char = ownedBalls[name];
            return _char.Value;
        }
        else
        {
            return TypeOwn.ToBuy;
        }
    }
    public void setBallType(string name, TypeOwn type)
    {
        ownedBalls[name] = type;
    }
    public void setGunType(int id, TypeOwn type)
    {
        ownedGuns[id] = type;
    }
    public TypeOwn getGunType(int name)
    {
        if (ownedGuns.ContainsKey(name))
        {
            TypeOwn? _char = ownedGuns[name];
            return _char.Value;
        }
        else
        {
            return TypeOwn.ToBuy;
        }
    }
    public void setColorBallType(int id, TypeOwn type)
    {
        ownedColorToBalls[id] = type;
    }
    public TypeOwn getColorBallType(int name)
    {
        if (ownedColorToBalls.ContainsKey(name))
        {
            TypeOwn? _char = ownedColorToBalls[name];
            return _char.Value;
        }
        else
        {
            return TypeOwn.ToBuy;
        }
    }
    public List<ItemToBuy> listBall;
    public List<ShootToBuy> Player_Gun; 
    public List<ColorToBuy> listBallColor;  

    private static string PATH_TO_SAVE = Application.persistentDataPath + "/usrData_1110000";
    public static User Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new User();
                mInstance.init();
            }
            return mInstance;
        }
    }
    public void saveUser()
    {
		try
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(PATH_TO_SAVE);
            bf.Serialize(file, this);           
            file.Close();
		}
		catch (System.Exception e) {
			Debug.Log("saving error " + e.Message);
		}
	}

	public void loadUser()
    {
        User u;
        if (File.Exists(PATH_TO_SAVE))
        {
            try
            {
                BinaryFormatter bf2 = new BinaryFormatter();
                FileStream file2 = File.Open(PATH_TO_SAVE, FileMode.Open);
                u = (User)bf2.Deserialize(file2);
                if (u != null)
                {
                    this.overallScore = u.overallScore;
                    this.powerLevel = u.powerLevel;
                    this.speedLevel = u.speedLevel;
                    this.hightScore = u.hightScore;
                    this.namePlayer = u.namePlayer;
                    this.countShotPlayer = u.countShotPlayer;
                    this.nameBall = u.nameBall;
                    this.colorBall = u.colorBall;
                    this.ownedCharacters = u.ownedCharacters;    
                    this.ownedBalls = u.ownedBalls;
                    this.Player_Gun = u.Player_Gun;
                    this.listBallColor = u.listBallColor;
                    this.ownedColorToBalls = u.ownedColorToBalls;
                }  
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }    
    }

    public long getSpeedUpCost()
    {
		long cost = 4;
        float mult = 1.2f;
        if (speedLevel > 75)
        {
            mult = 40;
        }
        if (speedLevel > 50)
        {
            mult = 20;
        }
        else if (speedLevel > 30)
        {
            mult = 7;
        }
        else if (speedLevel > 20)
        {
            mult = 5.0f;
        }
        else if (speedLevel > 10)
        {
            mult = 2.2f;
        }
        cost *= ((long)((6 * speedLevel) * (mult + (speedLevel * 0.3)) * speedLevel/2) );
		return cost/2;
	}

	public bool buySpeedUp()
    {
		long cost = getSpeedUpCost ();
		if (overallScore >= cost ) 
        {
			overallScore -= cost;
			speedLevel++;
            saveUser();
            return true;
		} 
        else 
        {
			return false;
		}
    }

    public long getPowerUpCost()
    {
		long cost = 4;
        float mult = 1.2f;
        if (powerLevel > 25)
        {
            mult = 40;
        }
        if (powerLevel > 20)
        {
            mult = 20;
        }
        else if (powerLevel > 15)
        {
            mult = 7;
        }
        else if (powerLevel > 10)
        {
            mult = 5.0f;
        }
        else if (powerLevel > 5)
        {
            mult = 2.2f;
        }
        cost *= ((long)((6 * powerLevel) * (mult + (powerLevel* 0.3)) * powerLevel/2));
		return cost;
	}

	public bool buyPowerUp()
    {
		long cost = getPowerUpCost ();
		if (overallScore >= cost) 
        {
			overallScore -= cost;
			powerLevel++;
            saveUser ();
			return true;
		} 
        else 
        {
			return false;
		}
	}
    
    [System.Serializable]
    public class ItemToBuy{
		public string name;
		public TypeOwn type;
		public int cost;
        public List<ColorToBuy> listColor;

		public ItemToBuy(string name, int cost)
        {
			this.name = name;

			this.cost = cost;

		}
		public int Getcost()
		{
			return cost;
		}
        public List<ColorToBuy> GetlistColor()
        {
            return listColor;
        }
    }
    [System.Serializable]
    public class CharacterToBuy
    {
        public string name;
        
        public int cost;
        public CharacterToBuy(string name, int cost)
        {
            this.name = name;
            this.cost = cost;

        }
        public int Getcost()
        {
            return cost;
        }

    }
    [System.Serializable]
    public class ColorToBuy{
		public int color;
		public int cost;

		public ColorToBuy(int color, int cost ){
			this.color = color;
			this.cost = cost;
		}
	}
    [System.Serializable]
    public class ShootToBuy
    {
        public int countShot;
        public int cost;
        public ShootToBuy(int countShot, int cost)
        {
            this.countShot = countShot;
            this.cost = cost;
        }
    }
    public enum TypeOwn{
        ToBuy, Bought, Chosen
	}

	public void init(){
        Player_Gun = new List<ShootToBuy>();
        Player_Gun.Add(new ShootToBuy(1, 0));
        Player_Gun.Add(new ShootToBuy(2, 10000));
        Player_Gun.Add(new ShootToBuy(3, 80000));
        Player_Gun.Add(new ShootToBuy(4, 250000));
        Player_Gun.Add(new ShootToBuy(5, 500000));
        
        listPlayer = new List<CharacterToBuy>();
		listPlayer.Add(new CharacterToBuy("Player_00", 0));
        listPlayer.Add(new CharacterToBuy("Player_01", 10000));
        listPlayer.Add(new CharacterToBuy("Player_02", 20000));
        listPlayer.Add(new CharacterToBuy("Player_03", 50000));
        listPlayer.Add(new CharacterToBuy("Player_04", 100000));

        listBallColor = new List<ColorToBuy>();
        listBallColor.Add(new ColorToBuy(0,  0));
        listBallColor.Add(new ColorToBuy(1,  1000));
        listBallColor.Add(new ColorToBuy(2,  10000));
        listBallColor.Add(new ColorToBuy(3,  100000));

        listBall = new List<ItemToBuy>();
        listBall.Add(new ItemToBuy("Ball_0", 0));
        listBall.Add(new ItemToBuy("Ball_1", 10000));
        listBall.Add(new ItemToBuy("Ball_2", 100000));

        ownedCharacters = new Dictionary<string, TypeOwn>();
        ownedCharacters["Player_00"] = TypeOwn.Chosen;
        ownedBalls = new Dictionary<string, TypeOwn>();
        ownedBalls["Ball_0"] = TypeOwn.Chosen;
        ownedGuns = new Dictionary<int, TypeOwn>();
        ownedGuns[0] = TypeOwn.Chosen;
        ownedColorToBalls = new Dictionary<int, TypeOwn>();
        ownedColorToBalls[0] = TypeOwn.Chosen;
    }
    public Color GetColor(int index)
    {
        switch (index)
        {
            case 0:
                return new Color(0, 0.8588235f, 0, 1);
            case 1:
                return new Color(1, 1, 0, 1);
            case 2:
                return Color.red;
            case 3:
                return Color.cyan;

        }
        return Color.white;
    }
    public enum TypeShop { player,ball,power}
}

