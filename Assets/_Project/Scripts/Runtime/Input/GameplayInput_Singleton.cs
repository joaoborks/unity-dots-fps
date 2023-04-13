using UnityEngine;

namespace MyFps.Input
{
    public partial class GameplayInput
    {
        public static GameplayInput Instance => _instance;

        static GameplayInput _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize()
        {
#if UNITY_EDITOR
            _instance?.Dispose();
#endif
            _instance = new GameplayInput();
        }
    }
}