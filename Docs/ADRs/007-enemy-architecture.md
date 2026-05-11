# ADR-007: Enemy Architecture — EnemyDirector + Data-Driven Behaviors

| Field       | Value                    |
|-------------|--------------------------|
| Status      | **Proposed**             |
| Date        | 2026-05-11               |
| Author      | Senior Architecture AI   |
| GDD Section | [Enemies and Threats](file:///c:/Users/Apricha/Documents/GitHub/Dog/Assets/Core/Doc/GDD_Dog😋%20Instruction.md) §154–209, [EnemyDirector ownership](file:///c:/Users/Apricha/Documents/GitHub/Dog/Assets/Core/Doc/GDD_Dog😋%20Instruction.md) §508 |

## Context

Current enemy implementation consists of 4 isolated scripts:

| Script | Lines | GDD Match | Issues |
|--------|-------|-----------|--------|
| `Stalker.cs` | 51 | Partial | Simple chase-and-pass, no anti-stall pressure, no audio warning phase, no persistent threat |
| `SpawnerSt.cs` | ~40 | Partial | Spawn-behind logic works, but light blink is hardcoded and single-use |
| `The Ghast.cs` | ~60 | Mismatch | Behaves as red-light/green-light instead of GDD's hallway rush lethality |
| `Enemy(indoor).cs` | ~30 | Minimal | Basic damage + self-destruct, no City Dweller flashlight-interaction |

**Key problems:**
1. No shared enemy interface — each enemy has completely different APIs
2. No central **pacing/orchestration** — enemies spawn by scene, not by threat escalation
3. Balance values are all `public float` fields scattered across 4 files, not in data
4. Naming: `SpawnerSt` is cryptic, `The Ghast` has a space, `Enemy(indoor)` has parentheses in the filename
5. No concept of "warning → approach → lethal" phases described in the GDD

## Decision

### 1. `IEnemy` Interface + `EnemyBase` Abstract Class

```csharp
public interface IEnemy
{
    EnemyDefinition Definition { get; }
    EnemyPhase CurrentPhase { get; }        // Warning, Approaching, Attacking, Retreating
    void Initialize(EnemyDefinition def);
    void ForceRetreat();                     // For counterplay (Revolver, Energy Drink escape)
}

public abstract class EnemyBase : MonoBehaviour, IEnemy { ... }
```

Concrete enemies: `StalkerEnemy`, `GhastEnemy`, `CityDwellerEnemy`, `LockerHeartbeat` — all extend `EnemyBase`.

### 2. `EnemyDirector` Service

```csharp
public class EnemyDirector : MonoBehaviour
{
    // Threat pacing state
    public float ThreatLevel { get; private set; }

    public void RegisterFloorEncounters(FloorDefinition floor);
    public void OnPlayerStall(float stallDuration);
    public void OnPlayerBacktrack(float distance);
    public void SpawnThreat(EnemyDefinition def, SpawnContext context);
}
```

The `EnemyDirector` replaces `SpawnerSt.cs` and arbitrary scene-trigger spawning. It tracks threat accumulation (based on time, backtracking, search time) and orchestrates when/where enemies appear — matching the GDD's pressure-based design.

### 3. Phase-Based Enemy Behavior

Every enemy follows: **Warning → Approach → Attack → Resolve**

- **Warning**: Audio cue, visual cue (light flicker), brief delay
- **Approach**: Movement toward player or hazard zone
- **Attack**: Damage/kill window
- **Resolve**: Retreat, despawn, or persist (Stalker persists)

This matches the GDD's emphasis on *"threat anticipation"* and *"warning language"* before lethality.

## Alternatives Considered

| Option | Pros | Cons |
|--------|------|------|
| A: Keep per-enemy isolated scripts | No migration | No pacing, no shared interface, naming mess |
| B: Full ECS/DOTS enemy system | Maximum performance | Overkill, learning curve, Unity 6 DOTS maturity |
| **C: Interface + Director + SO definitions** | GDD-aligned, scalable, data-driven balance | More architecture up front |

## Consequences

### Positive
- Enemy balance tuning is SO-only (no code changes)
- `EnemyDirector` implements GDD's anti-stall, backtrack-pressure, and floor-cadence rules
- New enemies are a new class + SO asset — no existing code touched
- Naming is cleaned up in the process

### Negative / Trade-offs
- Existing enemy scripts need significant rewrite (they are small, so risk is low)
- Phase system adds complexity vs current simple chase

### Migration
1. Create `IEnemy.cs`, `EnemyBase.cs`, `EnemyPhase.cs` enum
2. Create `EnemyDefinition.cs` SO (from ADR-005)
3. Rewrite `Stalker.cs` → `StalkerEnemy.cs` extending `EnemyBase` with phase logic
4. Rewrite `The Ghast.cs` → `GhastEnemy.cs` with hallway rush + warning
5. Rewrite `Enemy(indoor).cs` → `CityDwellerEnemy.cs` with flashlight interaction
6. Create `EnemyDirector.cs` service, absorbing `SpawnerSt.cs` logic
7. Delete old scripts: `Stalker.cs`, `SpawnerSt.cs`, `The Ghast.cs`, `Enemy(indoor).cs`
8. Each enemy rewrite is an independent commit

## Related
- ADR-002: `EnemyDirector` registers as a service
- ADR-005: `EnemyDefinition` SO provides balance data
- GDD §154–209: Full enemy design specs
- GDD §508: EnemyDirector system ownership
- Scripts affected: All 4 enemy scripts (rewrite/delete), `Player.cs` (remove jumpscare — moves to `StalkerEnemy`)
