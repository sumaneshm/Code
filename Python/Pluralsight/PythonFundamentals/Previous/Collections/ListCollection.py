# list can be created by calling split as well

theList = "This is a very cool feature of Python".split()
print(theList)

# or initialized using square brackets
theList = [0, 1, 2, 3, 4, 5]
print(theList)

# two ways to access elements by index
# positive => lef to right
print(theList[0])

# negative => right to left
print(theList[-1])

# we can slice a part of the list as shown below
print(theList[1:3])

# reverse slicing is also possible
print(theList[-3:-1])

# slice from middle till the end
print(theList[2:])

# slice from the beginnning till the middle
print(theList[:2])

# split the list into two lists
print(theList[:2], ' after', theList[2:])

# copying the list
# Method 1
newList1 = theList[:]
print(newList1 == theList)  # newList is equal to list
print(newList1 is theList)  # newList reference is not equal to list

# Method 2
newList2 = theList.copy()
print(newList2 == theList)
print(newList2 is theList)

# Method 3
newList3 = list(theList)
print(newList3 == theList)
print(newList3 is theList)

family = ["Sumanesh", "Saveetha", "Aadhavan", "Aghilan", "Aadhavan"]

# index will find the index of the searched string (if not found, it will throw an exception)
print(family.index('Sumanesh'))

# count function will find the number of occurrence of the word (case sensitive match)
print(family.count('Aadhavan'))

# you can delete any element using the position
del family[-1]
print(family)

# delete using the element name
family += ["Nila", "Nila"]

# this will remove only the first match, not all the matches
family.remove("Nila")
print(family)




