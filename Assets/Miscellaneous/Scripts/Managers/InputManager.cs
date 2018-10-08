namespace Battlerock
{
    public class InputManager : Singleton<InputManager>
    {
        #region Public Variables

        public const string ATTACK = "Fire1";
        public const string HORIZONTAL_MOVEMENT = "Horizontal";
        public const string VERTICAL_MOVEMENT = "Vertical";

        #endregion

        private void Awake()
        {
            
        }
    }
}