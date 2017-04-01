a = 20
b = 10
c = 5

# simple if else statement
if a > b:
    print('a is greater')
else:
    print('b is greater')

# no case statement in python, so use
if a > b:
    print('a is greater')
elif a < b:
    print('a is lesser')
else:
    print('a is equal')

# boolean operators are called 'and' 'or' 'not'

if a > b and a > c:
    print('a is the greatest')

# you can break to next line with a \
if a > b and \
                b > c:
    print('some relationship')