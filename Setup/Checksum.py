import hashlib
import sys
import os
#import ftp

html = """<!DOCTYPE html>
<!-- PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta charset="UTF-8" />
<meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
<title>Centipede</title>
<style type="text/css">
tt
{ 
	background: #e9e9e9;
	font-family: consolas, courier new, monospace;
	font-size: 90%%;
}
div.GetSatisfactionBadge
{
	position: absolute; 
	bottom: 30px; 
	right: 30px;
}
</style>
<script type="text/javascript" src="https://loader.engage.gsfn.us/loader.js"></script>
</head>
<body>
<div id="getsat-widget-4615"></div>
<script type="text/javascript">
//<![CDATA[ 
if (typeof GSFN !== "undefined") { GSFN.loadWidget(4615,{"containerId":"getsat-widget-4615"}); } 
//]]>
</script>
<h1>Centipede Installer</h1>

<p style="font-weight:bold"><a href="CentipedeSetup.exe">Click here to download.</a></p>

<p>Please report bugs, ideas, feature requests etc on the <a href="https://getsatisfaction.com/centipede">Centipede Get Satisfaction page</a>, or using the Feedback button below.</p>

<table border=1>
<tr><th>Hash</th><th>Checksum</th></tr>
<tr><td>MD5</td> <td><tt>%s</tt></td></tr>
<tr><td>SHA1</td><td><tt>%s</tt></td></tr>
</table>

<div class="GetSatisfactionBadge"><a href="https://getsatisfaction.com/centipede">
<img alt="Badge_logo_small" width=184 height=43 src="https://getsatisfaction.com/images/badges/badge_logo_small.png" /></a></div>
</body>
</html>
"""

if __name__ == "__main__":
    inputFile = " ".join(sys.argv[1:])
    md5 = hashlib.md5()
    sha1 = hashlib.sha1()
    with open(inputFile, "rb") as f:
        for bytes in f:
            md5.update(bytes)
            sha1.update(bytes)
    with open(os.path.dirname(inputFile) + '/index.html', 'w') as outfile:
        outfile.write(html % (md5.hexdigest(), sha1.hexdigest()))
        outfile.flush()
