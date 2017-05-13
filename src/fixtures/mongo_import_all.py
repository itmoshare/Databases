import subprocess
import os

for file in os.listdir('./data'):
	subprocess.Popen("/usr/bin/mongoimport --db test --jsonArray --collection units --file ./data/{}".format(file))