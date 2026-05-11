# Dog 🐕 — Project Stack & Context (For AI Assistants)

> **Copy-paste this into any AI chatbot's system prompt or first message to bootstrap context.**

## Project Identity
- **Game:** "Dog" — 2D side-scrolling horror/puzzle/mystery
- **Engine:** Unity 6000.3.4f1 with URP 2D lighting
- **Platform:** Mobile-first (touch), PC port secondary
- **Core Fantasy:** Cute anime girl descends 100-floor condo while the world has collapsed into monstrosity

## Tech Stack
- Unity Input System (no legacy input)
- Cinemachine for camera
- UGUI + TextMeshPro for UI
- Light2D for darkness/flashlight gameplay
- Unity Purchasing for IAP
- C# only, no ECS, no third-party networking currently

## Core Systems (Current State)
| System | Location | State |
|--------|----------|-------|
| Player Controller | `Assets/Core/Script/Player.cs` | Prototype, works |
| Game Manager | `Assets/_custom/Scrip/GameManager.cs` | Singleton, needs splitting |
| Flashlight | `Assets/_custom/Scrip/Flaslight/` | Works, typo in folder name |
| Enemies | `Assets/_custom/Scrip/Enemy/` | Prototype, partially matches GDD |
| Inventory | `Assets/_custom/Scrip/ItemPickUp.cs` | Basic, needs refactor |
| Room Search | `Assets/_custom/Scrip/roomManage.cs` | Random spawner, needs data-driven redesign |
| Scene Loading | `Assets/_custom/Scrip/LoedScene.cs` | Direct string loading |
| IAP/Economy | `Assets/_custom/Stor/` | Functional but secondary |

## Architecture Rules (From GDD)
1. **No jump, no vertical platforming** — left/right movement only
2. **Mobile-first** — all mechanics must work on touch before PC
3. **Game feel > clean code** — if a refactor weakens tension/mood, it's wrong
4. **Don't grow GameManager** — split into separate system owners
5. **Data-driven content** — move toward ScriptableObject definitions
6. **English-only identifiers** — PascalCase, no typos in new code

## Key Docs
- GDD: `Docs/GDD/GDD.md`
- ADRs: `Docs/ADRs/` — check before making architecture decisions
- DevLog: `Docs/DevLog/` — check recent entries for current state

## Commit Convention
```
type(scope): description

Types: feat, fix, refactor, docs, style, test, chore
Scopes: player, inventory, enemy, flashlight, room-search, ui, economy, build
```
