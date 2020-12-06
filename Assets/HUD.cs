using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private int Life = 8;
    public Text coinsText;
    public Image LifeBar;
    public GameManager Manager;

    // Start is called before the first frame update
    void Start()
    {
        DependencyInjector.GetDependency<ICoinsManager>().m_CoinsChangedFn += UpdateCoins;
        DependencyInjector.GetDependency<IHpManager>().m_LifeChangeFn += UpdateHp;

    }

    // Update is called once per frame
    void UpdateCoins(ICoinsManager coinsManager)
    {
        if (coinsManager.getCoins() < 10)
        {
            coinsText.text = "0"+coinsManager.getCoins().ToString();

        }
        else
        {
            coinsText.text = coinsManager.getCoins().ToString();

        }
    }
    void UpdateHp(IHpManager hpManager)
    {
      
            float Pct = Manager.getLife() / 8.0f;
            LifeBar.fillAmount = Pct;
            if (Manager.getLife() <= 2)
            {

                LifeBar.color = Color.red;
            }
            else if (Manager.getLife() >= 6)
            {

                LifeBar.color = Color.green;

            }
            else if (Manager.getLife() < 6 && Manager.getLife() > 2)
            {

                LifeBar.color = Color.yellow;
            }
           Manager.HUDIn();
        

    }
    private void OnDestroy()
    {
        DependencyInjector.GetDependency<ICoinsManager>().m_CoinsChangedFn -= UpdateCoins;
    }
}
