#define OutputFileBaseName "CentipedeSetup"
#define OutputDir "Output"
#define AppName "Centipede"
[Setup]
AppName={#AppName}
AppVersion=0.1.0
AppId={{3783CEE5-0F79-4707-A264-31034B228BB0}
DefaultDirName={pf}\{#AppName}
UninstallDisplayName={#AppName}
Compression=zip
MinVersion=0,5.01
OnlyBelowVersion=0,6.2
DisableStartupPrompt=False
UsePreviousSetupType=False
UsePreviousTasks=False
UsePreviousLanguage=False
FlatComponentsList=False
ShowTasksTreeLines=True
OutputBaseFilename={#OutputFileBaseName}
AllowNoIcons=True
DefaultGroupName=Centipede
OutputDir={#OutputDir}

[Files]
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\dotNetFx40_Full_x86_x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall; Components: Centipede; Check: FrameworkIsNotInstalled
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\IronPython-2.7.3.msi"; DestDir: "{tmp}"; Components: Actions\Python\Python_Engine; Check: IronPythonNotInstalled
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\favourites.xml"; DestDir: "{userappdata}\Centipede"; Flags: confirmoverwrite; Components: Centipede
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Centipede.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Action.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Actions Actions\Python Actions\Python\Python_Engine Actions\Text_File
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\ScintillaNET.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\TextFile.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Text_File
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\Microsoft.Scripting.Core.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python\Python_Engine
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\PythonAction.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\PythonEngine.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python\Python_Engine
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\SolidworksActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Solidworks
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\XMLActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Xml
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\OfficeActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Office
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\CentipedeInterfaces.dll"; DestDir: "{app}"
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\bin\Debug\Plugins\ShellActions.dll"; DestDir: "{app}\Plugins"; Components: Actions\ShellActions

; sdk items
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction.sln"; DestDir: "{code:GetSdkDir}\sdk"; Flags: confirmoverwrite; Components: SDK
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction\Program.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Components: SDK
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction\CustomActionDisplayControl.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Components: SDK
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction\MyCentipedeAction.csproj"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Flags: ignoreversion; Components: SDK
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction\Resources\MyIcon.ico"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Resources"; Flags: ignoreversion; Components: SDK
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction\Properties\Resources.resx"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction\Properties\AssemblyInfo.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction\CentipedeAction\Properties\Resources.Designer.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK

[Run]
Filename: "{tmp}\dotNetFx40_Full_x86_x64.exe";   StatusMsg: "Installing the .NET framework";                                                                     Components: Centipede;                     Check: FrameworkIsNotInstalled
Filename: "msiexec";                             StatusMsg: "Installing IronPython 2.7"; Parameters: "/i {tmp}\IronPython-2.7.3.msi"; Flags: shellexec;          Components: Actions\Python\Python_Engine;  Check: IronPythonNotInstalled
Filename: "{app}\Centipede.exe";                 StatusMsg: "Starting Centipede"; Description: "Start Centipede";                     Flags: nowait postinstall; Components: Centipede

[Dirs]
Name: "{userappdata}\Centipede"
Name: "{app}\Resources"
Name: "{app}\Plugins"
Name: "{app}\Plugins\Resources"

; SDK items
Name: "{code:GetSdkDir}\sdk";                             Components: SDK
Name: "{code:GetSdkDir}\sdk\CentipedeAction";             Components: SDK
Name: "{code:GetSdkDir}\sdk\CentipedeAction\Resources";   Components: SDK
Name: "{code:GetSdkDir}\sdk\CentipedeAction\Properties";  Components: SDK

[Components]
Name: "Centipede"; Description: "The Centipede application"; Types: Complete Custom Minimal; Flags: fixed
Name: "Actions"; Description: "Actions"; Types: Complete Custom
Name: "Actions\Text_File"; Description: "Text File"; Types: Complete
Name: "Actions\Python"; Description: "Python Actions, Including flow control"; Types: Complete Custom
Name: "Actions\Python\Python_Engine"; Description: "The IronPython 2.7 Interpretter"; Types: Complete Custom
Name: "Actions\Solidworks"; Description: "Solidworks 2012"; Types: Complete Custom
Name: "Actions\Xml"; Description: "Xml"; Types: Complete Custom
Name: "Actions\Office"; Description: "Office"; Types: Complete Custom
Name: "SDK"; Description: "CentipedeAction SDK"
Name: "Actions\ShellActions"; Description: "Shell Actions"; Types: Complete Custom

[InstallDelete]
Type: dirifempty; Name: "{app}"

[Types]
Name: "Complete"; Description: "Install all components"
Name: "Minimal";  Description: "Install only required items"
Name: "Custom";   Description: "Customise the installation"; Flags: iscustom

[UninstallRun]

[Registry]

[Tasks]

[Icons]
Name: "{userprograms}\Centipede\Centipede";           Filename: "{app}\Centipede.exe"; IconFilename: "{app}\Centipede.exe"; Components: Centipede; Flags: useapppaths
Name: "{userprograms}\Centipede\Uninstall Centipede"; Filename: "{uninstallexe}";      IconFilename: "{uninstallexe}";      Components: Centipede

[PostCompile]
Name: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\ChecksumGen.exe"; Parameters: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\Output\CentipedeSetup.exe"

[Code]
var
  SdkDirPage: TInputDirWizardPage;

procedure InitializeWizard;
begin
  { Taken from CodeDlg.iss example script }
  { Create custom pages to show during install }
  SdkDirPage := CreateInputDirPage(wpSelectComponents,
    '{#AppName} SDK Directory', '',
    'Please select a location for the {#AppName} sdk. (We recommend the default.)',
    False, '');
  SdkDirPage.Add('');
  #ifdef DEBUG
  MsgBox('configured datadirpage', mbError, MB_OK);
  #endif

  SdkDirPage.Values[0] := GetPreviousData('DataDir', ExpandConstant('{userdocs}\{#appname}SDK'));
  
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;
  // skip sdk path request if sdk isn't installed }
  if PageID = SdkDirPage.ID then
    Result := not IsComponentSelected('SDK');
end;

// This is needed only if you use GetPreviousData above.
procedure RegisterPreviousData(PreviousDataKey: Integer);
begin
  SetPreviousData(PreviousDataKey, 'DataDir', SdkDirPage.Values[0]);
end;

function GetSdkDir(param: String): String;
begin
  { Return the selected DataDir }
  #ifdef DEBUG
  MsgBox('GetDataDir.' + param, mbError, MB_OK);
  #endif
  Result := SdkDirPage.Values[0];
  #ifdef DEBUG
  MsgBox(DataDirPage.Values[0], mbError, MB_OK);
  #endif
end;

function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;

function IronPythonNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\IronPython\2.7');
end;
