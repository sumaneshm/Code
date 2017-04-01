import os

# Method 1 : where we open, read, close explicitly
f = open("Conditionals.py")
line = f.readline()
while line:
    print(line, end='')  # omits the end line character
    line = f.readline()
f.close()

# Method 2 : Read using simple loop

for l in open("Conditionals.py"):
    print(l, end="")


# Writing a new file
print("Trying to create a file")
path = "c:\\temp\\pythonwritetest.txt"

wf = open(path, 'w')
for i in range(0, 10):
    for j in range(0, i):
        print('%3d %3d' % (i, j), file=wf)
wf.close()

if os.path.exists(path):
    print('File found, deleting the file')
#    os.remove(path)
