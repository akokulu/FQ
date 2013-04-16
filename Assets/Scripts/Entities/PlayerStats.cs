using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public static int lampsFetched = 0;
    public static int enemiesKilled = 0;
    public static string playerName = "Warrior";

    public static void increaseLamps()
    {
        lampsFetched++;
    }

    public static void increaseKillCount()
    {
        enemiesKilled++;
    }
}