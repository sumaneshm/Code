from constraint import Problem

problem = Problem()
problem.addVariables(
    ['A', 'B' ,'C'],    
    [1, 2, 3],
)
problem.addVariables(
    ['Red', 'Green', 'Blue'],
    [1, 2, 3],
)

problem.addVariables(
    ['Bird', 'Fish', 'Cat'],    
    [1, 2, 3],
)



#### Explicit constraints from the problem statement:
# Red house is to the left of the Green house
problem.addConstraint(lambda red, green : red < green, ('Red', 'Green'))

# A person who owns a the Bird lives in the Blue house
problem.addConstraint(lambda a, b, c, bird, blue : (a == blue or b==blue or c == blue) and bird == blue, ('A', 'B', 'C', 'Bird', 'Blue') )

# A person in the middle house owns a Cat
problem.addConstraint(lambda a, b, c, cat : (a == 2 or b == 2 or c == 2) and cat == 2, ('A', 'B', 'C', 'Cat'))

# Fish is not in the Red house
problem.addConstraint(lambda red, fish : red != fish, ('Red', 'Fish'))

#### Explicit constraints not mentioned in the problem statement
# no two people own the same pet
problem.addConstraint(lambda bird, fish, cat : bird != fish and bird != cat and fish != cat, ('Bird', 'Fish', 'Cat'))

# no two people live in the same house
problem.addConstraint(lambda a, b, c : a != b and a != c and b != c, ('A', 'B', 'C'))

# no two houses are the same color
problem.addConstraint(lambda red, green, blue : red != green and red != blue and green != blue, ('Red', 'Green', 'Blue'))   

# no two pets are the same color
problem.addConstraint(lambda bird, fish, cat : bird != fish and bird != cat and fish != cat, ('Bird', 'Fish', 'Cat'))


print('Total number of solutions:', len(problem.getSolutions()))

for s in problem.getSolutions():

    print(s)


# Obsidian reference : [[Backtracking search#Mini Zebra Puzzle]]