from constraint import *


problem = Problem()
problem.addVariables(
    ['A', 'B', 'C'],
    [9, 10, 11]
    )

problem.addConstraint(lambda a,b : a < b, ('A', 'B'))
problem.addConstraint(lambda a, b, c : c > a or c > b, ('A', 'B', 'C'))
problem.addConstraint(lambda a, b, c : a != b and b != c and c != a, ('A', 'B', 'C'))

print(problem.getSolution())

# Obsidian reference : [[Backtracking search#3 Tasks Scheduling]]