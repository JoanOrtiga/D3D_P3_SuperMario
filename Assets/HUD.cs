using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text coinsText;
    // Start is called before the first frame update
    void Start()
    {
        DependencyInjector.GetDependency<ICoinsManager>().m_CoinsChangedFn += UpdateCoins;
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
    private void OnDestroy()
    {
        DependencyInjector.GetDependency<ICoinsManager>().m_CoinsChangedFn -= UpdateCoins;
    }
}
