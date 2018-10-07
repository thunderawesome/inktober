namespace Battlerock
{
    public interface IWavesController
    {
        Waves GetCurrentWave { get; }

        int GetCurrentWaveIndex { get; }

        bool IsCurrentWaveCompleted { get; }
    }
}