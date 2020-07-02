using UnityEngine;

namespace NS_GameOfLife {
  public interface IInvisibility {
    void toggleVisibility();

    void setVisibility(bool newState);
  }
}
