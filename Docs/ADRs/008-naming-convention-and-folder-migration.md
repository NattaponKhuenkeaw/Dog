# ADR-008: Naming Convention Enforcement and Folder Structure Migration

| Field       | Value                    |
|-------------|--------------------------|
| Status      | **Accepted**             |
| Date        | 2026-05-11               |
| Author      | Senior Architecture AI   |
| GDD Section | [Naming Policy](file:///c:/Users/Apricha/Documents/GitHub/Dog/Assets/Core/Doc/GDD_Dog😋%20Instruction.md) §639–665 |

## Context

The codebase has significant naming drift from the GDD's naming policy (*"English-only identifiers, PascalCase class names, no typo-driven permanent names"*):

### Current Naming Issues

| Current Name | Problem | Target Name |
|-------------|---------|-------------|
| `Core/Script/` | Was `_custom/Scrip/` — incomplete rename | `_Project/Gameplay/` (per GDD folder target) |
| `Flaslight/` | Typo: missing 'h' | `Flashlight/` |
| `LoedScene.cs` | Typo: "Loed" | Delete (replaced by `SceneLoader.cs`) |
| `roomManage.cs` / class `roomManag` | camelCase, truncated | Delete (replaced by `RoomSearchDirector.cs`) |
| `ItemPickUp.cs` / class `ItemPickup` | Inconsistent casing (file vs class) | `ItemPickup.cs` |
| `SpawnerSt.cs` | Cryptic abbreviation | `StalkerSpawner.cs` or absorbed into `EnemyDirector` |
| `The Ghast.cs` | Space in filename | `GhastEnemy.cs` |
| `Enemy(indoor).cs` | Parentheses in filename | `CityDwellerEnemy.cs` |
| `FL_Manager.cs` | Cryptic abbreviation | `FlashlightManager.cs` |
| `flashlight_Control.cs` | snake_case, lowercase | `FlashlightController.cs` |
| `DoorClick` (class in LoedScene.cs) | Misleading name in wrong file | `DoorInteractable.cs` |
| `ItemType.Baterry` | Typo: "Baterry" | `Battery` (or `Flashlight` item category) |
| Mixed-language comments | Thai comments throughout | English-only in new code |

### Folder Structure Gap

Current:
```
Assets/Core/Script/          ← flat, mixed responsibilities
Assets/Core/Store/           ← IAP only
```

GDD target:
```
Assets/_Project/
  Core/        (Bootstrap, GameFlow, SaveProfile)
  Gameplay/    (Player, Interaction, Inventory, Flashlight, RoomSearch, Enemies)
  Content/     (Items, Enemies, Floors, Encounters, SafeRoom)
  UI/          (HUD, Menus, Shop)
```

## Decision

### 1. Adopt GDD Target Folder Structure

Create the `Assets/_Project/` hierarchy as ADRs 002-007 produce new scripts. Do **not** move existing files until their replacement is written and tested.

### 2. Naming Rules (Enforced Going Forward)

| Rule | Example |
|------|---------|
| Class names: PascalCase | `PlayerMovement`, `HealthSystem` |
| File name = Class name | `PlayerMovement.cs` |
| No spaces, parens, or special chars in filenames | `GhastEnemy.cs` not `The Ghast.cs` |
| Folders: PascalCase | `Flashlight/` not `Flaslight/` |
| Comments: English only for new/modified code | Existing Thai comments left until file is rewritten |
| No abbreviated names | `FlashlightManager` not `FL_Manager` |

### 3. Legacy Bridge Period

During migration, both `Core/Script/` (old) and `_Project/` (new) coexist. When an old script is fully replaced:
1. Mark old script with `[Obsolete("Replaced by NewScript — ADR-XXX")]`
2. Delete old script in the same PR that validates new script works
3. Update any prefab/scene references

## Alternatives Considered

| Option | Pros | Cons |
|--------|------|------|
| A: Rename files in-place now | Quick | Breaks prefab/scene GUID references, high risk |
| B: Big-bang folder restructure | Clean in one step | Massive PR, many merge conflicts, risky |
| **C: Incremental migration with bridge period** | Safe, reviewable, no broken references | Two folder trees temporarily |

## Consequences

### Positive
- Every new file follows clean conventions from day one
- No surprise GUID breaks from mass renames
- Folder structure aligns with GDD system boundaries
- Contributors always know where new code goes

### Negative / Trade-offs
- Temporary coexistence of `Core/Script/` and `_Project/` (acceptable during refactor phases)
- Requires discipline to not add new code to old locations

### Migration
1. Create `Assets/_Project/` folder skeleton (empty folders, `.gitkeep`)
2. As each ADR (002–007) is implemented, new scripts go into `_Project/` path
3. Old scripts get `[Obsolete]` when replaced
4. After all old scripts are replaced, delete `Assets/Core/Script/` folder
5. No standalone "rename" commits — naming fixes are part of functional changes

## Related
- ADR-002 through ADR-007: All produce new scripts in `_Project/`
- GDD §639–665: Naming policy
- GDD §540–571: Recommended folder direction
