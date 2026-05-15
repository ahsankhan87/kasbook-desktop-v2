# Copilot Instructions

## Project Snapshot
- This is a **.NET Framework 4.8 WinForms** ERP/POS solution.
- Main app: `pos` (startup in `pos/Program.cs` → `Login` → `frm_main`).
- Layered projects:
  - `POS.Core`: shared models/state (`*Modal` classes, `UsersModal` session context).
  - `POS.BLL`: business orchestration (`*BLL` classes).
  - `POS.DLL`: data access (`*DLL` classes, SQL/stored-procedure calls, `dbConnection`).
  - `BenchmarkSuite1`: BenchmarkDotNet perf harness for UI/theme startup scenarios.

## Architecture & Data Flow (important)
- Standard flow for features: **Form (`pos`) → BLL → DLL → SQL Server**.
- Most data retrieval/mutations are via `DataTable` and stored procedures (e.g., `sp_ProductsCrud`, `sp_LogAction`).
- Authentication/session data is global in `POS.Core/POS/UsersModal.cs` and consumed across layers.
- Security is runtime/DB-backed:
  - Context: `pos/Security/Authorization/AppSecurityContext.cs`
  - Role/claim persistence: `SqlRoleRepository`
  - UI enforcement: `FormSecurityExtensions.ApplyPermissions` using `Tag` permission keys.

## UI/UX Conventions (use these, avoid ad-hoc variants)
- Centralize styling through `pos/UI/AppTheme.cs`; call `AppTheme.Apply(this)` in form load.
- For list/grid screens, follow existing helpers like `AppTheme.ApplyListFormStyle(...)` / `ApplyListFormStyleLightHeader(...)`.
- Use `pos/UI/UiMessages.cs` for bilingual messages (EN/AR) instead of raw `MessageBox.Show` in new/updated screens.
- Use `pos/UI/Busy/BusyScope.cs` (`using (BusyScope.Show(...))`) for long-running form actions.
- Use dedicated lock screen form `pos/frm_session_lock.*` (do not build inline lock UI in `frm_main`).
- Keep label text color/theme aligned with `AppTheme.TextPrimary` (dark text).
- Create designer page when creating winform.

## Security, Logging, and Session Rules
- Do not bypass authorization checks; permission gating is expected through `Tag` + `ApplyPermissions`.
- Preserve audit logging patterns using `POS.DLL.Log.LogAction(...)` for major user actions.
- `frm_main` contains inactivity lock/logout and runtime subscription checks—changes here can affect global session behavior.
- Implement discount limits for users on the sales page, enforcing per-user limits (e.g., User A max 10% or fixed 100). This is a security/business rule enforcement feature similar to sales amount limits already in the system.

## Reporting/Integration Notes
- Reporting is mixed: Crystal Reports forms (`Reports/*`, `.rpt` generated classes) and newer shared report helpers (`Reports/Common/BaseReportForm.cs`).
- External integrations include ZATCA SDK assemblies referenced from local path (`..\zatca dlls\...`) and QR/crypto libs.
- Database connection comes from `pos/App.config` connection string `cn` via `POS.DLL/dbConnection.cs`.

## Build/Test/Perf Workflows
- Build app project: `msbuild pos\POS.csproj /t:Build /p:Configuration=Debug`
- Build business/data layer projects similarly (`POS.BLL`, `POS.DLL`, `POS.Core`).
- There are no standard unit test projects currently; use `BenchmarkSuite1` for performance verification:
  - Build: `msbuild BenchmarkSuite1\BenchmarkSuite1.csproj /t:Build /p:Configuration=Debug`
  - Run benchmarks from output executable.

## Code Change Guidance for Agents
- Prefer minimal, localized edits; keep existing naming (`frm_*`, `*BLL`, `*DLL`, `*Modal`) and WinForms partial/designer structure intact.
- Follow existing layer boundaries: avoid putting SQL logic directly in forms.
- When modernizing a legacy form, align to `AppTheme`, `UiMessages`, `BusyScope`, and permission-tag conventions already used in newer forms.