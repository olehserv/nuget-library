# Nuget Library packages Repo project by [Oleh Shevtsiv](https://www.linkedin.com/in/olegshevtsiv/)

> [!TIP]
> 🖐️ *This repo inspired by first version: https://github.com/olehserv/oleh-shevtsiv-pet*

## 📌 Overview

> ⚠️ **Disclaimer**  
> This project was created as a **technical assignment for a job application**
> and is intended for demonstration purposes only.

This Repo consists of: 
1) Basic CI workflows
    - Check [repo files existance, .Net Rebase/Build/Test on push commits](.github/workflows/ci.yml)
    - [PRs title check](.github/workflows/pr-title-check.yml) if contains Semantic Prefixes
    - [Create new tag](.github/workflows/release.yml) on each merge to `main` branch according to semantic release <sub>(TODO: backward patches policy)</sub>
    - [Pack pachages and Push to Nuget server](.github/workflows/publish-nuget-org.yml) after new tag created by workflow.
2) .Net solution:
    - [`.slnx`](Library.slnx) solution file (new simplest one instead of old bulky `.sln`)
    - [Directory.Packages.props](Directory.Packages.props) for centralyzed packages versions managing
    - [global.json](global.json) to define wich .Net version required to be used
    - [src](src) - folder with main source code and [Directory.Build.props](src/Directory.Build.props) file for centralyzed MSBuild configs (applies to all `.csproj` files in all nested directories)
    - [test](test) - folder with unit tests and [Directory.Build.props](src/Directory.Build.props) file.
3) Git settings files: [.gitattributes](.gitattributes) for repo files settings; [.gitignore](.gitignore) for ignoring files and folders when pushing.
4) License, Contribution rules, Security policies.

---

## 🛠 Tech Stack
- .Net 10
- Github Actions
- Nuget
- XUnit
- FluentAssertion
- Moq
- bash

---

## 🧱 Architecture & Design
- **File Management**
    - Source abstraction & Physical source implementation
    - File Formats abstraction & Xml format processor
- **Models collectio**
    - Book model
- **BL: Management**
    - Book Service for managing Books data 
        - *load from file*
        - *add to collection*
        - *sort by author asc than by title asc*
        - *find by title part*
        - *save in file*

## 📂 Project Structure

```text
src/
 ├── Library.File.Core  
 ├── Library.File.Format.Xml 
 ├── Library.File.Source.Physical 
 ├── LLibrary.Management             
 └── Library.Models              
test/
 ├── test.Common 
 ├── test.Library.File.Core
 ├── test.Library.File.Format.Xml
 ├── test.Library.File.Source.Physical           
 └── test.Library.Management      
```

---

## 📦 Huget packages
| Package | Version | Downloads |
|--------|---------|-----------|
| `Library.File.Core` | [![NuGet](https://img.shields.io/nuget/v/olehserv.fulcrum-software-test.Library.File.Core.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.File.Core/) | [![NuGet](https://img.shields.io/nuget/dt/olehserv.fulcrum-software-test.Library.File.Core.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.File.Core/) |
| `Library.File.Format.Xml` | [![NuGet](https://img.shields.io/nuget/v/olehserv.fulcrum-software-test.Library.File.Format.Xml.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.File.Format.Xml/) | [![NuGet](https://img.shields.io/nuget/dt/olehserv.fulcrum-software-test.Library.File.Format.Xml.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.File.Format.Xml/) |
| `Library.File.Source.Physical` | [![NuGet](https://img.shields.io/nuget/v/olehserv.fulcrum-software-test.Library.File.Source.Physical.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.File.Source.Physical/) | [![NuGet](https://img.shields.io/nuget/dt/olehserv.fulcrum-software-test.Library.File.Source.Physical.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.File.Source.Physical/) |
| `Library.Management` | [![NuGet](https://img.shields.io/nuget/v/olehserv.fulcrum-software-test.Library.Management.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.Management/) | [![NuGet](https://img.shields.io/nuget/dt/olehserv.fulcrum-software-test.Library.Management.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.Management/) |
| `Library.Models` | [![NuGet](https://img.shields.io/nuget/v/olehserv.fulcrum-software-test.Library.Models.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.Models/) | [![NuGet](https://img.shields.io/nuget/dt/olehserv.fulcrum-software-test.Library.Models.svg)](https://www.nuget.org/packages/olehserv.fulcrum-software-test.Library.Models/) |


---

## 🚀 Getting Started

### 🧰 Prerequisites
- .Net 10

### 🧪 Testing
```bash
dotnet test {path to slnx file}
```

---

## 🤝 Contributing
[**Follow**](./CONTRIBUTING.md) for more details.

---

## 📄 License
This project is licensed under the **MIT License**.  
See the [LICENSE](LICENSE) file for details.

---

### TODO:
- Add dry run for semantic release on pull request creation
- Add dry run for packing nugets on pull request
- Backward patches policy
- Fix serialization to make it more versatile
