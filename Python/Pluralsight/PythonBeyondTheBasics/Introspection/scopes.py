from pprint import pprint as pp

def experiment1():
    i = 10
    pp(globals())               # Prints the objects in global namespace
    pp(locals())                # Local space
    j = 10
    pp(locals())                # now will have j as well
    globals()['s'] = "Something"    #declare a new variable in global space (locals won't work)
    print(s)                    # we can directly use this as normal variable


def experiment2():
    name = "Sumanesh"
    age = 35

    # note how we are unpacking the locals and pass it directly to the function
    pp("{name} is {age} years old".format(**locals()))


if __name__ == '__main__':
    experiment2()
