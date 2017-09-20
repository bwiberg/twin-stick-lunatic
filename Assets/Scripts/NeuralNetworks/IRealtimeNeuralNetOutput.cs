namespace NeuralNetworks {
    public interface IRealtimeNeuralNetOutput {
        int OutputCount { get; }

        void HandleOutput(float[] values, int offset);
    }
}
