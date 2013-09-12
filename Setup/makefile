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

iscc = "$$PROGRAMFILES/Inno Setup 5/ISCC"

binfolder = ../bin/Debug

pluginsfolder = $(binfolder)/Plugins
setupoutputfolder = ./Output

installerfilename = CentipedeSetup.exe
installerfilepath = $(setupoutputfolder)/$(installerfilename)

webpagefilename = $(setupoutputfolder)/index.html
uploadlog = $(setupoutputfolder)/upload.log

binfolderfiles = $(binfolder)/Action.dll $(binfolder)/Centipede.exe $(binfolder)/CentipedeInterfaces.dll \
    $(binfolder)/SciLexer.dll $(binfolder)/SciLexer64.dll $(binfolder)/Tutorial/*
pluginfiles = $(pluginsfolder)/*

installersource = centipede-setup.iss
    
setupfiles = $(installersource) favourites.xml dotNetFx40_Full_x86_x64.exe IronPython-2.7.3.msi readme.txt license.txt \
	images/* New\ Job.100p CentipedeAction/* Samples/*

pagescript = create-webpage.py
scriptengine = python

logDir = .

ifndef comment
    commentsarg = -f comments.txt
else
    commentsarg = -c "$(comment)"
    updateCommand = @touch comments.txt
endif

uploadscript = sftp-upload.sftpc
uploadtool = /usr/bin/sftp

#assumes sftp is set up for unattended login to this account
webuser = www-data
server = 172.25.2.16

installer: $(installerfilepath)
$(installerfilepath): $(binfolderfiles) $(pluginfiles) $(setupfiles)
	$(iscc) $(installersource)



updatecomments:
ifdef updateCommand
	$(updateCommand)
endif

#repo:
#    @hg pull default
#    @hg update

webpage: updatecomments $(webpagefilename)
$(webpagefilename): $(pagescript) $(installerfilepath) index.htm comments.txt
	$(scriptengine) $(pagescript) $(installerfilepath) $(webpagefilename) $(commentsarg) 

upload: upload.log
upload.log: $(installerfilepath) $(webpagefilename)
	@date >> upload.log.tmp
	-@echo "Uploading installer" \
	&& $(uploadtool) -b - $(webuser)@$(server) >> upload.log.tmp <<< "put $(installerfilepath) /var/www/" \
	&& echo "Done." | tee upload.log.tmp \
	|| ($(echoUploadErrorHelp))
	@echo "Uploading weboage" \
	&& $(uploadtool) -b - $(webuser)@$(server) >> upload.log.tmp <<< "put $(webpagefilename) /var/www/" \
	&& echo "Done." | tee upload.log.tmp 
	@cat upload.log.tmp >> upload.log
	@echo "Done."

echoUploadErrorHelp = (echo "Uploading failed."; \
echo "You might not have unattended sftp login set up."; \
echo "Instead, you can use the following commands from a cygwin prompt:"; \
echo ""; \
echo " sftp root@$(server)"; \
echo " (enter password when prompted)"; \
echo " put $(installerfilepath) /var/www/"; \
echo " put $(webpagefilename) /var/www/"; \
echo " quit (or ctrl-D)"; \
echo "")

notify: notify.log
notify.log: upload.log
	@date >> notify.log
    
checksum: checksum.log
	@tail -3 checksum.log
    
checksum.log: upload.log $(installerfilepath)
	@date >> checksum.log
	@echo 'local sha1:  ' `sha1sum $(installerfilepath) | awk '{print $$1}'` >> checksum.log
	@echo 'Server sha1: ' `ssh $(webuser)@$(server) -C 'sha1sum $(installerfilename)' | awk '{print $$1}' ` >> checksum.log

view logs:
	@echo Upload:
	@tail -6 upload.log
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
	@echo 'Available targets:'
	@echo
	@echo 'installer (default)' 
	@echo '    Compiles the installer executable'
	@echo 'webpage'
	@echo '    Generates the download webpage with md5 and sha1 checksums'
	@echo 'upload'
	@echo '    Uploads the installer and download page to the webserver'
	@echo 'install'
	@echo '    Runs the installer'
	@echo 'clean'
	@echo '    Deletes all files from the output folder'
	
