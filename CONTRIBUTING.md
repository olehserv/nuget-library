# Contributing

Thank you for your interest in contributing to this repository! This document describes the development workflow, branching strategy, and rules that help keep the project stable and releases predictable.

> [!NOTE]
> This repository is intended as a technical test / demo project. Contributions should stay clean, minimal, and easy to review.

---

## 🔂 Development Workflow

### 1. All regular development Pull Requests must target the `main` branch

- Feature work, fixes, refactoring, and documentation updates should be merged directly into `main`
- Typical branch examples:
  - `feature/add-authentication`
  - `fix/null-reference`
  - `chore/update-ci`
  - `docs/update-readme`

---

### 2. Release process

Once PR merged into `main` a new tag is automatically created depending on commit prefix.
After that a new changes are packing to be ready to push on Nuget Server.
Once new tag created, a package pushes onto Nuget Server.

---

## ‼️ Pull Request Requirements

### 📝 Conventional Commits (PR title)

Pull Request titles **must follow the [Conventional Commits specification](https://www.conventionalcommits.org/en/v1.0.0/)**. This is required for automated versioning and CI checks.

Allowed prefixes:

- `fix:` — bug fix (patch)
- `feat:` — new feature (minor)
- `feat!:` — breaking change (major)
- `chore:` — maintenance / configuration (unversioned)
- `docs:` — documentation only (unversioned)
- `test:` — unit tests (unversioned)

Examples:

- `fix: handle null input`
- `feat: add docker support`
- `feat!: change authentication flow`
- `chore: configure CI workflow`
- `docs: update README`
- `test: add unit tests`

---

## ☑️ PR Checklist

Before opening a Pull Request, make sure that:

- PR title follows Conventional Commits
- The project builds successfully
- Tests pass (if applicable)
- Documentation is updated when behavior or API changes

---

## 👍 Code Quality Guidelines

- Keep changes small and focused
- Avoid unrelated refactoring in the same PR
- Do not commit temporary files, secrets, or debug artifacts
- If behavior or public API changes, update the documentation accordingly

---

## 🔒 Security

Do **not** commit secrets (API keys, tokens, credentials).
If something sensitive is accidentally committed:
1. Revoke the secret immediately
2. Remove it from the repository history

---

## Need Help?

If anything is unclear:
- Open an Issue, or
- Ask questions directly in your Pull Request

🙏 We appreciate your contribution!
