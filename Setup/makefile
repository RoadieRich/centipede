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

binfolderfiles = $(binfolder)/Action.dll $(binfolder)/Centipede.exe $(binfolder)/CentipedeInterfaces.dll \
	$(binfolder)/SciLexer.dll $(binfolder)/SciLexer64.dll
pluginfiles = $(pluginsfolder)/CentipedeAction.dll $(pluginsfolder)/SolidworksActions.dll \
	$(pluginsfolder)/Microsoft.Scripting.Core.dll $(pluginsfolder)/TextFile.dll \
	$(pluginsfolder)/OfficeActions.dll $(pluginsfolder)/XMLActions.dll $(pluginsfolder)/PythonAction.dll \
	$(pluginsfolder)/ShellActions.dll

installersource = centipede-setup.iss
	
setupfiles = $(installersource) favourites.xml dotNetFx40_Full_x86_x64.exe IronPython-2.7.3.msi

pagescript = create-webpage.py
scriptengine = python

uploadscript = sftp-upload.sftpc
uploadtool = /usr/bin/sftp

#assumes sftp is set up for unattended log in to this account
webuser = www-data
server = ps-der-hg1

all: installer webpage upload notify checksum.log

installer: $(installerfilepath)

webpage: $(webpagefilename)

upload: upload.log

notify: notify.log

$(installerfilepath): $(binfolderfiles) $(pluginfiles) $(setupfiles)
	$(iscc) $(installersource)

$(webpagefilename): $(pagescript) $(installerfilepath) 
	$(scriptengine) $(pagescript) $(installerfilepath) $(webpagefilename) $(comments)

upload.log: $(uploadscript) $(installerfilepath) $(webpagefilename)
	@date >> upload.log
	$(uploadtool) -b $(uploadscript) $(webuser)@$(server) >> upload.log
	@echo Upload complete.

notify.log: upload.log
	@date >> notify.log
	
checksum: 
	@tail -3 checksum.log
	
checksum.log: upload.log $(installerfilepath)
	@date >> checksum.log
	@echo 'local sha1:  ' `sha1sum $(installerfilepath) | awk '{print $$1}'` >> checksum.log
	@echo 'Server sha1: ' `ssh $(webuser)@$(server) -C 'sha1sum $(installerfilename)' | awk '{print $$1}' ` >> checksum.log
		
		
view logs:
	@echo Upload:
	@tail -4 upload.log
	@echo
	@echo Notify:
	@tail -1 notify.log
	@echo
	@echo Checksum:
	@tail -3 checksum.log
		
clean:
	@rm -rf $(setupoutputfolder)
	@#rm upload.log notify.log
	
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
	
