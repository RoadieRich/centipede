from hashlib import md5, sha1
from optparse import OptionParser

from datetime import datetime
import sys

import itertools
import string

htmlFile = "index.htm"

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
    
    comment = ""
    
    if options.commentText:
        if options.commentText != "--":
            comment = "<p>%s</p>" % options.commentText
        else:
            comment = "<p>%s</p>" % sys.stdin.read()
    elif options.commentFile:
        with open(options.commentFile, "r") as commentFile:
            commentLines = (line for line in commentFile if not line.lstrip().startswith("#"))  #delete comments
            commentLines = itertools.imap(string.strip, commentLines)                           #remove redundant whitespace
            commentLines = itertools.ifilter(None, commentLines)                                #delete blank lines 
            commentLines = (s[0].upper() + s[1:] for s in commentLines)                         #ensure first letter is capital
            commentLines = (s + ("." if s[-1] not in ".!?" else "") for s in commentLines if s) #ensure final punctuatiuon
            commentLines = ("<li>%s</li>" % line for line in commentLines)                             #add <li> tags
            comment = "<ul>\n%s\n</ul>\n" % "\n".join(commentLines)
    
    updateTime = datetime.now().strftime("%Y-%m-%d %H:%M")
    
    with open(htmlFile) as inFile:
        template = string.Template(inFile.read())
    
    pageText = template.safe_substitute(md5 = md5.hexdigest(), 
                                        sha1 = sha1.hexdigest(), 
                                        comments = comment,
                                        date = updateTime)
    
    with open(outputFile, "w") as outFile:
        outFile.write(pageText)
        
    print "Page created!"
