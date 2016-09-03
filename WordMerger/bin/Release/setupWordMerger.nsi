; Include Modern UI
!include "MUI2.nsh"

; Name of the installer
Name "Word Merger"

; File to write the installer to
OutFile "setupWordMerger.exe"

; License stuff
; LicenseText "License page"
; LicenseData "license.txt"

; The default installation directory
InstallDir "$PROGRAMFILES\Pieterv24\WordMerger"

; The privilage needed to run
RequestExecutionLevel admin

; Variables

Var StartMenuFolder

; Interface settings
!define MUI_ABORTWARNING

; Pages

!insertmacro MUI_PAGE_WELCOME
; !insertmacro MUI_PAGE_LICENSE "license.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY

; Start menu folder page configuration
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\Pieterv24\WordMerger"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH


; Languages

!insertmacro MUI_LANGUAGE "English"

Function .onInit

	Call IsDotNETInstalled
	Pop $0
	StrCmp $0 1 found_dotNETFramework no_dotNETFramework
		
	no_dotNETFramework:
	MessageBox MB_OK|MB_ICONSTOP "Framework was not found"
	abort
	
	found_dotNETFramework:

FunctionEnd

; Main Installer
Section "Word Merger (required)" MainInstall
	
	; Set as required
	SectionIn RO
	
	; Set output path to the installation directory
	SetOutPath $INSTDIR
	
	; File to write to there
	File WordMerger.exe
	
	; Write uninstall keys for windows
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WordMerger" "DisplayName" "Word Merger"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WordMerger" "UninstallString" '"$INSTDIR\uninstall.exe"'
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WordMerger" "Publisher" "Pieter Verweij"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WordMerger" "DisplayVersion" "1.0"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WordMerger" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WordMerger" "NoRepair" 1
	
	; Create uninstall file
	WriteUninstaller "Uninstall.exe"
	
	!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
	
		; Create shortcuts
		CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
		CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Word Merger.lnk" "$INSTDIR\WordMerger.exe"
		
	!insertmacro MUI_STARTMENU_WRITE_END
	
SectionEnd

; Add right click options
Section  "Add right click options" RightClick
	;Create Shortcut
	CreateShortCut "$SENDTO\Merge Word Documents.lnk" "$INSTDIR\WordMerger.exe"
SectionEnd

; Description
LangString DESC_MainInstall ${LANG_ENGLISH} "The main installer."
LangString DESC_RightClick ${LANG_ENGLISH} "Add a option to the context menu(right click menu)"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${MainInstall} $(DESC_MainInstall)
	!insertmacro MUI_DESCRIPTION_TEXT ${RightClick} $(DESC_RightClick)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

; Uninstaller
Section "Uninstall"
	
	!insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
	
	; Remove registry keys
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WordMerger"
	DeleteRegKey HKLM "SOFTWARE\WordMerger"
	DeleteRegKey /ifempty HKCU "Software\WordMerger"
	
	; Remove files and uninstaller
	Delete $INSTDIR\WordMerger.exe
	Delete $INSTDIR\uninstall.exe
	
	; Remove shortcuts
	Delete "$SMPROGRAMS\$StartMenuFolder\*.*"
	Delete "$SENDTO\Merge Word Documents.lnk"
	
	; Remove directories
	RMDir "$SMPROGRAMS\$StartMenuFolder"
	RMDir "$INSTDIR"
SectionEnd

Function IsDotNETInstalled
	Push $0
	Push $1
	
	StrCpy $0 1
	System::Call "mscoree::GetCORVersion(w, i ${NSIS_MAX_STRLEN}, *i) i .r1"
	StrCmp $1 0 +2
		StrCpy $0 0
	
	Pop $1
	Exch $0
FunctionEnd