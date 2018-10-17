using UnityEngine;

namespace Battlerock
{
    public class CustomScriptableObject : ScriptableObject
    {
        public string objectName = "New MyScriptableObject";
        public GameMode gameMode = GameMode.BulletHell;
        public int numberOfPlayers = 1;
    }
}