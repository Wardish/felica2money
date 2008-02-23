; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
AppName=FeliCa2Money
AppVerName=FeliCa2Money ver 2.0
AppPublisher=Takuya Murakami
AppPublisherURL=http://moneyimport.sourceforge.jp
AppSupportURL=http://moneyimport.sourceforge.jp
AppUpdatesURL=http://moneyimport.sourceforge.jp
DefaultDirName={pf}\FeliCa2Money
DefaultGroupName=FeliCa2Money
LicenseFile=D:\dev\moneyimport\FeliCa2Money.net\Release\2.0\LICENSE.TXT
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese-5-5.1.11.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\dev\moneyimport\FeliCa2Money.net\Release\2.0\FeliCa2Money.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\dev\moneyimport\FeliCa2Money.net\Release\2.0\FeliCa2Money.html"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\dev\moneyimport\FeliCa2Money.net\Release\2.0\LICENSE.TXT"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\FeliCa2Money"; Filename: "{app}\FeliCa2Money.exe"
Name: "{commondesktop}\FeliCa2Money"; Filename: "{app}\FeliCa2Money.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\FeliCa2Money"; Filename: "{app}\FeliCa2Money.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\FeliCa2Money.exe"; Description: "{cm:LaunchProgram,FeliCa2Money}"; Flags: nowait postinstall skipifsilent

