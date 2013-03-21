from hashlib import md5, sha1
from optparse import OptionParser

import sys

htmlFile = "index.htm"



def formatLine(line):
    return "<p>%s</p>" % (line.strip() or "&nbsp;")

if __name__ == "__main__":
    parser = OptionParser()
    parser.add_option("-f", "--comment-file", dest="commentFile")
    parser.add_option("-c", "--comment", dest="commentText")
    
    (options, args) = parser.parse_args()

    
    inputFile = args[0]
    outputFile = args[1]
    md5 = md5()
    sha1 = sha1()
    with open(inputFile, "rb") as inFile:
        for bytes in inFile:
            md5.update(bytes)
            sha1.update(bytes)
    
    commentText = ""
    
    if options.commentText:
        commentText = "<p>%s</p>" % options.commentText
    elif options.commentFile:
        with open(options.commentFile, "r") as commentFile:
            commentLines = (line for line in commentFile if not line.lstrip().startswith("#"))
        
            commentText = "\n".join(formatLine(line) for line in commentLines if line.strip("\r\n"))
    
    comment = (commentText + "\n") if commentText else ""
    
    with open(outputFile, "w") as outFile:
        outFile.write(open(htmlFile).read() % (md5.hexdigest(), sha1.hexdigest(), comment))
        
    print "Page created!"
