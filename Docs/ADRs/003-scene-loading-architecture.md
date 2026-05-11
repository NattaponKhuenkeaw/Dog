# ADR-003: Scene Loading Architecture

| Field | Value |
|-------|-------|
| Status | **Accepted** |
| Date | 2026-05-11 |
| Author | Senior Architecture AI |
| GDD Section | Canonical Future Scene Roles |

## Context

Scene transitions were previously scattered across `DoorClick` with raw `SceneManager.LoadScene(sceneName)` calls and mixed concerns for door traversal, restart, and quit behavior.

## Decision

Introduce a dedicated scene-loading layer:

- `SceneLoader` centralizes transitions.
- `SceneTransitionData` carries transition side effects such as resetting session state or locking a door.
- `DoorInteractable` owns gameplay door transitions.
- `DoorClick` remains as a compatibility subclass so existing scene references keep working.
- `SceneReference` is the target typed scene asset format.

## Current Implementation Notes

- `SceneLoader`, `SceneTransitionData`, and `DoorInteractable` are implemented.
- `DoorClick` now delegates to the new flow.
- The codebase supports `SceneReference`, but live `SceneReference` assets still need to be created and assigned in the editor.
- Until those assets exist, legacy `sceneName` strings remain as the fallback source of truth for active scene objects.

## Consequences

### Positive

- Loading behavior now has a single control point.
- Door transitions can attach session side effects without duplicating logic.
- The project can move to typed scene assets without another runtime rewrite.

### Trade-Offs

- Runtime remains dual-path until `SceneReference` assets replace legacy string fields in scenes and prefabs.

## Related

- ADR-002: `SessionManager` and `SceneLoader` are part of the service layer
- ADR-006: Interaction components can now dispatch into `DoorInteractable`
