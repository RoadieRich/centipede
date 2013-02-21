[Setup]
AppName=Centipede
AppVersion=0.1.0
AppId={{3783CEE5-0F79-4707-A264-31034B228BB0}
DefaultDirName={pf}\Centipede
UninstallDisplayName=Centipede
Compression=none
MinVersion=0,5.01
OnlyBelowVersion=0,6.2
DisableStartupPrompt=False
UsePreviousSetupType=False
UsePreviousTasks=False
UsePreviousLanguage=False
FlatComponentsList=False
ShowTasksTreeLines=True
OutputBaseFilename=Centipede Setup

[Files]
Source: "C:\Documents and Settings\RLovely\My Documents\Dropbox\dotNetFx40_Full_x86_x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall; Components: Centipede; Check: FrameworkIsNotInstalled
Source: "C:\Documents and Settings\RLovely\My Documents\Dropbox\IronPython-2.7.3.msi"; DestDir: "{tmp}"; Components: Actions\Python\Python_Engine; Check: IronPythonNotInstalled
Source: "C:\Documents and Settings\RLovely\My Documents\Dropbox\APPDATA\Centipede\Favourites.xml"; DestDir: "{userappdata}\Centipede"; Flags: confirmoverwrite; Components: Centipede
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Centipede.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Action.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Actions Actions\Python Actions\Python\Python_Engine Actions\Text_File
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\ScintillaNET.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\TextFile.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Text_File
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\Microsoft.Scripting.Core.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python\Python_Engine
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\PythonAction.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\PythonEngine.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python\Python_Engine
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\SolidworksActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Solidworks2010
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\XMLActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Xml
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\OfficeActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Office

[Run]
Filename: "{tmp}\dotNetFx40_Full_x86_x64.exe"; Components: Centipede; Check: FrameworkIsNotInstalled
Filename: "msiexec"; Parameters: "/i {tmp}\IronPython-2.7.3.msi"; WorkingDir: "{tmp}"; Flags: shellexec; Components: Actions\Python\Python_Engine; Check: IronPythonNotInstalled
Filename: "{app}\Centipede.exe"; Components: Centipede; Tasks: StartCentipede

[Dirs]
Name: "{userappdata}\Centipede"
Name: "{app}\Resources"
Name: "{app}\Plugins"
Name: "{app}\Plugins\Resources"
Name: "{app}\Uninstallers"

[Components]
Name: "Centipede"; Description: "The Centipede application"; Types: Complete Custom Minimal; Flags: fixed
Name: "Actions"; Description: "Actions"; Types: Complete Custom
Name: "Actions\Text_File"; Description: "Text File"; ExtraDiskSpaceRequired: 11264; Types: Complete
Name: "Actions\Python"; Description: "Python Actions, Including flow control"; ExtraDiskSpaceRequired: 31744; Types: Complete Custom
Name: "Actions\Python\Python_Engine"; Description: "The IronPython 2.7 Interpretter"; ExtraDiskSpaceRequired: 14336; Types: Complete Custom
Name: "Actions\Solidworks2010"; Description: "Solidworks 2010"; Types: Complete
Name: "Actions\Xml"; Description: "Xml"; Types: Complete Custom
Name: "Actions\Office"; Description: "Office"; Types: Complete Custom

[InstallDelete]
Type: dirifempty; Name: "{app}"

[Types]
Name: "Complete"; Description: "Install all components"
Name: "Minimal"; Description: "Install only required items"
Name: "Custom"; Description: "Cusomise the installation"; Flags: iscustom

[UninstallRun]

[Registry]
Root: "HKCU"; Subkey: "Chemineer\Centipede"; ValueType: string; ValueName: "FavouriteFile"; ValueData: "%APPDATA%\Centipede\favourites.xml"; Flags: createvalueifdoesntexist; Components: Centipede

[Tasks]
Name: "StartCentipede"; Description: "Start Centipede"; Flags: unchecked; Components: Centipede

[Code]
var
  UninstallPython: Boolean;
  
  
function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;

function IronPythonNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\IronPython\2.7');
end;

function AskToUninstallPython: Boolean;
begin
  Result := UninstallPython;
end;
