using UnityEngine;
using UnityEditor;

namespace WatchTryOn.Editor
{
    public class WatchTryOnSetup : EditorWindow
    {
        [MenuItem("Watch Try-On/Setup Scene")]
        public static void SetupScene()
        {
            // 1. Create Manager
            GameObject manager = GameObject.Find("WatchTryOnManager");
            if (manager == null)
            {
                manager = new GameObject("WatchTryOnManager");
            }

            // 2. Add System and Loader
            WatchTryOnSystem tryOnSystem = manager.GetComponent<WatchTryOnSystem>();
            if (tryOnSystem == null) tryOnSystem = manager.AddComponent<WatchTryOnSystem>();

            GLBModelLoader loader = manager.GetComponent<GLBModelLoader>();
            if (loader == null) loader = manager.AddComponent<GLBModelLoader>();
            tryOnSystem.modelLoader = loader;

            // 3. Create Placement Anchor
            GameObject anchor = GameObject.Find("WatchPlacementAnchor");
            if (anchor == null)
            {
                anchor = new GameObject("WatchPlacementAnchor");
            }
            WatchPlacementController placementController = anchor.GetComponent<WatchPlacementController>();
            if (placementController == null) placementController = anchor.AddComponent<WatchPlacementController>();
            tryOnSystem.watchController = placementController;

            // 4. Create Root for Model
            Transform root = anchor.transform.Find("WatchRoot");
            if (root == null)
            {
                GameObject rootGo = new GameObject("WatchRoot");
                rootGo.transform.SetParent(anchor.transform);
                rootGo.transform.localPosition = Vector3.zero;
                rootGo.transform.localRotation = Quaternion.identity;
                root = rootGo.transform;
            }
            tryOnSystem.watchModelRoot = root.gameObject;

            // Set scene as dirty to save changes
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

            Debug.Log("<b>Watch Try-On Scene setup complete!</b> Manager and Anchor have been created and linked.");
        }
    }
}
