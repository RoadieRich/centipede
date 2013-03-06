#define OutputFileBaseName "CentipedeSetup"
#define OutputDir "Output"
#define AppName "Centipede"
#define ProjectDir "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede"
#define SetupDir ProjectDir + "\Setup"
#define BinaryDir ProjectDir + "\bin\Debug"
#define SDKDir "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\CentipedeAction"
#define CygwinSetupDir "/home/RLovely/MyDocuments/Visual\ Studio\ 2010/Projects/Centipede/Setup"

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
WizardImageFile=C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\images\big.bmp
WizardSmallImageFile=C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\images\small.bmp
AlwaysShowGroupOnReadyPage=True
AlwaysShowDirOnReadyPage=True
UninstallDisplayIcon={app}\Centipede.exe
ShowLanguageDialog=auto

[Files]
Source: "{#SetupDir}\dotNetFx40_Full_x86_x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall; Components: Centipede; Check: FrameworkIsNotInstalled
Source: "{#SetupDir}\IronPython-2.7.3.msi"; DestDir: "{tmp}"; Components: Actions\Python\Python_Engine; Check: IronPythonNotInstalled
Source: "{#SetupDir}\favourites.xml"; DestDir: "{userappdata}\Centipede"; Flags: confirmoverwrite; Components: Centipede UserFiles
Source: "{#BinaryDir}\Centipede.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\Action.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Actions Actions\Python Actions\Python\Python_Engine Actions\Text_File
Source: "{#BinaryDir}\Plugins\ScintillaNET.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python
Source: "{#BinaryDir}\Plugins\TextFile.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Text_File
Source: "{#BinaryDir}\Plugins\Microsoft.Scripting.Core.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python\Python_Engine
Source: "{#BinaryDir}\Plugins\PythonAction.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python
Source: "{#BinaryDir}\Plugins\PythonEngine.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python\Python_Engine
Source: "{#BinaryDir}\Plugins\SolidworksActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Solidworks
Source: "{#BinaryDir}\Plugins\XMLActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Xml
Source: "{#BinaryDir}\Plugins\OfficeActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Office
Source: "{#BinaryDir}\CentipedeInterfaces.dll"; DestDir: "{app}"
Source: "{#BinaryDir}\Plugins\ShellActions.dll"; DestDir: "{app}\Plugins"; Components: Actions\ShellActions

; sdk items
Source: "{#SDKDir}\CentipedeAction.sln"; DestDir: "{code:GetSdkDir}\sdk"; Flags: confirmoverwrite; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Program.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Components: SDK
Source: "{#SDKDir}\CentipedeAction\CustomActionDisplayControl.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Components: SDK
Source: "{#SDKDir}\CentipedeAction\MyCentipedeAction.csproj"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Resources\MyIcon.ico"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Resources"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\Resources.resx"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\AssemblyInfo.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\Resources.Designer.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK

[Run]
Filename: "{tmp}\dotNetFx40_Full_x86_x64.exe"; StatusMsg: "Installing the .NET framework"; Components: Centipede; Check: FrameworkIsNotInstalled
Filename: "msiexec"; Parameters: "/i {tmp}\IronPython-2.7.3.msi"; Flags: shellexec; StatusMsg: "Installing IronPython 2.7"; Components: Actions\Python\Python_Engine; Check: IronPythonNotInstalled
Filename: "{app}\Centipede.exe"; Flags: nowait postinstall; Description: "Start Centipede"; StatusMsg: "Starting Centipede"; Components: Centipede UserFiles

[Dirs]
Name: "{userappdata}\Centipede"; Components: UserFiles
Name: "{app}\Resources"
Name: "{app}\Plugins"
Name: "{app}\Plugins\Resources"

; SDK items
Name: "{code:GetSdkDir}\sdk"; Components: SDK
Name: "{code:GetSdkDir}\sdk\CentipedeAction"; Components: SDK
Name: "{code:GetSdkDir}\sdk\CentipedeAction\Resources"; Components: SDK
Name: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Components: SDK

[Components]
Name: "Centipede"; Description: "The Centipede application"; Types: Complete Custom Minimal; Flags: fixed
Name: "Actions"; Description: "Actions"; Types: Complete Custom
Name: "Actions\Text_File"; Description: "Text File"; Types: Complete
Name: "Actions\Python"; Description: "Python Actions, Including flow control"; Types: Complete Custom
Name: "Actions\Python\Python_Engine"; Description: "The IronPython 2.7 Interpretter"; Types: Complete Custom
Name: "Actions\Solidworks"; Description: "Solidworks 2012"; Types: Complete Custom
Name: "Actions\Xml"; Description: "Xml"; Types: Complete Custom
Name: "Actions\Office"; Description: "Office"; Types: Complete Custom
Name: "Actions\ShellActions"; Description: "Shell Actions"; Types: Complete Custom
Name: "SDK"; Description: "CentipedeAction SDK"; Types: Complete
Name: "UserFiles"; Description: "User Configuration"; Types: UserSetup; Flags: fixed

[InstallDelete]
Type: dirifempty; Name: "{app}"

[Types]
Name: "Complete"; Description: "Install all components"
Name: "Minimal"; Description: "Install only required items"
Name: "Custom"; Description: "Customise the installation"; Flags: iscustom
Name: "UserSetup"; Description: "Setup pre-installed Centipede for local user"

[UninstallRun]

[Registry]

[Tasks]

[Icons]
Name: "{userprograms}\Centipede\Centipede"; Filename: "{app}\Centipede.exe"; Flags: useapppaths; IconFilename: "{app}\Centipede.exe"; Components: Centipede UserFiles
Name: "{userprograms}\Centipede\Uninstall Centipede"; Filename: "{uninstallexe}"; IconFilename: "{uninstallexe}"; Components: Centipede UserFiles

[PostCompile]
Name: "C:\Python32\python.exe"; Parameters: ""{#SetupDir}\Checksum.py" "{#SetupDir}\{#OutputDir}\{#OutputFileBaseName}.exe""; Flags: runminimized cmdprompt redirectoutput


;I'm sure I had this working at one point...
;Name: "C:\cygwin\bin\bash.exe"; Parameters: "-c /usr/bin/echo lcd {#CygwinSetupDir}/Output > {#CygwinSetupDir}/upload.sftpc"
;Name: "C:\cygwin\bin\bash.exe"; Parameters: "-c /usr/bin/echo put * >> {#CygwinSetupDir}/upload.sftpc"
;Name: "C:\cygwin\bin\sftp.exe"; Parameters: "-b "C:/Documents and Settings/RLovely/My Documents/Visual Studio 2010/Projects/Centipede/Setup/upload.sftpc" www-data@ps-der-hg1"
;Name: "C:\cygwin\bin\sftp.exe"; Parameters: "-b "{#CygwinSetupDir}/upload.sftpc" www-data@ps-der-hg1"
;Name: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\ChecksumGen.exe"; Parameters: "C:\Documents and Settings\RLovely\My Documents\Visual Studio 2010\Projects\Centipede\Setup\Output\CentipedeSetup.exe"

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
  Result := SdkDirPage.Values[0];
end;

function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;

function IronPythonNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\IronPython\2.7');
end;

#expr SaveToFile(AddBackslash(SetupDir) + "Preprocessed.iss")
