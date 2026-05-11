# Dog 🐕 — Project Documentation Hub

All documentation lives here, **outside Unity Assets**, version-controlled alongside the codebase.

## Folder Structure

```
Docs/
├── GDD/                    # Game Design Document + AI context files
│   ├── GDD.md              # Master GDD (migrated from Assets/Core/Doc)
│   └── AI-Context/         # Pre-built context snippets for AI agents
│       ├── project-stack.md
│       └── system-*.md     # Per-system context files
│
├── ADRs/                   # Architecture Decision Records
│   ├── _TEMPLATE.md        # Copy this to create new ADRs
│   └── 001-*.md            # Numbered decisions
│
├── DevLog/                 # Git-ready development logs
│   ├── _TEMPLATE.md        # Copy this for each session
│   └── YYYY-MM-DD-*.md    # One per work session
│
└── Reports/                # Auto-generated PM reports (future)
```

## How to Use

1. **Before coding** → Read the relevant `AI-Context/` file + GDD section
2. **Before deciding architecture** → Check existing ADRs, write a new one if needed
3. **After each session** → Write a DevLog entry, use it for your commit message
4. **Weekly** → DevLogs feed into PM reports
