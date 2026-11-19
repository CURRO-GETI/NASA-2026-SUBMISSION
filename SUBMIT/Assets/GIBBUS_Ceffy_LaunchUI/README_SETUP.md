# GIBBUS — Ceffy Launch UI

A full-screen HTML/CSS/JavaScript launch-operations interface for the GIBBUS Unity project. Ceffy embeds the page inside Unity. The 3D launch vehicle and actual launch sequence remain Unity-controlled.

## Included files

```text
Assets/
├── Scripts/
│   ├── GibbusLaunchHud.cs
│   ├── GibbusLaunchSnapshot.cs
│   └── IGibbusLaunchCommands.cs
└── StreamingAssets/
    └── GIBBUS/
        └── LaunchUI/
            ├── index.html
            ├── launch.css
            └── launch.js
```

## 1. Install Ceffy

Ceffy is a Windows x64 Unity 6 package. The package embeds Chromium (CEF) and uses a native plugin. It requires a Windows x64 build using Direct3D 11 or Direct3D 12.

Recommended installation:
1. Open **Window → Package Manager**.
2. Click **+ → Install package from git URL…**
3. Enter `https://github.com/hovgaardgames/ceffy.git`
4. Install the package and wait for Unity to finish importing.

Alternative: add Ceffy to your Unity Asset Store account and install it through Package Manager.

If you install from Git and the browser's native runtime fails to load, check that Git LFS is installed and that the native files are real binaries rather than Git LFS pointer text files.

Official repository: https://github.com/hovgaardgames/ceffy  
Unity Asset Store: https://assetstore.unity.com/packages/tools/ceffy-2270691

## 2. Copy the files into your Unity project

Extract this ZIP into your Unity project root, allowing the included `Assets` folder to merge with your existing `Assets` folder.

Do not put `StreamingAssets` inside `Resources`. Its exact location matters:

`Assets/StreamingAssets/GIBBUS/LaunchUI/index.html`

The HTML, CSS and JavaScript files must remain together in the `LaunchUI` folder.

## 3. Add the fullscreen Ceffy view

1. Open your launch scene.
2. Create a Canvas if the scene does not already have one:
   - Render Mode: **Screen Space - Overlay**
   - Add a **Canvas Scaler** and set UI Scale Mode to **Scale With Screen Size**
   - Reference Resolution: **1920 × 1080**
   - Add a **Graphic Raycaster**
3. Create an empty child GameObject under the Canvas named `GIBBUS_HTML_LaunchUI`.
4. Add the **CeffyInstance** component to it.
5. Add the **GibbusLaunchHud** component to the same GameObject.
6. Set the RectTransform anchors to stretch in both directions:
   - Anchor Min: (0, 0)
   - Anchor Max: (1, 1)
   - Left / Right / Top / Bottom: 0
7. In CeffyInstance, leave **Use Shared Instance** disabled. This is a full-screen, dedicated browser instance.
8. In GibbusLaunchHud, assign the CeffyInstance field to the CeffyInstance component on this GameObject.

The script sets the Start URL before Ceffy starts its browser:
`streaming-assets://GIBBUS/LaunchUI/index.html`

Ceffy adds its RawImage and CeffyInstanceView at runtime. The page background is transparent so the central area can show your Unity rocket and launch pad.

## 4. Add an EventSystem

The Ceffy view forwards mouse interaction through Unity's UI event system.

- If your project uses the old Input Manager, add an EventSystem with `StandaloneInputModule`.
- If your project uses the new Input System, use `InputSystemUIInputModule`.
- Do not create a second EventSystem if the scene already has one.

## 5. Connect the launch event

Select `GIBBUS_HTML_LaunchUI` and find **Unity Events** on `GibbusLaunchHud`.

### On Launch Authorized
Add a listener and connect the GameObject that controls your launch sequence. Select the public method that begins your actual launch countdown / Timeline.

The HTML button does not directly start a fake countdown. It requests authorisation from C#. C# checks that every checklist item has been acknowledged, then invokes this UnityEvent.

### On Return To Mission Design
Connect this to the public method that returns to your mission-design scene or opens your mission-design UI.

If your current launch controller does not expose public methods yet, leave these events unassigned while testing the HTML interface. The UI and checklist bridge can still be tested.

## 6. Connect telemetry to Unity

The starter values are demonstration values, not live launch telemetry. Connect your actual simulation to these public methods on `GibbusLaunchHud`:

```csharp
launchHud.SetMissionPhase("COUNTDOWN");
launchHud.SetCountdownSeconds(10);

launchHud.SetTelemetry(
    altitudeKm,
    velocityKmPerSecond,
    propellantPercent,
    powerPercent,
    communicationsStatus,
    "EARTH → LUNAR TRANSFER"
);

launchHud.AddMissionLog("Main engine ignition confirmed.");
```

Call `SetCountdownSeconds(-1)` when there is no active countdown. The HTML interface displays the values received from Unity.

## 7. Test in the Editor

1. Open the launch scene and press Play.
2. Wait for the connection indicator in the top-right to show **UNITY LINK ESTABLISHED**.
3. Acknowledge the six checklist entries.
4. Confirm the readiness indicator reaches 100%.
5. Click **AUTHORISE LAUNCH**, then confirm the request.
6. Check the Unity Console and the `GibbusLaunchHud` UnityEvent.
7. Test the return-to-design button.

The checklist acknowledgements are operator inputs in this starter implementation. They are not automatic engineering validation. Before treating the interface as a real mission-readiness gate, connect each checklist item to your existing mission configuration, budget, power, mass and spacecraft validation systems.

## 8. Build settings

- Target platform: **Windows**
- Architecture: **x86_64**
- Graphics API: Direct3D 11 or Direct3D 12
- Test a standalone build; do not rely only on Editor Play Mode.
- Keep the Ceffy package and its Windows native runtime included in the build.

## Troubleshooting

**Blank page**
- Check that `index.html` is at `Assets/StreamingAssets/GIBBUS/LaunchUI/index.html`.
- Check that the Ceffy Start URL is exactly `streaming-assets://GIBBUS/LaunchUI/index.html`.
- Check the Console for CEF runtime or missing native plugin errors.

**Page loads but stays on CONNECTING**
- Confirm `GibbusLaunchHud` and `CeffyInstance` are on the same GameObject.
- Confirm the `CeffyInstance` reference is assigned.
- Check for JavaScript errors in the Unity Console / Ceffy browser DevTools.

**Buttons do not respond**
- Ensure the Canvas has a Graphic Raycaster.
- Ensure an EventSystem exists and uses the correct input module.
- Ensure the Ceffy RawImage is not covered by another Canvas element.

**Native runtime error in a Git installation**
- Verify Git LFS is installed.
- Reinstall/update the package if native runtime files were downloaded as LFS pointer files.

## Important implementation boundary

This package is a working UI bridge and a launch-scene starter. It deliberately does not guess the names or behaviour of your existing launch controller, MissionDesignFlow, or mission-calculation classes. Connect the two UnityEvents to the actual methods in your project rather than replacing those systems.
