def experiment1():
    names = ['Aghilan', 'Aadhavan', 'Saveetha', 'Sumanesh']
    print("Going to call...")

    # map accepts a function and few iterables and calls the function using the values from each iterables
    # as parameters to the function
    # map even accepts infinite iterable, it stops when any "one" of the iterables terminates
    print(list(map(test, range(100), names)))


def test(r, name):
    return "Roll {}, Name {}".format(r, name)


if __name__ == '__main__':
    experiment1()