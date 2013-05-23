import clr
clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import MessageBox, MessageBoxButtons, MessageBoxIcon
from System.Windows.Forms import MessageBoxDefaultButton, DialogResult

import os

def resetDefaults():
        searchPath = os.path.join(os.environ["USERPROFILE"], "Local Settings", "Application Data", "Chemineer")
        appDirs = [dir for dir in os.listdir(searchPath) if dir.lower().startswith("centipede.exe")]
        for appDir in (dir for dir in os.listdir(searchPath) if dir.lower().startswith("centipede.exe")):
            for versionDir in os.listdir(os.path.join(searchPath, appDir)):
                fileToRemove = os.path.join(searchPath, appDir, versionDir, "user.config")
                if not os.path.exists(fileToRemove):
                    continue
                
                print "removing %s" % fileToRemove
                os.remove(fileToRemove)
        
        MessageBox.Show("Your defaults have been reset.", "Done")
        
if __name__ == "__main__":
    message = "\n".join(["This will reset all window and slider positions, your message level filters, " +
                         "and your favourite jobs back to default.",
                        "",
                        "Do you wish to continue?"])
    result = MessageBox.Show(message, "Reset Centipde Detfaults", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
    if result == DialogResult.OK:
        resetDefaults()