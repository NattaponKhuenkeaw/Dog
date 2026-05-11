# DevLog: 2026-05-11 - Phase 2 Runtime Implementation

## Goal

Implement the Phase 2 ADR stack in code and update the documentation so it describes the shipped runtime shape instead of the ideal end-state only.

## Completed

- [x] Added the `Services` locator and focused runtime services:
  `HealthSystem`, `EnergySystem`, `FlashlightSystem`, `InventoryManager`, `DoorLockRegistry`, `SessionManager`, `SceneLoader`
- [x] Reworked `GameManager` into a bootstrap and compatibility facade over the new services
- [x] Split player runtime into `PlayerMovement`, `PlayerInteraction`, `PlayerHiding`, and `PlayerAudio`
- [x] Added `PlayerSettings` for future data-driven tuning
- [x] Added `IInteractable`, `InteractInputType`, `DoorInteractable`, `HideSpotInteractable`, and `StairInteractable`
- [x] Reworked `DoorClick` to delegate to the new interaction and scene-loading path
- [x] Added event-driven UI binders:
  `HealthUI`, `EnergyUI`, `FlashlightUI`, `InventoryHotbarUI`, `DeathSequenceUI`
- [x] Reworked `SceneInitializer` into a binder bridge for existing scenes
- [x] Updated ADR docs to reflect the implemented compatibility-shim architecture

## Why The Compatibility Layer Stays

Current scenes and prefabs are still serialized against `GameManager`, `Player`, `SceneInitializer`, and `DoorClick`. Keeping those scripts as thin bridges lets the new architecture ship now without hand-editing Unity YAML or breaking serialized references.

## Follow-Up

- Create `SceneReference` assets and replace active `sceneName` string usage in the editor
- Assign `PlayerSettings` assets where designers should own movement tuning
- Convert live scene objects from tag fallback to direct `IInteractable` components
- Remove compatibility facades after scene and prefab references are fully migrated

## Validation Notes

- Performed a code-level migration sweep for remaining `GameManager.instance` usage and confined the new architecture to compatibility-safe entry points.
- Unity Editor compilation was not runnable through MCP in this session because the Unity MCP server required API-key authentication.
