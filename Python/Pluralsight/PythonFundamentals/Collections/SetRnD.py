s = {}
print(type(s))  # Empty {} gives an empty dictionary

# to create an empty set, use set constructor
s = set()
print("empty set can be created by set constructor.s=", s)

# common use of set is to effectively remove duplicates
s = {1, 20, 2, 3, 3, 4, 5, -1, 2, 12, 14}
print("After a duplicate 3 is removed off the set, s=", s)

# sets are iterable but the order is orbitrary
for i in s:
    print(i)

# presence can be tested using in/not in
print("4 in s gives true as it is present ", 4 in s)
print("13 is not there is s, 13 not in s", 13 not in s)

# two ways to delete elements
s = {1, 2, 3}
s.remove(3)  # 'remove' will throw exception if the element passed in not there in the list
print("After s.delete(3)", s)

s.discard(3)  # 'discard' won't bother to throw exception if the element is not in the list

# shallow copying a set
t = s.copy()
print(s is t)


############################ set alegbra ############################
odd = {1, 3, 5, 7, 9}
even = {2, 4, 6, 8, 10}
prime = {1, 2, 3, 5, 7}

print("odd and prime : ", odd.intersection(prime))
print("odd and not prime : ", odd.difference(prime))
print("odd plus even : ", odd.union(even))



