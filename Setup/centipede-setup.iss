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
WizardImageFile={#SetupDir}\images\big2.bmp
WizardSmallImageFile={#SetupDir}\images\centipede.bmp
AlwaysShowGroupOnReadyPage=True
AlwaysShowDirOnReadyPage=True
UninstallDisplayIcon={app}\Centipede.exe
ShowLanguageDialog=auto
ChangesAssociations=Yes

[Types]
Name: "Complete";	Description: "Install all components"
Name: "Minimal";	Description: "Install only required items"
Name: "Custom";	Description: "Customise the installation";	Flags: iscustom
Name: "CompleteSDK";	Description: "Install all components and SDK"

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
Name: "{userappdata}\Centipede";	Components: Centipede
;Name: "{app}\Resources"; Components: Centipede
Name: "{app}\Plugins"; Components: Centipede
Name: "{app}\Plugins\Resources"; Components: Centipede

; SDK items
Name: "{code:GetSdkDir}";	Components: SDK
Name: "{code:GetSdkDir}\CentipedeAction";	Components: SDK
Name: "{code:GetSdkDir}\CentipedeAction\Resources";	Components: SDK
Name: "{code:GetSdkDir}\CentipedeAction\Properties";	Components: SDK

; Example job folder
Name: "{code:GetExampleDir}";	Components: ExampleJobs

[Files]
; Dependencies
Source: "{#SetupDir}\dotNetFx40_Full_x86_x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall external; ExternalSize: 182; Components: Centipede; Check: FrameworkIsNotInstalled
Source: "{#SetupDir}\IronPython-2.7.3.msi"; DestDir: "{tmp}"; Flags: deleteafterinstall external; ExternalSize: 35; Components: Centipede; Check: IronPythonNotInstalled

; Program
Source: "{#BinaryDir}\Centipede.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\CentipedeInterfaces.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\PythonEngine.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\IronPython.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\IronPython.Modules.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\Plugins\Microsoft.Scripting.Core.dll"; DestDir: "{app}\Plugins"; Components: Centipede
Source: "{#BinaryDir}\Action.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede
Source: "{#BinaryDir}\Resources\CentipedeFile.ico"; DestDir: "{app}"; Flags: ignoreversion; Components: Centipede

Source: "{#SetupDir}\New Job.100p"; DestDir: "{win}\SHELLNEW"; Components: Centipede

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
Source: "{#SDKDir}\CentipedeAction.sln"; DestDir: "{code:GetSdkDir}\sdk"; Flags: confirmoverwrite; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Program.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Components: SDK
Source: "{#SDKDir}\CentipedeAction\CustomActionDisplayControl.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Components: SDK
Source: "{#SDKDir}\CentipedeAction\MyCentipedeAction.csproj"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Resources\MyIcon.ico"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Resources"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\Resources.resx"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\AssemblyInfo.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK
Source: "{#SDKDir}\CentipedeAction\Properties\Resources.Designer.cs"; DestDir: "{code:GetSdkDir}\sdk\CentipedeAction\Properties"; Flags: ignoreversion; Components: SDK

; Sample Jobs
;

[Icons]
Name: "{group}\Centipede"; Filename: "{app}\Centipede.exe"; Flags: useapppaths; IconFilename: "{app}\Centipede.exe"
Name: "{group}\Uninstall Centipede"; Filename: "{uninstallexe}"; IconFilename: "{uninstallexe}"
Name: "{group}\Centipede Help Community"; Filename: "http://getsatisfaction.com/centipede"

[Run]
Filename: "{tmp}\dotNetFx40_Full_x86_x64.exe";	StatusMsg: "Installing the .NET framework";	Components: Centipede;	Check: FrameworkIsNotInstalled
Filename: "msiexec";	Parameters: "/i {tmp}\IronPython-2.7.3.msi";	Flags: shellexec;	StatusMsg: "Installing IronPython 2.7"; Components: Centipede;	Check: IronPythonNotInstalled
Filename: "{app}\Centipede.exe";	Flags: nowait postinstall;	Description: "Start Centipede";	StatusMsg: "Starting Centipede";	Components: Centipede

[Registry]
Root: "HKLM"; Subkey: "SOFTWARE\Microsoft\Internet Explorer\Main\Feature Control\FEATURE_BROWSER_EMULATION";	ValueType: dword;	ValueName: "centipede.exe";	ValueData: "{code:GetIEEmulationValue}";	Flags: createvalueifdoesntexist uninsdeletevalue; Components: Centipede
Root: "HKCR"; Subkey: ".100p"; ValueType: string; ValueName: ""; ValueData: "CentipedeJob"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: ".100p\ShellNew"; ValueType: string; ValueName: "FileName"; ValueData: "{win}\SHELLNEW\New Job.100p"
Root: "HKCR"; Subkey: "CentipedeJob"; ValueType: string; ValueName: ""; ValueData: "Centipede Job"; Flags: uninsdeletekey
Root: "HKCR"; Subkey: "CentipedeJob\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\CentipedeFile.ico"
Root: "HKCR"; Subkey: "CentipedeJob\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\Centipede.exe"" ""%1"""
Root: "HKCR"; Subkey: "CentipedeJob\shell\run"; ValueType: string; ValueName: ""; ValueData: "&Run";
Root: "HKCR"; Subkey: "CentipedeJob\shell\run\command"; ValueType: string; ValueName: ""; ValueData: """{app}\Centipede.exe"" /r ""%1"""  

[Code]
var
    SdkDirPage: TInputDirWizardPage;
    ExamplesDirPage: TInputDirWizardPage;
	
procedure InitializeWizard;
begin
    { Taken from CodeDlg.iss example script }
    { Create custom pages to show during install }
    SdkDirPage := CreateInputDirPage(wpSelectComponents,
        '{#AppName} SDK Directory', '',
        'Please select a location for the {#AppName} sdk.', False, '');
    SdkDirPage.Add('');
    SdkDirPage.Values[0] := GetPreviousData('SdkDir', ExpandConstant('{userdocs}\{#appname}SDK'));
	
	ExamplesDirPage := CreateInputDirPage(wpSelectComponents,
        '{#AppName} Example Jobs location Directory', '',
        'Please select a location for the {#AppName} example jobs.', False, '');
    ExamplesDirPage.Add('');
    ExamplesDirPage.Values[0] := GetPreviousData('ExamplesDir', ExpandConstant('{userdocs}'));
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  mRes : integer;
begin
  { Ask to remove favourites file }
  case CurUninstallStep of
    usUninstall:
      begin
        mRes := MsgBox('Do you want to remove your Favourites file?', 
                       mbConfirmation, 
                       MB_YESNO or MB_DEFBUTTON2)
        if mRes = IDYES then
          begin
            DeleteFile(GetPreviousData('SdkDir', ExpandConstant('{userappdata}\Centipede\favourites.xml')));
          end
        else
      end;
  end;
end;


function ShouldSkipPage(PageID: Integer): Boolean;
begin
    Result := False;
    // skip sdk path request if sdk isn't installed
    if PageID = SdkDirPage.ID then
        Result := not IsComponentSelected('SDK');
		
	// skip example jobs page if example jobhs not selected to install
	if PageID = ExamplesDirPage.ID then
		Result := not IsComponentSelected('ExampleJobs');
end;

// This is needed only if you use GetPreviousData above.
procedure RegisterPreviousData(PreviousDataKey: Integer);
begin
    SetPreviousData(PreviousDataKey, 'SdkDir', SdkDirPage.Values[0]);
    SetPreviousData(PreviousDataKey, 'ExampleDir', ExamplesDirPage.Values[0]);
	
end;

function GetSdkDir(param: String): String;
begin
    { Return the selected DataDir }
    Result := SdkDirPage.Values[0];
end;

function GetExampleDir(param: String): String;
begin
    { Return the selected DataDir }
    Result := ExamplesDirPage.Values[0];
end;


function FrameworkIsNotInstalled: Boolean;
begin
    Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;

function IronPythonNotInstalled: Boolean;
begin
    Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\IronPython\2.7');
end;

procedure Explode(var Dest: TArrayOfString;	Text: String; Separator: String);
var
	i: Integer;
begin
	i := 0;
	repeat
		SetArrayLength(Dest, i+1);
		if Pos(Separator,Text) > 0 then	begin
			Dest[i] := Copy(Text, 1, Pos(Separator, Text)-1);
			Text := Copy(Text, Pos(Separator,Text) + Length(Separator), Length(Text));
			i := i + 1;
		end 
		else begin
			 Dest[i] := Text;
			 Text := '';
		end;
	until Length(Text)=0;
end;

function GetIEVersion: Longint;
var
    versionString : String;
    versionParts : TArrayOfString;
begin
    if not RegQueryStringValue(HKEY_LOCAL_MACHINE, 'Software\Microsoft\Internet Explorer', 
							   'version', versionString) then begin
        Result := 3
	end
    else begin
        Explode(versionParts, versionString, '.');
        Result := StrToInt(versionParts[0]);
    end;
end;

function GetIEEmulationValue(Param: String): String;
begin
    Result := inttostr(GetIEVersion * 1000)
end;

procedure BackupFavourites;
begin
    FileCopy(ExpandConstant('{userappdata}\Centipede\favourites.xml'),
             ExpandConstant('{userappdata}\Centipede\favourites.xml.old'),
             False);
end;