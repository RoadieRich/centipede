import clr
clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import (MessageBox, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton, DialogResult)

import os
from os import path

def resetDefaults():
        searchPath = path.join(os.getenv("USERPROFILE"), "Local Settings", "Application Data", "Chemineer")
        for appDir in (dir for dir in os.listdir(searchPath) if dir.lower().startswith("centipede.exe")):
            for versionDir in os.listdir(path.join(searchPath, appDir)):
                fileToRemove = path.join(searchPath, appDir, versionDir, "user.config")
                if not path.exists(fileToRemove):
                    continue
                
                print "removing %s" % fileToRemove
                os.remove(fileToRemove)
        
        MessageBox.Show("Your defaults have been reset.", "Done")
        
if __name__ == "__main__":
    message = ("This will reset all window and slider positions, your message level filters, "
                   "and your favourite jobs back to default.\n"
               "\n"
               "Do you wish to continue?")
    result = MessageBox.Show(message, "Reset Centipde Detfaults", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
    if result == DialogResult.OK:
        resetDefaults()