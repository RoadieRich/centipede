######################################
#                                    #
#   ______________________________   #
#  /   Yes, I'm not a centipede.  \  #
#  \ Are you surprised to see me? /  #
#   ------------------------------   #
#          \   ^__^                  #
#           \  (oo)\_______          #
#              (__)\       )\/\      #
#                  ||----w |         #
#                  ||     ||         #
#                                    #
#                                    #
######################################

iscc = /cygdrive/c/Program\ Files/Inno\ Setup\ 5/ISCC
binfolder = ../bin/Debug
pluginsfolder = $(binfolder)/Plugins
setupoutputfolder = ./Output

installerfilename = CentipedeSetup.exe
installerfilepath = $(setupoutputfolder)/$(installerfilename)
webpagefilename = $(setupoutputfolder)/index.html
uploadlog = $(setupoutputfolder)/upload.log

binfolderfiles = $(binfolder)/Action.dll $(binfolder)/Centipede.exe $(binfolder)/CentipedeInterfaces.dll $(binfolder)/SciLexer.dll
pluginfiles = $(pluginsfolder)/CentipedeAction.dll $(pluginsfolder)/PythonEngine.dll $(pluginsfolder)/SolidworksActions.dll \
	$(pluginsfolder)/Microsoft.Scripting.Core.dll $(pluginsfolder)/SciLexer.dll $(pluginsfolder)/TextFile.dll \
	$(pluginsfolder)/OfficeActions.dll $(pluginsfolder)/ScintillaNET.dll $(pluginsfolder)/XMLActions.dll \
	$(pluginsfolder)/PythonAction.dll $(pluginsfolder)/ShellActions.dll

setupfiles = centipede-setup.iss favourites.xml dotNetFx40_Full_x86_x64.exe IronPython-2.7.3.msi

pagescript = create-webpage.py
scriptengine = python

uploadscript = sftp-upload.sftpc
uploadtool = /usr/bin/sftp

#assumes sftp is set up for unattended log in to this account
webuser = www-data
server = ps-der-hg1

all: installer webpage upload notify

installer: $(installerfilepath)
webpage: $(webpagefilename)
upload: upload.log
notify: notify.log

$(installerfilepath): $(binfolderfiles) $(pluginfiles) $(setupfiles)
	$(iscc) centipede-setup.iss

$(webpagefilename): $(pagescript) $(installerfilepath) 
	$(scriptengine) $(pagescript) $(installerfilepath) $(webpagefilename)

upload.log: $(uploadscript) $(installerfilepath) $(webpagefilename)
	$(uploadtool) -b $(uploadscript) $(webuser)@$(server) > upload.log
	@echo Upload complete.

notify.log: upload.log
	@#nothing for now.
	touch notify.log
	
checksum:
	@echo 'Local: '
	@echo 'md5:         ' `md5sum $(installerfilepath) | awk '{print $$1}'`
	@echo 'sha1:        ' `sha1sum $(installerfilepath) | awk '{print $$1}'`
	@echo 'Server sha1: ' `ssh $(webuser)@$(server) -C 'sha1sum $(installerfilename)' | awk '{print $$1}' `
		
clean:
	@rm -rf $(setupoutputfolder)
	@rm upload.log notify.log
	
install: $(installerfilepath)
	$(installerfilepath)
	
help:
	@echo Available targets:
	@echo
	@echo 'installer'
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
	
