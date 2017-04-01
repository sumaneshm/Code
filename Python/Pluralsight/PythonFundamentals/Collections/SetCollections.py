# to create a set, use {}
s = {1, 3, 2, 5, 2, }
print(s)

# sets can't have duplicates and hence is a good way to remove duplicates in a list
duplicateList = [1, 1, 1, 2, 2, 2, 2]

print(type(duplicateList), duplicateList)

setToRemoveDuplicates = set(duplicateList)
print(type(setToRemoveDuplicates), setToRemoveDuplicates)

# new element can be added using add method
k = {1, 2, 3}
k.add(4)

# adding an existing element will do nothing and won't even throw an exception
k.add(4)
print(k)

# multiple elements can be added using
k.update([5, 6, 7])
print(k)

# elements can be removed using two ways
# 1. remove = will throw an exception if the element doesn't exist
k.remove(5)
# k.remove(5) => This would throw an exception.
print(k)
# 2. discard = remove only when it exists, otherwise doesn't throw any exception
k.discard(5)  # no effect as the element is not there in the set.
k.discard(6)
print(k)

# 'set's are very powerful due to set algebra usage
odd = {1, 3, 5, 7, 9, 11}
even = {2, 4, 6, 8, 10, 12}
prime = {1, 3, 5, 7, 11}


odd_or_even = odd.union(even)
print(odd_or_even)

odd_but_not_prime = odd.difference(prime)
print(odd_but_not_prime)

odd_and_even = odd.intersection(even)
print(odd_and_even)

# relationships
print(prime.issubset(odd))      # all elements in prime are in odd
print(odd.issuperset(prime))    # all elements in prime are in odd, method 2
print(odd.isdisjoint(even))     # no members are common?

