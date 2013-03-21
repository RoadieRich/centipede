from hashlib import md5, sha1
import sys

htmlFile = "index.htm"

def formatLine(line):
    return "<p>%s</p>" % (line.strip() or "&nbsp;")

if __name__ == "__main__":
    inputFile = sys.argv[1]
    outputFile = sys.argv[2]
    md5 = md5()
    sha1 = sha1()
    with open(inputFile, "rb") as inFile:
        for bytes in inFile:
            md5.update(bytes)
            sha1.update(bytes)
    
    if len(sys.argv) > 3:
        commentText = "<p>%s</p>" % " ".join(sys.argv[3:])
    else:
        commentLines = (line for line in sys.stdin.readlines() if not line.lstrip().startswith("#"))
        
        commentText = "\n".join(formatLine(line) for line in commentLines if line.strip("\r\n"))
    
    comment = (commentText + "\n") if commentText else ""
    
    with open(outputFile, "w") as outFile:
        outFile.write(open(htmlFile).read() % (md5.hexdigest(), sha1.hexdigest(), comment))
        
    print "Page created!"
