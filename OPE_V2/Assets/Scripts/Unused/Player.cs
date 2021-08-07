using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxEnergy = 100;
    public int currentEnergy;

    public EnergyBar EnergyBar;

    // Start is called before the first frame update
    void Start()
    {
        EnergyBar.SetMaxEnergy(maxEnergy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HealDmg(10);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TakeDmg(10);
        }
    }

    public void TakeDmg(int dmg)
    {
        currentEnergy -= dmg;
        EnergyBar.SetEnergy(currentEnergy);
        setCorrectEnergy();
    }

    public void HealDmg(int dmg)
    {
        currentEnergy += dmg;
        EnergyBar.SetEnergy(currentEnergy);
        setCorrectEnergy();
    }

    public void setCorrectEnergy() //Não permite que a energia ultrapasse os valores min/máx
    {
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }
}
