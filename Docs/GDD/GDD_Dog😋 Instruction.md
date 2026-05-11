# Dog - GameOverall Instruction

## Purpose

This document is the shared north star for AI coders, senior engineers, technical designers, and future contributors working on **Dog**.

It has two equally important jobs:

1. Preserve the intended identity of the game.
2. Give the current prototype a clean technical direction without pretending that the clean architecture already exists.

Read this as a **design-led execution charter**:

- The **game fantasy, emotional tone, and player loop are authoritative**.
- The **current codebase is prototype truth**, not final architecture.
- Any refactor must protect the intended player experience while improving maintainability.

---

## Project Identity

| Field | Direction |
| --- | --- |
| Working Title | **Dog** |
| Genre | **2D side-scrolling horror / puzzle / mystery / Single-Player** |
| Core Fantasy | A cute anime girl must descend from the **100th floor condo** to the **ground floor** while the world has collapsed into monstrosity. |
| Player Promise | Survive, investigate, evade, and piece together why people changed, who can still be trusted, and whether escape is even possible. |
| Platform Priority | **Mobile-first**, with **PC port support** |
| Camera / Play Plane | **2D side view**, tight traversal lanes, eerie hallway framing |
| Tone | **Cute melancholy**, anxiety, liminal loneliness, eerie intimacy, horror relief cycles |
| Story Intent | Mystery-driven escape narrative with suspicious or faithless Waifu characters and uncertain truths |

### Experience Pillars

1. **Tension by movement and delay**
   The player is always pushed forward. Taking too long, backtracking, or over-searching increases threat pressure.

2. **Cute protagonist in a hostile world**
   The contrast between vulnerable anime softness and ugly horror is a core identity, not a decoration.

3. **Short-term safety, long-term dread**
   Safe rooms and hiding spots are relief tools, not permanent comfort.

4. **Mystery through survival**
   Story is not delivered as passive exposition. It is discovered while making risky decisions under pressure.

5. **Mobile-first clarity**
   Every mechanic must remain readable and playable on touch devices before it is optimized for PC.

---

## Visual, Mood, and Audio Direction

### Mood Board Keywords

- Blue
- Dark
- Loneliness
- Warm highlights
- Red danger accents
- High contrast
- Weird and eerie
- Liminal space aesthetic
- Anxious girl energy

### Art Direction

- Anime-style heroine with readable silhouettes and expressive fear states.
- Interiors should feel familiar but wrong: condo hallways, rooms, corridors, utility spaces, elevator zones, stairs, and transitional dead spaces.
- Warm safe zones must feel emotionally rare and intentional, not generic bright rooms.
- Horror entities should contrast the heroine with shape, speed, sound, and unpredictability rather than gore alone.

### Audio Direction

- Sound is critical for awareness, not just atmosphere.
- Footsteps, heartbeat, light flicker, chase cues, locker pressure, and distant warnings should function as gameplay information.
- Silence and near-silence are valuable. Do not overfill scenes with constant ambience.

---

## Platform and Input Philosophy

## Primary Rule

**Mobile is the authoritative input model.** PC support is a mapped port layer, not a separate game design.

### Mobile Controls

- Move left or right only
- No jump
- Dynamic input for sprint
- Joystick up for world interaction:
  - doors
  - lockers
  - stairs
- Joystick down for exit interaction:
  - leave hiding
  - cancel or disengage context states
- Tap to open inventory and use items

### PC Port Rules

- Mirror the same verbs and timing windows.
- Do not redesign encounters around keyboard precision.
- PC input may feel cleaner, but mobile readability and fairness remain the balance target.

---

## Core Game Loop

The fundamental loop is:

1. Enter a floor segment.
2. Read the environment for danger, branching rooms, and progression blockers.
3. Search for a **key**, **progress item**, or **puzzle solution**.
4. Manage time pressure and enemy/event escalation.
5. Use hiding, sprint bursts, flashlight, and limited inventory to survive.
6. Unlock access to the next floor or next segment.
7. Reach periodic relief beats such as safe rooms.
8. Repeat until the descent reaches the ground floor and the narrative climax.

### Progression Cadence

- Every floor should feel like a pressured traversal puzzle, not a combat stage.
- Every 5 floors should culminate in a **Safe Room** relief beat.
- The player should repeatedly weigh:
  - speed vs search
  - safety vs reward
  - knowledge vs exposure
  - item use now vs item saving for later

### Failure / Recovery Pattern

- Most fatal failures come from awareness mistakes, hesitation, greed, or misreading threat rules.
- Recovery tools exist, but resources should remain limited enough to preserve dread.
- Relief should reset tension slightly, never erase it fully.

---

## Canonical Gameplay Content

## Items

| Item | Role | Intended Effect |
| --- | --- | --- |
| Energy Drink | Escape / chase utility | Gain **Speed Boost I**, fully restore stamina, and prevent stamina drain for 2 seconds |
| Bandage | Recovery | Stand still to heal **+5 HP** |
| Battery | Flashlight sustain | Increase flashlight durability by **+1** unit / charge step |
| Revolver Requiem | Rare anti-threat tool | Stun enemy from long range; requires bullets and has a jam chance |
| Bullet | Ammunition | Required to make Revolver Requiem function |
| Coin | Economy | Utility exchange currency for in-game and out-game systems |
| พระรอด | Passive survival charm | On lethal damage, prevent death, grant brief immunity and **Speed Boost II** for 2 seconds, then break |

## Enemies and Threats

### STALKER

**Role:** Relentless pursuit pressure and anti-stalling system.

**Design intent**

- Follows the player closely.
- Can pursue even into Safe Rooms.
- Triggers if the player takes too long.
- Backtracking within a specified distance increases encounter chance.
- Only appears from behind the player.
- May shadow the player first with audio warning before full aggression.
- If the player notices it, it becomes extremely fast.
- Contact means game over unless a special survival rule intervenes.

**Counterplay**

- Revolver Requiem can interrupt it, but with increased jam risk.
- Energy Drink can create escape space if a hiding route exists.
- Advanced players may eventually kill it with special items and skill.

### City Dweller

**Role:** Room Search pressure threat.

**Design intent**

- Can spawn during **Room Search**.
- If the player flashes them, the player takes **25 damage**.
- The threat has a delay before the damage resolves.

### Locker Heartbeat

**Role:** Anti-turtling hiding punishment.

**Design intent**

- Triggers if the player remains in a locker too long.
- Reduces **50% of max HP**.

### The Ghast

**Role:** Sudden hallway lethality with warning language.

**Design intent**

- Randomized appearance.
- Brief warning before engagement.
- Rushes through the hallway and can instantly kill.

**Counterplay**

- Hide or run away depending on scene layout and timing.

## System Mechanics

### Hiding Spot

- Spawn and placement are randomized by scene.
- The player runs to the spot and uses the interact input to hide.
- While hidden, visibility is heavily limited.
- Enemy heartbeat and direction feedback should still communicate danger.
- Hiding is not a universal invulnerability state.
- Enemies may still sense or pressure the player depending on threat type.

### Room Search

- Room Search content is randomized per scene.
- Players interact with doors to open room search spaces.
- The room view becomes extremely dark and flashlight-dependent.
- Input context may temporarily shift toward search and flashlight use.
- Items, enemies, and event payloads are primarily sourced from this system.
- The player is still vulnerable during Room Search.
- Once a room is fully cleared and its event progress reaches 100%, that room may later become a safer hiding zone.
- Target content rule: roughly **90% of items** should originate from Room Search content.

### Safe Room

- Appears every 5 floors.
- Exists as a relief beat with cozy aesthetic contrast.
- Supports item purchase using coins.
- Is safer than normal traversal spaces, but not absolutely safe from STALKER.
- If STALKER enters a Safe Room, its timing pressure should be slower and random event triggers should be reduced.

---

## Inspirations and Design Boundaries

### Primary Inspirations

- **ROBLOX DOORS**
- **ROBLOX Pressure**
- **White Day: A Labyrinth Named School**
- **The Coma**
- **Kageroh: Shadow Corridor**

### What To Borrow

- Pressure-based progression
- Threat anticipation
- Environmental dread
- Search-risk tension
- Spatial memory and pattern learning

### What Not To Copy Blindly

- The project should not become a pure chase game.
- The project should not become combat-first survival horror.
- The heroine's anime identity should not be stripped away in pursuit of generic realism.
- Mobile usability should not be sacrificed for complexity that only feels good on PC.

---

## Actual Project Stack and Repo Reality

This section reflects the repository as it exists today.

### Technical Stack

| Area | Current Repo Truth |
| --- | --- |
| Engine | **Unity 6000.3.4f1** |
| Render Pipeline | **Universal Render Pipeline** with 2D lighting |
| Gameplay Dimension | **2D** |
| Input | **Unity Input System** (`com.unity.inputsystem`) |
| Camera | **Cinemachine** package present (`com.unity.cinemachine`) |
| UI | **UGUI** + **TextMeshPro** |
| Lighting | `Light2D`, URP 2D light behavior, dark-room flashlight logic |
| Monetization | **Unity Purchasing** (`com.unity.purchasing`) |
| Test Package | `com.unity.test-framework` is installed |
| Build Scenes | `Assets/Scenes/Menu.unity`, `Assets/Scenes/main.unity`, `Assets/Scenes/room.unity` |
| Prototype Code Zone | `Assets/_custom/Scrip` |
| Key Asset Zones | `Assets/_custom`, `Assets/Scenes`, `Assets/CrystalFramework`, `Assets/Nine Pines Animation` |

### Notable Current Packages

- `com.unity.feature.2d`
- `com.unity.inputsystem`
- `com.unity.render-pipelines.universal`
- `com.unity.cinemachine`
- `com.unity.ugui`
- `com.unity.purchasing`
- `com.unity.timeline`
- `com.unity.test-framework`

---

## Current Prototype Truth

This is the most important reality check for future contributors.

**The current implementation already contains usable gameplay ideas, but it is still prototype-grade.**

### Existing Runtime Shape

- A `GameManager` singleton is used as the central runtime state hub.
- Scene transitions are handled directly by scene-loading components.
- Scene UI references are reconnected at runtime through a scene initializer pattern.
- Item inventory, flashlight state, health, energy, death flow, and door-lock state are all managed from the singleton layer.
- Enemy logic is mostly direct MonoBehaviour behavior with limited abstraction.

### What Exists Today

#### Player Runtime

Current script: `Assets/_custom/Scrip/Player.cs`

What it currently does:

- Reads movement through the Input System `Move` action.
- Supports only left/right movement.
- Uses input magnitude to distinguish walk and sprint behavior.
- Uses vertical input for door entry, hiding, and stairs interaction.
- Disables movement while hidden or in stop states.
- Handles footstep audio cadence for walking and running.
- Clamps movement to x-axis boundaries.
- Consumes energy while running.
- Plays a jumpscare sequence on STALKER collision.

#### Global Runtime State

Current script: `Assets/_custom/Scrip/GameManager.cs`

What it currently does:

- Uses singleton + `DontDestroyOnLoad`.
- Stores health, max health, energy, max energy, flashlight state, and door lock state.
- Controls damage overlay and death video flow.
- Tracks movement and running flags used by other systems.
- Owns a 3-slot inventory / hotbar model.
- Updates scene UI references through `InitScene`.
- Stores locked door ids in a `HashSet<string>`.
- Resets game state through a direct reset function.

#### Scene Wiring

Current scripts:

- `Assets/_custom/Scrip/SceneInitializer.cs`
- `Assets/_custom/Scrip/LoedScene.cs` (`DoorClick`)

What they currently do:

- Rebind scene-local UI to the persistent `GameManager`.
- Reposition the player on scene load using last known position.
- Load scenes directly by string name.
- Lock doors immediately after scene transition.
- Reset global state on main scene changes.

#### Room and Spawn Prototype

Current script: `Assets/_custom/Scrip/roomManage.cs`

What it currently does:

- Randomly picks a room prefab.
- Instantiates the selected room.
- Scans child transforms for spawn markers by name.
- Randomly spawns item prefabs into valid points by chance.

This is useful as a prototype, but should eventually become a data-driven encounter / room-search system.

#### Item and Inventory Prototype

Current script: `Assets/_custom/Scrip/ItemPickUp.cs`

What it currently does:

- Allows click/touch pickup using screen-to-world ray checks.
- Blocks pickup if inventory is full.
- Wraps item metadata into `ItemData`.
- Pushes items into `GameManager` inventory.

Current implementation note:

- Existing item code only partially covers the intended design item list.
- `ItemType` currently reflects prototype categories rather than the full future item taxonomy.

#### Flashlight Prototype

Current scripts:

- `Assets/_custom/Scrip/Flaslight/flashlight_Control.cs`
- `Assets/_custom/Scrip/Flaslight/FL_Manager.cs`

What they currently do:

- Manage flashlight toggle and drain behavior.
- Use `Light2D` as the core darkness gameplay tool.
- Add flicker behavior and enemy interaction logic.
- Support enemy damage and jumpscare feedback through flashlight exposure logic.

#### Enemy Prototype

Current scripts:

- `Assets/_custom/Scrip/Enemy/Stalker.cs`
- `Assets/_custom/Scrip/Enemy/SpawnerSt.cs`
- `Assets/_custom/Scrip/Enemy/The Ghast.cs`
- `Assets/_custom/Scrip/Enemy/Enemy(indoor).cs`

What they currently do:

- `Stalker.cs`: simple chase-and-pass behavior toward the player.
- `SpawnerSt.cs`: spawns a stalker behind the player, plays sound, blinks lights, and disables repeat triggering.
- `The Ghast.cs`: currently behaves more like a red-light / green-light punish state than the intended hallway rush design.
- `Enemy(indoor).cs`: acts as a basic damage payload enemy with sound and self-destruction.

Important design note:

**Current enemy scripts do not yet fully match the richer intended behavior described earlier in this document.** Future contributors must treat the design section as target behavior and the prototype scripts as temporary implementation.

#### Hiding Prototype

Current related scripts:

- `Assets/_custom/Scrip/Player.cs`
- `Assets/_custom/Scrip/HideSpot.cs`

Current truth:

- Hiding already exists in playable form through the main player logic.
- There is also a duplicate / rough prototype script with inconsistent naming (`Coppy`) that should not be treated as final architecture.

#### Economy / IAP Prototype

Current scripts:

- `Assets/_custom/Stor/IAP_Store.cs`
- `Assets/_custom/Stor/IAP_ButtonView.cs`

What they currently do:

- Handle Unity IAP purchase callbacks.
- Award coins via `PlayerPrefs`.
- Toggle remove-ads style UI states.
- Toggle subscription / battle-pass-like UI states.

Important production note:

- Monetization exists in the repo and must be documented.
- Monetization is **not** the primary driver of the core horror loop and should remain secondary to the survival experience.

### Current Prototype Debt

The repo currently has these major technical issues:

- Singleton-heavy flow
- Direct scene references and scene-string loading
- Inconsistent English naming
- Typographical naming drift such as `Scrip`, `roomManag`, `Baterry`, `LoedScene`
- Mixed language comments and inconsistent comment quality
- Multiple systems coupled directly into `GameManager`
- Limited separation between data, state, presentation, and scene wiring
- Prototype duplicates and temporary scripts that blur source-of-truth ownership

---

## Canonical Future Scene Roles

These are contributor-facing target roles. They do **not** mean the repo already uses this structure.

| Scene Role | Purpose |
| --- | --- |
| `Bootstrap/Menu` | App startup, profile init, menu flow, options, session boot |
| `FloorRuntime` | Main traversal scene for hallway progression, encounters, floor objectives, and descent flow |
| `RoomSearch` | Search-mode space or additive sub-scene for dark room investigation and randomized search events |
| `SafeRoom` | Every-5-floor relief, shop, narrative pause, and resource planning |
| `TestSandbox` | Debug-only validation scene for isolated system testing |

### Mapping From Current Scenes

| Current Scene | Likely Future Role |
| --- | --- |
| `Menu.unity` | `Bootstrap/Menu` |
| `main.unity` | `FloorRuntime` |
| `room.unity` | Early `RoomSearch` prototype |
| `Test.unity` | `TestSandbox` or legacy internal scene |

---

## Canonical Future System Ownership

Future contributors should separate responsibilities into these system boundaries.

| System | Ownership |
| --- | --- |
| `GameFlow` | Session startup, floor progression, safe room cadence, win/lose state, scene transitions |
| `PlayerController` | Movement, stamina use, input interpretation, locomotion state |
| `Interaction` | Doors, lockers, stairs, pickups, search-state entry, contextual prompts |
| `Inventory` | Slot rules, item use, item definitions, pickup rules, passive item triggers |
| `Flashlight` | Toggle, drain, recharge, darkness readability, light-based threat interactions |
| `EnemyDirector` | Threat pacing, spawn rules, chase escalation, encounter orchestration, anti-stall pressure |
| `RoomSearch` | Search-state setup, event generation, room progress, item distribution, room safety completion |
| `Save/Profile` | Profile state, unlocked knowledge, coins, persistence, session recovery |
| `Economy/IAP` | Shop integration, entitlement handling, coin packs, non-consumables, subscription logic |

### Hard Rule

No future refactor should dump new features back into a single global MonoBehaviour just because it is faster in the short term.

---

## Target Architecture Direction

This is the intended technical destination after refactoring.

### Architecture Principles

1. **Game design authority first**
   The mood, pace, pressure, and mobile-first loop are more important than neat code alone.

2. **Data-driven content**
   Enemies, items, floors, encounters, and safe-room inventory should move toward authorable data assets rather than hardcoded scene logic.

3. **Thin scene wiring**
   Scenes should assemble references and presentation. They should not permanently own progression logic.

4. **State separated from presentation**
   UI, animation, sound, and VFX should react to gameplay state instead of defining it.

5. **Explicit module boundaries**
   Movement, interaction, inventory, economy, lighting, and enemy pacing should be individually testable or replaceable.

### Recommended Folder Direction

This is a target structure, not a claim about current repo state.

```text
Assets/_Project
  Core
    Bootstrap
    GameFlow
    SaveProfile
  Gameplay
    Player
    Interaction
    Inventory
    Flashlight
    RoomSearch
    Enemies
  Content
    Items
    Enemies
    Floors
    Encounters
    SafeRoom
  UI
    HUD
    Menus
    Shop
  Audio
  Art
  Tools
  Tests
```

### Data-Driven Content Targets

Future refactors should move toward ScriptableObject-style content definitions such as:

- `ItemDefinition`
- `EnemyDefinition`
- `FloorDefinition`
- `EncounterDefinition`
- `SafeRoomShopDefinition`

These definitions should eventually own:

- display metadata
- balance values
- spawn rules
- audio / VFX references
- rarity / risk tags
- economy hooks where relevant

### Runtime State Direction

Target runtime separation should roughly become:

- `SessionState`
- `PlayerRuntimeState`
- `InventoryState`
- `FloorProgressState`
- `RoomSearchState`
- `ThreatState`
- `EconomyState`

The current `GameManager` should eventually be reduced into orchestration, bootstrap, or service registration rather than permanent ownership of every system.

---

## Contributor Guardrails For AI Coders and Senior Engineers

### Preserve These Non-Negotiables

- The game remains **2D side-scrolling**.
- The player remains **left/right only** with **no jump**.
- Mobile-first input remains the source of truth.
- The heroine must remain emotionally readable and visually distinct from the horror world.
- Safe Rooms must preserve relief pacing every 5 floors.
- Room Search must remain risky, dark, and flashlight-dependent.
- STALKER must remain a pressure mechanic, not just a normal enemy unit.

### You May Refactor These Aggressively

- Global singleton sprawl
- Naming inconsistencies
- Scene reference wiring
- Duplicate scripts
- Inventory representation
- Room search spawn plumbing
- Enemy implementation details that do not yet match intended behavior

### Do Not Do These Things

- Do not silently reinterpret the core fantasy into generic survival horror.
- Do not turn the game into a combat-heavy shooter loop.
- Do not add jump, vertical platforming, or action-combo systems unless the design direction changes explicitly.
- Do not assume current prototype code reflects final balance or intended behavior.
- Do not grow `GameManager` with more unrelated responsibilities.
- Do not keep adding new logic under `Assets/_custom/Scrip` once a cleaner target module exists, except for temporary compatibility fixes.

### Naming Policy

Future production code should follow:

- English-only identifiers
- PascalCase class names
- clear noun / verb ownership
- no typo-driven permanent names
- one canonical script per responsibility

Examples of names that should eventually be normalized:

- `Scrip` -> `Scripts` or a better feature folder
- `roomManag` -> `RoomManager` or `RoomSearchDirector`
- `Baterry` -> `Battery`
- `LoedScene` -> `SceneLoader` or `DoorSceneTransition`
- `Coppy` -> remove or replace with the actual system owner

### AI Safety Rule

Every future implementation must clearly separate:

- **Current implementation**
- **Target architecture**

If a system is only planned but not built, contributors must say so explicitly in code comments, docs, task notes, and change summaries.

Do not hallucinate completed architecture.

---

## Delivery Expectations For Future Work

When adding or refactoring systems, contributors should prefer this order:

1. Stabilize behavior.
2. Clarify ownership.
3. Convert repeated logic into reusable modules.
4. Move gameplay definitions into data.
5. Expand content only after the system is stable enough to carry it.

### New Enemy Work

New enemies should eventually live under the future enemy module and be driven by data plus encounter orchestration rules, not by isolated one-off scene hacks.

Until the refactor is complete:

- prototype behavior may still be added near current enemy scripts if necessary
- but design intent, timing rules, and counterplay must be documented in a clean way
- any new enemy should specify:
  - trigger condition
  - warning language
  - kill or damage rule
  - counterplay
  - relation to Room Search, hallway traversal, or Safe Room logic

### New Item Work

New items should eventually become data-defined content with:

- category
- stack / slot behavior
- active or passive use
- duration and cooldown rules
- audio / VFX hooks
- save / economy flags

Until then, contributors must keep item behavior readable and avoid scattering item logic across unrelated scripts.

---

## Phased Refactor Roadmap

### Phase 1 - Stabilize the Prototype

Goal:
Make the current game loop less fragile without changing the design identity.

Priority work:

- audit all scripts under `Assets/_custom/Scrip`
- remove or quarantine obvious duplicates
- normalize the most dangerous naming inconsistencies
- reduce scene-loading brittleness
- document actual scene responsibilities
- verify player movement, hiding, flashlight, door flow, and inventory do not break during cleanup

### Phase 2 - Split Core Runtime Systems

Goal:
Break the prototype monolith into clear system owners.

Priority work:

- separate player movement from global state
- split inventory from UI slot presentation
- split flashlight state from visual effect handling
- move threat pacing away from direct scene-only scripts
- isolate scene bootstrap from gameplay runtime logic

### Phase 3 - Data-Drive Content

Goal:
Make floors, enemies, items, and encounters authorable without rewriting runtime code every time.

Priority work:

- introduce definition assets for items, enemies, floors, and search encounters
- replace hardcoded balance values where practical
- formalize safe room shop content
- formalize spawn and encounter rules

### Phase 4 - Build the Vertical Slice Properly

Goal:
Turn the descent loop into a production-worthy slice.

Priority work:

- floor progression pacing
- stronger Room Search structure
- proper STALKER design implementation
- safe-room relief polish
- story encounter hooks
- clearer fail-state and recovery-state presentation

### Phase 5 - Expand Content and Port Readiness

Goal:
Scale content while preserving mobile-first usability and preparing PC support cleanly.

Priority work:

- broader floor variety
- more encounter permutations
- economy tuning
- progression persistence
- platform QA for touch and PC input mapping

---

## Final Instruction To Contributors

When working on **Dog**, do not optimize only for clean code and do not optimize only for vibes.

The correct target is:

- **emotionally faithful horror design**
- **mobile-first readable gameplay**
- **data-driven scalable architecture**
- **clear ownership instead of prototype sprawl**

If a choice improves architecture but weakens tension, mystery, or player readability, it is the wrong choice.

If a choice preserves mood but makes the code impossible to scale, it is also the wrong choice.

Build toward both.
