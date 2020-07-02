using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NS_GameOfLife {
  public class Cube : MonoBehaviour, IInvisibility {
    public List<Cube> neighbors = new List<Cube>();

    public void toggleVisibility() {
      Renderer r = this.GetComponent<Renderer>();

      r.enabled = !r.enabled;
    }

    public void setVisibility(bool newState) {
      Renderer r = this.GetComponent<Renderer>();

      r.enabled = newState;
    }

    public bool getState() {
      Renderer r = this.GetComponent<Renderer>();

      return r.enabled;
    }

    public bool computeNextState() {
      int liveNeighbors = neighbors.ToArray().Aggregate(0, (acc, each) => acc + (each.getState() ? 1 : 0));

      switch (liveNeighbors) {
        case 2:
          return getState();
          break;
        case 3:
          return true;
          break;
        default:
          return false;
          break;
      }
    }
  }
}
