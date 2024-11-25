using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSetup : MonoBehaviour
{
    public Button Atack, Dash, skill1, skill2, skill3;

    public PlayerMovement playuer;


    private void Start()
    {
        playuer = FindObjectOfType<PlayerMovement>();
        Atack.onClick.AddListener(() => playuer.Attack());
        Dash.onClick.AddListener(() => playuer.Dash());
        skill1.onClick.AddListener(() => playuer.UseAbility1());
        skill2.onClick.AddListener(() => playuer.UseAbility2());
        skill3.onClick.AddListener(() => playuer.UseAbility3());

    }

}
