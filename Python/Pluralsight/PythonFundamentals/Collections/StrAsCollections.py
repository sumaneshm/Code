import math

# Strings can be concatenated directly
added = "a" "b"
print(added)

# Join can be used to "join" array of strings
joined = ','.join(["a", "b", "c"])
print(joined)

# For concatinating many strings into a single string, use empty separator and join method - it will be faster
print(''.join(["Sumanesh", "Saveetha", "Aadhavan", "Aghilan"]))

# Partition method will split the error into 3 by partitioning and returns tuple
fullStr = "Singapore-Chennai"
(source, dash, destination) = fullStr.partition("-")

print("Source ", source, " & Destination ", destination)

# most of the time, you won't be interested in the separator, and you can suppress it by using a special variable called
# underscore
(source, _, destination) = fullStr.partition("-")

# string format method is useful for formatting in many ways
print("Simple usage is : {0}".format("Exactly like C#"))
print("We can format without numbers like {} and {}".format("1", "2"))
print("We can supply names : {son1}, {son2}".format(son1="Aadhavan", son2="Aghilan"))
print("we can also pass complete module, pi:{m.pi},e={m.e}".format(m=math))
print("we can format usging {m.pi:1.4f}".format(m=math))
