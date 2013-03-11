


iscc = /cygdrive/c/Program\ Files/Inno\ Setup\ 5/ISCC
binfolder = ../bin/Debug
pluginsfolder = $(binfolder)/Plugins
setupoutputfolder = ./Output


installerfilename = $(setupoutputfolder)/CentipedeSetup.exe
webpagefilename = $(setupoutputfolder)/index.html
uploadlog = $(setupoutputfolder)/upload.log

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



all: compile webpage upload notify

installer: $(installerfilename)
compile: installer
webpage: $(webpagefilename)
upload: upload.log
notify: notify.log

$(installerfilename): $(binfolderfiles) $(pluginfiles) $(setupfiles)
	$(iscc) centipede-setup.iss

$(webpagefilename): $(pagescipt) $(installerfilename) 
	$(scriptengine) $(pagescript) $(installerfilename) $(webpagefilename)

upload.log: $(uploadscript) $(installerfilename) $(webpagefilename)
	$(uploadtool) -b $(uploadscript) $(webuser)@$(server) > upload.log
	@echo Upload complete.

notify.log: upload.log
	@#nothing for now.
	@touch notify.log
	
clean:
	@rm -rf $(setupoutputfolder)
	@rm upload.log notify.log
	
install: $(installerfilename)
	$(installerfilename)
	
help:
	@echo Available targets:
	@echo
	@echo 'installer / compile'
	@echo '	Compliles the installer executable'
	@echo 'webpage'
	@echo '	Generates the download webpage with md5 and sha1 checksums'
	@echo 'upload'
	@echo '	Uploads the installer and executable to the webserver'
	@echo 'all (default)'
	@echo '	Compiles, generates the webpage, and uploads.'
	@echo 'install'
	@echo '	Runs the installer'
	@echo 'clean'
	@echo '	Deletes all files from the output folder'
	
