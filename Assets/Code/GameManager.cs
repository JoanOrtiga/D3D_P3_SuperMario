using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void CoinsChangedFn(ICoinsManager CoinsManager);
public interface ICoinsManager
{
    void AddCoins(int f);
    int getCoins();
    event CoinsChangedFn m_CoinsChangedFn;
}
public class GameManager : MonoBehaviour,ICoinsManager
{
    List<IRestartGameElement> restartGameElements = new List<IRestartGameElement>();

    public Animation HudAnimation;
    public AnimationClip HudInAnimation;
    public event CoinsChangedFn m_CoinsChangedFn;

    public Image LifeBar;
    private int Life = 8;

    public Text CoinsText;
    private int coins = 0;
    private AnimationEvent evt;
    private void Awake()
    {
        DependencyInjector.AddDependency<ICoinsManager>(this);
    }
    void Start()
    {
        evt = new AnimationEvent();
    }
    public void AddRestartGameElement(IRestartGameElement restartGameElement)
    {
        restartGameElements.Add(restartGameElement);
    }

    public void RestartGame()
    {
        foreach (IRestartGameElement item in restartGameElements)
        {
            item.Restart();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HUDIn();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            --Life;
            ShowLifeBar();
        }
    }
    public void HUDIn()
    {
        
            HudAnimation.CrossFade(HudInAnimation.name, 0.0f);
            HudAnimation.Rewind(HudInAnimation.name);
            HudAnimation.Sample();
        
        
    }
    public void Heal()
    {
        ++Life;
        ShowLifeBar();

    }
    public void LoseHeal()
    {
        --Life;
        ShowLifeBar();

    }
    public void LoseHeal(int damage)
    {
        Life = Life - damage;
        ShowLifeBar();
    }
    void ShowLifeBar()
    {
        float Pct = Life / 8.0f;
        LifeBar.fillAmount = Pct;
        if (Life <=2)
        {

            LifeBar.color = Color.red;
        }
        else if (Life >= 6)
        {

            LifeBar.color = Color.green;

        }
        else if(Life<6 &&Life>2)
        {

            LifeBar.color = Color.yellow;
        }
        HUDIn();
    }
    //public void AddCoin()
    //{
    //    ++coins;
    //    CoinsText.text = coins.ToString();
    //}
    public void AddCoins(int value)
    {
        coins+=value;
        m_CoinsChangedFn?.Invoke(this);
    }
    public int getCoins()
    {
        return coins;
    }
}

public interface IRestartGameElement
{
    void Restart();
    void SetRestartPoint();
}


