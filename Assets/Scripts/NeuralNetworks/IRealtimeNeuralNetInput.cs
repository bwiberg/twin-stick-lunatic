namespace NeuralNetworks {
    public interface IRealtimeNeuralNetInput {
        int InputCount { get; }

        float[] GetInput();
    }
}
