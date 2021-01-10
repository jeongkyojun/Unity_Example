using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory
{
    public PlayerController controller { get; private set; }
    public Player model { get; private set;}
    public PlayerView view { get; private set; }
    
    public void Load()
    {
        GameObject prefab = Resources.Load<GameObject>("Player");
        GameObject instance = GameObject.Instantiate<GameObject>(prefab);
        this.model = new Player();
        this.view = instance.GetComponent<PlayerView>();
        this.controller = new PlayerController(model, view);
    }
}
