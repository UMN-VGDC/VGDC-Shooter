using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoadCount : MonoBehaviour
{
    [SerializeField] private int maxBossLoad = 4;
    [SerializeField] private int maxMediumLoad = 10;
    [SerializeField] private int maxSmallLoad = 15;
    [SerializeField] private int maxFlyingLoad = 12;
    private int bossLoad, mediumLoad, smallLoad, flyingLoad;
    public static EnemyLoadCount Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        bossLoad = maxBossLoad;
        mediumLoad = maxMediumLoad;
        smallLoad = maxSmallLoad;
        flyingLoad = maxFlyingLoad;
    }

    public bool ModifyLoad(int amount, string type)
    {
        if (amount == 0) return true;
        int load = 0;
        switch(type)
        {
            case "Boss": load = bossLoad; break;
            case "Medium": load = mediumLoad; break;
            case "Small": load = smallLoad; break;
            case "Flying": load = flyingLoad; break;
        }

        if (load + amount < 0)
        {
            //Debug.Log($"{type} load exceeded");
            return false;
        }

        switch (type)
        {
            case "Boss": 
                bossLoad += amount;
                //Debug.Log($"{bossLoad}/{maxBossLoad}");
                break;
            case "Medium": 
                mediumLoad += amount;
                //Debug.Log($"{mediumLoad}/{maxMediumLoad}");
                break;
            case "Small": 
                smallLoad += amount;
                //Debug.Log($"{smallLoad}/{maxSmallLoad}");
                break;
            case "Flying": flyingLoad += amount; break;
        }
        
        return true;

    }
}
