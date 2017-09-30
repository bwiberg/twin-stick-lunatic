using NUnit.Framework;
using ProceduralGeneration;
using Utility;

public class QuadMeshTests {
    [Test]
    public void LinearizeIndex_Works() {
        IntVector2 dimensions = new IntVector2(10, 10);

        Assert.AreEqual(5, QuadMesh.LinearizeIndex(new IntVector2(5, 0), dimensions));
        Assert.AreEqual(15, QuadMesh.LinearizeIndex(new IntVector2(5, 1), dimensions));
        Assert.AreEqual(95, QuadMesh.LinearizeIndex(new IntVector2(5, 9), dimensions));
        Assert.AreEqual(99, QuadMesh.LinearizeIndex(new IntVector2(9, 9), dimensions));

        dimensions = new IntVector2(3, 7);

        Assert.AreEqual(20, QuadMesh.LinearizeIndex(new IntVector2(2, 6), dimensions));
        Assert.AreEqual(15, QuadMesh.LinearizeIndex(new IntVector2(0, 5), dimensions));
    }

    private void AssertUnlinearizeLinearize(IntVector2 original, IntVector2 dimensions) {
        IntVector2 actual = QuadMesh.UnlinearizeIndex(QuadMesh.LinearizeIndex(original, dimensions), dimensions); 
        Assert.IsTrue(EqualIntVector2(original, actual),
            string.Format("expected: {0}, actual: {1}", original, actual));
    }

    private bool EqualIntVector2(IntVector2 a, IntVector2 b) {
        return a.x == b.x && a.y == b.y;
    }

    [Test]
    public void UnlinearizeIndex_Works() {
        IntVector2 dimensions = new IntVector2(10, 10);

        AssertUnlinearizeLinearize(new IntVector2(0, 0), dimensions);
        AssertUnlinearizeLinearize(new IntVector2(2, 5), dimensions);
        AssertUnlinearizeLinearize(new IntVector2(1, 9), dimensions);
        AssertUnlinearizeLinearize(new IntVector2(9, 9), dimensions);
        AssertUnlinearizeLinearize(new IntVector2(33, 11), new IntVector2(57, 24));
    }
}
