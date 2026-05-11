# ADR-002: Decompose GameManager Singleton Into Focused System Owners

| Field | Value |
|-------|-------|
| Status | **Accepted** |
| Date | 2026-05-11 |
| Author | Senior Architecture AI |
| GDD Section | Canonical Future System Ownership |

## Context

`GameManager.cs` had become a God-object that owned player health, energy, flashlight state, inventory, door locks, death flow, UI references, and reset logic. Every major runtime script depended on `GameManager.instance.*`, which made scene transitions fragile and every change cross-cutting.

## Decision

Split the runtime into focused services registered in a static locator:

| Service | Responsibility |
|---------|----------------|
| `HealthSystem` | Health state, damage, healing, death events |
| `EnergySystem` | Stamina value, drain, regeneration |
| `FlashlightSystem` | Toggle state, power drain, recharge, light binding |
| `InventoryManager` | Item list, add/use flow, inventory change events |
| `DoorLockRegistry` | Door lock tracking |
| `SessionManager` | Session reset, persistent state, player spawn restore |
| `SceneLoader` | Centralized scene transition entry point |

`Services.cs` is the shared lookup surface for these systems.

## Current Implementation Notes

- The service split is implemented.
- `GameManager` is intentionally still present as a bootstrap and compatibility facade.
- The facade keeps current scenes working while scripts migrate off direct `GameManager.instance.*` access and while prefabs/scenes still serialize references to the old component.
- This is an accepted transitional state, not a rollback.

## Consequences

### Positive

- Runtime responsibilities are now isolated and event-friendly.
- New code can target `Services.*` instead of extending a monolithic manager.
- Health, energy, flashlight, inventory, and session flow can evolve independently.

### Trade-Offs

- A compatibility layer remains until scene assets are fully rewired.
- Some legacy scripts still read through `GameManager` until the next cleanup pass.

## Related

- ADR-003: Scene loading now routes through `SceneLoader`
- ADR-004: Player runtime now targets the split services
- ADR-009: UI now binds to service events instead of manager polling
