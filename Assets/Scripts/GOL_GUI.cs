using UnityEngine;
using UnityEngine.InputSystem;

namespace NS_GameOfLife {
  public class GOL_GUI : MonoBehaviour {
    public bool DefaultEnabled = true;

    bool enabled;
    Controls controls = null;

    Controls getControls() {
      if (controls == null) {
        Object[] objects = GameObject.FindObjectsOfType(typeof(Controls));

        if (objects.Length > 0) {
          controls = (Controls) objects[0];
        }
      }

      return controls;
    }

    void Awake() {
      enabled = DefaultEnabled;
      Controls c = getControls();

      helpButtons = new GOL_Help_Button[] {
        new GOL_Help_Button("F1 - Toggle this Help", () => c.Help()),
        new GOL_Help_Button("q - Quit", () => c.Quit()),
        new GOL_Help_Button("p - Pause/Resume", () => c.Pause()),
        new GOL_Help_Button("r - Reset", () => c.Reset())
      };
    }

    delegate void ZeroParamVoidDelegate();

    struct GOL_Help_Button {
      public string name;
      public ZeroParamVoidDelegate clicked;

      public GOL_Help_Button(string pName, ZeroParamVoidDelegate pDelegate) {
        name = pName;
        clicked = pDelegate;
      }
    };

    GOL_Help_Button[] helpButtons;

    void OnGUI() {
      if (!enabled) {
        return;
      }

      int buttonSize = 15;
      int hMargin = 10;
      int vMargin = 8;
      int hSpacing = 5;
      int vSpacing = 5;
      int height = (helpButtons.Length * buttonSize) + ((helpButtons.Length + 1) * vSpacing);
      int width = 400;
      int top = Screen.height - height - vMargin;
      int left = Screen.width - width - hMargin;

      GUI.Box(new Rect(left, top, width, height), "");

      Rect rect;
      GOL_Help_Button helpButton;

      for (int i = 0; i < helpButtons.Length; ++i) {
        helpButton = helpButtons[i];

        rect = new Rect(
          left + hSpacing,
          top + (i * buttonSize) + ((i + 1) * vSpacing),
          width - hSpacing,
          buttonSize
        );

        if (GUI.Button(rect, helpButton.name)) {
          helpButton.clicked();
        }
      }
    }

    public void setEnabled(bool newState) {
      enabled = newState;
    }

    public void toggleEnabled() {
      enabled = !enabled;
    }
  }
}
