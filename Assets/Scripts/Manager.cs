using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public WorldOptions _worldOptions;



    public static WorldOptions worldOptions => instance._worldOptions;

    static Manager instance;
    public Manager Instance {
        get {
            if (!instance) {
                instance = this;
                return instance;
            }
            else
                return instance;
        }
    }

    private void Awake() {
        if (Instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
    }

    public void PlayerStock(Player player)
    {

    }
}
