# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

VR educational experience for Meta Quest about historical R-class rigid airships. Built with Unity 2022.3.39f1, targeting Android (Meta Quest). Package: `com.HyperactiveDevelopments.Airships`.

## Build Commands

Builds are done through the Unity Editor (no CLI build scripts exist). The project builds to Android APK via **File > Build Settings > Android > Build**.

- **Unity version required:** 2022.3.39f1
- **Build output directory:** `/Builds/` — naming convention: `build_{version}_{MM.DD}.apk`
- **Android SDK min/target:** API 32
- **Signing:** `user.keystore` in the project root (already staged in git — handle with care)

To deploy to a connected Meta Quest device, use Meta's developer tools or ADB.

## Architecture

### Single-Scene Data-Driven Model

The app uses a single scene (`Assets/Scenes/Main.unity`) with a data-driven interaction system:

1. **ExperienceManager** (singleton) initializes when MRUK (Mixed Reality Utility Kit) fires `RoomCreatedEvent`. It spawns the `Airship.prefab` at 1.75m height on the floor plane of the mapped room.
2. Each **InteractionPoint** component on the airship references an `InteractionPointData` scriptable object.
3. When a player selects an interaction point, `ExperienceManager.SpawnInteraction()` instantiates content 0.75m in front of the camera — either a text/image canvas, a 3D prefab, or a video canvas.
4. Only one interaction point can be active at a time (mutex via a static event `InteractionPoint.OnDeselect`).

### Core Scripts (`Assets/Scripts/`)

| Script | Role |
|---|---|
| `ExperienceManager.cs` | Singleton. Owns airship lifecycle, room anchoring, and content spawning logic. |
| `InteractionPoint.cs` | Attached to each hotspot on the airship. Handles hover/click states, material swapping, and triggers `ExperienceManager`. |
| `InteractionCanvas.cs` | Billboarding canvas that displays title, description, up to 2 images, or video. Faces camera each frame. |

### Data (`Assets/Resources/Data/`)

`InteractionPointData` is a ScriptableObject (create via **Assets > Create > Airships > InteractionPointData**) with:
- `Name`, `Text` — title and body copy
- `image0`, `image1` — optional Sprite references
- `Prefab` — optional 3D prefab to spawn instead of a canvas
- `VideoClip` — optional video; when set, spawns `VideoCanvas.prefab` instead of `Canvas.prefab`

Over 30 data assets already exist for airship components (ControlCar, HydrogenBags, PassengerDeck, etc.).

### Adding a New Interaction Point

1. Create a new `InteractionPointData` asset in `Assets/Resources/Data/`.
2. Populate its fields with content.
3. Add an `InteractionPoint` component to the relevant geometry in the scene or prefab, and assign the new data asset.

### Key Prefabs (`Assets/Prefabs/`)

- `Airship.prefab` — root airship model with all InteractionPoint components
- `Interface/Canvas.prefab` — text/image display canvas
- `Interface/VideoCanvas.prefab` — video playback canvas (uses `VideoRT.renderTexture`)
- `Interface/InteractionPoint/InteractionPoint.prefab` — reusable hotspot marker

## Dependencies

Managed in `Packages/manifest.json`. Key packages:

- `com.meta.xr.sdk.all` 74.0.2 — Meta XR (VR input, hand tracking, MRUK room mapping)
- `com.unity.xr.oculus` 4.2.0 — Oculus XR provider
- `com.unity.timeline` 1.7.6
- `com.unity.textmeshpro` 3.0.6
- `com.unity.visualscripting` 1.9.4

## Media Assets

Images live in `Assets/Resources/Images/`, videos in `Assets/Resources/Videos/`. Videos are tracked via Git LFS (`.mp4` extension). The large jump from build 1.2 (~83 MB) to build 1.5 (~625 MB) is due to embedded video content.