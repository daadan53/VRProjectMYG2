using TMPro;
using Unity.Profiling;
using UnityEngine;

public class DevMod : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtFPS;
    [SerializeField] private TextMeshProUGUI txtBatches;
    [SerializeField] private TextMeshProUGUI txtTris;
    [SerializeField] private TextMeshProUGUI txtVerts;

    private ProfilerRecorder bacthesRecorder;
    private ProfilerRecorder trisRecorder;
    private ProfilerRecorder vertsRecorder;

    private float deltaTime = 0.0f;

    //On record les perf
    private void OnEnable() 
    {
        bacthesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
        trisRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        vertsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
    }

    private void OnDisable() 
    {
        bacthesRecorder.Dispose();
        trisRecorder.Dispose();
        vertsRecorder.Dispose();
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; //Calcule du nb de fps sans prendre en compte les effets de relenti du jeu par ex

        if(this.gameObject.activeSelf)
        {
            ShowStats();
        }
    }

    void ShowStats()
    {
        txtFPS.text = $"FPS : {Mathf.Ceil(1.0f / deltaTime)}";
        txtBatches.text = $"Batches : {bacthesRecorder.LastValue}";
        txtTris.text = $"Triangles : {trisRecorder.LastValue}";
        txtVerts.text = $"Vertices : {vertsRecorder.LastValue}";
    }
}
