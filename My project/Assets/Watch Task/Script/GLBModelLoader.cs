using UnityEngine;
using System.Collections;

namespace WatchTryOn
{
    /// <summary>
    /// Handles loading GLB models at runtime.
    /// Replace the dummy logic in LoadGLBCoroutine with your actual GLB loading SDK 
    /// (e.g., glTFast, TriLib, UnityGLTF).
    /// </summary>
    public class GLBModelLoader : MonoBehaviour
    {
        [Header("Model Adjustments")]
        [Tooltip("Pre-rotation applied to loaded models to ensure they align correctly with the generated pose axes. Adjust this if your model is flipped.")]
        public Vector3 defaultPreRotation = new Vector3(0, 0, 0);

        private GameObject currentModel;

        /// <summary>
        /// Loads a GLB file and parents it to the specified transform.
        /// </summary>
        public void LoadModel(string path, Transform parentTransform)
        {
            // Cleanup previous watch
            if (currentModel != null)
            {
                Destroy(currentModel);
            }

            StartCoroutine(LoadGLBCoroutine(path, parentTransform));
        }

        private IEnumerator LoadGLBCoroutine(string path, Transform parentTransform)
        {
            // =========================================================
            // TODO: Integrate actual GLB loading library here.
            // Example using glTFast pseudo-code:
            // var gltf = new GLTFast.GltfImport();
            // yield return gltf.Load(path);
            // var go = new GameObject("WatchGLB");
            // gltf.InstantiateMainScene(go.transform);
            // currentModel = go;
            // =========================================================

            yield return null; // Simulate async load

            // --- PROTOTYPE FALLBACK MOCKUP ---
            // Remove this once actual loading is hooked up.
            if (currentModel == null)
            {
                currentModel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                currentModel.name = "Mock_Watch_Model";
                
                // Adjust primitive to look somewhat like a watch shape
                currentModel.transform.localScale = new Vector3(0.05f, 0.01f, 0.05f); 
                Destroy(currentModel.GetComponent<Collider>()); // No colliders needed
            }
            // ---------------------------------

            // Setup hierarchy and zero-out transforms
            currentModel.transform.SetParent(parentTransform, false);
            currentModel.transform.localPosition = Vector3.zero;
            
            // Align Pivot / Apply Pre-rotation
            // To ensure the dial faces outward and the strap aligns, you may need to pre-rotate 
            // the 3D model if it wasn't exported with a clean Z-forward, Y-up orientation.
            currentModel.transform.localRotation = Quaternion.Euler(defaultPreRotation);
        }
    }
}
