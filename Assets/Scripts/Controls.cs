using UnityEngine;
using UnityEngine.InputSystem;

namespace NS_GameOfLife {
  public class Controls : MonoBehaviour {
    bool hasQuit = false;
    GOL_GUI[] guiComponents = new GOL_GUI[] {};
    GameOfLife gameOfLife = null;

    // Lazy load guiComponents
    GOL_GUI[] getGUIComponents() {
      if (guiComponents.Length == 0) {
        guiComponents = (GOL_GUI[]) GameObject.FindObjectsOfType(typeof(GOL_GUI));
      }

      return guiComponents;
    }

    GameOfLife getGameOfLife() {
      if (gameOfLife == null) {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameOfLife));

        if (objects.Length > 0) {
          gameOfLife = (GameOfLife) objects[0];
        }
      }

      return gameOfLife;
    }

    public void Quit() {
      if (!hasQuit) {
        hasQuit = true;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
      }
    }

    public void Help() {
      foreach (GOL_GUI guiComponent in getGUIComponents()) {
        guiComponent.toggleEnabled();
      }
    }

    public void Pause() {
      getGameOfLife().TogglePaused();
    }

    public void Reset() {
      getGameOfLife().Reset();
    }
  }
}
