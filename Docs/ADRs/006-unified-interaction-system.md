# ADR-006: Unified Interaction System

| Field | Value |
|-------|-------|
| Status | **Accepted** |
| Date | 2026-05-11 |
| Author | Senior Architecture AI |
| GDD Section | Interaction system ownership |

## Context

Player interaction was previously hardcoded through trigger tag checks in player logic, which meant every new interactable required changes to the player script.

## Decision

Adopt an interaction contract:

- `IInteractable` defines prompt, input requirement, availability, interaction start, and interaction end.
- `InteractInputType` defines the expected control gesture.
- `DoorInteractable`, `HideSpotInteractable`, and `StairInteractable` are the new component model.
- `PlayerInteraction` is the dispatch layer.

## Current Implementation Notes

- The interface and dispatcher are implemented.
- `DoorInteractable` is live, with `DoorClick` preserved as the serialized compatibility type.
- `HideSpotInteractable` and `StairInteractable` are available for scene migration.
- `PlayerInteraction` still falls back to legacy tags while live scenes are converted.

## Consequences

### Positive

- New interactables can become self-contained components.
- Player logic no longer needs to know every interaction type in advance.
- The runtime now supports a clean migration from tag routing to component routing.

### Trade-Offs

- The project temporarily supports both interface and tag paths.
- Editor-side scene cleanup is still required to remove the fallback behavior.

## Related

- ADR-003: `DoorInteractable` uses the centralized scene-loading path
- ADR-004: `PlayerInteraction` is the player-side dispatcher
