#! /bin/bash

echo ''
echo 'Compiling'
/cygdrive/c/Program\ Files/Inno\ Setup\ 5/ISCC centipede-setup.iss

echo ''
echo 'Creating index.htm'
python create-webpage.py ./Output/CentipedeSetup.exe

echo ''
echo 'Uploading'
/usr/bin/sftp -b sftp-upload.sftpc www-data@ps-der-hg1

echo ''
echo ''
echo 'All done.'