using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private string[] player_Names = { "Knight", "BasicSamurai", "ArcherSamurai", "CommanderSamurai" };
    private string[] enemy_Names = { };
    private string gameObjectName ;
    // Start is called before the first frame update
    private string abl1_name;
    private string abl2_name;
    private string abl3_name;
    private Animator amt;

    void Start()
    {
        //attackArea = transform.GetChild(0).gameObject;
        amt = GetComponent<Animator>();
        gameObjectName = gameObject.name;
        //gắn chiêu thúc
    }

    bool findCharacterByTagPlayer()
    {
        foreach (string playerName in player_Names)
        {
            if (playerName == gameObjectName)
            {
                return true; // Nếu tìm thấy, trả về true
            }
        }
        return false; // Nếu không tìm thấy, trả về false
    }

    bool findCharacterByTagEnemy()
    {
        foreach (string enemyName in enemy_Names)
        {
            if (enemyName == gameObjectName)
            {
                return true; // Nếu tìm thấy, trả về true
            }
        }
        return false; // Nếu không tìm thấy, trả về false
    }

    void takeThisAbibity()
    {
        //gắn chiêu thúc

        abl1_name = gameObjectName + "Ability_1";
        abl2_name = gameObjectName + "Ability_1";
        abl3_name = gameObjectName + "Ability_1";

    }

    void ArcherAbility_1()
    {
    }

}