# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

COBIM Explorer (국토교통부 COBIM 탐색기) is a Windows WPF desktop client (.NET Framework 4.8, C# 9) for the COBIM CDE web service (`https://cobim.io/api/...`). Users log in with a tenant ID, browse projects → teams → folders → files/containers in a lazy-loaded tree, download, check out, check in/upload, and open BIM360 HubBox web-viewer/translate results through an embedded CefSharp browser.

Code comments, log messages and UI strings are in Korean. Follow the existing style: `#region` blocks, `// TODO : ... (YYYY.MM.DD minjae)` comments, and a `try/catch` in each method that logs `Logger.GetMethodPath(MethodBase.GetCurrentMethod()) + Logger.errorMessage + ex.Message` through Serilog and then rethrows (lower layers) or shows `MessageBox.Show(ex.Message, CobimHelper.ErrorTitle)` (view models).

## Build / run

These are old-style (non-SDK) `.csproj` projects that use `packages.config`. NuGet packages restore into `packages\` next to `CobimExplorer.sln`. Build with Visual Studio 2019+ or MSBuild from a Developer prompt:

```
nuget restore CobimExplorer.sln
msbuild CobimExplorer.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

- Startup project: `CobimExplorer` (WinExe). It has `x64` configurations; CefSharp needs a matching platform at runtime.
- Release builds start AppCenter analytics/crash reporting and swallow unhandled dispatcher exceptions (`StyletBootstrapper.OnUnhandledException`). Debug builds do neither.
- Serilog logs go under `Documents\COBIM\CobimExplorer\Logs` (see `CobimHelper.LogDirectoryPath`).
- **Adding files:** the csproj files list every source file explicitly (`<Compile Include>` / `<Page Include>`), so a new `.cs` or `.xaml` file must also be added to its `.csproj`. A file on disk that is missing from the csproj is not compiled. For example, `ViewModels/Page/ExplorerVM.cs` and `Views/Page/ExplorerV.xaml` are stale copies. The live explorer is `ViewModels/Page/Project/CDE/Explorer/ExplorerVM.cs`.

There is no working test suite. `CobimExplorer.TestDataMessenger` (xunit refs) and `CobimExplorer.Core.Rest.Api` (an ASP.NET Core 3.1 MVC template) are in the repo but **not in the solution**. The `Test*VM`/`Test*V` classes in the app are prototype screens, not tests.

## Solution layout (dependency order)

- `CobimNet`: MVVM primitives (`BindableBase`, `INotifyPropertyChanged` extensions).
- `CobimUtil`: shared Serilog `Logger` helpers and string/util helpers used by the non-UI projects.
- `CobimExplorer.Data.Models` → CobimNet: request/response/state models shared by UI and REST layers.
- `CobimExplorer.Rest.Api` → Data.Models, CobimNet, CobimUtil: all HTTP calls to cobim.io.
- `CobimExplorer` → all of the above: the WPF app (Stylet).

## Architecture

### Stylet MVVM bootstrapping
`App.xaml` loads `StyletBootstrapper` (root VM = `MainVM`). In `StyletBootstrapper`:
- `ConfigureIoC` binds VMs and views by hand. Views are mostly **singletons**, and `IPage` is bound to all implementations. **A new page VM/view must be registered here.**
- `Configure` sets the ViewManager convention: `XxxVM` ↔ `XxxV`, namespace `CobimExplorer.ViewModels.*` ↔ `CobimExplorer.Views.*`. Keep that mirrored folder structure and the `VM`/`V` suffixes.

### Base classes (`CobimExplorer/Services`)
- `CobimScreen` / `CobimScreen<TData>` derives from `Conductor<IPage>.Collection.OneActive`. It injects `IWindowManager` and `IEventAggregator`, calls `Container.BuildUp(this)` so `[Inject]` properties work, and subscribes to and unsubscribes from the event aggregator on activate/close. `TData` becomes `InputParamData`, the VM's state/model object.
- `PageBase<TData>` extends `CobimScreen` with `ITab` (tab name, `CloseCommand`/`CloseRequested`). Page VMs derive from this.
- `[HiddenVm]` marks abstract bases.

### Navigation flow
`MainVM.OnInitialActivate` first shows the modal `LoginVM` window (`ToShowData()`). If the user closes it without logging in, the app exits. Pages open through the event aggregator: `TabParentPageVM` (ribbon/tab host) publishes an `OpenPageMsg` (`ViewModels/Message`), and `MainVM.Handle(OpenPageMsg)` calls `ActivateItem(page)`. VM/tab name constants (`ExplorerVMName`, `HubBoxVMName`, ...) live in `Common/CobimBase/CobimHelper.cs` together with paths, titles and other app-wide constants.

### Data model pattern (`CobimExplorer.Data.Models/CobimBase/<Feature>/`)
Each feature (Login, Main, Explorer, HubBox) has a pair: `TXxxView` (base, derives `CDEModelBase<T>` → `BindableBase`) and `XxxView : TXxxView<XxxView>`. The `XxxView` acts as the VM's `InputParamData` and also carries the `HttpClient`, the request header/tokens (`LoginView.GetRequestHeader()` is static and shared) and the "pack" request DTOs, through explicit `SetXxx`/`GetXxx` methods. Response DTOs are deserialized with `System.Text.Json`.

### REST layer (`CobimExplorer.Rest.Api/CobimBase/<Feature>/`)
Each feature has a static `XxxRestServer` with `async` methods that take the feature's `XxxView` (e.g. `LoginRestServer.PostLoginAsync(loginView)`, `ExplorerRestServer.GetFolderListAsync(explorerView)`), plus `XxxHelper` classes that hold endpoint URLs, content types and result codes as constants. View models call these directly; there is no service interface layer. `LoginRestServer` schedules a token refresh 20 minutes after login. `NetWork/Http/httpManager.cs` is an older helper.

### Styling
`Styles/All.xaml` is merged in `App.xaml` and aggregates colors, brushes, control templates and seasonal themes (`Styles/Themes/{Spring,Summer,Fall,Winter}`). Custom chrome (title bar, ribbon, nav bar) lives in `Controls/`, and its behaviors live in `Behaviors/` (Microsoft.Xaml.Behaviors).
