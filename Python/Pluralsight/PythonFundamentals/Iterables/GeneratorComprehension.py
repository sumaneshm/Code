# generator expressions are similar to list comprehension except it is declared using ()
#   () => Generator expression
#   [] => list expression
#   {} => set expression

# the main difference is that this million squares are evaluated 'lazily' as we have declared them using generator
# expression

def give_million_squares():
    return (a * a for a in range(1, 1000001))

million_squares = give_million_squares()

print(million_squares)

# force it to evaluate the entire expression by converting it to list

list(million_squares)

# once when the generator expression has been evaluated it will be empty
print(list(million_squares))

# calculation of sum of million_squares using generator expression takes very less memory when compared to the
# memory it would take had we used list comprehensions
print(sum(give_million_squares()))

# notice two things in the following statements
# 1. when calling a generator inside a functional, we can omit the parameters required for generators to aid readability
# 2. we can use conditions in the same way as we would for list and sets
print(sum(a * a for a in range(1, 1000001) if a % 2 == 0))
