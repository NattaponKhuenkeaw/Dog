# ADR-010: Scene Roles and Responsibilities

| Field       | Value                    |
|-------------|--------------------------|
| Status      | **Accepted**             |
| Date        | 2026-05-11               |
| Author      | Senior Architecture AI   |
| GDD Section | [Canonical Future Scene Roles](file:///c:/Users/Apricha/Documents/GitHub/Dog/Assets/Core/Doc/GDD_Dog😋%20Instruction.md) §474–494 |

## Context

The project currently has several scenes with inconsistent naming and usage. The GDD identifies specific roles for scenes, but the existing repo state reflects a prototype-heavy approach where testing logic and experimental features are mixed with gameplay.

## Decision

Assign existing scenes to their **Canonical Roles** as defined in the GDD.

### Scene Role Mapping

| Current Scene | Canonical Role | Responsibility |
|---------------|----------------|----------------|
| `Menu.unity` | `Bootstrap/Menu` | Initial app setup, main menu, persistence loading, session bootstrap. |
| `main.unity` | `FloorRuntime` | Hallway progression, traversal encounters, floor objectives, and descent flow. |
| `room.unity` | `RoomSearch` | Dark room investigation, additive search sub-scenes, randomized search events. |
| `Test.unity` | `TestSandbox` | Isolated verification of flashlight, movement, or enemy AI (Debug only). |

### Scene Hierarchy Rule

1. **Bootstrap Scene** must remain the entry point for all data systems (`SessionManager`, `Services`).
2. **Runtime Scenes** should not own persistent state; they react to state loaded from `Services`.
3. **Sandbox Scenes** must never be included in production builds.

## Alternatives Considered

| Option | Pros | Cons |
|--------|------|------|
| A: Keep scenes as-is | No refactor effort | Confusion over where performance tests vs gameplay happen |
| **B: Enforce Role Mapping** | Clear development focus, GDD alignment | Requires minor repositioning of scene content |

## Consequences

### Positive
- Unified understanding of which scene handles which part of the loop.
- Easy to identify "stray" scenes that should be quarantined.
- Clear path for future floor generation (building on `FloorRuntime` principles).

### Negative / Trade-offs
- None significant.

## Related
- ADR-003: Scene Loading Architecture
- ADR-009: Scene UI Binding
