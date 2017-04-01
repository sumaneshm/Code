#######################################################################################
# List comprehensions
#######################################################################################
words = "Why am I so lazy now when clearly I was not like this few years back?".split()

print(words)

# list comprehensions works on each element of the array and are to be enclosed within []

wordsLengths = [len(word) for word in words]

print(wordsLengths)

# source of the words need not be a list, it just needs to be any iterable
tupl = (1, 2, 3)

print([(i, pow(i, 2)) for i in tupl])

#######################################################################################
# set comprehensions
#######################################################################################
s = {1, 2, 3, 4, 5}

# set comprehensions can be given using {}

print({(i, pow(i, 3)) for i in s})

#######################################################################################
# Dictionary comprehension
#######################################################################################

from pprint import pprint as pp

state_to_capital = {'TamilNadu': 'Chennai',
                    'Karnataka': 'Bengaluru',
                    'Kerala': 'Thiruvanandhapuram',
                    'AndhraPradesh': 'Hyderabad',
                    'Maharashtra': 'Mumbai'
                    }

# dictionary comprehension is a very good choice if we want to convert key to values
# and vice versa

capital_to_state = {capital: state for state, capital in state_to_capital.items()}

pp(capital_to_state)
