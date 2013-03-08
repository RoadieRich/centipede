


iscc = /cygdrive/c/Program\ Files/Inno\ Setup\ 5/ISCC
binfolder = ../bin/Debug
pluginsfolder = $(binfolder)/Plugins
setupoutputfolder = ./Output

installerfilename = $(setupoutputfolder)/CentipedeSetup.exe
webpagefilename = $(setupoutputfolder)/index.html

binfolderfiles = $(binfolder)/Action.dll $(binfolder)/Centipede.exe $(binfolder)/CentipedeInterfaces.dll $(binfolder)/SciLexer.dll
pluginfiles1 = $(pluginsfolder)/CentipedeAction.dll $(pluginsfolder)/PythonEngine.dll $(pluginsfolder)/SolidworksActions.dll  
pluginfiles2 = $(pluginsfolder)/Microsoft.Scripting.Core.dll $(pluginsfolder)/SciLexer.dll $(pluginsfolder)/TextFile.dll
pluginfiles3 = $(pluginsfolder)/OfficeActions.dll $(pluginsfolder)/ScintillaNET.dll $(pluginsfolder)/XMLActions.dll
pluginfiles4 = $(pluginsfolder)/PythonAction.dll $(pluginsfolder)/ShellActions.dll

pluginfiles = $(pluginfiles1) $(pluginfiles2) $(pluginfiles3) $(pluginfiles4)

setupfiles = centipede-setup.iss favourites.xml dotNetFx40_Full_x86_x64.exe IronPython-2.7.3.msi

pagescript = create-webpage.py
scriptengine = python

uploadscript = sftp-upload.sftpc
uploadtool = /usr/bin/sftp

webuser = www-data
server = ps-der-hg1

all: compile webpage upload

compile: $(binfolderfiles) $(pluginfiles) $(setupfiles)
	echo ''
	echo 'Compiling'
	$(iscc) centipede-setup.iss

webpage: create-webpage.py $(installerfilename) 
	echo ''
	echo 'Creating index.htm'
	$(scriptengine) $(pagescript) $(installerfilename) $(webpagefilename)

upload: sftp-upload.sftpc $(installerfilename) $(webpagefilename)
	echo ''
	echo 'Uploading'
	$(uploadtool) -b $(uploadscript) $(webuser)@$(server)

	echo ''
	echo ''
	echo 'All done.'
	