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
    public Text ExtraLifes;

    // Start is called before the first frame update
    void Start()
    {
        DependencyInjector.GetDependency<ICoinsManager>().m_CoinsChangedFn += UpdateCoins;
        DependencyInjector.GetDependency<IHpManager>().m_LifeChangeFn += UpdateHp;

    }

    // Update is called once per frame
    void UpdateCoins(ICoinsManager coinsManager)
    {
        if (coinsManager.GetCoins() < 10)
        {
            coinsText.text = "0"+coinsManager.GetCoins().ToString();

        }
        else
        {
            coinsText.text = coinsManager.GetCoins().ToString();

        }
    }
    void UpdateHp(IHpManager hpManager)
    {

            ExtraLifes.text = hpManager.getExtraLifes().ToString();
            float Pct = Manager.GetLife() / 8.0f;
            LifeBar.fillAmount = Pct;
            if (Manager.GetLife() <= 2)
            {

                LifeBar.color = Color.red;
            }
            else if (Manager.GetLife() >= 6)
            {

                LifeBar.color = Color.green;

            }
            else if (Manager.GetLife() < 6 && Manager.GetLife() > 2)
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
