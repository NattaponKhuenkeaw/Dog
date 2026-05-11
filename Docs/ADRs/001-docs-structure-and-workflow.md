# ADR-001: Documentation Structure and AI-Assisted Workflow

| Field       | Value                    |
|-------------|--------------------------|
| Status      | **Accepted**             |
| Date        | 2026-05-11               |
| Author      | Apricha + AI Assistant   |
| GDD Section | N/A (process decision)   |

## Context

The project relies heavily on AI coding assistants (multiple IDE chatbots used simultaneously). Currently there is no structured workflow — all context is duplicated manually across chatbot sessions, commits are done from memory, and there is no QA, code review, or PM reporting process.

The existing GDD lives inside `Assets/Core/Doc/` as a Unity-tracked asset, which works but mixes documentation with game assets.

## Decision

1. Create a `Docs/` folder at the **repo root** (outside Unity Assets) for all project documentation.
2. Structure it into four areas: **GDD + AI Context**, **ADRs**, **DevLog**, and **Reports**.
3. Use **ADRs** (Architecture Decision Records) to track every significant technical decision.
4. Use **DevLog** entries as the source-of-truth for commit messages and PM reports.
5. Use **AI Context** files as pre-built snippets that can be copy-pasted into any AI chatbot to give it project-specific context without re-explaining everything.
6. Keep the original GDD inside `Assets/Core/Doc/` as a symlink/reference since Unity may reference it, but the canonical copy lives in `Docs/GDD/`.

## Alternatives Considered

| Option | Pros | Cons |
|--------|------|------|
| Keep docs in Assets/Core/Doc | Unity tracks it, single location | Mixes with game assets, no structure |
| Use Notion/Confluence | Pretty UI, collaboration | Not version-controlled, context-switching |
| Wiki in GitHub | Built-in, web-accessible | Separate repo, can drift from code |

## Consequences

### Positive
- Every AI session can be bootstrapped with the same context files
- Architecture decisions are traceable and searchable
- Commit messages become consistent (generated from DevLog)
- PM reports can be auto-generated from DevLog + git log

### Negative / Trade-offs
- Must remember to write DevLog entries (but this replaces "remembering" commit messages)
- Two GDD locations until the Assets copy is deprecated
