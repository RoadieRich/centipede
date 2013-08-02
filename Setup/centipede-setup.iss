#define OutputFileBaseName "CentipedeSetup"
#define OutputDir "Output"
#define AppName "Centipede"
#define ProjectDir ".."
#define SetupDir "."
#define BinaryDir ProjectDir + "\bin\Debug"
#define SDKDir SetupDir + "\CentipedeAction"

[Setup]
AppName={#AppName}
AppVersion=0.1.0
AppId={{3783CEE5-0F79-4707-A264-31034B228BB0}
DefaultDirName={pf}\{#AppName}
UninstallDisplayName={#AppName}
Compression=zip
MinVersion=0,5.01
OnlyBelowVersion=0,6.2
DisableStartupPrompt=true
UsePreviousSetupType=true
UsePreviousTasks=true
UsePreviousLanguage=true
FlatComponentsList=False
ShowTasksTreeLines=True
OutputBaseFilename={#OutputFileBaseName}
AllowNoIcons=True
DefaultGroupName=Centipede
OutputDir={#OutputDir}
WizardImageFile={#SetupDir}\images\big2.bmp
WizardSmallImageFile={#SetupDir}\images\centipede.bmp
AlwaysShowGroupOnReadyPage=True
AlwaysShowDirOnReadyPage=True
UninstallDisplayIcon={app}\Centipede.exe
ShowLanguageDialog=auto
ChangesAssociations=Yes
;LicenseFile={#SetupDir}\License.txt
;InfoBeforeFile={#SetupDir}\readme.txt

[Types]
Name: "Complete"; Description: "Install all components"
Name: "Minimal"; Description: "Install only required items"
Name: "Custom"; Description: "Customise the installation"; Flags: iscustom
Name: "CompleteSDK"; Description: "Install all components and SDK"

[Components]
Name: "Centipede"; Description: "The Centipede application (required)"; Types: Complete Custom Minimal CompleteSDK; Flags: fixed
Name: "ExampleJobs"; Description: "Example jobs (disabled)"; Flags: fixed
Name: "Actions"; Description: "Actions"; Types: Complete Custom CompleteSDK
Name: "Actions\Text_File"; Description: "Text File"; Types: Complete CompleteSDK
Name: "Actions\Python"; Description: "Python Actions, Including flow control"; Types: Complete Custom CompleteSDK; Flags: checkablealone
;Name: "Actions\Python\Python_Engine"; Description: "The IronPython 2.7 Interpretter"; ExtraDiskSpaceRequired: 53183775; Types: Complete Custom CompleteSDK; Flags: dontinheritcheck disablenouninstallwarning
Name: "Actions\Solidworks"; Description: "Solidworks 2012"; Types: Complete Custom CompleteSDK
Name: "Actions\Xml"; Description: "Xml"; Types: Complete Custom CompleteSDK
Name: "Actions\Office"; Description: "Office"; Types: Complete Custom CompleteSDK
Name: "Actions\ShellActions"; Description: "Shell Actions"; Types: Complete Custom CompleteSDK
Name: "Actions\MathCad"; Description: "MathCad Actions"; Types: Complete CompleteSDK
Name: "SDK"; Description: "CentipedeAction SDK"; Types: CompleteSDK

[Dirs]
Name: "{app}\Resources"; Components: Centipede
Name: "{app}\Tutorial"; Components: Centipede
Name: "{app}\Plugins"; Components: Centipede
Name: "{app}\Plugins\Resources"; Components: Centipede

; SDK items
Name: "{code:GetSdkDestDir}"; Components: SDK
Name: "{code:GetSdkDestDir}\CentipedeAction"; Components: SDK
Name: "{code:GetSdkDestDir}\CentipedeAction\Resources"; Components: SDK
Name: "{code:GetSdkDestDir}\CentipedeAction\Properties"; Components: SDK

; Example job folder
Name: "{code:GetExampleDir}"; Components: ExampleJobs

[Files]
; Dependencies
Source: "{#SetupDir}\dotNetFx40_Full_x86_x64.exe"; DestDir: "{app}\Resources"; Components: Centipede;
Source: "{#SetupDir}\IronPython-2.7.3.msi"; DestDir: "{app}\Resources"; Components: Centipede;

; Program
Source: "{#BinaryDir}\Centipede.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\CentipedeInterfaces.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\PythonEngine.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\IronPython.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\IronPython.Modules.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\Plugins\Microsoft.Scripting.Core.dll"; DestDir: "{app}\Plugins"; Components: Centipede
Source: "{#BinaryDir}\Action.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\Resources\CentipedeFile.ico"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede

; item for the Explorer New menu
Source: "{#SetupDir}\New Job.100p"; DestDir: "{win}\SHELLNEW"; Components: Centipede

; Tutorial
Source: "{#BinaryDir}\Tutorial\*"; DestDir: "{app}\Tutorial"; Components: Centipede

; Action plugins
Source: "{#BinaryDir}\Plugins\PythonAction.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Python
Source: "{#BinaryDir}\SciLexer.dll"; DestDir: "{app}"; Components: Actions\Python
Source: "{#BinaryDir}\SciLexer64.dll"; DestDir: "{app}"; Components: Actions\Python; Check: IsWin64
Source: "{#BinaryDir}\ScintillaNET.dll"; DestDir: "{app}"; Components: Actions\Python
Source: "{#BinaryDir}\Plugins\SolidworksActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Solidworks
Source: "{#BinaryDir}\Plugins\XMLActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Xml
Source: "{#BinaryDir}\Plugins\OfficeActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Office
Source: "{#BinaryDir}\Plugins\ShellActions.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\ShellActions
Source: "{#BinaryDir}\Plugins\TextFile.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: Actions\Text_File
Source: "{#BinaryDir}\Plugins\MathCADActions.dll"; DestDir: "{app}\Plugins"; Components: Actions\MathCad

; sdk items
Source: "{#SDKDir}\CentipedeAction.sln"; DestDir: "{code:GetSdkDestDir}\sdk"; Flags: confirmoverwrite; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Program.cs"; DestDir: "{code:GetSdkDestDir}\sdk\CentipedeAction"; Components: SDK
Source: "{#SDKDir}\CentipedeAction\CustomActionDisplayControl.cs"; DestDir: "{code:GetSdkDestDir}\sdk\CentipedeAction"; Components: SDK
Source: "{#SDKDir}\CentipedeAction\MyCentipedeAction.csproj"; DestDir: "{code:GetSdkDestDir}\sdk\CentipedeAction"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Resources\MyIcon.ico"; DestDir: "{code:GetSdkDestDir}\sdk\CentipedeAction\Resources"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\Resources.resx"; DestDir: "{code:GetSdkDestDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\AssemblyInfo.cs"; DestDir: "{code:GetSdkDestDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\Resources.Designer.cs"; DestDir: "{code:GetSdkDestDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK

; Sample Jobs
Source: "{#SetupDir}\Samples\*"; DestDir: "{code:GetExampleDir}"; Components: Centipede

[Icons]
Name: "{group}\Centipede"; Filename: "{app}\Centipede.exe"; Flags: useapppaths; IconFilename: "{app}\Centipede.exe"
Name: "{group}\Uninstall Centipede"; Filename: "{uninstallexe}"; IconFilename: "{uninstallexe}"
Name: "{group}\Centipede Help Community"; Filename: "http://getsatisfaction.com/centipede"
Name: "{group}\Centipede Tutiorial"; Filename: "{app}\Tutorial\Tutorial.htm"

[Run]
Filename: "{app}\Resources\dotNetFx40_Full_x86_x64.exe"; Flags: waituntilterminated; StatusMsg: "Installing the .NET framework"; Components: Centipede; Check: FrameworkIsNotInstalled
Filename: "msiexec.exe"; Parameters: "/i ""{app}\Resources\IronPython-2.7.3.msi"" /qb"; WorkingDir: "{app}\Resources"; Flags: shellexec waituntilterminated; StatusMsg: "Installing IronPython 2.7"; Components: Centipede; Check: IronPythonNotInstalled
Filename: "{app}\Centipede.exe"; Flags: nowait postinstall; Description: "Start Centipede"; StatusMsg: "Starting Centipede"; Components: Centipede

[Registry]
Root: "HKLM"; Subkey: "SOFTWARE\Microsoft\Internet Explorer\Main\Feature Control\FEATURE_BROWSER_EMULATION"; ValueType: dword; ValueName: "centipede.exe"; ValueData: "{code:GetIEEmulationValue}"; Flags: createvalueifdoesntexist uninsdeletevalue; Components: Centipede
Root: "HKCR"; Subkey: ".100p"; ValueType: string; ValueName: ""; ValueData: "CentipedeJob"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: ".100p\ShellNew"; ValueType: string; ValueName: "FileName"; ValueData: "{win}\SHELLNEW\New Job.100p"
Root: "HKCR"; Subkey: "CentipedeJob"; ValueType: string; ValueName: ""; ValueData: "Centipede Job"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: "CentipedeJob\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\CentipedeFile.ico"
Root: "HKCR"; Subkey: "CentipedeJob\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\Centipede.exe"" ""%1"" ""%2"" ""%3"" ""%4"" ""%5"" ""%6"" ""%7"" ""%8"" ""%9"""
Root: "HKCR"; Subkey: "CentipedeJob\shell\run"; ValueType: string; ValueName: ""; ValueData: "&Run";
Root: "HKCR"; Subkey: "CentipedeJob\shell\run\command"; ValueType: string; ValueName: ""; ValueData: """{app}\Centipede.exe"" /r ""%1"" ""%2"" ""%3"" ""%4"" ""%5"" ""%6"" ""%7"" ""%8"" ""%9"""

[Code]
#include SetupDir + "\code.pas"
