using System;
using NUnit.Framework;

public static class AssertFloat {
    public const float Epsilon = 1e-6f;

    public static void AreAlmostEqual(float expected, float actual, float e = Epsilon) {
        float diff = Math.Abs(expected - actual);
        Assert.True(diff < e, "Expected: {0}, actual: {1}", expected, actual);
    }
}
