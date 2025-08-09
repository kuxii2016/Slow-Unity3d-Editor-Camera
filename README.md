# Slow Scene Camera Control for Unity Editor (2017+)

This Unity Editor script enhances the Scene View camera controls by providing a slow and smooth camera movement mode, ideal for precise navigation and editing in complex scenes.

## Features

- Toggle slow camera mode on/off with **L** key.
- Move camera with **WASD + QE** keys.
- Adjust movement speed with **Shift** (faster) and **Ctrl** (slower) modifiers.
- Rotate camera by holding **right mouse button** and moving the mouse.
- Reset camera position and rotation with **R** key.
- Displays an in-scene HUD with current speed and control hints.
- Prevents accidental object selection or scene interaction while active.
- Compatible with Unity Editor 2017 and later.

## Installation

1. Place the `SlowSceneCameraControl.cs` script inside an `Editor` folder in your Unity project.
2. Open the Scene View in Unity Editor.
3. Press **L** to toggle the slow camera mode on or off.

## Usage

When slow mode is active:

- Use **WASD** to move forward, left, backward, and right.
- Use **Q** and **E** to move down and up.
- Hold **Shift** to increase movement speed by 5×.
- Hold **Ctrl** to decrease movement speed to 3% of the base speed.
- Hold the **right mouse button** and move the mouse to rotate the camera smoothly.
- Press **R** to reset the camera to the initial position and rotation.

The HUD shows current base speed and available controls.

## Notes

- The script “consumes” all mouse and keyboard events while active, preventing accidental selection or interaction with scene objects.
- Adjust the base movement speed in the HUD slider as needed.

## License

MIT License
