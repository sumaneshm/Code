# Modules   - Typically refers to a single py source file
# Packages  - A special type of module
#           - Can contain another package or modules
#           - Used to create module hierarchy
#           - Represented by directory

import sys
import urllib
import urllib.request

print(type(urllib))
print(type(urllib.request))

# urllib is a package, urllib.request is a module
# but when we get the type, both will show only as modules


# Only packages will have __path__ attribute to say where it will look for modules
print(urllib.__path__)

# Python looks for modules from Path environment variable
# we can set a special environment variable called PYTHONPATH such that in all the
# python sessions, this path will automatically be added to PATH variable.


# __init.py__ inside a folder will make it as a package,
# when we import a package, __init.py__ gets imported and executed
# 


