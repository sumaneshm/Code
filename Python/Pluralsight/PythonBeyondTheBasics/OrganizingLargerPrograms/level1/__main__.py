# when a folder contain a python script called __main__.py, it would make this directory "executable directory" and when
# we pass this directory when calling python, it will execute this script.

import sys
from level2 import reader

try:
    r = reader.Reader(sys.argv[1])
    print(r.read())
finally:
    r.close()

print("LEarning?")