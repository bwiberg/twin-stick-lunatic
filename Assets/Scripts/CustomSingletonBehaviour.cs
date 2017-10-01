using UnityEngine.Assertions;

public class CustomSingletonBehaviour<T> : CustomBehaviour where T: UnityEngine.MonoBehaviour {
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<T>();
            }
            Assert.IsNotNull(instance, "Accessing Singleton instance, but none present in scene.");
            return instance;
        }
    }

}
