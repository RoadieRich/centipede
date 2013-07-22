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
                DeleteFile(GetPreviousData('SdkDir', ExpandConstant('{userappdata}\Centipede\favourites.xml')));
        end;
    end;
end;


function ShouldSkipPage(PageID: Integer): Boolean;
begin
    Result := False;
    { skip sdk path request if sdk isn't installed }
    if PageID = SdkDirPage.ID then
        Result := not IsComponentSelected('SDK');
    { skip example jobs page if example jobs not selected to install }
    if PageID = ExamplesDirPage.ID then
        Result := not IsComponentSelected('ExampleJobs');
end;

procedure RegisterPreviousData(PreviousDataKey: Integer);
begin
    SetPreviousData(PreviousDataKey, 'SdkDir', SdkDirPage.Values[0]);
    SetPreviousData(PreviousDataKey, 'ExampleDir', ExamplesDirPage.Values[0]);
end;

function GetSdkDestDir(param: String): String;
begin
    Result := SdkDirPage.Values[0];
end;

function GetExampleDir(param: String): String;
begin
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

{ Split string on Separator, returning an array of strings }
procedure Explode(var Dest: TArrayOfString;	Text: String; Separator: String);
var
    i: Integer;
begin
    i := 0;
    repeat
        SetArrayLength(Dest, i+1);
        if Pos(Separator,Text) > 0 then begin
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
                               'version', versionString) then
        Result := 3
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