# .NET 10.0 Upgrade Plan - PasswordPurgatory.Web

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [PasswordPurgatory.Web.csproj](#passwordpurgatorywebcsproj)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Overview
This plan outlines the upgrade path for the PasswordPurgatory Blazor web application from **.NET 8.0** to **.NET 10.0 (LTS)**. The solution consists of a single Blazor Server project with minimal complexity and low migration risk.

### Key Metrics
- **Projects**: 1 (PasswordPurgatory.Web)
- **Total Issues**: 5 (2 Mandatory, 1 Potential, 1 Optional, 1 Informational)
- **NuGet Packages**: 2 (1 deprecated, 1 compatible)
- **Code Files**: 18 files (391 lines of code, 3 files with issues)
- **Migration Complexity**: ⚠️ **LOW**
- **Estimated Effort**: 2-4 hours

### Critical Findings
1. **Binary Incompatible API**: `ServiceCollectionExtensions` (used in dependency injection setup)
2. **Behavioral Change**: `UseExceptionHandler` middleware behavior changed
3. **Deprecated Package**: `Microsoft.ApplicationInsights.AspNetCore 2.22.0` needs replacement
4. **Target Framework**: Must update from `net8.0` to `net10.0`

### Migration Approach
**All-at-Once Strategy** — Given the single-project structure and low complexity, we'll upgrade the entire solution in one coordinated operation.

---

## Migration Strategy

### Chosen Strategy: All-at-Once (Single Phase)

**Rationale:**
- Single project with no internal dependencies
- Minimal external package dependencies (2 packages)
- Low code volume (391 LOC)
- No high-risk features detected
- Clean SDK-style project structure

### Execution Phases

#### Phase 1: Pre-Migration Validation
- Verify .NET 10.0 SDK installation
- Review current build and test status
- Create backup/branch (already on `upgrade-to-NET10`)
- Document baseline functionality

#### Phase 2: Core Framework Upgrade
- Update project target framework from `net8.0` to `net10.0`
- Update deprecated NuGet packages
- Resolve API compatibility issues

#### Phase 3: Post-Migration Validation
- Build verification
- Address breaking changes
- Run tests (if available)
- Validate application functionality

### Rollback Plan
- Source branch: `net10upgrades`
- Upgrade branch: `upgrade-to-NET10`
- Rollback command: `git checkout net10upgrades`

---

## Detailed Dependency Analysis

### Project Structure
```
PasswordPurgatory.sln
└── PasswordPurgatory.Web (Blazor Server)
    ├── Target: net8.0 → net10.0
    ├── Type: AspNetCore (SDK-style)
    └── Dependencies: 0 project references
```

**Dependency Depth**: Level 0 (no internal dependencies)

### Package Inventory

| Package Name | Current Version | Status | Action Required |
|-------------|----------------|--------|-----------------|
| **Microsoft.ApplicationInsights.AspNetCore** | 2.22.0 | ⚠️ Deprecated | Replace with 3.0.0+ |
| **Telerik.UI.for.Blazor** | 6.1.0 | ✅ Compatible | Verify/Update to latest |

### Transitive Dependencies
No critical transitive dependency issues detected. The .NET 10.0 framework will automatically resolve updated system packages.

---

## Project-by-Project Plans

### PasswordPurgatory.Web.csproj

**Project Type**: Blazor Server Application  
**Current Framework**: net8.0  
**Target Framework**: net10.0  
**Migration Priority**: N/A (single project)

#### Step 1: Update Target Framework
**File**: `PasswordPurgatory.Web\PasswordPurgatory.Web.csproj`

Change:
```xml
<TargetFramework>net8.0</TargetFramework>
```

To:
```xml
<TargetFramework>net10.0</TargetFramework>
```

#### Step 2: Update NuGet Packages

##### Package: Microsoft.ApplicationInsights.AspNetCore
- **Current**: 2.22.0 (⚠️ Deprecated)
- **Target**: 3.0.0+
- **Impact**: This package was deprecated. Version 3.0.0 is the recommended replacement.
- **Action**: Update package reference in `.csproj`

**Before:**
```xml
<PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.22.0" />
```

**After:**
```xml
<PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="3.0.0" />
```

**Code Impact**: Review usage in `Program.cs`:
- Line 7-10: `AddApplicationInsightsTelemetry()` — API is compatible, but verify configuration options

##### Package: Telerik.UI.for.Blazor
- **Current**: 6.1.0
- **Status**: ✅ Compatible (likely requires update for .NET 10 support)
- **Action**: Check Telerik's documentation for .NET 10 compatible version
- **Recommendation**: Update to latest version supporting .NET 10.0

#### Step 3: Resolve API Breaking Changes

##### 3.1 Binary Incompatible API (Mandatory)
**Issue**: `ServiceCollectionExtensions` type changed  
**File**: `PasswordPurgatory.Web\Program.cs`, Line 6  
**Code**: `builder.Services.AddTelerikBlazor();`

**Analysis**:
- This is a Telerik extension method
- Breaking change likely resolved by updating Telerik package
- No direct code change needed if package is updated

**Action**: 
1. Update Telerik.UI.for.Blazor package
2. Verify extension method still works
3. Check Telerik migration guide for .NET 10

##### 3.2 Behavioral Change (Potential)
**Issue**: `UseExceptionHandler` behavior changed in .NET 10  
**File**: `PasswordPurgatory.Web\Program.cs`, Line 28  
**Code**: `app.UseExceptionHandler("/Error");`

**What Changed**:
- In .NET 10, exception handling middleware may have modified behavior
- Review the [breaking changes documentation](https://go.microsoft.com/fwlink/?linkid=2262679)

**Action**:
1. Test exception handling in development
2. Verify `/Error` page still receives exceptions correctly
3. Check for any new configuration options in .NET 10

**Mitigation**: No immediate code changes required, but thorough testing needed.

#### Step 4: Optional Improvements

##### NuGet Source Mapping (Informational)
**Issue**: Package source mappings recommended  
**File**: `NuGet.config`

**Benefit**: Improves supply chain security when using multiple package sources.

**Action** (Optional):
Add package source mapping to `NuGet.config`:
```xml
<packageSourceMapping>
  <packageSource key="nuget.org">
    <package pattern="Microsoft.*" />
    <package pattern="System.*" />
  </packageSource>
  <packageSource key="telerik">
    <package pattern="Telerik.*" />
  </packageSource>
</packageSourceMapping>
```

#### Step 5: Build & Validate
1. Restore NuGet packages: `dotnet restore`
2. Build project: `dotnet build`
3. Address any compilation errors
4. Run application and test core functionality

---

## Package Update Reference

### Required Package Updates

| Package | Current | Target | Priority | Notes |
|---------|---------|--------|----------|-------|
| Microsoft.ApplicationInsights.AspNetCore | 2.22.0 | 3.0.0 | 🔴 High | Deprecated, must update |
| Telerik.UI.for.Blazor | 6.1.0 | Latest .NET 10 compatible | 🟡 Medium | Verify compatibility, consult vendor |

### Package Update Commands

```powershell
# Update Application Insights
dotnet add package Microsoft.ApplicationInsights.AspNetCore --version 3.0.0

# Update Telerik (check latest version supporting .NET 10)
dotnet add package Telerik.UI.for.Blazor --version [latest-compatible-version]
```

### Compatibility Notes

#### Microsoft.ApplicationInsights.AspNetCore 3.0.0
- **Breaking Changes**: Configuration API changes may exist
- **Verification Required**: 
  - `AddApplicationInsightsTelemetry()` method signature
  - Connection string configuration
- **Documentation**: https://learn.microsoft.com/azure/azure-monitor/app/asp-net-core

#### Telerik.UI.for.Blazor
- **Vendor-Specific**: Check Telerik's release notes
- **Action**: Visit Telerik's website for .NET 10 support matrix
- **Fallback**: If not yet supported, delay upgrade or contact Telerik support

---

## Breaking Changes Catalog

### API Breaking Changes

#### 1. Binary Incompatible API (Mandatory)
**Rule ID**: Api.0001  
**Severity**: 🔴 Mandatory  
**Type**: ServiceCollectionExtensions  

**Location**: `PasswordPurgatory.Web\Program.cs`, Line 6  
**Code**: 
```csharp
builder.Services.AddTelerikBlazor();
```

**Issue Description**:
The `ServiceCollectionExtensions` type has binary-incompatible changes in .NET 10. This affects existing binaries and requires recompilation.

**Resolution Strategy**:
1. Update `Telerik.UI.for.Blazor` package to .NET 10 compatible version
2. Recompile the project after package update
3. If issues persist, check Telerik's migration guide

**References**:
- [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

#### 2. Behavioral Change (Potential)
**Rule ID**: Api.0003  
**Severity**: 🟡 Potential  
**Method**: `UseExceptionHandler(IApplicationBuilder, String)`  

**Location**: `PasswordPurgatory.Web\Program.cs`, Line 28  
**Code**:
```csharp
app.UseExceptionHandler("/Error");
```

**Issue Description**:
The exception handler middleware has behavioral changes in .NET 10. While code compiles without changes, runtime behavior may differ:
- Exception propagation may work differently
- Error page routing behavior may have changed
- Status code handling may be updated

**Resolution Strategy**:
1. **No immediate code change required**
2. Thoroughly test exception scenarios:
   - Unhandled exceptions in Razor pages
   - Exceptions in Blazor components
   - Custom error page rendering
3. Monitor application logs after upgrade
4. Review .NET 10 breaking changes documentation

**Testing Checklist**:
- [ ] Test exception thrown in Razor page
- [ ] Test exception thrown in Blazor component
- [ ] Verify `/Error` page loads correctly
- [ ] Check HTTP status codes on errors
- [ ] Verify error information is properly passed

**Mitigation**: If behavior is undesirable, consider:
- Using the new exception handler options in .NET 10
- Custom exception middleware
- Updating error handling configuration

---

### Package Breaking Changes

#### 3. Deprecated Package (Optional)
**Rule ID**: NuGet.0005  
**Severity**: 🟠 Optional (but recommended)  
**Package**: Microsoft.ApplicationInsights.AspNetCore 2.22.0  

**Location**: `PasswordPurgatory.Web\PasswordPurgatory.Web.csproj`

**Issue Description**:
This version is deprecated. Microsoft recommends upgrading to version 3.0.0+.

**Potential Issues**:
- Security vulnerabilities in old version
- Missing features and improvements
- Possible future incompatibility

**Resolution**:
Update to version 3.0.0:
```xml
<PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="3.0.0" />
```

**Code Review Required**:
Check if API changes affect configuration in `Program.cs`:
```csharp
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
});
```

**References**:
- [Package deprecation documentation](https://go.microsoft.com/fwlink/?linkid=2262531)

---

### Project Configuration Changes

#### 4. Target Framework Update (Mandatory)
**Rule ID**: Project.0002  
**Severity**: 🔴 Mandatory  

**Location**: `PasswordPurgatory.Web\PasswordPurgatory.Web.csproj`

**Change Required**:
```xml
<!-- Before -->
<TargetFramework>net8.0</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**References**:
- [Overview of porting to .NET](https://go.microsoft.com/fwlink/?linkid=2265227)
- [.NET project SDKs](https://go.microsoft.com/fwlink/?linkid=2265226)

---

### Informational Items

#### 5. NuGet Source Mapping Recommendation (Informational)
**Rule ID**: NuGet.0006  
**Severity**: ℹ️ Informational  

**Location**: `NuGet.config`

**Recommendation**:
Implement package source mapping for improved supply chain security.

**Benefits**:
- Deterministic package source selection
- Reduced supply chain attack surface
- Better control over package origins

**Implementation** (Optional):
See "Optional Improvements" in Project-by-Project Plans section.

**References**:
- [Package Source Mapping](https://go.microsoft.com/fwlink/?linkid=2288407)

---

## Risk Management

### Risk Assessment Matrix

| Risk Category | Likelihood | Impact | Severity | Mitigation |
|--------------|------------|--------|----------|------------|
| Build Failures | Low | Medium | 🟡 Low-Medium | Clear error messages, incremental build |
| Telerik Compatibility | Medium | High | 🟠 Medium | Verify vendor support, test UI components |
| Exception Handling Changes | Low | Medium | 🟡 Low-Medium | Comprehensive exception testing |
| Application Insights Breaking Changes | Low | Low | 🟢 Low | Review API docs, test telemetry |
| Runtime Errors | Low | High | 🟡 Low-Medium | Full application testing |

### Detailed Risk Analysis

#### Risk 1: Telerik Package Compatibility
**Description**: Telerik.UI.for.Blazor 6.1.0 may not fully support .NET 10.0

**Indicators**:
- Binary incompatible API detected in Telerik extension method
- .NET 10.0 is relatively new; vendor support may lag

**Mitigation Strategy**:
1. **Pre-upgrade**: Check Telerik's website for .NET 10 compatibility
2. **During upgrade**: Update to latest Telerik version if available
3. **Post-upgrade**: Thoroughly test all Telerik UI components
4. **Fallback**: If not supported, consider:
   - Delay upgrade until Telerik releases compatible version
   - Contact Telerik support for guidance
   - Evaluate alternative UI frameworks (if timeline is critical)

**Probability**: Medium (30-40%)  
**Impact**: High (UI components may not work)  
**Overall Risk**: 🟠 **Medium**

---

#### Risk 2: Exception Handling Behavioral Changes
**Description**: `UseExceptionHandler` behavior changed; error handling may work differently

**Indicators**:
- Behavioral change detected by analysis tool
- Exception middleware is critical for production reliability

**Mitigation Strategy**:
1. **Testing**: Create comprehensive exception test scenarios
2. **Monitoring**: Add logging to exception handler
3. **Validation**: Compare error handling before/after upgrade
4. **Documentation**: Review [.NET 10 breaking changes](https://learn.microsoft.com/dotnet/core/compatibility/10.0)

**Testing Checklist**:
- Razor page exceptions
- Blazor component exceptions
- Error page rendering
- HTTP status codes
- Error details in logs

**Probability**: Low (10-20%)  
**Impact**: Medium (errors may not be handled correctly)  
**Overall Risk**: 🟡 **Low-Medium**

---

#### Risk 3: Application Insights Configuration Changes
**Description**: Upgrading from 2.22.0 to 3.0.0 may introduce API changes

**Indicators**:
- Major version bump (2.x → 3.x) suggests breaking changes
- Telemetry is important for production monitoring

**Mitigation Strategy**:
1. Review [Application Insights migration guide](https://learn.microsoft.com/azure/azure-monitor/app/asp-net-core)
2. Verify connection string configuration still works
3. Test telemetry in development environment
4. Validate metrics in Azure portal after deployment

**Probability**: Low (15%)  
**Impact**: Low (telemetry may not work; doesn't break functionality)  
**Overall Risk**: 🟢 **Low**

---

#### Risk 4: Unknown Runtime Issues
**Description**: Undetected breaking changes may surface at runtime

**Mitigation Strategy**:
1. **Comprehensive Testing**: Test all major application features
2. **Staged Rollout**: Deploy to dev → staging → production
3. **Monitoring**: Watch logs and metrics closely after deployment
4. **Rollback Plan**: Keep `net10upgrades` branch for quick revert

**Probability**: Low (10%)  
**Impact**: High (application may crash or malfunction)  
**Overall Risk**: 🟡 **Low-Medium**

---

### Risk Mitigation Timeline

#### Pre-Upgrade (Before Starting)
- [ ] Verify .NET 10 SDK installed
- [ ] Check Telerik .NET 10 compatibility documentation
- [ ] Review Application Insights 3.0 migration guide
- [ ] Ensure working on correct branch (`upgrade-to-NET10`)
- [ ] Backup current working state

#### During Upgrade
- [ ] Make changes incrementally
- [ ] Build after each change
- [ ] Address compilation errors immediately
- [ ] Don't proceed if critical issues arise

#### Post-Upgrade
- [ ] Full build validation
- [ ] Exception handling tests
- [ ] UI component tests (all Telerik components)
- [ ] Telemetry verification
- [ ] Integration testing
- [ ] Code review of changes

---

## Testing & Validation Strategy

### Test Levels

#### Level 1: Build Validation (Mandatory)
**Objective**: Ensure code compiles without errors

**Steps**:
1. Clean solution: `dotnet clean`
2. Restore packages: `dotnet restore`
3. Build solution: `dotnet build`
4. Verify zero errors, address warnings

**Success Criteria**:
- ✅ Build completes with exit code 0
- ✅ All packages restored successfully
- ✅ No compilation errors

---

#### Level 2: Unit Testing (If Available)
**Objective**: Verify existing tests still pass

**Steps**:
1. Discover test projects: Check if solution contains test projects
2. Run tests: `dotnet test`
3. Review test results

**Success Criteria**:
- ✅ All tests pass
- ✅ No new test failures introduced
- ⚠️ If tests fail, investigate if due to upgrade or existing issues

**Note**: Assessment did not detect test projects, so this may not apply.

---

#### Level 3: Functional Testing (Mandatory)
**Objective**: Verify application works correctly

**Test Scenarios**:

##### A. Application Startup
- [ ] Application starts without exceptions
- [ ] Home page loads correctly
- [ ] No console errors in browser

##### B. Blazor UI Components (Telerik)
- [ ] All pages with Telerik components load
- [ ] Interactive components work (buttons, grids, forms, etc.)
- [ ] Component styling renders correctly
- [ ] No JavaScript errors

##### C. Exception Handling
- [ ] Trigger exception in Razor page
- [ ] Verify error page (`/Error`) displays
- [ ] Check appropriate HTTP status code
- [ ] Verify exception logged (if logging configured)

##### D. Application Insights Telemetry
- [ ] Telemetry initialization succeeds
- [ ] Connection string loaded correctly
- [ ] Events sent to Application Insights (check Azure portal)
- [ ] No telemetry errors in logs

##### E. CORS Configuration
- [ ] CORS policy applied correctly
- [ ] Allowed origins work as expected

##### F. Static Files & Routing
- [ ] Static files serve correctly
- [ ] Routing works for all pages
- [ ] Blazor Hub connection established

---

#### Level 4: Integration Testing (Recommended)
**Objective**: Verify integration points work correctly

**Test Scenarios**:
- [ ] External API calls (if any)
- [ ] Database connectivity (if applicable)
- [ ] Authentication/Authorization (if configured)
- [ ] Third-party service integrations

---

### Testing Checklist Summary

#### Pre-Flight Checks
- [ ] .NET 10 SDK installed and verified
- [ ] Branch `upgrade-to-NET10` checked out
- [ ] Current state documented/committed

#### Build Phase
- [ ] Target framework updated to `net10.0`
- [ ] Application Insights package updated to 3.0.0
- [ ] Telerik package updated (if new version available)
- [ ] `dotnet restore` successful
- [ ] `dotnet build` successful (zero errors)

#### Runtime Phase
- [ ] Application starts without errors
- [ ] All Telerik UI components tested
- [ ] Exception handling validated
- [ ] Application Insights telemetry working
- [ ] No console errors or warnings

#### Final Validation
- [ ] Full application smoke test completed
- [ ] All critical features tested
- [ ] No regressions identified
- [ ] Ready for deployment to higher environments

---

### Test Environment Requirements

**Development Environment**:
- .NET 10.0 SDK
- Visual Studio 2022 17.12+ or VS Code
- Browser for Blazor testing (Chrome, Edge, Firefox)

**Optional**:
- Azure Application Insights instance (for telemetry testing)
- Integration test environment (for full validation)

---

## Complexity & Effort Assessment

### Complexity Rating: ⚠️ **LOW**

#### Factors Contributing to Low Complexity

✅ **Single Project**: Only one project to upgrade (no multi-project coordination)  
✅ **No Internal Dependencies**: No project-to-project references  
✅ **SDK-Style Project**: Modern project format, easier to upgrade  
✅ **Small Codebase**: 391 lines of code across 18 files  
✅ **Minimal Package Dependencies**: Only 2 NuGet packages  
✅ **Standard Blazor Stack**: Well-documented upgrade path  
✅ **No Database Migrations**: No schema changes required  
✅ **No Framework Changes**: Staying within .NET ecosystem  

#### Complexity Indicators

| Factor | Assessment | Weight | Score |
|--------|------------|--------|-------|
| Project Count | 1 project | Low | 1/5 |
| Dependency Depth | 0 levels | Low | 1/5 |
| Code Volume | 391 LOC | Low | 1/5 |
| Package Count | 2 packages | Low | 1/5 |
| Breaking Changes | 2 mandatory, 1 potential | Low | 2/5 |
| External Dependencies | 1 vendor (Telerik) | Medium | 2/5 |
| **Overall Complexity** | | | **1.3/5** |

---

### Effort Estimation

#### Time Breakdown

| Phase | Tasks | Estimated Time |
|-------|-------|----------------|
| **Preparation** | SDK verification, branch setup, review docs | 15-30 min |
| **Core Upgrade** | Update framework, packages, build | 30-45 min |
| **Issue Resolution** | Fix compilation errors, address warnings | 15-30 min |
| **Testing** | Functional testing, exception handling, UI validation | 45-60 min |
| **Documentation** | Update docs, commit messages, notes | 15-30 min |
| **Contingency** | Unexpected issues (Telerik compatibility) | 0-60 min |
| **Total Estimated Time** | | **2-4 hours** |

#### Effort by Role

**Developer**: 2-3 hours
- Framework update
- Package management
- Build troubleshooting
- Basic testing

**QA/Tester**: 1 hour
- Comprehensive functional testing
- Exception scenario testing
- UI component validation

**DevOps** (Optional): 30 min
- CI/CD pipeline updates (if .NET 10 SDK not yet available)
- Deployment configuration

---

### Assumptions

This estimate assumes:
- ✅ .NET 10 SDK is already installed
- ✅ Telerik has .NET 10 compatible version available
- ✅ Developer is familiar with .NET upgrades
- ✅ No major undocumented breaking changes
- ✅ Test environment is readily available

**Risk Factors** that could increase effort:
- ⚠️ Telerik does not yet support .NET 10 (+2-4 hours for workarounds/alternative solutions)
- ⚠️ Undiscovered breaking changes in dependencies (+1-2 hours per issue)
- ⚠️ Application Insights API changes require code updates (+30-60 min)

---

### Confidence Level: 🟢 **HIGH (85%)**

We have high confidence in this estimate because:
- Small, well-understood codebase
- Clear upgrade path documented
- Minimal external dependencies
- Standard technologies with good documentation
- Pre-upgrade analysis completed

---

## Source Control Strategy

### Branch Structure

```
main / master
├── net10upgrades (source branch)
│   └── Current .NET 8.0 code
└── upgrade-to-NET10 (upgrade branch) ← Work happens here
    └── .NET 10.0 migration in progress
```

**Current Status**: ✅ Already on `upgrade-to-NET10` branch

---

### Commit Strategy

#### Recommended Commit Sequence

**Commit 1: Update Target Framework**
```bash
git add PasswordPurgatory.Web/PasswordPurgatory.Web.csproj
git commit -m "chore: Update target framework from net8.0 to net10.0"
```

**Commit 2: Update Deprecated Packages**
```bash
git add PasswordPurgatory.Web/PasswordPurgatory.Web.csproj
git commit -m "chore: Update Microsoft.ApplicationInsights.AspNetCore to 3.0.0"
```

**Commit 3: Update Telerik Package (if needed)**
```bash
git add PasswordPurgatory.Web/PasswordPurgatory.Web.csproj
git commit -m "chore: Update Telerik.UI.for.Blazor to .NET 10 compatible version"
```

**Commit 4: Fix Breaking Changes (if code changes needed)**
```bash
git add PasswordPurgatory.Web/Program.cs
git commit -m "fix: Address .NET 10 breaking changes in exception handling"
```

**Commit 5: Optional - Add NuGet Source Mapping**
```bash
git add NuGet.config
git commit -m "chore: Add package source mappings for improved security"
```

**Final Commit: Build Verification**
```bash
git add .
git commit -m "chore: Verify successful build after .NET 10 upgrade"
```

---

### Pull Request Strategy

#### PR Title
```
[Upgrade] Migrate PasswordPurgatory.Web from .NET 8.0 to .NET 10.0 (LTS)
```

#### PR Description Template
```markdown
## Summary
Upgrades the PasswordPurgatory Blazor web application from .NET 8.0 to .NET 10.0 (LTS).

## Changes
- ✅ Updated target framework: `net8.0` → `net10.0`
- ✅ Updated `Microsoft.ApplicationInsights.AspNetCore`: `2.22.0` → `3.0.0`
- ✅ Updated `Telerik.UI.for.Blazor` (if applicable)
- ✅ Addressed breaking changes in exception handling
- ✅ Build successful with zero errors

## Breaking Changes Addressed
1. **ServiceCollectionExtensions**: Resolved by updating Telerik package
2. **UseExceptionHandler**: Tested exception scenarios, behavior validated
3. **Application Insights**: Updated to non-deprecated version 3.0.0

## Testing Completed
- [x] Build validation (clean, restore, build)
- [x] Application startup
- [x] Telerik UI components functional
- [x] Exception handling works correctly
- [x] Application Insights telemetry operational
- [x] No console errors or warnings

## Risks & Mitigations
- **Telerik Compatibility**: Verified .NET 10 support, all components tested
- **Exception Handling Changes**: Comprehensive testing completed
- **Application Insights**: Telemetry validated in dev environment

## Rollback Plan
Source branch `net10upgrades` preserved for rollback if needed.

## References
- Assessment: `.github/upgrades/scenarios/new-dotnet-version_285252/assessment.md`
- Plan: `.github/upgrades/scenarios/new-dotnet-version_285252/plan.md`
- [.NET 10 Breaking Changes](https://learn.microsoft.com/dotnet/core/compatibility/10.0)
```

---

### Merge Strategy

**Recommended Approach**: Squash and Merge

**Rationale**:
- Clean history on main branch
- Single commit represents entire upgrade
- Easier to revert if issues arise

**Alternative**: Standard Merge (if you want to preserve detailed commit history)

---

### Post-Merge Actions

1. **Tag the release**:
   ```bash
   git tag -a v1.0-net10.0 -m "Upgraded to .NET 10.0 LTS"
   git push origin v1.0-net10.0
   ```

2. **Update CI/CD pipelines**:
   - Ensure .NET 10 SDK available in build agents
   - Update build scripts if necessary

3. **Deploy to staging**:
   - Test in staging environment before production
   - Monitor for runtime issues

4. **Update documentation**:
   - README.md (update .NET version requirement)
   - Development setup guide
   - Deployment documentation

---

### Rollback Procedure

If critical issues arise after merge:

```bash
# Option 1: Revert the merge commit
git revert -m 1 <merge-commit-hash>
git push origin main

# Option 2: Reset to previous state (if not yet public)
git reset --hard <commit-before-merge>
git push --force origin main

# Option 3: Deploy from backup branch
git checkout net10upgrades
# Deploy from this branch
```

---

## Success Criteria

### Critical Success Criteria (Must Pass)

#### 1. Build Success ✅
- [ ] `dotnet clean` completes successfully
- [ ] `dotnet restore` completes with no errors
- [ ] `dotnet build` completes with **zero errors**
- [ ] All warnings reviewed and acceptable

**Validation Command**:
```bash
dotnet build --configuration Release
```

**Expected Output**: `Build succeeded. 0 Error(s)`

---

#### 2. Target Framework Updated ✅
- [ ] Project file contains `<TargetFramework>net10.0</TargetFramework>`
- [ ] No references to `net8.0` remain in project files

**Validation**:
```bash
# Check project file
grep "TargetFramework" PasswordPurgatory.Web/PasswordPurgatory.Web.csproj
```

**Expected Output**: `<TargetFramework>net10.0</TargetFramework>`

---

#### 3. Application Starts Successfully ✅
- [ ] Application launches without exceptions
- [ ] Home page loads within 5 seconds
- [ ] No critical errors in console or logs

**Validation**:
```bash
dotnet run --project PasswordPurgatory.Web
# Navigate to https://localhost:<port>
```

**Expected**: Application accessible in browser

---

#### 4. Deprecated Packages Resolved ✅
- [ ] `Microsoft.ApplicationInsights.AspNetCore` updated to 3.0.0+
- [ ] No deprecated package warnings in build output

**Validation**:
```bash
dotnet list package --deprecated
```

**Expected Output**: No deprecated packages listed

---

### Functional Success Criteria (Should Pass)

#### 5. UI Components Work ✅
- [ ] All Telerik Blazor components render correctly
- [ ] Interactive components respond to user input
- [ ] No JavaScript errors in browser console
- [ ] Page navigation works correctly

**Validation**: Manual testing of all pages with Telerik components

---

#### 6. Exception Handling Validated ✅
- [ ] Unhandled exceptions redirect to `/Error` page
- [ ] Error page displays correctly
- [ ] Appropriate HTTP status codes returned
- [ ] Exceptions logged (if logging configured)

**Validation**: Trigger test exception and verify behavior

---

#### 7. Application Insights Operational ✅
- [ ] Telemetry initialization succeeds
- [ ] Connection string loaded correctly
- [ ] Events visible in Azure Application Insights portal
- [ ] No telemetry errors in application logs

**Validation**: Check Application Insights portal for incoming telemetry

---

#### 8. No Regressions ✅
- [ ] Existing functionality works as before upgrade
- [ ] No new bugs introduced
- [ ] Performance is equal or better

**Validation**: Full smoke test of application features

---

### Documentation Success Criteria (Recommended)

#### 9. Documentation Updated 📝
- [ ] README reflects .NET 10.0 requirement
- [ ] Migration notes added to documentation
- [ ] Known issues documented (if any)
- [ ] Deployment guide updated

---

#### 10. Code Quality Maintained ✅
- [ ] No new compiler warnings introduced
- [ ] Code analysis rules still pass
- [ ] No security warnings

**Validation**:
```bash
dotnet build /p:TreatWarningsAsErrors=true
```

---

### Optional Success Criteria

#### 11. NuGet Source Mapping Implemented ℹ️
- [ ] `NuGet.config` includes source mappings
- [ ] Package sources properly mapped
- [ ] Restore works correctly with mappings

**Validation**:
```bash
dotnet restore --verbosity detailed
```

---

### Final Go/No-Go Checklist

Before merging to main branch:

**Go Criteria** (All must be ✅):
- [ ] All **Critical Success Criteria** passed
- [ ] At least 90% of **Functional Success Criteria** passed
- [ ] No blocking issues identified
- [ ] Code review completed and approved
- [ ] Staging environment tested successfully

**No-Go Criteria** (Any means do not merge):
- [ ] ❌ Build fails
- [ ] ❌ Application crashes on startup
- [ ] ❌ Critical functionality broken
- [ ] ❌ Telerik components completely non-functional
- [ ] ❌ Unresolved security vulnerabilities

---

### Post-Deployment Validation (Production)

Within 24 hours of production deployment:

- [ ] Monitor application logs for errors
- [ ] Check Application Insights for anomalies
- [ ] Verify user traffic and engagement metrics
- [ ] Review error rates compared to pre-upgrade baseline
- [ ] Validate performance metrics (response times, throughput)
- [ ] Confirm no increase in support tickets

**Rollback Trigger**: If any critical metric degrades by >20%, initiate rollback procedure.

---

## Conclusion

This migration plan provides a structured, low-risk path to upgrade PasswordPurgatory.Web from .NET 8.0 to .NET 10.0 (LTS). The single-project structure and minimal dependencies make this a straightforward upgrade with an estimated effort of 2-4 hours.

### Key Takeaways

✅ **Low Complexity**: Single project, small codebase, minimal dependencies  
⚠️ **Primary Risk**: Telerik package compatibility (monitor vendor support)  
🎯 **Success Factors**: Thorough testing, especially UI components and exception handling  
📋 **Execution**: Follow commit strategy for clean history and easy rollback  

### Next Steps

1. **Review this plan** with the development team
2. **Verify Telerik .NET 10 compatibility** on their website
3. **Ensure .NET 10 SDK installed** on development machines
4. **Proceed to execution phase** following the project-by-project plan
5. **Track progress** using the tasks markdown (generated after approval)

**Ready to Execute?** Proceed with confidence knowing risks are identified and mitigated! 🚀
