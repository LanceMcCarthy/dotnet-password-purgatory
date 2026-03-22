# PasswordPurgatory.Web .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the PasswordPurgatory Blazor web application upgrade from .NET 8.0 to .NET 10.0 (LTS). The upgrade follows an all-at-once strategy with all components upgraded simultaneously in a single atomic operation.

**Progress**: 1/2 tasks complete (50%) ![0%](https://progress-bar.xyz/50)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-03-22 22:07)*
**References**: Plan §Phase 1 Pre-Migration Validation

- [✓] (1) Verify .NET 10.0 SDK installed per Plan §Execution Phases Phase 1
- [✓] (2) .NET 10.0 SDK meets minimum requirements (**Verify**)

---

### [▶] TASK-002: Atomic framework and package upgrade
**References**: Plan §Phase 2 Core Framework Upgrade, Plan §Project-by-Project Plans, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [▶] (1) Update TargetFramework from net8.0 to net10.0 in PasswordPurgatory.Web\PasswordPurgatory.Web.csproj per Plan §Step 1
- [ ] (2) TargetFramework updated to net10.0 (**Verify**)
- [ ] (3) Update Microsoft.ApplicationInsights.AspNetCore from 2.22.0 to 3.0.0 in PasswordPurgatory.Web.csproj per Plan §Step 2
- [ ] (4) Update Telerik.UI.for.Blazor to latest .NET 10 compatible version in PasswordPurgatory.Web.csproj per Plan §Step 2
- [ ] (5) All package references updated (**Verify**)
- [ ] (6) Restore all dependencies
- [ ] (7) All dependencies restored successfully (**Verify**)
- [ ] (8) Build solution and fix all compilation errors per Plan §Breaking Changes Catalog (focus: ServiceCollectionExtensions API compatibility from Telerik package update, UseExceptionHandler behavioral changes)
- [ ] (9) Solution builds with 0 errors (**Verify**)
- [ ] (10) Commit changes with message: "TASK-002: Complete .NET 10.0 atomic framework and package upgrade"

---


