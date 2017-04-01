__author__ = 'sumaneshm'

import requests


print("getting request")
r = requests.get(url="http://www.develop.com")


print("got request")
print("Number of bytes : {0}".format(len(r.text)))