# Conventional Commits Guide for Dog 🐕

## Format

```
type(scope): short description (imperative mood, lowercase)

[optional body — explain WHY, not WHAT]

[optional footer — breaking changes, issue refs]
```

## Types

| Type       | When to use                                      | Example |
|------------|--------------------------------------------------|---------|
| `feat`     | New feature or behavior for the player           | `feat(enemy): add Ghast hallway rush behavior` |
| `fix`      | Bug fix                                          | `fix(inventory): prevent pickup when slots full` |
| `refactor` | Code change that doesn't add features or fix bugs | `refactor(player): extract stamina into own component` |
| `docs`     | Documentation only                               | `docs: add ADR-002 for inventory refactor` |
| `style`    | Formatting, naming fixes, no logic change        | `style(flashlight): rename Flaslight → Flashlight` |
| `test`     | Adding or fixing tests                           | `test(room-search): add spawn distribution test` |
| `chore`    | Build, CI, dependencies, tooling                 | `chore: update URP to 17.0.4` |
| `juice`    | Game feel / polish / VFX / SFX only              | `juice(player): add screen shake on damage` |

## Scopes (use what fits)

`player` · `inventory` · `enemy` · `flashlight` · `room-search` · `ui` · `economy` · `scene` · `audio` · `build` · `docs`

## Examples

```
feat(enemy): implement STALKER shadow phase before chase

The STALKER now follows silently for 3-5 seconds with audio cues
before entering chase mode, matching the GDD design intent.

Ref: GDD Section "STALKER Design Intent"
```

```
fix(flashlight): prevent drain while paused

Flashlight was losing battery during pause menu.
This broke the tension loop by punishing menu usage.
```

```
juice(player): add heartbeat audio when near STALKER

- 3 intensity levels based on distance
- BPM increases: 60 → 90 → 120
- Fades in/out smoothly
```

## Branch Naming

- Create feature/update branches with the `neo_` prefix.
- Format: `neo_<short-description>`
- Example: `neo_refactoring`

## Quick Rule

> Before committing, check your DevLog entry in `Docs/DevLog/`.
> The commit message should be a condensed version of what you wrote there.
