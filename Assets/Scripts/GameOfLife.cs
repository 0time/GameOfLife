using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace NS_GameOfLife {
  public class GameOfLife : MonoBehaviour {
    public GameObject NodePrefab = null;

    [Header("Game of Life Settings")]

    [Tooltip("Length (Columns)")]
    public int GameOfLifeLength = 16;
    [Tooltip("Height (Rows)")]
    public int GameOfLifeHeight = 16;

    [Tooltip("Initial State Seed (Default: \"\", meaning random seed)")]
    public string InitialStateSeed = "";

    [Tooltip("The minimum amount of time (in seconds) in between steps")]
    public float FastestStepTime = 0.1f;

    bool paused = false;
    float lastStep = 0f;
    float timeElapsed = 0f;

    void SpawnAllNodes() {
      Vector3 spawnPosition = Vector3.zero;
      GameObject mb = null;

      if (!NodePrefab) {
        Debug.LogWarning("Cannot instantiate null prefab, exiting");

        return;
      }

      for (int i = 0; i < GameOfLifeLength; ++i) {
        for (int j = 0; j < GameOfLifeHeight; ++j) {
          spawnPosition = this.transform.position;

          spawnPosition.x += i;
          spawnPosition.z += j;

          mb = Instantiate(NodePrefab, spawnPosition, new Quaternion(0, 0, 0, 0), this.transform);

          mb.name += "i " + i + " j " + j;
        }
      }
    }

    void InitializeRandomizer() {
      if (InitialStateSeed != "") {
        MD5 hasher = MD5.Create();

        byte[] hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(InitialStateSeed));
        int iseed = BitConverter.ToInt32(hashed, 0);

        UnityEngine.Random.InitState(iseed);
      }
    }

    void InitializeNodes() {
      foreach (Cube child in this.gameObject.GetComponentsInChildren<Cube>()) {
        AssociateNeighbors(child);
        RandomizeNode(child);
      }
    }

    void AssociateNeighbors(Cube node) {
      Vector3[] directions = new Vector3[] {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        Vector3.forward + Vector3.left,
        Vector3.forward + Vector3.right,
        Vector3.back + Vector3.left,
        Vector3.back + Vector3.right
      };
      RaycastHit rh ;

      node.neighbors.Clear();

      foreach (Vector3 direction in directions) {
        if (Physics.Raycast(node.transform.position, transform.TransformDirection(direction), out rh)) {
          node.neighbors.Add(rh.transform.gameObject.GetComponent<Cube>());
        }
      }

      if (node.neighbors.Count < 3 || node.neighbors.Count > 8) {
        Debug.Log("Unexpected count of neighbors for node " + node + " count: " + node.neighbors.Count);
      }
    }

    void RandomizeNode(Cube node) {
      int nextInt = UnityEngine.Random.Range(0, 2);
      bool state = nextInt == 1 ? false : true;
      node.setVisibility(state);
    }

    void StepAllNodes() {
      Dictionary<Cube, bool> map = new Dictionary<Cube, bool>();

      foreach (Cube child in this.gameObject.GetComponentsInChildren<Cube>()) {
        map.Add(child, child.computeNextState());
      }

      foreach (KeyValuePair<Cube, bool> kvp in map) {
        kvp.Key.setVisibility(kvp.Value);
      }
    }

    public void Reset() {
      paused = false;
      InitializeRandomizer();
      InitializeNodes();
      lastStep = -timeElapsed;
    }

    public void TogglePaused() {
      paused = !paused;
    }

    void Awake() {
      SpawnAllNodes();
    }

    void OnEnable() {
      Reset();
    }

    void Update() {
      if (!paused) {
        timeElapsed += Time.deltaTime;

        if (lastStep + FastestStepTime < timeElapsed) {
          StepAllNodes();
          lastStep = timeElapsed;
        }
      }
    }
  }
}
