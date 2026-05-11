# ADR-004: Player Controller Refactor

| Field | Value |
|-------|-------|
| Status | **Accepted** |
| Date | 2026-05-11 |
| Author | Senior Architecture AI |
| GDD Section | Player controller system ownership |

## Context

`Player.cs` previously contained movement, input thresholds, interaction dispatch, hiding, footstep audio, jumpscare handling, and direct `GameManager.instance.*` mutation in one large component.

## Decision

Split the runtime into focused player components:

| Component | Responsibility |
|-----------|----------------|
| `PlayerMovement` | Input reading, walking/running, boundary clamping, animation, energy drain |
| `PlayerInteraction` | Trigger detection and interaction dispatch |
| `PlayerHiding` | Hide lifecycle, warning fade, hide damage timing |
| `PlayerAudio` | Footstep timing and clip switching |
| `PlayerSettings` | Optional ScriptableObject for thresholds and tuning values |

## Current Implementation Notes

- The split runtime components are implemented and initialized from `Player`.
- `Player` now acts as a composition root and compatibility shell for existing serialized scene references.
- `PlayerInteraction` prefers the new `IInteractable` path but still supports legacy tag fallback during migration.
- Jumpscare behavior is still bridged through `Player` for compatibility with current content.

## Consequences

### Positive

- Movement, hiding, interaction, and audio can now evolve independently.
- New mechanics can land as new components instead of growing one file.
- The player now talks to `Services.*` for runtime state instead of owning those systems directly.

### Trade-Offs

- The final deletion of the compatibility shell is deferred until scenes no longer depend on the old serialized fields.
- Some legacy trigger-based behavior still exists as a bridge.

## Related

- ADR-002: Player logic now consumes focused services
- ADR-006: Interaction dispatch now has an interface path
