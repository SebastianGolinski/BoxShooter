using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ManagerChangePlayer : MonoBehaviour {

    public GameObject[] arrow;
    public GameObject playerSelect;
    public GameObject ballSelect;
    public GameObject playerList;
    public GameObject ballList;
    public Text buttonToBuyDown;
    public Text buttonToBuyUp;
    public Text score;
    public static whichMoveType whichMove;
    private ThingsToBuy thingsToBuy;
    public Image ballButton;
    public Image playerButton;
    public Text ballButtonText;
    public Text playerButtonText;
    public Sprite iconUIDefault;
    public Sprite iconUiEvent;
    private void Awake()
    {
        User.Instance.loadUser();
        MoveTopScroll.tempTopItem = GameObject.Find("Player_00");
    }
    void Start() {
        
        whichMove = whichMoveType.player;
        ChangeSelectTypeShop();
        buttonToBuyUp.text = "text";
        score.text = User.Instance.overallScore.ToString()+" pts";
    }

    void Update()
    {
        refreshTextButton();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitButton();
        }
        score.text = User.Instance.overallScore.ToString() + " pts";
    }
    private void ChangeSelectTypeShop()
    {
        if (User.Instance.typeShopSelect == User.TypeShop.player)
        {
            ballButton.sprite = iconUIDefault;
            playerButton.sprite = iconUiEvent;
            ballButtonText.color = Color.yellow;
            playerButtonText.color = Color.black;
            playerSelect.SetActive(true);
            ballSelect.SetActive(false);
            thingsToBuy = ThingsToBuy.player;
            User.Instance.typeShopSelect = User.TypeShop.player;
        }
        else
        {
            ballButton.sprite = iconUiEvent;
            playerButton.sprite = iconUIDefault;
            ballButtonText.color = Color.black;
            playerButtonText.color = Color.yellow;
            playerSelect.SetActive(false);
            ballSelect.SetActive(true);
            thingsToBuy = ThingsToBuy.ball;
            User.Instance.typeShopSelect = User.TypeShop.ball;
        }
    }

    void refreshTextButton()
    {
        if (MoveDownScroll.tempDownItem != null)
        {
            ChangeTextButtonGunColor();
            ChangeTextButtonPlayerBall();
        }
    }
    public void ExitButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void DragButtonPlayer()
    {
        whichMove = whichMoveType.player;  
    }
    public void DragButtonColor()
    {
        whichMove = whichMoveType.color;
    }
    public void PlayerShowButton()
    {
        User.Instance.typeShopSelect = User.TypeShop.player;
        thingsToBuy = ThingsToBuy.player;
        ChangeSelectTypeShop();
        arrow[0].SetActive(true);
        arrow[1].SetActive(true);
        arrow[2].SetActive(true);
        arrow[3].SetActive(true);
    }
    public void BallShowButton()
    {
        User.Instance.typeShopSelect = User.TypeShop.ball;
        GameObject[] pl = GameObject.FindGameObjectsWithTag("Shop_player");
        foreach (GameObject p in pl)
        {
            p.transform.eulerAngles = new Vector3(-10, 0, 0);
            p.transform.Rotate(new Vector3(-14, 0, 0));
        }
        ChangeSelectTypeShop();
        arrow[0].SetActive(true);
        arrow[1].SetActive(true);
        arrow[2].SetActive(true);
        arrow[3].SetActive(true);
        thingsToBuy = ThingsToBuy.ball;
    }
    public void ButtonToBuyPlayerBall() 
    {
        if (thingsToBuy == ThingsToBuy.player) 
        {
            User.TypeOwn playerType = User.Instance.getCharacterType(MoveTopScroll.tempTopItem.name);
            if (playerType == User.TypeOwn.ToBuy) 
            {
                if (getPlayer().cost < User.Instance.overallScore) 
                {
                    User.Instance.overallScore -= getPlayer().cost;
                    User.Instance.setCharacterType(MoveTopScroll.tempTopItem.name, User.TypeOwn.Bought);
                    User.Instance.saveUser();
                }
            } 
            else if (playerType == User.TypeOwn.Bought) 
            {
                ChoosePlayerBall();
                User.Instance.saveUser();
            }
        }
        else if(thingsToBuy == ThingsToBuy.ball)
        {
            User.TypeOwn ballType = User.Instance.getBallType(MoveTopScroll.tempTopItem.name);
            if (ballType == User.TypeOwn.ToBuy)
            {
                if (getBall().cost < User.Instance.overallScore) 
                {
                    User.Instance.setBallType(MoveTopScroll.tempTopItem.name, User.TypeOwn.Bought);
                    User.Instance.overallScore -= getBall().cost;
                    User.Instance.saveUser();
                }
            }
            else if (ballType == User.TypeOwn.Bought) 
            {
                ChoosePlayerBall();
                User.Instance.saveUser();
            }
        }
        score.text = User.Instance.overallScore.ToString();
    }
    public void ButtonToBuyGunColor() 
    {
        if (thingsToBuy == ThingsToBuy.player) 
        {
            User.TypeOwn playerType = User.Instance.getCharacterType(MoveTopScroll.tempTopItem.name);
            User.TypeOwn gunType = User.Instance.getGunType(GetPlayerGun().countShot-1);
            if (gunType == User.TypeOwn.ToBuy && playerType != User.TypeOwn.ToBuy) 
            {
                if (GetPlayerGun().cost < User.Instance.overallScore) 
                {
                    User.Instance.overallScore -= GetPlayerGun().cost;
                    User.Instance.setGunType(GetPlayerGun().countShot - 1, User.TypeOwn.Bought);
                    User.Instance.saveUser();
                }
            }
            else if (gunType == User.TypeOwn.Bought) {
                ChooseGunColor();
                User.Instance.saveUser();
            }
        } 
        else if(thingsToBuy == ThingsToBuy.ball && MoveDownScroll.tempDownItem.tag.Equals("Shop_color"))
        {
            User.TypeOwn ballType = User.Instance.getBallType(MoveTopScroll.tempTopItem.name);
            User.TypeOwn colorBallType = User.Instance.getColorBallType(GetBallColor().color);
            if (colorBallType == User.TypeOwn.ToBuy && ballType != User.TypeOwn.ToBuy) 
            {
                if (GetBallColor().cost < User.Instance.overallScore) 
                {
                    User.Instance.overallScore -= GetBallColor().cost;
                    User.Instance.setColorBallType(GetBallColor().color, User.TypeOwn.Bought);
                    User.Instance.saveUser();
                }
            } 
            else if (colorBallType == User.TypeOwn.Bought) 
            {
                ChooseGunColor();
                User.Instance.saveUser();
            }
        }
        score.text = User.Instance.overallScore.ToString();
    }
    void ChangeTextButtonPlayerBall() 
    {
        if (thingsToBuy == ThingsToBuy.player && buttonToBuyDown != null) 
        {
            User.TypeOwn playerType = User.Instance.getCharacterType(MoveTopScroll.tempTopItem.name);
            if (MoveTopScroll.tempTopItem != null )
            {
                if (playerType == User.TypeOwn.ToBuy)
                {
                    buttonToBuyDown.text = "Kup\n " + getPlayer().cost.ToString()+ " pts";
                }
                else if (playerType == User.TypeOwn.Bought)
                {
                    buttonToBuyDown.text = "Wybierz";
                }
                else
                {
                    buttonToBuyDown.text = "Wybrane";
                }
            }
        } 
        else if(thingsToBuy == ThingsToBuy.ball && MoveDownScroll.tempDownItem.tag.Equals("Shop_color") && buttonToBuyDown != null)
        {
            User.TypeOwn ballType = User.Instance.getBallType(MoveTopScroll.tempTopItem.name);
            if (ballType == User.TypeOwn.ToBuy) 
            {
                buttonToBuyDown.text = "Kup\n " + getBall().cost.ToString() + " pts";
            }
            else if (ballType == User.TypeOwn.Bought) 
            {
                buttonToBuyDown.text = "Wybierz";
            }
            else
            {
                buttonToBuyDown.text = "Wybrane";
            }
        }
    }
    void ChangeTextButtonGunColor() {
        if (thingsToBuy == ThingsToBuy.player && buttonToBuyUp != null) 
        {
            User.TypeOwn playerType = User.Instance.getCharacterType(MoveTopScroll.tempTopItem.name);
            User.TypeOwn gunType = User.Instance.getGunType(GetPlayerGun().countShot - 1);
            if (playerType != User.TypeOwn.ToBuy)
            {
                if (gunType == User.TypeOwn.ToBuy)
                {
                    buttonToBuyUp.text = "Kup\n " + GetPlayerGun().cost.ToString() + " pts";
                }
                else if (gunType == User.TypeOwn.Bought)
                {
                    buttonToBuyUp.text = "Wybierz";
                }
                else
                {
                    buttonToBuyUp.text = "Wybrane";
                }
            }
            else
            {
                buttonToBuyUp.text = "Kup postac";
            }
        } 
        else if(thingsToBuy == ThingsToBuy.ball && MoveDownScroll.tempDownItem.tag.Equals("Shop_color") && buttonToBuyUp != null)
        {
            User.TypeOwn ballType = User.Instance.getBallType(MoveTopScroll.tempTopItem.name);
            User.TypeOwn colorBallType = User.Instance.getColorBallType(GetBallColor().color);
            if (ballType != User.TypeOwn.ToBuy )
            {
                if (colorBallType == User.TypeOwn.ToBuy ) {
                    buttonToBuyUp.text = "Kup\n " + GetBallColor().cost.ToString() + " pts";
                } 
                else if (colorBallType == User.TypeOwn.Bought) 
                {
                    buttonToBuyUp.text = "Wybierz";
                } 
                else {

                    buttonToBuyUp.text = "Wybrane";
                }
            }
            else
            {
                buttonToBuyUp.text = "Kup pocisk";
            }
        }

    }

    void ChoosePlayerBall() {
        if (thingsToBuy == ThingsToBuy.player) {
            
            foreach (User.CharacterToBuy pl in User.Instance.listPlayer)
            {
                User.TypeOwn playerType = User.Instance.getCharacterType(pl.name);
                if (playerType == User.TypeOwn.Chosen)
                {
                    User.Instance.setCharacterType(pl.name, User.TypeOwn.Bought);
                }
            }
            User.Instance.setCharacterType(MoveTopScroll.tempTopItem.name, User.TypeOwn.Chosen);
            User.Instance.namePlayer = MoveTopScroll.tempTopItem.name;
        } 
        else if(thingsToBuy == ThingsToBuy.ball && MoveDownScroll.tempDownItem.tag.Equals("Shop_color")) 
        {
            foreach (User.ItemToBuy bal in User.Instance.listBall)
            {
                User.TypeOwn ballType = User.Instance.getBallType(bal.name);
                if (ballType == User.TypeOwn.Chosen)
                {
                    User.Instance.setBallType(bal.name, User.TypeOwn.Bought);
                }
            }
            User.Instance.setBallType(MoveTopScroll.tempTopItem.name, User.TypeOwn.Chosen);
            User.Instance.nameBall = MoveTopScroll.tempTopItem.name;
        }
    }
    void ChooseGunColor() {
        if (thingsToBuy == ThingsToBuy.player) 
        {
            foreach (User.ShootToBuy pl in User.Instance.Player_Gun)
            {
                User.TypeOwn gunType = User.Instance.getGunType(pl.countShot - 1);
                if (gunType == User.TypeOwn.Chosen)
                {
                    User.Instance.setGunType(pl.countShot - 1,User.TypeOwn.Bought);
                }
            }
            User.Instance.setGunType(GetPlayerGun().countShot-1, User.TypeOwn.Chosen);
            User.Instance.countShotPlayer = GetPlayerGun().countShot;
        } 
        else if(thingsToBuy == ThingsToBuy.ball && MoveDownScroll.tempDownItem.tag.Equals("Shop_color"))
        {
            foreach (User.ColorToBuy pl in User.Instance.listBallColor)
            {
                User.TypeOwn colorBallType = User.Instance.getColorBallType(pl.color);
                if (colorBallType == User.TypeOwn.Chosen)
                {
                    User.Instance.setColorBallType(pl.color, User.TypeOwn.Bought);                   
                }
            }
            User.Instance.setColorBallType(GetBallColor().color, User.TypeOwn.Chosen);
            User.Instance.colorBall = GetBallColor().color;
        }
    }

    public User.CharacterToBuy getPlayer()
    {
        foreach (User.CharacterToBuy item in User.Instance.listPlayer)
        {
            if (item.name == MoveTopScroll.tempTopItem.name)
            {
                return item;
            }
        }
        return User.Instance.listPlayer[0];
    }
    public User.ItemToBuy getBall()
    {
        foreach (User.ItemToBuy item in User.Instance.listBall)
        {
            if (item.name == MoveTopScroll.tempTopItem.name)
            {
                return item;
            }
        }
        return User.Instance.listBall[0];

    }
    public User.ShootToBuy GetPlayerGun()
    {
        foreach (User.ShootToBuy item in User.Instance.Player_Gun)
        {
            if (item.countShot == MoveDownScroll.tempDownItem.transform.childCount)
            {
                return item;
            }
        }
        return User.Instance.Player_Gun[0];
    }

    public User.ColorToBuy GetBallColor()
    {
        foreach (User.ColorToBuy item in User.Instance.listBallColor)
        {
            if (User.Instance.GetColor(item.color) == MoveDownScroll.tempDownItem.GetComponent<Renderer>().material.color)
            {
                return item;
            }
        }
        return User.Instance.listBallColor[0];
    }
    public int GetIndexBallColorBall()
    {
        int i = 0;
        foreach (User.ColorToBuy t in User.Instance.listBallColor)
        {
            if (User.Instance.colorBall == t.color)
            {
                i = User.Instance.listBallColor.IndexOf(t);
            }
        }
        return i;
    }
    public enum whichMoveType
    {
        player, color
    }
    public enum ThingsToBuy
    {
        player, ball,other
    }
}
