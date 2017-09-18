using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NeuralNetworks;

public class FullyConnectedNNTests {
    [Test]
    public void Constructor_CalledWithLessThanTwoLayers_ThrowsException() {
        Assert.Throws<Exception>(() => {
            FullyConnectedNN nn = new FullyConnectedNN(new int[] { });
        });
        Assert.Throws<Exception>(() => {
            FullyConnectedNN nn = new FullyConnectedNN(new[] {1});
        });
        Assert.DoesNotThrow(() => {
            FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 2});
            FullyConnectedNN nn2 = new FullyConnectedNN(new[] {1, 2, 3, 4, 5});
        });
    }

    [Test]
    public void NeuronsPerLayer_ConstructedWithValue_GetsValue() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 4, 5});
        Assert.AreEqual(nn.NeuronsPerLayer[0], 1);
        Assert.AreEqual(nn.NeuronsPerLayer[1], 4);
        Assert.AreEqual(nn.NeuronsPerLayer[2], 5);
    }

    [Test]
    public void LayerCount_HasCorrectValue() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 4, 5});
        Assert.AreEqual(nn.LayerCount, 3);
    }

    [Test]
    public void HiddenLayerCount_HasCorrectValue() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 4, 5});
        Assert.AreEqual(nn.HiddenLayerCount, 1);
    }

    [Test]
    public void NeuronCount_HasCorrectValue() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 4, 5});
        Assert.AreEqual(nn.NeuronCount, 10);
    }

    [Test]
    public void ActivationFunction_IsDefaultFunction_IfNoneProvidedToConstructor() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 4, 1337});
        Assert.True(nn.ActivationFunction == FullyConnectedNN.AF.Default);
    }

    [Test]
    public void ActivationFunction_IsFunction_IfFunctionProvidedToConstructor() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 4, 1337}, FullyConnectedNN.AF.Smooth);
        Assert.True(nn.ActivationFunction == FullyConnectedNN.AF.Smooth);
    }

    [Test]
    public void EvaluateNeuron_WithPassthroughAF_WithSingleInput_ReturnsInputValueTimesWeight() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 1},
            FullyConnectedNN.AF.Passthrough);

        AssertFloat.AreAlmostEqual(-1.0f, nn.EvaluateNeuron(new[] {-1.0f}, 1, new[] {1.0f}, 0));
        AssertFloat.AreAlmostEqual(0.35f, nn.EvaluateNeuron(new[] {0.35f}, 1, new[] {1.0f}, 0));
        AssertFloat.AreAlmostEqual(0.5f, nn.EvaluateNeuron(new[] {1.0f}, 1, new[] {0.5f}, 0));
    }

    [Test]
    public void EvaluateNeuron_WithPassthroughAF_WithMultipleInputs_ReturnsCorrectValue() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {2, 1},
            FullyConnectedNN.AF.Passthrough);

        AssertFloat.AreAlmostEqual(0.0f, nn.EvaluateNeuron(new[] {0.0f, 0.0f}, 1, new[] {1.0f, 1.0f}, 0));
        AssertFloat.AreAlmostEqual(0.8f, nn.EvaluateNeuron(new[] {0.3f, 0.5f}, 1, new[] {1.0f, 1.0f}, 0));
        AssertFloat.AreAlmostEqual(0.3f, nn.EvaluateNeuron(new[] {0.5f, -0.4f}, 1, new[] {1.0f, 0.5f}, 0));
    }

    [Test]
    public void EvaluateLayer_WithPassthroughAF_WithSingleInput_ReturnsInputValueTimesWeights() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 2},
            FullyConnectedNN.AF.Passthrough);

        int offsetWeight = 0;
        float[] output = nn.EvaluateLayer(new[] {1.0f}, 1, new[] {0.3f, -1.0f}, ref offsetWeight);

        AssertFloat.AreAlmostEqual(0.3f, output[0]);
        AssertFloat.AreAlmostEqual(-1.0f, output[1]);

        offsetWeight = 0;
        output = nn.EvaluateLayer(new[] {-0.5f}, 1, new[] {0.5f, -1.0f}, ref offsetWeight);

        AssertFloat.AreAlmostEqual(-0.25f, output[0]);
        AssertFloat.AreAlmostEqual(0.5f, output[1]);
    }

    [Test]
    public void EvaluateLayer_WithPassthroughAF_WithMultipleInputs_ReturnsCorrectValues() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {2, 2},
            FullyConnectedNN.AF.Passthrough);

        int offsetWeight = 0;
        float[] output = nn.EvaluateLayer(new[] {1.0f, -1.0f}, 1, new[] {0.9f, 1.0f, 1.0f, 0.5f}, ref offsetWeight);

        AssertFloat.AreAlmostEqual(-0.1f, output[0]);
        AssertFloat.AreAlmostEqual(0.5f, output[1]);
    }

    [Test]
    public void Evaluate_WithPassthroughAF_WithSingleInputAndOutput_ReturnsInputTimesWeight() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 1},
            FullyConnectedNN.AF.Passthrough);

        float[] output = nn.Evaluate(new[] {1.0f}, new[] {1.0f});
        AssertFloat.AreAlmostEqual(1.0f, output[0]);

        output = nn.Evaluate(new[] {0.5f}, new[] {0.3f});
        AssertFloat.AreAlmostEqual(0.15f, output[0]);
    }

    [Test]
    public void Evaluate_WithPassthroughAF_WithSingleHiddenLayer_WithSingleInputAndOutput_ReturnsInputTimesWeight() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {1, 1, 1},
            FullyConnectedNN.AF.Passthrough);

        float[] output = nn.Evaluate(new[] {0.8f}, new[] {0.8f, 0.8f});
        AssertFloat.AreAlmostEqual(0.512f, output[0]);
    }

    [Test]
    public void Evaluate_WithPassthroughAF_WithSingleHiddenLayer_WithMultipleInputAndOutput_ReturnsCorrect() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {2, 1, 2},
            FullyConnectedNN.AF.Passthrough);

        float[] output = nn.Evaluate(new[] {1.0f, -1.0f}, new[] {0.2f, 0.4f, 0.2f, 0.4f});
        AssertFloat.AreAlmostEqual(-0.04f, output[0]);
        AssertFloat.AreAlmostEqual(-0.08f, output[1]);
    }

    [Test]
    public void Evaluate_WithPassthroughAF_WithMultipleHiddenLayers_WithMultipleInputAndOutput_ReturnsCorrect() {
        FullyConnectedNN nn = new FullyConnectedNN(new[] {2, 2, 2},
            FullyConnectedNN.AF.Passthrough);

        float[] output = nn.Evaluate(new[] {1.0f, -1.0f}, new[] {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f});
        AssertFloat.AreAlmostEqual(-0.11f, output[0]);
        AssertFloat.AreAlmostEqual(-0.15f, output[1]);
    }
}
