# range is used to give iterator for the given range

for i in range(5):
    print(i)

# range with only one value indicates the stop value
print(list(range(5)))

# range with two value indicates start, stop
print(list(range(2, 7)))

# range with three values indicates start. stop and step
print(list(range(5, 0, -1)))

# we can use enumerate method on tuples to iterate over the list
# enumerate yields (index, value) tuples
t = [1, 2, 3, 4, 5]
for p in enumerate(t):
    print(p)

# more useful way to iterate through a list
for i, p in enumerate(t):
    print("i={}, v={}".format(i, p))
