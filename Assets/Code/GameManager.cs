using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void CoinsChangedFn(ICoinsManager CoinsManager);
public delegate void LifeChangefn(IHpManager HpManager);

public interface ICoinsManager
{
    void AddCoins(int f);
    int GetCoins();
    event CoinsChangedFn m_CoinsChangedFn;
}
public interface IHpManager
{
    void AddLife(float f);
    float GetLife();
    void LoseLife(float f);
    event LifeChangefn m_LifeChangeFn;
}
public class GameManager : MonoBehaviour,ICoinsManager,IHpManager
{
    List<IRestartGameElement> restartGameElements = new List<IRestartGameElement>();

    public Animation HudAnimation;
    public AnimationClip HudInAnimation;
    public event CoinsChangedFn m_CoinsChangedFn;
    public event LifeChangefn m_LifeChangeFn;

    public SoundSelector sounds;

    private float hp = 8;

    private int coins = 0;
    private AnimationEvent evt;
    private void Awake()
    {
        DependencyInjector.AddDependency<IHpManager>(this);
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
            //--Life;
            //ShowLifeBar();
        }
    }
    public void HUDIn()
    {
        
            HudAnimation.CrossFade(HudInAnimation.name, 0.0f);
            HudAnimation.Rewind(HudInAnimation.name);
            HudAnimation.Sample();
        
        
    }
   
    
    public void AddLife(float value)
    {
        hp += value;
        sounds.StarUp();
        m_LifeChangeFn.Invoke(this);
        

    }
    
    public float GetLife()
    {
        return hp; 
    }
    public void LoseLife(float value)
    {
        hp-=value;
        sounds.StarDown();
        m_LifeChangeFn.Invoke(this);

    }

    public void AddCoins(int value)
    {
        coins+=value;
        sounds.Coin();
        m_CoinsChangedFn?.Invoke(this);
    }
    public int GetCoins()
    {
        return coins;
    }
}

public interface IRestartGameElement
{
    void Restart();
    void SetRestartPoint();
}


