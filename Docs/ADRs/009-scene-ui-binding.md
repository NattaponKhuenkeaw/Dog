# ADR-009: Scene UI Binding

| Field | Value |
|-------|-------|
| Status | **Accepted** |
| Date | 2026-05-11 |
| Author | Senior Architecture AI |
| GDD Section | State separated from presentation |

## Context

Scene UI had been wired through `GameManager.InitScene(...)` and per-frame `UpdateUI()` polling. That approach tightly coupled UI to the runtime manager and made scene wiring fragile.

## Decision

Move scene UI to event-driven binders:

| Binder | Responsibility |
|--------|----------------|
| `HealthUI` | Health slider refresh and damage overlay flash |
| `EnergyUI` | Energy slider refresh |
| `FlashlightUI` | Flashlight text refresh and light binding |
| `InventoryHotbarUI` | Hotbar button binding and icon refresh |
| `DeathSequenceUI` | Video/death screen response to health events |

## Current Implementation Notes

- The binder components are implemented and subscribe to service events.
- `SceneInitializer` now acts as a bridge that instantiates and configures the binders from existing serialized scene references.
- `GameManager.UpdateUI()` polling is gone from the runtime path.
- `SceneInitializer` is intentionally still present until binders are attached directly in scenes and prefabs.

## Consequences

### Positive

- UI now reacts to health, energy, flashlight, inventory, and death events.
- New UI elements can be added without expanding a monolithic manager API.
- Scene-local UI wiring is now isolated from gameplay state ownership.

### Trade-Offs

- `SceneInitializer` remains as a migration helper for current scenes.
- The final editor cleanup pass still needs to move binder ownership fully into scene assets.

## Related

- ADR-002: Service events provide the runtime source of truth
- ADR-004: Player runtime no longer owns UI state
