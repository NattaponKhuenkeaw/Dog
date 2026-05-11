# ADR-005: Data-Driven Item, Enemy, and Room Definitions via ScriptableObjects

| Field | Value |
|-------|-------|
| Status | **Implemented** |
| Date | 2026-05-11 |
| Author | Senior Architecture AI |
| GDD Section | `Docs/GDD/GDD_Dog😋 Instruction.md` Data-Driven Content Targets |

## Context

Game content was previously defined inline through hardcoded item enums, enemy balance fields, and room-local spawn structs. That made every new item, enemy, or room variant a code-change task and kept content iteration tightly coupled to runtime logic.

The GDD explicitly calls for `ItemDefinition`, `EnemyDefinition`, `FloorDefinition`, `EncounterDefinition`, and `SafeRoomShopDefinition` as ScriptableObject content assets.

## Decision

Phase 3 introduces a ScriptableObject-backed content layer with backward-compatible runtime integration:

1. `ItemDefinition` owns item metadata and effect settings.
2. `EnemyDefinition` owns shared enemy tuning values.
3. `RoomSearchDefinition` owns room prefab, item spawns, and threat spawns.
4. `EncounterDefinition`, `FloorDefinition`, and `SafeRoomShopDefinition` establish the authoring surface for later Phase 3 and Phase 4 systems.

Existing scene-facing MonoBehaviours stay in place so prefab and scene serialization does not break during the migration.

## Implemented Runtime Changes

- Added `ItemDefinition.cs`, `EnemyDefinition.cs`, `RoomSearchDefinition.cs`, `EncounterDefinition.cs`, `FloorDefinition.cs`, and `SafeRoomShopDefinition.cs`
- Added reusable item and enemy spawn entry types for room and encounter authoring
- Migrated `ItemPickup.cs` to support `ItemDefinition` while preserving legacy inspector fields
- Migrated `InventoryManager.UseItem()` to a definition-driven category handler path with legacy fallback
- Migrated `Stalker`, `TheGhast`, and `Enemy` to optionally read tuning from `EnemyDefinition`
- Upgraded `roomManag` to consume `RoomSearchDefinition[]` without breaking the existing serialized scene component

## Consequences

### Positive

- Designers can add new content categories without changing core runtime code first
- Balance moves toward asset editing instead of code recompilation
- Runtime ownership is clearer: definitions describe content, MonoBehaviours execute behavior
- The migration stays safe for the current prototype scenes

### Trade-Offs

- Unity asset authoring is still required to get the full benefit of the new definitions
- Some content categories such as weapon, ammo, currency, and charm still need their dedicated runtime handlers
- `roomManag` remains as a compatibility wrapper until scenes are fully migrated

## Follow-Up

1. Create live `.asset` instances for current items, enemies, rooms, floors, and safe-room shop content.
2. Assign `ItemDefinition` references on pickup prefabs and `EnemyDefinition` references on active threat prefabs.
3. Replace remaining legacy room arrays once room scenes are authored entirely through `RoomSearchDefinition`.
4. Continue ADR-007 by introducing encounter orchestration around the new enemy definitions.

## Related

- ADR-002: `InventoryManager` remains the runtime consumer of item data
- ADR-007: `EnemyDefinition` is the data foundation for future `EnemyDirector` work
- GDD Phase 3: Data-driven content targets
