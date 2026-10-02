# Meadowbrook (working title)

Original mobile farming simulation, built with Unity 6 (6000.0.23f1) and C#.

## Status
Phase 1: farm grid, locked/unlocked land, tap selection, pan/pinch camera.

## Build an iPhone .ipa from GitHub
1. Add repo secrets `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD` (Settings > Secrets and variables > Actions).
2. Push to `main` (or run **Build iOS** manually from the Actions tab).
3. Download the `Meadowbrook-ipa` artifact. It is **unsigned**: install it with Sideloadly or AltStore using a free Apple ID (re-sign expires after 7 days).

## Layout
`Assets/_Project/Scripts/{Core,Farm,Input,CameraSystem,UI,...}` - see the Phase 1 plan.
The game constructs itself at startup via `GameBootstrapper`; override defaults with ScriptableObjects at `Resources/Configs/`.
