namespace Battlerock
{
    public class InputManager : Singleton<InputManager>
    {
        #region Public Variables

        public const string ATTACK = "Attack";
        public const string HORIZONTAL_MOVEMENT = "Horizontal";
        public const string VERTICAL_MOVEMENT = "Vertical";

        public const string RSTICK_HORIZONTAL = "RStick_Horizontal";
        public const string RSTICK_VERTICAL = "RStick_Vertical";

        public const string JUMP = "Jump";

        #endregion

        private void Awake()
        {
            
        }
    }
}