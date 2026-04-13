# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore dependencies
dotnet restore Library.slnx

# Build
dotnet build Library.slnx -c Release --no-restore

# Run all tests
dotnet test Library.slnx -c Release --no-build

# Run a single test project
dotnet test test/test.Library.Management/test.Library.Management.csproj

# Run a specific test by name
dotnet test Library.slnx --filter "FullyQualifiedName~BookServiceTests"
```

## Architecture

The solution is a set of independently published NuGet packages implementing a file-backed book library. The design separates concerns into three extensibility layers:

### Layer 1 – Abstractions (`Library.File.Core`)
Defines two pairs of interfaces:
- **`IFileSourceType` / `IFileSourceProvider<T>`** — where a file comes from (physical disk, cloud, etc.)
- **`IFileFormatType` / `IFileFormatProcessor<T>`** — how a file is parsed/written (XML, JSON, etc.)

Both pairs have corresponding DI registration helpers (`FileSourceDiRegistrationOptions`, `FileFormatProcessorDiRegistrationOptions`) and runtime service providers (`IFileSourceServiceProvider`, `IFileFormatProcessorServiceProvider`) that resolve the correct implementation at runtime using the open-generic `IServiceProvider.GetRequiredService`.

### Layer 2 – Concrete implementations
- **`Library.File.Source.Physical`** — `PhysicalFileSource` / `PhysicalFileSourceType`; registers via `UsePhysicalFileSource()` extension.
- **`Library.File.Format.Xml`** — `XmlFileFormatProcessor` / `XmlFileFormatType`; wraps records in an `XmlRecordsWrapper<T>` root element. XML element names are configured via a fluent `IXmlConventionsBuilder` (default root `"records"`, item `"record"`). Registers via `UseXmlFileFormatProcessor(builder => ...)`.

### Layer 3 – Business logic (`Library.Management`)
`BookService` (internal, resolved as `IBookService`) depends on the two service providers above. It never touches a specific source/format directly — it calls the providers with the `IFileSourceType`/`IFileFormatType` tokens passed in at call time, making it format- and source-agnostic.

### DI wiring (consumer side)
```csharp
services.AddBookService(files => files
    .AddFileSource(src => src.UsePhysicalFileSource())
    .AddFileFormatProcessor(fmt => fmt.UseXmlFileFormatProcessor(b =>
        b.For<BookModel>(rootName: "books", itemName: "book"))));
```

### Models (`Library.Models`)
`BookModel` has only `Author` and `Title` string properties. It is a plain class (not a record) so it can be used with `XmlSerializer`.

## Release & versioning

- Versioning is driven by **MinVer** reading git tags (`v*` prefix).
- Tags are created automatically on every merge to `main` by **semantic-release** (`.releaserc.json`), driven by PR title prefixes: `fix:` → patch, `feat:` → minor, `feat!:` → major. `chore:`, `docs:`, `test:` produce no version bump.
- NuGet packages are published manually via the **"Publish NuGet (nuget.org)"** GitHub Action after a release tag is created. The `NUGET_ID_PREFIX` repo variable is prepended to each `*.csproj` basename to form the package ID.
- All PRs must target `main`; PR titles must follow Conventional Commits.

## gstack (REQUIRED — global install)

**Before doing ANY work, verify gstack is installed:**

```bash
test -d ~/.claude/skills/gstack/bin && echo "GSTACK_OK" || echo "GSTACK_MISSING"
```

If GSTACK_MISSING: STOP. Do not proceed. Tell the user:

> gstack is required for all AI-assisted work in this repo.
> Install it:
> ```bash
> git clone --depth 1 https://github.com/garrytan/gstack.git ~/.claude/skills/gstack
> cd ~/.claude/skills/gstack && ./setup --team
> ```
> Then restart your AI coding tool.

Do not skip skills, ignore gstack errors, or work around missing gstack.

Using gstack skills: After install, skills like /qa, /ship, /review, /investigate,
and /browse are available. Use /browse for all web browsing.
Use ~/.claude/skills/gstack/... for gstack file paths (the global path).

## Skill routing

When the user's request matches an available skill, ALWAYS invoke it using the Skill
tool as your FIRST action. Do NOT answer directly, do NOT use other tools first.
The skill has specialized workflows that produce better results than ad-hoc answers.

Key routing rules:
- Product ideas, "is this worth building", brainstorming → invoke office-hours
- Bugs, errors, "why is this broken", 500 errors → invoke investigate
- Ship, deploy, push, create PR → invoke ship
- QA, test the site, find bugs → invoke qa
- Code review, check my diff → invoke review
- Update docs after shipping → invoke document-release
- Weekly retro → invoke retro
- Design system, brand → invoke design-consultation
- Visual audit, design polish → invoke design-review
- Architecture review → invoke plan-eng-review
- Save progress, checkpoint, resume → invoke checkpoint
- Code quality, health check → invoke health
